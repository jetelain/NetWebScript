using System;
using System.IO;
using System.Linq;
using Microsoft.Build.Framework;
using NetWebScript.JsClr.Compiler;

namespace NetWebScript.Task
{
    /// <summary>
    /// Task to launch NetWebScript compiler
    /// </summary>
    public sealed class CompileScriptTask : Microsoft.Build.Utilities.AppDomainIsolatedTask, IErrorReporter
    {
        [Required]
        public string OutputPath { get; set; }

        [Required]
        public string ModuleName { get; set; }

        public bool Debuggable { get; set; }

        public bool PrettyPrint { get; set; }

        public string PageClassName { get; set; }

        [Required]
        public ITaskItem[] Assemblies { get; set; }

        public override bool Execute()
        {
            var compiler = new Compiler(this);

            var result = false;
            try
            {
                var directory = new DirectoryInfo(OutputPath);
                if (!directory.Exists)
                {
                    directory.Create();
                }

                compiler.OutputPath = OutputPath;
                compiler.Name = ModuleName;
                compiler.Debug = Debuggable;
                compiler.Pretty = PrettyPrint;
                compiler.Page = PageClassName;
                compiler.AssembliesPath.AddRange(Assemblies.Select(a => a.ItemSpec));

                result = compiler.Compile();
            }
            catch (Exception e)
            {
                Log.LogErrorFromException(e);
                result = false;
            }
            return result;
        }

        public void Error(string p)
        {
            Log.LogError(p);
        }

        public void Warning(string p)
        {
            Log.LogWarning(p);
        }

        public void Info(string p)
        {
            Log.LogMessage(p);
        }

        public void Report(CompilerMessage message)
        {
            switch(message.Severity)
            {
                case MessageSeverity.Error:
                    Log.LogError(null, null, null, message.Filename, message.LineNumber, message.LinePosition, message.EndLineNumber, message.EndLinePosition, message.Message);
                    break;
                case MessageSeverity.Warning:
                    Log.LogWarning(null, null, null, message.Filename, message.LineNumber, message.LinePosition, message.EndLineNumber, message.EndLinePosition, message.Message);
                    break;
                case MessageSeverity.Info:
                    Log.LogMessage(null, null, null, message.Filename, message.LineNumber, message.LinePosition, message.EndLineNumber, message.EndLinePosition, message.Message);
                    break;
            }
        }
    }
}
