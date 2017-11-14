# Limitations
## JS CLR 

Not all .Net code can be converted into JavaScript due to JavaScript limitations and to NetWebScript implementations.
* "unsafe" code (pointers etc.) is not supported.
* "struct", aka value types, are not supported, and cannot be made script available.
* "extern" is not supported.
* Generic methods into an interface is not supported.
* Int64/UInt64 (long, unsigned long) are not supported as JavaScript cannot handle properly such "large" numbers.

Note: Limitations to "unsafe", "struct" and "extern" in existing code can be solved using [script equivalent types](ScriptEquivalent).

Limitations to be solved in future versions :
* Integers overflows are not yet "simulated"

## API Limitations 

* System.Reflection is not, and will not be supported, to limit footprint of generated JavaScript.
* Most APIs have no script equivalent at this time, and thus cannot be yet used into script available types.