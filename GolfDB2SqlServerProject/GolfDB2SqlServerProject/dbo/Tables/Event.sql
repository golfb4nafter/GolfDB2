CREATE TABLE [dbo].[Event] (
    [id]       INT           IDENTITY (1, 1) NOT NULL,
    [CourseId] INT           NOT NULL,
    [text]     NVARCHAR (50) NOT NULL,
    [start]    DATETIME      NOT NULL,
    [end]      DATETIME      NOT NULL,
    [locked]   BIT           DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Event] PRIMARY KEY CLUSTERED ([id] ASC)
);



