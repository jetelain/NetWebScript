# Report an issue

## Incorrect behaviour

A method, a class, or a property does not work the same way when executed by the regular .Net CLR and by NetWebScript.

Create an unit test that works with regular .Net CLR, and submit it into the "Issue Tracker" (affect to component "Base Class Library")

## Missing feature ("method is not script available")

A method, class or property you would like to use is not script available, and thus create a compilation error.

Submit a feature request in the "Issue Tracker" (affect to component "Base Class Library")

## Fatal exception of compiler 

Possible errors (but not limited to that list) :
* "Unable to enforce inline."
* "Unsupported flow stack behaviour."
* "Operation is not valid due to the current state of the object".
* "Stack should be empty"
* "Inconsistant stack height"
* "Avoid returning a value in a try, or using statement."
* "Unsupported loop detected from block"
* "Unsupported condition detected from block"
* "Unsupported switch detected from block"

Extract the method, in your code, that cause the error. Copy all outputs from compilation.
Submit those informations to the "Issue Tracker" (affect to component "Compiler")

Try to refactor your method, to continue working. Reducing method size, and cyclomatic complexity can avoid the problem.


