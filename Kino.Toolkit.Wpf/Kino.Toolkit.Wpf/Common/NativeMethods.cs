using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Kino.Toolkit.Wpf
{
    public static class NativeMethods
    {
        [DllImport("user32.dll")]
        public static extern int GetSystemMetrics(SM nIndex);
    }

    /// <summary>
    /// https://msdn.microsoft.com/en-us/library/windows/desktop/ms724385(v=vs.85).aspx
    /// Retrieves the specified system metric or system configuration setting.
    /// Note that all dimensions retrieved by GetSystemMetrics are in pixels.
    /// </summary>
    public enum SM
    {
        /// <summary>
        /// The amount of border padding for captioned windows, in pixels.
        /// Returns the amount of extra border padding around captioned windows 
        ///Windows XP/2000:  This value is not supported.
        /// </summary>
        CXPADDEDBORDER = 92,
    }
}

