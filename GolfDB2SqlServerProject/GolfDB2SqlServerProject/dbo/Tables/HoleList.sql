CREATE TABLE [dbo].[HoleList] (
    [Id]       INT            IDENTITY (1, 1) NOT NULL,
    [CourseId] INT            NOT NULL,
    [Label]    NVARCHAR (128) NOT NULL,
    [HoleList] NVARCHAR (256) NOT NULL,
    [BList]    NVARCHAR (256) NULL,
    CONSTRAINT [PK_HoleList_Id] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_HoleList_CourseId] FOREIGN KEY ([CourseId]) REFERENCES [dbo].[CourseData] ([Id])
);



