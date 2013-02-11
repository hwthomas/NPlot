/*
 * NPlot - A charting library for .NET
 * 
 * Gtk.AxisTestsForm.cs
 * Copyright (C) 2003-2009 Matt Howlett and others.
 * All rights reserved.
 * 
 * Redistribution and use in source and binary forms, with or without modification,
 * are permitted provided that the following conditions are met:
 * 
 * 1. Redistributions of source code must retain the above copyright notice, this
 *    list of conditions and the following disclaimer.
 * 2. Redistributions in binary form must reproduce the above copyright notice,
 *    this list of conditions and the following disclaimer in the documentation
 *    and/or other materials provided with the distribution.
 * 3. Neither the name of NPlot nor the names of its contributors may
 *    be used to endorse or promote products derived from this software without
 *    specific prior written permission.
 * 
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
 * ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
 * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
 * IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT,
 * INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING,
 * BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
 * DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
 * LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE
 * OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED
 * OF THE POSSIBILITY OF SUCH DAMAGE.
 */

using System;
using System.Drawing;

using NPlot;
using Gtk;
using GtkDotNet = Gtk.DotNet;

namespace NPlotDemo
{
	//
	// Create a Window and Drawing Area into which
	// the Axis Tests can be drawn.
	//
	public class AxisTestsForm : Gtk.Window
	{
		private DrawingArea da_;
		
		System.Drawing.Bitmap bitmap_cache; // The drawing area, and...
		Gdk.Rectangle current_allocation;	// its current allocation. 
		bool allocated = false;

		public AxisTestsForm() : base ( "NPlot Axis Tests" )
		{
			InitializeComponent();	// Setup Axis Tests Window
		}
		
		#region Window Setup code
		/// <summary>
		/// Axis Tests Form setup
		/// </summary>
		private void InitializeComponent()
		{
			SetSizeRequest( 480, 320 );		// Define Window size
			da_ = new DrawingArea();
			da_.ExposeEvent += new ExposeEventHandler(da_ExposeEvent);
			da_.SizeAllocated += new SizeAllocatedHandler (da_SizeAllocated);
	
			Gtk.Frame axisFrame = new Frame();
			axisFrame.ShadowType = Gtk.ShadowType.In;
			axisFrame.Add (da_);
			this.Add(axisFrame);
			
		}
		
		#endregion
		#region Axis Tests
		//
		// The actual Axis tests
		//
		private void DrawAxisTests(Graphics g, Rectangle bounds )
		{
			NPlot.LinearAxis a = new LinearAxis(0, 10);
			Rectangle boundingBox;
			
			a.Draw( g, new Point(30,10), new Point(30, 200), out boundingBox );

			a.Reversed = true;
			a.Draw( g, new Point(60,10), new Point(60, 200), out boundingBox );

			a.SmallTickSize = 0;
			a.Draw( g, new Point(90,10), new Point(90, 200), out boundingBox );

			a.LargeTickStep = 2.5;
			a.Draw( g, new Point(120,10), new Point(120,200), out boundingBox );

			a.NumberOfSmallTicks = 5;
			a.SmallTickSize = 2;
			a.Draw( g, new Point(150,10), new Point(150,200), out boundingBox );

			a.AxisColor = Color.Cyan;
			a.Draw( g, new Point(180,10), new Point(180,200), out boundingBox );

			a.TickTextColor= Color.Cyan;
			a.Draw( g, new Point(210,10), new Point(210,200), out boundingBox );

			a.TickTextBrush = Brushes.Black;
			a.AxisPen = Pens.Black;
			a.Draw( g, new Point(240,10), new Point(300,200), out boundingBox );

			a.WorldMax = 100000;
			a.WorldMin = -3;
			a.LargeTickStep = double.NaN;
			a.Draw( g, new Point(330,10), new Point(330,200), out boundingBox );

			a.NumberFormat = "{0:0.0E+0}";
			a.Draw( g, new Point(380,10), new Point(380,200), out boundingBox );
			
			// Test for default TicksAngle on positive X-axis, ie Ticks below X-axis
			NPlot.LinearAxis aX = new LinearAxis(0, 10);
			aX.Draw( g, new Point(30,240), new Point(380, 240), out boundingBox );
			
			// Set TicksAngle to PI/4 anti-clockwise from positive X-axis direction
			aX.TicksAngle = (float)Math.PI / 4.0f;
			aX.Draw( g, new Point(30,280), new Point(380, 280), out boundingBox );
			
		}
		#endregion
		
		/// <summary>
		/// Obtain a Graphics and draw all Axis Tests
		/// </summary>
		void UpdateCache ()
		{
			if (!allocated)
				return;
			
			if (bitmap_cache != null)
				bitmap_cache.Dispose ();
			
			bitmap_cache = new System.Drawing.Bitmap (current_allocation.Width, current_allocation.Height);
			using (Graphics g = Graphics.FromImage (bitmap_cache)){
				Rectangle bounds = new Rectangle (
					0, 0, current_allocation.Width, current_allocation.Height);
				DrawAxisTests (g, bounds);	
			}
		}


		/// <summary>
		/// Handler for the DrawingArea SizeAllocated Event
		/// </summary>
		private void da_SizeAllocated(object o, SizeAllocatedArgs args)
		{
			allocated = true;
			current_allocation = args.Allocation;
			UpdateCache ();
			args.RetVal = true;
		}
		
		/// <summary>
		/// Handles ExposeEvents by obtaining a GC from the Gdk.Window
		/// and copying the PlotSurface from the cache to the screen
		/// </summary>
		private void da_ExposeEvent(object o, ExposeEventArgs args)
		{
			Gdk.Rectangle area = args.Event.Area;
			using (Graphics g = Gtk.DotNet.Graphics.FromDrawable (args.Event.Window)){
				Rectangle bounds = new Rectangle (area.X, area.Y, area.Width, area.Height);
				g.DrawImage (bitmap_cache, bounds, bounds, GraphicsUnit.Pixel);
			}
			args.RetVal = true;
		}


	} // class AxisTestForm
	
} // namespace NPlotDemo
