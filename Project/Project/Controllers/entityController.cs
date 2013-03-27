using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Project.Models;
using System.Transactions;
using Project.Controllers.enumsController;

namespace Project.Controllers
{
    public  class entityController
    {
        protected void New(int entityTypeID)
        {
            using (var ctx = new accountingEntities())
            {
                var newEntity = new entity()
                {
                    entityTypeID = entityTypeID
                };
                ctx.entities.AddObject(newEntity);
                ctx.SaveChanges();
            }
        }
        

        public void addCard(int cardID,int entityID)
        {
            using (var ctx = new accountingEntities())
            {
                var person = ctx.people.Where(x => x.entityID == entityID).SingleOrDefault();
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
                List<int> transactions = new List<int>();
                var trans1 = Transaction.createNew(entityID, (int)AssetCategories.W, +1 * (decimal)amount, currencyID);
                transactions.Add(trans1);
                var trans2 = Transaction.createNew(this.ENTITYID, (int)AssetCategories.CCCASH, -1 * (decimal)amount, currencyID);
                transactions.Add(trans2);

                //Record Wallet entity and walletEntityTransaction
                var entityWallet = new entityWallet()
                {
                    entityID = this.ENTITYID,
                    amount = amount,
                    title = title,
                    currencyID = currencyID
                };
                ctx.entityWallet.AddObject(entityWallet);
                ctx.SaveChanges();

                foreach (var txn in transactions)
                {
                    var entityWalletTxn = new entityWalletTransaction()
                    {
                        entityWalletID = entityWallet.ID,
                        transactionID = txn
                    };
                    ctx.entityWalletTransaction.AddObject(entityWalletTxn);
                    ctx.SaveChanges();
                }

                ts.Complete();
            }
        }

        /// <summary>
        /// Just for entittyType {person,Organization}
        /// </summary>
        /// <param name="inv"></param>
        /// <param name="amount"></param>
        /// <param name="cardID"></param>
        /// <param name="cardType"></param>
        protected void payInvoiceByCC(invoice inv, decimal amount, int cardID, enums.ccCardType cardType)
        {
            inv.doCCExtPayment(amount, cardID, cardType);
        }

        /// <summary>
        /// Just for entittyType {person,Organization}
        /// </summary>
        /// <param name="inv"></param>
        /// <param name="amount"></param>
        /// <param name="cardID"></param>
        /// <param name="cardType"></param>
        protected void payInvoiceByInterac(invoice inv, decimal amount, int cardID)
        {
            inv.doINTERACPayment(amount, cardID);
        }

        /// <summary>
        /// Just for entittyType {person,Organization}
        /// </summary>
        /// <param name="inv"></param>
        /// <param name="amount"></param>
        /// <param name="cardID"></param>
        /// <param name="cardType"></param>
        protected void payInvoiceByInternal(invoice inv, decimal amount)
        {
            inv.doINTERNALTransfer(amount);
        }

        /// <summary>
        /// create invoice with services/amount dectionary
        /// </summary>
        /// <param name="receiverEntityID"></param>
        /// <param name="currencyID"></param>
        /// <param name="servicesAmt"></param>
        public invoice createInvoice(int receiverEntityID, int currencyID, Dictionary<deliverable, decimal> servicesAmt)
        {
            var inv = new accounting.classes.Invoice();
            inv.New(this.ENTITYID, receiverEntityID, currencyID);

            foreach (var item in servicesAmt)
                inv.addService((item.Key as classes.Service).serviceID, item.Value);

            inv.finalizeInvoice();

            return inv;
        }
        
    }
}
