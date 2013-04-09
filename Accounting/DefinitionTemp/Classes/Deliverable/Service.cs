using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Accounting.Definition.Models;

namespace Accounting.Definition.Structure
{
    public class Service:Deliverable
    {
        public service SERVICE;

        public void New(service s)
        {
            using (var ctx = new accountingEntities())
            {
                SERVICE = s;

                ctx.services.AddObject(s);
                ctx.SaveChanges();
            }
        }
    }
}
