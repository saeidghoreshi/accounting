using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

using Project.Models;

namespace Project.Structure
{
    public class DebitTransfer: ExternalTransfer
    {
        public debitTransfer DBTSFR { get; set; }
        public readonly int EXTPAYMENTTYPEID = (int)projectEnums.extTransferType.InteracPayment;


        public DebitTransfer() { }
        public DebitTransfer(int dbTransferID)
        {
            this.Load(dbTransferID);
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

                EXTERNALTSFR = new externalTransfer { ID = TRANSFER.ID };
                ctx.externalTransfers.AddObject(EXTERNALTSFR);

                DBTSFR = new debitTransfer { ID = EXTERNALTSFR.ID };
                ctx.debitTransfers.AddObject(DBTSFR);

                ctx.SaveChanges();
                ts.Complete();
            }
        }
        public void Load(int dbTransferID)
        {
            using (var ctx = new accountingEntities())
            {
                var all= ctx.debitTransfers
                    .Where(x => x.ID == dbTransferID)
                    .SingleOrDefault();

                DBTSFR = all;
                base.EXTERNALTSFR = all.externalTransfer;
                base.TRANSFER = all.externalTransfer.transfer;
            }
        }
    }

}
