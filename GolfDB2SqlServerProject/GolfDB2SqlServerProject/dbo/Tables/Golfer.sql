CREATE TABLE [dbo].[Golfer] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [FirstName]      NVARCHAR (MAX) NOT NULL,
    [LastName]       NVARCHAR (MAX) NOT NULL,
    [Phone]          NVARCHAR (MAX) NOT NULL,
    [EMail]          NVARCHAR (MAX) NOT NULL,
    [OrganizationId] INT            DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_Golfer_Id] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Golfer_ToOrganization] FOREIGN KEY ([OrganizationId]) REFERENCES [dbo].[Organization] ([Id])
);



