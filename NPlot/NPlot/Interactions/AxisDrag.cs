//
// NPlot - A charting library for .NET
// 
// AxisDrag.cs
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
using System.Collections;
using System.Drawing;

namespace NPlot
{
	public class AxisDrag : Interaction
	{
		/// <summary>
		/// Dragging the axes increases or decreases the axes scaling factors
		/// with the expansion being about the point where the drag is started
		/// Only the axis in which the mouse is clicked will be modified
		/// </summary>
		private Axis axis_ = null;
		private bool dragging_ = false;
		private PhysicalAxis physicalAxis_ = null;
		private Point lastPoint_  = new Point();
		private Point startPoint_ = new Point();
		private double focusRatio_ = 0.5;
		private double sensitivity_ = 1.0;

		/// <summary>
		/// MouseDown method for AxisDrag interaction
		/// </summary>
		/// <param name="X">mouse X position</param>
		/// <param name="Y">mouse Y position</param>
		/// <param name="keys">mouse and keyboard modifiers</param>
		/// <param name="ps">the InteractivePlotSurface2D</param>
		public override bool DoMouseDown (int X, int Y, Modifier keys, InteractivePlotSurface2D ps)
		{
			// if the mouse is inside the plot area [the tick marks may be here,
			// and are counted as part of the axis], then don't invoke drag. 
			if (ps.PlotAreaBoundingBoxCache.Contains(X,Y)) {
				return false;
			}
			if ((keys & Modifier.Button1) != 0) {
				// see if hit with axis. NB Only one axis object will be returned
				ArrayList objects = ps.HitTest(new Point(X, Y));

				foreach (object o in objects) {
					if (o is NPlot.Axis) {
						dragging_ = true;
						axis_ = (Axis)o;

						if (ps.PhysicalXAxis1Cache.Axis == axis_) {
							physicalAxis_ = ps.PhysicalXAxis1Cache;
							ps.plotCursor = CursorType.LeftRight;
						}
						else if (ps.PhysicalXAxis2Cache.Axis == axis_) {
							physicalAxis_ = ps.PhysicalXAxis2Cache;
							ps.plotCursor = CursorType.LeftRight;
						}
						else if (ps.PhysicalYAxis1Cache.Axis == axis_) {
							physicalAxis_ = ps.PhysicalYAxis1Cache;
							ps.plotCursor = CursorType.UpDown;
						}
						else if (ps.PhysicalYAxis2Cache.Axis == axis_) {
							physicalAxis_ = ps.PhysicalYAxis2Cache;
							ps.plotCursor = CursorType.UpDown;
						}

						startPoint_ = new Point(X, Y);	// don't combine these - Mono
						lastPoint_ = startPoint_;		// bug #475205 prior to 2.4
						// evaluate focusRatio about which axis is expanded
						float  x = startPoint_.X - physicalAxis_.PhysicalMin.X;
						float  y = startPoint_.Y - physicalAxis_.PhysicalMin.Y;
						double r = Math.Sqrt(x*x + y*y);
						focusRatio_ = r/physicalAxis_.PhysicalLength;
	
						return false;
					}
				}
			}
			return false;
		}
			

		/// <summary>
		/// MouseMove method for AxisDrag interaction
		/// </summary>
		/// <param name="X">mouse X position</param>
		/// <param name="Y"> mouse Y position</param>
		/// <param name="keys"> mouse and keyboard modifiers</param>
		/// <param name="ps">the InteractivePlotSurface2D</param>
		public override bool DoMouseMove (int X, int Y, Modifier keys, InteractivePlotSurface2D ps)
		{
			if (((keys & Modifier.Button1) != 0) && dragging_&& physicalAxis_ != null ) {
				ps.CacheAxes();

				float dX = (X - lastPoint_.X);
				float dY = (Y - lastPoint_.Y);
				lastPoint_ = new Point(X, Y);
					
				// In case the physical axis is not horizontal/vertical, combine dX and dY
				// in a way which preserves their sign and intuitive axis zoom sense, ie
				// because the physical origin is top-left, expand with +ve dX, but -ve dY 
				double distance = dX - dY;
				double proportion = distance*sensitivity_ /physicalAxis_.PhysicalLength;
										
				axis_.IncreaseRange(proportion, focusRatio_);
				
				return true;
			}
			return false;
		}


		/// <summary>
		/// MouseUp method for AxisDrag interaction
		/// </summary>
		/// <param name="X">mouse X position</param>
		/// <param name="Y"> mouse Y position</param>
		/// <param name="keys"> mouse and keyboard modifiers</param>
		/// <param name="ps">the InteractivePlotSurface2D</param>
		public override bool DoMouseUp ( int X, int Y, Modifier keys, InteractivePlotSurface2D ps )
		{
			if (dragging_) {
				dragging_ = false;
				axis_ = null;
				physicalAxis_ = null;
				lastPoint_ = new Point();
				ps.plotCursor = CursorType.LeftPointer;
			}
			return false;
		}

	
		/// <summary>
		/// Sensitivity factor for axis scaling
		/// </summary>
		/// <value></value>
		public double Sensitivity
		{
			get { return sensitivity_; }
			set { sensitivity_ = value; }
		}

	} // AxisDrag (Zoom)

}
