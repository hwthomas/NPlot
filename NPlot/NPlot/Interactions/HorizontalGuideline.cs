//
// NPlot - A charting library for .NET
// 
// HorizontalGuideline.cs
//
// Copyright (C) Hywel Thomas and others.
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without modification,
// are permitted provided that the following conditions are met:
// 
// 1. Redistributions of source code must retain the above copyright notice, this
//	  list of conditions and the following disclaimer.
// 2. Redistributions in binary form must reproduce the above copyright notice,
//	  this list of conditions and the following disclaimer in the documentation
//	  and/or other materials provided with the distribution.
// 3. Neither the name of NPlot nor the names of its contributors may
//	  be used to endorse or promote products derived from this software without
//	  specific prior written permission.
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

namespace NPlot
{
	/// <summary>
	/// Horizontal guideline
	/// </summary>
	public class HorizontalGuideline : Interaction
	{
		private Color lineColor;
		private Rectangle lineExtent = Rectangle.Empty;

		private bool drawPending;		// for testing
		private int overRuns = 0;

		/// <summary>
		/// Constructor with specific color
		/// </summary>
		public HorizontalGuideline (Color color)
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
				int h = 1;
				int w = plotArea.Right - plotArea.Left + 1;
				lineExtent = new Rectangle (plotArea.X, Y, w, h);
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
				Point end = new Point (lineExtent.X + lineExtent.Width - 1, lineExtent.Y);
				using (Pen pen = new Pen (lineColor)) {
					g.DrawLine (pen, start, end);
				}
			}
			drawPending = false;	// clear over-run flag
		}

	} // HorizontalGuideline

}
 
