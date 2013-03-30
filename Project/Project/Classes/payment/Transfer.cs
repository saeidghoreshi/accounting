using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

using Project.Models;

namespace Project.Structure
{
    interface ITransfer 
    {
        List<transaction> cancelTransfer(enumsController.transferAction tsfrAction);
    }
    public abstract class Transfer:ITransfer
    {
        public transfer TRANSFER {get;set;}

        public List<transaction> cancelTransfer(enumsController.transferAction tsfrAction)
        {
            List<transaction> reveresedTransactions=new List<transaction>();

            //get Related transacions and input reveres ones
            using (var ctx = new accountingEntities())
            {
                //Transfer Transactions
                var Transactions = ctx.transferActions
                    .Join(ctx.sysActionTransactions, ta => ta.ID, sat => sat.sysActionID, (ta, sat) => new { ta,sat})
                    .Where(x => x.ta.transferID.Equals(TRANSFER.ID))
                    .Where(x => x.ta.transferActionTypeID.Equals(tsfrAction))
                    .Join(ctx.transactions, J => J.sat.transactionID, tr => tr.ID, (J, tr) =>  tr)
                    .ToList();

                //enter and save revered Transactions
                foreach (var txn in Transactions)
                {
                    account acc = Account.getAccount((int)txn.accountID);
                    var tx = new Transaction(-1 * (decimal)txn.amount, acc);
                    reveresedTransactions.Add(tx.TXN);
                }

                //IF PAYMENT ACTION IS REFUND, NEW FEE HANDLING TRANSACTIONS WOULD BE NEEDED
                
            }
            return reveresedTransactions;
        }
    }

}
