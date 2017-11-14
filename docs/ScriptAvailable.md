# Script Available Types

A script available type is a type that code will be transformed into JavaScript. Only classes, interfaces, and enums can be made script available.

To make a type script available, you have simply to set the "ScriptAvaible" attribute on the type declaration.

## Types manipulable by a script available type

A script available type can use only types that have a JavaScript version, or that are native JavaScript types.

In a script available you can use :
* Other script avaible types
* [Imported types](Imported), aka natives JavaScript types
* Types that have a [script equivalent version](ScriptEquivalent) (NetWebScript includes equivalents of some .Net standard classes)
* Primitives types supported by NetWebScript (bool, int, char, etc.)

## {anchor:Exported}Make a type manipulable into plain JavaScript

By default, a script avaible type cannot be used in plain JavaScript : all identifiers are generated. 
You have to set the "Exported" attribute on the class or interface declaration (in addition to "ScriptAvaible" attribute), to make the type visible to plain JavaScript.

Some limitations :
* Only public members will be visible
* Only one member can be public by name (JavaScript cannot handle easily overloads)
* Only one constructor can be public (same reason)
* Static fields are not supported

In the "Exported" attribute you can set the JavaScript name of the class (Name and IgnoreNamespace) and specify members name transformation (CaseConvention).

### Example
C# side :
{code:c#}
    [ScriptAvailable, Exported(IgnoreNamespace=true)](ScriptAvailable,-Exported(IgnoreNamespace=true))
    public class ExportedA
    {
        private readonly string info;

        public ExportedA(string info)
        {
            this.info = info;
        }

        public string Info
        {
            get { return info; }
        }
    }
{code:c#}
JavaScript side :
{code:javascript}
var obj = new ExportedA("Hello world");
alert(obj.get_Info());
{code:javascript}