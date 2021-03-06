﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Accounting.Definition.Models;

namespace Accounting.Definition.Structure
{
    public class Currency
    {
        public void New(string CurrencyName, int currencyTypeID)
        {
            using (var ctx = new accountingEntities())
            {
                var newCur = new currency
                {
                    currencyTypeID = currencyTypeID,
                    name = CurrencyName
                };
                var result = ctx.currencies.FirstOrDefault(x => x.name.Equals(CurrencyName) &&  x.currencyTypeID.Equals(currencyTypeID));
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
