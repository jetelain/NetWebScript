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
using System.Windows.Shapes;
using NetWebScript.Debug.App.ViewModel;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Document;
using System.IO;
using ICSharpCode.AvalonEdit.Highlighting;
using NetWebScript.Debug.App.Controls;
using ICSharpCode.AvalonEdit.Editing;
using ICSharpCode.AvalonEdit.Rendering;
using NetWebScript.Debug.Server;

namespace NetWebScript.Debug.App
{
    /// <summary>
    /// Interaction logic for ProgramWindow.xaml
    /// </summary>
    public partial class ProgramWindow : Window
    {
        private readonly ProgramModel program;
        private readonly ProgramsListWindow parent;
        private readonly BreakPointsMargin breakPointMargin;
        private readonly BreakPointsBackground background;

        private string currentFile = null;

        public ProgramWindow(ProgramsListWindow parent, ProgramModel program)
        {
            this.program = program;
            this.parent = parent;

            DataContext = program;

            InitializeComponent();

            Title = string.Format("NWS Debugger: {0}", program.Name);

            breakPointMargin = new BreakPointsMargin();
            breakPointMargin.LineClick += LineClick;

            background = new BreakPointsBackground();

            editor.TextArea.LeftMargins.Insert(0, breakPointMargin);
            editor.TextArea.TextView.BackgroundRenderers.Add(background);

            program.BreakPointsChanged += BreakPointsChanged;
            program.CurrentPointChanged += CurrentPointChanged;
        }

        void CurrentPointChanged(object sender, EventArgs e)
        {
            if (program.CurrentPoint != null && program.CurrentPoint.FileName != currentFile)
            {
                OpenFile(program.CurrentPoint.FileName);
            }
            else
            {
                UpdateCurrentPoint();
                InvalidateDebugVisuals();
            }
            if ( program.CurrentPoint != null )
            {
                editor.ScrollTo(program.CurrentPoint.StartRow, program.CurrentPoint.StartCol);
            }
            Activate();
        }

        void BreakPointsChanged(object sender, EventArgs e)
        {
            UpdateBreakPoints();
            InvalidateDebugVisuals();
        }

        void LineClick(object sender, SelectLineEventArgs e)
        {
            if (currentFile != null)
            {
                program.TryAddBreakPoint(currentFile, e.LineNumber);
            }
        }

        public ProgramModel Program
        {
            get { return program; }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            parent.Unregister(this);
        }

        protected void OpenFile(object sender, MouseButtonEventArgs e)
        {
            var file = ((TreeViewItem)sender).DataContext as SourceFileModel;
            if (file != null)
            {
                OpenFile(file.Fullname);
            }
        }

        private void OpenFile(string filename)
        {
            background.ActiveBreakSegments = null;
            background.CurrentSegment = null;
            breakPointMargin.ActiveBreakLines = null;
            currentFile = filename;

            editor.Document =  new TextDocument(File.ReadAllText(currentFile));

            UpdateBreakPoints();
            UpdateCurrentPoint();

            InvalidateDebugVisuals();
        }

        private ISegment Segment(JSDebugPoint point)
        {
            var document = editor.Document;

            return new SimpleSelection(
                document.GetOffset(point.StartRow, point.StartCol), 
                document.GetOffset(point.EndRow, point.EndCol));
        }

        private void UpdateBreakPoints()
        {
            var activesOnCurrentFile = program.ActivePoints.Where(a => a.FileName == currentFile);

            breakPointMargin.ActiveBreakLines = activesOnCurrentFile.Select(p => p.StartRow).ToList();
            background.ActiveBreakSegments = activesOnCurrentFile.Select(Segment).ToList();
        }

        private void UpdateCurrentPoint()
        {
            if (program.CurrentPoint == null || program.CurrentPoint.FileName != currentFile)
            {
                background.CurrentSegment = null;
            }
            else
            {
                background.CurrentSegment = Segment(program.CurrentPoint);
            }
        }

        private void InvalidateDebugVisuals()
        {
            breakPointMargin.InvalidateVisual();
            editor.TextArea.TextView.InvalidateLayer(KnownLayer.Background);
        }

        private void Continue(object sender, RoutedEventArgs e)
        {
            program.Continue();
        }
        private void StepOver(object sender, RoutedEventArgs e)
        {
            program.StepOver();
        }
        private void StepIn(object sender, RoutedEventArgs e)
        {
            program.StepInto();
        }
        private void StepOut(object sender, RoutedEventArgs e)
        {
            program.StepOut();
        }

    }
}
