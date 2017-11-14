# Get ready

You can retrieve the sources using TFS or subversion. Go to "Source Code" tab for more information.

You require Visual Studio 2010 in order to work with solutions.

# Projects

Projects are organized in 2 main solutions.

* Compiler : NetWebScript.Test.sln
	* NetWebScript : The compiler ("JsClr") (includes remoting code generator)
	* NetWebScript.Metadata : Metadata shared with debugger and remoting
	* NetWebScript.CompilerCLI : Command line to execute the compiler
	* NetWebScript.Task : MsBuild Task to execute the compiler
	* NetWebScript.Test : Compiler unit tests
	* _Jint : Do not make any changes here_
	* _Antlr3.Runtime : Do not make any changes here_
	* _Microsoft.Samples.Debugging : Do not make any changes here_
	* MappingGenerator : JQuery API mapping generator

* Base Class Library : NetWebScript.Test.sln
	* NetWebScript.Core : APIs (runtime, remoting, script equivalents and imports)
	* NetWebScript.Test.Material : APIs (and compiler) unit tests
	* NetWebScript.UnitTestFramework : Visual Studio Unit Test extensions and equivalents
	* NetWebScript.Test.Server : Future unit test server (to remotely execute tests on multiple browsers)

* Debugger : NetWebScript.Debug.sln
	* NetWebScript.Debug.Server : Debugger server and library
	* NetWebScript.Debug.App : Standalone debugger
	* NetWebScript.Debug.Engine : Visual Studio Debugger integration
	* NetWebScript.Debug.Install : Visual Studio Debugger  integration installation (for debug version)
	* NetWebScript.Debug.Launcher : Visual Studio AddIn (to easily launch debugger, not yet functional)

* Packaging : NetWebScript.Install\NetWebScript.Install.sln
	* NetWebScript.Install : WiX packaging (Requires WiX Toolset from [http://wix.codeplex.com/](http://wix.codeplex.com/))

Note: Use "Debug" target, "Release" requires a private key that is not provided.

# How to work on Compiler or on Class Library

Open NetWebScript.Test.sln. Compile solution.

## Standard test projects

Some unit tests can be run directly in Visual Studio.

To run browser side unit tests, compile solution, then open in Internet Explorer (other browsers does not allow to run JavaScript on html pages from file system), {{[NWS Solution Directory](NWS-Solution-Directory)\NetWebScript.Test.Material\bin\Debug\nws.pretty\NetWebScript.Test.Material.htm}}

## Custom test projects

To use compiler on a custom test project, you should use the command line interface. If you use the regular MsBuild Task, Visual Studio will lock all NetWebScript DLLs and will prevent you to compile them again.

### Web Projects

The command line interface is available throw a specific MsBuild targets file, that supports the same options than the release one.

Setup your project following the standard way (see [Project Setup](Project-Setup)). But with the following changes :
* Do not use NetWebScript.Core from the GAC, but browse use the version compiled in the "bin\Debug" from the corresponding project.
* Replace the <Import /> (in the csproj) directive by the following lines :
{{
    <PropertyGroup>
      <NwsSolutionPath>[NWS Solution Directory](NWS-Solution-Directory)</NwsSolutionPath>
    </PropertyGroup>
    <Import Project="$(NwsSolutionPath)\NetWebScript.CompilerCLI\bin\Debug\NetWebScript.Dev.targets" />
}}

### Unit Test Project

Add reference to NetWebScript.UnitTestFramework and to NetWebScript.Core from the NWS solution, each TestClass should be ScriptAvailable, and set the following command as post-build action :
{{[NWS Solution Directory](NWS-Solution-Directory)\NetWebScript.CompilerCLI\bin\Debug\nwsc.exe /path "$(TargetDir)nws" /name "$(ProjectName)" /add "$(TargetDir)$(TargetFileName)" /add "$(TargetDir)NetWebScript.UnitTestFramework.dll" /page NetWebScript.UnitTestFramework.Compiler.UnitTestPageFactory}}
An html page will be produced in "nws" in the output directory (usually bin\Debug)

To allow the use of the debugger, add {{/debug}} to the command line
To have human readable JavaScript, add {{/pretty}}. See next paragraph on how to use/test debugger.

# How to work on Debugger
 
Open NetWebScript.Debug.sln. Compile solution.

## Standalone Debugger

In order to test the debugger, follow this sequence:
# Start project "NetWebScript.Debug.App"
# Start application to debug in a web browser
# In the debugger, double-click on the application (attach to application)
# Browse source files, and set one or more break points
# Refresh the application in the web browser (to debug "onload" code), or continue to use it
# Debug application (and debugger ;-))
# Close browser and debugger windows

## Visual Studio Integrated Debugger

In order to test the debugger engine, follow this sequence :
# Solution NetWebScript.Debug.sln must be opened
# Launch as administrator {{[NWS Solution Dircetory](NWS-Solution-Dircetory)\NetWebScript.Debug.Install\install.cmd}}. Important: From now any opened Visual Studio instance will includes NWS Debugger. You will not be able to compile debugger again until you run uninstall.
# Launch a new instance of Visual Studio 
## First option: Set "devenv.exe" as Start Action for Debug of project "NetWebScript.Debug.Engine", then start debug
## Second option: A regular instance of Visual Studio
# In the new instance of Visual Studio, open application to debug
# Launch application in a web browser
# Go into "Debug" menu, select "Attach to process...", choose transport "NetWebScript" (in first drop down list), then click on "Refresh"
# Select application, and click on "Attach"
# Set one or more break points
# Refresh the application in the web browser (to debug "onload" code), or continue to use it
# Debug application (and debugger ;-))
# Close browser and Visual Studio
# Launch as administrator {{[NWS Solution Dircetory](NWS-Solution-Dircetory)\NetWebScript.Debug.Install\uninstall.cmd}} in order to be able to compile debugger again.