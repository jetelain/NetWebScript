using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.AvalonEdit.Rendering;
using ICSharpCode.AvalonEdit.Document;
using System.Windows.Media;

namespace NetWebScript.Debug.App.Controls
{
    class BreakPointsBackground : IBackgroundRenderer
    {
        public void Draw(TextView textView, DrawingContext drawingContext)
        {
            if (ActiveBreakSegments != null && ActiveBreakSegments.Count > 0)
            {
                var brush = new SolidColorBrush(Colors.Red);
                brush.Opacity = 0.4;
                brush.Freeze();

                foreach (var segment in ActiveBreakSegments)
                {
                    BackgroundGeometryBuilder backgroundGeometryBuilder = new BackgroundGeometryBuilder();
                    backgroundGeometryBuilder.AlignToMiddleOfPixels = true;
                    backgroundGeometryBuilder.AddSegment(textView, segment);
                    drawingContext.DrawGeometry(brush, null, backgroundGeometryBuilder.CreateGeometry());
                }
            }
            if (CurrentSegment != null)
            {
                var brush = new SolidColorBrush(Colors.Yellow);
                BackgroundGeometryBuilder backgroundGeometryBuilder = new BackgroundGeometryBuilder();
                backgroundGeometryBuilder.AlignToMiddleOfPixels = true;
                backgroundGeometryBuilder.AddSegment(textView, CurrentSegment);
                drawingContext.DrawGeometry(brush, null, backgroundGeometryBuilder.CreateGeometry());
            }
        }

        public KnownLayer Layer
        {
            get { return KnownLayer.Background; }
        }

        public List<ISegment> ActiveBreakSegments { get; set; }

        public ISegment CurrentSegment { get; set; }
    }
}
