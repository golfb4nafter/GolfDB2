CREATE TABLE [dbo].[CourseRatings] (
    [Id]                   INT            IDENTITY (1, 1) NOT NULL,
    [CourseId]             INT            NOT NULL,
    [TeeName]              NVARCHAR (50)  NOT NULL,
    [Course_Rating]        DECIMAL (5, 2) NOT NULL,
    [SlopeRating18]        INT            NOT NULL,
    [Front9]               NVARCHAR (50)  NOT NULL,
    [Back9]                NVARCHAR (50)  NOT NULL,
    [BogeyRating]          DECIMAL (5, 2) NOT NULL,
    [Gender]               NCHAR (1)      NULL,
    [HolesListDescription] NVARCHAR (MAX) NULL,
    [HandicapByHole]       NVARCHAR (MAX) NULL
);


GO
CREATE NONCLUSTERED INDEX [IX_CourseRatings]
    ON [dbo].[CourseRatings]([Id] ASC);

