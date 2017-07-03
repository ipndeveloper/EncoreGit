--*******************************************************************************
-- Table of Contents - Start
--*******************************************************************************
-- Alert Widget
-- Activate Email Widget
-- 
--*******************************************************************************
-- Table of Contents - End
--*******************************************************************************

--*******************************************************************************
-- Alert Widget
--*******************************************************************************

GO

DECLARE @WIDGETNAME NVARCHAR(50)

SET @WIDGETNAME = 'Alerts'

IF NOT EXISTS(SELECT * FROM Widgets WHERE Name = @WIDGETNAME)
BEGIN

	DECLARE @ALERTWIDGETID INT
	DECLARE @SITEID INT
	DECLARE @DISPLAYCOLUMN INT
	DECLARE @WIDGETVIEWNAME NVARCHAR(50)
	DECLARE @SORTINDEX INT
	DECLARE @ISONTOP BIT
	DECLARE @EDITABLE BIT

	SET @SITEID = 420
	SET @DISPLAYCOLUMN = 3   
	SET @WIDGETVIEWNAME = 'Alerts'
	SET @ISONTOP = 0
	SET @EDITABLE = 1
  
	SELECT @SORTINDEX = MAX(SortIndex) + 1 FROM SiteWidgets WHERE DisplayColumn = @DISPLAYCOLUMN

	INSERT INTO Widgets (Name, ViewName, Active) VALUES (@WIDGETNAME, @WIDGETVIEWNAME, 1)

	SET @ALERTWIDGETID = SCOPE_IDENTITY()
	
	INSERT INTO SiteWidgets (SiteID, WidgetID, DisplayColumn, SortIndex, IsOnTop, Editable) VALUES(@SITEID, @ALERTWIDGETID, @DISPLAYCOLUMN, @SORTINDEX, @ISONTOP, @EDITABLE)

END

GO

--*******************************************************************************
-- Activate Email Widget
--*******************************************************************************

GO

IF EXISTS (SELECT * FROM Widgets WHERE Name = 'Email Overview')
BEGIN

   UPDATE Widgets SET Active = 1 WHERE Name = 'Email Overview'
   
END

GO

--*******************************************************************************
-- 
--*******************************************************************************

GO