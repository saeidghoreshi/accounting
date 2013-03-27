using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project.Controllers
{
    public class glType
    {
        public int ID { get; set; }
        public string name { get; set; }
    }
    public class catType
    {
        public int ID { get; set; }
        public string name { get; set; }
        public int glTypeID { get; set; }
    }

    public static class enumsController
    {
        public static List<glType> glTypes = new List<glType>() 
        {
            new glType{ID=1,name="ASSET"},
            new glType{ID=2,name="OE"},
            new glType{ID=3,name="LIB"}
        };

        public static List<catType> catTypes = new List<catType>() 
        {
            new catType{ID=1,name="AR",glTypeID=1},
            new catType{ID=2,name="W",glTypeID=1},
            new catType{ID=3,name="DBCASH",glTypeID=1},
            new catType{ID=4,name="CCASH",glTypeID=1},

            new catType{ID=8,name="INC",glTypeID=2},
            new catType{ID=9,name="EXP",glTypeID=2},

            new catType{ID=10,name="AP",glTypeID=3}
        };

        
        public enum entityType
        {
            Organization = 1,
            Person = 2,
            Card=3,
            invoice=4,
            transfer=5
        }
        public enum transferType
        {
            External = 1,
            Internal = 2
        }
        public enum extTransferType
        {
            CreditPayment = 1,
            InteracPayment = 2
        }
        public enum cardType
        {
            DebitCard = 1,
            CreditCard = 2
        }
        public enum ccCardType
        {
            MASTERCARD = 1,
            VISACARD = 2
        }
        public enum invoiceAction 
        {
            CancelInvoice=1,
            CancelPayment = 2,
            Delete=3,
            Finalize=4,
            payment=5
        }
        public enum invoiceStatus
        {
            Generated = 1,
            Finalized = 2,
            Deleted = 3       /*If no Payments of any kind ever happend*/,
            Cancelled = 4     /*if no payments occured or all payments voided or refunded*/,

            internalPaymant = 5,
            interacPaymant = 6,
            visaCardPaymant = 7,
            masterCardPaymant = 8,

            partialInternalPaymantCancelled = 9,
            partialInteracPaymantCancelled = 10,
            partialCreditCardPaymantCancelled = 11
        }
        public enum transferStatus
        {
            PaidApproved = 1,
            VoidApproved = 2,
            RefundApproved = 3,
            NotApprovedPaid = 4,
            NotApprovedVoid = 5,
            NotApprovedRefund = 6
        }
        public enum transferAction
        {
            Void = 1,
            Refund = 2
        }
        public enum sysAction
        {
            transfer=1,
            invoice=2
        }
        public enum currencyType
        {
            Real = 1,
            UnReal = 2
        }
    }
}
