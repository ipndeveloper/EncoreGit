-- ====================================================
-- **GO STATEMENTS**
-- Plain GO statements will not execute properly!!!!!
-- Please avoid using GO statements, or if you need to
-- use a GO then append a --GO to the end of the GO 
-- statement without any spaces as shown below.
--
-- i.e.   GO--GO
--
-- **TRANSACTIONS**
-- Transactions are NOT Supported! The run once logic
-- will roll back if there is a problem.
--
-- **TEMP TABLES**
-- If you want to use temporary tables please use 
-- global temp tables. 
-- 
-- i.e.   ##
-- ====================================================

USE [BelcorpBRACore]
GO
/****** Object:  StoredProcedure [dbo].[upsValidateHolidays]    Script Date: 31/05/2017 04:50:06 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/* modificado por Hundred = > de cambio el tipo de dato a date time del parametro @DateHoliday */

ALTER proc [dbo].[upsValidateHolidays] -- upsValidateHolidays  79,105, '2016-10-03'
@HolidayID int,
@StateID  int,
@DateHoliday  DATETIME
as
begin
select  *  from Holiday 
where 
StateID=@StateID
and 
DateHoliday=@DateHoliday
and 
(HolidayID<>@HolidayID )

end




