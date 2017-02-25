CREATE TABLE [dbo].[ScoreEntry] (
    [Id]          INT IDENTITY (1, 1) NOT NULL,
    [HoleId]      INT NOT NULL,
    [ScoreCardId] INT NOT NULL,
    [Score]       INT NOT NULL,
    [Ordinal]     INT NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ScoreEntry_Hole] FOREIGN KEY ([HoleId]) REFERENCES [dbo].[Hole] ([Id]),
    CONSTRAINT [FK_ScoreEntry_ScoreCard] FOREIGN KEY ([ScoreCardId]) REFERENCES [dbo].[ScoreCard] ([Id])
);

