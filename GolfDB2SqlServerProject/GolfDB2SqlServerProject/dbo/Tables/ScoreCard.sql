CREATE TABLE [dbo].[ScoreCard] (
    [Id]              INT            IDENTITY (1, 1) NOT NULL,
    [EventId]         INT            NOT NULL,
    [StartingHole]    INT            NOT NULL,
    [Division]        NCHAR (30)     NOT NULL,
    [Names]           NVARCHAR (128) NOT NULL,
    [TeeTimeDetailId] INT            NOT NULL,
    [Handicap]        INT            DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_ScoreCard] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ScoreCard_Event] FOREIGN KEY ([EventId]) REFERENCES [dbo].[Event] ([id]),
    CONSTRAINT [FK_ScoreCard_TeeTimeDetail] FOREIGN KEY ([TeeTimeDetailId]) REFERENCES [dbo].[TeeTimeDetail] ([Id])
);









