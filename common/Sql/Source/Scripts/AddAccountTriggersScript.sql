
DROP TRIGGER AuditAccountsTable
DROP TRIGGER AuditAccountPublicContactInfoTable
DROP TRIGGER AuditAccountLanguagesTable
DROP TRIGGER AuditAccountPhonesTable
DROP TRIGGER AuditAccountPaymentMethodsTable
DROP TRIGGER AuditAccountPoliciesTable
DROP TRIGGER AuditAccountSponsorsTable
DROP TRIGGER AuditAddressesTable
DROP TRIGGER AuditNotesTable


GO
CREATE TRIGGER AuditAccountsTable
   ON [dbo].[Accounts]
   FOR INSERT,DELETE,UPDATE
AS 
EXTERNAL NAME NetStepsSql.Triggers.AuditTrigger

GO
CREATE TRIGGER AuditAccountPublicContactInfoTable
   ON [dbo].[AccountPublicContactInfo]
   FOR INSERT,DELETE,UPDATE
AS 
EXTERNAL NAME NetStepsSql.Triggers.AuditTrigger

GO
CREATE TRIGGER AuditAccountLanguagesTable
   ON [dbo].[AccountLanguages]
   FOR INSERT,DELETE,UPDATE
AS 
EXTERNAL NAME NetStepsSql.Triggers.AuditTrigger

GO
CREATE TRIGGER AuditAccountPhonesTable
   ON [dbo].[AccountPhones]
   FOR INSERT,DELETE,UPDATE
AS 
EXTERNAL NAME NetStepsSql.Triggers.AuditTrigger

GO
CREATE TRIGGER AuditAccountPaymentMethodsTable
   ON [dbo].[AccountPaymentMethods]
   FOR INSERT,DELETE,UPDATE
AS 
EXTERNAL NAME NetStepsSql.Triggers.AuditTrigger

GO
CREATE TRIGGER AuditAccountSponsorsTable
   ON [dbo].[AccountSponsors]
   FOR INSERT,DELETE,UPDATE
AS 
EXTERNAL NAME NetStepsSql.Triggers.AuditTrigger

GO
CREATE TRIGGER AuditAccountPoliciesTable
   ON [dbo].[AccountPolicies]
   FOR INSERT,DELETE,UPDATE
AS 
EXTERNAL NAME NetStepsSql.Triggers.AuditTrigger

GO
CREATE TRIGGER AuditAddressesTable
   ON [dbo].[Addresses]
   FOR INSERT,DELETE,UPDATE
AS 
EXTERNAL NAME NetStepsSql.Triggers.AuditTrigger

GO
CREATE TRIGGER AuditNotesTable
   ON [dbo].[Notes]
   FOR INSERT,DELETE,UPDATE
AS 
EXTERNAL NAME NetStepsSql.Triggers.AuditTrigger


