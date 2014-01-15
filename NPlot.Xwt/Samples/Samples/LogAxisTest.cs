//
// XwPlot - A cross-platform charting library using the Xwt toolkit
// 
// LogAxisTest.cs
// Copyright (C) 2003-2006 Matt Howlett and others
// Ported from NPlot to Xwt 2012-2014 : Hywel Thomas <hywel.w.thomas@gmail.com>
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
	public class LogAxisTest : Canvas
	{

		public LogAxisTest () : base ()
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

			tl.X = bounds.Left + 30;	tl.Y = bounds.Top + 20;
			br.X = bounds.Right - 30;	br.Y = bounds.Top + 20;

			LogAxis log = new LogAxis (1, 10000);

			log.Draw (ctx, tl, br, out boundingBox);

			log.Reversed = true;
			tl.Y += 50;	br.Y += 50;

			log.Draw (ctx, br, tl, out boundingBox);


			// Test for default TicksAngle on positive X-axis, ie Ticks below X-axis
			LogAxis lx = new LogAxis (100, 100000);
			lx.NumberFormat = "{0:0.0E+0}";

			tl.X = bounds.Left + 30;	tl.Y = bounds.Bottom - 60;
			br.X = bounds.Right - 30;	br.Y = bounds.Bottom - 60;

			lx.Draw (ctx, tl, br, out boundingBox);

		}
		#endregion

	} // class LogAxisTests
	
} // namespace Samples
