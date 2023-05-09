/* Copyright (c) 2011, 2023, Oracle and/or its affiliates.

 This program is free software; you can redistribute it and/or modify
 it under the terms of the GNU General Public License as published by
 the Free Software Foundation; version 2 of the License.

 This program is distributed in the hope that it will be useful,
 but WITHOUT ANY WARRANTY; without even the implied warranty of
 MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 GNU General Public License for more details.

 You should have received a copy of the GNU General Public License
 along with this program; if not, write to the Free Software
 Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301  USA */

using MySql.Configurator.Core.Classes.Logging;
using MySql.Configurator.Properties;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Principal;

namespace MySql.Configurator.Core.Classes
{

  public static class DirectoryServicesWrapper
  {
    public static string AdministratorUserName { get; set; }
    public static string AdministratorPassword { get; set; }

    /// <summary>
    /// Converts inherited permissions of a directory and its childs to explicit permissions.
    /// </summary>
    /// <param name="path">The path to the directory.</param>
    /// <param name="recursive">Flag indicating whether permissions will be converted for subdirectorie and files.</param>
    /// <returns></returns>
    public static bool ConvertInheritedPermissionsToExplicitPermissions(string path, bool recursive)
    {
      if (string.IsNullOrEmpty(path))
      {
        Logger.LogError(Resources.PathInvalidError);
        return false;
      }

      try
      {
        var directoryInfo = new DirectoryInfo(path);
        if (!directoryInfo.Exists)
        {
          Logger.LogError(string.Format(Resources.PathDoesNotExist, path));
          return false;
        }

        var directorySecurity = directoryInfo.GetAccessControl();
        directorySecurity.SetAccessRuleProtection(true, true);
        directoryInfo.SetAccessControl(directorySecurity);
        foreach (FileInfo fileInfo in directoryInfo.GetFiles())
        {
          var accessControl = fileInfo.GetAccessControl();
          accessControl.SetAccessRuleProtection(true, true);
          fileInfo.SetAccessControl(accessControl);
        }

        if (!recursive)
        {
          return true;
        }

        directoryInfo.GetDirectories().ToList()
          .ForEach(directory => ConvertInheritedPermissionsToExplicitPermissions(directory.FullName, true));

        return true;
      }
      catch (Exception ex)
      {
        Logger.LogException(ex);
        return false;
      }
    }

    /// <summary>
    /// Validates that the specified directory has inherited permissions.
    /// </summary>
    /// <returns><c>true</c> if the directory has inherited permissions; otherwise, <c>false</c>.</returns>
    public static bool? DirectoryPermissionsAreInherited(string path)
    {
      if (string.IsNullOrEmpty(path))
      {
        Logger.LogError(Resources.PathInvalidError);
        return null;
      }

      try
      {
        var directoryInfo = new DirectoryInfo(path);
        if (!directoryInfo.Exists)
        {
          Logger.LogError(string.Format(Resources.PathDoesNotExist, path));
          return null;
        }

        var directorySecurity = directoryInfo.GetAccessControl();
        return !directorySecurity.AreAccessRulesProtected;
      }
      catch (Exception ex)
      {
        Logger.LogException(ex);
        return null;
      }
    }

    /// <summary>
    /// Gets a string representing the name of the account associated to the provided SID.
    /// </summary>
    /// <param name="principalSid">The SID to get the name from.</param>
    /// <returns>The name of the account associated the provided SID.</returns>
    public static string GetAccountName(SecurityIdentifier principalSid)
    {
      if (principalSid == null)
      {
        return null;
      }

      try
      {
        var account = principalSid.Translate(typeof(NTAccount)).ToString();
        if (string.IsNullOrEmpty(account))
        {
          return null;
        }

        if (account.Contains('\\'))
        {
          var items = account.Split('\\');
          if (items.Length > 1)
          {
            return items[1];
          }

          return items[0];
        }
        else
        {
          return account;
        }
      }
      catch (Exception ex)
      {
        Logger.LogException(ex);
      }

      return null;
    }

    public static AuthorizationRuleCollection GetAuthorizationRules(DirectoryInfo directoryInfo)
    {
      try
      {
        if (!directoryInfo.Exists)
        {
          Logger.LogError(string.Format(Resources.PathDoesNotExist, directoryInfo.Name));
          return null;
        }

        var directorySecurity = directoryInfo.GetAccessControl();
        return directorySecurity.GetAccessRules(true, true, typeof(NTAccount));
      }
      catch (Exception ex)
      {
        Logger.LogException(ex);
        return null;
      }
    }

    /// <summary>
    /// Gets the authorization rules associated to the specified file.
    /// </summary>
    /// <param name="fileInfo">The <see cref="FileInfo"/> instance representing the file.</param>
    /// <returns>The list of authorization rules for the specified file.</returns>
    public static AuthorizationRuleCollection GetAuthorizationRules(FileInfo fileInfo)
    {
      try
      {
        if (!fileInfo.Exists)
        {
          Logger.LogError(string.Format(Resources.PathDoesNotExist, fileInfo.Name));
          return null;
        }

        var directorySecurity = fileInfo.GetAccessControl();
        return directorySecurity.GetAccessRules(true, true, typeof(NTAccount));
      }
      catch (Exception ex)
      {
        Logger.LogException(ex);
        return null;
      }
    }

    /// <summary>
    /// Validates that the specified group has access to the specified directory.
    /// </summary>
    /// <param name="sid">The SID instance of the group.</param>
    /// <param name="path">The path to the directory.</param>
    /// <returns><c>true</c> if the group representing the provided SID has access to the directory; otherwise, <c>false</c>.</returns>
    public static bool? GroupHasAcessToDirectory(SecurityIdentifier sid, string path)
    {
      if (string.IsNullOrEmpty(path))
      {
        Logger.LogError(Resources.PathInvalidError);
        return null;
      }

      if (sid == null)
      {
        Logger.LogError(Resources.SidNotProvided);
        return null;
      }

      try
      {
        var directoryInfo = new DirectoryInfo(path);
        var directorySecurity = directoryInfo.GetAccessControl();
        var rules = directorySecurity.GetAccessRules(true, true, typeof(NTAccount));
        foreach (AuthorizationRule rule in rules)
        {
          var ruleValue = rule.IdentityReference.Value;
          var securityIdentifier = GetSecurityIdentifier(ruleValue.Contains('\\') ? ruleValue.Split('\\')[1] : ruleValue);
          if (securityIdentifier != null
              && securityIdentifier.Value == sid.Value)
          {
            return true;
          }
        }
      }
      catch (Exception ex)
      {
        Logger.LogException(ex);
        return null;
      }

      return false;
    }

    public static void Init()
    {
      try
      {
        DirectoryEntry userEntry = GetUserEntry(string.Empty);
      }
      catch
      {
        // ignored
      }
    }

    public static bool UserExists(string securityToken)
    {
      bool returnValue = false;
      try
      {
        if (GetUserEntry(securityToken) != null)
        {
          returnValue = true;
        }
      }
      catch (COMException ex)
      {
        if ((ex.ErrorCode & 0xFFFF) == (int)Win32.NET_API_STATUS.NERR_UserNotFound)
        {
          returnValue = false;
        }
        else
        {
          throw;
        }
      }

      return returnValue;
    }

    public static bool GroupExists(string groupName)
    {
      bool returnValue = false;
      try
      {
        if (GetGroupEntry(groupName) != null)
        {
          returnValue = true;
        }
      }
      catch (COMException ex)
      {
        if ((ex.ErrorCode & 0xFFFF) == (int)Win32.NET_API_STATUS.NERR_GroupNotFound)
        {
          returnValue = false;
        }
        else
        {
          throw;
        }
      }

      return returnValue;
    }

    public static bool TokenExists(string tokenName)
    {
      return UserExists(tokenName) || GroupExists(tokenName);
    }

    public static Guid AddUser(string userName, string password)
    {
      DirectoryEntry localMachine = new DirectoryEntry("WinNT://" + Environment.MachineName);
      DirectoryEntry newUser = localMachine.Children.Add(userName, "user");
      newUser.Invoke("SetPassword", password);
      newUser.CommitChanges();
      var userGuid = newUser.Guid;
      localMachine.Close();
      newUser.Close();
      return userGuid;
    }

    public static void DeleteUser(string userName)
    {
      DirectoryEntry localMachine = new DirectoryEntry("WinNT://" + Environment.MachineName);
      DirectoryEntry userEntry = GetUserEntry(userName);
      localMachine.Children.Remove(userEntry);
    }

    public static void SetPassword(string userName, string password)
    {
      DirectoryEntry userEntry = GetUserEntry(userName);
      userEntry.Invoke("SetPassword", password);
      userEntry.CommitChanges();
    }

    public static void AddUserToGroup(string userName, string groupName)
    {
      DirectoryEntry groupEntry = GetGroupEntry(groupName);
      DirectoryEntry userEntry = GetUserEntry(userName);
      groupEntry.Invoke("Add", userEntry.Path);
    }

    public static void SetUserAccountControlFlags(string userName, int flags)
    {
      DirectoryEntry userEntry = GetUserEntry(userName);
      int currentFlags = (int)(userEntry.Properties["userAccountControl"].Value ?? 0);
      userEntry.Properties["UserFlags"].Value = currentFlags | flags;
      userEntry.CommitChanges();
    }

    public static void SetUserAccountRights(string userName, string policyName)
    {
      IntPtr sid = Win32.GetSIDInformation(GetActiveDirectoryUserAccount(userName).UserAccountName);
      Win32.LSA_UNICODE_STRING systemName = new Win32.LSA_UNICODE_STRING();
      IntPtr policyHandle;
      Win32.LSA_OBJECT_ATTRIBUTES objectAttributes = new Win32.LSA_OBJECT_ATTRIBUTES();
      Win32.LsaOpenPolicy(ref systemName, ref objectAttributes, Win32.POLICY_ALL_ACCESS, out policyHandle);
      int response = Win32.LsaAddAccountRights(policyHandle, sid, new[] { Win32.InitLsaString(policyName) }, 1);
      int windowsErrorCode = Win32.LsaNtStatusToWinError(response);
      try
      {
        if (windowsErrorCode != 0)
        {
          throw new ApplicationException("LsaAddAccountRights failed: " + windowsErrorCode);
        }
      }
      finally
      {
        Win32.LsaClose(policyHandle);
        Win32.FreeSid(sid);
      }
    }

    public static void GrantUserPermissionToDirectory(string directoryName, string userName, FileSystemRights permission, AccessControlType access)
    {
      try
      {
        DirectoryInfo di = new DirectoryInfo(directoryName);
        DirectorySecurity ds = di.GetAccessControl();
        ds.AddAccessRule(new FileSystemAccessRule(userName, permission, InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit, PropagationFlags.None, access));
        di.SetAccessControl(ds);
      }
      catch (Exception ex)
      {
        Logger.LogException(ex);
      }
    }

    /// <summary>
    /// Grants the specified permissions to the selected file.
    /// </summary>
    /// <param name="fileName">The name of the file.</param>
    /// <param name="userName">The user to which permissions will be granted.</param>
    /// <param name="permission">The type of permission to grant.</param>
    /// <param name="access">The type of access to grant.</param>
    public static void GrantUserPermissionToFile(string fileName, string userName, FileSystemRights permission, AccessControlType access)
    {
      try
      {
        var fileInfo = new FileInfo(fileName);
        FileSecurity fileSecurity = fileInfo.GetAccessControl();
        fileSecurity.AddAccessRule(new FileSystemAccessRule(userName, permission, InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit, PropagationFlags.None, access));
        fileInfo.SetAccessControl(fileSecurity);
      }
      catch (Exception ex)
      {
        Logger.LogException(ex);
      }
    }

    public static bool GrantPermissionsToDirectory(string directoryName, string userName, FileSystemRights permission, AccessControlType access, bool recursive)
    {
      try
      {
        var directoryInfo = new DirectoryInfo(directoryName);
        DirectorySecurity directorySecurity = directoryInfo.GetAccessControl();
        directorySecurity.AddAccessRule(new FileSystemAccessRule(userName, permission, InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit, PropagationFlags.None, access));
        directoryInfo.SetAccessControl(directorySecurity);

        foreach (FileInfo fileInfo in directoryInfo.GetFiles())
        {
          FileSecurity fileSecurity = fileInfo.GetAccessControl();
          fileSecurity.AddAccessRule(new FileSystemAccessRule(userName, permission, InheritanceFlags.None, PropagationFlags.None, access));
          fileInfo.SetAccessControl(fileSecurity);
        }

        if (!recursive)
        {
          return true;
        }

        directoryInfo.GetDirectories().ToList()
          .ForEach(directory => GrantPermissionsToDirectory(directory.FullName, userName, FileSystemRights.FullControl, AccessControlType.Allow, true));

        return true;
      }
      catch (Exception ex)
      {
        Logger.LogException(ex);
        return false;
      }
    }

    /// <summary>
    /// Validates the credentials of a Windows account.
    /// </summary>
    /// <param name="userName">The user name.</param>
    /// <param name="password">The password of the associated user account.</param>
    /// <returns><c>true</c> if the credentials are valid; otherwise, <c>false</c>.</returns>
    public static bool IsValidAccount(string userName, string password)
    {
      if (string.IsNullOrEmpty(userName))
      {
        return false;
      }

      var validCredentials = false;
      using (PrincipalContext context = new PrincipalContext(ContextType.Machine))
      {
        validCredentials = context.ValidateCredentials(userName, password);
      }

      return validCredentials;
    }

    /// <summary>
    /// Gets a <see cref="SecurityIdentifier"/> instance for the provided principa name.
    /// </summary>
    /// <param name="principalName">The name of the principal to convert to an SID.</param>
    /// <returns>A <see cref="SecurityIdentifier"/> instance representing the provided principal.</returns>
    public static SecurityIdentifier GetSecurityIdentifier(string principalName)
    {
      try
      {
        var account = new NTAccount(principalName);
        return account.Translate(typeof(SecurityIdentifier)) as SecurityIdentifier;
      }
      catch (Exception)
      {
        return null;
      }
    }

    public static string[] GetUserAccountRights(string userName)
    {
      IntPtr sid = Win32.GetSIDInformation(GetActiveDirectoryUserAccount(userName).UserAccountName);
      Win32.LSA_UNICODE_STRING systemName = new Win32.LSA_UNICODE_STRING();
      IntPtr policyHandle;
      Win32.LSA_OBJECT_ATTRIBUTES objectAttributes = new Win32.LSA_OBJECT_ATTRIBUTES();
      Win32.LsaOpenPolicy(ref systemName, ref objectAttributes, Win32.POLICY_ALL_ACCESS, out policyHandle);

      try
      {
        int countOfRights;
        IntPtr userRightsPointer;
        int response = Win32.LsaEnumerateAccountRights(policyHandle, sid, out userRightsPointer, out countOfRights);
        int windowsErrorCode = Win32.LsaNtStatusToWinError(response);

        if (windowsErrorCode != 0)
        {
          throw new ApplicationException("LsaEnumerateAccountRights failed: " + windowsErrorCode);
        }

        return Win32.GetLsaStringArray(userRightsPointer, countOfRights).Select(i => i.Buffer).ToArray();
      }
      finally
      {
        Win32.LsaClose(policyHandle);
        Win32.FreeSid(sid);
      }
    }

    /// <summary>
    /// Gets the list of local Windows groups.
    /// </summary>
    /// <returns>A string array containing the local groups.</returns>
    public static string[] GetLocalGroups()
    {
      var groups = new List<string>();
      try
      {
        var context = new PrincipalContext(ContextType.Machine, Environment.MachineName);
        var group = new GroupPrincipal(context);
        group.Name = "*";
        var principalSearcher = new PrincipalSearcher();
        principalSearcher.QueryFilter = group;
        PrincipalSearchResult<Principal> result = principalSearcher.FindAll();
        foreach (Principal principal in result)
        {
          using (GroupPrincipal groupPrincipal = (GroupPrincipal)principal)
          {
            groups.Add(groupPrincipal.Name);
          }
        }

        return groups.ToArray();
      }
      catch (Exception ex)
      {
        Logger.LogException(ex);
        return null;
      }
    }

    /// <summary>
    /// Gets the list of local Windows users.
    /// </summary>
    /// <returns>A string array containing the local users.</returns>
    public static string[] GetLocalUsers()
    {
      var users = new List<string>();
      try
      {
        var context = new PrincipalContext(ContextType.Machine, Environment.MachineName);
        var user = new UserPrincipal(context);
        user.Name = "*";
        var principalSearcher = new PrincipalSearcher();
        principalSearcher.QueryFilter = user;
        PrincipalSearchResult<Principal> result = principalSearcher.FindAll();
        foreach (Principal principal in result)
        {
          using (UserPrincipal userPrincipal = (UserPrincipal)principal)
          {
            users.Add(userPrincipal.Name);
          }
        }

        return users.ToArray();
      }
      catch (Exception ex)
      {
        Logger.LogException(ex);
        return null;
      }
    }

    /// <summary>
    /// Checks if a given principal name is a group.
    /// </summary>
    /// <param name="principalName"><c>true</c> if the principal name is a group, <c>false</c> if it is not a group; otherwise, <c>null</c>.</param>
    /// <returns></returns>
    public static bool? IsGroup(string principalName)
    {
      try
      {
        var domain = new PrincipalContext(ContextType.Machine);
        var user = GroupPrincipal.FindByIdentity(domain, principalName);
        return user != null;
      }
      catch (Exception ex)
      {
        Logger.LogException(ex);
      }

      return null;
    }

    public static bool IsUserInGroup(string userName, string groupName)
    {
      try
      {
        DirectoryEntry user = GetUserEntry(userName);
        DirectoryEntry group = GetGroupEntry(groupName);
        return (bool)group.Invoke("IsMember", user.Path);
      }
      catch (Exception ex)
      {
        Logger.LogException(ex);
        return false;
      }
    }

    /// <summary>
    /// Checks if the principal has the specified access to the directory.
    /// </summary>
    /// <param name="principalName">The principal to validate.</param>
    /// <param name="path">The path to the directory.</param>
    /// <param name="fileSystemRights">The expected access level.</param>
    /// <returns><c>true</c> if access matches the specified access; otherwise, <c>false</c>.</returns>
    public static bool HasAccessToDirectory(SecurityIdentifier principalSid, string path, FileSystemRights? fileSystemRights)
    {
      if (principalSid == null)
      {
        throw new ArgumentNullException(string.Format(Resources.MissingParameterGenericError, "principal name"));
      }

      if (string.IsNullOrEmpty(path))
      {
        throw new ArgumentNullException(string.Format(Resources.MissingParameterGenericError, "file path"));
      }

      var directoryInfo = new DirectoryInfo(path);
      if (!directoryInfo.Exists)
      {
        throw new Exception(string.Format(Resources.PathDoesNotExist, path));
      }

      var rules = GetAuthorizationRules(directoryInfo);
      if (rules == null)
      {
        throw new Exception(string.Format(Resources.AuthorizationRulesRetrievalFailed, path));
      }

      foreach (FileSystemAccessRule rule in rules)
      {
        var ruleValue = rule.IdentityReference.Value;
        if (string.IsNullOrEmpty(ruleValue))
        {
          throw new ArgumentNullException(string.Format(Resources.MissingParameterGenericError, "rule"));
        }

        var securityIdentifier = GetSecurityIdentifier(ruleValue.Contains("\\") ? ruleValue.Split('\\')[1] : ruleValue);
        if (securityIdentifier == null
            || securityIdentifier.Value != principalSid.Value)
        {
          continue;
        }

        if (rule.AccessControlType != AccessControlType.Allow)
        {
          return false;
        }

        if (fileSystemRights != null
            && rule.FileSystemRights == fileSystemRights)
        {
          return true;
        }
      }

      return false;
    }

    /// <summary>
    /// Checks if the principal has the specified access to the file.
    /// </summary>
    /// <param name="principalName">The principal to validate.</param>
    /// <param name="path">The path to the file.</param>
    /// <param name="fileSystemRights">The expected access level.</param>
    /// <returns><c>true</c> if access matches the specified access; otherwise, <c>false</c>.</returns>
    public static bool HasAccessToFile(SecurityIdentifier principalSid, string path, FileSystemRights? fileSystemRights)
    {
      if (principalSid == null)
      {
        throw new ArgumentNullException(string.Format(Resources.MissingParameterGenericError, "principal name"));
      }

      if (string.IsNullOrEmpty(path))
      {
        throw new ArgumentNullException(string.Format(Resources.MissingParameterGenericError, "file path"));
      }

      var fileInfo = new FileInfo(path);
      if (!fileInfo.Exists)
      {
        throw new Exception(string.Format(Resources.FileDoesNotExist, path));
      }

      var rules = GetAuthorizationRules(fileInfo);
      if (rules == null)
      {
        throw new Exception(string.Format(Resources.AuthorizationRulesRetrievalFailed, path));
      }

      foreach (FileSystemAccessRule rule in rules)
      {
        var ruleValue = rule.IdentityReference.Value;
        if (string.IsNullOrEmpty(ruleValue))
        {
          throw new ArgumentNullException(string.Format(Resources.MissingParameterGenericError, "rule"));
        }

        var securityIdentifier = GetSecurityIdentifier(ruleValue.Contains("\\") ? ruleValue.Split('\\')[1] : ruleValue);
        if (securityIdentifier == null
            || securityIdentifier.Value != principalSid.Value)
        {
          continue;
        }

        if (rule.AccessControlType != AccessControlType.Allow)
        {
          return false;
        }

        if (fileSystemRights != null
            && rule.FileSystemRights == fileSystemRights)
        {
          return true;
        }
      }

      return false;
    }

    /// <summary>
    /// Removes all permissions to a directory and its childs for a specific group.
    /// </summary>
    /// <param name="path">The path to the directory.</param>
    /// <param name="principalSid">The SID instance of the group.</param>
    /// <param name="recursive">Flag indicating whether permissions will be removed on subdirectories and files.</param>
    /// <returns><c>true</c> if the permissions of the group were successfully removed from the directory; otherwise, <c>false</c>.</returns>
    public static bool RemoveGroupPermissions(string path, SecurityIdentifier principalSid, bool recursive)
    {
      if (string.IsNullOrEmpty(path))
      {
        Logger.LogError(Resources.PathInvalidError);
        return false;
      }

      if (principalSid == null)
      {
        Logger.LogError(Resources.SidNotProvided);
        return false;
      }

      try
      {
        var directoryInfo = new DirectoryInfo(path);
        if (!directoryInfo.Exists)
        {
          Logger.LogError(string.Format(Resources.PathDoesNotExist, path));
          return false;
        }

        var directorySecurity = directoryInfo.GetAccessControl();
        directorySecurity.PurgeAccessRules(principalSid);
        directoryInfo.SetAccessControl(directorySecurity);
        foreach (FileInfo fileInfo in directoryInfo.GetFiles())
        {
          var accessControl = fileInfo.GetAccessControl();
          accessControl.PurgeAccessRules(principalSid);
          fileInfo.SetAccessControl(accessControl);
        }

        if (!recursive)
        {
          return true;
        }

        directoryInfo.GetDirectories().ToList()
          .ForEach(directory => RemoveGroupPermissions(directory.FullName, principalSid, true));

        return true;
      }
      catch (Exception ex)
      {
        Logger.LogException(ex);
        return false;
      }
    }

    private static DirectoryEntry GetUserEntry(string securityToken)
    {
      var userAccount = GetActiveDirectoryUserAccount(securityToken.Trim());
      return GetEntry(userAccount.UserAccountName, userAccount.Domain, "user");
    }

    private static DirectoryEntry GetGroupEntry(string securityToken)
    {
      var userAccount = GetActiveDirectoryUserAccount(securityToken.Trim());
      return GetEntry(userAccount.UserAccountName, userAccount.Domain, "group");
    }

    private static DirectoryEntry GetEntry(string userName, string domain, string type)
    {
      var machineEntry = string.IsNullOrEmpty(domain) ?
        new DirectoryEntry($"WinNT://{Environment.MachineName}") :
        string.IsNullOrEmpty(AdministratorUserName)
          ? new DirectoryEntry($"WinNT://{domain}")
          : new DirectoryEntry($"WinNT://{domain}", AdministratorUserName, AdministratorPassword ?? string.Empty, AuthenticationTypes.ReadonlyServer);
      DirectoryEntry entry = machineEntry.Children.Find(userName, type);
      entry.Close();
      machineEntry.Close();

      return entry;
    }

    private static ActiveDirectoryUserAccount GetActiveDirectoryUserAccount(string securityToken)
    {
      var userAccount = new ActiveDirectoryUserAccount();

      // Standarize backslashes since user can input single or double backslashes.
      if (securityToken.Contains("\\\\"))
      {
        securityToken = securityToken.Replace("\\\\", "\\");
      }

      if (securityToken.Contains("\\"))
      {
        string[] userNameParts = securityToken.Split('\\');
        userAccount.Domain = userNameParts[0];
        if (userNameParts.Length > 1)
        {
          userAccount.UserAccountName = userNameParts[1];
        }
      }
      else if (securityToken.Contains("@"))
      {
        string[] userNameParts = securityToken.Split('@');
        userAccount.UserAccountName = userNameParts[0];
        if (userNameParts.Length > 1)
        {
          userAccount.Domain = userNameParts[1];
        }
      }
      else
      {
        userAccount.UserAccountName = securityToken;
      }

      return userAccount;
    }

    /// <summary>
    /// Defines a user account within the Active Directory.
    /// </summary>
    class ActiveDirectoryUserAccount
    {
      /// <summary>
      /// Gets or sets the domain of the user account.
      /// </summary>
      public string Domain { get; set; }

      /// <summary>
      /// Gets or sets the name of the user account.
      /// </summary>
      public string UserAccountName { get; set; }
    }
  }
}
