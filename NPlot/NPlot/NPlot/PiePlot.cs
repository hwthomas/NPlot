/*
 * NPlot - A charting library for .NET
 * 
 * PiePlot.cs
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

/*
using System;
using System.Drawing;

namespace scpl
{
	/// <summary>
	/// Description résumée de PiePlot.
	/// </summary>
	public class PiePlot : BasePlot, IPlot
	{
		public PiePlot(ISequenceAdapter datas)
		{
			//
			// TODO : ajoutez ici la logique du constructeur
			//
			this.Data=datas;

			_brushes=new Brush[_MaxBrush];

        _brushes[0] = Brushes.DarkBlue;
        _brushes[1] = Brushes.Yellow;
        _brushes[2] = Brushes.Green;
        _brushes[3] = Brushes.Brown;
        _brushes[4] = Brushes.Blue;
        _brushes[5] = Brushes.Red;
        _brushes[6] = Brushes.LightGreen;
        _brushes[7] = Brushes.Salmon;
}

		private ISequenceAdapter data_;
		private double _Total;
		const int _MaxBrush=8;
		private Brush[] _brushes;

		public ISequenceAdapter Data
		{
			get
			{
				return data_;
			}
			set
			{
				data_ = value;

				// calculate the sum of all value (this is related to 360°)
				_Total = 0;
				for ( int i=0; i<data_.Count; ++i )
				{
					_Total += data_[i].Y;
				}
			}
		}

		#region SuggestXAxis
		public virtual Axis SuggestXAxis()
		{
				return data_.SuggestXAxis();
		}
		#endregion

		#region SuggestXAxis
		public virtual Axis SuggestYAxis()
		{
			return data_.SuggestYAxis();
		}
		#endregion

		public virtual void Draw( Graphics g, PhysicalAxis xAxis, PhysicalAxis yAxis )
		{
			int LastAngle=0,ReelAngle;

			float rx=(xAxis.PhysicalMax.X - xAxis.PhysicalMin.X);
			float ry=(yAxis.PhysicalMin.Y - yAxis.PhysicalMax.Y);

			int h=(int) (ry * 0.8);
			int w= (int) (rx * 0.8);

			// This is to keep the pie based on a circle (i.e. inside a square)
			int s=Math.Min(h,w);

			// calculate boundary rectangle coordinate
			int cy=(int) (yAxis.PhysicalMax.Y + (h * 1.2 - s) / 2);
			int cx=(int) (xAxis.PhysicalMin.X + (w * 1.2 - s) / 2);

			for (int i=0; i<this.Data.Count; ++i)
			{
				ReelAngle = (int) (data_[i].Y * 360 / _Total);
				g.FillPie(_brushes[i % _MaxBrush], cx, cy, s, s, LastAngle, ReelAngle);
				LastAngle += ReelAngle;
			}
		}
	}
}
*/
