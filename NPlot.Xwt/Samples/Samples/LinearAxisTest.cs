//
// NPlot - A charting library for .NET
// 
// LinearAxisTest.cs
// Copyright (C) 2003-2006 Matt Howlett and others
// Port to Xwt 2012-2013 : Hywel Thomas <hywel.w.thomas@gmail.com>
//
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

using NPlot;

using Xwt;
using Xwt.Drawing;

namespace Samples
{
	//
	// Create a Canvas into which the Axis Tests can be drawn.
	//
	public class LinearAxisTest : Canvas
	{

		public LinearAxisTest () : base ()
		{
			InitializeComponent();	// Setup Axis Tests Window
		}
		
		/// <summary>
		/// Axis Tests Form setup - all done in base ()
		/// </summary>
		private void InitializeComponent()
		{
		}
		

		/// <summary>
		/// Handles OnDraw Events by drawing AxisTests to the canvas
		/// </summary>
		protected override void OnDraw (Context ctx, Rectangle dirtyRect)
		{
			// Get Canvas Bounds as region to draw into
			Rectangle bounds = this.Bounds;

			DrawAxisTests (ctx, bounds);

			base.OnDraw (ctx, dirtyRect);

		}

		#region Axis Tests

		private void DrawAxisTests (Context ctx, Rectangle bounds)
		{
			Rectangle boundingBox;
			Point tl = Point.Zero;
			Point br = Point.Zero;;

			tl.X = bounds.Left + 30;	tl.Y = bounds.Top + 10;
			br.X = tl.X;				br.Y = bounds.Bottom - 100;
			
			LinearAxis a = new LinearAxis (0, 10);

			a.Draw (ctx, tl, br, out boundingBox);

			a.Reversed = true;
			a.Draw (ctx, new Point (60,10), new Point (60, 200), out boundingBox);

			a.SmallTickSize = 0;
			a.Draw (ctx, new Point(90,10), new Point(90, 200), out boundingBox);

			a.LargeTickStep = 2.5;
			a.Draw (ctx, new Point(120,10), new Point(120,200), out boundingBox);

			a.NumberOfSmallTicks = 5;
			a.SmallTickSize = 2;
			a.Draw (ctx, new Point(150,10), new Point(150,200), out boundingBox);

			a.LineColor = Colors.DarkBlue;
			a.Draw (ctx, new Point(180,10), new Point(180,200), out boundingBox);

			a.TickTextColor= Colors.DarkBlue;
			a.Draw (ctx, new Point(210,10), new Point(210,200), out boundingBox);

			a.TickTextColor = Colors.Black;
			a.Draw (ctx, new Point(240,10), new Point(300,200), out boundingBox);

			a.WorldMax = 100000;
			a.WorldMin = -3;
			a.LargeTickStep = double.NaN;
			a.Draw (ctx, new Point(330,10), new Point(330,200), out boundingBox);

			a.NumberFormat = "{0:0.0E+0}";
			a.Draw (ctx, new Point(380,10), new Point(380,200), out boundingBox);
			
			// Test for default TicksAngle (+90) on positive X-axis, ie Ticks below X-axis
			LinearAxis aX = new LinearAxis (0, 10);

			tl.X = bounds.Left + 30;	tl.Y = bounds.Bottom - 60;
			br.X = bounds.Right - 20;	br.Y = bounds.Bottom - 60;

			aX.Draw (ctx, tl, br, out boundingBox);

			// Set TicksAngle to -45 anti-clockwise from positive X-axis direction
			aX.TicksAngle = Math.PI / 4.0;
			tl.Y += 50;		br.Y += 50;

			aX.Draw (ctx, tl, br, out boundingBox);
			
		}
		#endregion

	} // class LinearAxisTests
	
} // namespace Samples
