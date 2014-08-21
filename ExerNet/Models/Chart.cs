using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Exernet.Models
{
    public class Chart
    {
        public int Id { get; set; }
        public string Expression { get; set; }
        public string To { get; set; }
        public string From { get; set; }
        public string Step { get; set; }
    }
}