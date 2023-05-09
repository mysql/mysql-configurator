using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace WexInstaller.Core.Classes
{
  public class ScheduledTaskManager
  {
    public static string TaskName = @"\MySQL\Installer\CatalogUpdate";

    public static void Update()
    {
      // if user has set that task should run and we can't find it 
      // then we need to recreate it. Someone has been mucking around with it

      // if the user has set that the task should not run and we do find it
      // then we need to delete it
    }

    public static bool TaskExists()
    {
      string path = Environment.GetFolderPath(Environment.SpecialFolder.System);
      path = Path.Combine(path, "Tasks", "MySQL", "Installer", "CatalogUpdate");
//      if (File.Exists(path))
  //      return XmlCheck(path);


      string strJobId = "";
      try
      {
//        ManagementObject mo = new ManagementObject(new ManagementPath("Win32_ScheduleObject"));


//        ConnectionOptions conn = new ConnectionOptions();
  //      conn.EnablePrivileges = true;
    //    conn.Impersonation = System.Management.ImpersonationLevel.Impersonate;
      //  ManagementScope scope = new ManagementScope("root\\cimv2", conn);
        //scope.Options.Impersonation = System.Management.ImpersonationLevel.Impersonate;
        //scope.Options.EnablePrivileges = true;
        //scope.Connect();

        ManagementPath managementPath = new ManagementPath("Win32_ScheduledJob");

        ObjectGetOptions objectGetOptions = new ObjectGetOptions();
        ManagementClass classInstance = new ManagementClass(@"root\cimv2", "Win32_ScheduledJob", objectGetOptions);

        object[] objectsIn = new object[7];
        objectsIn[0] = "notepad.exe";
        objectsIn[1] = "********140000.000000+480";
        objectsIn[5] = true;
        object outParams = classInstance.InvokeMethod("Create", objectsIn);
        String response = "Creation of the process returned: " + outParams;

//        DateTime dt = DateTime.Now.AddMinutes(1);
//        ManagementClass classInstance = new ManagementClass("root\\CIMV2", "Win32_ScheduledJob", null);
//        ManagementBaseObject inParams = classInstance.GetMethodParameters("Create");
//        inParams["Command"] = "Notepad.exe";
//        inParams["InteractWithDesktop"] = true;
//        inParams["RunRepeatedly"] = false;
//        inParams["DaysOfMonth"] = 0;
//        inParams["DaysOfWeek"] = 0;
////        string s = dt.ToUniversalTime().ToString("yyyyMMddTHHmmss.fffK"); //."20101129135500.000000+330";;
//        inParams["StartTime"] = "20140416141100.000000-300";
//        ManagementBaseObject outParams =
//                classInstance.InvokeMethod("Create", inParams, null);

//        strJobId = outParams["JobId"].ToString();
//        Console.WriteLine("Out parameters:");
  //      Console.WriteLine("JobId: " + outParams["JobId"]);

    //    Console.WriteLine("ReturnValue: " + outParams["ReturnValue"]);
      }
      catch (ManagementException err)
      {

      }
      //return strJobId;


      //var query = new ManagementObjectSearcher();
      //query."SELECT * FROM Win32_ScheduledJob");
      //var tasks = query.Get();
      //foreach (var t in tasks)
      //{
      // foreach (var p in t.Properties)
      // {
      //   string s = p.Name;
      // }
      //}
      try
      {
        //ITaskService taskService = new TaskScheduler();
        //taskService.Connect();
        //ITaskFolder folder = taskService.GetFolder(@"\MySQL\Installer");
        //IRegisteredTask task = folder.GetTask(@"CatalogUpdate");
        //Marshal.ReleaseComObject(folder);
        //Marshal.ReleaseComObject(task);
        return true;
      }
      catch (FileNotFoundException)
      {
        return false;
      }

//      string output = Utilities.RunCommand("schtasks.exe", "/query");
  //    return output.Contains(TaskName);
    }

    public static bool ScheduleTask(int hour)
    {
      string file = "";
      string cmd = String.Format("/create /sc DAILY /tr {0} /tn \"{1}\" /st {2:0}:00", file, TaskName, hour);
      string output = Utilities.RunCommand("schtasks.exe", cmd);
      return output.Contains("has successfully been created");
    }
  }

//  [ComImport, TypeLibType((short)0x10c0), DefaultMember("TargetServer"), Guid("2FABA4C7-4DA9-4013-9697-20CC3FD40F85"), System.Security.SuppressUnmanagedCodeSecurity]
//  internal interface ITaskService
//  {
//    [return: MarshalAs(UnmanagedType.Interface)]
//    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(1)]
//    ITaskFolder GetFolder([In, MarshalAs(UnmanagedType.BStr)] string Path);
//    [return: MarshalAs(UnmanagedType.Interface)]
//    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(2)]
//    IRunningTaskCollection GetRunningTasks(int flags);
//    [return: MarshalAs(UnmanagedType.Interface)]
//    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(3)]
//    ITaskDefinition NewTask([In] uint flags);
//    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(4)]
//    void Connect([In, Optional, MarshalAs(UnmanagedType.Struct)] object serverName, [In, Optional, MarshalAs(UnmanagedType.Struct)] object user, [In, Optional, MarshalAs(UnmanagedType.Struct)] object domain, [In, Optional, MarshalAs(UnmanagedType.Struct)] object password);
//    [DispId(5)]
//    bool Connected { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(5)] get; }
//    [DispId(0)]
//    string TargetServer { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0)] get; }
//    [DispId(6)]
//    string ConnectedUser { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(6)] get; }
//    [DispId(7)]
//    string ConnectedDomain { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(7)] get; }
//    [DispId(8)]
//    uint HighestVersion { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(8)] get; }
//  }

////  [ComImport, CoClass(typeof(TaskSchedulerClass)), Guid("2FABA4C7-4DA9-4013-9697-20CC3FD40F85"), System.Security.SuppressUnmanagedCodeSecurity]
//  [ComImport, Guid("2FABA4C7-4DA9-4013-9697-20CC3FD40F85"), System.Security.SuppressUnmanagedCodeSecurity]
//  internal interface TaskScheduler : ITaskService
//  {
//  }
}
