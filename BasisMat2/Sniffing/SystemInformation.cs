using System.Runtime.InteropServices;
using System.Security.Principal;

namespace BasisMat2.Sniffing
{
    public static class SystemInformation
    {        
        public static bool IsAdmin()
        {
            return new WindowsPrincipal(WindowsIdentity.GetCurrent())
                    .IsInRole(WindowsBuiltInRole.Administrator);    
        }
    }
}
