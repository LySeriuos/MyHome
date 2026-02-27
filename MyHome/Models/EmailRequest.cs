using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_Home.Models
{
    public class EmailRequest
    {
        public int Id { get; set; }
        public string To { get;set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string FileLink { get; set; }

    }
}
