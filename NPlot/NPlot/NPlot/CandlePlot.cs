/*
 * NPlot - A charting library for .NET
 * 
 * CandlePlot.cs
 * Copyright (C) 2003-2006 Matt Howlett and others.
 * All rights reserved.
 * 
 * Redistribution and use in source and binary forms, with or without modification,
 * are permitted provided that the following conditions are met:
 * 
 * 1. Redistributions of source code must retain the above copyright notice, this
 *	  list of conditions and the following disclaimer.
 * 2. Redistributions in binary form must reproduce the above copyright notice,
 *	  this list of conditions and the following disclaimer in the documentation
 *	  and/or other materials provided with the distribution.
 * 3. Neither the name of NPlot nor the names of its contributors may
 *	  be used to endorse or promote products derived from this software without
 *	  specific prior written permission.
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
using System.Collections;
using System.Drawing;
using System.Data;

namespace NPlot
{

	/// <summary>
	/// Encapsulates open, low, high and close values useful for specifying financial data
	/// over a time period, together with a [single] x-value indicating the time [period] the
	/// data corresponds to. 
	/// </summary>
	public class PointOLHC
	{

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="x">value representing the time period that the financial values refer to</param>
		/// <param name="open">The value at open of time period.</param>
		/// <param name="low">The low value over the time period</param>
		/// <param name="high">The high value over the time period.</param>
		/// <param name="close">The value at close of time period.</param>
		public PointOLHC( double x, double open, double low, double high, double close )
		{
			this.x_ = x;
			this.open_ = open;
			this.close_ = close;
			this.low_ = low;
			this.high_ = high;
		}


		/// <summary>
		/// value representing the time period that the financial values apply to.
		/// </summary>
		public double X
		{
			get
			{
				return x_;
			}
			set
			{
				this.x_ = value;
			}
		}
		private double x_;


		/// <summary>
		/// The value at open of time period.
		/// </summary>
		public double Open
		{
			get
			{
				return open_;
			}
			set
			{
				open_ = value;
			}
		}
		private double open_;


		/// <summary>
		/// The value at close of time period.
		/// </summary>
		public double Close
		{
			get
			{
				return close_;
			}
			set
			{
				close_ = value;
			}
		}
		private double close_;


		/// <summary>
		/// Low value of the time period.
		/// </summary>
		public double Low
		{
			get
			{
				return low_;
			}
			set
			{
				low_ = value;
			}
		}
		private double low_;


		/// <summary>
		/// High value of the time period.
		/// </summary>
		public double High
		{
			get
			{
				return high_;
			}
			set
			{
				high_ = value;
			}
		}
		private double high_;
	}


	/// <summary>
	/// Encapsulates functionality for drawing finacial candle charts. 
	/// </summary>
	public class CandlePlot : BasePlot, IPlot
	{

		/// <summary>
		/// 
		/// </summary>
		public abstract class CandleStyle
		{
			/// <summary>
			/// 
			/// </summary>
			/// <param name="d"></param>
			/// <returns></returns>
			public abstract CandleStyle Create(CandleDataAdapter d);
		}

		/// <summary>
		/// 
		/// </summary>
		public class Stick : CandleStyle
		{
			private Stick() { }

			/// <summary>
			/// 
			/// </summary>
			/// <param name="d"></param>
			/// <returns></returns>
			public override CandleStyle Create(CandleDataAdapter d)
			{
				return new Stick();
			}
		}
		

		/// <summary>
		/// This class is responsible for interpreting the various ways you can 
		/// specify data to CandlePlot objects
		/// </summary>
		public class CandleDataAdapter
		{
			private object openData_;
			private object lowData_;
			private object highData_;
			private object closeData_;
			private object abscissaData_;

			private object dataSource_;
			private string dataMember_;
			DataRowCollection rows_ = null;

			// speed optimizations if data is double.
			private double[] openDataArray_;
			private double[] lowDataArray_;
			private double[] highDataArray_;
			private double[] closeDataArray_;
			private double[] abscissaDataArray_;
			private bool useDoublesArrays_;
			

			/// <summary>
			/// Constructor
			/// </summary>
			/// <param name="dataSource"></param>
			/// <param name="dataMember"></param>
			/// <param name="abscissaData"></param>
			/// <param name="openData"></param>
			/// <param name="lowData"></param>
			/// <param name="highData"></param>
			/// <param name="closeData"></param>
			public CandleDataAdapter( 
				object dataSource, string dataMember, object abscissaData,
				object openData, object lowData, object highData, object closeData )
			{
				this.openData_ = openData;
				this.lowData_ = lowData;
				this.highData_ = highData;
				this.closeData_ = closeData;
				this.abscissaData_ = abscissaData;

				this.dataSource_ = dataSource;
				this.dataMember_ = dataMember;

				if (dataSource_ != null)
				{
					if ( dataSource_ is DataSet )
					{
						if (dataMember_ != null)
						{
							rows_ = ((DataTable)((DataSet)dataSource_).Tables[dataMember_]).Rows;
						}
						else
						{
							rows_ = ((DataTable)((DataSet)dataSource_).Tables[0]).Rows;
						}
					}
					else if (dataSource_ is DataTable )
					{
						rows_ = ((DataTable)dataSource_).Rows;
					}
					else
					{
						throw new NPlotException ( "not implemented yet" );
					}
				}

				openDataArray_ = openData_ as System.Double[];
				lowDataArray_ = lowData_ as System.Double[];
				highDataArray_ = highData_ as System.Double[];
				closeDataArray_ = closeData_ as System.Double[];
				abscissaDataArray_ = abscissaData_ as System.Double[];
				useDoublesArrays_ = (
					openDataArray_ != null &&
					lowDataArray_ != null &&
					highDataArray_ != null &&
					lowDataArray_ != null &&
					abscissaDataArray_ != null
				);

			}


			/// <summary>
			/// Gets the ith point in the candle adapter
			/// </summary>
			/// <param name="i">index of datapoint to get</param>
			/// <returns>the datapoint.</returns>
			public PointOLHC this[int i]
			{
				get
				{
					// try a fast track first
					if (useDoublesArrays_)
					{
						return new PointOLHC(
							abscissaDataArray_[i],
							openDataArray_[i],
							lowDataArray_[i],
							highDataArray_[i],
							closeDataArray_[i]);
					}
					// is the data coming from a data source?
					else if (rows_ != null)
					{
						double x = Utils.ToDouble(((DataRow)(rows_[i]))[(string)abscissaData_]);
						double open = Utils.ToDouble(((DataRow)(rows_[i]))[(string)openData_]);
						double low = Utils.ToDouble(((DataRow)(rows_[i]))[(string)lowData_]);
						double high = Utils.ToDouble(((DataRow)(rows_[i]))[(string)highData_]);
						double close = Utils.ToDouble(((DataRow)(rows_[i]))[(string)closeData_]);

						return new PointOLHC(x, open, low, high, close);
					}
					// the data is coming from individual ILists.
					else if (abscissaData_ is IList && openData_ is IList && lowData_ is IList && highData_ is IList && closeData_ is IList)
					{
						double x = Utils.ToDouble(((IList)abscissaData_)[i]);
						double open = Utils.ToDouble(((IList)openData_)[i]);
						double low = Utils.ToDouble(((IList)lowData_)[i]);
						double high = Utils.ToDouble(((IList)highData_)[i]);
						double close = Utils.ToDouble(((IList)closeData_)[i]);

						return new PointOLHC(x, open, low, high, close);
					}
					else
					{
						throw new NPlotException("not implemented yet");
					}
				}
			}


			/// <summary>
			/// The number of datapoints available via the candle adapter.
			/// </summary>
			/// <value>the number of datapoints available.</value>
			public int Count
			{
				get
				{
					// this is inefficient [could set up delegates in constructor].
					if (useDoublesArrays_)
					{
						return openDataArray_.Length;
					}

					if (openData_ == null)
					{
						return 0;
					}

					if (rows_ != null)
					{
						return rows_.Count;
					}

					if (openData_ is IList)
					{
						int size = ((IList)openData_).Count;
						if (size != ((IList)closeData_).Count)
						{
							throw new NPlotException("open and close arrays are not of same length");
						}
						if (size != ((IList)lowData_).Count)
						{
							throw new NPlotException("open and low arrays are not of same length");
						}
						if (size != ((IList)highData_).Count)
						{
							throw new NPlotException("open and high arrays are not of same length");
						}
						return size;
					}

					throw new NPlotException( "data not in correct format" );
				}
			}


			/// <summary>
			/// Returns an x-axis that is suitable for drawing the data.
			/// </summary>
			/// <returns>A suitable x-axis.</returns>
			public Axis SuggestXAxis()
			{
				double min;
				double max;
				double minStep = 0.0;

				if (this.rows_ == null)
				{
					Utils.ArrayMinMax((System.Collections.IList)this.abscissaData_, out min, out max);

					if (((System.Collections.IList)abscissaData_).Count > 1)
					{
						double first = Utils.ToDouble(((IList)abscissaData_)[0]);
						double second = Utils.ToDouble(((IList)abscissaData_)[1]);
						minStep = Math.Abs(second - first);
					}

					if (((System.Collections.IList)abscissaData_).Count > 2)
					{
						double first = Utils.ToDouble(((IList)abscissaData_)[0]);
						double second = Utils.ToDouble(((IList)abscissaData_)[1]);
						if (Math.Abs(second - first) < minStep)
							minStep = Math.Abs(second - first);
					}

					if (((System.Collections.IList)abscissaData_)[0] is DateTime)
					{
						return new DateTimeAxis(min - minStep / 2.0, max + minStep / 2.0);
					}
					else
					{
						return new LinearAxis(min - minStep / 2.0, max + minStep / 2.0);
					}
				}
				else
				{
					Utils.RowArrayMinMax(this.rows_, out min, out max, (string)this.abscissaData_);

					if (rows_.Count > 1)
					{
						double first = Utils.ToDouble(rows_[0][(string)abscissaData_]);
						double second = Utils.ToDouble(rows_[1][(string)abscissaData_]);
						minStep = Math.Abs(second - first);
					}

					if (rows_.Count > 2)
					{
						double first = Utils.ToDouble(rows_[1][(string)abscissaData_]);
						double second = Utils.ToDouble(rows_[2][(string)abscissaData_]);
						if (Math.Abs(second - first) < minStep)
							minStep = Math.Abs(second - first);
					}

					if ((rows_[0])[(string)abscissaData_] is DateTime)
					{
						return new DateTimeAxis(min - minStep / 2.0, max + minStep / 2.0);
					}
					else
					{
						return new LinearAxis(min - minStep / 2.0, max + minStep / 2.0);
					}
				}
			}


			/// <summary>
			/// Returns a y-axis that is suitable for drawing the data.
			/// </summary>
			/// <returns>A suitable y-axis.</returns>
			public Axis SuggestYAxis()
			{
				double min_l;
				double max_l;
				double min_h;
				double max_h;

				if (this.rows_ == null)
				{
					Utils.ArrayMinMax((System.Collections.IList)lowData_, out min_l, out max_l);
					Utils.ArrayMinMax((System.Collections.IList)highData_, out min_h, out max_h);
				}
				else
				{
					Utils.RowArrayMinMax(this.rows_, out min_l, out max_l, (string)this.lowData_);
					Utils.RowArrayMinMax(this.rows_, out min_h, out max_h, (string)this.highData_);
				}

				Axis a = new LinearAxis( min_l, max_h );
				a.IncreaseRange( 0.08 );
				return a;
			}
		}

		
		/// <summary>
		/// Default constructor.
		/// </summary>
		public CandlePlot()
		{
		}


		/// <summary>
		/// Calculates the physical (not world) separation between abscissa values.
		/// </summary>
		/// <param name="cd">Candle adapter containing data</param>
		/// <param name="xAxis">Physical x axis the data is plotted against.</param>
		/// <returns>physical separation between abscissa values.</returns>
		private static float CalculatePhysicalSeparation( CandleDataAdapter cd, PhysicalAxis xAxis )
		{
			if (cd.Count > 1)
			{
				int xPos1 = (int)(xAxis.WorldToPhysical( ((PointOLHC)cd[0]).X, false )).X;
				int xPos2 = (int)(xAxis.WorldToPhysical( ((PointOLHC)cd[1]).X, false )).X;		
				int minDist = xPos2 - xPos1;

				if (cd.Count > 2)
				{  // to be pretty sure we get the smallest gap.
					int xPos3 = (int)(xAxis.WorldToPhysical(((PointOLHC)cd[2]).X, false)).X;
					if (xPos3 - xPos2 < minDist)
					{
						minDist = xPos3 - xPos2;
					}

					if (cd.Count > 3)
					{
						int xPos4 = (int)(xAxis.WorldToPhysical(((PointOLHC)cd[3]).X, false)).X;
						if (xPos4 - xPos3 < minDist)
						{
							minDist = xPos4 - xPos3;
						}
					}
				}

				return minDist;
			}

			return 0.0f;
		}


		/// <summary>
		/// Draws the candle plot on a GDI+ surface agains the provided x and y axes.
		/// </summary>
		/// <param name="g">The GDI+ surface on which to draw.</param>
		/// <param name="xAxis">The X-Axis to draw against.</param>
		/// <param name="yAxis">The Y-Axis to draw against.</param>
		public void Draw( Graphics g, PhysicalAxis xAxis, PhysicalAxis yAxis )
		{
			CandleDataAdapter cd = new CandleDataAdapter( this.DataSource, this.DataMember, 
				this.AbscissaData, this.OpenData, this.LowData, this.HighData, this.CloseData );

			Brush bearishBrush = new SolidBrush( BearishColor );
			Brush bullishBrush = new SolidBrush( BullishColor );

			uint offset = 0;
			if (this.centered_)
			{
				offset = (uint)(CalculatePhysicalSeparation(cd,xAxis) / 2.0f);
			}

			uint addAmount = (uint)StickWidth/2;
			uint stickWidth = (uint)StickWidth;

			if (StickWidth == AutoScaleStickWidth)
			{
				// default
				addAmount = 2;
				stickWidth = 4;
			
				float minDist = CalculatePhysicalSeparation( cd, xAxis );

				addAmount = (uint)(minDist / 3);
				stickWidth = addAmount * 2;
			}

			Pen	p =	new	Pen(this.color_);

			/*
			// brant hyatt proposed.
			if (this.Style == Styles.Stick)
			{
				p.Width = stickWidth;
				addAmount = stickWidth + 2;
			}
			*/

			for (int i=0; i<cd.Count; ++i)
			{

				PointOLHC point = (PointOLHC)cd[i];
				if ( (!double.IsNaN (point.Open)) && (!double.IsNaN(point.High)) && (!double.IsNaN (point.Low)) && (!double.IsNaN(point.Close)) )
				{
					int xPos = (int)(xAxis.WorldToPhysical( point.X, false )).X;

					if (xPos + offset + addAmount < xAxis.PhysicalMin.X || xAxis.PhysicalMax.X < xPos + offset - addAmount)
					{
						continue;
					}

					int yPos1 = (int)(yAxis.WorldToPhysical( point.Low, false )).Y;
					int yPos2 = (int)(yAxis.WorldToPhysical( point.High, false )).Y;
					int yPos3 = (int)(yAxis.WorldToPhysical( point.Open, false )).Y;
					int yPos4 = (int)(yAxis.WorldToPhysical( point.Close, false )).Y;

					if (this.Style == Styles.Stick)
					{
						/*
						// brant hyatt proposed.
						if (i > 0) 
						{
							if ( ((PointOLHC)cd[i]).Close > ((PointOLHC)cd[i-1]).Close)
							{
								p.Color = BullishColor;
							}
							else
							{
								p.Color = BearishColor;
							}
						}
						*/

						g.DrawLine( p, xPos+offset, yPos1, xPos+offset, yPos2 );
						g.DrawLine( p, xPos-addAmount+offset, yPos3, xPos+offset, yPos3 );
						g.DrawLine( p, xPos+offset, yPos4, xPos+addAmount+offset, yPos4 );
					}
					else if (this.Style == Styles.Filled)
					{
						g.DrawLine( p, xPos+offset, yPos1, xPos+offset, yPos2 );
						if (yPos3 > yPos4)
						{
							g.FillRectangle( bullishBrush, xPos-addAmount+offset, yPos4, stickWidth, yPos3 - yPos4 );
							g.DrawRectangle( p, xPos-addAmount+offset, yPos4, stickWidth, yPos3 - yPos4 );
						}
						else if (yPos3 < yPos4)
						{
							g.FillRectangle( bearishBrush, xPos-addAmount+offset, yPos3, stickWidth, yPos4 - yPos3 );
							g.DrawRectangle( p, xPos-addAmount+offset, yPos3, stickWidth, yPos4 - yPos3 );
						}
						else
						{
							g.DrawLine( p, xPos-addAmount+offset, yPos3, xPos-addAmount+stickWidth+offset, yPos3 );
						}
					}
				}
			}
		}


		/// <summary>
		/// Returns an x-axis that is suitable for drawing this plot.
		/// </summary>
		/// <returns>A suitable x-axis.</returns>
		public Axis SuggestXAxis()
		{
			CandleDataAdapter candleData = new CandleDataAdapter( this.DataSource, this.DataMember, 
				this.AbscissaData, this.OpenData, this.LowData, this.HighData, this.CloseData );
		
			return candleData.SuggestXAxis();
		}


		/// <summary>
		/// Returns a y-axis that is suitable for drawing this plot.
		/// </summary>
		/// <returns>A suitable y-axis.</returns>
		public Axis SuggestYAxis()
		{
			CandleDataAdapter candleData = new CandleDataAdapter( this.DataSource, this.DataMember, 
				this.AbscissaData, this.OpenData, this.LowData, this.HighData, this.CloseData );

			return candleData.SuggestYAxis();
		}


		/// <summary>
		/// Gets or sets the data, or column name for the open values.
		/// </summary>
		public object OpenData
		{
			get
			{
				return openData_;
			}
			set
			{
				openData_ = value;
			}
		}
		private object openData_ = null;


		/// <summary>
		/// Gets or sets the data, or column name for the interval low values.
		/// </summary>
		public object LowData
		{
			get
			{
				return lowData_;
			}
			set
			{
				lowData_ = value;
			}
		}
		private object lowData_ = null;


		/// <summary>
		/// Gets or sets the data, or column name for the interval high values.
		/// </summary>
		public object HighData
		{
			get
			{
				return highData_;
			}
			set
			{
				highData_ = value;
			}
		}
		private object highData_ = null;


		/// <summary>
		/// Gets or sets the data, or column name for the close values.
		/// </summary>
		public object CloseData
		{
			get
			{
				return closeData_;
			}
			set
			{
				closeData_ = value;
			}
		}	
		private object closeData_ = null;


		/// <summary>
		/// Gets or sets the data, or column name for the abscissa [x] axis.
		/// </summary>
		public object AbscissaData
		{
			get
			{
				return abscissaData_;
			}
			set
			{
				abscissaData_ = value;
			}
		}
		private object abscissaData_ = null;


		/// <summary>
		/// Draws a representation of this plot in the legend.
		/// </summary>
		/// <param name="g">The graphics surface on which to draw.</param>
		/// <param name="startEnd">A rectangle specifying the bounds of the area in the legend set aside for drawing.</param>
		public virtual void DrawInLegend( Graphics g, Rectangle startEnd )
		{
			Pen	p =	new	Pen(this.color_);

			g.DrawLine( p, startEnd.Left, (startEnd.Top + startEnd.Bottom)/2, 
				startEnd.Right, (startEnd.Top + startEnd.Bottom)/2 );
		}


		/// <summary>
		/// Color of this plot [excluding interior of filled boxes if Style is fill]. To
		/// change the Bullish and Bearish colours in Filled mode, use the BullishColor
		/// and BearishColor properties.
		/// </summary>
		public System.Drawing.Color Color
		{
			get
			{
				return color_;
			}
			set
			{
				color_ = value;
			}
		}
		Color color_ = Color.Black;
		

		/// <summary>
		/// Possible CandleStick styles.
		/// </summary>
		public enum Styles
		{
			/// <summary>
			/// Draw vertical line between low and high, tick on left for open and tick on right for close.
			/// </summary>
			Stick,

			/// <summary>
			/// Draw vertical line between low and high and place on top of this a box with bottom
			/// and top determined by open and high values. The box is filled using the colors specified
			/// in BullishColor and BearishColor properties.
			/// </summary>
			Filled
		}


		/// <summary>
		/// Specifies the CandleStick style to use.
		/// </summary>
		public Styles Style = Styles.Filled;


		/// <summary>
		/// If CandlePlot.Style is Filled, then bullish open-close moves are displayed in this color.
		/// </summary>
		public Color BullishColor = Color.White;


		/// <summary>
		/// If CandlePlot.Style is Filled, then bearish moves are displayed in this color.
		/// </summary>
		public Color BearishColor = Color.Black;


		/// <summary>
		/// Width of each stick in pixels. It is best if this is an odd number.
		/// </summary>
		public int StickWidth
		{
			get
			{
				return stickWidth_;
			}
			set
			{
				if (value < 1)
				{
					throw new NPlotException( "Stick width must be greater than 0." );
				}
				stickWidth_ = value;
			}
		}
		private int stickWidth_ = AutoScaleStickWidth;


		/// <summary>
		/// If stick width is set equal to this value, the width will be 
		/// automatically scaled dependant on the space between sticks.
		/// </summary>
		public const int AutoScaleStickWidth = 0;

		/// <summary>
		/// If true (default), bars will be centered on the abscissa times. 
		/// If false, bars will be drawn between the corresponding abscissa time
		/// and the next abscissa time. 
		/// </summary>
		/// <value></value>
		public bool Centered
		{
			get
			{
				return centered_;
			}
			set
			{
				centered_ = value;
			}
		}
		private bool centered_ = true;


		/// <summary>
		/// Write data associated with the plot as text.
		/// </summary>
		/// <param name="sb">the string builder to write to.</param>
		/// <param name="region">Only write out data in this region if onlyInRegion is true.</param>
		/// <param name="onlyInRegion">If true, only data in region is written, else all data is written.</param>
		/// <remarks>TODO: not implemented.</remarks>
		public void WriteData( System.Text.StringBuilder sb, RectangleD region, bool onlyInRegion )
		{
		}
	}
}
