//
// NPlot - A charting library for .NET
// 
// ISequencePlot.cs
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

using NPlot;

namespace NPlot
{
	/// <summary>
	/// Defines a mix-in style interface for plots that use SequenceAdapter to interpret supplied data.
	/// </summary>
	public interface ISequencePlot
	{

		/// <summary>
		/// Gets or sets the source containing a list of values used to populate the plot object.
		/// </summary>
		object DataSource { get; set; }

		/// <summary>
		/// Gets or sets the specific data member in a multimember data source to get data from.
		/// </summary>
		string DataMember { get; set; }

		/// <summary>
		/// Gets or sets the data, or column name for the abscissa [x] axis.
		/// </summary>
		object AbscissaData { get; set; }

		/// <summary>
		/// Gets or sets the data, or column name for the ordinate [y] axis.
		/// </summary>
		object OrdinateData { get; set; }
	}
}
