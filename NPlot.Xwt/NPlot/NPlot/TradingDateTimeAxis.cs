//
// XwPlot - A cross-platform charting library using the Xwt toolkit
// 
// TradingDateTimeAxis.cs
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
using Xwt;
using Xwt.Drawing;

namespace NPlot
{

	/// <summary>
	/// Provides a DateTime axis that removes non-trading days.
	/// </summary>
	public class TradingDateTimeAxis : DateTimeAxis
	{
		private double virtualWorldMin_ = double.NaN;
		private double virtualWorldMax_ = double.NaN;
		private long startTradingTime_;
		private long endTradingTime_;
		private long tradingTimeSpan_;

		// we keep shadow "virtual" copies of WorldMin/Max for speed
		// which are already remapped, so it is essential that changes
		// to WorldMin/Max are captured here

		/// <summary>
		/// The axis world min value.
		/// </summary>
		public override double WorldMin
		{
			get { return base.WorldMin; }
			set {
				base.WorldMin = value;
				virtualWorldMin_ = SparseWorldRemap(value);
			}
		}

		/// <summary>
		/// The axis world max value.
		/// </summary>
		public override double WorldMax
		{
			get { return base.WorldMax; }
			set {
				base.WorldMax = value;
				virtualWorldMax_ = SparseWorldRemap(value);
			}
		}

		/// <summary>
		/// Optional time at which trading begins.
		/// All data points earlied than that (same day) will be collapsed.
		/// </summary>
		public virtual TimeSpan StartTradingTime
		{
			get { 
				return new TimeSpan (startTradingTime_);
			}
			set {
				startTradingTime_ = value.Ticks;
				tradingTimeSpan_ = endTradingTime_ - startTradingTime_;
			}
		}

		/// <summary>
		/// Optional time at which trading ends.
		/// All data points later than that (same day) will be collapsed.
		/// </summary>
		public virtual TimeSpan EndTradingTime
		{
			get {
				return new TimeSpan (endTradingTime_); 
			}
			set {
				endTradingTime_ = value.Ticks;
				tradingTimeSpan_ = endTradingTime_ - startTradingTime_;
			}
		}


		/// <summary>
		/// Get whether or not this axis is linear.
		/// </summary>
		public override bool IsLinear
		{
			get { return false; }
		}


		/// <summary>
		/// Constructor
		/// </summary>
		public TradingDateTimeAxis () : base()
		{
			Init();
		}


		/// <summary>
		/// Copy Constructor
		/// </summary>
		/// <param name="a">construct a TradingDateTimeAxis based on this provided axis.</param>
		public TradingDateTimeAxis (Axis a) : base(a)
		{
			Init();
			if (a is TradingDateTimeAxis) 
				DoClone ((TradingDateTimeAxis)a, this);
			else if (a is DateTimeAxis)
				DoClone ((DateTimeAxis)a, this);
			else {
				DoClone (a, this);
				this.NumberFormat = null;
			}
		}


		/// <summary>
		/// Helper function for constructors.
		/// </summary>
		private void Init()
		{
			startTradingTime_ = 0;
			endTradingTime_ = TimeSpan.TicksPerDay;
			tradingTimeSpan_ = endTradingTime_ - startTradingTime_;
			virtualWorldMin_ = SparseWorldRemap(WorldMin);
			virtualWorldMax_ = SparseWorldRemap(WorldMax);
		}


		/// <summary>
		/// Deep copy of DateTimeAxis.
		/// </summary>
		/// <returns>A copy of the DateTimeAxis Class.</returns>
		public override object Clone()
		{
			TradingDateTimeAxis a = new TradingDateTimeAxis ();
			// ensure that this isn't being called on a derived type. If it is, then oh no!
			if (this.GetType() != a.GetType()) {
				throw new NPlotException ( "Clone not defined in derived type. Help!" );
			}
			DoClone (this, a);
			return a;
		}


		/// <summary>
		/// Helper method for Clone.
		/// </summary>
		/// <param name="a">The cloned target object.</param>
		/// <param name="b">The cloned source object.</param>
		protected static void DoClone (TradingDateTimeAxis b, TradingDateTimeAxis a)
		{
			DateTimeAxis.DoClone (b, a);
			a.startTradingTime_ = b.startTradingTime_;
			a.endTradingTime_ = b.endTradingTime_;
			a.tradingTimeSpan_ = b.tradingTimeSpan_;
			a.WorldMin = b.WorldMin;
			a.WorldMax = b.WorldMax;
		}


		/// <summary>
		/// World to physical coordinate transform.
		/// </summary>
		/// <param name="coord">The coordinate value to transform.</param>
		/// <param name="physicalMin">The physical position corresponding to the world minimum of the axis.</param>
		/// <param name="physicalMax">The physical position corresponding to the world maximum of the axis.</param>
		/// <param name="clip">if false, then physical value may extend outside worldMin / worldMax. If true, the physical value returned will be clipped to physicalMin or physicalMax if it lies outside this range.</param>
		/// <returns>The transformed coordinates.</returns>
		/// <remarks>Not sure how much time is spent in this often called function. If it's lots, then
		/// worth optimizing (there is scope to do so).</remarks>
		public override Point WorldToPhysical (
			double coord,
			Point physicalMin,
			Point physicalMax,
			bool clip)
		{
			// (1) account for reversed axis. Could be tricky and move
			// this out, but would be a little messy.
			Point _physicalMin;
			Point _physicalMax;

			if (this.Reversed) {
				_physicalMin = physicalMax;
				_physicalMax = physicalMin;
			}
			else {
				_physicalMin = physicalMin;
				_physicalMax = physicalMax;
			}


			// (2) if want clipped value, return extrema if outside range.
			if (clip) {
				if (WorldMin < WorldMax) {
					if (coord > WorldMax) {
						return _physicalMax;
					}
					if (coord < WorldMin) {
						return _physicalMin;
					}
				}
				else {
					if (coord < WorldMax) {
						return _physicalMax;
					}
					if (coord > WorldMin) {
						return _physicalMin;
					}
				}
			}

			// (3) we are inside range or don't want to clip.
			coord = SparseWorldRemap(coord);
			double range = virtualWorldMax_ - virtualWorldMin_;
			double prop = (double)((coord - virtualWorldMin_) / range);
			//double range = WorldMax - WorldMin;
			//double prop = (double)((coord - WorldMin) / range);
			//if (range1 != range)
			//	  range1 = range;

			// Force clipping at bounding box largeClip times that of real bounding box
			// anyway. This is effectively at infinity.
			double largeClip = 100.0;
			if (prop > largeClip && clip) {
				prop = largeClip;
			}

			if (prop < -largeClip && clip) {
				prop = -largeClip;
			}

			if (range == 0) {
				if (coord >= virtualWorldMin_) {
					prop = largeClip;
				}
				else {
					prop = -largeClip;
				}		
			}

			// calculate the physical coordinate.
			Point offset = new Point (prop*(_physicalMax.X-_physicalMin.X), prop * (_physicalMax.Y-_physicalMin.Y));
			return new Point (_physicalMin.X + offset.X, _physicalMin.Y + offset.Y);
		}

		/// <summary>
		/// Transforms a physical coordinate to an axis world 
		/// coordinate given the physical extremites of the axis.
		/// </summary>
		/// <param name="p">the point to convert</param>
		/// <param name="physicalMin">the physical minimum extremity of the axis</param>
		/// <param name="physicalMax">the physical maximum extremity of the axis</param>
		/// <param name="clip">whether or not to clip the world value to lie in the range of the axis if it is outside.</param>
		/// <returns></returns>
		public override double PhysicalToWorld (Point p, Point physicalMin, Point physicalMax, bool clip)
		{
			// (1) account for reversed axis. Could be tricky and move
			// this out, but would be a little messy.
			Point _physicalMin;
			Point _physicalMax;

			if (this.Reversed) {
				_physicalMin = physicalMax;
				_physicalMax = physicalMin;
			}
			else {
				_physicalMin = physicalMin;
				_physicalMax = physicalMax;
			}

			// normalised axis dir vector
			double axis_X = _physicalMax.X - _physicalMin.X;
			double axis_Y = _physicalMax.Y - _physicalMin.Y;
			double len = Math.Sqrt (axis_X * axis_X + axis_Y * axis_Y);
			axis_X /= len;
			axis_Y /= len;

			// point relative to axis physical minimum.
			Point posRel = new Point (p.X - _physicalMin.X, p.Y - _physicalMin.Y);

			// dist of point projection on axis, normalised.
			double prop = (axis_X * posRel.X + axis_Y * posRel.Y) / len;

			//double world = prop * (WorldMax - WorldMin) + WorldMin;
			double world = prop * (virtualWorldMax_ - virtualWorldMin_) + virtualWorldMin_;
			world = ReverseSparseWorldRemap(world);

			// if want clipped value, return extrema if outside range.
			if (clip) {
				world = Math.Max (world, WorldMin);
				world = Math.Min (world, WorldMax);
			}
			return world;
		}


		/// <summary>
		/// Remap a world coordinate into a "virtual" world, where non-trading dates and times are collapsed.
		/// </summary>
		/// <remarks>
		/// This code works under asumption that there are exactly 24*60*60 seconds in a day
		/// This is strictly speaking not correct but apparently .NET 2.0 does not count leap seconds.
		/// Luckilly, Ticks == 0  =~= 0001-01-01T00:00 =~= Monday
		/// First tried a version fully on floating point arithmetic,
		/// but failed hopelessly due to rounding errors.
		/// </remarks>
		/// <param name="coord">world coordinate to transform.</param>
		/// <returns>equivalent virtual world coordinate.</returns>
		protected double SparseWorldRemap (double coord)
		{
			long ticks = (long)coord;
			long whole_days = ticks / TimeSpan.TicksPerDay;
			long ticks_in_last_day = ticks % TimeSpan.TicksPerDay;
			long full_weeks = whole_days / 7;
			long days_in_last_week = whole_days % 7;
			if (days_in_last_week >= 5) {
				days_in_last_week = 5;
				ticks_in_last_day = 0;
			}
			if (ticks_in_last_day < startTradingTime_) {
				ticks_in_last_day = startTradingTime_;
			}
			else if (ticks_in_last_day > endTradingTime_) {
				ticks_in_last_day = endTradingTime_;
			}
			ticks_in_last_day -= startTradingTime_;

			long whole_working_days = (full_weeks * 5 + days_in_last_week);
			long working_ticks = whole_working_days * tradingTimeSpan_;
			long new_ticks = working_ticks + ticks_in_last_day;

			return (double)new_ticks;
		}


		/// <summary>
		/// Remaps a "virtual" world coordinates back to true world coordinates.
		/// </summary>
		/// <param name="coord">virtual world coordinate to transform.</param>
		/// <returns>equivalent world coordinate.</returns>
		protected double ReverseSparseWorldRemap (double coord)
		{
			long ticks = (long)coord;
			//ticks += startTradingTime_;
			long ticks_in_last_day = ticks % tradingTimeSpan_;
			ticks /= tradingTimeSpan_;
			long full_weeks = ticks / 5;
			long week_part = ticks % 5;

			long day_ticks = (full_weeks * 7 + week_part) * TimeSpan.TicksPerDay;

			return (double)(day_ticks + ticks_in_last_day + startTradingTime_);
		}

		/// <summary>
		/// Adds a delta amount to the given world coordinate in such a way that
		/// all "sparse gaps" are skipped.	In other words, the returned value is
		/// in delta distance from the given in the "virtual" world.
		/// </summary>
		/// <param name="coord">world coordinate to shift.</param>
		/// <param name="delta">shif amount in "virtual" units.</param>
		/// <returns></returns>
		public double SparseWorldAdd (double coord, double delta)
		{
			return ReverseSparseWorldRemap (SparseWorldRemap(coord) + delta);
		}

		/// <summary>
		/// World extent in virtual (sparse) units.
		/// </summary>
		public double SparseWorldLength
		{
			get {
				return SparseWorldRemap (WorldMax) - SparseWorldRemap (WorldMin);
			}
		}

		/// <summary>
		/// Check whether the given coordinate falls within defined trading hours.
		/// </summary>
		/// <param name="coord">world coordinate in ticks to check.</param>
		/// <returns>true if in trading hours, false if in non-trading gap.</returns>
		public bool WithinTradingHours (double coord)
		{
			long ticks = (long)coord;
			long whole_days = ticks / TimeSpan.TicksPerDay;
			long ticks_in_last_day = ticks % TimeSpan.TicksPerDay;
			long days_in_last_week = whole_days % 7;
			if (days_in_last_week >= 5) {
				return false;
			}

			if ((ticks_in_last_day < startTradingTime_) || (ticks_in_last_day >= endTradingTime_)) {
				return false;
			}
			return true;
		}

		/// <summary>
		/// Check whether the given coordinate falls on trading days.
		/// </summary>
		/// <param name="coord">world coordinate in ticks to check.</param>
		/// <returns>true if on Mon - Fri.</returns>
		public bool OnTradingDays (double coord)
		{
			long ticks = (long)coord;
			long whole_days = ticks / TimeSpan.TicksPerDay;
			long days_in_last_week = whole_days % 7;

			return (days_in_last_week < 5);
		}

		/// <summary>
		/// Determines the positions of all Large and Small ticks.
		/// </summary>
		/// <remarks>
		/// The method WorldTickPositions_FirstPass() from the base works just fine, except that it
		/// does not account for non-trading gaps in time, therefore, when less than two days are visible
		/// an own algorithm is used (to show intraday time).  Otherwise the base class implementation is used
		/// but the output is corrected to remove ticks on non-trading days (Sat, Sun).
		/// </remarks>
		/// <param name="physicalMin">The physical position corresponding to the world minimum of the axis.</param>
		/// <param name="physicalMax">The physical position corresponding to the world maximum of the axis.</param>
		/// <param name="largeTickPositions">ArrayList containing the positions of the large ticks.</param>
		/// <param name="smallTickPositions">null</param>
		internal override void WorldTickPositions_FirstPass (
			Point physicalMin,
			Point physicalMax,
			out ArrayList largeTickPositions,
			out ArrayList smallTickPositions
			)
		{
			if (LargeTickStep != TimeSpan.Zero || SparseWorldLength > 2.0 * (double)tradingTimeSpan_) { // utilise base class
				ArrayList largeTickPositions_FirstPass;
				base.WorldTickPositions_FirstPass(physicalMin, physicalMax, out largeTickPositions_FirstPass, out smallTickPositions);

				if (largeTickPositions_FirstPass.Count < 2) {
					// leave it alone, whatever that single tick may be (better something than nothing...)
					largeTickPositions = largeTickPositions_FirstPass;
				}
				else if ((double)largeTickPositions_FirstPass[1] - (double)largeTickPositions_FirstPass[0] > 27.0 * (double)TimeSpan.TicksPerDay) {
					// For distances between ticks in months or longer, just accept all ticks
					largeTickPositions = largeTickPositions_FirstPass;
				}
				else {
					// for daily ticks, ignore non-trading hours but obey (skip) non-trading days
					largeTickPositions = new ArrayList();
					foreach (object tick in largeTickPositions_FirstPass) {
						if (OnTradingDays ((double)tick)) {
							largeTickPositions.Add(tick);
						}
					}
				}
			}
			else { // intraday ticks, own algorithm
				smallTickPositions = null;
				largeTickPositions = new ArrayList ();

				TimeSpan timeLength = new TimeSpan ((long)SparseWorldLength);
				DateTime worldMinDate = new DateTime ((long)this.WorldMin);
				DateTime worldMaxDate = new DateTime ((long)this.WorldMax);

				DateTime currentTickDate;
				long skip; // in time ticks

				// The following if-else flow establishes currentTickDate to the beginning of series
				// and skip to the optimal distance between ticks

				// if less than 10 minutes, then large ticks on second spacings.

				if ( timeLength < new TimeSpan(0,0,10,0,0) ) {
					this.LargeTickLabelType_ = LargeTickLabelType.hourMinuteSeconds;

					int secondsSkip;

					if (timeLength < new TimeSpan(0, 0, 0, 10, 0)) {
						secondsSkip = 1;
					}
					else if (timeLength < new TimeSpan(0, 0, 0, 20, 0)) {
						secondsSkip = 2;
					}
					else if (timeLength < new TimeSpan(0, 0, 0, 50, 0)) {
						secondsSkip = 5;
					}
					else if (timeLength < new TimeSpan(0, 0, 2, 30, 0)) {
						secondsSkip = 15;
					}
					else {
						secondsSkip = 30;
					}

					int second = worldMinDate.Second;
					second -= second % secondsSkip;

					currentTickDate = new DateTime (
						worldMinDate.Year,
						worldMinDate.Month,
						worldMinDate.Day,
						worldMinDate.Hour,
						worldMinDate.Minute,
						second,0 );

					skip = secondsSkip * TimeSpan.TicksPerSecond;
				}
				// Less than 2 hours, then large ticks on minute spacings.
				else if (timeLength < new TimeSpan(0,2,0,0,0)) {
					this.LargeTickLabelType_ = LargeTickLabelType.hourMinute;

					int minuteSkip;

					if (timeLength < new TimeSpan(0, 0, 10, 0, 0)) {
						minuteSkip = 1;
					}
					else if (timeLength < new TimeSpan(0, 0, 20, 0, 0)) {
						minuteSkip = 2;
					}
					else if (timeLength < new TimeSpan(0, 0, 50, 0, 0)) {
						minuteSkip = 5;
					}
					else if (timeLength < new TimeSpan(0, 2, 30, 0, 0)) {
						minuteSkip = 15;
					}
					else { //( timeLength < new TimeSpan( 0,5,0,0,0) )
						minuteSkip = 30;
					}

					int minute = worldMinDate.Minute;
					minute -= minute % minuteSkip;

					currentTickDate = new DateTime (
						worldMinDate.Year,
						worldMinDate.Month,
						worldMinDate.Day,
						worldMinDate.Hour,
						minute,0,0 );

					skip = minuteSkip * TimeSpan.TicksPerMinute;
				}
				// Else large ticks on hour spacings.
				else {
					this.LargeTickLabelType_ = LargeTickLabelType.hourMinute;

					int hourSkip;
					if (timeLength < new TimeSpan(0, 10, 0, 0, 0)) {
						hourSkip = 1;
					}
					else if (timeLength < new TimeSpan(0, 20, 0, 0, 0)) {
						hourSkip = 2;
					}
					else {
						hourSkip = 6;
					}

					int hour = worldMinDate.Hour;
					hour -= hour % hourSkip;

					currentTickDate = new DateTime (
						worldMinDate.Year,
						worldMinDate.Month,
						worldMinDate.Day,
						hour, 0, 0, 0);

					skip = hourSkip * TimeSpan.TicksPerHour;
				}

				// place ticks
				while (currentTickDate < worldMaxDate) {
					double world = (double)currentTickDate.Ticks;

					if (!WithinTradingHours(world)) {
						// add gap boundary instead
						world = ReverseSparseWorldRemap (SparseWorldRemap(world)); // moves forward
						long gap = (long)world;
						gap -= gap % skip;
						currentTickDate = new DateTime (gap);
					}

					if (world >= this.WorldMin && world <= this.WorldMax) {
						largeTickPositions.Add(world);
					}
					currentTickDate = currentTickDate.AddTicks (skip);
				}
			}
		}

		/// <summary>
		/// Get an appropriate label name, given the DateTime of a label
		/// </summary>
		/// <param name="tickDate">the DateTime to get the label name for</param>
		/// <returns>A label name appropriate to the supplied DateTime.</returns>
		protected override string LargeTickLabel(DateTime tickDate)
		{
			string label;

			if (this.NumberFormat == null
				&& (LargeTickLabelType_ == LargeTickLabelType.hourMinute ||
				LargeTickLabelType_ == LargeTickLabelType.hourMinuteSeconds)
				&& tickDate.TimeOfDay == StartTradingTime) {
				// in such case always show the day date
				label = (tickDate.Day).ToString ();
				label += " ";
				label += tickDate.ToString ("MMM");
			}
			else {
				label = base.LargeTickLabel(tickDate);
			}
			return label;
		}
	}
}

