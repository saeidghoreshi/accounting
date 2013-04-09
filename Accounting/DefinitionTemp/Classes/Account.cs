
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Accounting.Definition.Models;

namespace Accounting.Definition.Structure
{
    public class Account
    {
        public static account getAccount(int entityID, int catTypeID, int currencyID)
        {
            account account = new account() { };
            using (var ctx = new accountingEntities())
            {
                account = ctx.accounts.Where(
                    x => x.catTypeID == catTypeID
                    && x.ownerEntityID.Equals(entityID)
                    && x.currencyID.Equals(currencyID))
                    .SingleOrDefault();
            }

            return account;
        }
        public static account getAccount(int accountID)
        {
            account account = new account() { };
            using (var ctx = new accountingEntities())
            {
                account = ctx.accounts.Where(x => x.ID.Equals(accountID)).SingleOrDefault();
            }

            return account;
        }
    }
    public abstract class AssetAccount : Account
    {
        public readonly int accountTYPE = projectEnums.glTypes.FindIndex(x => x.name.Equals("ASSET"));
    }
    public abstract class OEAccount : Account
    {
        public readonly int accountTYPE = projectEnums.glTypes.FindIndex(x => x.name.Equals("OE"));
    }
    public abstract class LibAccount : Account
    {
        public readonly int accountTYPE = projectEnums.glTypes.FindIndex(x => x.name.Equals("LIB"));
    }
}
