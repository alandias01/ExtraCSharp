﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectBilling.DataAccess
{
    public class DataServiceStub : IDataService
    {
        public IList<Project> GetProjects()
        {
            List<Project> projects = new List<Project>()
                {
                    new Project()
                    {
                        ID = 0,
                        Name = "Halloway",
                        Estimate = 500
                    },
                    new Project()
                    {
                        ID = 1,
                        Name = "Jones",
                        Estimate = 1500
                    },
                    new Project()
                    {
                        ID = 2,
                        Name = "Smith",
                        Estimate = 2000
                    }
                };
            return projects;
        }
    }
}
