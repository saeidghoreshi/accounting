using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Project.Models;

namespace Project.Controllers
{
    public class accountController
    {
        public static account getAccount(int entityID, int catTypeID, int currencyID)
        {
            account account = null;
            using (var ctx = new accountingEntities())
            {
                account = ctx.accounts.Where(x => x.catTypeID == catTypeID && x.ownerEntityID == entityID && x.currencyID == currencyID).SingleOrDefault();
            }
            return account;
        }
    }
    public abstract class AssetAccount : accountController
    {
        public readonly int accountTYPE = enumsController.glTypes.FindIndex(x => x.name.Equals("ASSET"));
    }
    public abstract class OEAccount : accountController
    {
        public readonly int accountTYPE = enumsController.glTypes.FindIndex(x => x.name.Equals("OE"));
    }
    public abstract class LibAccount : accountController
    {
        public readonly int accountTYPE = enumsController.glTypes.FindIndex(x => x.name.Equals("LIB"));
    }
}
