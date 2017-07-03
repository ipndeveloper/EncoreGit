
DROP TRIGGER AuditOrdersTable
DROP TRIGGER AuditOrderItemsTable
DROP TRIGGER AuditOrderItemReturnsTable
DROP TRIGGER AuditOrderCustomersTable
DROP TRIGGER AuditOrderPaymentsTable
DROP TRIGGER AuditOrderPaymentResultsTable


GO
CREATE TRIGGER AuditOrdersTable
   ON [dbo].[Orders]
   FOR INSERT,DELETE,UPDATE
AS 
EXTERNAL NAME NetStepsSql.Triggers.AuditTrigger

GO
CREATE TRIGGER AuditOrderItemsTable
   ON [dbo].[OrderItems]
   FOR INSERT,DELETE,UPDATE
AS 
EXTERNAL NAME NetStepsSql.Triggers.AuditTrigger

GO
CREATE TRIGGER AuditOrderItemReturnsTable
   ON [dbo].[OrderItemReturns]
   FOR INSERT,DELETE,UPDATE
AS 
EXTERNAL NAME NetStepsSql.Triggers.AuditTrigger

GO
CREATE TRIGGER AuditOrderCustomersTable
   ON [dbo].[OrderCustomers]
   FOR INSERT,DELETE,UPDATE
AS 
EXTERNAL NAME NetStepsSql.Triggers.AuditTrigger

GO
CREATE TRIGGER AuditOrderPaymentsTable
   ON [dbo].[OrderPayments]
   FOR INSERT,DELETE,UPDATE
AS 
EXTERNAL NAME NetStepsSql.Triggers.AuditTrigger

GO
CREATE TRIGGER AuditOrderPaymentResultsTable
   ON [dbo].[OrderPaymentResults]
   FOR INSERT,DELETE,UPDATE
AS 
EXTERNAL NAME NetStepsSql.Triggers.AuditTrigger

