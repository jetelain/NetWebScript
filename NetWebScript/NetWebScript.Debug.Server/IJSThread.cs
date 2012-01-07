
namespace NetWebScript.Debug.Server
{
    public interface IJSThread
    {
        void RegisterCallback(IJSThreadCallback callback);

        void UnRegisterCallback(IJSThreadCallback callback);

        /// <summary>
        /// If thread is breaked, step over current line of code
        /// </summary>
        void StepOver();

        /// <summary>
        /// If thread is breaked, step out to current method caller
        /// </summary>
        void StepOut();

        /// <summary>
        /// If thread is breaked, step in current line method call
        /// </summary>
        void StepInto();

        /// <summary>
        /// If thread is breaked, continue until next break point
        /// </summary>
        void Continue();

        /// <summary>
        /// Current state of thread
        /// </summary>
        JSThreadState State { get; }

        /// <summary>
        /// Unique identifier number within parent <see cref="IJSProgram"/>.
        /// </summary>
        int Id { get; }

        /// <summary>
        /// Retreive missing data of supplied <see cref="JSData"/>.
        /// </summary>
        /// <param name="data">Data to expand</param>
        /// <returns>Expanded version of data</returns>
        JSData Expand(JSData data);
    }
}
