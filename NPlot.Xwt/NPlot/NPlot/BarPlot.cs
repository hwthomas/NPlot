//
// NPlot - A charting library for .NET
// 
// BarPlot.cs
//
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
using Xwt;
using Xwt.Drawing;

namespace NPlot
{
	/// <summary>
	/// Draws a bar chart
	/// </summary>
	public class BarPlot : BasePlot, IPlot, IDrawable
	{
		/// <summary>
		/// Default Constructor
		/// </summary>
		public BarPlot()
		{
		}

		/// <summary>
		/// Gets or sets the data, or column name for the ordinate [y] axis.
		/// </summary>
		public object OrdinateDataTop
		{
			get { return ordinateDataTop_; }
			set { ordinateDataTop_ = value; }
		}
		private object ordinateDataTop_ = null;

		
		/// <summary>
		/// Gets or sets the data, or column name for the ordinate [y] axis.
		/// </summary>
		public object OrdinateDataBottom
		{
			get { return ordinateDataBottom_; }
			set { ordinateDataBottom_ = value; }
		}
		private object ordinateDataBottom_ = null;


		/// <summary>
		/// Gets or sets the data, or column name for the abscissa [x] axis.
		/// </summary>
		public object AbscissaData
		{
			get { return abscissaData_; }
			set { abscissaData_ = value; }
		}
		private object abscissaData_ = null;


		/// <summary>
		/// Draws the line plot using the Drawing Context and x and y axes provided
		/// </summary>
		/// <param name="ctx">The Drawing Context with which to draw.</param>
		/// <param name="xAxis">The X-Axis to draw against.</param>
		/// <param name="yAxis">The Y-Axis to draw against.</param>
		public void Draw (Context ctx, PhysicalAxis xAxis, PhysicalAxis yAxis)
		{
			SequenceAdapter dataTop = new SequenceAdapter (DataSource, DataMember, OrdinateDataTop, AbscissaData);
			SequenceAdapter dataBottom = new SequenceAdapter (DataSource, DataMember, OrdinateDataBottom, AbscissaData);

			ITransform2D t = Transform2D.GetTransformer (xAxis, yAxis);

			ctx.Save ();
			for (int i=0; i<dataTop.Count; ++i) {
				Point physicalBottom = t.Transform (dataBottom[i]);
				Point physicalTop = t.Transform (dataTop[i]);

				if (physicalBottom != physicalTop) {
					Rectangle r = new Rectangle (physicalBottom.X - BarWidth/2, physicalTop.Y,BarWidth, (physicalBottom.Y - physicalTop.Y) );
					ctx.SetColor (fillColor);
					ctx.Rectangle (r);
					ctx.FillPreserve ();
					ctx.SetColor (borderColor);
					ctx.Stroke ();
				}
			}
			ctx.Restore ();
		}

		/// <summary>
		/// Returns an x-axis that is suitable for drawing this plot.
		/// </summary>
		/// <returns>A suitable x-axis.</returns>
		public Axis SuggestXAxis()
		{
			SequenceAdapter dataBottom_ = new SequenceAdapter( DataSource, DataMember, OrdinateDataBottom, AbscissaData );
			SequenceAdapter dataTop_ = new SequenceAdapter( DataSource, DataMember, OrdinateDataTop, AbscissaData );

			Axis axis = dataTop_.SuggestXAxis ();
			axis.LUB(dataBottom_.SuggestXAxis ());
			return axis;
		}


		/// <summary>
		/// Returns a y-axis that is suitable for drawing this plot.
		/// </summary>
		/// <returns>A suitable y-axis.</returns>
		public Axis SuggestYAxis()
		{
			SequenceAdapter dataBottom_ = new SequenceAdapter( DataSource, DataMember, OrdinateDataBottom, AbscissaData );
			SequenceAdapter dataTop_ = new SequenceAdapter( DataSource, DataMember, OrdinateDataTop, AbscissaData );

			Axis axis = dataTop_.SuggestYAxis ();
			axis.LUB(dataBottom_.SuggestYAxis ());
			return axis;
		}


		/// <summary>
		/// Draws a representation of this plot in the legend.
		/// </summary>
		/// <param name="g">The graphics surface on which to draw.</param>
		/// <param name="startEnd">A rectangle specifying the bounds of the area in the legend set aside for drawing.</param>
		public virtual void DrawInLegend (Context ctx, Rectangle startEnd)
		{
			double smallerHeight = (startEnd.Height * 0.5);
			//double heightToRemove = (startEnd.Height * 0.5f);
			ctx.Save ();
			ctx.SetColor (fillColor);
			Rectangle newRectangle = new Rectangle (startEnd.Left, startEnd.Top + smallerHeight/2, startEnd.Width, smallerHeight);
			ctx.Rectangle (newRectangle);
			ctx.FillPreserve ();
			ctx.SetColor (borderColor);
			ctx.Stroke ();
			ctx.Restore ();
		}


		/// <summary>
		/// The border color used for the bars
		/// </summary>
		public Color BorderColor
		{
			get { return borderColor; }
			set { borderColor = value; }
		}
		private Color borderColor = Colors.Black;


		/// <summary>
		/// The Bar color used
		/// </summary>
		public Color FillColor
		{
			get { return fillColor; }
			set { fillColor = value; }
		}
		private Color fillColor = Colors.LightGray;


		/// <summary>
		/// The width of the bar in physical pixels.
		/// </summary>
		public double BarWidth					 
		{
			get { return barWidth; }
			set { barWidth = value; }
		}
		private double barWidth = 8;


		/// <summary>
		/// Write data associated with the plot as text.
		/// </summary>
		/// <param name="sb">the string builder to write to.</param>
		/// <param name="region">Only write out data in this region if onlyInRegion is true.</param>
		/// <param name="onlyInRegion">If true, only data in region is written, else all data is written.</param>
		/// <remarks>TODO: not implemented.</remarks>
		public void WriteData( System.Text.StringBuilder sb, Rectangle region, bool onlyInRegion )
		{
			sb.Append ( "Write data not implemented yet for BarPlot\r\n" );
		}
	}
}
