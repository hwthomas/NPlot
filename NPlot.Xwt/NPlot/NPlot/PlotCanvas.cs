//
// NPlot - A charting library for .NET
// 
// PlotCanvas.cs
// 
// Copyright (C) 2013 Hywel Thomas <hywel.w.thomas@gmail.com>
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
using System.Diagnostics;
using System.Collections;

using Xwt;
using Xwt.Drawing;

namespace NPlot
{
	/// <summary>
	/// Extends PlotSurface2D by implementing a drawing surface (Xwt.Canvas)
	/// to obtain a Drawing Context with which the PlotSurface2D is drawn.
	/// </summary>
	/// <remarks>
	/// The Canvas is exposed so that it may be added to any Xwt Widget
	/// </remarks>
	public class PlotCanvas : PlotSurface2D
	{
		private DrawingSurface surface;	// The Xwt Drawing Surface

		/// <summary>
		/// Default constructor
		/// </summary>
		public PlotCanvas () : base ()
		{
			surface = new DrawingSurface (this);	// Create Drawing surface
		}

		/// <summary>
		/// Exposes the local plotCanvas
		/// </summary>
		public Canvas Canvas
		{
			get { return (Canvas)surface; }
		}

		/// <summary>
		/// Invalidate the entire plot area on the Canvas.
		/// OnDraw then gets called to draw the contents
		/// </summary>
		public void Refresh ()
		{
			surface.QueueDraw (surface.Bounds);
		}

		/// <summary>
		/// Xwt.Drawing Canvas with access to PlotSurface Draw routines 
		/// </summary>
		internal class DrawingSurface : Canvas
		{
			// This extension of Xwt.Canvas only overrides OnDraw, and can only draw 'static' plots.
			// Any user Interactions (with the mouse, keyboard) are handled by InteractivePlotCanvas

			PlotSurface2D plotSurface;	// To allow access to PlotSurface Draw routine

			/// <summary>
			/// Creates a new DrawingSurface and copies a reference to the calling PlotSurface
			/// </summary>
			internal DrawingSurface (PlotSurface2D ps) : base ()
			{
				plotSurface = ps;
			}

			protected override void OnDraw (Context ctx, Rectangle dirtyRect)
			{
				// PlotSurface2D draws itself into the rectangle specified when Draw is called.
				// Always specify the entire area of the DrawingSurface when drawing the plot,
				// since a smaller part of that area cannot (at present, anyway) be redrawn.
 
				plotSurface.Draw (ctx, Bounds);

			}

		}

	} 

} 


