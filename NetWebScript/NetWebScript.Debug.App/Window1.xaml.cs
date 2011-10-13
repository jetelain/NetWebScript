using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using NetWebScript.Debug.Server;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace NetWebScript.Debug.App
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window, IJSServerCallback, IJSProgramCallback
    {
        private readonly WebServer server = new WebServer("http://localhost:9090/");

        private readonly HashSet<IJSThread> threads = new HashSet<IJSThread>();

        private readonly ObservableCollection<ThreadInfo> threadsList = new ObservableCollection<ThreadInfo>();

        private class ThreadInfo : INotifyPropertyChanged, IJSThreadCallback
        {
            private JSThreadState state;

            public Window1 Owner { get; set; }

            public IJSThread Thread { get; set; }

            public String Name { get; set; }

            public JSThreadState State
            {
                set
                {
                    state = value; 
                    if (PropertyChanged != null)
                    { 
                        PropertyChanged(this, new PropertyChangedEventArgs("StateName")); 
                    }
                }
            }
            public String StateName { get { return state.ToString(); } }

            public event PropertyChangedEventHandler PropertyChanged;

            public void OnBreakpoint(string id, string stackXml)
            {
                Owner.Dispatcher.BeginInvoke(new Action<ThreadInfo>(Owner.UpdateThreadState), this);
            }

            public void OnStepDone(string id, string stackXml)
            {
                Owner.Dispatcher.BeginInvoke(new Action<ThreadInfo>(Owner.UpdateThreadState), this);
            }

            public void OnStopped()
            {
                Owner.Dispatcher.BeginInvoke(new Action<ThreadInfo>(Owner.UpdateThreadState), this);
            }
        }


        public Window1()
        {
            InitializeComponent();

            list.ItemsSource = threadsList;

            server.RegisterCallback(this);

            server.Start();
        }

        ~Window1()
        {
            if (server != null)
            {
                server.Dispose();
            }
        }


        void UpdateThreadList(IJSThread thread)
        {
            var info = new ThreadInfo() { Thread = thread, Name = threadsList.Count.ToString(), State = thread.State };
            thread.RegisterCallback(info);

            threadsList.Add(info);
        }

        void UpdateThreadState(ThreadInfo info)
        {
            info.State = info.Thread.State;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            server.Dispose();
        }

        private void Continue(object sender, RoutedEventArgs e)
        {
            ThreadInfo info = list.SelectedValue as ThreadInfo;
            if (info != null)
            {
                info.Thread.Continue();
            }
        }

        #region IJSProgramCallback Members

        public void OnNewThread(IJSThread thread)
        {
            Dispatcher.BeginInvoke(new Action<IJSThread>(UpdateThreadList), thread);
        }

        public void OnNewModule(ModuleInfo module)
        {

        }

        #endregion

        #region IJSServerCallback Members

        public void OnNewProgram(IJSProgram program)
        {
            program.RegisterCallback(this);
        }

        #endregion

    }
}
