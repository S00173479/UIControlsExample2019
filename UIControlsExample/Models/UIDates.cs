using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace UIControlsExample.Models
{
    public class UINode
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NodeID { get; set; }

        [Column(TypeName = "datetime")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy HH:mm}", ApplyFormatInEditMode = false)]
        public DateTime? FromDate { get; set; }

        [Column(TypeName = "datetime")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy HH:mm}", ApplyFormatInEditMode = false)]
        public DateTime? ToDate { get; set; }


        public int NumberOfHits { get; set; }
    }

    public class UIContext : DbContext
    {
        public DbSet<UINode> SampleNodes { get; set; }

        public UIContext() : base("UIConnectionString")
        {
            Database.SetInitializer(new UIDateInitializer());
            Database.Initialize(true);
        }
    }
}