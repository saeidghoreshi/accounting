using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Accounting.Definition.Structure
{
    public class GLType
    {
        public int ID { get; set; }
        public string name { get; set; }
    }
    public class CATType
    {
        public int ID { get; set; }
        public string name { get; set; }
        public int glTypeID { get; set; }
    }

    public static class projectEnums
    {
        public static List<GLType> glTypes = new List<GLType>() 
        {
            new GLType{ID=1,name="ASSET"},
            new GLType{ID=2,name="OE"},
            new GLType{ID=3,name="LIB"}
        };

        public static List<CATType> catTypes = new List<CATType>() 
        {
            new CATType{ID=1,name=catType.AR.ToString(),glTypeID=(int)glType.ASSET},
            new CATType{ID=2,name=catType.W.ToString(),glTypeID=(int)glType.ASSET},
            new CATType{ID=3,name=catType.DBCASH.ToString(),glTypeID=(int)glType.ASSET},
            new CATType{ID=4,name=catType.CCCASH.ToString(),glTypeID=(int)glType.ASSET},

            new CATType{ID=5,name=catType.INC.ToString(),glTypeID=(int)glType.OE},
            new CATType{ID=6,name=catType.EXP.ToString(),glTypeID=(int)glType.OE},

            new CATType{ID=7,name=catType.AP.ToString(),glTypeID=(int)glType.LIB}
        };

        public enum glType 
        {
            ASSET=1,
            OE=2,
            LIB=3
        }
        public enum catType
        {
            AR = 1,
            W = 2,
            DBCASH = 3,
            CCCASH=4,
            INC=5,
            EXP=6,
            AP=7
        }
        public enum entityType
        {
            Person = 1,
            Deliverable=2,
            Card=3,
            Organization = 4,
            Account=5,
            Order=6,
            invoice=7,
            Transaction=8,
            transfer=9
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

            internalTransfer = 5,
            interacTransfer = 6,
            visaCardTransfer = 7,
            masterCardTransfer = 8,

            partialInternalTransferCancelled = 9,
            partialInteracTransferCancelled = 10,
            partialCreditCardTransferCancelled = 11
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
