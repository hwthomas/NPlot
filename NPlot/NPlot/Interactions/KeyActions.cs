//
// NPlot - A charting library for .NET
// 
// KeyActions.cs
//
// Copyright (C) Hywel Thomas, Matt Howlett and others 2003-2013
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

namespace NPlot
{

	public class KeyActions : Interaction
	{
		/// <summary>
		/// Links some of the standard keyboard keys to plot scrolling and zooming.
		/// Since all key-interactions are applied to the complete PlotSurface, any
		/// translation or zooming is applied to all axes that have been defined
		/// 
		/// The following key actions are currently implemented :-
		/// Left	- scrolls the viewport to the left
		/// Right	- scrolls the viewport to the right
		/// Up		- scrolls the viewport up
		/// Down	- scrolls the viewport down
		/// +		- zooms in
		/// -		- zooms out
		/// Alt		- reduces the effect of the above actions
		/// Home	- restores original view and dimensions
		/// More could be added, but these are a start.
		/// </summary>
		/// 

		const double right = +0.25, left  = -0.25;
		const double up = +0.25, down = -0.25;
		const double altFactor = 0.4;	// Alt key reduces effect
		const double zoomIn	 = -0.5;	// Should give reversible
		const double zoomOut = +1.0;	// ZoomIn / ZoomOut actions
		const double symmetrical = 0.5;

		private float sensitivity_ = 1.0f;	// default value

		/// <summary>
		/// Sensitivity factor for axis zoom
		/// </summary>
		/// <value></value>
		public float Sensitivity
		{
			get { return sensitivity_; }
			set { sensitivity_ = value; }
		}
	
		/// <summary>
		/// Handler for KeyPress events
		/// </summary>
		/// <param name="key">the NPlot key enumeration</param>
		/// <param name="ps">the InteractivePlotSurface2D</param>
		/// <returns></returns>
		public override bool DoKeyPress(Modifier keys, InteractivePlotSurface2D ps)
		{
			double factor = Sensitivity;
			if (((keys & Modifier.Alt) != 0)) {
				factor *= altFactor;
			}
			if ((keys & Modifier.Home) != 0) {
				ps.SetOriginalDimensions();
				return true;
			}
			if ((keys & Modifier.Left) != 0) {
				ps.CacheAxes();
				ps.TranslateXAxes (left*factor);
				return true;
			}
			if ((keys & Modifier.Right) != 0) {
				ps.CacheAxes();
				ps.TranslateXAxes (right*factor);
				return true;
			}
			if ((keys & Modifier.Up) != 0) {
				ps.CacheAxes();
				ps.TranslateYAxes (up*factor);
				return true;
			}
			if ((keys & Modifier.Down) != 0) {
				ps.CacheAxes();
				ps.TranslateYAxes (down*factor);
				return true;
			}
			if ((keys & Modifier.Plus) != 0) {
				ps.CacheAxes();
				ps.ZoomXAxes (zoomIn*factor,symmetrical);
				ps.ZoomYAxes (zoomIn*factor,symmetrical);
				return true;
			}
			if ((keys & Modifier.Minus) != 0) {
				ps.CacheAxes();
				ps.ZoomXAxes (zoomOut*factor,symmetrical);
				ps.ZoomYAxes (zoomOut*factor,symmetrical);
				return true;
			}
			return false;
		}

		/// <summary>
		/// Handler for KeyRelease events
		/// </summary>
		/// <param name="key">the NPlot key enumeration</param>
		/// <param name="ps">the InteractivePlotSurface2D</param>
		/// <returns></returns>
		public override bool DoKeyRelease(Modifier keys, InteractivePlotSurface2D ps)
		{
			return false;
		}

	} // Key Actions

}