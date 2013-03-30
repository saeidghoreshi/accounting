--Delete Lookup Tables
 delete from accounting.lu_catType
 delete from accounting.lu_glType
 delete from accounting.lu_deliverableType
 delete from accounting.lu_contactType
 delete from accounting.lu_entityType
 delete from accounting.lu_currencyType
 delete from accounting.lu_transferType
 delete from accounting.lu_externalTsfrType
 delete from accounting.lu_transferActionType
 delete from accounting.lu_transferStatus
 delete from accounting.lu_invoiceStatus
 delete from accounting.lu_invoiceActionType
 delete from accounting.lu_cardType
 delete from accounting.lu_ccCardType

 --GLTYPE
 insert into Accounting.lu_glType(id,name)values(1,'ASSET');
 insert into Accounting.lu_glType(id,name)values(2,'OE');
 insert into Accounting.lu_glType(id,name)values(3,'LIB');
 
 --CATEGORYTYPE 
 insert into Accounting.lu_catType(id,name,glTypeID)values(1,'AR',1);
 insert into Accounting.lu_catType(id,name,glTypeID)values(2,'W',1);
 insert into Accounting.lu_catType(id,name,glTypeID)values(3,'DBCASH',1);
 insert into Accounting.lu_catType(id,name,glTypeID)values(4,'CCCASH',1);
 insert into Accounting.lu_catType(id,name,glTypeID)values(8,'INC',2);
 insert into Accounting.lu_catType(id,name,glTypeID)values(9,'EXP',2);
 insert into Accounting.lu_catType(id,name,glTypeID)values(10,'AP',3);
 
 --CardType
 insert into Accounting.lu_cardType (id,name)values(1,'DEBITCARD');
 insert into Accounting.lu_cardType (id,name)values(2,'CREDITCARD');
 
 --ccCardType
 insert into Accounting.lu_ccCardType (id,name)values(1,'MASTERCARD');
 insert into Accounting.lu_ccCardType (id,name)values(2,'VISACARD');
 
 --CurrencyType
 insert into Accounting.lu_currencyType (id,name)values(1,'REAL');
 insert into Accounting.lu_currencyType (id,name)values(2,'UNREAL');
 
 --entityType
 insert into Accounting.lu_entityType (id,name)values(1,'Person');
 insert into Accounting.lu_entityType (id,name)values(2,'Deliverable');
 insert into Accounting.lu_entityType (id,name)values(3,'Card');
 insert into Accounting.lu_entityType (id,name)values(4,'Organization');
 insert into Accounting.lu_entityType (id,name)values(5,'Account');
 insert into Accounting.lu_entityType (id,name)values(6,'Order');
 insert into Accounting.lu_entityType (id,name)values(7,'Invoice');
 insert into Accounting.lu_entityType (id,name)values(8,'Transaction');
 insert into Accounting.lu_entityType (id,name)values(9,'Transfer');
 
 --invoiceStat
 insert into Accounting.lu_invoiceStatus (id,name)values(1,'Generated');
 insert into Accounting.lu_invoiceStatus(id,name)values(2,'Finalized');
 insert into Accounting.lu_invoiceStatus (id,name)values(3,'Deleted');
 insert into Accounting.lu_invoiceStatus (id,name)values(4,'Cancelled');
 insert into Accounting.lu_invoiceStatus(id,name)values(5,'internalPaymant');
 insert into Accounting.lu_invoiceStatus(id,name)values(6,'interacPaymant');
 insert into Accounting.lu_invoiceStatus(id,name)values(7,'visaCardPaymant');
 insert into Accounting.lu_invoiceStatus(id,name)values(8,'masterCardPaymant');
 insert into Accounting.lu_invoiceStatus(id,name)values(9,'partialInternalPaymantCancelled');
 insert into Accounting.lu_invoiceStatus(id,name)values(10,'partialInteracPaymantCancelled');
 insert into Accounting.lu_invoiceStatus(id,name)values(11,'partialCreditCardPaymantCancelled');
 
 --external Payment Type
 insert into Accounting.lu_externalTsfrType(id,name)values(1,'CreditPayment');
 insert into Accounting.lu_externalTsfrType(id,name)values(2,'InteracPayment');
 
 --Payment Type
 insert into Accounting.lu_transferType(id,name)values(1,'External');
 insert into Accounting.lu_transferType(id,name)values(2,'Internal');
 
 --Transfer Status
 insert into Accounting.lu_transferStatus(id,name)values(1,'PaidApproved');
 insert into Accounting.lu_transferStatus(id,name)values(2,'VoidApproved');
 insert into Accounting.lu_transferStatus(id,name)values(3,'RefundApproved');
 insert into Accounting.lu_transferStatus(id,name)values(4,'NotApprovedPaid');
 insert into Accounting.lu_transferStatus(id,name)values(5,'NotApprovedVoid');
 insert into Accounting.lu_transferStatus(id,name)values(6,'NotApprovedRefund');
 
 insert into accounting.lu_deliverableType(ID,name)values(1,'Service')
 insert into accounting.lu_deliverableType(ID,name)values(2,'Product')
 
 insert into accounting.lu_contactType(ID,name)values(1,'Email')
 insert into accounting.lu_contactType(ID,name)values(2,'Primary Phone')
 
 insert into  accounting.lu_transferActionType(ID,name)values(1,'Void')
 insert into  accounting.lu_transferActionType(ID,name)values(2,'Refund')
 
 insert into  accounting.lu_invoiceActionType(ID,name)values(1,'CancelInvoice')
 insert into  accounting.lu_invoiceActionType(ID,name)values(2,'CancelTransfer')
 insert into  accounting.lu_invoiceActionType(ID,name)values(3,'Delete')
 insert into  accounting.lu_invoiceActionType(ID,name)values(4,'Finalize')
 insert into  accounting.lu_invoiceActionType(ID,name)values(5,'Transfer')


 select * from accounting.lu_glType
 select * from accounting.lu_catType
 select * from accounting.lu_deliverableType
 select * from accounting.lu_contactType
 select * from accounting.lu_entityType
 select * from accounting.lu_currencyType
 select * from accounting.lu_transferType
 select * from accounting.lu_externalTsfrType
 select * from accounting.lu_transferActionType
 select * from accounting.lu_transferStatus
 select * from accounting.lu_invoiceStatus
 select * from accounting.lu_invoiceActionType
 select * from accounting.lu_cardType
 select * from accounting.lu_ccCardType
 
 
 
 