using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Project.Models;

namespace Project.Structure
{
    public class Transaction
    {
        public transaction txn{ get; set; }
        public Transaction(decimal amount , account acc)
        {   
            using (var ctx = new accountingEntities())
            {   
                txn = new transaction
                {
                    accountID = acc.ID,
                    amount = amount
                };
                ctx.transactions.AddObject(txn);
                ctx.SaveChanges();
            }
        }    
    }
}
