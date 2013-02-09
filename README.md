NPlot
=====

NPlot is an open source charting library for .NET

Background
==========

NPlot is an open source charting library for the .NET framework. Its interface is elegent, simple to use and flexible.
The ability to quickly create charts makes NPlot an ideal tool for inspecting data in your software for debugging or
analysis purposes. On the other hand, the library's flexibility makes it a great choice for creating carefully tuned
charts for publications or as part of the interface to your Gtk#, Swf,  or Web application.

The current version of NPlot is hosted at SourceForge (http://sourceforge.net/projects/nplot/) and is available
as a zip file to download (nplot-0.9.10.0.zip)  The current svn revision is Rev 35, but there has been very little
activity since the project was transferred from its original developer (Matt Howlett) to SourceForge in 2006.
The current maintainers appear to have moved to new interests, and there has been no development for several years.

Library Overview
================

This overview describes the library as defined in the current version, and is taken from an article written by
Matt Howlett (http://netcontrols.org/nplot/downloads/nplot_introduction.pdf  - also available as an e-book).

Extensions and bug-fixes added in this repository are described in the Developments section at the end. 


The PlotSurface2D Class
-----------------------

This class implements the IPlotSurface2D interface, and its role is to coordinate the display of axes, title and
legend as well as all the data dependent elements of the chart. NPlot provides three such classes:

* Windows.PlotSurface2D - a System.Windows.Forms control that implements IPlotSurface2D. This class also encapsulates
  an interaction framework which specifies how an application user can interact with the chart.

* Bitmap.PlotSurface2D - This class allows you to easily draw charts on a System.Drawing.Bitmap object. This class is
  often used in web applications to generate dynamic charts and is also useful whilst debugging.

* Web.PlotSurface2D - This is an ASP.NET control that implements the IPlotSurface2D functionality. This control should
  be considered experimental.  In all but the smallest applications, you should implement dynamic charting on the web
  via the Bitmap.PlotSurface2D class. A PlotSurface2D class has also been written for an older version of NPlot which
  allows charts to be used in GTK# applications created with the free C# compiler / .NET implementation called Mono
  under Linux (but see Developments section).

Plot Elements
-------------

With a PlotSurface2D created, it is then necessary to create an instance of a class that implements the IPlot interface
(NPlot provides many of these classes). You then point this object to your data, optionally set a few display properties
and add it to PlotSurface2D.  Classes that implement IPlot include LinePlot, PointPlot and ImagePlot. The purpose of a
'plot' class is to wrap data and provide functionality for drawing it against a pair of axes.  The different plot
classes display data in different ways (e.g. as a series of points, or a line). Plot classes can also draw a
representation of themselves in a legend if present and can also 'suggest' to the PlotSurface2D the axes they would
optimally be drawn against.

NPlot also provides a lightweight IDrawable interface (which IPlot extends) and classes which implement IDrawable
can also be added to a PlotSurface2D.  These items cannot draw a representation of themselves in the legend or
influence the PlotSurface2D's selection of axes.

A selection of the commonly used classes that implement IDrawable are:
* ArrowItem - Draws an arrow pointing to a particular world coordinate.
* FilledRegion - Creates a filled area between two line plots.
* Grid - Adds grid lines to the plot surface which automatically align to axis tick positions.
* TextItem - Places text at a specific world coordinate.

A selection of the commonly used plot types are described below:

LinePlot
--------

Use a line plot when it makes sense to interpolate between successive data points. For example you may
have measurements of the ambient temperature of a room at various times throughout a day. If the  reading
was taken frequently, a line plot of temperature vs time would make sense.
Tip: You can create lines of any color, thickness or dash pattern by specifying the Pen used for drawing.

PointPlot
---------

Use a point plot (scatter chart) when it does not make sense to interpolate between successive data points.
For example you might want to visualize the height and weight of a group of people on a chart. You could plot
a point for each person with the x position determined by their weight and y position determined by their height.
Tip: Around 15 different pre-defined marker styles are available.

StepPlot
--------

Step plots are useful for displaying sample based data (such as PCM audio) where each value can be thought of as
representing the value of the measured quantity over a specific time period.
Tip: You can choose whether the horizontal sections of the step plot are centered on the abscissa values
     or drawn between successive abscissa values.

CandlePlot
----------

This type of plot is often used for displaying financial data. Each bar summarizes a price over a particular time period.
The lower and upper positions of the thin sticks indicate the highest and lowest values of the price over that period.
If the filled region is red, the top of the filled region represents the price at the beginning of the time period and
the bottom of the filled region the price at the end of the time period. If the filled region is green, the bottom of the
filled region is the opening price and the top is the closing price.
Tip: The candle fill colors are configurable. Also, this plot type can generate 'stick' plots (similar to candle plots).

BarPlot
-------

A bar plot is usually used to chart the number of data values belonging to one or more categories. The height of the bar
represents the number of values in the given category. For example, you might have a collection of dogs and data on the
breed of each. You could create a chart of the number of each type of breed.
Tips: You will often want to make the lower X axis a LabelAxis (in the above example, the names of the dog breeds).
      Also, Bar plots can be stacked on top of each other.

ImagePlot
---------

This type of chart is often used to display the variation of a value over a spatial area.
Each value in the region is mapped to a color.
Tip: You can specify the color to value mapping using an object of any class that implements IGradient (eg LinearGradient)
Of course, if the built in IPlot or IDrawable classes don't provide the functionality you require, it is straightforward
to create your own class that implements one of these interfaces. This is perhaps the most common way of extending NPlot.

You can add as many plots to a PlotSurface2D object as you like and can control the order in which they are drawn
with the Z order parameter of the Add method. Also, the PlotSurface2D classes define two independent X axes and two
independent Y axes. When you add an item, you can chose which X and Y axis you would like it to be associated with.

Specifying Data
---------------

The IPlot interface does not enforce how data associated with the specific plot classes should be specified. However,
where it makes sense, these classes provide a consistent interface for this purpose.
Data can be provided in one of two forms:
* In an object of type DataSet, DataTable or DataView from the System.Data namespace.
* In any collection that implements the IEnumerable interface where it is valid to cast each of the elements to type
  double. Examples of such collections are:-
    * Double[]
    * System.Collections.ArrayList
    * System.Collections.Generic.List<System.Int16>
If you are working with very large data sets and efficiency is of concern, you should prefer to pass your data to NPlot
via the built in array type double[], as this case has been highly optimized. The following four properties provided
by most plot classes are used to specify your data:

DataSource: If the data is to be taken from a DataSet, DataTable or DataView, this property should be set to that object.
DataMember: If the data is to be taken from a DataSet, this property should be set to a string containing the name of the
DataTable in the DataSet to take the data from.
AbscissaData: The x coordinates of the data to plot. This property is optional - if not specified (or set to null),
the abscissa data will be assumed to be 0, 1, 2, ... If data is being read from a DataTable or DataView, this should be
a string containing the name of the column to take the data from. Otherwise, this can be set to any container that
implements the IEnumerable interface.
OrdinateData: The y coordinates of the data to plot. If data is being read from a DataTable or DataView, this should be
a string containing the name of the column to take the data from. Otherwise, this can be set to any container that
implements the IEnumerable interface. Where the above interface is not suitable for a particular plot type, the
interface is as close to this as possible. For example CandlePlot provides OpenData, LowData, HighData and CloseData
properties instead of OrdinateData.

Axes
----

A PlotSurface2D object automatically determines axes suitable for displaying the plot objects that you add to it.
However, these are highly customizable. Some common things you might wish to add / adjust are:
* A label for the axis (using the Label property)
* Tick Text / Label fonts (using the LabelFont and TickTextFont properties)
* The angle of the text next to the ticks (using the TicksAngle property)
* The pen used to draw the axis (using the AxisPen property)
* World minimum and maximum values (using the WorldMin and WorldMax properties)

You can also replace the default axes with a completely different axis type. NPlot provides a number of different Axis
types shown below whose characteristics are individually configurable.
* LinearAxis
* LogAxis
* LabelAxis
* DateTimeAxis

Developments
============

The Gtk# control referred to in NPlot is written by Miguel de Icaza (http://tirania.org/blog//index.html) and is based
on an early version of NPlot.  The port was straightforward because all drawing in NPlot is by System.Drawing classes,
so that by providing a Gtk# implementation of PlotSurface2D, which could be added to any Gtk# container, all drawing
code within NPlot could be left unchanged.  However, this control was never extended to include the interactions added
by class Windows.PlotSurface2D, which were very specific to the System.Windows.Forms (Swf) implementation.

The extensions here remedy that, and provide full demonstrations of interactive plot surfaces that can be used equally
well in Windows or Gtk#, and which maximise the use of common code.

The intention is also to port the library to Xwt for a more cross-platform experience.  This will, however, require
porting all the drawing routines from System.Drawing to the more Cairo-like facilities of Xwt.

