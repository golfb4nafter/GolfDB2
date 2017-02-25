CREATE TABLE [dbo].[Tmp_ScoreCard] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [EventId]      INT            NOT NULL,
    [StartingHole] INT            NOT NULL,
    [Division]     INT            NOT NULL,
    [Names]        NVARCHAR (128) NOT NULL,
    [TeeTimeId]    INT            NOT NULL
);

