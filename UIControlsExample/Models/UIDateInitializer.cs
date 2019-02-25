using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web;

namespace UIControlsExample.Models
{
    public class UIDateInitializer : DropCreateDatabaseIfModelChanges<UIContext>
    {
        public UIDateInitializer()
        {

        }
        protected override void Seed(UIContext context)
        {

            List<UINode> nodes = new List<UINode>();
            DateTime StartDate = new DateTime(2016, 1, 1);
            int range = (DateTime.Now - StartDate).Days;
            Random r = new Random();
            CultureInfo cultureinfo = CultureInfo.CreateSpecificCulture("en-IE");
            for (int i = 0; i < 20; i++)
            {
                nodes.Add(new UINode
                {
                    FromDate = DateTime.ParseExact(DateTime.Now.AddDays(-r.Next(range - 1)).ToString("dd/MM/yyyy HH:mm"), "dd/MM/yyyy HH:mm", cultureinfo),
                    NumberOfHits = r.Next(500),
                    ToDate = DateTime.ParseExact(DateTime.Now.AddDays(-r.Next(range)).ToString("dd/MM/yyyy HH:mm"), "dd/MM/yyyy HH:mm", cultureinfo)
                });
            }
            context.SampleNodes.AddRange(nodes);
            context.SaveChanges();
            base.Seed(context);
        }

    }
}