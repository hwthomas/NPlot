using System;
using System.Drawing;

namespace NPlot
{
	/// <summary>
	/// Mouse Scroll (wheel) increases or decreases both axes scaling factors
	/// Zoom direction is Up/+ve/ZoomIn or Down/-ve/ZoomOut.  If the mouse
	/// pointer is inside the plot area, its position is used as the focus point
	/// of the zoom, otherwise the centre of the plot is used as the default
	/// </summary>
	public class PlotZoom : Interaction
	{
		private double sensitivity_ = 1.0;	// default value
		private Rectangle focusRect = Rectangle.Empty;
		private Point pF = Point.Empty;
   
		/// <summary>
		/// Mouse Scroll (wheel) method for AxisZoom interaction
		/// </summary>
		public override bool DoMouseScroll (int X, int Y, int direction, Modifier keys, InteractivePlotSurface2D ps)
		{
			double proportion = 0.1*sensitivity_;	// use initial zoom of 10%
			double focusX = 0.5, focusY = 0.5;		// default focus point
				
			// Zoom direction is +1 for Up/ZoomIn, or -1 for Down/ZoomOut
			proportion *= -direction;

			// delete previous focusPoint drawing - this is all a bit 'tentative'
			ps.QueueDraw (focusRect);

			Rectangle area = ps.PlotAreaBoundingBoxCache;
			if (area.Contains(X,Y)) {
				pF.X = X;
				pF.Y = Y;
				focusX = (double)(X - area.Left)/(double)area.Width;
				focusY = (double)(area.Bottom - Y)/(double)area.Height;
			}

			// Zoom in/out for all defined axes
			ps.CacheAxes();
			ps.ZoomXAxes (proportion,focusX);
			ps.ZoomYAxes (proportion,focusY);

			int x = pF.X-10;
			int y = pF.Y-10;

			focusRect = new Rectangle (x, y, 21, 21);
			// draw new focusRect
			ps.QueueDraw (focusRect);
				
			return (true);
		}

		/// <summary>
		/// MouseMove method for PlotScroll
		/// </summary>
		public override bool DoMouseMove (int X, int Y, Modifier keys, InteractivePlotSurface2D ps)
		{
			// delete previous focusPoint drawing
			ps.QueueDraw (focusRect);
			return false;
		}

		public override void DoDraw (Graphics g, Rectangle dirtyRect)
		{
			DrawFocus (g);
		}

		/// <summary>
		/// Sensitivity factor for axis zoom
		/// </summary>
		/// <value></value>
		public double Sensitivity
		{
			get { return sensitivity_; }
			set { sensitivity_ = value; }
		}

		private void DrawFocus (Graphics g)
		{
			// Draw the Focus-point when zooming
			if (focusRect != Rectangle.Empty) {
				using (Pen rPen = new Pen (Color.White)) {
					g.DrawRectangle (rPen, focusRect);
				}
			}
		}

	} // Mouse Wheel Zoom
	
}
