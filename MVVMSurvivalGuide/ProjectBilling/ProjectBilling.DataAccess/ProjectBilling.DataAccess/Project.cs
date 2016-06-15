using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectBilling.DataAccess
{
    public class Project : IProject
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public double Estimate { get; set; }
        public double Actual { get; set; }

        public void Update(IProject project)
        {
            Name = project.Name;
            Estimate = project.Estimate;
            Actual = project.Actual;
        }

        
    }
}
