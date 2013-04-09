using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

using Accounting.Definition.Models;

namespace Accounting.Definition.Structure
{   
    public class InternalTransfer: Transfer
    {
        public internalTransfer INTERNALTSFR { get; set; }
        public const projectEnums.transferType TRANSFERTYPE = projectEnums.transferType.Internal;

        public InternalTransfer() { }
        public InternalTransfer(int transferID)
        {
            this.Load(transferID);
        }

        public void New(int issuerEntityID, int receiverEntityID, decimal amount, int currencyID)
        {
            using (var ctx = new accountingEntities())
            using (var ts = new TransactionScope())
            {
                base.TRANSFER = new transfer()
                {
                    issuerEntityID = issuerEntityID,
                    receiverEntityID = receiverEntityID,
                    amount = amount,
                    currencyID = currencyID,
                    transferTypeID = (int)TRANSFERTYPE
                };
                ctx.transfers.AddObject(TRANSFER);
                
                INTERNALTSFR= new internalTransfer{ID = TRANSFER.ID};

                ctx.internalTransfers.AddObject(INTERNALTSFR);
                ctx.SaveChanges();

                ts.Complete();
            }
        }
        public void Load(int transferID)
        {
            using (var ctx = new accountingEntities())
            {
                INTERNALTSFR= ctx.internalTransfers
                    .Where(x => x.ID == transferID)
                    .SingleOrDefault();

                if (INTERNALTSFR == null)
                    throw new Exception("no such a EXT Payment Exists");
            }
        }
    }
}
