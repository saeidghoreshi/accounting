using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Accounting.Definition.Models;

namespace Accounting.Definition.Structure
{
    public class Transaction
    {
        public transaction TXN{ get; set; }

        public Transaction(decimal amount , account acc)
        {   
            using (var ctx = new accountingEntities())
            {   
                TXN = new transaction
                {
                    accountID = acc.ID,
                    amount = amount
                };
                ctx.transactions.AddObject(TXN);
                ctx.SaveChanges();
            }
        }    
    }
}
