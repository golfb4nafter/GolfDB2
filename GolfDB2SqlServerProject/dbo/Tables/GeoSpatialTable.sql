CREATE TABLE [dbo].[GeoSpatialTable](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Latitude] [nvarchar](20) NULL,
	[Longitude] [nvarchar](20) NULL,
	[Altitude] [nvarchar](10) NULL,
	[LocationDescription] [nvarchar](MAX) NULL,
	[GoogleMapsViewUrl] [nvarchar](MAX) NULL,
	[CourseId] [int] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[GeoSpatialTable] ADD  CONSTRAINT [DF_GeoSpatialTable_CourseId]  DEFAULT ((0)) FOR [CourseId]
GO
