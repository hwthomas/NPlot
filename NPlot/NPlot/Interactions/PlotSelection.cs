using System;
using System.Drawing;

namespace NPlot
{
    /// <summary>
    /// Uses a Rubberband rectangle to select an area of the plot to become the new Plot Range
    /// </summary>
    public class PlotSelection : Interaction
    {
        private Point unset_ = new Point(-1, -1);       // any unset point
        private Point startPoint_ = new Point(-1, -1);
        private Point endPoint_ = new Point(-1, -1);

        private bool selectionActive_ = false;
 
        /// <summary>
        /// Mouse Down method for RubberBand selection
        /// </summary>
        public override bool DoMouseDown (int X, int Y, Modifier keys, InteractivePlotSurface2D ps)
        {
            // Only start selection if mouse is inside plot area (excluding axes)
            if (ps.PlotAreaBoundingBoxCache.Contains(X,Y)) {
                selectionActive_ = true;
                startPoint_.X = X;
                startPoint_.Y = Y;
                // invalidate end point
                endPoint_ = unset_;
            }
            return false;
        }

        /// <summary>
        /// MouseMove method for Rubberband selection
        /// </summary>
        public override bool DoMouseMove (int X, int Y, Modifier keys, InteractivePlotSurface2D ps)
        {
            if (((keys & Modifier.Button1) != 0) && selectionActive_)
            {
                // clip X and Y to PlotArea
                Rectangle bounds = ps.PlotAreaBoundingBoxCache;
                // clip selection rectangle
                X = Math.Max(X, bounds.Left);
                X = Math.Min(X, bounds.Right);
                Y = Math.Max(Y, bounds.Top);
                Y = Math.Min(Y, bounds.Bottom);

                // normalise previous selection rectangle and delete
                if (endPoint_ != unset_) {
                    int x = (int)Math.Min (startPoint_.X, endPoint_.X);
                    int y = (int)Math.Min (startPoint_.Y, endPoint_.Y);
                    int w = (int)Math.Abs (startPoint_.X - endPoint_.X);
                    int h = (int)Math.Abs (startPoint_.Y - endPoint_.Y);
                    Rectangle extents = new Rectangle (x, y, w+1, h+1);
                    ps.QueueDraw (extents);
                }
                endPoint_.X = X;
                endPoint_.Y = Y;
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

            if (selectionActive_) {
                // normalise and delete previous overlay rectangle
                int x = (int)Math.Min (startPoint_.X, endPoint_.X);
                int y = (int)Math.Min (startPoint_.Y, endPoint_.Y);
                int w = (int)Math.Abs (startPoint_.X - endPoint_.X);
                int h = (int)Math.Abs (startPoint_.Y - endPoint_.Y);
                Rectangle extents = new Rectangle (x, y, w+1, h+1);
                ps.QueueDraw (extents);
                endPoint_.X = X;
                endPoint_.Y = Y;

                Rectangle bounds = ps.PlotAreaBoundingBoxCache;
                if (!bounds.Contains(endPoint_)) {
                    // MouseUp outside plotArea - cancel selection
                    modified = false;
                }
                else {
                    ps.CacheAxes();
                    // Redefine range based on selection. The proportions for
                    // Min and Max do not require Min < Max, since they will
                    // be re-ordered by Axis.DefineRange if necessary
                    double xMin = startPoint_.X - bounds.Left;
                    double yMin = bounds.Bottom - startPoint_.Y;
                
                    double xMax = endPoint_.X - bounds.Left;
                    double yMax = bounds.Bottom - endPoint_.Y;
                
                    double xMinProp = xMin/bounds.Width;
                    double xMaxProp = xMax/bounds.Width;
                    double yMinProp = yMin/bounds.Height;
                    double yMaxProp = yMax/bounds.Height;
                
                    ps.DefineXAxes (xMinProp, xMaxProp);
                    ps.DefineYAxes (yMinProp, yMaxProp);
                    modified = true;
                }
                selectionActive_ = false;
                startPoint_ = unset_;
                endPoint_ = unset_;
            }
            return modified;
        }

        /// <summary>
        /// MouseLeave method for RubberBand selection
        /// </summary>
        /// <param name="ps">the InteractivePlotSurface2D</param>
        public override bool DoMouseLeave (InteractivePlotSurface2D ps)
        {
            if (selectionActive_) {
                selectionActive_ = false;
                if (endPoint_ != unset_) {
                    int x = (int)Math.Min (startPoint_.X, endPoint_.X);
                    int y = (int)Math.Min (startPoint_.Y, endPoint_.Y);
                    int w = (int)Math.Abs (startPoint_.X-endPoint_.X);
                    int h = (int)Math.Abs (startPoint_.Y-endPoint_.Y);
                    Rectangle extents = new Rectangle (x, y, w+1, h+1);
                    ps.QueueDraw (extents);
                }
                startPoint_ = unset_;
                endPoint_ = unset_;
            }
            return false;
        }

        public override void DoDraw (Graphics g, Rectangle dirtyRect)
        {
            if (startPoint_ != unset_ && endPoint_ != unset_) {
                // Draw the rectangle defined by (startPoint_, endPoint_)
                int x = (int)Math.Min (startPoint_.X, endPoint_.X);
                int y = (int)Math.Min (startPoint_.Y, endPoint_.Y);
                int w = (int)Math.Abs (startPoint_.X - endPoint_.X);
                int h = (int)Math.Abs (startPoint_.Y - endPoint_.Y);
                Rectangle extents = new Rectangle (x, y, w, h);
                using (Pen rPen = new Pen (Color.White)) {
                    g.DrawRectangle (rPen, extents);
                }
            }
        }

    } // Plot Selection
        
}
