//
// XwPlot - A cross-platform charting library using the Xwt toolkit
// 
// SequenceAdapter.cs
// 
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
using System.Collections;
using System.Data;
using System.Xml;
using Xwt;

namespace NPlot
{

	/// <summary>
	/// This class is responsible for interpreting the various ways you can 
	/// specify data to plot objects using the DataSource, DataMember, ordinateData
	/// and AbscissaData properties. It is a bridge that provides access to this
	/// data via a single interface.
	/// </summary>
	public class SequenceAdapter
	{

		private AdapterUtils.IAxisSuggester XAxisSuggester_;
		private AdapterUtils.IAxisSuggester YAxisSuggester_;
		private AdapterUtils.ICounter counter_;
		private AdapterUtils.IDataGetter xDataGetter_;
		private AdapterUtils.IDataGetter yDataGetter_;

		/// <summary>
		/// Constructor. The data source specifiers must be specified here.
		/// </summary>
		/// <param name="dataSource">The source containing a list of values to plot.</param>
		/// <param name="dataMember">The specific data member in a multimember data source to get data from.</param>
		/// <param name="ordinateData">The source containing a list of values to plot on the ordinate axis, or a the name of the column to use for this data.</param>
		/// <param name="abscissaData">The source containing a list of values to plot on the abscissa axis, or a the name of the column to use for this data.</param>
		public SequenceAdapter (object dataSource, string dataMember, object ordinateData, object abscissaData)
		{
			if (dataSource == null && dataMember == null) {
				if (ordinateData is IList) {
					YAxisSuggester_ = new AdapterUtils.AxisSuggester_IList((IList)ordinateData);
					if (ordinateData is Double[]) {
						yDataGetter_ = new AdapterUtils.DataGetter_DoublesArray ((Double[])ordinateData);
					}
					else {
						yDataGetter_ = new AdapterUtils.DataGetter_IList((IList)ordinateData);
					}

					counter_ = new AdapterUtils.Counter_IList ((IList)ordinateData);

					if (abscissaData is IList) {
						XAxisSuggester_ = new AdapterUtils.AxisSuggester_IList ((IList)abscissaData);
						if (abscissaData is Double[]) {
							xDataGetter_ = new AdapterUtils.DataGetter_DoublesArray ((Double[])abscissaData);
						}
						else {
							xDataGetter_ = new AdapterUtils.DataGetter_IList ((IList)abscissaData);
						}
						return;
					}
					else if (abscissaData is StartStep) {
						XAxisSuggester_ = new AdapterUtils.AxisSuggester_StartStep ((StartStep)abscissaData, (IList)ordinateData);
						xDataGetter_ = new AdapterUtils.DataGetter_StartStep ((StartStep)abscissaData);
						return;
					}
					else if (abscissaData == null) {
						XAxisSuggester_ = new AdapterUtils.AxisSuggester_Auto ((IList)ordinateData);
						xDataGetter_ = new AdapterUtils.DataGetter_Count ();
						return;
					}
				}
				else if (ordinateData == null) {
					if (abscissaData == null) {
						XAxisSuggester_ = new AdapterUtils.AxisSuggester_Null();
						YAxisSuggester_ = new AdapterUtils.AxisSuggester_Null();
						counter_ = new AdapterUtils.Counter_Null();
						xDataGetter_ = new AdapterUtils.DataGetter_Null();
						yDataGetter_ = new AdapterUtils.DataGetter_Null();
						return;
					}
					else if (abscissaData is IList)
					{
						XAxisSuggester_ = new AdapterUtils.AxisSuggester_IList ((IList)abscissaData);
						YAxisSuggester_ = new AdapterUtils.AxisSuggester_Auto ((IList)abscissaData);
						counter_ = new AdapterUtils.Counter_IList ((IList)abscissaData);
						if (abscissaData is Double[])
							xDataGetter_ = new AdapterUtils.DataGetter_DoublesArray ((Double[])abscissaData);
						else
							xDataGetter_ = new AdapterUtils.DataGetter_IList ((IList)abscissaData);

						yDataGetter_ = new AdapterUtils.DataGetter_Count();
						return;
					}
					else {
						// unknown.
					}
				}
				else {
					// unknown
				}
			}
			else if (dataSource is IList && dataMember == null) {
				if (dataSource is DataView) {
					DataView data = (DataView)dataSource;

					counter_ = new AdapterUtils.Counter_DataView (data);
					xDataGetter_ = new AdapterUtils.DataGetter_DataView (data, (string)abscissaData);
					yDataGetter_ = new AdapterUtils.DataGetter_DataView (data, (string)ordinateData);
					XAxisSuggester_ = new AdapterUtils.AxisSuggester_DataView (data, (string)abscissaData);
					YAxisSuggester_ = new AdapterUtils.AxisSuggester_DataView (data, (string)ordinateData);
					return;
				}
				else {
					YAxisSuggester_ = new AdapterUtils.AxisSuggester_IList ((IList)dataSource);
					counter_ = new AdapterUtils.Counter_IList ((IList)dataSource);
					yDataGetter_ = new AdapterUtils.DataGetter_IList ((IList)dataSource);

					if ((ordinateData == null) && (abscissaData == null)) {
						XAxisSuggester_ = new AdapterUtils.AxisSuggester_Auto ((IList)dataSource);
						xDataGetter_ = new AdapterUtils.DataGetter_Count ();
						return;
					}
					else if ((ordinateData == null) && (abscissaData is StartStep)) {
						XAxisSuggester_ = new AdapterUtils.AxisSuggester_StartStep ((StartStep)abscissaData, (IList)ordinateData);
						xDataGetter_ = new AdapterUtils.DataGetter_StartStep ((StartStep)abscissaData);
						return;
					}
					else if ((ordinateData == null) && (abscissaData is IList)) {
						XAxisSuggester_ = new AdapterUtils.AxisSuggester_IList ((IList)abscissaData);
						xDataGetter_ = new AdapterUtils.DataGetter_IList ((IList)abscissaData);
						return;
					}
					else {
						// unknown.
					}
				}
			}
			else if (((dataSource is DataTable) && (dataMember == null)) || (dataSource is DataSet)) {
				DataRowCollection rows = null;

				if (dataSource is DataSet) {
					if (dataMember != null) {
						rows = ((DataTable)((DataSet)dataSource).Tables[dataMember]).Rows;
					}
					else {
						rows = ((DataTable)((DataSet)dataSource).Tables[0]).Rows;
					}
				}
				else {
					rows = ((DataTable)dataSource).Rows;
				}

				yDataGetter_ = new AdapterUtils.DataGetter_Rows (rows, (string)ordinateData);
				YAxisSuggester_ = new AdapterUtils.AxisSuggester_Rows (rows, (string)ordinateData);
				counter_ = new AdapterUtils.Counter_Rows(rows);

				if ((abscissaData is string) && (ordinateData is string)) {
					XAxisSuggester_ = new AdapterUtils.AxisSuggester_Rows (rows, (string)abscissaData);
					xDataGetter_ = new AdapterUtils.DataGetter_Rows (rows, (string)abscissaData);
					return;
				}
				else if ((abscissaData == null) && (ordinateData is string)) {
					XAxisSuggester_ = new AdapterUtils.AxisSuggester_RowAuto (rows);
					xDataGetter_ = new AdapterUtils.DataGetter_Count ();
					return;
				}
				else {
					// unknown.
				}
			}
			else {
				// unknown.
			}

			throw new NPlotException( "Do not know how to interpret data provided to chart." );
		}


		/// <summary>
		/// Returns the number of points.
		/// </summary>
		public int Count
		{
			get { return counter_.Count; }
		}


		/// <summary>
		/// Returns the ith point.
		/// </summary>
		public Point this[int i] 
		{
			get { return new Point (xDataGetter_.Get(i), yDataGetter_.Get(i));
			}
		}


		/// <summary>
		/// Returns an x-axis that is suitable for drawing the data.
		/// </summary>
		/// <returns>A suitable x-axis.</returns>
		public Axis SuggestXAxis()
		{
			Axis a = XAxisSuggester_.Get ();

			// The world length should never be returned as 0
			// This would result in an axis with a span of 0 units
			// which can not be properly displayed.
			if (a.WorldLength == 0.0) {
				// TODO make 0.08 a parameter.
				a.IncreaseRange(0.08);
			}
			return a;
		}


		/// <summary>
		/// Returns a y-axis that is suitable for drawing the data.
		/// </summary>
		/// <returns>A suitable y-axis.</returns>
		public Axis SuggestYAxis()
		{
			Axis a = YAxisSuggester_.Get ();
			// TODO make 0.08 a parameter.
			a.IncreaseRange( 0.08 );
			return a;
		}


		/// <summary>
		/// Writes data out as text. 
		/// </summary>
		/// <param name="sb">StringBuilder to write to.</param>
		/// <param name="region">Only write out data in this region if onlyInRegion is true.</param>
		/// <param name="onlyInRegion">If true, only data in region is written, else all data is written.</param>
		public void WriteData (System.Text.StringBuilder sb, Rectangle region, bool onlyInRegion)
		{
			for (int i=0; i<Count;	++i) {
				if (!(onlyInRegion &&
					   (this[i].X >= region.X && this[i].X <= region.X + region.Width) &&
					   (this[i].Y >= region.Y && this[i].Y <= region.Y + region.Height)))
				{
					continue;
				}

				sb.Append ( this[i].ToString());
				sb.Append ( "\r\n" );
			}
		}
	}
}
