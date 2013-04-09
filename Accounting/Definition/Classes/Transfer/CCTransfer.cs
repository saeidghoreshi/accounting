using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

using Accounting.Definition.Models;

namespace Accounting.Definition.Structure
{
    public class CreditTransfer : ExternalTransfer
    {
        public creditTransfer CCTSFR { get; set; }
        public readonly int EXTPAYMENTTYPEID = (int)projectEnums.extTransferType.CreditPayment;


        public CreditTransfer() { }
        public CreditTransfer(int dbTransferID)
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

                CCTSFR = new creditTransfer { ID = EXTERNALTSFR.ID };
                ctx.creditTransfers.AddObject(CCTSFR);

                ctx.SaveChanges();
                ts.Complete();
            }
        }
        public void Load(int ccTransferID)
        {
            using (var ctx = new accountingEntities())
            {
                var all = ctx.creditTransfers
                    .Where(x => x.ID == ccTransferID)
                    .SingleOrDefault();

                CCTSFR = all;
                base.EXTERNALTSFR = all.externalTransfer;
                base.TRANSFER = all.externalTransfer.transfer;
            }
        }
    }

}
