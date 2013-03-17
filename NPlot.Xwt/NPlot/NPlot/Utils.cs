//
// NPlot - A charting library for .NET
// 
// Utils.cs
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
using System.Data;
using System.Collections;

using Xwt;
using Xwt.Drawing;

namespace NPlot.Xwt
{

	/// <summary>
	/// General purpose utility functions used internally.
	/// </summary>
	internal class Utils
	{

		/// <summary>
		/// Numbers less than this are considered insignificant. This number is
		/// bigger than double.Epsilon.
		/// </summary>
		public const double Epsilon = double.Epsilon * 1000.0;

		/// <summary>
		/// Returns true if the absolute difference between parameters is less than Epsilon
		/// </summary>
		/// <param name="a">first number to compare</param>
		/// <param name="b">second number to compare</param>
		/// <returns>true if equal, false otherwise</returns>
		public static bool DoubleEqual (double a, double b)
		{
			if (System.Math.Abs (a-b) < Epsilon) {
				return true;
			}
			return false;
		}

		/// <summary>
		/// Swaps the value of two doubles.
		/// </summary>
		/// <param name="a">first value to swap.</param>
		/// <param name="b">second value to swap.</param>
		public static void Swap (ref double a, ref double b)
		{
			double c = a;
			a = b;
			b = c;
		}

		/// <summary>
		/// Calculate the distance between two points, a and b.
		/// </summary>
		/// <param name="a">First point</param>
		/// <param name="b">Second point</param>
		/// <returns>Distance between points a and b</returns>
		public static double Distance (Point a, Point b)
		{ 
			return System.Math.Sqrt ((a.X - b.X)*(a.X - b.X) + (a.Y - b.Y)*(a.Y - b.Y));
		}

		/// <summary>
		/// Converts an object of type DateTime or IConvertible to double representation. 
		/// Mapping is 1:1. Note: the System.Convert.ToDouble method can not convert a boxed 
		/// DateTime to double. This implementation can - but the "is" check probably makes
		/// it much slower.
		/// </summary>
		/// <remarks>Compare speed with System.Convert.ToDouble and revise code that calls this if significant speed difference.</remarks>
		/// <param name="o">The object to convert to double.</param>
		/// <returns>double value associated with the object.</returns>
		public static double ToDouble (object o)
		{
			if (o is DateTime) {
				return (double)(((DateTime)o).Ticks);
			}
			else if (o is IConvertible) {
				return System.Convert.ToDouble(o);
			}
			throw new NPlotException ("Invalid datatype");
		}

		/// <summary>
		/// Returns the minimum and maximum values in an IList. The members of the list
		/// can be of different types - any type for which the function Utils.ConvertToDouble
		/// knows how to convert into a double.
		/// </summary>
		/// <param name="a">The IList to search.</param>
		/// <param name="min">The minimum value.</param>
		/// <param name="max">The maximum value.</param>
		/// <returns>true if min max set, false otherwise (a == null or zero length).</returns>
		public static bool ArrayMinMax( IList a, out double min, out double max )
		{
			if (a == null || a.Count == 0) {
				min = 0.0;
				max = 0.0;
				return false;
			}

			min = Utils.ToDouble (a[0]);
			max = Utils.ToDouble (a[0]);
			
			foreach ( object o in a ) {

				double e = Utils.ToDouble(o);

				if ((min.Equals (double.NaN)) && (!e.Equals (double.NaN))) {
					// if min/max are double.NaN and the current value not, then
					// set them to the current value.
					min = e;
					max = e;
				}
				if (!double.IsNaN(e)) {
					if (e < min) {
						min = e;
					}
					if (e > max) {
						max = e;
					}
				}
			}
			
			if (min.Equals (double.NaN)) {
				// if min == double.NaN, then max is also double.NaN
				min = 0.0;
				max = 0.0;
				return false;
			}
			return true;
		}

		/// <summary>
		/// Returns the minimum and maximum values in a DataRowCollection.
		/// </summary>
		/// <param name="rows">The row collection to search.</param>
		/// <param name="min">The minimum value.</param>
		/// <param name="max">The maximum value.</param>
		/// <param name="columnName">The name of the column in the row collection to search over.</param>
		/// <returns>true is min max set, false otherwise (a = null or zero length).</returns>
		public static bool RowArrayMinMax( DataRowCollection rows, 
			out double min, out double max, string columnName )
		{
			// double[] is a reference type and can be null, if it is then I reckon the best
			// values for min and max are also null. double is a value type so can't be set
			//	to null. So min an max return object, and we understand that if it is not null
			// it is a boxed double (same trick I use lots elsewhere in the lib). The 
			// wonderful comment I didn't write at the top should explain everything.
			if (rows == null || rows.Count == 0) {
				min = 0.0;
				max = 0.0;
				return false;
			}

			min = Utils.ToDouble( (rows[0])[columnName] );
			max = Utils.ToDouble( (rows[0])[columnName] );

			foreach (DataRow r in rows) {
				double e = Utils.ToDouble( r[columnName] );
				if ((min.Equals (double.NaN)) && (!e.Equals (double.NaN))) {
					// if min/max are double.NaN and the current value not, then
					// set them to the current value.
					min = e;
					max = e;
				}
				if (!double.IsNaN(e)) {
					if (e < min) {
						min = e;
					}
					if (e > max) {
						max = e;
					}
				}
			}
			if (min.Equals (double.NaN)) {
				// if min == double.NaN, then max is also double.NaN
				min = 0.0;
				max = 0.0;
				return false;
			}
			return true;
		}

		/// <summary>
		/// Returns the minimum and maximum values in a DataView.
		/// </summary>
		/// <param name="data">The DataView to search.</param>
		/// <param name="min">The minimum value.</param>
		/// <param name="max">The maximum value.</param>
		/// <param name="columnName">The name of the column in the row collection to search over.</param>
		/// <returns>true is min max set, false otherwise (a = null or zero length).</returns>
		public static bool DataViewArrayMinMax( DataView data, 
			out double min, out double max, string columnName )
		{
			// double[] is a reference type and can be null, if it is then I reckon the best
			// values for min and max are also null. double is a value type so can't be set
			//	to null. So min an max return object, and we understand that if it is not null
			// it is a boxed double (same trick I use lots elsewhere in the lib). The 
			// wonderful comment I didn't write at the top should explain everything.
			if (data == null || data.Count == 0) {
				min = 0.0;
				max = 0.0;
				return false;
			}

			min = Utils.ToDouble ((data[0])[columnName]);
			max = Utils.ToDouble ((data[0])[columnName]);

			for (int i=0; i<data.Count; ++i) {

				double e = Utils.ToDouble( data[i][columnName] );
				if (e < min) {
					min = e;
				}
				if (e > max) {
					max = e;
				}
			}
			return true;
		}

		/// <summary>
		/// Returns unit vector along the line	a->b.
		/// </summary>
		/// <param name="a">line start point.</param>
		/// <param name="b">line end point.</param>
		/// <returns>The unit vector along the specified line.</returns>
		public static Point UnitVector (Point a, Point b)
		{
			Point dir = new Point (b.X - a.X, b.Y - a.Y);
			double dirNorm = System.Math.Sqrt (dir.X*dir.X + dir.Y*dir.Y);
			if (dirNorm > 0.0) {
				dir = new Point (dir.X/dirNorm, dir.Y /dirNorm);
			}
			return dir;
		}

		/// <summary>
		/// Get a Font exactly the same as the passed in one, except for scale factor.
		/// </summary>
		/// <param name="initial">The font to scale.</param>
		/// <param name="scale">Scale by this factor.</param>
		/// <returns>The scaled font.</returns>
		public static Font ScaleFont (Font initial, double scale)
		{
			Font scaled = initial.WithScaledSize (scale);
			return scaled;
		}

		/// <summary>
		/// Creates an Image from a tile repeated in each direction as required
		/// </summary>
		/// <param name="tile">image to tile</param>
		/// <param name="final">final image size</param>
		/// <returns>the tiled image</returns>
		public static Image TiledImage (Image tile, Size final)
		{
			Image tiled;
			using (ImageBuilder ib = new ImageBuilder ((int)final.Width, (int)final.Height)) {
				using (Context ctx = ib.Context) {
					double w = tile.Size.Width;
					double h = tile.Size.Height;
					for (double x = 0; x < final.Width; x += w) {
						for (double y = 0; y < final.Height; y += h) {
							ctx.DrawImage (tile, x, y);
						}
					}
				}
				tiled = ib.ToImage ();
			}
			return tiled;
		}
	}
}
