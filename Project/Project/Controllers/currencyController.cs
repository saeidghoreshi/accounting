using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Project.Models;

namespace Project.Controllers
{
    public static class currencyController
    {
        public void create(string CurrencyName, int currencyTypeID)
        {
            using (var ctx = new accountingEntities())
            {
                var newCur = new currency
                {
                    currencyTypeID = currencyTypeID,
                    name = CurrencyName
                };
                var result = ctx.currencies.FirstOrDefault(x => x.name == CurrencyName && x.currencyType.ID == currencyTypeID);
                if (result != null)
                    throw new Exception("Currency Duplicated");
                else
                {
                    ctx.currencies.AddObject(newCur);
                    ctx.SaveChanges();

                }
            }

        }
    }
}
