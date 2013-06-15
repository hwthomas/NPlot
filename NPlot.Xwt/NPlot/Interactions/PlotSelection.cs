using System;
using System.Drawing;

namespace NPlot
{
	/// <summary>
	/// Uses a Rubberband rectangle to select an area of the plot for the new Plot Range
	/// </summary>
	public class PlotSelection : Interaction
	{
		private bool selectionActive = false;
		private Point startPoint = Point.Empty;
		private Point endPoint = Point.Empty;
		private Rectangle selection = Rectangle.Empty;
		private Color lineColor = Color.White;


		public PlotSelection ()
		{
			lineColor = Color.White;
		}

		/// <summary>
		/// Constructor with specific color
		/// </summary>
		public PlotSelection (Color color)
		{
			lineColor = color;
		}

		/// <summary>
		/// PlotSelection LineColor
		/// </summary>
		public Color LineColor
		{
			get { return lineColor; }
			set { lineColor = value; }
		}

		/// <summary>
		/// Mouse Down method for Rubberband selection of plot region
		/// </summary>
		public override bool DoMouseDown (int X, int Y, Modifier keys, InteractivePlotSurface2D ps)
		{
			// Only start selection if mouse is inside plot area (excluding axes)
			if (ps.PlotAreaBoundingBoxCache.Contains(X,Y)) {
				selectionActive = true;
				startPoint.X = X;
				startPoint.Y = Y;
				endPoint = startPoint;
			}
			return false;
		}

		/// <summary>
		/// MouseMove method for Rubberband selection of plot area
		/// </summary>
		public override bool DoMouseMove (int X, int Y, Modifier keys, InteractivePlotSurface2D ps)
		{
			if (((keys & Modifier.Button1) != 0) && selectionActive) {
				// note last selection rectangle
				Rectangle lastSelection = selection;
				Rectangle bounds = ps.PlotAreaBoundingBoxCache;
				// clip selection rectangle to PlotArea
				X = Math.Max(X, bounds.Left);
				X = Math.Min(X, bounds.Right);
				Y = Math.Max(Y, bounds.Top);
				Y = Math.Min(Y, bounds.Bottom);

				endPoint.X = X;
				endPoint.Y = Y;
				selection = FromPoints (startPoint, endPoint);

				ps.QueueDraw (lastSelection);
				//Console.WriteLine ("Erase: {0} {1} {2} {3} ", lastSelection.X, lastSelection.Y, lastSelection.Width, lastSelection.Height);
				ps.QueueDraw (selection);
			}
			return false;
		}

		/// <summary>
		/// MouseUp method for RubberBand selection
		/// </summary>
		/// <param name="X">mouse X position</param>
		/// <param name="Y"> mouse Y position</param>
		/// <param name="keys"> mouse and keyboard modifiers</param>
		/// <param name="ps">the InteractivePlotSurface2D</param>
		public override bool DoMouseUp (int X, int Y, Modifier keys, InteractivePlotSurface2D ps)
		{
			bool modified = false;

			// delete previous overlay rectangle
			ps.QueueDraw (selection);
			if (selectionActive) {

				selectionActive = false;
				Rectangle bounds = ps.PlotAreaBoundingBoxCache;
				if (!bounds.Contains(endPoint)) {
					// MouseUp outside plotArea - cancel selection
					modified = false;
				}
				else {
					ps.CacheAxes();
					// Redefine range based on selection. The proportions for
					// Min and Max do not require Min < Max, since they will
					// be re-ordered by Axis.DefineRange if necessary
					double xMin = startPoint.X - bounds.Left;
					double yMin = bounds.Bottom - startPoint.Y;
				
					double xMax = endPoint.X - bounds.Left;
					double yMax = bounds.Bottom - endPoint.Y;
				
					double xMinProp = xMin/bounds.Width;
					double xMaxProp = xMax/bounds.Width;
					double yMinProp = yMin/bounds.Height;
					double yMaxProp = yMax/bounds.Height;
				
					ps.DefineXAxes (xMinProp, xMaxProp);
					ps.DefineYAxes (yMinProp, yMaxProp);
					modified = true;
				}
			}
			return modified;
		}

		/// <summary>
		/// MouseLeave method for RubberBand selection
		/// </summary>
		/// <param name="ps">the InteractivePlotSurface2D</param>
		public override bool DoMouseLeave (InteractivePlotSurface2D ps)
		{
			if (selectionActive) {
				ps.QueueDraw (selection);
				selectionActive = false;
			}
			return false;
		}

		public override void DoDraw (Graphics g, Rectangle dirtyRect)
		{
			if (selectionActive && selection != Rectangle.Empty) {
				Rectangle rect = selection;
				// allow for GDI+ rectangle draw/fill anomaly
				rect.Width	-= 1;
				rect.Height -= 1;
				using (Pen rPen = new Pen (lineColor)) {
					g.DrawRectangle (rPen, rect);
				}
				//Console.WriteLine ("Draw: {0} {1} {2} {3} ", rect.X, rect.Y, rect.Width, rect.Height);
			}
		}

		/// <summary>
		/// Return normalised Rectangle from two diagonal points, reordering if necessary
		/// </summary>
		private Rectangle FromPoints (Point start, Point end)
		{
			Point tl = start;
			Point br = end;
			if (start.X > end.X) {
				tl.X = end.X;
				br.X = start.X;
			}
			if (start.Y > end.Y) {
				tl.Y = end.Y;
				br.Y = start.Y;
			}
			int w = br.X - tl.X + 1;
			int h = br.Y - tl.Y + 1;
			return new Rectangle (tl.X, tl.Y, w, h);
		}

	} // Plot Selection
		
}
