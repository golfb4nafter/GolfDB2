
-- =============================================
-- Author:		James O. Smith Jr.
-- Create date: 2017-1-4
-- Description:	
-- =============================================
--CREATE PROCEDURE ProcedureName 
--	-- Add the parameters for the stored procedure here
--	@HoleId int = 0, 

CREATE PROCEDURE [dbo].[insertTeeTime]
	@TeeTimeOffset int,
    @Tee_Time datetime,
    @CourseId int,
    @EventId int,
    @ReservedByName nvarchar,
    @TelephoneNumber nchar,
    @HoleId int,
    @NumberOfPlayers int,
    @PlayerNames nvarchar

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @errorNumber int;
	SET @errorNumber = 0;

BEGIN TRY  

INSERT INTO [dbo].[TeeTime]
           ([TeeTimeOffset]
           ,[Tee_Time]
           ,[CourseId]
           ,[EventId]
           ,[ReservedByName]
           ,[TelephoneNumber]
           ,[HoleId]
           ,[NumberOfPlayers]
           ,[PlayerNames])
     VALUES
           (@TeeTimeOffset
           ,@Tee_Time
           ,@CourseId
           ,@EventId
           ,@ReservedByName
           ,@TelephoneNumber
           ,@HoleId
           ,@NumberOfPlayers
           ,@PlayerNames)

END TRY  
BEGIN CATCH  
   	SET @errorNumber = -1;
END CATCH  

RETURN @errorNumber;

END