using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Accounting.Definition.Models;

namespace Accounting.Definition.Structure
{
    public class Product:Deliverable
    {
        public product PRODUCT;

        public void New(product p)
        {
            using (var ctx = new accountingEntities())
            {
                PRODUCT = p;

                ctx.products.AddObject(p);
                ctx.SaveChanges();
            }
        }
    }
}
