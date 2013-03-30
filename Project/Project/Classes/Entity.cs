using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Project.Models;
using System.Transactions;

namespace Project.Structure
{
    public class Entity
    {
        public entity ENTITY{get;set;}

        public void addCard(int cardID,int entityID)
        {
            using (var ctx = new accountingEntities())
            {
                var person = ctx.people.Where(x => x.ID.Equals(entityID)).SingleOrDefault();
                var newEntityCard = new entityCard()
                {
                    entityID = entityID,
                    cardID = cardID
                };
                ctx.entityCards.AddObject(newEntityCard);
                ctx.SaveChanges();
            }
        }

        public List<card> fetchCards(int entityID)
        {
            using (var ctx = new accountingEntities())
            {
                var cardsList = ctx.entityCards
                    .Where(x => x.entityID == entityID)
                    .Select(x => x.card).ToList();

                return cardsList;
            }
        }

        protected static void addWalletMoney(decimal amount, string title, int currencyID, int entityID)
        {
            using (var ctx = new accountingEntities())
            using (var ts = new TransactionScope())
            {
                //Record related transctions
                List<transaction> transactions = new List<transaction>();

                account acc_W= Account.getAccount(entityID, (int)projectEnums.catType.W, currencyID);
                var trans1 = new Transaction( +1 * (decimal)amount, acc_W);
                transactions.Add(trans1.TXN);

                account acc_CCCASH = Account.getAccount(entityID, (int)projectEnums.catType.CCCASH, currencyID);
                var trans2 = new Transaction(-1 * (decimal)amount, acc_CCCASH);
                transactions.Add(trans2.TXN);

                ts.Complete();
            }
        }

        protected void payInvoiceByCC(Invoice inv, decimal amount, int cardID, projectEnums.ccCardType cardType)
        {
            inv.Transfer_Ext_Credit(amount, cardID, cardType);
        }

        protected void payInvoiceByInterac(Invoice inv, decimal amount, int cardID)
        {
            inv.Transfer_Ext_Debit(amount, cardID);
        }

        public void payInvoiceByInternal(Invoice inv, decimal amount)
        {
            inv.Transfer_Internal(amount);
        }

        public Invoice issueInvoice(int receiverEntityID, int currencyID, Dictionary<deliverable, decimal> servicesAmt)
        {
            var invoiceData=new invoice{};
            var inv = new Invoice();
            inv.New(invoiceData);

            foreach (var item in servicesAmt)
                inv.addInvoiceOrderDetail((item.Key as deliverable));

            inv.finalizeInvoice();

            return inv;
        }

    }
}
