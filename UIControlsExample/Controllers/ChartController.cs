//using Tracker.DataLayer;
//using Microsoft.WindowsAzure.Storage;
//using Microsoft.WindowsAzure.Storage.Table;
//using MVCChartHelperExample.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
//using static ActivityTracker.Activity;
using System.Drawing;
using System.Web.UI.DataVisualization.Charting;
using System.Text;
using System.Xml;
using UIControlsExample.Models;

//using System.Web.Helpers;

namespace MoniMe.Controllers
{
    public class ChartController : Controller
    {
        public static UINode currentClient = new UINode();
        
        private UIContext db = new UIContext();

        // GET: Chart
        public ActionResult Index()
        {
            return PartialView();
        }

        public ActionResult BoxPlot()
        {
            CultureInfo cultureinfo = CultureInfo.CreateSpecificCulture("en-IE");
            List<UINode> nodeActivities = db.SampleNodes.ToList();
            List<DateTime> dateVals = nodeActivities.Select(s => (DateTime)s.FromDate)
                .OrderBy(o => o.ToOADate()).ToList();

            if (dateVals.Count() < 1) return null;
            DateTime first = dateVals.Min();
            DateTime last = dateVals.Max();
            int noOfDays = Math.Abs((last - first).Days);
            int noOfHours = Math.Abs((last - first).Hours);
            List<double> tickDiff = new List<double>();

            for (int i = 0; i < dateVals.Count; i++)
            {
                tickDiff.Add(Math.Abs(dateVals[i].ToOADate()));
            }
            // Create a list of vlues to plot 
            List<int> HitVals = new List<int>();
             HitVals.AddRange(nodeActivities.Select( d =>d.NumberOfHits).ToList());
            var chart = new System.Web.Helpers.Chart(width: 800, height: 600, theme: GetTheme(noOfDays, noOfHours))
                .SetXAxis("Time between Hit sample", min: dateVals.Min().ToOADate(), max: dateVals.Max().ToOADate())
                .AddLegend("Legend")
                .AddSeries("No of Hits", chartType: "Point", xValue: dateVals, yValues: HitVals)
                .AddSeries("No of Hits over time", chartType: "FastLine", xValue: dateVals , yValues: HitVals)
                .Write("bmp");


            return null;


        }

        


        
        // Theme Code source https://truncatedcodr.wordpress.com/2012/09/18/system-web-helpers-chart-custom-themes/
        // Chart Library Reference https://docs.microsoft.com/en-us/dotnet/api/system.web.ui.datavisualization.charting.chart?view=netframework-4.7.2
        public static string GetTheme(int days, int hours)
        {
            //ChartArea ca = new System.Web.UI.DataVisualization.Charting.ChartArea("Default");
            var chart = new System.Web.UI.DataVisualization.Charting.Chart
            {
                BackColor = Color.Azure,
                BackGradientStyle = GradientStyle.TopBottom,
                BackSecondaryColor = Color.White,
                BorderColor = Color.FromArgb(26, 59, 105),
                BorderlineDashStyle = ChartDashStyle.Solid,
                BorderWidth = 2,
                Palette = ChartColorPalette.None,
                PaletteCustomColors = new Color[] { Color.Lime, Color.Red,
                        Color.Orange, Color.Yellow, Color.Green, Color.Blue, Color.Purple,
                         Color.Black }
            };
            chart.ChartAreas.Add(new ChartArea("Chart Area 1")
            {
                //BackColor = Color.FromArgb(64, 165, 191, 228),
                BackColor = Color.Beige,
                BackGradientStyle = GradientStyle.TopBottom,
                BackSecondaryColor = Color.White,
                BorderColor = Color.FromArgb(64, 64, 64, 64),
                BorderDashStyle = ChartDashStyle.Solid,
                ShadowColor = Color.Transparent,
            });
            chart.Legends.Add(new Legend("Previous History of Hits")
            {
                BackColor = Color.Transparent,
                Font = new Font("Segoe UI Black", 8.25f, FontStyle.Bold,
                    GraphicsUnit.Point),
                IsTextAutoFit = true
            }
                );
            chart.BorderSkin.SkinStyle = BorderSkinStyle.Emboss;
            // Create Customise x Axis layout Scale break only works with BoxPlot chart type
            Axis X = new Axis()
            {
                ScaleBreakStyle = new AxisScaleBreakStyle
                {
                    Enabled = true,
                    CollapsibleSpaceThreshold = 10,
                    LineColor = Color.Red,
                    LineWidth = 2,
                    BreakLineStyle = BreakLineStyle.Ragged,
                    Spacing = 2,
                    LineDashStyle = ChartDashStyle.Dot
                },
                IsInterlaced = true,
                // change for Hours rather than Days
                IsLabelAutoFit = true, //Title = "Dates and Times",
                LabelStyle = new LabelStyle() { Format = "MM/dd/yyyy hh:mm:ss.ff" }
            };
            // Change to Hour time if the number of Days is less
            if (days < 2)
            {

                if (hours <= 2)
                {
                    X.Interval = 5;
                    X.IntervalType = DateTimeIntervalType.Minutes;
                }
                else
                {
                    X.Interval = 1;
                    X.IntervalType = DateTimeIntervalType.Hours;
                }

            }

            X.StripLines.Add(new StripLine
            {
                StripWidth = 0,
                BorderColor = Color.Black,
                BorderWidth = 3,
                Interval = 5,
                BackGradientStyle = GradientStyle.LeftRight
            });

            // Set Y axis
            chart.ChartAreas["Chart Area 1"].AxisX = X;
            //chart.ChartAreas["Chart Area 1"].AxisY.Interval = 1;
            //chart.ChartAreas["Chart Area 1"].AxisX.Interval = 1;
            var cs = chart.Serializer;
            cs.IsTemplateMode = true;
            cs.Content = SerializationContents.Default;
            cs.Format = SerializationFormat.Xml;
            var sb = new StringBuilder();
            using (XmlWriter xw = XmlWriter.Create(sb))
            {
                cs.Save(xw);
            }
            string theme = sb.ToString().Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "");
            // <?xml version="1.0" encoding="utf-16"?>
            //using (StringWriter writer = new StringWriter(sb))
            //{
            //    chart.Serializer.Content = SerializationContents.Default;
            //    chart.Serializer.Save(writer);
            //}

            return @theme;
        }
    }
}