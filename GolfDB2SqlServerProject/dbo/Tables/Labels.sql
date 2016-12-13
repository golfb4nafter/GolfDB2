/*
   Friday, December 9, 201611:23:54 AM
   User: 
   Server: DESKTOP-S7JQFF1\SQLEXPRESS
   Database: GolfDB20161207-01
   Application: 
*/

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Labels
	(
	Id int NOT NULL IDENTITY (1, 1),
	CourseId int NULL,
	Ordinal int NULL,
	LabelType varchar(MAX) NOT NULL,
	Label nvarchar(MAX) NULL,
	Notes nvarchar(MAX) NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE dbo.Labels SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
