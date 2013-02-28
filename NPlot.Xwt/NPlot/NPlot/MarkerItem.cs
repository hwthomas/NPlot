/*
 * NPlot - A charting library for .NET
 * 
 * MarkerItem.cs
 * Copyright (C) 2003-2006 Matt Howlett and others.
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

namespace NPlot
{

	/// <summary>
	/// Class for placement of a single marker.
	/// </summary>
	public class MarkerItem : IDrawable
	{

		private Marker marker_; 
		private double x_;
		private double y_; 


		/// <summary>
		/// Constructs a square marker at the (world) point point.
		/// </summary>
		/// <param name="point">the world position at which to place the marker</param>
		public MarkerItem( PointD point )
		{
			marker_ = new Marker( Marker.MarkerType.Square );
			x_ = point.X;
			y_ = point.Y;
		}


		/// <summary>
		/// Default constructor - a square black marker.
		/// </summary>
		/// <param name="x">The world x position of the marker</param>
		/// <param name="y">The world y position of the marker</param>
		public MarkerItem( double x, double y )
		{
			marker_ = new Marker( Marker.MarkerType.Square );
			x_ = x;
			y_ = y;
		}


		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="marker">The marker to place on the chart.</param>
		/// <param name="x">The world x position of the marker</param>
		/// <param name="y">The world y position of the marker</param>
		public MarkerItem( Marker marker, double x, double y )
		{
			marker_ = marker;
			x_ = x;
			y_ = y;
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="marker">The marker to place on the chart.</param>
		/// <param name="point">The world position of the marker</param>
		public MarkerItem( Marker marker, PointD point )
		{
			marker_ = marker;
			x_ = point.X;
			y_ = point.Y;
		}

		/// <summary>
		/// Draws the marker on a plot surface.
		/// </summary>
		/// <param name="g">graphics surface on which to draw</param>
		/// <param name="xAxis">The X-Axis to draw against.</param>
		/// <param name="yAxis">The Y-Axis to draw against.</param>
		public void Draw( System.Drawing.Graphics g, PhysicalAxis xAxis, PhysicalAxis yAxis )
		{
			PointF point = new PointF( 
				xAxis.WorldToPhysical( x_, true ).X,
				yAxis.WorldToPhysical( y_, true ).Y );

			marker_.Draw( g, (int)point.X, (int)point.Y );
		}
	}
}
