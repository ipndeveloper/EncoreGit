
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		John Egbert
-- Create date: 06-16-2010
-- Description:	Return the AccountIDs of of Accounts with 'Main' Addresses @withinMilesProximity of the GeoCode passed in.
-- =============================================
ALTER PROCEDURE [dbo].[usp_accounts_distributor_locator_lookup] 
	@lat2Degrees decimal(15,12),
	@lon2Degrees decimal(15,12),
	@withinMilesProximity int
AS 
BEGIN 
    SET NOCOUNT ON 
  
		SELECT a.AccountID, dbo.LatLonRadiusDistance(Latitude, Longitude, @lat2Degrees, @lon2Degrees) as MilesAway
		FROM Accounts a
		JOIN AccountAddresses aa on a.AccountID = aa.AccountID
		JOIN Addresses ad on aa.AddressID = ad.AddressID
		WHERE Latitude is not null and Longitude is not null
		AND ad.AddressTypeID = 1
		AND dbo.LatLonRadiusDistance(Latitude, Longitude, @lat2Degrees, @lon2Degrees) < @withinMilesProximity
END