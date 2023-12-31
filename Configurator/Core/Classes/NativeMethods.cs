/* Copyright (c) 2023, Oracle and/or its affiliates.

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

using System;
using System.Runtime.InteropServices;

namespace MySql.Configurator.Core.Classes
{
  internal class NativeMethods
  {
    // Keep the compiler from generating a default ctor
    private NativeMethods()
    {
    }

    //Constants for dwDesiredAccess:
    public const uint GENERIC_READ = 0x80000000;
    public const uint GENERIC_WRITE = 0x40000000;

    //Constants for return value:
    public const int INVALID_PIPE_HANDLE_VALUE = -1;

    //Constants for dwFlagsAndAttributes:
    public const uint FILE_FLAG_OVERLAPPED = 0x40000000;
    public const uint FILE_FLAG_NO_BUFFERING = 0x20000000;

    //Constants for dwCreationDisposition:
    public const uint OPEN_EXISTING = 3;

    [StructLayout(LayoutKind.Sequential)]
    public class SecurityAttributes
    {
      public SecurityAttributes()
      {
        Length = Marshal.SizeOf(typeof(SecurityAttributes));
      }
      public int Length;
      public IntPtr securityDescriptor = IntPtr.Zero;
      public bool inheritHandle;
    }

    [DllImport("Kernel32", CharSet = CharSet.Unicode)]
    public static extern IntPtr CreateFile(
            string fileName,
      uint desiredAccess,
      uint shareMode,
      SecurityAttributes securityAttributes,
      uint creationDisposition,
      uint flagsAndAttributes,
      uint templateFile);

    [return: MarshalAs(UnmanagedType.Bool)]
    [DllImport("kernel32.dll", EntryPoint = "PeekNamedPipe", SetLastError = true)]
    public static extern bool PeekNamedPipe(IntPtr handle,
      byte[] buffer,
      uint nBufferSize,
      ref uint bytesRead,
      ref uint bytesAvail,
      ref uint bytesLeftThisMessage);

    [return: MarshalAs(UnmanagedType.Bool)]
    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool ReadFile(IntPtr hFile, [Out] byte[] lpBuffer, uint nNumberOfBytesToRead,
  out uint lpNumberOfBytesRead, IntPtr lpOverlapped);

    [return: MarshalAs(UnmanagedType.Bool)]
    [DllImport("Kernel32")]
    public static extern bool WriteFile(IntPtr hFile, [In]byte[] buffer,
  uint numberOfBytesToWrite, out uint numberOfBytesWritten, IntPtr lpOverlapped);

    [return: MarshalAs(UnmanagedType.Bool)]
    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool CloseHandle(IntPtr handle);

    [return: MarshalAs(UnmanagedType.Bool)]
    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool CancelIo(IntPtr handle);

    [return: MarshalAs(UnmanagedType.Bool)]
    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool FlushFileBuffers(IntPtr handle);

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
    public static extern IntPtr OpenEvent(uint dwDesiredAccess,
        [MarshalAs(UnmanagedType.Bool)]bool bInheritHandle,
        string lpName);

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
    public static extern IntPtr OpenFileMapping(uint dwDesiredAccess,
        [MarshalAs(UnmanagedType.Bool)]bool bInheritHandle,
        string lpName);

    [DllImport("kernel32.dll")]
    public static extern IntPtr MapViewOfFile(IntPtr hFileMappingObject, uint
        dwDesiredAccess, uint dwFileOffsetHigh, uint dwFileOffsetLow,
        IntPtr dwNumberOfBytesToMap);

    [DllImport("kernel32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool UnmapViewOfFile(IntPtr lpBaseAddress);

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern int FlushViewOfFile(IntPtr address, uint numBytes);

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool WaitNamedPipe(string namedPipeName, uint timeOut);
    #region Winsock functions

    // SOcket routines
    [DllImport("ws2_32.dll", SetLastError = true)]
    public static extern IntPtr socket(int af, int type, int protocol);

    [DllImport("ws2_32.dll", SetLastError = true)]
    public static extern int ioctlsocket(IntPtr socket, uint cmd, ref uint arg);

    [DllImport("ws2_32.dll", SetLastError = true)]
    public static extern int WSAIoctl(IntPtr s, uint dwIoControlCode, byte[] inBuffer, uint cbInBuffer,
      byte[] outBuffer, uint cbOutBuffer, IntPtr lpcbBytesReturned, IntPtr lpOverlapped,
      IntPtr lpCompletionRoutine);

    [DllImport("ws2_32.dll", SetLastError = true)]
    public static extern int WSAGetLastError();

    [DllImport("ws2_32.dll", SetLastError = true)]
    public static extern int connect(IntPtr socket, byte[] addr, int addrlen);

    [DllImport("ws2_32.dll", SetLastError = true)]
    public static extern int recv(IntPtr socket, byte[] buff, int len, int flags);

    [DllImport("ws2_32.Dll", SetLastError = true)]
    public static extern int send(IntPtr socket, byte[] buff, int len, int flags);

    #endregion

  }
}
