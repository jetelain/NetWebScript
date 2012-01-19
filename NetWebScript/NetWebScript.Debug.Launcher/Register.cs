using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.Runtime.InteropServices;

namespace NetWebScript.Debug.Launcher
{
    public static class Register
    {
        private static string ClsId(Type type)
        {
            object[] attributes = type.GetCustomAttributes(typeof(GuidAttribute),false);
            return "{" + ((GuidAttribute)attributes[0]).Value + "}";
        }

        public static void Install()
        {
            using (RegistryKey context = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\VisualStudio\10.0", true))
            {
                Install(context);
            }
        }

        public static void UnInstall()
        {
            using (RegistryKey context = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\VisualStudio\10.0", true))
            {
                UnInstall(context);
            }
        }

        public static void InstallHive()
        {
            using (RegistryKey context = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\VisualStudio\10.0Exp", true))
            {
                Install(context);
            }
        }

        public static void UnInstallHive()
        {

            using (RegistryKey context = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\VisualStudio\10.0Exp", true))
            {
                UnInstall(context);
            }
        }

        public static void Install(RegistryKey context)
        {
            var packageGuid = ClsId(typeof(NwsFlavorPackage));
            var projectFactoryGuid = ClsId(typeof(NwsFlavorProjectFactory));

            Console.WriteLine(@"Create Packages\{0}", packageGuid);
            using (RegistryKey key = context.CreateSubKey(String.Format(@"Packages\{0}", packageGuid)))
            {
                key.SetValue("Assembly", typeof(NwsFlavorPackage).Assembly.FullName);
                key.SetValue("Class", typeof(NwsFlavorPackage).FullName);
                key.SetValue("CodeBase", typeof(NwsFlavorPackage).Assembly.CodeBase);
                key.SetValue("InProcServer32", @"C:\Windows\system32\mscoree.dll");
            }

            Console.WriteLine(@"Create Projects\{0}", projectFactoryGuid);
            using (RegistryKey key = context.CreateSubKey(String.Format(@"Projects\{0}", projectFactoryGuid)))
            {
                key.SetValue("Name", "NetWebScript Flavor");
                key.SetValue("Package", packageGuid);
            }

        }


        public static void UnInstall(RegistryKey context)
        {
            var packageGuid = ClsId(typeof(NwsFlavorPackage));
            var projectFactoryGuid = ClsId(typeof(NwsFlavorProjectFactory));

            Console.WriteLine(@"Remove Packages\{0}", packageGuid);
            context.DeleteSubKeyTree(String.Format(@"Packages\{0}", packageGuid));

            Console.WriteLine(@"Remove Projects\{0}", projectFactoryGuid);
            context.DeleteSubKeyTree(String.Format(@"Projects\{0}", projectFactoryGuid));
        }


    }
}
