//
// NPlot - A charting library for .NET
// 
// Measure.cs
// Contributed by Dan Parnham
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
using System.Drawing;
using System.Drawing.Drawing2D;


namespace NPlot
{
	/// <summary>
	/// Contains the Start and End points being measured in world coordinates
	/// based on Axis1. Also provides helper properties for calculating the
	/// measurement line length and angle.
	/// </summary>
	public class MeasurementArgs
	{
		public PointF Start { get; set; }
		public PointF End	{ get; set; }
		
		public double Distance 
		{ 
			get 
			{
				double dx = End.X - Start.X;
				double dy = End.Y - Start.Y;
				
				return Math.Sqrt(dx * dx + dy * dy);
			}
		}
		
		public double Angle 
		{ 
			get 
			{ 
				return 180 * Math.Atan2(End.Y - Start.Y, End.X - Start.X) / Math.PI; 
			}
		}
	}
	

	/// <summary>
	/// An interaction module for measuring points in the plot. Simply click,
	/// drag and the measurement coordinates will be emitted via an event.
	/// </summary>
	public class Measure : Interaction
	{
		// Sender is the InteractivePlotSurface2D

		/// <summary>
		/// The Measurement event signature.
		/// </summary>
		/// <param name="sender">The instance of InteractivePlotSurface2D that this interation is measuring.</param>
		/// <param name="args">
		/// Structure containing the start and end points in world coordinates. The start and end points will 
		/// be the same and simply represent the cursor position when the plot is not being clicked on.
		/// </param>
		public delegate void MeasurementHandler(object sender, MeasurementArgs args);

		/// <summary>
		/// Event raised whenever the mouse moves over/interacts with the plot.
		/// </summary>
		public event MeasurementHandler Measurement;
		

		Rectangle extent 	= Rectangle.Empty;
		bool active 		= false;
		Color colour;
		Point start;
		Point end;
		int offset;
		

		/// <summary>
		/// Initializes a new instance of the <see cref="NPlot.Measure"/> class.
		/// </summary>
		/// <param name="colour">
		/// Colour used to visualise the measurement points and the line connecting them.
		/// </param>
		public Measure(Color colour)
		{
			this.colour		= colour;
			this.MarkerSize	= 7;
		}


		/// <summary>
		/// Gets or sets the size of the marker used to visualise the start and end
		/// measurement points. Since these are displayed as a cross this number
		/// will always read back as odd.
		/// </summary>
		public int MarkerSize 
		{ 	
			get { return this.offset * 2 + 1; }
			set { this.offset = value > 1 ? value / 2 : 1; }
		}
		
		
		public override void DoDraw(Graphics g, Rectangle dirtyRect)
		{
			if (active)
			{
				using (Pen pen = new Pen(this.colour))
				{
					// Set the smoothing mode here since it is not carried over
					// from the InteractivePlotSurface2D and otherwise the line
					// connecting start and end points will not look very nice.
					var smooth		= g.SmoothingMode;
					g.SmoothingMode = SmoothingMode.AntiAlias;
					
					this.DrawCross(g, pen, this.start);
					this.DrawCross(g, pen, this.end);
					g.DrawLine(pen, this.start, this.end);
					
					g.SmoothingMode = smooth;
				}
			}
		}
		
		
		void DrawCross(Graphics g, Pen pen, Point p)
		{
			g.DrawLine(pen, p.X - this.offset, p.Y, p.X + this.offset, p.Y);
			g.DrawLine(pen, p.X, p.Y - this.offset, p.X, p.Y + this.offset);
		}
		
		
		public override bool DoMouseMove (int x, int y, Modifier keys, InteractivePlotSurface2D ps)
		{
			Rectangle previous = this.extent;

			// If the mouse is being dragged then set the end point and extent
			// rectangle accordingly. Otherwise simply set the start coordinates
			// the current cursor position.
			if (this.active)
			{
				this.end	= new Point(x, y);
				this.extent	= new Rectangle(
					Math.Min(this.start.X, this.end.X) - this.offset, 
					Math.Min(this.start.Y, this.end.Y) - this.offset, 
					Math.Abs(this.end.X - this.start.X) + this.offset * 2 + 1, 
					Math.Abs(this.end.Y - this.start.Y) + this.offset * 2 + 1
				);
			}
			else 
			{
				this.start	= new Point(x, y);
				this.extent = Rectangle.Empty;
			}

			// If an event handler has been set then throw the latest world
			// coordinates at it. Start and end points are set the same if
			// the cursor is not being dragged.
			if (this.Measurement != null)
			{
				if (ps.PhysicalXAxis1Cache != null && ps.PhysicalYAxis1Cache != null)
				{
					var args	= new MeasurementArgs();
					args.Start	= new PointF(
						(float)ps.PhysicalXAxis1Cache.PhysicalToWorld(this.start, true),
						(float)ps.PhysicalYAxis1Cache.PhysicalToWorld(this.start, true)
					);
					
					args.End = !this.active ? args.Start : new PointF(
						(float)ps.PhysicalXAxis1Cache.PhysicalToWorld(this.end, true),
						(float)ps.PhysicalYAxis1Cache.PhysicalToWorld(this.end, true)
					);
					
					this.Measurement(ps, args);
				}
			}
			
			ps.QueueDraw(previous);
			ps.QueueDraw(this.extent);
			
			return false;
		}
		
		
		public override bool DoMouseDown(int x, int y, Modifier keys, InteractivePlotSurface2D ps)
		{
			Rectangle area = ps.PlotAreaBoundingBoxCache;

			// Left mouse button triggers a drag.
			if ((keys & Modifier.Button1) > 0)
			{
				if (area.Contains(x, y))
				{
					this.active = true;
					this.start	= new Point(x, y);
					this.end	= this.start;
					this.extent = new Rectangle(x - this.offset, y - this.offset, this.offset * 2 + 1, this.offset * 2);
				}
			}
			
			ps.QueueDraw(this.extent);
			
			return false;
		}
		
		
		public override bool DoMouseUp(int x, int y, Modifier keys, InteractivePlotSurface2D ps)
		{
			ps.QueueDraw(this.extent);
			
			this.active = false;
			this.extent = Rectangle.Empty;
			
			if (this.Measurement != null)
			{
				if (ps.PhysicalXAxis1Cache != null && ps.PhysicalYAxis1Cache != null)
				{
					var args = new MeasurementArgs();
					
					args.End = args.Start = new PointF(
						(float)ps.PhysicalXAxis1Cache.PhysicalToWorld(this.start, true),
						(float)ps.PhysicalYAxis1Cache.PhysicalToWorld(this.start, true)
					);
					
					this.Measurement(ps, args);
				}
			}
			
			return false;
		}
	}
}
