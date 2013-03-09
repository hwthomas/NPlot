//
// NPlot - A charting library for .NET
// 
// PlotZoom.cs
//
// Copyright (C) Hywel Thomas, Matt Howlett and others 2003-2013
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without modification,
// are permitted provided that the following conditions are met:
// 
// 1. Redistributions of source code must retain the above copyright notice, this
//    list of conditions and the following disclaimer.
// 2. Redistributions in binary form must reproduce the above copyright notice,
//    this list of conditions and the following disclaimer in the documentation
//    and/or other materials provided with the distribution.
// 3. Neither the name of NPlot nor the names of its contributors may
//    be used to endorse or promote products derived from this software without
//    specific prior written permission.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
// IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT,
// INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING,
// BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
// DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
// LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE
// OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED
// OF THE POSSIBILITY OF SUCH DAMAGE.
//
using System;
using System.Drawing;
//using GLib;			// Omit timeout for now

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
        private double sensitivity_ = 1.0;  // default value
        private Rectangle focusRect = Rectangle.Empty;
        private Point p = Point.Empty;
		private bool zoomActive = false;
		private InteractivePlotSurface2D surface;

		public PlotZoom ()
		{
		}

        /// <summary>
        /// Mouse Scroll (wheel) method for AxisZoom interaction
        /// </summary>
        public override bool DoMouseScroll (int X, int Y, int direction, Modifier keys, InteractivePlotSurface2D ps)
        {
			// Add timeout into Gtk loop when scroll starts - Omit for now
			// GLib.Timeout.Add (500, new GLib.TimeoutHandler (zoomTimeout) );
			zoomActive = true;
			surface = ps;

            double proportion = 0.1*sensitivity_;   // use initial zoom of 10%
            double focusX = 0.5, focusY = 0.5;      // default focus point
                
            // Zoom direction is +1 for Up/ZoomIn, or -1 for Down/ZoomOut
            proportion *= -direction;

            // delete previous focusPoint drawing - this is all a bit 'tentative'
            ps.QueueDraw (focusRect);

            Rectangle area = ps.PlotAreaBoundingBoxCache;
            if (area.Contains(X,Y)) {
                p.X = X;
                p.Y = Y;
                focusX = (double)(X - area.Left)/(double)area.Width;
                focusY = (double)(area.Bottom - Y)/(double)area.Height;
            }

            // Zoom in/out for all defined axes
            ps.CacheAxes();
            ps.ZoomXAxes (proportion,focusX);
            ps.ZoomYAxes (proportion,focusY);


			// Note: r = 16, and Focus extents range from x-2*r-1 to x+2*r+1, y-2*r-1 to y+2*r+1
			int x = p.X-31;
			int y = p.Y-31;
			focusRect = new Rectangle (x, y, 64, 64);

			// draw new focusRect
            ps.QueueDraw (focusRect);
                
            return (true);
        }

        /// <summary>
        /// MouseMove method for PlotScroll
        /// </summary>
        public override bool DoMouseMove (int X, int Y, Modifier keys, InteractivePlotSurface2D ps)
        {
			zoomActive = false;
			ps.QueueDraw (focusRect);
            return false;
        }

        public override void DoDraw (Graphics g, Rectangle dirtyRect)
        {
            DrawFocus (g, p);
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

		private void DrawFocus (Graphics g, Point p)
		{
			// Draw a 'Google-Earth'-type Focus-point when zooming
			if (zoomActive) {
				using (Pen rPen = new Pen (Color.White)) {
					Rectangle r1 = new Rectangle (p.X-15, p.Y-15, 32,32);
					g.DrawArc (rPen, r1, 0, 360);
					r1 = new Rectangle (p.X-19, p.Y-19, 40,40);
					g.DrawArc (rPen, r1, 0, 360);
					g.DrawLine (rPen, p.X-30, p.Y, p.X-15, p.Y);
					g.DrawLine (rPen, p.X+30, p.Y, p.X+17, p.Y);
					g.DrawLine (rPen, p.X, p.Y-30, p.X, p.Y-15);
					g.DrawLine (rPen, p.X, p.Y+30, p.X, p.Y+17);
				}
			}
		}


        /// <summary>
        /// Callback for zoom timeout - compiled but not active at present
        /// </summary>
        private bool zoomTimeout ()
		{
			// clear zoom flag and remove last focusPoint
            zoomActive = false;
			surface.QueueDraw (focusRect);
            //returning true means that the timeout routine should be invoked
            //again after the timeout period expires.  Returning false will 
            //terminate the timeout (ie until invoked again by Scrolling)
            return false;
        }

    } // Mouse Wheel Zoom
    
}
