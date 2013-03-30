using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Project.Models;
using System.Transactions;
using Project.Structure.enumsController;

namespace Project.Structure
{
    public class Entity
    {
        entity ENTITY;
        public void Entity(){}

        public void Entity(int entityTypeID)
        {
            using (var ctx = new accountingEntities())
            {
                var newEntity = new entity()
                {
                    entityTypeID = entityTypeID
                };
                ENTITY = newEntity;

                ctx.entities.AddObject(newEntity);
                ctx.SaveChanges();
            }
        }
        

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

                account acc_W= Account.getAccount(entityID, (int)enumsController.catType.W, currencyID);
                var trans1 = new Transaction( +1 * (decimal)amount, acc_W);
                transactions.Add(trans1.TXN);

                account acc_CCCASH = Account.getAccount(entityID, (int)enumsController.catType.CCCASH, currencyID);
                var trans2 = new Transaction(-1 * (decimal)amount, acc_CCCASH);
                transactions.Add(trans2.TXN);

                ts.Complete();
            }
        }

        protected void payInvoiceByCC(Invoice inv, decimal amount, int cardID, enumsController.ccCardType cardType)
        {
            this.doCCExtPayment(amount, cardID, cardType);
        }

        protected void payInvoiceByInterac(invoice inv, decimal amount, int cardID)
        {
            inv.doINTERACPayment(amount, cardID);
        }

        public void payInvoiceByInternal(invoice inv, decimal amount)
        {
            this.doINTERNALTransfer(amount);
        }

        /// <summary>
        /// create invoice with services/amount dectionary
        /// </summary>
        /// <param name="receiverEntityID"></param>
        /// <param name="currencyID"></param>
        /// <param name="servicesAmt"></param>
        public invoice issueInvoice(int receiverEntityID, int currencyID, Dictionary<deliverable, decimal> servicesAmt)
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
