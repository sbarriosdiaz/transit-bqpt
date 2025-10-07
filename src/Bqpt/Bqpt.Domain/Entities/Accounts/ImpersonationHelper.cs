using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Bqpt.Domain
{
    public class ImpersonationHelper : IDisposable
    {
        private IntPtr _userHandle = IntPtr.Zero;
        private readonly WindowsImpersonationContext _impersonationContext;

        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern bool LogonUser(
            string lpszUsername,
            string lpszDomain,
            string lpszPassword,
            int dwLogonType,
            int dwLogonProvider,
            out IntPtr phToken);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern bool CloseHandle(IntPtr handle);

        private const int LOGON32_LOGON_NEW_CREDENTIALS = 9;
        private const int LOGON32_PROVIDER_WINNT50 = 3;

        public ImpersonationHelper(string domain, string username, string password)
        {
            bool success = LogonUser(username, domain, password,
                LOGON32_LOGON_NEW_CREDENTIALS, LOGON32_PROVIDER_WINNT50, out _userHandle);

            if (!success)
                throw new Win32Exception(Marshal.GetLastWin32Error());

            var identity = new WindowsIdentity(_userHandle);
            _impersonationContext = identity.Impersonate();
        }

        public void Dispose()
        {
            _impersonationContext?.Undo();
            if (_userHandle != IntPtr.Zero)
            {
                CloseHandle(_userHandle);
                _userHandle = IntPtr.Zero;
            }
        }
    }
}