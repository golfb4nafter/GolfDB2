CREATE TABLE [dbo].[EventDetail] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [EventId]        INT            NOT NULL,
    [CourseId]       INT            NOT NULL,
    [PlayFormat]     INT            NOT NULL,
    [NumberOfHoles]  INT            NOT NULL,
    [IsShotgunStart] BIT            CONSTRAINT [DF__EventDeta__IsSho__4F7CD00D] DEFAULT ((0)) NOT NULL,
    [Sponsor]        NVARCHAR (MAX) NOT NULL,
    [PlayListId]     INT            NOT NULL,
    [OrgId]          INT            DEFAULT ((1)) NOT NULL,
    [StartHoleId]    INT            DEFAULT ((1)) NOT NULL,
    [NumGroups]      INT            DEFAULT ((18)) NOT NULL,
    [NumPerGroup]    INT            DEFAULT ((4)) NOT NULL,
    [SortOn]         NCHAR (20)     NULL,
    CONSTRAINT [PK__EventDet__3214EC074D94879B] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_EventDetail_CourseId] FOREIGN KEY ([CourseId]) REFERENCES [dbo].[CourseData] ([Id]),
    CONSTRAINT [FK_EventDetail_EventId] FOREIGN KEY ([EventId]) REFERENCES [dbo].[Event] ([id]),
    CONSTRAINT [FK_EventDetail_OrgId] FOREIGN KEY ([OrgId]) REFERENCES [dbo].[Organization] ([Id]),
    CONSTRAINT [FK_EventDetail_PlayListId] FOREIGN KEY ([PlayListId]) REFERENCES [dbo].[HoleList] ([Id]),
    CONSTRAINT [FK_EventDetail_StartHoleId] FOREIGN KEY ([StartHoleId]) REFERENCES [dbo].[Hole] ([Id])
);








