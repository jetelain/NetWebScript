using System;
using Extensibility;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.CommandBars;
using System.Resources;
using System.Globalization;
using System.Reflection;
using Microsoft.VisualStudio.Shell;
using System.Diagnostics;
namespace NetWebScript.Debug.Launcher
{
	/// <summary>The object for implementing an Add-in.</summary>
	/// <seealso class='IDTExtensibility2' />
	/// <summary>The object for implementing an Add-in.</summary>
	/// <seealso class='IDTExtensibility2' />
    public class Connect : IDTExtensibility2, IDTCommandTarget
    {
        public Connect()
        {
        }

        private Project GetProjectByUniqueName(String projectUniqueName)
        {
            foreach (Project project in _dte.Solution.Projects)
            {
                if (projectUniqueName == project.UniqueName)
                {
                    return project;
                }
            }
            return null;
        }

        private Project GetStartupProject()
        {
            if (_dte.Solution != null && _dte.Solution.SolutionBuild != null)
            {
                object[] array = (object[])_dte.Solution.SolutionBuild.StartupProjects;
                if (array.Length != 0)
                {
                    return GetProjectByUniqueName((String)array[0]);
                }
            }
            return null;
        }

        DebuggerEvents _debugEvents;

        /// <summary>Implements the OnConnection method of the IDTExtensibility2 interface. Receives notification that the Add-in is being loaded.</summary>
        /// <param term='application'>Root object of the host application.</param>
        /// <param term='connectMode'>Describes how the Add-in is being loaded.</param>
        /// <param term='addInInst'>Object representing this Add-in.</param>
        /// <seealso class='IDTExtensibility2' />
        public void OnConnection(object application, ext_ConnectMode connectMode, object addInInst, ref Array custom)
        {
            _dte = (DTE2)application;
            _addInInstance = (AddIn)addInInst;

            //if (connectMode == ext_ConnectMode.ext_cm_Startup || connectMode == ext_ConnectMode.ext_cm_AfterStartup)
            //{
            //    _debugEvents = _dte.Events.DebuggerEvents;
            //    _debugEvents.OnEnterRunMode += new _dispDebuggerEvents_OnEnterRunModeEventHandler(_debugEvents_OnEnterRunMode);
            //}


            if (connectMode == ext_ConnectMode.ext_cm_UISetup)
            {
                object[] contextGUIDS = new object[] { };
                Commands2 commands = (Commands2)_dte.Commands;
                string toolsMenuName;

                try
                {
                    //If you would like to move the command to a different menu, change the word "Tools" to the 
                    //  English version of the menu. This code will take the culture, append on the name of the menu
                    //  then add the command to that menu. You can find a list of all the top-level menus in the file
                    //  CommandBar.resx.
                    string resourceName;
                    ResourceManager resourceManager = new ResourceManager("NetWebScript.Debug.Launcher.CommandBar", Assembly.GetExecutingAssembly());
                    CultureInfo cultureInfo = new CultureInfo(_dte.LocaleID);

                    if (cultureInfo.TwoLetterISOLanguageName == "zh")
                    {
                        System.Globalization.CultureInfo parentCultureInfo = cultureInfo.Parent;
                        resourceName = String.Concat(parentCultureInfo.Name, "Tools");
                    }
                    else
                    {
                        resourceName = String.Concat(cultureInfo.TwoLetterISOLanguageName, "Tools");
                    }
                    toolsMenuName = resourceManager.GetString(resourceName);
                }
                catch
                {
                    //We tried to find a localized version of the word Tools, but one was not found.
                    //  Default to the en-US word, which may work for the current culture.
                    toolsMenuName = "Tools";
                }

                //Place the command on the tools menu.
                //Find the MenuBar command bar, which is the top-level command bar holding all the main menu items:
                Microsoft.VisualStudio.CommandBars.CommandBar menuBarCommandBar = ((Microsoft.VisualStudio.CommandBars.CommandBars)_dte.CommandBars)["MenuBar"];

                //Find the Tools command bar on the MenuBar command bar:
                CommandBarControl toolsControl = menuBarCommandBar.Controls[toolsMenuName];
                CommandBarPopup toolsPopup = (CommandBarPopup)toolsControl;

                //This try/catch block can be duplicated if you wish to add multiple commands to be handled by your Add-in,
                //  just make sure you also update the QueryStatus/Exec method to include the new command names.
                try
                {
                    //Add a command to the Commands collection:
                    Command command = commands.AddNamedCommand2(_addInInstance, "NetWebScript", "NetWebScript", "Executes the command for ProjectLauncher", true, 59, ref contextGUIDS, (int)vsCommandStatus.vsCommandStatusSupported + (int)vsCommandStatus.vsCommandStatusEnabled, (int)vsCommandStyle.vsCommandStylePictAndText, vsCommandControlType.vsCommandControlTypeButton);

                    //Add a control for the command to the tools menu:
                    if ((command != null) && (toolsPopup != null))
                    {
                        command.AddControl(toolsPopup.CommandBar, 1);
                    }
                }
                catch (System.ArgumentException)
                {
                    //If we are here, then the exception is probably because a command with that name
                    //  already exists. If so there is no need to recreate the command and we can 
                    //  safely ignore the exception.
                }
            }
        }

        //void _debugEvents_OnEnterRunMode(dbgEventReason Reason)
        //{
        //    //LaunchDebugStartupProject();
        //}

        /// <summary>Implements the OnDisconnection method of the IDTExtensibility2 interface. Receives notification that the Add-in is being unloaded.</summary>
        /// <param term='disconnectMode'>Describes how the Add-in is being unloaded.</param>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />
        public void OnDisconnection(ext_DisconnectMode disconnectMode, ref Array custom)
        {
        }

        /// <summary>Implements the OnAddInsUpdate method of the IDTExtensibility2 interface. Receives notification when the collection of Add-ins has changed.</summary>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />		
        public void OnAddInsUpdate(ref Array custom)
        {
        }

        /// <summary>Implements the OnStartupComplete method of the IDTExtensibility2 interface. Receives notification that the host application has completed loading.</summary>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />
        public void OnStartupComplete(ref Array custom)
        {
        }

        /// <summary>Implements the OnBeginShutdown method of the IDTExtensibility2 interface. Receives notification that the host application is being unloaded.</summary>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />
        public void OnBeginShutdown(ref Array custom)
        {
        }

        /// <summary>Implements the QueryStatus method of the IDTCommandTarget interface. This is called when the command's availability is updated</summary>
        /// <param term='commandName'>The name of the command to determine state for.</param>
        /// <param term='neededText'>Text that is needed for the command.</param>
        /// <param term='status'>The state of the command in the user interface.</param>
        /// <param term='commandText'>Text requested by the neededText parameter.</param>
        /// <seealso class='Exec' />
        public void QueryStatus(string commandName, vsCommandStatusTextWanted neededText, ref vsCommandStatus status, ref object commandText)
        {
            if (neededText == vsCommandStatusTextWanted.vsCommandStatusTextWantedNone)
            {
                if (commandName == "NetWebScript.Debug.Launcher.Connect.NetWebScript")
                {
                    status = (vsCommandStatus)vsCommandStatus.vsCommandStatusSupported | vsCommandStatus.vsCommandStatusEnabled;
                    return;
                }
            }
        }

        private static string GetProperty(Project project, string key)
        {
            try
            {
                var property = project.Properties.Item(key);
                return property.Value as string;
            }
            catch 
            {
                return null;
            }
        }

        /// <summary>Implements the Exec method of the IDTCommandTarget interface. This is called when the command is invoked.</summary>
        /// <param term='commandName'>The name of the command to execute.</param>
        /// <param term='executeOption'>Describes how the command should be run.</param>
        /// <param term='varIn'>Parameters passed from the caller to the command handler.</param>
        /// <param term='varOut'>Parameters passed from the command handler to the caller.</param>
        /// <param term='handled'>Informs the caller if the command was handled or not.</param>
        /// <seealso class='Exec' />
        public void Exec(string commandName, vsCommandExecOption executeOption, ref object varIn, ref object varOut, ref bool handled)
        {
            handled = false;
            if (executeOption == vsCommandExecOption.vsCommandExecOptionDoDefault)
            {
                if (commandName == "NetWebScript.Debug.Launcher.Connect.NetWebScript")
                {
                    LaunchDebugStartupProject();
                    handled = true;
                    return;
                }
            }
        }

        private void LaunchDebugStartupProject()
        {
            string uri = null;
            var project = GetStartupProject();
            if (project != null)
            {
                uri = GetProperty(project, "WebApplication.StartExternalUrl");
                if (string.IsNullOrEmpty(uri))
                {
                    uri = GetProperty(project, "StartURL");
                }
            }
            if (!string.IsNullOrEmpty(uri))
            {
                Trace.TraceInformation("Attach NetWebScript, Url={0}", uri);
                LaunchDebugTarget(uri);
            }
            else
            {
                Trace.TraceWarning("Startup Project has no StartExternalUrl or StartURL");
            }
        }

        /// <summary>
        /// Launch an executible using the sample debug engine.
        /// </summary>
        private void LaunchDebugTarget(string uri)
        {
            var sp =
                 new Microsoft.VisualStudio.Shell.ServiceProvider((Microsoft.VisualStudio.OLE.Interop.IServiceProvider)_dte);

            var dbg = (IVsDebugger)sp.GetService(typeof(SVsShellDebugger));

            VsDebugTargetInfo info = new VsDebugTargetInfo();
            info.dlo = DEBUG_LAUNCH_OPERATION.DLO_CreateProcess;
            info.bstrExe = uri;
            info.bstrCurDir = null;
            info.bstrArg = null; // no command line parameters
            info.bstrRemoteMachine = null; // debug locally
            info.fSendStdoutToOutputWindow = 0; // Let stdout stay with the application.
            info.clsidCustom = new Guid("{88EBAE0F-1B09-4c57-9463-799D483E4097}"); // Set the launching engine the sample engine guid
            
            info.clsidPortSupplier = new Guid("{9D17E7CB-E7DB-4eb8-BF2C-DA2A454703AC}");
            info.bstrPortName = "localhost";

            info.grfLaunch = 0;


            try
            {
                VsShellUtilities.LaunchDebugger(sp, info);
            }
            catch (Exception e)
            {
            }
            
            

        }

        private DTE2 _dte;
        private AddIn _addInInstance;
    }
}