using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using NetWebScript;
using System.Collections;
using System;
using System.Collections.Generic;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("NetWebScript.Core")]
[assembly: AssemblyDescription("Core classes and attributes to write script runnable .Net code")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("NetWebScript")]
[assembly: AssemblyCopyright("Copyright © 2010-2011")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("ded593fc-c716-4403-84ba-75791a4c4e96")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]

[assembly: CLSCompliant(true)]
        
[assembly: ForceScriptAvailable(typeof(IEnumerable))]
[assembly: ForceScriptAvailable(typeof(IEnumerator))]
[assembly: ForceScriptAvailable(typeof(IEnumerable<>))]
[assembly: ForceScriptAvailable(typeof(IEnumerator<>))]
[assembly: ForceScriptAvailable(typeof(IDisposable))]

[assembly: ForceScriptAvailable(typeof(IEqualityComparer))]
[assembly: ForceScriptAvailable(typeof(IEqualityComparer<>))]
[assembly: ForceScriptAvailable(typeof(ICollection))]
[assembly: ForceScriptAvailable(typeof(ICollection<>))]

[assembly: ForceScriptAvailable(typeof(ICustomFormatter))]
[assembly: ForceScriptAvailable(typeof(IFormatProvider))]
[assembly: ForceScriptAvailable(typeof(IFormattable))]

#if SIGNED
[assembly: InternalsVisibleTo("NetWebScript.Test.Material, PublicKey=002400000480000094000000060200000024000052534131000400000100010093f5b897959d0750070b0857c5f2d123987ee46191d4688797c5df2a8ae8c5ce2593a971842b334ab3eba209566ce6cbb92490f5f1bbba5be3364fad1817aece8951d4923870f910566510b2e8453c9ea5fe943a180096df648f397ad7efb072acdd1674bbaf06fe2e68a61c8cbfcbf944bb926756fd29f368951f8b53e897a7")]
[assembly: InternalsVisibleTo("NetWebScript.Test, PublicKey=002400000480000094000000060200000024000052534131000400000100010093f5b897959d0750070b0857c5f2d123987ee46191d4688797c5df2a8ae8c5ce2593a971842b334ab3eba209566ce6cbb92490f5f1bbba5be3364fad1817aece8951d4923870f910566510b2e8453c9ea5fe943a180096df648f397ad7efb072acdd1674bbaf06fe2e68a61c8cbfcbf944bb926756fd29f368951f8b53e897a7")]
#else
[assembly: InternalsVisibleTo("NetWebScript.Test.Material")]
[assembly: InternalsVisibleTo("NetWebScript.Test")]
#endif

