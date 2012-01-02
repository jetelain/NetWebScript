using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using NetWebScript.Debug.Server;
using System.Windows.Threading;

namespace NetWebScript.Debug.App.ViewModel
{
    public class DebuggerModel : IJSServerCallback, IDisposable
    {
        private readonly IJSServer server;
        private readonly Dispatcher dispatcher;

        private readonly DispatcherObservableCollection<ProgramModel> programs;

        public DebuggerModel (Dispatcher dispatcher, IJSServer server)
        {
            this.dispatcher = dispatcher;
            this.server = server;
            this.programs = new DispatcherObservableCollection<ProgramModel>(dispatcher);
            server.RegisterCallback(this);
            foreach (var program in server.Programs)
            {
                programs.Add(new ProgramModel(dispatcher, program));
            }
        }

        public ObservableCollection<ProgramModel> Programs
        {
            get { return programs; }
        }

        public void OnNewProgram(IJSProgram program)
        {
            programs.AddAsync(new ProgramModel(dispatcher, program));
        }

        public void Dispose()
        {
            server.UnRegisterCallback(this);
        }
    }
}
