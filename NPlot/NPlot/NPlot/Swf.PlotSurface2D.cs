/*
 * NPlot - A charting library for .NET
 * 
 * Windows.PlotSurface2d.cs
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
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Drawing.Printing;

using NPlot;

namespace NPlot.Windows
{

	/// <summary>
	/// A Windows.Forms PlotSurface2D control.
	/// </summary>
	/// <remarks>
	/// Unfortunately it's not possible to derive from both Control and
	/// NPlot.PlotSurface2D, so Windows.PlotSurface2D derives from 
	/// Windows.Forms.Control, and implements IPlotSurface2D interface.
	/// </remarks>
	[ ToolboxBitmapAttribute(typeof(NPlot.Windows.PlotSurface2D),"PlotSurface2D.ico") ]
	public class PlotSurface2D : System.Windows.Forms.Control, IPlotSurface2D, ISurface
	{

		private System.Drawing.Bitmap bitmapCache = null;	// The off-screen plot area

        private System.Windows.Forms.ToolTip coordinates_;

		private System.Collections.ArrayList selectedObjects_;
        private NPlot.PlotSurface2D ps_;

		private Axis xAxis1ZoomCache_;
		private Axis yAxis1ZoomCache_;
		private Axis xAxis2ZoomCache_;
		private Axis yAxis2ZoomCache_;


        /// <summary>
		/// Flag to display a coordinates in a tooltip.
		/// </summary>
		[ 
		Category("PlotSurface2D"),
		Description("Whether or not to show coordinates in a tool tip when the mouse hovers above the plot area."),
		Browsable(true),
		Bindable(true)
		]
		public bool ShowCoordinates
		{
			get
			{
				return this.coordinates_.Active;
			}
			set
			{
				this.coordinates_.Active = value;
			}
		}


		/// <summary>
		/// Default constructor.
		/// </summary>
		public PlotSurface2D()
		{
 			ps_ = new NPlot.PlotSurface2D();

			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

           // double buffer, and update when resize.
			base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			base.SetStyle(ControlStyles.DoubleBuffer, true);
			base.SetStyle(ControlStyles.UserPaint, true);
			base.ResizeRedraw = true;

            this.InteractionOccured += new InteractionHandler( OnInteractionOccured );
            this.PreRefresh += new PreRefreshHandler( OnPreRefresh );
		}


		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		/// <remarks>Modified! :-)</remarks>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.coordinates_ = new System.Windows.Forms.ToolTip(this.components);
			// 
			// PlotSurface2D
			// 
			this.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.Size = new System.Drawing.Size(328, 272);
		}


        /// <summary>
        /// the key down callback
        /// </summary>
        /// <param name="e">information pertaining to the event</param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            lastKeyEventArgs_ = e;
        }
        KeyEventArgs lastKeyEventArgs_ = null;


        /// <summary>
        /// The key up callback.
        /// </summary>
        /// <param name="e">information pertaining to the event</param>
        protected override void OnKeyUp(KeyEventArgs e)
        {
            lastKeyEventArgs_ = e;
        }

        
		/// <summary>
		/// Handler for the Control.SizeChanged Event
		/// </summary>
		protected override void OnSizeChanged(EventArgs args)
		{
			UpdateCache ();	// create new offscreen Cache and draw plot
			base.OnSizeChanged(args);
		}
       
        
		/// <summary>
		/// Override Control.Refresh to redraw PlotSurface
		/// </summary>
		public override void Refresh ()
		{
			// Update the off-screen drawing cache, then Invalidate raises a
			// PaintEvent to copy the cache to the screen.  NB Update forces an
			// immediate PaintEvent, to ensure that rescaling, scrolling, etc
			// are responsive to the mouse movements as they occur

			this.PreRefresh(this);	// raise PreRefresh event

			UpdateCache();			// Redraw main plot
			this.Invalidate ();		// These calls are equivalent
			this.Update();			// to calling base.Refresh()
			
		}
		
		
		/// <summary>
		/// Create a new Bitmap cache, and obtain a System.Drawing.Graphics from it
		/// The Draw method is called to draw the PlotSurface into the cache, and
		/// this is copied to the visible plotControl (as necessary) by Paint Events
		/// </summary>
		void UpdateCache ()
		{
			if (bitmapCache != null) bitmapCache.Dispose ();
			
			bitmapCache = new System.Drawing.Bitmap(this.Width, this.Height);
			using (Graphics g = Graphics.FromImage(bitmapCache))
			{
				Rectangle plotArea = new Rectangle (0, 0, this.Width, this.Height);
				Draw (g, plotArea);	
			}
		}

		
		/// <summary>
		/// Draws the plot surface on the supplied graphics surface,
		/// obtained from the off-screen bitmapCache. Paint events
		/// then copy this off-screen cache to the display as necessary
		/// </summary>
		/// <param name="g">The graphics surface on which to draw</param>
		/// <param name="bounds">A bounding box on this surface that denotes the area on the
		/// surface to confine drawing to.</param>
		public void Draw( Graphics g, Rectangle bounds )
		{
			ps_.Draw( g, bounds );
		}


		/// <summary>
		/// Draw a lightweight representation of us for design mode.
		/// </summary>
		private void drawDesignMode( Graphics g, Rectangle bounds )
		{
			g.DrawRectangle( new Pen(Color.Black), bounds.X + 2, bounds.Y + 2, bounds.Width-4, bounds.Height-4 );
			g.DrawString( "PlotSurface2D: " + this.Title, this.TitleFont, this.TitleBrush, bounds.X + bounds.Width/2.0f, bounds.Y + bounds.Height/2.0f );
		}

					
		/// <summary>
		/// Handles Control.PaintEvents by obtaining a GC from the PaintEventArgs
		/// and copying	the PlotSurface from the offscreen cache to the display.
		/// Any Interaction Overlays are then added by the call to DoPaint.
		/// Drawing the main PlotSurface2D is not done here, but in UpdateCache
		/// </summary>
		protected override void OnPaint( PaintEventArgs pe )
		{
			Graphics g = pe.Graphics;
			Rectangle clip = pe.ClipRectangle;
			
			// If we are in design mode then just draw PlotSurface outline
			if (LicenseManager.UsageMode == LicenseUsageMode.Designtime) 
			{ 
				this.drawDesignMode( g, clip );
			}
			else
			{
				if(bitmapCache != null)
				{
					// copy off-screen plot cache to the display
					g.DrawImage(bitmapCache, clip, clip, GraphicsUnit.Pixel);
				}
				
				// now add any Interaction overlay drawing
				DoPaint( pe, clip.Width, clip.Height );
			}

			base.OnPaint(pe);
		}

		
		/// <summary>
		/// Adds any Interaction Overlays after the main plot
		/// </summary>
		/// <param name="pe">the PaintEventArgs from paint event.</param>
		/// <param name="width">width of the exposed area</param>
		/// <param name="height">height of the exposed area</param>
		public void DoPaint( PaintEventArgs pe, int width, int height )
		{
 			foreach (Interactions.Interaction i in interactions_)
            {
                i.DoPaint (pe, width, height);
            }
		}
    

		/// <summary>
		/// Clears the plot and resets to default values.
		/// </summary>
		public void Clear()
		{
			xAxis1ZoomCache_ = null;
			yAxis1ZoomCache_ = null;
			xAxis2ZoomCache_ = null;
			yAxis2ZoomCache_ = null;
			ps_.Clear();
            interactions_.Clear();
        }
		

		#region IPlotSurface implementation
		

		/// <summary>
		/// Adds a drawable object to the plot surface. If the object is an IPlot, 
		/// the PlotSurface2D axes will also be updated.
		/// </summary>
		/// <param name="p">The IDrawable object to add to the plot surface.</param>
		public void Add( IDrawable p )
		{
			ps_.Add( p );
		}


		/// <summary>
		/// Adds a drawable object to the plot surface against the specified axes. If
		/// the object is an IPlot, the PlotSurface2D axes will also be updated.
		/// </summary>
		/// <param name="p">the IDrawable object to add to the plot surface</param>
		/// <param name="xp">the x-axis to add the plot against.</param>
		/// <param name="yp">the y-axis to add the plot against.</param>
		public void Add( IDrawable p, NPlot.PlotSurface2D.XAxisPosition xp, NPlot.PlotSurface2D.YAxisPosition yp )
		{
			ps_.Add( p, xp, yp );
		}


		/// <summary>
		/// Adds a drawable object to the plot surface. If the object is an IPlot, 
		/// the PlotSurface2D axes will also be updated.
		/// </summary>
		/// <param name="p">The IDrawable object to add to the plot surface.</param>
		/// <param name="zOrder">The z-ordering when drawing (objects with lower numbers are drawn first)</param>
		public void Add( IDrawable p, int zOrder )
		{
			ps_.Add( p, zOrder );
		}


		/// <summary>
		/// Adds a drawable object to the plot surface against the specified axes. If
		/// the object is an IPlot, the PlotSurface2D axes will also be updated.
		/// </summary>
		/// <param name="p">the IDrawable object to add to the plot surface</param>
		/// <param name="xp">the x-axis to add the plot against.</param>
		/// <param name="yp">the y-axis to add the plot against.</param>
		/// <param name="zOrder">The z-ordering when drawing (objects with lower numbers are drawn first)</param>
		public void Add( IDrawable p, NPlot.PlotSurface2D.XAxisPosition xp,
			NPlot.PlotSurface2D.YAxisPosition yp, int zOrder )
		{
			ps_.Add( p, xp, yp , zOrder);
		}


		/// <summary>
		/// Gets or Sets the legend to use with this plot surface.
		/// </summary>
		[
		Browsable(false),
		Bindable(false)
		]
		public NPlot.Legend Legend
		{
			get
			{
				return ps_.Legend;
			}
			set
			{
				ps_.Legend = value;
			}
		}


		/// <summary>
		/// Gets or Sets the legend z-order.
		/// </summary>
		[
		Browsable(true),
		Bindable(true),
		Category("PlotSurface2D"),
		Description("Determines the order with respect to other IDrawables on the plot surface in which the legend is drawn. " +
			"The higher this value, the higher the position in the draw order." )
		]
		public int LegendZOrder
		{
			get
			{
				return ps_.LegendZOrder;
			}
			set
			{
				ps_.LegendZOrder = value;
			}
		}


		/// <summary>
		/// Whether or not the title will be scaled according to size of the plot 
		/// surface.
		/// </summary>
		[
		Browsable(true),
		Bindable(true),
		Description("Whether or not the title will be scaled according to size of the plot surface."),
		Category("PlotSurface2D")
		]
		public bool AutoScaleTitle
		{
			get
			{
				return ps_.AutoScaleTitle;
			}
			set
			{
				ps_.AutoScaleTitle = value;
			}
		}


		/// <summary>
		/// When plots are added to the plot surface, the axes they are attached to
		/// are immediately modified to reflect data of the plot. If 
		/// AutoScaleAutoGeneratedAxes is true when a plot is added, the axes will
		/// be turned in to auto scaling ones if they are not already [tick marks,
		/// tick text and label size scaled to size of plot surface]. If false,
		/// axes will not be autoscaling.
		/// </summary>
		[
		Browsable(true),
		Bindable(true),
		Description( "When plots are added to the plot surface, the axes they are attached to are immediately modified " +
			"to reflect data of the plot. If AutoScaleAutoGeneratedAxes is true when a plot is added, the axes will be " +
			"turned in to auto scaling ones if they are not already [tick marks, tick text and label size scaled to size " +
			"of plot surface]. If false, axes will not be autoscaling." ),
		Category("PlotSurface2D")
		]
		public bool AutoScaleAutoGeneratedAxes
		{
			get
			{
				return ps_.AutoScaleAutoGeneratedAxes;
			}
			set
			{
				ps_.AutoScaleAutoGeneratedAxes = value;
			}
		}


		/// <summary>
		/// The plot surface title.
		/// </summary>
		[
		Category("PlotSurface2D"),
		Description("The plot surface title"),
		Browsable(true),
		Bindable(true)
		]
		public string Title
		{
			get 
			{
				return ps_.Title;
			}
			set 
			{
				ps_.Title = value;
				//helpful in design view. But crap in applications!
				//this.Refresh();
			}
		}


		/// <summary>
		/// The font used to draw the title.
		/// </summary>
		[
		Category("PlotSurface2D"),
		Description("The font used to draw the title."),
		Browsable(true),
		Bindable(false)
		]
		public Font TitleFont 
		{
			get 
			{
				return ps_.TitleFont;
			}
			set 
			{
				ps_.TitleFont = value;
			}
		}

#if API_1_1
		/// <summary>
		/// It has been renamed to <see cref="SurfacePadding" /> and can be used with .NET 1.1 only.
		/// </summary>
		[
		Category("PlotSurface2D"),
		Description("See SurfacePadding."),
		Browsable(true),
		Bindable(true),
		Obsolete("This property is only maintained in .NET 1.1 profile for compatibility, but it might be removed in the future. Use SurfacePadding instead")
		]
		public int Padding
		{
			get
			{
				return SurfacePadding;
			}
			set
			{
				SurfacePadding = value;
			}
		}
#endif
		/// <summary>
		/// Padding of this width will be left between what is drawn and the control border.
		/// </summary>
		[
		Category("PlotSurface2D"),
		Description("Padding of this width will be left between what is drawn and the control border."),
		Browsable(true),
		Bindable(true)
		]
		public int SurfacePadding
		{
			get
			{
				return ps_.SurfacePadding;
			}
			set
			{
				ps_.SurfacePadding = value;
			}
		}


		/// <summary>
		/// The first abscissa axis.
		/// </summary>
		/// 
		[
		Browsable(false)
		]
		public Axis XAxis1
		{
			get
			{
				return ps_.XAxis1;
			}
			set
			{
				ps_.XAxis1 = value;
			}
		}


		/// <summary>
		/// The first ordinate axis.
		/// </summary>
		[
		Browsable(false)
		]
		public Axis YAxis1
		{
			get
			{
				return ps_.YAxis1;
			}
			set
			{
				ps_.YAxis1 = value;
			}
		}


		/// <summary>
		/// The second abscissa axis.
		/// </summary>
		[
		Browsable(false)
		]
		public Axis XAxis2
		{
			get
			{
				return ps_.XAxis2;
			}
			set
			{
				ps_.XAxis2 = value;
			}
		}


		/// <summary>
		/// The second ordinate axis.
		/// </summary>
		[
		Browsable(false)
		]
		public Axis YAxis2
		{
			get
			{
				return ps_.YAxis2;
			}
			set
			{
				ps_.YAxis2 = value;
			}
		}


		/// <summary>
		/// The physical XAxis1 that was last drawn.
		/// </summary>
		[
		Browsable(false)
		]
		public PhysicalAxis PhysicalXAxis1Cache
		{
			get
			{
				return ps_.PhysicalXAxis1Cache;
			}
		}


		/// <summary>
		/// The physical YAxis1 that was last drawn.
		/// </summary>
		[
		Browsable(false)
		]
		public PhysicalAxis PhysicalYAxis1Cache
		{
			get
			{
				return ps_.PhysicalYAxis1Cache;
			}
		}


		/// <summary>
		/// The physical XAxis2 that was last drawn.
		/// </summary>
		[
		Browsable(false)
		]
		public PhysicalAxis PhysicalXAxis2Cache
		{
			get
			{
				return ps_.PhysicalXAxis2Cache;
			}
		}


		/// <summary>
		/// The physical YAxis2 that was last drawn.
		/// </summary>
		[
		Browsable(false)
		]
		public PhysicalAxis PhysicalYAxis2Cache
		{
			get
			{
				return ps_.PhysicalYAxis2Cache;
			}
		}


		/// <summary>
		/// A color used to paint the plot background. Mutually exclusive with PlotBackImage and PlotBackBrush
		/// </summary>
		/// <remarks>not browsable or bindable because only set method.</remarks>
		[
		Category("PlotSurface2D"),
		Description("Set the plot background color."),
		Browsable(true),
		Bindable(false)
		]
		public System.Drawing.Color PlotBackColor
		{
			set
			{
				ps_.PlotBackColor = value;
			}
		}


		/// <summary>
		/// An imaged used to paint the plot background. Mutually exclusive with PlotBackColor and PlotBackBrush
		/// </summary>
		/// <remarks>not browsable or bindable because only set method.</remarks>
		[
		Browsable(false),
		Bindable(false)
		]
		public System.Drawing.Bitmap PlotBackImage
		{
			set
			{
				ps_.PlotBackImage = value;
			}
		}


		/// <summary>
		/// A Rectangle brush used to paint the plot background. Mutually exclusive with PlotBackColor and PlotBackBrush
		/// </summary>
		/// <remarks>not browsable or bindable because only set method.</remarks>
		[
		Browsable(false),
		Bindable(false)
		]
		public IRectangleBrush PlotBackBrush
		{
			set
			{
				ps_.PlotBackBrush = value;
			}
		}


		/// <summary>
		/// Sets the title to be drawn using a solid brush of this color.
		/// </summary>
		/// <remarks>not browsable or bindable because only set method.</remarks>
		[
		Browsable(false),
		Bindable(false)
		]
		public Color TitleColor
		{
			set
			{
				ps_.TitleColor = value;
			}
		}


		/// <summary>
		/// The brush used for drawing the title.
		/// </summary>
		[
		Browsable(true),
		Bindable(true),
		Description("The brush used for drawing the title."),
		Category("PlotSurface2D")
		]
		public Brush TitleBrush
		{
			get
			{
				return ps_.TitleBrush;
			}
			set
			{
				ps_.TitleBrush = value;
			}
		}

		/// <summary>
		/// Set smoothing mode for drawing plot objects.
		/// </summary>
		[
		Category("PlotSurface2D"),
		Description("Set smoothing mode for drawing plot objects."),
		Browsable(true),
		Bindable(true)
		]
		public System.Drawing.Drawing2D.SmoothingMode SmoothingMode 
		{ 
			get
			{
				return ps_.SmoothingMode;
			}
			set
			{
				ps_.SmoothingMode = value;
			}
		}

		#endregion	// IPlotSurface implementation
		

        /// <summary>
		/// Mouse down event handler.
		/// </summary>
		/// <param name="e">the event args.</param>
		protected override void OnMouseDown(MouseEventArgs e)
		{
			DoMouseDown(e);
			base.OnMouseDown(e);
		}


        /// <summary>
		/// All functionality of the OnMouseDown function is contained here.
		/// This allows use of the all encompasing PlotSurface.
		/// </summary>
		/// <param name="e">The mouse event args from the window we are drawing to.</param>
		public void DoMouseDown( MouseEventArgs e )
		{
			bool dirty = false;
            foreach (Interactions.Interaction i in interactions_)
            {
				dirty |= i.DoMouseDown(e,this);
            }
			if (dirty)
			{
				Refresh();
			}
		}


        /// <summary>
        /// Mouse Wheel event handler.
        /// </summary>
        /// <param name="e">the event args</param>
		protected override void OnMouseWheel( System.Windows.Forms.MouseEventArgs e )
		{
			DoMouseWheel(e);
			base.OnMouseWheel(e);
		}


        /// <summary>
        /// All functionality of the OnMouseWheel function is containd here.
        /// This allows use of the all encompasing PlotSurface.
        /// </summary>
        /// <param name="e">the event args.</param>
        public void DoMouseWheel(MouseEventArgs e)
        {

			bool dirty = false;
            foreach (Interactions.Interaction i in interactions_)
            {
				dirty |= i.DoMouseWheel(e, this);
            }
			if (dirty)
			{
				Refresh();
			}
        }


        /// <summary>
		/// All functionality of the OnMouseMove function is contained here.
		/// This allows use of the all encompasing PlotSurface.
		/// </summary>
		/// <param name="e">The mouse event args from the window we are drawing to.</param>
		/// <param name="ctr">The control that the mouse event happened in.</param>
		public void DoMouseMove( MouseEventArgs e, System.Windows.Forms.Control ctr )
		{
			bool dirty = false;
            foreach (Interactions.Interaction i in interactions_)
            {
				dirty |= i.DoMouseMove(e, ctr, lastKeyEventArgs_);
            }
			if (dirty)
			{
				Refresh();
			}

            // Update coordinates if necessary. 

			if ( coordinates_.Active )
			{
				// we are here
				Point here = new Point( e.X, e.Y );
				if ( ps_.PlotAreaBoundingBoxCache.Contains(here) )
				{
					coordinates_.ShowAlways = true;
					
					// according to Måns Erlandson, this can sometimes be the case.
                    if ((this.PhysicalXAxis1Cache == null) || (this.PhysicalYAxis1Cache == null))
                    {
                        return;
                    }

					double x = this.PhysicalXAxis1Cache.PhysicalToWorld( here, true );
					double y = this.PhysicalYAxis1Cache.PhysicalToWorld( here, true );
					string s = "";
					if (!DateTimeToolTip)
					{
						s = "(" + x.ToString("g4") + "," + y.ToString("g4") + ")"; 
					}
					else
					{
						DateTime dateTime = new DateTime((long)x);
						s = dateTime.ToShortDateString() + " " + dateTime.ToLongTimeString() + Environment.NewLine + y.ToString("f4");
					}
					coordinates_.SetToolTip( this, s );
				}
				else
				{
					coordinates_.ShowAlways = false;
				}
			}
		}


		/// <summary>
		/// MouseMove event handler.
		/// </summary>
		/// <param name="e">The event arguments.</param>
		protected override void OnMouseMove(MouseEventArgs e)
		{
			DoMouseMove( e, this );
			base.OnMouseMove( e );
		}
		

		/// <summary>
		/// MouseLeave event handler. It has to invalidate the control to get rid of
		/// any remnant of vertical and horizontal guides.
		/// </summary>
		/// <param name="e">The event arguments.</param>
		protected override void OnMouseLeave(EventArgs e)        
		{

			DoMouseLeave( e, this );
			base.OnMouseLeave(e);
		}
	

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		/// <param name="ctr"></param>
		public void DoMouseLeave(EventArgs e, System.Windows.Forms.Control ctr) 
		{
			bool dirty = false;
			foreach (Interactions.Interaction i in interactions_)            
			{
				dirty = i.DoMouseLeave(e, this) || dirty;
			}
            if (dirty)
            {
                Refresh();
            }
		}


		/// <summary>
		/// When true, tool tip will display x value as a DateTime. Quick hack - this will probably be 
		/// changed at some point.
		/// </summary>
		[
		Bindable(true),
		Browsable(true),
		Category("PlotSurface2D"),
		Description("When true, tool tip will display x value as a DateTime. Quick hack - this will probably be changed at some point.")
		]
		public bool DateTimeToolTip
		{
			get
			{
				return dateTimeToolTip_;
			}
			set
			{
				dateTimeToolTip_ = value;
			}
		}
		private bool dateTimeToolTip_ = false;


		/// <summary>
		/// All functionality of the OnMouseUp function is contained here.
		/// This allows use of the all encompasing PlotSurface.
		/// </summary>
		/// <param name="e">The mouse event args from the window we are drawing to.</param>
		/// <param name="ctr">The control that the mouse event happened in.</param>
		public void DoMouseUp( MouseEventArgs e, System.Windows.Forms.Control ctr )
		{
			bool dirty = false;

			ArrayList local_interactions = (ArrayList)interactions_.Clone();
			foreach (Interactions.Interaction i in local_interactions)
            {
				dirty |= i.DoMouseUp(e,ctr);
            }
			if (dirty)
			{
				Refresh();
			}

            if (e.Button == MouseButtons.Right)
            {
                Point here = new Point(e.X, e.Y);
                selectedObjects_ = ps_.HitTest(here);
                if (rightMenu_ != null)
                {
                    rightMenu_.Menu.Show(ctr, here);
                }
            }
        }


		/// <summary>
		/// mouse up event handler.
		/// </summary>
		/// <param name="e">The event arguments.</param>
		protected override void OnMouseUp( MouseEventArgs e )
		{
			DoMouseUp(e, this);
			base.OnMouseUp(e);
		}


		/// <summary>
		/// sets axes to be those saved in the cache.
		/// </summary>
		public void OriginalDimensions()
		{
			if ( xAxis1ZoomCache_ != null )
			{
                this.XAxis1 = xAxis1ZoomCache_;
                this.XAxis2 = xAxis2ZoomCache_;
                this.YAxis1 = yAxis1ZoomCache_;
                this.YAxis2 = yAxis2ZoomCache_;

                xAxis1ZoomCache_ = null;
                xAxis2ZoomCache_ = null;
                yAxis1ZoomCache_ = null;
                yAxis2ZoomCache_ = null;
            }					
			this.Refresh();
		}


		/// <summary>
		/// Add an axis constraint to the plot surface. Axis constraints can
		/// specify relative world-pixel scalings, absolute axis positions etc.
		/// </summary>
		/// <param name="c">The axis constraint to add.</param>
		public void AddAxesConstraint( AxesConstraint c )
		{
			ps_.AddAxesConstraint( c );
		}


		/// <summary>
		/// Print the chart as currently shown by the control
		/// </summary>
		/// <param name="preview">If true, show print preview window.</param>
		public void Print( bool preview ) 
		{
			PrintDocument printDocument = new PrintDocument();
			printDocument.PrintPage += new PrintPageEventHandler(NPlot_PrintPage);
			printDocument.DefaultPageSettings.Landscape = true;
				 	
			DialogResult result;
			if (!preview) 
			{
				PrintDialog dlg = new PrintDialog();
				dlg.Document = printDocument;
				result = dlg.ShowDialog();
			} 
			else 
			{
				PrintPreviewDialog dlg = new PrintPreviewDialog();
				dlg.Document = printDocument;
				result = dlg.ShowDialog();
			}
			if (result == DialogResult.OK) 
			{
				try 
				{
					printDocument.Print();
				}								 				
				catch 
				{
					Console.WriteLine( "caught\n" );
				}
			}
		}


		private void NPlot_PrintPage(object sender, PrintPageEventArgs ev) 
		{
			Rectangle r = ev.MarginBounds;
			this.Draw( ev.Graphics, r );
			ev.HasMorePages = false;
		}
	
		
		/// <summary>
		/// Coppies the chart currently shown in the control to the clipboard as an image.
		/// </summary>
		public void CopyToClipboard()
		{
			System.Drawing.Bitmap b = new System.Drawing.Bitmap( this.Width, this.Height );
			System.Drawing.Graphics g = Graphics.FromImage( b );
			g.Clear(Color.White);
			this.Draw( g, new Rectangle( 0, 0, b.Width-1, b.Height-1 ) );
			Clipboard.SetDataObject( b, true );
		}


		/// <summary>
		/// Coppies data in the current plot surface view window to the clipboard
		/// as text.
		/// </summary>
		public void CopyDataToClipboard()
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder();

			for (int i=0; i<ps_.Drawables.Count; ++i)
			{
				IPlot plot = ps_.Drawables[i] as IPlot;
				if (plot != null)
				{
					Axis xAxis = ps_.WhichXAxis( plot );
					Axis yAxis = ps_.WhichYAxis( plot );

					RectangleD region = new RectangleD( 
						xAxis.WorldMin, 
						yAxis.WorldMin,
						xAxis.WorldMax - xAxis.WorldMin,
						yAxis.WorldMax - yAxis.WorldMin );

					plot.WriteData( sb, region, true );
				}
			}

			Clipboard.SetDataObject( sb.ToString(), true );
		}


        /// <summary>
        /// Remove a drawable object from the plot surface.
        /// </summary>
        /// <param name="p">the drawable to remove</param>
        /// <param name="updateAxes">whether or not to update the axes after removing the idrawable.</param>
        public void Remove(IDrawable p, bool updateAxes)
        {
            ps_.Remove(p, updateAxes);
        }


        /// <summary>
		/// Gets an array list containing all drawables currently added to the PlotSurface2D.
		/// </summary>
		[
		Browsable(false),
		Bindable(false)
		]
		public ArrayList Drawables
		{
			get
			{
				return ps_.Drawables;
			}
		}


		/// <summary>
		/// Sets the right context menu. Custom menus can be designed by overriding
		/// NPlot.Windows.PlotSurface2D.ContextMenu.
		/// </summary>
		[
		Browsable(false),
		Bindable(false)
		]
		public NPlot.Windows.PlotSurface2D.PlotContextMenu RightMenu
		{
			get
			{
				return rightMenu_;
			}
			set
			{
				rightMenu_ = value;
				if (rightMenu_ != null)
				{
					rightMenu_.PlotSurface2D = this;
				}
			}
		}
		private NPlot.Windows.PlotSurface2D.PlotContextMenu rightMenu_ = null;


		/// <summary>
		/// Gets an instance of a NPlot.Windows.PlotSurface2D.ContextMenu that
		/// is useful in typical situations.
		/// </summary>
		public static PlotContextMenu DefaultContextMenu
		{
			get
			{
				return new NPlot.Windows.PlotSurface2D.PlotContextMenu();
			}
		}


        /// <summary>
        /// Allows access to the PlotSurface2D.
        /// </summary>
        [
		Browsable(false),
		Bindable(false)
		]
        public NPlot.PlotSurface2D Inner
        {
            get
            {
                return ps_;
            }
        }


        /// <summary>
        /// Remembers the current axes - useful in interactions.
        /// </summary>
        public void CacheAxes()
        {
            if (xAxis1ZoomCache_ == null && xAxis2ZoomCache_ == null &&
                 yAxis1ZoomCache_ == null && yAxis2ZoomCache_ == null)
            {
                if (this.XAxis1 != null)
                {
                    xAxis1ZoomCache_ = (Axis)this.XAxis1.Clone();
                }
                if (this.XAxis2 != null)
                {
                    xAxis2ZoomCache_ = (Axis)this.XAxis2.Clone();
                }
                if (this.YAxis1 != null)
                {
                    yAxis1ZoomCache_ = (Axis)this.YAxis1.Clone();
                }
                if (this.YAxis2 != null)
                {
                    yAxis2ZoomCache_ = (Axis)this.YAxis2.Clone();
                }
            }
        }


        /// <summary>
        /// Encapsulates a number of separate "Interactions". An interaction is basically 
        /// a set of handlers for mouse and keyboard events that work together in a 
        /// specific way. 
        /// </summary>
        public abstract class Interactions
        {

        	#region Base Class
 
        	/// <summary>
            /// Base class for an interaction. All methods are virtual. Not abstract as not all interactions
            /// need to use all methods. Default functionality for each method is to do nothing. 
            /// </summary>
            public class Interaction
            {
                /// <summary>
                /// Handler for this interaction if a mouse down event is received.
                /// </summary>
                /// <param name="e">event args</param>
                /// <param name="ctr">reference to the control</param>
                /// <returns>true if plot surface needs refreshing.</returns>
                public virtual bool DoMouseDown(MouseEventArgs e, System.Windows.Forms.Control ctr) { return false; }
                
                /// <summary>
                /// Handler for this interaction if a mouse up event is received.
                /// </summary>
				/// <param name="e">event args</param>
				/// <param name="ctr">reference to the control</param>
				/// <returns>true if plot surface needs refreshing.</returns>
                public virtual bool DoMouseUp(MouseEventArgs e, System.Windows.Forms.Control ctr) { return false; }
                
                /// <summary>
                /// Handler for this interaction if a mouse move event is received.
                /// </summary>
				/// <param name="e">event args</param>
				/// <param name="ctr">reference to the control</param>
                /// <param name="lastKeyEventArgs"></param>
                /// <returns>true if plot surface needs refreshing.</returns>
                public virtual bool DoMouseMove(MouseEventArgs e, System.Windows.Forms.Control ctr, KeyEventArgs lastKeyEventArgs) { return false; }
                
                /// <summary>
                /// Handler for this interaction if a mouse move event is received.
                /// </summary>
				/// <param name="e">event args</param>
				/// <param name="ctr">reference to the control</param>
				/// <returns>true if plot surface needs refreshing.</returns>
                public virtual bool DoMouseWheel(MouseEventArgs e, System.Windows.Forms.Control ctr) { return false; }
                
				/// <summary>
				/// Handler for this interaction if a mouse Leave event is received.
				/// </summary>
				/// <param name="e">event args</param>
				/// <param name="ctr">reference to the control</param>
				/// <returns>true if the plot surface needs refreshing.</returns>
				public virtual bool DoMouseLeave(EventArgs e, System.Windows.Forms.Control ctr) { return false; }

                /// <summary>
                /// Handler for this interaction if a paint event is received.
                /// </summary>
                /// <param name="pe">paint event args</param>
                /// <param name="width"></param>
                /// <param name="height"></param>
                public virtual void DoPaint(PaintEventArgs pe, int width, int height) { }
                
            }
       		#endregion
 
            #region RubberBandSelection
            /// <summary>
            /// 
            /// </summary>
            public class RubberBandSelection : Interaction
            {
                private bool selectionInitiated_ = false;
                private int minimumSelectSize = 5;	// make user-defined, like RangeSelection?
                private Overlay outline = new Overlay (0,0);

                /// <summary>
                /// 
                /// </summary>
                /// <param name="e"></param>
                /// <param name="ctr"></param>
                public override bool DoMouseDown(MouseEventArgs e, Control ctr)
                {
	               	Windows.PlotSurface2D wps = (Windows.PlotSurface2D)ctr;
                    NPlot.PlotSurface2D ps = wps.Inner;
                    
                 	// Only start selection within the plot area
                 	if(ps.PlotAreaBoundingBoxCache.Contains(e.Location))
                 	{
                    	// record start point and initiate the selection
                    	selectionInitiated_ = true;
                    	outline.StartPoint = e.Location;
                 	}
 					return false;
                }

                /// <summary>
                /// 
                /// </summary>
                /// <param name="e"></param>
                /// <param name="ctr"></param>
                /// <param name="lastKeyEventArgs"></param>
                public override bool DoMouseMove(MouseEventArgs e, Control ctr, KeyEventArgs lastKeyEventArgs)
                {
 	               	Windows.PlotSurface2D wps = (Windows.PlotSurface2D)ctr;
                    NPlot.PlotSurface2D ps = wps.Inner;
                    
                	if ((e.Button == MouseButtons.Left) && selectionInitiated_)
                    {
                        // delete the previous outline
                        ctr.Invalidate(outline.Extent);
                        
                        // clip mouse to plot area, so selection is always valid
                        Rectangle clip = ps.PlotAreaBoundingBoxCache;
                        Point here = e.Location;
                        here.X = Math.Min(here.X, clip.Right);
                        here.X = Math.Max(here.X, clip.Left);
                        here.Y = Math.Min(here.Y, clip.Bottom);
                        here.Y = Math.Max(here.Y, clip.Top);
                       
                        // update overlay endPoint and redraw by forcing an Expose event
                        outline.EndPoint = here;
                        ctr.Invalidate(outline.Extent);
						ctr.Update();
                    }
                   
					return false;
                }

                /// <summary>
                /// 
                /// </summary>
                /// <param name="e"></param>
                /// <param name="ctr"></param>
                public override bool DoMouseUp(MouseEventArgs e, Control ctr)
                {
  	               	Windows.PlotSurface2D wps = (Windows.PlotSurface2D)ctr;
                    NPlot.PlotSurface2D ps = wps.Inner;
 
                    Rectangle selection = outline.Extent;
                    Point minPoint = outline.TopLeft;
                    Point maxPoint = outline.BottomRight;
                    
                    // erase and clear current outline
                    ctr.Invalidate(outline.Extent);
                	outline.Clear();
                    
                    bool outside  = !ps.PlotAreaBoundingBoxCache.Contains(e.Location);
                    bool tooSmall = (selection.Width < minimumSelectSize || selection.Height < minimumSelectSize);

                    // Cancel selection if too small or mouse outside plot area
                    if(outside || tooSmall)
                    {
	                   	selectionInitiated_ = false;
                 		return false;
                 	}
                    	
                    // handle left button (selecting region).
                    if ((e.Button == MouseButtons.Left) && selectionInitiated_)
                    {
                    	selectionInitiated_ = false;

 						wps.CacheAxes();
						
						wps.PhysicalXAxis1Cache.SetWorldLimitsFromPhysical(minPoint, maxPoint);
                        wps.PhysicalXAxis2Cache.SetWorldLimitsFromPhysical(minPoint, maxPoint);
                        wps.PhysicalYAxis1Cache.SetWorldLimitsFromPhysical(maxPoint, minPoint);
                        wps.PhysicalYAxis2Cache.SetWorldLimitsFromPhysical(maxPoint, minPoint);

                        wps.InteractionOccured(this);

						return true;
 					}
               		return false;
                }
 
                
                /// <summary>
                /// Handler for this interaction if a paint event is received.
                /// This draws the overlay (selection) rectangle onto the plot,
                /// using the overlay.Extent dimensions
                /// </summary>
                /// <param name="pe">paint event args</param>
                /// <param name="width"></param>
                /// <param name="height"></param>
                public override void DoPaint(PaintEventArgs pe, int width, int height)
                {
	               	Graphics g = pe.Graphics;
                	
	               	// use 100% opacity for overlay outline
                	Color penColor = Color.FromArgb(0xFF, Color.White);
					Pen pen = new Pen(penColor);
					Rectangle rect = outline.Extent;
					rect.Width -= 1;
					rect.Height -= 1;

                	g.DrawRectangle (pen, rect);
 					pen.Dispose();
                	
                }

            }
            #endregion
            #region HorizontalGuideline
            /// <summary>
            /// Horizontal line interaction
            /// </summary>
            public class HorizontalGuideline : Interaction
            {
                private Color color_;
                private Overlay guideline = new Overlay (0,0);

                /// <summary>
                /// Constructor
                /// </summary>
                public HorizontalGuideline()
                {
                    color_ = Color.Black;
                }


                /// <summary>
                /// Constructor
                /// </summary>
                /// <param name="lineColor"></param>
                public HorizontalGuideline(Color lineColor)
                {
                    color_ = lineColor;
                }


                /// <summary>
                /// 
                /// </summary>
                /// <param name="e"></param>
                /// <param name="ctr"></param>
                /// <param name="lastKeyEventArgs"></param>
				public override bool DoMouseMove(MouseEventArgs e, System.Windows.Forms.Control ctr, KeyEventArgs lastKeyEventArgs)
				{
                	Windows.PlotSurface2D wps = (Windows.PlotSurface2D)ctr;
                    NPlot.PlotSurface2D ps = wps.Inner;

                    Rectangle clip = ps.PlotAreaBoundingBoxCache;
                    
                    // erase previous guideline
                    ctr.Invalidate(guideline.Extent);
                    
                    // if mouse is in plot region, draw horizontal line
                    if (clip.Contains(e.Location))
                    {
                       	guideline.StartPoint = new Point(clip.Left, e.Y);
                        guideline.EndPoint = new Point(clip.Right, e.Y);
                        ctr.Invalidate(guideline.Extent);
                        ctr.Update();
                    }
                    else
                    {
                    	guideline.Clear();
                    }
					return false;
				}


				/// <summary>
				/// Handler for mouse leave event
				/// </summary>
				/// <param name="e">event args</param>
				/// <param name="ctr"></param>
				/// <returns></returns>
				public override bool DoMouseLeave(EventArgs e, System.Windows.Forms.Control ctr)	                 
				{
					ctr.Invalidate(guideline.Extent);
					guideline.Clear();
					return false;
				}
			
                /// <summary>
                /// Handler for this interaction if a paint event is received.
                /// This draws the overlay (guideline) onto the plot,
                /// using the overlay.Extent dimensions
                /// </summary>
                /// <param name="pe">paint event args</param>
                /// <param name="width"></param>
                /// <param name="height"></param>
                public override void DoPaint(PaintEventArgs pe, int width, int height)
                {
	               	Graphics g = pe.Graphics;
                	
	               	// use 100% opacity for overlay guideline
                	Color penColor = Color.FromArgb(0xFF, color_);
                	
					Pen pen = new Pen(penColor);
					Point start = guideline.TopLeft;
					Point end = guideline.BottomRight;
	
	               	g.DrawLine (pen, start, end);
 					pen.Dispose();
               	
                }
				
				
				
				
				
	         }
            #endregion
            #region VerticalGuideline
            /// <summary>
            /// 
            /// </summary>
            public class VerticalGuideline : Interaction
            {
                private Color color_;
                private Overlay guideline = new Overlay (0,0);

                /// <summary>
                /// 
                /// </summary>
                public VerticalGuideline()
                {
                    color_ = Color.Black;
                }


                /// <summary>
                /// 
                /// </summary>
                /// <param name="lineColor"></param>
                public VerticalGuideline(Color lineColor)
                {
                    color_ = lineColor;
                }


                /// <summary>
                /// 
                /// </summary>
                /// <param name="e"></param>
                /// <param name="ctr"></param>
                /// <param name="lastKeyEventArgs"></param>
                public override bool DoMouseMove(MouseEventArgs e, System.Windows.Forms.Control ctr, KeyEventArgs lastKeyEventArgs)
                {
                	Windows.PlotSurface2D wps = (Windows.PlotSurface2D)ctr;
                    NPlot.PlotSurface2D ps = wps.Inner;

                    Rectangle clip = ps.PlotAreaBoundingBoxCache;
                    
                    // erase previous guideline
                    ctr.Invalidate(guideline.Extent);
                    
                    // if mouse is in plot region, draw vertical line
                    if ((e.Button == MouseButtons.Left) && clip.Contains(e.Location))
                    {
                       	guideline.StartPoint = new Point(e.X, clip.Top);
                        guideline.EndPoint = new Point(e.X, clip.Bottom);
                        ctr.Invalidate(guideline.Extent);
                        ctr.Update();
                    }
                    else
                    {
                    	guideline.Clear();
                    }
					return false;
                }

				/// <summary>
				/// Handler for mouse leave event
				/// </summary>
				/// <param name="e">event args</param>
				/// <param name="ctr"></param>
				/// <returns></returns>
				public override bool DoMouseLeave(EventArgs e, System.Windows.Forms.Control ctr)	                 
				{
					ctr.Invalidate(guideline.Extent);
					guideline.Clear();
					return false;
				}
				
				
                /// <summary>
                /// Handler for this interaction if a paint event is received.
                /// This draws the overlay (guideline) onto the plot,
                /// using the overlay.Extent dimensions
                /// </summary>
                /// <param name="pe">paint event args</param>
                /// <param name="width"></param>
                /// <param name="height"></param>
                public override void DoPaint(PaintEventArgs pe, int width, int height)
                {
	               	Graphics g = pe.Graphics;
                	
	               	// use 100% opacity for overlay guideline
                	Color penColor = Color.FromArgb(0xFF, color_);
                	
					Pen pen = new Pen(penColor);
					Point start = guideline.TopLeft;
					Point end = guideline.BottomRight;

	               	g.DrawLine (pen, start, end);
 					pen.Dispose();
               	
                }
				
				
            }
            #endregion
            #region HorizontalDrag
            /// <summary>
            /// 
            /// </summary>
            public class HorizontalDrag : Interaction
            {
                /// <summary>
                /// 
                /// </summary>
                /// <param name="e"></param>
                /// <param name="ctr"></param>
                public override bool DoMouseDown(MouseEventArgs e, Control ctr)
                {
                    NPlot.PlotSurface2D ps = ((Windows.PlotSurface2D)ctr).Inner;

                    if (e.X > ps.PlotAreaBoundingBoxCache.Left && e.X < (ps.PlotAreaBoundingBoxCache.Right) &&
                        e.Y > ps.PlotAreaBoundingBoxCache.Top && e.Y < ps.PlotAreaBoundingBoxCache.Bottom)
                    {
                        dragInitiated_ = true;

                        lastPoint_.X = e.X;
                        lastPoint_.Y = e.Y;
                    }

					return false;
                }


                /// <summary>
                /// 
                /// </summary>
                /// <param name="e"></param>
                /// <param name="ctr"></param>
                /// <param name="lastKeyEventArgs"></param>
                public override bool DoMouseMove(MouseEventArgs e, Control ctr, KeyEventArgs lastKeyEventArgs)
                {
                    NPlot.PlotSurface2D ps = ((Windows.PlotSurface2D)ctr).Inner;

					if ((e.Button == MouseButtons.Left) && dragInitiated_)
					{
						int diffX = e.X - lastPoint_.X;

						((Windows.PlotSurface2D)ctr).CacheAxes();

						// original code was using PixelWorldLength of the physical axis
						// but it was not working for non-linear axes - the code below works
						// in all cases
						if (ps.XAxis1 != null)
						{
							Axis axis = ps.XAxis1;
							PointF pMin = ps.PhysicalXAxis1Cache.PhysicalMin;
							PointF pMax = ps.PhysicalXAxis1Cache.PhysicalMax;

                            PointF physicalWorldMin = pMin;
                            PointF physicalWorldMax = pMax;
							physicalWorldMin.X -= diffX;
							physicalWorldMax.X -= diffX;
							double newWorldMin = axis.PhysicalToWorld(physicalWorldMin, pMin, pMax, false);
							double newWorldMax = axis.PhysicalToWorld(physicalWorldMax, pMin, pMax, false);
							axis.WorldMin = newWorldMin;
							axis.WorldMax = newWorldMax;
						}

						if (ps.XAxis2 != null)
						{
							Axis axis = ps.XAxis2;
							PointF pMin = ps.PhysicalXAxis2Cache.PhysicalMin;
							PointF pMax = ps.PhysicalXAxis2Cache.PhysicalMax;
						
                            PointF physicalWorldMin = pMin;
                            PointF physicalWorldMax = pMax;
							physicalWorldMin.X -= diffX;
							physicalWorldMax.X -= diffX;
							double newWorldMin = axis.PhysicalToWorld(physicalWorldMin, pMin, pMax, false);
							double newWorldMax = axis.PhysicalToWorld(physicalWorldMax, pMin, pMax, false);
							axis.WorldMin = newWorldMin;
							axis.WorldMax = newWorldMax;
						}
						
						lastPoint_ = new Point(e.X, e.Y);

						((Windows.PlotSurface2D)ctr).InteractionOccured(this);

						return true;
					}
                    
					return false;
                }


                /// <summary>
                /// 
                /// </summary>
                /// <param name="e"></param>
                /// <param name="ctr"></param>
                public override bool DoMouseUp(MouseEventArgs e, Control ctr)
                {
                    if ((e.Button == MouseButtons.Left) && dragInitiated_)
                    {
                       lastPoint_ = unset_;
                       dragInitiated_ = false;
                    }

					return false;
                }

                private bool dragInitiated_ = false;
                private Point lastPoint_ = new Point(-1, -1);
                // this is the condition for an unset point
                private Point unset_ = new Point(-1, -1);
            }
            #endregion
            #region VerticalDrag
            /// <summary>
            /// 
            /// </summary>
            public class VerticalDrag : Interaction
            {
                /// <summary>
                /// 
                /// </summary>
                /// <param name="e"></param>
                /// <param name="ctr"></param>
                public override bool DoMouseDown(MouseEventArgs e, Control ctr)
                {
                    NPlot.PlotSurface2D ps = ((Windows.PlotSurface2D)ctr).Inner;

                    if (e.X > ps.PlotAreaBoundingBoxCache.Left && e.X < (ps.PlotAreaBoundingBoxCache.Right) &&
                        e.Y > ps.PlotAreaBoundingBoxCache.Top && e.Y < ps.PlotAreaBoundingBoxCache.Bottom)
                    {
                        dragInitiated_ = true;

                        lastPoint_.X = e.X;
                        lastPoint_.Y = e.Y;
                    }

					return false;
                }


                /// <summary>
                /// 
                /// </summary>
                /// <param name="e"></param>
                /// <param name="ctr"></param>
                /// <param name="lastKeyEventArgs"></param>
				public override bool DoMouseMove(MouseEventArgs e, Control ctr, KeyEventArgs lastKeyEventArgs)
				{
					NPlot.PlotSurface2D ps = ((Windows.PlotSurface2D)ctr).Inner;

					if ((e.Button == MouseButtons.Left) && dragInitiated_)
					{
  
						int diffY = e.Y - lastPoint_.Y;

						((Windows.PlotSurface2D)ctr).CacheAxes();

						if (ps.YAxis1 != null)
						{
							Axis axis = ps.YAxis1;
							PointF pMin = ps.PhysicalYAxis1Cache.PhysicalMin;
							PointF pMax = ps.PhysicalYAxis1Cache.PhysicalMax;
								
							PointF physicalWorldMin = pMin;
							PointF physicalWorldMax = pMax;
							physicalWorldMin.Y -= diffY;
							physicalWorldMax.Y -= diffY;
							double newWorldMin = axis.PhysicalToWorld(physicalWorldMin, pMin, pMax, false);
							double newWorldMax = axis.PhysicalToWorld(physicalWorldMax, pMin, pMax, false);
							axis.WorldMin = newWorldMin;
							axis.WorldMax = newWorldMax;
						}

						if (ps.YAxis2 != null)
						{
							Axis axis = ps.YAxis2;
							PointF pMin = ps.PhysicalYAxis2Cache.PhysicalMin;
							PointF pMax = ps.PhysicalYAxis2Cache.PhysicalMax;

							PointF physicalWorldMin = pMin;
							PointF physicalWorldMax = pMax;
							physicalWorldMin.Y -= diffY;
							physicalWorldMax.Y -= diffY;
							double newWorldMin = axis.PhysicalToWorld(physicalWorldMin, pMin, pMax, false);
							double newWorldMax = axis.PhysicalToWorld(physicalWorldMax, pMin, pMax, false);
							axis.WorldMin = newWorldMin;
							axis.WorldMax = newWorldMax;
						}

						lastPoint_ = new Point(e.X, e.Y);

						((Windows.PlotSurface2D)ctr).InteractionOccured(this);

						return true;
					}
				
					return false;
				}


                /// <summary>
                /// 
                /// </summary>
                /// <param name="e"></param>
                /// <param name="ctr"></param>
                public override bool DoMouseUp(MouseEventArgs e, Control ctr)
                {
                    if ((e.Button == MouseButtons.Left) && dragInitiated_)
                    {
                        lastPoint_ = unset_;
                        dragInitiated_ = false;
                    }

					return false;
                }

                private bool dragInitiated_ = false;
                private Point lastPoint_ = new Point(-1, -1);
                // this is the condition for an unset point
                private Point unset_ = new Point(-1, -1);
            }
            #endregion
            #region HorizontalRangeSelection
            /// <summary>
            /// This plot interaction allows the user to select horizontal regions.
            /// </summary>
            public class HorizontalRangeSelection : Interaction
            {
                private bool selectionInitiated_ = false;
                private Overlay selection = new Overlay (0,0);
 
                private int minimumPixelDistanceForSelect_ = 5;
				private double smallestAllowedRange_ = double.Epsilon * 100.0;


				/// <summary>
				/// Default constructor
				/// </summary>
				public HorizontalRangeSelection()
				{
				}


				/// <summary>
				/// Constructor
				/// </summary>
				/// <param name="smallestAllowedRange">the smallest distance between the selected xmin and xmax for the selection to be performed.</param>
				public HorizontalRangeSelection( double smallestAllowedRange )
				{
					this.smallestAllowedRange_ = smallestAllowedRange;
				}


				/// <summary>
				/// The minimum width of the selected region (in pixels) for the interaction to zoom.
				/// </summary>
				public int MinimumPixelDistanceForSelect
				{
					get
					{
						return minimumPixelDistanceForSelect_;
					}
					set
					{
						minimumPixelDistanceForSelect_ = value;
					}
				}


				/// <summary>
				/// The smallest range (distance between world min and world max) selectable.
				/// If a smaller region is selected, the selection will do nothing.
				/// </summary>
				public double SmallestAllowedRange
				{
					get
					{
						return smallestAllowedRange_;
					}
					set
					{
						smallestAllowedRange_ = value;
					}
				}


				/// <summary>
				/// Handler for mouse down event for this interaction
				/// </summary>
				/// <param name="e">the mouse event args</param>
				/// <param name="ctr">the plot surface this event applies to</param>
                public override bool DoMouseDown(MouseEventArgs e, Control ctr )
                {
                 	Windows.PlotSurface2D wps = (Windows.PlotSurface2D)ctr;
                    NPlot.PlotSurface2D ps = wps.Inner;
                    
                    Rectangle clip = ps.PlotAreaBoundingBoxCache;

                    // if mouse is in plot region, and left-button pressed, start selection
                    if ((e.Button == MouseButtons.Left) && clip.Contains(e.Location))
                    {
                        // start selection and initialise overlay
                        selectionInitiated_ = true;
                       	selection.StartPoint = new Point(e.X, clip.Top);
                        selection.EndPoint = new Point(e.X, clip.Bottom);
     	            }
        			return false;
                }

                
				/// <summary>
				/// Handler for mouse move event for this interaction
				/// </summary>
				/// <param name="e">the mouse event args</param>
				/// <param name="ctr">the plot surface this event applies to</param>
                /// <param name="lastKeyEventArgs"></param>
                public override bool DoMouseMove(MouseEventArgs e, Control ctr, KeyEventArgs lastKeyEventArgs)
                {
					Windows.PlotSurface2D wps = (Windows.PlotSurface2D)ctr;
					NPlot.PlotSurface2D ps = wps.Inner;
                    Rectangle clip = ps.PlotAreaBoundingBoxCache;
                    
                   // delete the previous selection
                   ctr.Invalidate(selection.Extent);
                        
                   if ((e.Button == MouseButtons.Left) && selectionInitiated_)
                    {
                        // clip mouse to plot area, so selection is always valid
                        Point here = e.Location;
                        here.X = Math.Min(here.X, clip.Right);
                        here.X = Math.Max(here.X, clip.Left);
                        here.Y = clip.Bottom;
                       
                        // update overlay and redraw by forcing an Expose event
                        selection.EndPoint = here;
                        ctr.Invalidate(selection.Extent);
						ctr.Update();
                    }
					return false;
                }

                
                /// <summary>
                /// Handler for mouse up event for this interaction
                /// </summary>
                /// <param name="e">the mouse event args</param>
                /// <param name="ctr">the plot surface this event applies to</param>
                public override bool DoMouseUp(MouseEventArgs e, Control ctr)
                {
                 	Windows.PlotSurface2D wps = (Windows.PlotSurface2D)ctr;
                    NPlot.PlotSurface2D ps = wps.Inner;

                    // Cancel selection if mouse is outside plot area when released
                    if(!ps.PlotAreaBoundingBoxCache.Contains(e.Location))
                    {
	                   	selectionInitiated_ = false;
                        ctr.Invalidate(selection.Extent);
                		selection.Clear();
                 		return false;
                 	}
                    	
                    // handle left button (selecting region).
                    if ((e.Button == MouseButtons.Left) && selectionInitiated_)
                    {
              			// get (valid) selection extent
	               		Rectangle extent = selection.Extent;
	               		
	               		Point start = selection.TopLeft;
	               		Point end = selection.BottomRight;
	               		
                    	selectionInitiated_ = false;
						ctr.Invalidate(extent);
                		selection.Clear();
 
						// ignore very small selections
						if (extent.Width < minimumPixelDistanceForSelect_)  
						{
							return false;
						}

                        wps.CacheAxes();

						// determine the new x axis 1 world limits (and check to see if they are far enough appart).
						double xAxis1Min = double.NaN;
						double xAxis1Max = double.NaN;
                        if (ps.XAxis1 != null)
                        {
                            int x1 = (int)Math.Min(end.X, start.X);
                            int x2 = (int)Math.Max(end.X, start.X);
                            int y = ps.PhysicalXAxis1Cache.PhysicalMax.Y;

							xAxis1Min = ps.PhysicalXAxis1Cache.PhysicalToWorld(new Point(x1, y), true);
							xAxis1Max = ps.PhysicalXAxis1Cache.PhysicalToWorld(new Point(x2, y), true);
							if (xAxis1Max - xAxis1Min < this.smallestAllowedRange_)
							{
								return false;
							}
                        }

						// determine the new x axis 2 world limits (and check to see if they are far enough appart).
                        double xAxis2Min = double.NaN;
						double xAxis2Max = double.NaN;
						if (ps.XAxis2 != null)
                        {
                            int x1 = (int)Math.Min(end.X, start.X);
                            int x2 = (int)Math.Max(end.X, start.X);
                            int y = ps.PhysicalXAxis2Cache.PhysicalMax.Y;

                            xAxis2Min = ps.PhysicalXAxis2Cache.PhysicalToWorld(new Point(x1, y), true);
                            xAxis2Max = ps.PhysicalXAxis2Cache.PhysicalToWorld(new Point(x2, y), true);
							if (xAxis2Max - xAxis2Min < smallestAllowedRange_)
							{
								return false;
							}
                        }

						// now actually update the world limits.
						if (ps.XAxis1 != null)
						{
							ps.XAxis1.WorldMax = xAxis1Max;
							ps.XAxis1.WorldMin = xAxis1Min;
						}

						if (ps.XAxis2 != null)
						{
							ps.XAxis2.WorldMax = xAxis2Max;
							ps.XAxis2.WorldMin = xAxis2Min;
						}

                        wps.InteractionOccured(this);

                        return true;
                    }

					return false;
                }
                
                
                /// <summary>
                /// Handler for this interaction if a paint event is received.
                /// This draws the selection rectangle onto the plot,
                /// using the overlay.Extent dimensions
                /// </summary>
                /// <param name="pe">paint event args</param>
                /// <param name="width"></param>
                /// <param name="height"></param>
                public override void DoPaint(PaintEventArgs pe, int width, int height)
                {
	               	Graphics g = pe.Graphics;
                	
	               	// use 50% opacity for overlay area
	               	Color fillColor = Color.FromArgb(0x80, Color.White);
                	Brush fillBrush = new SolidBrush (fillColor);
					Rectangle rect = selection.Extent;

                	g.FillRectangle(fillBrush, rect);
 					fillBrush.Dispose();
                	
                }

                 

            }
            #endregion
            #region AxisDrag
            /// <summary>
            /// 
            /// </summary>
            public class AxisDrag : Interaction
            {

                /// <summary>
                /// 
                /// </summary>
                /// <param name="enableDragWithCtr"></param>
                public AxisDrag(bool enableDragWithCtr)
                {
                    enableDragWithCtr_ = enableDragWithCtr;
                }

                private bool enableDragWithCtr_ = false;
                private Axis axis_ = null;
                private bool doing_ = false;
                private Point lastPoint_ = new Point();
                private PhysicalAxis physicalAxis_ = null;
                private Point startPoint_ = new Point();


                /// <summary>
                /// 
                /// </summary>
                /// <param name="e"></param>
                /// <param name="ctr"></param>
                public override bool DoMouseDown(MouseEventArgs e, Control ctr)
                {
                    // if the mouse is inside the plot area [the tick marks are here and part of the 
                    // axis], then don't invoke drag. 
                    NPlot.PlotSurface2D ps = ((Windows.PlotSurface2D)ctr).Inner;
                    if (e.X > ps.PlotAreaBoundingBoxCache.Left && e.X < ps.PlotAreaBoundingBoxCache.Right &&
                        e.Y > ps.PlotAreaBoundingBoxCache.Top && e.Y < ps.PlotAreaBoundingBoxCache.Bottom)
                    {
                        return false;
                    }

                    if ((e.Button == MouseButtons.Left))
                    {
                        // see if hit with axis.
                        ArrayList objects = ps.HitTest(new Point(e.X, e.Y));

                        foreach (object o in objects)
                        {
                            if (o is NPlot.Axis)
                            {
                                doing_ = true;
                                axis_ = (Axis)o;

                                PhysicalAxis[] physicalAxisList = new PhysicalAxis[] { ps.PhysicalXAxis1Cache, ps.PhysicalXAxis2Cache, ps.PhysicalYAxis1Cache, ps.PhysicalYAxis2Cache };

                                if (ps.PhysicalXAxis1Cache.Axis == axis_)
                                {
                                    physicalAxis_ = ps.PhysicalXAxis1Cache;
                                }
                                else if (ps.PhysicalXAxis2Cache.Axis == axis_)
                                {
                                    physicalAxis_ = ps.PhysicalXAxis2Cache;
                                }
                                else if (ps.PhysicalYAxis1Cache.Axis == axis_)
                                {
                                    physicalAxis_ = ps.PhysicalYAxis1Cache;
                                }
                                else if (ps.PhysicalYAxis2Cache.Axis == axis_)
                                {
                                    physicalAxis_ = ps.PhysicalYAxis2Cache;
                                }

                                lastPoint_ = startPoint_ = new Point(e.X, e.Y);

                                return false;
                            }
                        }
                    }

					return false;
                }


                /// <summary>
                /// 
                /// </summary>
                /// <param name="e"></param>
                /// <param name="ctr"></param>
                /// <param name="lastKeyEventArgs"></param>
                public override bool DoMouseMove(MouseEventArgs e, Control ctr, KeyEventArgs lastKeyEventArgs)
                {
                    NPlot.PlotSurface2D ps = ((Windows.PlotSurface2D)ctr).Inner;

                    // if dragging on axis to zoom.
					if ((e.Button == MouseButtons.Left) && doing_ && physicalAxis_ != null)
                    {
						if (enableDragWithCtr_ && lastKeyEventArgs != null && lastKeyEventArgs.Control)
						{
						}
						else
						{
							float dist =
								(e.X - lastPoint_.X) +
								(-e.Y + lastPoint_.Y);

							lastPoint_ = new Point(e.X, e.Y);

							if (dist > sensitivity_ / 3.0f)
							{
								dist = sensitivity_ / 3.0f;
							}

							PointF pMin = physicalAxis_.PhysicalMin;
							PointF pMax = physicalAxis_.PhysicalMax;
							double physicalWorldLength = Math.Sqrt((pMax.X - pMin.X) * (pMax.X - pMin.X) + (pMax.Y - pMin.Y) * (pMax.Y - pMin.Y));

							float prop = (float)(physicalWorldLength * dist / sensitivity_);
							prop *= 2;

							((Windows.PlotSurface2D)ctr).CacheAxes();

							float relativePosX = (startPoint_.X - pMin.X) / (pMax.X - pMin.X);
							float relativePosY = (startPoint_.Y - pMin.Y) / (pMax.Y - pMin.Y);

                            if (float.IsInfinity(relativePosX) || float.IsNaN(relativePosX))
                            {
                                relativePosX = 0.0f;
                            }

                            if (float.IsInfinity(relativePosY) || float.IsNaN(relativePosY))
                            {
                                relativePosY = 0.0f;
                            }

							PointF physicalWorldMin = pMin;
							PointF physicalWorldMax = pMax;

							physicalWorldMin.X += relativePosX * prop;
							physicalWorldMax.X -= (1 - relativePosX) * prop;
							physicalWorldMin.Y -= relativePosY * prop;
							physicalWorldMax.Y += (1 - relativePosY) * prop;
							
							double newWorldMin = axis_.PhysicalToWorld(physicalWorldMin, pMin, pMax, false);
							double newWorldMax = axis_.PhysicalToWorld(physicalWorldMax, pMin, pMax, false);
							axis_.WorldMin = newWorldMin;
							axis_.WorldMax = newWorldMax;
							
							((Windows.PlotSurface2D)ctr).InteractionOccured(this);
							
							return true;
						}
                    }
				
					return false;
                }


                /// <summary>
                /// 
                /// </summary>
                /// <param name="e"></param>
                /// <param name="ctr"></param>
                public override bool DoMouseUp(MouseEventArgs e, Control ctr)
                {
                    if (doing_)
                    {
                        doing_ = false;
                        axis_ = null;
						physicalAxis_ = null;
                        lastPoint_ = new Point();
                    }

					return false;
                }
                private float sensitivity_ = 200.0f;
                

                /// <summary>
                /// 
                /// </summary>
                /// <value></value>
                public float Sensitivity
                {
                    get
                    {
                        return sensitivity_;
                    }
                    set
                    {
                        sensitivity_ = value;
                    }
                }
            }
            #endregion
            #region MouseWheelZoom
            /// <summary>
            /// 
            /// </summary>
            public class MouseWheelZoom : Interaction
            {

                private Point point_ = new Point(-1, -1);
                //private bool mouseDown_ = false;


                /// <summary>
                /// 
                /// </summary>
                /// <param name="e"></param>
                /// <param name="ctr"></param>
                public override bool DoMouseUp(MouseEventArgs e, Control ctr)
                {
                    //mouseDown_ = false;
					return false;
                }


                /// <summary>
                /// 
                /// </summary>
                /// <param name="e"></param>
                /// <param name="ctr"></param>
                public override bool DoMouseDown(MouseEventArgs e, Control ctr)
                {
                    //NPlot.PlotSurface2D ps = ((Windows.PlotSurface2D)ctr).Inner;

                    //if (e.X > ps.PlotAreaBoundingBoxCache.Left && e.X < ps.PlotAreaBoundingBoxCache.Right &&
                    //    e.Y > ps.PlotAreaBoundingBoxCache.Top && e.Y < ps.PlotAreaBoundingBoxCache.Bottom)
                    //{
                    //    point_.X = e.X;
                    //    point_.Y = e.Y;
                    //    mouseDown_ = true;
                    //}
					return false;
                }


                /// <summary>
                /// 
                /// </summary>
                /// <param name="e"></param>
                /// <param name="ctr"></param>
				public override bool DoMouseWheel(MouseEventArgs e, Control ctr)
				{
					NPlot.PlotSurface2D ps = ((Windows.PlotSurface2D)ctr).Inner;

					((Windows.PlotSurface2D)ctr).CacheAxes();

#if API_1_1
                    float delta = (float)e.Delta / (float)e.Delta;
#else
					float delta = (float)e.Delta / (float)SystemInformation.MouseWheelScrollDelta;
#endif
                    delta *= sensitivity_;
					Axis axis = null;
					PointF pMin = PointF.Empty;
					PointF pMax = PointF.Empty;
					KeyEventArgs keyArgs = ((Windows.PlotSurface2D)ctr).lastKeyEventArgs_;
					bool zoom = (keyArgs != null && keyArgs.Control);

					if (keyArgs != null && keyArgs.Shift)     
					{
						axis = ps.YAxis1;
						pMin = ps.PhysicalYAxis1Cache.PhysicalMin;
						pMax = ps.PhysicalYAxis1Cache.PhysicalMax;
					}
					else            
					{
						axis = ps.XAxis1;
						pMin = ps.PhysicalXAxis1Cache.PhysicalMin;
						pMax = ps.PhysicalXAxis1Cache.PhysicalMax;
					}
					if (axis == null) return false;
					
					PointF physicalWorldMin = pMin;
					PointF physicalWorldMax = pMax;
					physicalWorldMin.X -= delta;
					physicalWorldMax.X -= (zoom) ? -delta : delta;
					physicalWorldMin.Y += delta;
					physicalWorldMax.Y += (zoom) ? -delta : delta;
					double newWorldMin = axis.PhysicalToWorld(physicalWorldMin, pMin, pMax, false);
					double newWorldMax = axis.PhysicalToWorld(physicalWorldMax, pMin, pMax, false);
					axis.WorldMin = newWorldMin;
					axis.WorldMax = newWorldMax;
					
					((Windows.PlotSurface2D)ctr).InteractionOccured(this);
				
					return true;
                }


				/// <summary>
				/// Number of screen pixels equivalent to one wheel step.
				/// </summary>
				public float Sensitivity			                 
				{
					get				                     
					{
						return sensitivity_;
					}
					set			                     
					{
						sensitivity_ = value;
					}
				}
	            private float sensitivity_ = 60.0f;

            }
            #endregion
           
      	} // Abstract class Interactions
                    
      	#region Overlay drawing

		//
		// Overlay drawing is added to the double-buffered display surface after the main plot
		// has been copied from the off-screen cache in the Expose event.  This allows an overlay
		// to be erased by restoring the original (underlying) plot from the offscreen cache. The
		// Overlay is initialised when StartPoint is set, and updated as EndPoint is changed. Its
		// TopLeft and BottomRight corners are available (read-only), as is its bounding rectangle
		// which can be used to invalidate the relevant region of the display.
		// Drawing is by System.Drawing.Graphics classes, and is done in the Expose (Paint) event,
		// which provides a Graphics context from the Event arguments
		//

      	public class Overlay
      	{
       		/// <summary>
       		/// Create an overlay with identical start and end points
       		/// </summary>
       		/// <param name="X"></param>
       		/// <param name="Y"></param>
     		public Overlay ( int X, int Y )
      		{
      			StartPoint = new Point (X,Y);	// also sets EndPoint
      			extent = new Rectangle ( X, Y, 0,0 );
      		}
      		
      		
      		/// <summary>
      		/// Set startPoint for an overlay line or rectangle.  Write-only
      		/// Also sets EndPoint to same value, clearing overlay
      		/// </summary>
      		public Point StartPoint {
     			set {
      				startPoint = value;
      				endPoint = startPoint;
      			}
      		}
      		private Point startPoint = new Point(0,0);
 
      		
      		/// <summary>
      		/// end Point for an overlay line or rectangle. Write-only
      		/// </summary>
     		public Point EndPoint {
      			set {endPoint = value; }
       		}
      		private Point endPoint = new Point(0,0);
  
      		
      		/// <summary>
      		/// Normalised TopLeft of overlay (read-only)
      		/// </summary>
      		public Point TopLeft {
      			get {
      				int X = Math.Min(startPoint.X, endPoint.X);
      				int Y = Math.Min(startPoint.Y, endPoint.Y);
      				return new Point(X,Y);
       			}
      		}
      		
      		
       		/// <summary>
      		/// Normalised BottomRight of overlay (read-only)
      		/// </summary>
      		public Point BottomRight {
      			get {
     				int X = Math.Max(startPoint.X, endPoint.X);
      				int Y = Math.Max(startPoint.Y, endPoint.Y);
      				return new Point(X,Y);
      			}
      		}
  
 
      		
      		/// <summary>
      		/// Normalised Rectangle (ie topLeft <= bottomRight) enclosing the overlay
      		/// Read-only, as overlay is updated by setting StartPoint and EndPoint
      		/// </summary>
      		public Rectangle Extent {
      			get { 
      				Point tl = startPoint, br = endPoint;
      				if (tl.X > br.X)
      				{
      					tl.X = endPoint.X;
      					br.X = startPoint.X;
      				}
      				if (tl.Y > br.Y)
      				{
      					tl.Y = endPoint.Y;
      					br.Y = startPoint.Y;
      				}
 	      			int w = br.X - tl.X + 1;
    	  			int h = br.Y - tl.Y + 1;
      				return new Rectangle(tl.X, tl.Y, w, h);
      			}
     		}
      		private Rectangle extent = new Rectangle(0,0,0,0);
      		
      		
      		/*
      		/// <summary>
      		/// Returns true if startPoint and endPoint have different X or Y values
       		/// </summary>
       		/// <remarks>
       		/// This cannot be determined from Width or Height of Extent, since a
       		/// Rectangle with identical start and end points has width/height of 1
       		/// </remarks>
       		public bool NonZero {
       			get {
       				return (startPoint.X != endPoint.X) || (startPoint.Y != endPoint.Y) ;
       			}
       		}
      		
       		*/
       		
      		/// <summary>
      		/// Clears overlay by setting endPoint to startPoint
      		/// </summary>
      		public void Clear ()
      		{
      			endPoint = startPoint;
      		}
      		
      	}
      	
 	            
        #endregion
        
        private ArrayList interactions_ = new ArrayList();

        /// <summary>
        /// Adds and interaction to the plotsurface that adds functionality that responds 
        /// to a set of mouse / keyboard events. 
        /// </summary>
        /// <param name="i">the interaction to add.</param>
        public void AddInteraction(Interactions.Interaction i)
        {
            interactions_.Add(i);
        }


		/// <summary>
		/// Remove a previously added interaction
		/// </summary>
		/// <param name="i">interaction to remove</param>
		public void RemoveInteraction(Interactions.Interaction i)             
		{
			interactions_.Remove(i);
		}


        /// <summary>
        /// This is the signature of the function used for InteractionOccurred events.
        /// 
        /// TODO: expand this to include information about the event. 
        /// </summary>
        /// <param name="sender"></param>
        public delegate void InteractionHandler(object sender);
        

        /// <summary>
        /// Event is fired when an interaction happens with the plot that causes it to be modified.
        /// </summary>
        public event InteractionHandler InteractionOccured;


        /// <summary>
        /// Default function called when plotsurface modifying interaction occured. 
        /// 
        /// Override this, or add method to InteractionOccured event.
        /// </summary>
        /// <param name="sender"></param>
        protected void OnInteractionOccured(object sender)
        {
            // do nothing.
        }


        /// <summary>
        /// This is the signature of the function used for PreRefresh events.
        /// </summary>
        /// <param name="sender"></param>
        public delegate void PreRefreshHandler(object sender);


        /// <summary>
        /// Event fired when we are about to paint.
        /// </summary>
        public event PreRefreshHandler PreRefresh;


        /// <summary>
        /// Default function called just before a refresh happens.
        /// </summary>
        /// <param name="sender"></param>
        protected void OnPreRefresh(object sender)
        {
            // do nothing.
        }


		#region class PlotContextMenu
		/// <summary>
		/// Summary description for ContextMenu.
		/// </summary>
		public class PlotContextMenu
		{

			#region IPlotMenuItem
			/// <summary>
			/// elements of the MenuItems array list must implement this interface.
			/// </summary>
			public interface IPlotMenuItem
			{
				/// <summary>
				/// Gets the Windows.Forms.MenuItem associated with the PlotMenuItem
				/// </summary>
				System.Windows.Forms.MenuItem MenuItem { get; }

				/// <summary>
				/// This method is called for each menu item before the menu is 
				/// displayed. It is useful for implementing check marks, disabling
				/// etc.
				/// </summary>
				/// <param name="plotContextMenu"></param>
				void OnPopup( PlotContextMenu plotContextMenu );
			}
			#endregion
			#region PlotMenuSeparator
			/// <summary>
			/// A plot menu item for separators.
			/// </summary>
			public class PlotMenuSeparator : IPlotMenuItem
			{

				/// <summary>
				/// Constructor
				/// </summary>
				/// <param name="index"></param>
				public PlotMenuSeparator( int index )
				{
					menuItem_ = new System.Windows.Forms.MenuItem();
					index_ = index;

					menuItem_.Index = index_;
					menuItem_.Text = "-";
				}

				private int index_;

				/// <summary>
				/// Index of this menu item in the menu.
				/// </summary>
				public int Index
				{
					get
					{
						return index_;
					}
				}

				private System.Windows.Forms.MenuItem menuItem_;
				/// <summary>
				/// The Windows.Forms.MenuItem associated with this IPlotMenuItem
				/// </summary>
				public System.Windows.Forms.MenuItem MenuItem
				{
					get
					{
						return menuItem_;
					}
				}

				/// <summary>
				/// 
				/// </summary>
				/// <param name="plotContextMenu"></param>
				public void OnPopup( PlotContextMenu plotContextMenu )
				{
					// do nothing.
				}

			}
			#endregion
			#region PlotMenuItem
			/// <summary>
			/// A Plot menu item suitable for specifying basic menu items
			/// </summary>
			public class PlotMenuItem : IPlotMenuItem
			{

				/// <summary>
				/// Constructor
				/// </summary>
				/// <param name="text">Menu item text</param>
				/// <param name="index">Index in the manu</param>
				/// <param name="callback">EventHandler to call if menu selected.</param>
				public PlotMenuItem( string text, int index, EventHandler callback )
				{
					text_ = text;
					index_ = index;
					callback_ = callback;

					menuItem_ = new System.Windows.Forms.MenuItem();
					
					menuItem_.Index = index;
					menuItem_.Text = text;
					menuItem_.Click += new System.EventHandler(callback);

				}

				
				/// <summary>
				/// The text to put in the menu for this menu item.
				/// </summary>
				public string Text
				{
					get
					{
						return text_;
					}
				}
                private string text_;


				/// <summary>
				/// Index of this menu item in the menu.
				/// </summary>
				public int Index
				{
					get
					{
						return index_;
					}
				}
                private int index_;


				/// <summary>
				/// EventHandler to call if menu selected.
				/// </summary>
				public EventHandler Callback
				{
					get
					{
						return callback_;
					}
				}
                private EventHandler callback_;

				
				/// <summary>
				/// The Windows.Forms.MenuItem associated with this IPlotMenuItem
				/// </summary>
				public System.Windows.Forms.MenuItem MenuItem
				{
					get
					{
						return menuItem_;
					}
				}
                private System.Windows.Forms.MenuItem menuItem_;


				/// <summary>
				/// Called before menu drawn.
				/// </summary>
				/// <param name="plotContextMenu">The plot menu this item is a member of.</param>
				public virtual void OnPopup( PlotContextMenu plotContextMenu )
				{
					// do nothing.
				}
			}
			#endregion
			#region PlotZoomBackMenuItem
			/// <summary>
			/// A Plot Menu Item that provides necessary functionality for the
			/// zoom back menu item (graying out if zoomed right out in addition
			/// to basic functionality).
			/// </summary>
			public class PlotZoomBackMenuItem : PlotMenuItem
			{

				/// <summary>
				/// Constructor
				/// </summary>
				/// <param name="text">Text associated with this item in the menu.</param>
				/// <param name="index">Index of this item in the menu.</param>
				/// <param name="callback">EventHandler to call when menu item is selected.</param>
				public PlotZoomBackMenuItem( string text, int index, EventHandler callback )
					: base( text, index, callback )
				{
				}

				/// <summary>
				/// Called before menu drawn.
				/// </summary>
				/// <param name="plotContextMenu">The plot menu this item is a member of.</param>
				public override void OnPopup( PlotContextMenu plotContextMenu )
				{
					this.MenuItem.Enabled = plotContextMenu.plotSurface2D_.xAxis1ZoomCache_ != null;
				}

			}
			#endregion
			#region PlotShowCoordinatesMenuItem
			/// <summary>
			/// A Plot Menu Item that provides necessary functionality for the
			/// show coordinates menu item (tick mark toggle in addition to basic
			/// functionality).
			/// </summary>
			public class PlotShowCoordinatesMenuItem : PlotMenuItem
			{

				/// <summary>
				/// Constructor
				/// </summary>
				/// <param name="text">Text associated with this item in the menu.</param>
				/// <param name="index">Index of this item in the menu.</param>
				/// <param name="callback">EventHandler to call when menu item is selected.</param>
				public PlotShowCoordinatesMenuItem( string text, int index, EventHandler callback )
					: base( text, index, callback )
				{
				}

				/// <summary>
				/// Called before menu drawn.
				/// </summary>
				/// <param name="plotContextMenu">The plot menu this item is a member of.</param>
				public override void OnPopup( PlotContextMenu plotContextMenu )
				{
					this.MenuItem.Checked = plotContextMenu.plotSurface2D_.ShowCoordinates;
				}
			}
			#endregion

			private System.Windows.Forms.ContextMenu rightMenu_ = null;
			private ArrayList menuItems_ = null;


			/// <summary>
			/// Gets an arraylist of all PlotMenuItems that comprise the
			/// menu. If this list is changed, this class must be told to
			/// update using the Update method.
			/// </summary>
			public ArrayList MenuItems
			{
				get
				{
					return menuItems_;
				}
			}

			/// <summary>
			/// The PlotSurface2D associated with the context menu. Generally, the user
			/// should not set this. It is used internally by PlotSurface2D.
			/// </summary>
			public Windows.PlotSurface2D PlotSurface2D
			{
				set
				{
					this.plotSurface2D_ = value;
				}
			}

			/// <summary>
			/// The PlotSurface2D associated with the context menu. Classes inherited
			/// from PlotContextMenu will likely use this to implement their functionality.
			/// </summary>
			protected Windows.PlotSurface2D plotSurface2D_;

			
			/// <summary>
			/// Sets the context menu according to the IPlotMenuItem's in the provided
			/// ArrayList. The current menu items can be obtained using the MenuItems
			/// property and extended if desired.
			/// </summary>
			/// <param name="menuItems"></param>
			public void SetMenuItems(ArrayList menuItems)
			{
				this.menuItems_ = menuItems;

				this.rightMenu_ = new System.Windows.Forms.ContextMenu();
			
				foreach (IPlotMenuItem item in menuItems_)
				{
					this.rightMenu_.MenuItems.Add( item.MenuItem );
				}

				this.rightMenu_.Popup += new System.EventHandler(this.rightMenu__Popup);
			}


			/// <summary>
			/// Constructor creates
			/// </summary>
			public PlotContextMenu()
			{
				ArrayList menuItems = new ArrayList();

				menuItems = new ArrayList();
				menuItems.Add( new PlotZoomBackMenuItem( "Original Dimensions", 0, new EventHandler(this.mnuOriginalDimensions_Click) ) );
				menuItems.Add( new PlotShowCoordinatesMenuItem( "Show World Coordinates", 1, new EventHandler(this.mnuDisplayCoordinates_Click) ) ); 
				menuItems.Add( new PlotMenuSeparator(2) );
				menuItems.Add( new PlotMenuItem( "Print", 3, new EventHandler(this.mnuPrint_Click )) );
				menuItems.Add( new PlotMenuItem( "Print Preview", 4, new EventHandler(this.mnuPrintPreview_Click) ) );
				menuItems.Add( new PlotMenuItem( "Copy To Clipboard", 5, new EventHandler(this.mnuCopyToClipboard_Click) ) );
				menuItems.Add( new PlotMenuItem( "Copy Data To Clipboard", 6, new EventHandler(this.mnuCopyDataToClipboard_Click) ) );

				this.SetMenuItems( menuItems );
			}


			private void mnuOriginalDimensions_Click(object sender, System.EventArgs e)
			{
				plotSurface2D_.OriginalDimensions();
			}

			private void mnuCopyToClipboard_Click(object sender, System.EventArgs e) 
			{
				plotSurface2D_.CopyToClipboard();
			}


			private void mnuCopyDataToClipboard_Click(object sender, System.EventArgs e) 
			{
				plotSurface2D_.CopyDataToClipboard();
			}


			private void mnuPrint_Click(object sender, System.EventArgs e) 
			{
				plotSurface2D_.Print( false );
			}


			private void mnuPrintPreview_Click(object sender, System.EventArgs e) 
			{
				plotSurface2D_.Print( true );
			}


			private void mnuDisplayCoordinates_Click(object sender, System.EventArgs e)
			{
				plotSurface2D_.ShowCoordinates = !plotSurface2D_.ShowCoordinates;
			}


			private void rightMenu__Popup(object sender, System.EventArgs e)
			{
				foreach (IPlotMenuItem item in menuItems_)
				{
					item.OnPopup( this );
				}
			}

			/// <summary>
			/// Gets the Windows.Forms context menu managed by this object.
			/// </summary>
			public System.Windows.Forms.ContextMenu Menu
			{
				get
				{
					return rightMenu_;
				}
			}

		}
		#endregion

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if( components != null )
					components.Dispose();
			}
			base.Dispose( disposing );
		}

		private System.ComponentModel.IContainer components;
	}
}
