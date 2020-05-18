using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace JobOffersWebsite.Models
{
    public class Job
    {
        public int Id { get; set; }

        [DisplayName("اسم الوظيفة")]
        public string JobName { get; set; }

        [DisplayName("وصف الوظيفة")]
        public string JobDescription { get; set; }

        [DisplayName("صورة الوظيفة")]
        public string JobImage { get; set; }

        [DisplayName ("نوع الوظيفة")]
        public int CategoryID { get; set; }

        public virtual Category Category { get; set; }
    }
}