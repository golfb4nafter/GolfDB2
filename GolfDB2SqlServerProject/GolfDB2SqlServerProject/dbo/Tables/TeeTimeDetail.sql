CREATE TABLE [dbo].[TeeTimeDetail] (
    [Id]         INT           IDENTITY (1, 1) NOT NULL,
    [Name]       NVARCHAR (50) NOT NULL,
    [Cart]       BIT           DEFAULT ((0)) NOT NULL,
    [Pass]       BIT           DEFAULT ((0)) NOT NULL,
    [AmountPaid] MONEY         DEFAULT ((0.0)) NOT NULL,
    [TeeTimeId]  INT           NOT NULL,
    [Division]   NCHAR (30)    NOT NULL,
    [Handicap]   INT           DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_TeeTimeDetail_TeeTime] FOREIGN KEY ([TeeTimeId]) REFERENCES [dbo].[TeeTime] ([Id])
);



