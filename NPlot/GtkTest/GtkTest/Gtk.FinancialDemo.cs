/*
 *NPlot - A charting library for .NET
 * 
 * Gtk.FinancialDemo.cs
 * 
 * Copyright (C) 2003-2009 Matt Howlett and others.
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
using System.Collections;
using System.ComponentModel;
using Gtk;
using System.Data;
using System.Reflection;

using NPlot;


namespace NPlotDemo
{
	/// <summary>
	/// Summary description for Financial Demo.
	/// </summary>
	public class FinancialDemo : Gtk.Window
	{
        private NPlot.Gtk.InteractivePlotSurface2D volumePS;
        private NPlot.Gtk.InteractivePlotSurface2D costPS;
        private Gtk.Button closeButton;

        public FinancialDemo() : base ( "Multiple linked plot demo" )
        {
			//
			// Gtk Window Setup
			//
			InitializeComponent();

            costPS.Clear();
  
            // obtain stock information from xml file
            DataSet ds = new DataSet();
            System.IO.Stream file =
                Assembly.GetExecutingAssembly().GetManifestResourceStream("GtkTest.Resources.asx_jbh.xml");
            ds.ReadXml(file, System.Data.XmlReadMode.ReadSchema);
            DataTable dt = ds.Tables[0];
            // DataView dv = new DataView(dt);

            // create CandlePlot.
            CandlePlot cp = new CandlePlot();
            cp.DataSource = dt;
            cp.AbscissaData = "Date";
            cp.OpenData = "Open";
            cp.LowData = "Low";
            cp.HighData = "High";
            cp.CloseData = "Close";
            cp.BearishColor = Color.Red;
            cp.BullishColor = Color.Green;
            cp.Style = CandlePlot.Styles.Filled;
            costPS.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            costPS.Add(new Grid());
            costPS.Add(cp);
            costPS.Title = "AU:JBH";
            costPS.YAxis1.Label = "Price [$]";
            costPS.YAxis1.LabelOffset = 40;
            costPS.YAxis1.LabelOffsetAbsolute = true;
            costPS.XAxis1.HideTickText = true;
            costPS.SurfacePadding = 5;
 
            costPS.AddInteraction(new NPlot.PlotDrag(true,true));
            costPS.AddInteraction(new NPlot.AxisDrag());
            costPS.InteractionOccurred += new NPlot.InteractivePlotSurface2D.InteractionHandler(costPS_InteractionOccurred);
            costPS.AddAxesConstraint(new AxesConstraint.Position(YAxisPosition.Left, 60));

            costPS.Refresh();
            
            PointPlot pp = new PointPlot();
            pp.Marker = new Marker(Marker.MarkerType.Square, 0);
            pp.Marker.Pen = new Pen(Color.Red, 5.0f);
            pp.Marker.DropLine = true;
            pp.DataSource = dt;
            pp.AbscissaData = "Date";
            pp.OrdinateData = "Volume";  
            volumePS.Add(pp);
            volumePS.YAxis1.Label = "Volume";
            volumePS.YAxis1.LabelOffsetAbsolute = true;
            volumePS.YAxis1.LabelOffset = 40;
            volumePS.SurfacePadding = 5;
            
            volumePS.AddAxesConstraint(new AxesConstraint.Position(YAxisPosition.Left, 60));
            volumePS.AddInteraction(new NPlot.AxisDrag());
            volumePS.AddInteraction(new NPlot.PlotDrag(true,false));
            volumePS.InteractionOccurred += new NPlot.InteractivePlotSurface2D.InteractionHandler(volumePS_InteractionOccurred);
 	        volumePS.PreRefresh += new NPlot.InteractivePlotSurface2D.PreRefreshHandler(volumePS_PreRefresh);
		
 	        volumePS.Refresh();
        }

		#region Gtk Window Setup code
		
		private void InitializeComponent()
		{
            
			// 
			// closeButton
			// 
            this.closeButton = new Gtk.Button("Close");
            this.closeButton.Clicked += new System.EventHandler(this.closeButton_Click);

            // 
			// volumePS
			// 
            this.volumePS = new NPlot.Gtk.InteractivePlotSurface2D();
            this.volumePS.AutoScaleAutoGeneratedAxes = false;
            this.volumePS.AutoScaleTitle = false;
            this.volumePS.Canvas.ModifyBg (StateType.Normal);
            this.volumePS.Legend = null;
            this.volumePS.Canvas.Name = "volumePS";
            // HWT this.volumePS.RightMenu = null;
            // HWT this.volumePS.ShowCoordinates = false;
            this.volumePS.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            this.volumePS.Title = "";
            this.volumePS.TitleFont = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.volumePS.XAxis1 = null;
            this.volumePS.XAxis2 = null;
            this.volumePS.YAxis1 = null;
            this.volumePS.YAxis2 = null;

            // 
			// costPS
			// 
            this.costPS = new NPlot.Gtk.InteractivePlotSurface2D();
            this.costPS.AutoScaleAutoGeneratedAxes = false;
            this.costPS.AutoScaleTitle = false;
            this.costPS.Canvas.ModifyBg (StateType.Normal);
            this.costPS.Legend = null;
            this.costPS.Canvas.Name = "costPS";
            // HWT this.costPS.RightMenu = null;
            // HWT this.costPS.ShowCoordinates = false;
            this.costPS.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            this.costPS.Title = "";
            this.costPS.TitleFont = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.costPS.XAxis1 = null;
            this.costPS.XAxis2 = null;
            this.costPS.YAxis1 = null;
            this.costPS.YAxis2 = null;

            // 
			// FinancialDemo
			// 
			this.SetSizeRequest (630, 450);
            //
			// Define a 10x10 table on which to lay out the plots and button
			//
			Gtk.Table layout = new Gtk.Table ( 10, 10, true );
			layout.BorderWidth = 4;
			Add( layout );

			AttachOptions opt = AttachOptions.Expand | AttachOptions.Fill;
            uint xpad = 2, ypad = 10;
			
			layout.Attach (costPS.Canvas, 0, 10, 0, 6);
			layout.Attach (volumePS.Canvas, 0, 10, 6, 9);
            layout.Attach (closeButton, 1,2, 9,10, opt,opt, xpad,ypad );
            this.Name = "PlotSurface2DDemo";

            this.Name = "FinancialDemo";

        }
		#endregion


        /// <summary>
        /// Callback for close button.
        /// </summary>
		private void closeButton_Click(object sender, System.EventArgs e)
		{
			this.Destroy();
		}


        /// <summary>
        /// When the costPS chart has changed, this is called which updates the volumePS chart.
        /// </summary>
        /// <param name="sender"></param>
        private void costPS_InteractionOccurred(object sender)
        {
            DateTimeAxis axis = new DateTimeAxis(costPS.XAxis1);
            axis.Label = "Date / Time";
            axis.HideTickText = false;
            this.volumePS.XAxis1 = axis;
            this.volumePS.Refresh();
        }


        /// <summary>
        /// When the volumePS chart has changed, this is called which updates the costPS chart.
        /// </summary>
        private void volumePS_InteractionOccurred(object sender)
        {
            DateTimeAxis axis = new DateTimeAxis(volumePS.XAxis1);
            axis.Label = "";
            axis.HideTickText = true;
            this.costPS.XAxis1 = axis;
            this.costPS.Refresh();
        }


        /// <summary>
        /// This is called prior to volumePS refresh to enforce the WorldMin is 0. 
        /// This may have been changed by the axisdrag interaction.
        /// </summary>
        /// <param name="sender"></param>
        
        private void volumePS_PreRefresh(object sender)
        {
           volumePS.YAxis1.WorldMin = 0.0;
        }
        
    }
}
