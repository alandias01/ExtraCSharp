using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wpf01
{
    public class Dept
    {
        public int d_id { get; set; }
        public string d_name { get; set; }

        public Dept(int i, string name) { this.d_id = i; this.d_name = name; }
    }
}
