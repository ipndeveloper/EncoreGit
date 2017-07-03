
DROP TRIGGER AuditHtmlContentTable


GO
CREATE TRIGGER AuditHtmlContentTable
   ON [dbo].[HtmlContent]
   FOR INSERT,DELETE,UPDATE
AS 
EXTERNAL NAME NetStepsSql.Triggers.AuditTrigger

