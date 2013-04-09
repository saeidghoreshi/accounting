using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Accounting.Definition.Models;


namespace Accounting.Definition.Structure
{
    public class WAccount : AssetAccount
    {
        public readonly int CATTYPE = (int)projectEnums.catType.W;


        public account Create(int ownerEntityId,int currencyID,decimal balance=0)
        {
            using (var ctx = new accountingEntities())
            {
                var duplicate = ctx.accounts
                    .Where(x => x.ownerEntityID == ownerEntityId && x.currencyID == currencyID && x.catTypeID == CATTYPE)
                    .SingleOrDefault();
                if (duplicate != null)
                    ctx.DeleteObject(duplicate);
                var newAccount = new account()
                {
                    catTypeID=CATTYPE,
                    ownerEntityID=ownerEntityId,
                    currencyID=currencyID,
                    balance=balance
                };
                ctx.accounts.AddObject(newAccount);
                ctx.SaveChanges();

                return newAccount;
            }


        }

    }
}
