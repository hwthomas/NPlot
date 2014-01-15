//
// XwPlot - A cross-platform charting library using the Xwt toolkit
// 
// TradingDateTimeAxisTest.cs
// Copyright (C) 2003-2006 Matt Howlett and others
// Port to Xwt 2012-2014 : Hywel Thomas <hywel.w.thomas@gmail.com>
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
	public class TradingDateTimeAxisTest : Canvas
	{

		public TradingDateTimeAxisTest () : base ()
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
			Point br = Point.Zero;

			tl.X = bounds.Left + 30;	tl.Y = bounds.Top + 20;
			br.X = bounds.Right - 30;	br.Y = bounds.Top + 20;

			DateTime timeMin = new DateTime (2003, 10, 22, 15, 00, 00);
			DateTime timeMax = new DateTime (2004, 03, 12, 13, 30, 00);

			TradingDateTimeAxis axis = new TradingDateTimeAxis ();
			axis.WorldMin = (double)timeMin.Ticks;
			axis.WorldMax = (double)timeMax.Ticks;
			axis.Draw (ctx, tl, br, out boundingBox);

			timeMin = new DateTime (2013, 09, 17, 12, 30, 10);
			timeMax = new DateTime (2014, 01, 23, 12, 59, 30);
			axis.WorldMin = (double)timeMin.Ticks;
			axis.WorldMax = (double)timeMax.Ticks;

			tl.Y += 50;		br.Y += 50;
			axis.Draw (ctx, tl, br, out boundingBox);

		}
		#endregion

	} // class TradingDateTimeAxisTests
	
} // namespace Samples
