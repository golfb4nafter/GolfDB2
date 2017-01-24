CREATE TABLE [dbo].[GlobalSettings] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [SettingName] NVARCHAR (128) NOT NULL,
    [Value]       NVARCHAR (256) NOT NULL,
    [LastWritten] ROWVERSION     NOT NULL,
    [LastUserId]  INT            NOT NULL,
    CONSTRAINT [PK_GlobalSettings] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [AK_SettingName] UNIQUE NONCLUSTERED ([SettingName] ASC)
);



