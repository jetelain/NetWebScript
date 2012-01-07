using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.AvalonEdit.Editing;
using System.Windows.Media;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.TextFormatting;
using ICSharpCode.AvalonEdit.Rendering;
using System.Windows.Controls;

namespace NetWebScript.Debug.App.Controls
{
    public sealed class SelectLineEventArgs : EventArgs
    {
        public int LineNumber { get; set;}

    }

    public sealed class BreakPointsMargin : AbstractMargin
    {
        
        public BreakPointsMargin()
        {
            
        }

        public List<int> ActiveBreakLines { get; set; }

        /// <inheritdoc/>
        protected override HitTestResult HitTestCore(PointHitTestParameters hitTestParameters)
        {
            // accept clicks even when clicking on the background
            return new PointHitTestResult(this, hitTestParameters.HitPoint);
        }

        /// <inheritdoc/>
        protected override Size MeasureOverride(Size availableSize)
        {
            return new Size(15, 0);
        }

        /// <inheritdoc/>
        protected override void OnRender(DrawingContext drawingContext)
        {
            var textView = TextView;
            if (textView != null && textView.VisualLinesValid)
            {
                var renderSize = RenderSize;
                var halfWidth = renderSize.Width / 2;
                var discSize = halfWidth - 1;
                var breaks = ActiveBreakLines;
                var breakBrush = new SolidColorBrush(Colors.Crimson);
                foreach (VisualLine current in textView.VisualLines)
                {
                    var y = current.VisualTop - textView.VerticalOffset;
                    if (breaks != null && breaks.Contains(current.FirstDocumentLine.LineNumber))
                    {
                        drawingContext.DrawEllipse(breakBrush, null, new Point(halfWidth, y + (current.Height / 2)), discSize, discSize);
                    }
                }
            }
        }

        private int GetTextLineNumber(MouseEventArgs e)
        {
            Point position = e.GetPosition(base.TextView);
            position.X = 0.0;
            position.Y += base.TextView.VerticalOffset;
            VisualLine visualLineFromVisualTop = base.TextView.GetVisualLineFromVisualTop(position.Y);
            if (visualLineFromVisualTop == null)
            {
                return -1;
            }
            return visualLineFromVisualTop.FirstDocumentLine.LineNumber;
        }

        /// <inheritdoc/>
        protected override void OnMouseLeftButtonDown(System.Windows.Input.MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);
            if (!e.Handled && TextView != null && LineClick != null)
            {
                e.Handled = true;
                int lineNumber = GetTextLineNumber(e);
                if (lineNumber == -1)
                {
                    return;
                }
                LineClick(this, new SelectLineEventArgs() {  LineNumber = lineNumber});
            }
        }

        protected override void OnTextViewChanged(TextView oldTextView, TextView newTextView)
        {
            if (oldTextView != null)
            {
                oldTextView.VisualLinesChanged -= new EventHandler(this.TextViewVisualLinesChanged);
            }
            base.OnTextViewChanged(oldTextView, newTextView);
            if (newTextView != null)
            {
                newTextView.VisualLinesChanged += new EventHandler(this.TextViewVisualLinesChanged);
            }
            InvalidateVisual();
        }

        private void TextViewVisualLinesChanged(object sender, EventArgs e)
        {
            base.InvalidateVisual();
        }

        public event EventHandler<SelectLineEventArgs> LineClick;

    }
}
