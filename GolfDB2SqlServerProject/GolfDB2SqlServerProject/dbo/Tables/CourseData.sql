CREATE TABLE [dbo].[CourseData](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CourseName]    [nvarchar](128) NOT NULL,
    [Address1]      [nvarchar](128) NULL,
    [Address2]      [nvarchar](128) NULL,
    [City]          [nvarchar](128) NULL,
    [State]         [nvarchar](128) NULL,
    [PostalCode]    [nvarchar](128) NULL,
    [Email]         [nvarchar](128) NULL,
	[Phone]         [nvarchar](20) NULL,
	[Url]           [nvarchar](MAX) NULL,
	[GoogleMapUrl]  [nvarchar](MAX) NULL,
	[NumberOfHoles] [int] NULL,
	[NumberOfNines] [int] NULL
	CONSTRAINT [PK_dbo.CourseData] PRIMARY KEY CLUSTERED ([Id] ASC)
);

GO

CREATE UNIQUE NONCLUSTERED INDEX [CourseNameIndex]
    ON [dbo].[CourseData]([CourseName] ASC);

GO

