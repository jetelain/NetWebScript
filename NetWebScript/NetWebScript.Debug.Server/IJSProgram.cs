using System;
using System.Collections.Generic;

namespace NetWebScript.Debug.Server
{
    public interface IJSProgram
    {
        ICollection<IJSThread> Threads { get; }

        /// <summary>
        /// Unique identifier number within parent <see cref="IJSServer"/>.
        /// </summary>
        int Id { get; }

        string Name { get; }

        Uri Uri { get; }

        IList<JSModuleInfo> Modules { get; }

        /// <summary>
        /// Disable all active breakpoints and detach all threads
        /// </summary>
        void ClearAndDetachAll();

        void RegisterCallback ( IJSProgramCallback callback );

        void UnRegisterCallback(IJSProgramCallback callback);

        /// <summary>
        /// Lookup all potential break points at a line and a columns of a source file.
        /// </summary>
        /// <param name="fileName">Source file name</param>
        /// <param name="startCol">Columns number (start at 1)</param>
        /// <param name="startRow">Line number (start at 1)</param>
        /// <returns>List of matching potential break points</returns>
        List<JSDebugPoint> FindPoints(string fileName, int startCol, int startRow);

        /// <summary>
        /// Lookup all potential break points at a line of a source file.
        /// </summary>
        /// <param name="fileName">Source file name</param>
        /// <param name="startRow">Line number (start at 1)</param>
        /// <returns>List of matching potential break points</returns>
        List<JSDebugPoint> FindPoints(string fileName, int startRow);

        /// <summary>
        /// Build list of all files that have one or more known <see cref="JSDebugPoint"/>.
        /// </summary>
        /// <returns></returns>
        List<string> ListSourceFiles();

        /// <summary>
        /// List of all active break points.
        /// </summary>
        List<JSDebugPoint> ActivePoints { get; }

        /// <summary>
        /// Disable a break point
        /// </summary>
        /// <remarks>
        /// If breakpoint is not in <see cref="ActivePoints"/> list, this method has no effect.
        /// </remarks>
        /// <param name="point"></param>
        void RemoveBreakPoint(JSDebugPoint point);

        /// <summary>
        /// Activate a break point
        /// </summary>
        /// <remarks>
        /// If breakpoint is in <see cref="ActivePoints"/> list, this method has no effect.
        /// </remarks>
        /// <param name="point"></param>
        void AddBreakPoint(JSDebugPoint point);

    }
}
