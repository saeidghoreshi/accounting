using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.ServiceModel.Activation;


using Accounting.Definition;
using Accounting.Definition.Structure;

namespace Accounting.Definition.Contracts.v1
{
    [DataContract(Namespace = "accounting")]
    public class A_customer
    {
        [DataMember(Order=1)]
        public string firstname;
        [DataMember(Order = 2)]
        public string lastname;
        [DataMember(Order = 3)]
        public int curId;
    }
    [DataContract(Namespace = "accounting")]
    public class A_service
    {
        [DataMember(Order=1)]
        public string servicename;
        [DataMember(Order=2)]
        public int issuerEntityId;
        [DataMember(Order=3)]
        public int receiverEntityId;
    }
    [DataContract(Namespace = "accounting")]
    public class A_invoice
    {
        [DataMember(Order=1)]
        public int issuerEntityId;
        [DataMember(Order=2)]
        public int receiverEntityId;
        [DataMember(Order=3)]
        public int curId;
    }
    [DataContract(Namespace = "accounting")]
    public class A_InvoiceService
    {
        [DataMember(Order = 1)]
        public int invoiceId;
        [DataMember(Order = 2)]
        public int serviceId;
        [DataMember(Order = 3)]
        public decimal amount;
    }
    [DataContract(Namespace = "accounting")]
    public class A_newbank
    {
        [DataMember(Order=1)]
        public string bankname;
    }
    [DataContract(Namespace = "accounting")]
    public class A_finalizeInvoice
    {
        [DataMember(Order=1)]
        public int invoiceId;
    }
    [DataContract(Namespace = "accounting")]
    public class A_setBankInteracFee
    {
        [DataMember(Order=1)]
        public int bankId;
        [DataMember(Order=2)]
        public decimal amount;
        [DataMember(Order=3)]
        public string description;
    }
    [DataContract(Namespace = "accounting")]
    public class A_setBankCreditcardFee
    {
        [DataMember(Order=1)]
        public int bankId;
        [DataMember(Order=2)]
        public int ccCardTypeId;
        [DataMember(Order=3)]
        public decimal amount;
        [DataMember(Order=4)]
        public string description;
    }
    [DataContract(Namespace = "accounting")]
    public  class A_newCard
    {
        [DataMember(Order=1)]
        public string cardNumber;
        [DataMember(Order=2)]
        public string expirydate;
    }
    [DataContract(Namespace = "accounting")]
    public class A_assignCardToBank
    {
        [DataMember(Order=1)]
        public int cardId;
        [DataMember(Order=2)]
        public int bankId;
    }
    [DataContract(Namespace = "accounting")]
    public class A_assignCardToPerson
    {
        [DataMember(Order=1)]
        public int cardId;
        [DataMember(Order=2)]
        public int personEntityId;
    }
    [DataContract(Namespace = "accounting")]
    public class A_addWallet
    {
        [DataMember(Order=1)]
        public int personEntityId;
        [DataMember(Order=2)]
        public decimal amount;
        [DataMember(Order=3)]
        public int curId;
        [DataMember(Order=4)]
        public string description;
    }
    [DataContract(Namespace = "accounting")]
    public class A_invoiceSum
    {
        [DataMember(Order=1)]
        public int invoiceId;
    }
    [DataContract(Namespace = "accounting")]
    public class A_payInvoiceCredit
    {
        [DataMember(Order=1)]
        public int invoiceId;
        [DataMember(Order=2)]
        public decimal amount;
        [DataMember(Order=3)]
        public int cardId;
        [DataMember(Order=4)]
        public int ccCardTypeId;
    }
    [DataContract(Namespace = "accounting")]
    public class  A_payInvoiceInterac
    {
        [DataMember(Order=1)]
        public int invoiceId;
        [DataMember(Order=2)]
        public decimal amount;
        [DataMember(Order=3)]
        public int cardId;
    }
    [DataContract(Namespace = "accounting")]
    public class  A_payInvoiceInternal
    {
        [DataMember(Order=1)]
        public int invoiceId;
        [DataMember(Order=2)]
        public decimal amount;
    }
    [DataContract(Namespace = "accounting")]
    public class  A_cancelInvoicePayExt
    {
        [DataMember(Order=1)]
        public int invoiceId;
        [DataMember(Order=2)]
        public int paymentId;
    }
    [DataContract(Namespace = "accounting")]
    public class  A_cancelInvoicePayInt
    {
        [DataMember(Order=1)]
        public int invoiceId;
        [DataMember(Order=2)]
        public int paymentId;
    }
    [DataContract(Namespace = "accounting")]
    public class  A_cancelInvoice
    {
        [DataMember(Order=1)]
        public int invoiceId;
    }
    
    

    [ServiceContract(SessionMode = SessionMode.Allowed)]
    public interface IContracts
    {
        
        [WebInvoke(Method = "POST", UriTemplate = "Customer/new", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Xml)]
        [OperationContract]
        Person newCustomer(A_customer I);

        [WebInvoke(Method = "POST", UriTemplate = "Service/new", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Xml)]
        [OperationContract]
        Deliverable createDeliverable(A_service I);

        [WebInvoke(Method = "POST", UriTemplate = "Invoice/new", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Xml)]
        [OperationContract]
        Invoice createinvoice(A_invoice I);

        [WebInvoke(Method = "POST", UriTemplate = "invoice/service/add", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Xml)]
        [OperationContract]
        void createInvoiceService(A_InvoiceService I);

        [WebInvoke(Method = "POST", UriTemplate = "invoice/finalize", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Xml)]
        [OperationContract]
        void finalizeInvoice(A_finalizeInvoice I);

        [WebInvoke(Method = "POST", UriTemplate = "Bank/new", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Xml)]
        [OperationContract]
        void createBank(A_newbank I);

        [WebInvoke(Method = "POST", UriTemplate = "Bank/setFee/IntracCardType", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Xml)]
        [OperationContract]
        void setFeeForIntracCardType(A_setBankInteracFee I);

        [WebInvoke(Method = "POST", UriTemplate = "Bank/setFee/CreditCardType", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Xml)]
        [OperationContract]
        void setFeeForCreditCardType(A_setBankCreditcardFee I);

        [WebInvoke(Method = "POST", UriTemplate = "Card/Master/new", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Xml)]
        [OperationContract]
        void createMasterCard(A_newCard I);

        [WebInvoke(Method = "POST", UriTemplate = "Card/Visa/new", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Xml)]
        [OperationContract]
        void createVisaCard(A_newCard I);

        [WebInvoke(Method = "POST", UriTemplate = "Card/Debit/new", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Xml)]
        [OperationContract]
        void createDebitCard(A_newCard I);

        [WebInvoke(Method = "POST", UriTemplate = "Card/assignToBank", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Xml)]
        [OperationContract]
        void assignCardToBank(A_assignCardToBank I);

        [WebInvoke(Method = "POST", UriTemplate = "Card/AssignToPerson", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Xml)]
        [OperationContract]
        void assignCardToPerson(A_assignCardToPerson I);

        //Transactions

        [WebInvoke(Method = "POST", UriTemplate = "Person/txn/addWallet", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Xml)]
        [OperationContract]
        void addWalletMoney(A_addWallet I);

        [WebInvoke(Method = "POST", UriTemplate = "Invoice/sum", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Xml)]
        [OperationContract]
        decimal getInvoiceServicesSumAmt(A_invoiceSum I);

          
        [WebInvoke(Method = "POST", UriTemplate = "Invoice/Pay/Credit", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Xml)]
        [OperationContract]
        void payInvoiceByCC(A_payInvoiceCredit I);

        [WebInvoke(Method = "POST", UriTemplate = "Invoice/Pay/Interac", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Xml)]
        [OperationContract]
        void payInvoiceByInterac(A_payInvoiceInterac I);

        [WebInvoke(Method = "POST", UriTemplate = "Invoice/Pay/Internal", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Xml)]
        [OperationContract]
        void payInvoiceByInternal(A_payInvoiceInternal I);

        //Invoice Payment Cancellation

        [WebInvoke(Method = "POST", UriTemplate = "Invoice/Payment/Cancel/Ext", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Xml)]
        [OperationContract]
        void cancelInvoicePaymentEXT(A_cancelInvoicePayExt I);

        [WebInvoke(Method = "POST", UriTemplate = "Invoice/Payment/Cancel/INT", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Xml)]
        [OperationContract]
        void cancelInvoicePaymentINTERNAL(A_cancelInvoicePayInt I);

        //Invoice Cancellation
        [WebInvoke(Method = "POST", UriTemplate = "Invoice/Cancel", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Xml)]
        [OperationContract]
        void cancelInvoice(A_cancelInvoice I);

        //TEST
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "test", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        A_customer test(A_customer x);
        /*
            ALSO NEEDS
            <add name="Access-Control-Allow-Headers" value="Content-Type"/>
            <add name="Access-Control-Allow-Methods" value="POST, GET, OPTIONS"/>
         */
    }

    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class Contract1: IContracts
    {
        public void reset()
        {
            throw new NotImplementedException();
        }

        public Person newCustomer(A_customer I)
        {
            throw new NotImplementedException();
        }

        public Deliverable createDeliverable(A_service I)
        {
            throw new NotImplementedException();
        }

        public Invoice createinvoice(A_invoice I)
        {
            throw new NotImplementedException();
        }

        public void createInvoiceService(A_InvoiceService I)
        {
            throw new NotImplementedException();
        }

        public void finalizeInvoice(A_finalizeInvoice I)
        {
            throw new NotImplementedException();
        }

        public void createBank(A_newbank I)
        {
            throw new NotImplementedException();
        }

        public void setFeeForIntracCardType(A_setBankInteracFee I)
        {
            throw new NotImplementedException();
        }

        public void setFeeForCreditCardType(A_setBankCreditcardFee I)
        {
            throw new NotImplementedException();
        }

        public void createMasterCard(A_newCard I)
        {
            throw new NotImplementedException();
        }

        public void createVisaCard(A_newCard I)
        {
            throw new NotImplementedException();
        }

        public void createDebitCard(A_newCard I)
        {
            throw new NotImplementedException();
        }

        public void assignCardToBank(A_assignCardToBank I)
        {
            throw new NotImplementedException();
        }

        public void assignCardToPerson(A_assignCardToPerson I)
        {
            throw new NotImplementedException();
        }

        public void addWalletMoney(A_addWallet I)
        {
            throw new NotImplementedException();
        }

        public decimal getInvoiceServicesSumAmt(A_invoiceSum I)
        {
            throw new NotImplementedException();
        }

        public void payInvoiceByCC(A_payInvoiceCredit I)
        {
            throw new NotImplementedException();
        }

        public void payInvoiceByInterac(A_payInvoiceInterac I)
        {
            throw new NotImplementedException();
        }

        public void payInvoiceByInternal(A_payInvoiceInternal I)
        {
            throw new NotImplementedException();
        }

        public void cancelInvoicePaymentEXT(A_cancelInvoicePayExt I)
        {
            throw new NotImplementedException();
        }

        public void cancelInvoicePaymentINTERNAL(A_cancelInvoicePayInt I)
        {
            throw new NotImplementedException();
        }

        public void cancelInvoice(A_cancelInvoice I)
        {
            throw new NotImplementedException();
        }

        public A_customer test(A_customer x)
        {
            throw new NotImplementedException();
        }
    }
}

/*
 Ajax
 * $.ajax(
{
	url:"http://localhost:555/srv.svc/rest/test",
	type:"POST",
	contentType: "application/json; charset=utf-8",
	dataType:"json",
	data:JSON.stringify({x:{"firstname":"SAEID","lastname":"GHOR","curId":1}})
	
})
 */