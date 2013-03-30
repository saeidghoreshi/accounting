using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Project.Models;

namespace Project.Structure
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
