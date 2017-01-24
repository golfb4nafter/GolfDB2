CREATE TABLE [dbo].[GeoSpatialTable] (
    [Id]                  INT            IDENTITY (1, 1) NOT NULL,
    [Latitude]            NVARCHAR (20)  NULL,
    [Longitude]           NVARCHAR (20)  NULL,
    [Altitude]            NVARCHAR (10)  NULL,
    [LocationDescription] NVARCHAR (MAX) NULL,
    [GoogleMapsViewUrl]   NVARCHAR (MAX) NULL,
    [CourseId]            INT            CONSTRAINT [DF_GeoSpatialTable_CourseId] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [FK_GeoSpatialTable_ToHole] FOREIGN KEY ([CourseId]) REFERENCES [dbo].[CourseData] ([Id])
);



GO


GO
