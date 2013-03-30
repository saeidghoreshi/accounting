using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

using Project.Models;

namespace Project.Structure
{
    interface IExternalTransfer
    {
        List<transaction> cancelTransfer(enumsController.transferAction tsfrAction);
    }
    public abstract class ExternalTransfer: Transfer , IExternalTransfer
    {
        public externalTransfer EXTERNALTSFR { get; set; }
        public const enumsController.transferType TRANSFERTYPE = enumsController.transferType.External;
    }
}
