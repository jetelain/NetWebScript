using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using NetWebScript.Debug.Engine.Port;

namespace NetWebScript.Debug.Engine
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
            Console.WriteLine(@"Create AD7Metrics\Engine\{{{0}}}", NWSEngine.Id);
            using (RegistryKey key = context.CreateSubKey(String.Format(@"AD7Metrics\Engine\{{{0}}}", NWSEngine.Id)))
            {
                key.SetValue("Name", NWSEngine.Name);
                key.SetValue("CLSID", ClsId(typeof(NWSEngine)));
                key.SetValue("ProgramProvider", ClsId(typeof(NWSProgramProvider)));
                key.SetValue("Attach", 1);
                key.SetValue("AddressBP", 1);
                key.SetValue("CallstackBP", 1);
                key.SetValue("FunctionBP", 1);
                key.SetValue("AlwaysLoadLocal", 0);
                key.SetValue("AutoSelectPriority", 9);
                key.SetValue("PortSupplier", "{"+NWSPortSupplier.Id+"}");
            }

            Console.WriteLine(@"Create AD7Metrics\PortSupplier\{{{0}}}", NWSPortSupplier.Id);
            using (RegistryKey key = context.CreateSubKey(String.Format(@"AD7Metrics\PortSupplier\{{{0}}}", NWSPortSupplier.Id)))
            {
                key.SetValue("Name", NWSPortSupplier.Name);
                key.SetValue("CLSID", ClsId(typeof(NWSPortSupplier)));
            }

            RegisterClass(context, typeof(NWSEngine));
            RegisterClass(context, typeof(NWSProgramProvider));
            RegisterClass(context, typeof(NWSPortSupplier));
        }

        private static void RegisterClass(RegistryKey context, Type type)
        {
            Console.WriteLine(@"Create CLSID\{0}", ClsId(type));

            using (RegistryKey key = context.CreateSubKey(String.Format(@"CLSID\{0}", ClsId(type))))
            {
                key.SetValue("Assembly", type.Assembly.FullName);
                key.SetValue("Class", type.FullName);
                key.SetValue("CodeBase", type.Assembly.CodeBase);
                key.SetValue("InProcServer32", @"C:\Windows\system32\mscoree.dll");
            }
        }

        public static void UnInstall(RegistryKey context)
        {
            Console.WriteLine(@"Remove AD7Metrics\Engine\{{{0}}}", NWSEngine.Id);
            context.DeleteSubKeyTree(String.Format(@"AD7Metrics\Engine\{{{0}}}", NWSEngine.Id));

            Console.WriteLine(@"Remove AD7Metrics\PortSupplier\{{{0}}}", NWSPortSupplier.Id);
            context.DeleteSubKeyTree(String.Format(@"AD7Metrics\PortSupplier\{{{0}}}", NWSPortSupplier.Id));
            
            UnRegisterClass(context, typeof(NWSEngine));
            UnRegisterClass(context, typeof(NWSProgramProvider));
            UnRegisterClass(context, typeof(NWSPortSupplier));
        }

        private static void UnRegisterClass(RegistryKey context, Type type)
        {
            Console.WriteLine(@"Remove CLSID\{0}", ClsId(type));
            context.DeleteSubKeyTree(String.Format(@"CLSID\{0}", ClsId(type)));
        }

    }
}
