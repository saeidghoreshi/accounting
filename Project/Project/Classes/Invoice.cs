using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Project.Models;
using System.Transactions;

namespace Project.Structure
{
    public class Invoice
    {
        public invoice INV { get; set; }
        
        public void New(invoice inv)
        {
            INV = inv;

            using (var ctx = new accountingEntities())
            using (var ts = new TransactionScope())
            {
                //Entity
                var e = new entity
                {
                    entityTypeID = (int)enumsController.entityType.invoice
                };
                ctx.entities.AddObject(e);

                //new Invoice
                var newInvoice = new invoice()
                {
                    issuerEntityID = inv.issuerEntityID,
                    receiverEntityID = inv.receiverEntityID,
                    currencyID = inv.currencyID,
                    ID=e.ID
                };

                //sys Action
                var sysaction = new sysAction 
                { 
                    sysActionTypeID=(int)enumsController.sysAction.invoice
                };
                ctx.sysActions.AddObject(sysaction);

                //sys Action Invoice
                var sysActionInvoice = new invoiceAction
                {
                    ID = sysaction.ID,
                    invoiceStatusID= (int)enumsController.invoiceStatus.Generated,
                    name="Invoice Generated at "+DateTime.Now.ToLongTimeString()
                };
                ctx.invoiceActions.AddObject(sysActionInvoice);

                ctx.SaveChanges();

                ts.Complete();
            }
        }
        
        public void loadInvoiceByID(int invoiceID)
        {
            using (var ctx = new accountingEntities())
            {
                INV = ctx.invoices.Where(x => x.ID.Equals(invoiceID)).FirstOrDefault();
                if (INV == null)
                    throw new Exception("Invoice does not exists");
            }
        }

        
        public decimal getInvoiceServicesSumAmt()
        {
            using (var ctx = new accountingEntities())
            {
                var inv = ctx.invoices.Where(x => x.ID.Equals(INV.ID)).SingleOrDefault();
                if (inv == null)
                    throw new Exception();

                decimal invoiceServicesAmt = 0;

                invoiceServicesAmt = ctx.orderDetails
                    .Where(x=>x.orderID.Equals(inv.orderID))
                    .Sum(x => x.unitPrice*x.quantity).Value;

                return (decimal)invoiceServicesAmt;
            }
        }

        public void finalizeInvoice()
        {
            using (var ctx = new accountingEntities())
            using (var ts = new TransactionScope())
            {
                
                //Get Sum of Invoice Services added
                decimal invoiceServicesAmt = this.getInvoiceServicesSumAmt();


                
                //Record related transctions
                List<transaction> transactions = new List<transaction>();

                account acc_AP = Account.getAccount((int)INV.receiverEntityID, (int)enumsController.catType.AP, (int)INV.currencyID);
                var trans1 = new Transaction(-1 * (decimal)invoiceServicesAmt, acc_AP);
                transactions.Add(trans1.TXN);


                account acc_AR = Account.getAccount((int)INV.issuerEntityID, (int)enumsController.catType.AR, (int)INV.currencyID);
                var trans2 = new Transaction(+1 * (decimal)invoiceServicesAmt, acc_AR);
                transactions.Add(trans2.TXN);

                /*Record Invoice Transaction*/
                this.RecordInvoiceTransaction(transactions, enumsController.invoiceStatus.Finalized);

                ts.Complete();
            }
        }

        public void addInvoiceOrderDetail(orderDetail newOrderDetail)
        {
            using (var ctx = new accountingEntities())
            {   
                ctx.orderDetails.AddObject(newOrderDetail);
                ctx.SaveChanges();
            }
        }




        /*Payment for Invoice*/
        public void doINTERNALTransfer(decimal amount)
        {
            using (var ctx = new accountingEntities())
            using (var ts = new TransactionScope())
            {
                classes.internalPayment internalPayment = new classes.internalPayment();
                internalPayment.createNew(INV.receiverEntityID, INV.issuerEntityID, amount, INV.currencyID);

                /*Record New Invoice Payment*/
                var NewInvoicePayment = new invoicePayment()
                {
                    invoiceID = this.invoiceID,
                    paymentID = internalPayment.paymentID
                };
                ctx.invoicePayment.AddObject(NewInvoicePayment);
                ctx.SaveChanges();

                //Record related transctions [for invoice payment]
                List<int> transactions = new List<int>();
                transactions.Add(Transaction.createNew(issuerEntityID, (int)AssetCategories.W, -1 * amount, this.currencyID));
                transactions.Add(Transaction.createNew(receiverEntityID, (int)LibCategories.AP, +1 * amount, this.currencyID));
                transactions.Add(Transaction.createNew(receiverEntityID, (int)AssetCategories.W, +1 * amount, this.currencyID));
                transactions.Add(Transaction.createNew(issuerEntityID, (int)AssetCategories.AR, -1 * amount, this.currencyID));

                /*Record Invoice Payment transactions*/
                this.RecordInvoicePaymentTransactions(transactions, internalPayment.paymentID, enums.paymentStat.PaidApproved);

                /*Record Invoice Transaction*/
                this.RecordInvoiceTransaction(transactions, enums.invoiceStat.internalPaymant);

                ts.Complete();
            }
        }
        public void doCCExtPayment(decimal amount, int cardID, enumsController.ccCardType ccCardType)
        {
            //var ccFeeFor

            using (var ctx = new accountingEntities())
            using (var ts = new TransactionScope())
            {
                classes.ccPayment creditCardPayment = new ccPayment();
                creditCardPayment.createNew(this.receiverEntityID, this.issuerEntityID, amount, this.currencyID, cardID);


                /*Record New Invoice Payment*/
                var NewInvoicePayment = new invoicePayment()
                {
                    invoiceID = invoiceID,
                    paymentID = creditCardPayment.paymentID
                };
                ctx.invoicePayment.AddObject(NewInvoicePayment);
                ctx.SaveChanges();

                //get Fee bank cardType
                var card = new classes.card.CreditCard();
                card.loadByCardID(cardID);


                ccFee ccfee = new ccFee();
                ccfee.loadccFeeByBankCardTypeID((int)ccCardType, (card as Entity).getBankByCard(card.cardID).bankID);

                //Record related transctions [for invoice payment]
                List<int> transactions = new List<int>();

                transactions.Add(Transaction.createNew(receiverEntityID, (int)AssetCategories.CCCASH, -1 * amount, this.currencyID));
                transactions.Add(Transaction.createNew(receiverEntityID, (int)LibCategories.AP, +1 * amount, this.currencyID));

                transactions.Add(Transaction.createNew(issuerEntityID, (int)AssetCategories.W, +1 * amount - (decimal)ccfee.amount, this.currencyID));
                transactions.Add(Transaction.createNew(issuerEntityID, (int)AssetCategories.AR, -1 * amount, this.currencyID));
                transactions.Add(Transaction.createNew(issuerEntityID, (int)OECategories.EXP, (decimal)ccfee.amount, this.currencyID));

                /*Record Invoice Payment transactions*/
                this.RecordInvoicePaymentTransactions(transactions, creditCardPayment.paymentID, enums.paymentStat.PaidApproved);

                /*Record Invoice Transaction*/
                enums.invoiceStat? invoicestat = null;
                switch (ccCardType)
                {
                    case enums.ccCardType.MASTERCARD:
                        invoicestat = enums.invoiceStat.masterCardPaymant;
                        break;
                    case enums.ccCardType.VISACARD:
                        invoicestat = enums.invoiceStat.visaCardPaymant;
                        break;
                }
                this.RecordInvoiceTransaction(transactions, (enums.invoiceStat)invoicestat);


                ts.Complete();
            }


        }
        public static void doINTERACPayment(decimal amount, int cardID)
        {
            using (var ctx = new accountingEntities())
            using (var ts = new TransactionScope())
            {
                classes.dbPayment debitCardPayment = new dbPayment();
                debitCardPayment.createNew(this.receiverEntityID, this.issuerEntityID, amount, this.currencyID, cardID);

                /*Record New Invoice Payment*/
                var NewInvoicePayment = new invoicePayment()
                {
                    invoiceID = this.invoiceID,
                    paymentID = debitCardPayment.paymentID
                };
                ctx.invoicePayment.AddObject(NewInvoicePayment);
                ctx.SaveChanges();


                //get Fee bank cardType
                var card = new classes.card.DebitCard();
                card.loadByCardID(cardID);


                Fee fee = new Fee();
                fee.loadFeeByBankCardTypeID(card.cardTypeID, ((Entity)card).getBankByCard(cardID).bankID);

                //Record related transctions [for invoice payment]
                List<int> transactions = new List<int>();
                transactions.Add(Transaction.createNew(receiverEntityID, (int)AssetCategories.DBCASH, -1 * amount, this.currencyID));
                transactions.Add(Transaction.createNew(receiverEntityID, (int)LibCategories.AP, +1 * amount, this.currencyID));

                transactions.Add(Transaction.createNew(issuerEntityID, (int)AssetCategories.W, +1 * amount - (decimal)fee.amount, this.currencyID));
                transactions.Add(Transaction.createNew(issuerEntityID, (int)AssetCategories.AR, -1 * amount, this.currencyID));
                transactions.Add(Transaction.createNew(issuerEntityID, (int)OECategories.EXP, (decimal)fee.amount, this.currencyID));

                /*Record Invoice Payment transactions*/
                this.RecordInvoicePaymentTransactions(transactions, debitCardPayment.paymentID, enums.paymentStat.PaidApproved);

                /*Record Invoice Transaction*/
                this.RecordInvoiceTransaction(transactions, enums.invoiceStat.interacPaymant);


                ts.Complete();
            }


        }

        /*cancelling one payment od invoice at the time*/
        public static void cancelInvoicePaymentEXT(int paymentID)
        {
            using (var ctx = new accountingEntities())
            using (var ts = new TransactionScope())
            {
                accounting.classes.externalPayment payment = new accounting.classes.externalPayment();
                payment.loadByPaymentID(paymentID);

                List<int> transactions = payment.cancelPayment(enums.paymentAction.Void);

                /*Record Invoice Payment transactions*/
                this.RecordInvoicePaymentTransactions(transactions, paymentID, enums.paymentStat.VoidApproved);

                /*Record Invoice Transaction*/
                if (payment.extPaymentTypeID == (int)enums.extPaymentType.CreditPayment)
                    this.RecordInvoiceTransaction(transactions, enums.invoiceStat.partialCreditCardPaymantCancelled);

                if (payment.extPaymentTypeID == (int)enums.extPaymentType.InteracPayment)
                    this.RecordInvoiceTransaction(transactions, enums.invoiceStat.partialInteracPaymantCancelled);

                ts.Complete();
            }
        }
        public static void cancelInvoicePaymentINTERNAL(int paymentID)
        {
            using (var ctx = new accountingEntities())
            using (var ts = new TransactionScope())
            {
                accounting.classes.internalPayment payment = new accounting.classes.internalPayment();
                payment.loadByPaymentID(paymentID);

                List<int> transactions = payment.cancelPayment(enums.paymentAction.Void);
                /*Record Invoice Payment transactions*/
                this.RecordInvoicePaymentTransactions(transactions, paymentID, enums.paymentStat.VoidApproved);

                /*Record Invoice Transaction*/
                this.RecordInvoiceTransaction(transactions, enums.invoiceStat.partialInternalPaymantCancelled);

                ts.Complete();
            }
        }

        /*cancel invoice all payments*/
        public static void cancelInvoice()
        {
            using (var ctx = new accountingEntities())
            using (var ts = new TransactionScope())
            {
                /*get all invoice payments*/
                List<payment> payments = this.getAllPayments();

                /*cancel payments one by one*/
                foreach (var item in payments)
                {
                    if (item.paymentTypeID == (int)enums.paymentType.Internal)
                        cancelInvoicePaymentINTERNAL(item.ID);
                    if (item.paymentTypeID == (int)enums.paymentType.External)
                        cancelInvoicePaymentEXT(item.ID);
                }

                /*Cancel invoice*/
                this.loadInvoiceByInvoiceID(invoiceID);

                //Get Sum of Invoice Services added
                decimal invoiceServicesAmt = this.getInvoiceServicesSumAmt();

                //Record related transctions
                List<int> transactions = new List<int>();
                var trans1 = Transaction.createNew((int)this.receiverEntityID, (int)LibCategories.AP, +1 * (decimal)invoiceServicesAmt, (int)this.currencyID);
                transactions.Add(trans1);
                var trans2 = Transaction.createNew((int)this.issuerEntityID, (int)AssetCategories.AR, -1 * (decimal)invoiceServicesAmt, (int)this.currencyID);
                transactions.Add(trans2);

                /*Record Invoice Transaction*/
                this.RecordInvoiceTransaction(transactions, enums.invoiceStat.Cancelled);

                ts.Complete();

            }
        }

        public static List<payment> getAllPayments()
        {
            using (var ctx = new accountingEntities())
            {
                var payments = ctx.invoicePayment
                    .Where(z => (int)z.invoiceID == this.invoiceID)
                    //.Where(z=>z.invoice.in)
                    .Select(x => x.payment)
                    .ToList();

                return payments;
            }
        }

        public void RecordInvoiceTransaction(List<transaction> transactions, enumsController.invoiceStatus invoiceStat)
        {
            using (var ctx = new accountingEntities())
            using (var ts = new TransactionScope())
            {
                //create invoice Action
                var invAction = new invoiceAction()
                {
                    invoiceID = this.invoiceID,
                    invoiceStatID = (int)invoiceStat
                };
                ctx.invoiceAction.AddObject(invAction);
                ctx.SaveChanges();


                //create invoice Transactions and invoice action Transactions
                foreach (var item in transactions)
                {
                    var invActionTrans = new invoiceActionTransaction()
                    {
                        invoiceActionID = invAction.ID,
                        transactionID = item
                    };
                    ctx.invoiceActionTransaction.AddObject(invActionTrans);

                    ctx.SaveChanges();
                }

                ts.Complete();

            }
        }

        private void RecordInvoicePaymentTransactions(List<int> txns, int paymentID, enums.paymentStat payStat)
        {
            using (var ctx = new accountingEntities())
            {
                //Create Payment Action
                var payAction = new paymentAction()
                {
                    paymentID = paymentID,
                    paymentStatID = (int)payStat
                };
                ctx.paymentAction.AddObject(payAction);
                ctx.SaveChanges();

                //Record Pyament Action TXNS
                foreach (var txn in txns)
                {
                    var newPayActionTxn = new paymentActionTransaction()
                    {
                        paymentActionID = payAction.ID,
                        transactionID = txn
                    };
                    ctx.paymentActionTransaction.AddObject(newPayActionTxn);
                    ctx.SaveChanges();
                }
            }
        }
    }
}
