using System;
using System.Drawing;

namespace NPlot
{
	/// <summary>
	/// Vertical guideline
	/// </summary>
	public class VerticalGuideline : Interaction
	{
		private Color lineColor;
		private Rectangle lineExtent = Rectangle.Empty;

		private bool drawPending;		// for testing
		private int overRuns = 0;

		/// <summary>
		/// Constructor with specific color
		/// </summary>
		public VerticalGuideline (Color color)
		{
			lineColor = color;
		}

		/// <summary>
		/// LineColor property
		/// </summary>
		public Color LineColor
		{
			get { return lineColor; }
			set { lineColor = value; }
		}

		/// <summary>
		/// MouseMove method for Guideline
		/// </summary>
		/// <param name="X">mouse X position</param>
		/// <param name="Y"> mouse Y position</param>
		/// <param name="keys"> mouse and keyboard modifiers</param>
		/// <param name="ps">the InteractivePlotSurface2D</param>
		public override bool DoMouseMove (int X, int Y, Modifier keys, InteractivePlotSurface2D ps)
		{
			Rectangle plotArea = ps.PlotAreaBoundingBoxCache;

			if (drawPending) {
				overRuns += 1;
				return false;
			}

			// note previous guideline ready to erase it
			Rectangle prevExtent = lineExtent;

			// Only display guideline when mouse is within the plotArea
			if (plotArea.Contains(X,Y)) {
				int w = 1;
				int h = plotArea.Bottom - plotArea.Top + 1;
				lineExtent = new Rectangle (X, plotArea.Top, w, h);
				drawPending = true;
			} 
			else {
				lineExtent = Rectangle.Empty;
			}
			ps.QueueDraw (prevExtent);
			ps.QueueDraw (lineExtent);
			return false;
		}

		/// <summary>
		/// MouseLeave method for Guideline
		/// </summary>
		/// <param name="ps">the InteractivePlotSurface2D</param>
		public override bool DoMouseLeave (InteractivePlotSurface2D ps)
		{
			if (lineExtent != Rectangle.Empty) {
				// erase previous vertical guideline
				ps.QueueDraw (lineExtent);
			}
			lineExtent = Rectangle.Empty;
			return false;
		}


		public override void DoDraw (Graphics g, Rectangle dirtyRect)
		{
			// Draw guideline based on current lineExtent if non-Empty
			//Console.WriteLine ("Drawing: {0} {1} {2} {3} ", lineExtent.X, lineExtent.Y, lineExtent.Width, lineExtent.Height);
			if (lineExtent != Rectangle.Empty) {
				Point start = new Point (lineExtent.X, lineExtent.Y);
				Point end = new Point (lineExtent.X, lineExtent.Y + lineExtent.Height - 1);
				using (Pen pen = new Pen (lineColor)) {
					g.DrawLine (pen, start, end);
				}
				//Console.WriteLine ("Draw: {0} {1} {2} {3} ", lineExtent.X, lineExtent.Y, lineExtent.Width, lineExtent.Height);
			}
			drawPending = false;	// clear over-run flag
		}

	} // VerticalGuideline

}
 
