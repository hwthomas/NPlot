using System;
using System.Drawing;
using System.IO;
using System.Reflection;

using Gtk;
using NPlot;

namespace GtkTest
{
    public class PlotSample
    {
        protected NPlot.Gtk.InteractivePlotSurface2D plotSurface;
        protected string infoText = "";

        public string InfoText
        {
            get { return infoText; }
        }

        public DrawingArea Canvas
        {
            get { return plotSurface.Canvas; }
        }

        public PlotSample()
        {
            plotSurface = new NPlot.Gtk.InteractivePlotSurface2D ();
        }
    }
}

