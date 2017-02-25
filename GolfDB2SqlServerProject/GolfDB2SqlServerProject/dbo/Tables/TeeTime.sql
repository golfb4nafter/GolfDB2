CREATE TABLE [dbo].[TeeTime] (
    [Id]              INT            IDENTITY (1, 1) NOT NULL,
    [TeeTimeOffset]   INT            NOT NULL,
    [Tee_Time]        DATETIME       NOT NULL,
    [CourseId]        INT            CONSTRAINT [DF__TeeTime__CourseI__7A672E12] DEFAULT ((1)) NOT NULL,
    [EventId]         INT            NOT NULL,
    [ReservedByName]  NVARCHAR (128) NULL,
    [TelephoneNumber] NCHAR (80)     NULL,
    [HoleId]          INT            NOT NULL,
    [NumberOfPlayers] INT            NOT NULL,
    [PlayerNames]     NVARCHAR (256) NULL,
    CONSTRAINT [PK__TeeTime__3214EC076754599E] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_TeeTime_CourseId] FOREIGN KEY ([CourseId]) REFERENCES [dbo].[CourseData] ([Id]),
    CONSTRAINT [FK_TeeTime_HoleId] FOREIGN KEY ([HoleId]) REFERENCES [dbo].[Hole] ([Id]),
    CONSTRAINT [FK_TeeTime_TeeTime] FOREIGN KEY ([Id]) REFERENCES [dbo].[TeeTime] ([Id])
);

