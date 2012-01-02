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
using NetWebScript.Debug.App.ViewModel;

namespace NetWebScript.Debug.App
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class ProgramsListWindow : Window
    {
        private readonly WebServer server = new WebServer("http://localhost:9090/");
        private readonly List<ProgramWindow> programWindows = new List<ProgramWindow>();
        private readonly DebuggerModel model;

        public ProgramsListWindow()
        {
            model = new DebuggerModel(Dispatcher, server);

            DataContext = model;

            InitializeComponent();

            server.Start();
        }

        ~ProgramsListWindow()
        {
            if (server != null)
            {
                server.Dispose();
            }
        }

        public DebuggerModel Debugger
        {
            get { return model; }
        }


        protected override void OnClosed(EventArgs e)
        {
            foreach (var window in programWindows.ToArray())
            {
                window.Close();
            }
            base.OnClosed(e);
            server.Dispose();
        }

        protected void OpenProgramWindow(object sender, MouseButtonEventArgs e)
        {
            var program = ((ListViewItem)sender).DataContext as ProgramModel;
            if (program != null)
            {
                var window = programWindows.FirstOrDefault(w => w.Program == program);
                if (window == null)
                {
                    window = new ProgramWindow(this, program);
                    programWindows.Add(window);
                }
                window.Show();
            }
        }

        internal void Unregister(ProgramWindow programWindow)
        {
            programWindows.Remove(programWindow);
        }
    }
}
