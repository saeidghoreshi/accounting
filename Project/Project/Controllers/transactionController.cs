using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Project.Models;

namespace Project.Controllers
{
    public static class transactionController
    {
        public static transaction createNew(int entityID, int catTypeID, decimal amount, int currencyID)
        {
            transaction txn;
            using(var ctx=new accountingEntities())
            {
                var accountID = accountController.getAccount(entityID, catTypeID, currencyID).ID;
                txn= new transaction
                {
                    accountID = accountID,
                    amount = amount
                };
                ctx.transactions.AddObject(txn);
                ctx.SaveChanges();
            }
            return txn;
        }    
    }
}
