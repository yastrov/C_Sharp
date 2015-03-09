namespace RegistryUtil
{
    using Microsoft.Win32;
    /// <summary>
    /// Utility class for add Aplication to Windows Autostart.
    /// I Found this in http://simpcode.blogspot.ru/2008/07/c-set-and-unset-auto-start-for-windows.html
    /// And make little modification.
    /// </summary>
    public static class RegistryAutostartUtil
    {
        private const string RUN_LOCATION = @"Software\Microsoft\Windows\CurrentVersion\Run";

        /// <summary>
        /// Sets the autostart for the assembly.
        /// </summary>
        /// <param name="appName">Registry Application Name</param>
        /// <param name="assemblyLocation">Assembly location (e.g. Assembly.GetExecutingAssembly().Location)</param>
        public static void SetAutoStart(string appName, string assemblyLocation)
        {
            using (var key = Registry.CurrentUser.CreateSubKey(RUN_LOCATION))
            {
                key.SetValue(appName, assemblyLocation);
            }
        }

        /// <summary>
        /// Returns whether auto start is enabled.
        /// </summary>
        /// <param name="appName">Registry Application Name</param>
        /// <param name="assemblyLocation">Assembly location (e.g. Assembly.GetExecutingAssembly().Location)</param>
        public static bool IsAutoStartEnabled(string appName, string assemblyLocation)
        {
            using (var key = Registry.CurrentUser.OpenSubKey(RUN_LOCATION))
            {
                if (key == null)
                    return false;

                var value = key.GetValue(appName) as string;
                if (value == null)
                    return false;

                return value.Equals(assemblyLocation);
            }
        }

        /// <summary>
        /// Returns whether auto start is enabled. (For any location!)
        /// </summary>
        /// <param name="appName">Registry Application Name</param>
        public static bool IsAutoStartEnabled(string appName)
        {
            using (var key = Registry.CurrentUser.OpenSubKey(RUN_LOCATION))
            {
                if (key == null)
                    return false;

                if (key.GetValue(appName) == null)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Unsets the autostart value for the assembly.
        /// </summary>
        /// <param name="appName">Registry Application Name</param>
        public static void UnSetAutoStart(string appName)
        {
            using (var key = Registry.CurrentUser.CreateSubKey(RUN_LOCATION))
            {
                key.DeleteValue(appName);
            }
        }
    }
}
