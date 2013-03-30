delete from Accounting.transferAction;
delete from Accounting.invoiceAction;
delete from Accounting.sysActionTransaction;
delete from Accounting.sysAction;
delete from Accounting.[transaction];
delete from Accounting.invoiceTransfer;
delete from Accounting.invoice;
delete from Accounting.orderDetail;
delete from Accounting.[order];
delete from Accounting.creditTransfer;
delete from Accounting.debitTransfer;
delete from Accounting.externalTransfer;
delete from Accounting.internalTransfer;
delete from Accounting.[transfer];
delete from Accounting.account;
delete from Accounting.currency;
delete from Accounting.debitCard;
delete from Accounting.ccCard;
delete from Accounting.entityCard;
delete from Accounting.[card];
delete from Accounting.product;
delete from Accounting.[service];
delete from Accounting.deliverable;
delete from Accounting.carrierContact;
delete from Accounting.carrier;
delete from Accounting.supplier;
delete from Accounting.customer;
delete from Accounting.organization;
delete from Accounting.[user];
delete from Accounting.person;
delete from Accounting.contact;
delete from Accounting.[address];
delete from Accounting.entity;



DBCC CHECKIDENT ( 'Accounting.sysActionTransaction',reseed, 0);
DBCC CHECKIDENT ( 'Accounting.sysAction',reseed, 0);
DBCC CHECKIDENT ( 'Accounting.[transaction]',reseed, 0);
DBCC CHECKIDENT ( 'Accounting.invoiceTransfer',reseed, 0);
DBCC CHECKIDENT ( 'Accounting.orderDetail',reseed, 0);
DBCC CHECKIDENT ( 'Accounting.[order]',reseed, 0);
DBCC CHECKIDENT ( 'Accounting.internalTransfer',reseed, 0);
DBCC CHECKIDENT ( 'Accounting.account',reseed, 0);
DBCC CHECKIDENT ( 'Accounting.currency',reseed, 0);
DBCC CHECKIDENT ( 'Accounting.debitCard',reseed, 0);
DBCC CHECKIDENT ( 'Accounting.ccCard',reseed, 0);
DBCC CHECKIDENT ( 'Accounting.entityCard',reseed, 0);
DBCC CHECKIDENT ( 'Accounting.[card]',reseed, 0);
DBCC CHECKIDENT ( 'Accounting.product',reseed, 0);
DBCC CHECKIDENT ( 'Accounting.[service]',reseed, 0);
DBCC CHECKIDENT ( 'Accounting.deliverable',reseed, 0);
DBCC CHECKIDENT ( 'Accounting.carrierContact',reseed, 0);
DBCC CHECKIDENT ( 'Accounting.carrier',reseed, 0);
DBCC CHECKIDENT ( 'Accounting.supplier',reseed, 0);
DBCC CHECKIDENT ( 'Accounting.customer',reseed, 0);
DBCC CHECKIDENT ( 'Accounting.organization',reseed, 0);
DBCC CHECKIDENT ( 'Accounting.[user]',reseed, 0);
DBCC CHECKIDENT ( 'Accounting.person',reseed, 0);
DBCC CHECKIDENT ( 'Accounting.contact',reseed, 0);
DBCC CHECKIDENT ( 'Accounting.[address]',reseed, 0);
DBCC CHECKIDENT ( 'Accounting.entity',reseed, 0);;
 
 
 


