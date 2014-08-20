using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Exernet.Models
{
    public class Answer
    {
        public int Id { get; set; }
        public ExernetTask Task { get; set; }
        public string Text { get; set; }
    }
}