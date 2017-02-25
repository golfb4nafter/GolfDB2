CREATE TABLE [dbo].[Organization] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [OrgName]        NVARCHAR (128) NOT NULL,
    [ContactName]    NVARCHAR (128) NOT NULL,
    [ContactPhone]   NVARCHAR (50)  NOT NULL,
    [OrgDescription] NVARCHAR (256) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

