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
                var e = new entity{entityTypeID = (int)projectEnums.entityType.invoice};
                ctx.entities.AddObject(e);

                //New Invoice
                var newInvoice = new invoice()
                {
                    currencyID = inv.currencyID,
                    ID=e.ID
                };

                //sys Action
                var sysaction = new sysAction{sysActionTypeID=(int)projectEnums.sysAction.invoice};
                ctx.sysActions.AddObject(sysaction);

                //sys Action Invoice
                var sysActionInvoice = new invoiceAction
                {
                    ID = sysaction.ID,
                    invoiceStatusID= (int)projectEnums.invoiceStatus.Generated,
                    name="Invoice Generated at "+DateTime.Now.ToLongTimeString()
                };
                ctx.invoiceActions.AddObject(sysActionInvoice);

                ctx.SaveChanges();
                ts.Complete();
            }
        }
        
        public void Load(int invoiceID)
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

                account acc_AP = Account.getAccount((int)INV.order.receiverEntityID, (int)projectEnums.catType.AP, (int)INV.currencyID);
                var trans1 = new Transaction(-1 * (decimal)invoiceServicesAmt, acc_AP);
                transactions.Add(trans1.TXN);

                account acc_AR = Account.getAccount((int)INV.order.issuerEntityID, (int)projectEnums.catType.AR, (int)INV.currencyID);
                var trans2 = new Transaction(+1 * (decimal)invoiceServicesAmt, acc_AR);
                transactions.Add(trans2.TXN);

                /*Record Invoice Transaction*/
                this.RecordInvoiceTransaction(transactions, projectEnums.invoiceStatus.Finalized);

                ts.Complete();
            }
        }

        public void addInvoiceOrderDetail(deliverable newItem)
        {
            using (var ctx = new accountingEntities())
            {
                var newOrderDetail = new orderDetail 
                {
                    deliverableID=newItem.ID
                };
                ctx.orderDetails.AddObject(newOrderDetail);
                ctx.SaveChanges();
            }
        }




        /*Payment for Invoice*/
        public void Transfer_Internal(decimal amount)
        {
            if (INV.Equals(null))
                throw new Exception("No invocie set.pleae laod or create new Invoice");


            using (var ctx = new accountingEntities())
            using (var ts = new TransactionScope())
            {
                //create new Internal Payment
                InternalTransfer iPayment = new InternalTransfer();
                iPayment.New((int)INV.order.receiverEntityID, (int)INV.order.issuerEntityID, amount, (int)INV.currencyID);

                /*Record New Invoice Payment*/
                var NewInvTransfer= new invoiceTransfer()
                {
                    invoiceID = INV.ID,
                    transferID = iPayment.TRANSFER.ID
                };
                ctx.invoiceTransfers.AddObject(NewInvTransfer);
                ctx.SaveChanges();


                //Record related transctions [for invoice payment]
                List<transaction> transactions = new List<transaction>();

                account acc_W = Account.getAccount((int)INV.order.issuerEntityID, (int)projectEnums.catType.W, (int)INV.currencyID);
                var t1 = new Transaction(-1 * (decimal)amount, acc_W);
                transactions.Add(t1.TXN);

                account acc_AP = Account.getAccount((int)INV.order.receiverEntityID, (int)projectEnums.catType.AP, (int)INV.currencyID);
                var t2 = new Transaction(+1 * (decimal)amount, acc_AP);
                transactions.Add(t2.TXN);

                account acc_W2 = Account.getAccount((int)INV.order.receiverEntityID, (int)projectEnums.catType.W, (int)INV.currencyID);
                var t3 = new Transaction(+1 * (decimal)amount, acc_W2);
                transactions.Add(t3.TXN);

                account acc_AR = Account.getAccount((int)INV.order.issuerEntityID, (int)projectEnums.catType.AR, (int)INV.currencyID);
                var t4 = new Transaction(-1 * (decimal)amount, acc_AR);
                transactions.Add(t4.TXN);

                /*Record Invoice Payment transactions*/
                this.RecordInvoiceTransferTransactions(transactions, (int)iPayment.TRANSFER.ID, projectEnums.transferStatus.PaidApproved);

                /*Record Invoice Transaction*/
                this.RecordInvoiceTransaction(transactions, projectEnums.invoiceStatus.internalTransfer);

                ts.Complete();
            }
        }
        public void Transfer_Ext_Credit(decimal amount, int cardID, projectEnums.ccCardType ccCardType)
        {
            if (INV.Equals(null))
                throw new Exception("No invocie set.pleae laod or create new Invoice");

            using (var ctx = new accountingEntities())
            using (var ts = new TransactionScope())
            {
                //create new Internal Payment
                CreditTransfer ccPayment = new CreditTransfer();
                ccPayment.New((int)INV.order.receiverEntityID, (int)INV.order.issuerEntityID, amount, (int)INV.currencyID);

                /*Record New Invoice Payment*/
                var NewInvTransfer = new invoiceTransfer()
                {
                    invoiceID = INV.ID,
                    transferID = ccPayment.TRANSFER.ID
                };
                ctx.invoiceTransfers.AddObject(NewInvTransfer);
                ctx.SaveChanges();

                //Record related transctions [for invoice Transfer]
                var FEE = 1;
                List<transaction> transactions = new List<transaction>();

                account acc_CCCASH = Account.getAccount((int)INV.order.receiverEntityID, (int)projectEnums.catType.CCCASH, (int)INV.currencyID);
                var t1 = new Transaction(-1 * (decimal)amount, acc_CCCASH);
                transactions.Add(t1.TXN);

                account acc_AP = Account.getAccount((int)INV.order.receiverEntityID, (int)projectEnums.catType.AP, (int)INV.currencyID);
                var t2 = new Transaction(+1 * (decimal)amount, acc_AP);
                transactions.Add(t2.TXN);

                account acc_W = Account.getAccount((int)INV.order.issuerEntityID, (int)projectEnums.catType.W, (int)INV.currencyID);
                var t3 = new Transaction(+1 * (decimal)amount  -  FEE, acc_W);
                transactions.Add(t3.TXN);

                account acc_AR = Account.getAccount((int)INV.order.issuerEntityID, (int)projectEnums.catType.AR, (int)INV.currencyID);
                var t4 = new Transaction(-1 * (decimal)amount, acc_AR);
                transactions.Add(t4.TXN);

                account acc_EXP = Account.getAccount((int)INV.order.issuerEntityID, (int)projectEnums.catType.AR, (int)INV.currencyID);
                var t5 = new Transaction(FEE, acc_EXP);
                transactions.Add(t5.TXN);


                /*Record Invoice Payment transactions*/
                this.RecordInvoiceTransferTransactions(transactions, ccPayment.TRANSFER.ID, projectEnums.transferStatus.PaidApproved);

                /*Record Invoice Transaction*/
                projectEnums.invoiceStatus? invoicestat = null;
                switch (ccCardType)
                {
                    case projectEnums.ccCardType.MASTERCARD:
                        invoicestat = projectEnums.invoiceStatus.masterCardTransfer;
                        break;
                    case projectEnums.ccCardType.VISACARD:
                        invoicestat = projectEnums.invoiceStatus.visaCardTransfer;
                        break;
                }

                this.RecordInvoiceTransaction(transactions, (projectEnums.invoiceStatus)invoicestat);

                ts.Complete();
            }


        }
        public void Transfer_Ext_Debit(decimal amount, int cardID)
        {
            if (INV.Equals(null))
                throw new Exception("No invocie set.pleae laod or create new Invoice");


            using (var ctx = new accountingEntities())
            using (var ts = new TransactionScope())
            {
                //create new Internal Payment
                DebitTransfer dbPayment = new DebitTransfer();
                dbPayment.New((int)INV.order.receiverEntityID, (int)INV.order.issuerEntityID, amount, (int)INV.currencyID);

                /*Record New Invoice Payment*/
                var NewInvTransfer = new invoiceTransfer()
                {
                    invoiceID = INV.ID,
                    transferID = dbPayment.TRANSFER.ID
                };
                ctx.invoiceTransfers.AddObject(NewInvTransfer);
                ctx.SaveChanges();

                //Record related transctions [for invoice Transfer]
                var FEE = 1;
                List<transaction> transactions = new List<transaction>();

                account acc_CCCASH = Account.getAccount((int)INV.order.receiverEntityID, (int)projectEnums.catType.DBCASH, (int)INV.currencyID);
                var t1 = new Transaction(-1 * (decimal)amount, acc_CCCASH);
                transactions.Add(t1.TXN);

                account acc_AP = Account.getAccount((int)INV.order.receiverEntityID, (int)projectEnums.catType.AP, (int)INV.currencyID);
                var t2 = new Transaction(+1 * (decimal)amount, acc_AP);
                transactions.Add(t2.TXN);

                account acc_W = Account.getAccount((int)INV.order.issuerEntityID, (int)projectEnums.catType.W, (int)INV.currencyID);
                var t3 = new Transaction(+1 * (decimal)amount - FEE, acc_W);
                transactions.Add(t3.TXN);

                account acc_AR = Account.getAccount((int)INV.order.issuerEntityID, (int)projectEnums.catType.AR, (int)INV.currencyID);
                var t4 = new Transaction(-1 * (decimal)amount, acc_AR);
                transactions.Add(t4.TXN);

                account acc_EXP = Account.getAccount((int)INV.order.issuerEntityID, (int)projectEnums.catType.AR, (int)INV.currencyID);
                var t5 = new Transaction(FEE, acc_EXP);
                transactions.Add(t5.TXN);


                /*Record Invoice Payment transactions*/
                this.RecordInvoiceTransferTransactions(transactions, dbPayment.TRANSFER.ID, projectEnums.transferStatus.PaidApproved);

                /*Record Invoice Transaction*/
                this.RecordInvoiceTransaction(transactions, projectEnums.invoiceStatus.interacTransfer);

                ts.Complete();
            }
        }




        /*cancelling one Transfer OR invoice at the time*/
        public void cancelInvoiceTransfer(int transferID)
        {
            using (var ctx = new accountingEntities())
            using (var ts = new TransactionScope())
            {
                ITransfer invTrasfer=null;

                var extTransferTypeID = -1;
                var transferTypeID = Transfer.getTransferType(transferID);

                switch (transferID) 
                {
                    case 1:
                        extTransferTypeID = ExternalTransfer.getExtTransferType(transferID);

                        if(extTransferTypeID.Equals(1))
                            invTrasfer  = (ITransfer)new CreditTransfer();
                        else
                            invTrasfer  = (ITransfer)new DebitTransfer();
                        break;

                    case 2:
                        invTrasfer = (ITransfer)new InternalTransfer();
                        break;
                }
                
                List<transaction> transactions = invTrasfer.cancelTransfer(projectEnums.transferAction.Void);

                /*Record Invoice Payment transactions*/
                this.RecordInvoiceTransferTransactions(transactions, transferID, projectEnums.transferStatus.VoidApproved);

                /*Record Invoice Transaction*/
                if (!extTransferTypeID.Equals(-1))
                {
                    if (extTransferTypeID == (int)projectEnums.extTransferType.CreditPayment)
                        this.RecordInvoiceTransaction(transactions, projectEnums.invoiceStatus.partialCreditCardTransferCancelled);

                    if (extTransferTypeID == (int)projectEnums.extTransferType.InteracPayment)
                        this.RecordInvoiceTransaction(transactions, projectEnums.invoiceStatus.partialInteracTransferCancelled);
                }
                else 
                {
                    this.RecordInvoiceTransaction(transactions, projectEnums.invoiceStatus.partialInternalTransferCancelled);
                }
                ts.Complete();
            }
        }

        public static List<transfer> getInvoiceAllTransfers(int invoiceID)
        {
            using (var ctx = new accountingEntities())
            {
                var transfers = ctx.invoiceTransfers
                    .Where(x => (int)x.invoiceID == invoiceID)
                    .Select(x => x.transfer)
                    .ToList();
                return transfers;
            }
        }
        /*cancel invoice all payments*/
        public void cancelInvoice()
        {
            if (INV.Equals(null))
                throw new Exception("Invoice Mot Loaded");

            using (var ctx = new accountingEntities())
            using (var ts = new TransactionScope())
            {
                /*get all invoice payments*/
                List<transfer> transfers = Invoice.getInvoiceAllTransfers(INV.ID);

                /*cancel payments one by one*/
                foreach (var tsfr in transfers)
                    cancelInvoiceTransfer(tsfr.ID);


                //Get Sum of Invoice Services added
                decimal invoiceServicesAmt = this.getInvoiceServicesSumAmt();

                //Record related transctions
                List<transaction> transactions = new List<transaction>();

                account acc_AP = Account.getAccount((int)INV.order.receiverEntityID, (int)projectEnums.catType.AP, (int)INV.currencyID);
                var txn1 = new Transaction(+1 * (decimal)invoiceServicesAmt, acc_AP);
                transactions.Add(txn1.TXN);

                account acc_AR = Account.getAccount((int)INV.order.issuerEntityID, (int)projectEnums.catType.AR, (int)INV.currencyID);
                var txn2 = new Transaction(-1 * (decimal)invoiceServicesAmt, acc_AP);
                transactions.Add(txn2.TXN);

                /*Record Invoice Transaction*/
                this.RecordInvoiceTransaction(transactions, projectEnums.invoiceStatus.Cancelled);

                ts.Complete();
            }
        }
       

        public void RecordInvoiceTransaction(List<transaction> transactions, projectEnums.invoiceStatus invoiceStat)
        {
            if (INV.Equals(null))
                throw new Exception("Invoice does not exists");

            using (var ctx = new accountingEntities())
            using (var ts = new TransactionScope())
            {
                //create invoice Action
                var invAction = new invoiceAction()
                {
                    invoiceID = INV.ID,
                    invoiceStatusID = (int)invoiceStat
                };
                ctx.invoiceActions.AddObject(invAction);
                ctx.SaveChanges();


                //create invoice Transactions and invoice action Transactions
                foreach (var item in transactions)
                {
                    var invActionTrans = new sysActionTransaction()
                    {
                        sysActionID = invAction.ID,
                        transactionID = item.ID
                    };
                    ctx.sysActionTransactions.AddObject(invActionTrans);

                    ctx.SaveChanges();
                }

                ts.Complete();

            }
        }

        private void RecordInvoiceTransferTransactions(List<transaction> txns, int transferID, projectEnums.transferStatus transferStat)
        {
            using (var ctx = new accountingEntities())
            {
                //Create Payment Action
                var transferAction = new transferAction()
                {
                    transferID = transferID,
                    transferStatusID = (int)transferStat
                };
                ctx.transferActions.AddObject(transferAction);
                ctx.SaveChanges();

                //Record Pyament Action TXNS
                foreach (var txn in txns)
                {
                    var NewsysActionTxn = new sysActionTransaction()
                    {
                        sysActionID = -1,
                        transactionID = txn.ID
                    };
                    ctx.sysActionTransactions.AddObject(NewsysActionTxn);
                    ctx.SaveChanges();
                }
            }
        }
    }
}
