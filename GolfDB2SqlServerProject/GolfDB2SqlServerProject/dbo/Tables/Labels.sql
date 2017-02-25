CREATE TABLE [dbo].[Labels] (
    [Id]        INT            IDENTITY (1, 1) NOT NULL,
    [OwnerId]   INT            NOT NULL,
    [Ordinal]   INT            NOT NULL,
    [LabelType] VARCHAR (MAX)  NOT NULL,
    [Label]     NVARCHAR (MAX) NULL,
    [Notes]     NVARCHAR (MAX) NULL
);

