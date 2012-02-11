
namespace NetWebScript.Remoting
{
    /// <summary>
    /// Base class for proxy implementations.
    /// </summary>
    /// <seealso cref="ScriptTransparentProxy"/>
    [ScriptAvailable]
    public abstract class ScriptRealProxy
    {
        /// <summary>
        /// Invokes the specified method on the remote object
        /// </summary>
        /// <param name="typeId">Proxied type identifier (can be different of method declaring type)</param>
        /// <param name="methodId">Method identifier</param>
        /// <param name="arguments">Method arguments</param>
        /// <returns>Result of method (null if method return type is <c>void</c>)</returns>
        public abstract object Invoke(string typeId, string methodId, object[] arguments);
    }
}
