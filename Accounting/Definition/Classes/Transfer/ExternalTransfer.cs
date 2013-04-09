using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

using Accounting.Definition.Models;

namespace Accounting.Definition.Structure
{
    interface IExternalTransfer
    {
        List<transaction> cancelTransfer(projectEnums.transferAction tsfrAction);
    }
    public abstract class ExternalTransfer: Transfer , IExternalTransfer
    {
        public externalTransfer EXTERNALTSFR { get; set; }
        public const projectEnums.transferType TRANSFERTYPE = projectEnums.transferType.External;

        public static int getExtTransferType(int extTransferID)
        {
            int extTransferTypeID = -1;
            using (var ctx = new accountingEntities())
            {
                var tsfr = ctx.externalTransfers
                    .Where(x => x.ID.Equals(extTransferID))
                    .Select(x => new
                    {
                        extTransferTypeID = x.externalTsfrTypeID
                    });
                extTransferTypeID = (int)tsfr.Single().extTransferTypeID;
            }
            return extTransferTypeID;
        }
    }
}
