
DROP TRIGGER AuditAccountStatusChangeReasonsTable
DROP TRIGGER AuditArchiveTypesTable
DROP TRIGGER AuditNewsTypesTable
DROP TRIGGER AuditReturnReasonsTable
DROP TRIGGER AuditReturnTypesTable
DROP TRIGGER AuditAccountListValuesTable


GO
CREATE TRIGGER AuditAccountStatusChangeReasonsTable
   ON [dbo].[AccountStatusChangeReasons]
   FOR INSERT,DELETE,UPDATE
AS 
EXTERNAL NAME NetStepsSql.Triggers.AuditTrigger

GO
CREATE TRIGGER AuditArchiveTypesTable
   ON [dbo].[ArchiveTypes]
   FOR INSERT,DELETE,UPDATE
AS 
EXTERNAL NAME NetStepsSql.Triggers.AuditTrigger

GO
CREATE TRIGGER AuditNewsTypesTable
   ON [dbo].[NewsTypes]
   FOR INSERT,DELETE,UPDATE
AS 
EXTERNAL NAME NetStepsSql.Triggers.AuditTrigger

GO
CREATE TRIGGER AuditReturnReasonsTable
   ON [dbo].[ReturnReasons]
   FOR INSERT,DELETE,UPDATE
AS 
EXTERNAL NAME NetStepsSql.Triggers.AuditTrigger

GO
CREATE TRIGGER AuditReturnTypesTable
   ON [dbo].[ReturnTypes]
   FOR INSERT,DELETE,UPDATE
AS 
EXTERNAL NAME NetStepsSql.Triggers.AuditTrigger

GO
CREATE TRIGGER AuditAccountListValuesTable
   ON [dbo].[AccountListValues]
   FOR INSERT,DELETE,UPDATE
AS 
EXTERNAL NAME NetStepsSql.Triggers.AuditTrigger



