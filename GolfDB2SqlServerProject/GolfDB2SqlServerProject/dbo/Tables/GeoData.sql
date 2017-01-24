CREATE TABLE [dbo].[GeoData] (
    [Id]                   INT  IDENTITY (1, 1) NOT NULL,
    [GeoSpatialDataId]     INT  NOT NULL,
    [GeoObjectDescription] TEXT NOT NULL,
    [GeoObjectType]        INT  NOT NULL,
    [HoleId]               INT  NOT NULL,
    [OrderNumber]          INT  NOT NULL,
    [CourseId]             INT  NULL,
    [YardsToFront]         INT  NULL,
    [YardsToMiddle]        INT  NULL,
    [YardsToBack]          INT  NULL,
    CONSTRAINT [PK__GeoData__3214EC077F60ED59] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_GeoData_ToCourseData] FOREIGN KEY ([CourseId]) REFERENCES [dbo].[CourseData] ([Id]),
    CONSTRAINT [FK_GeoData_ToHole] FOREIGN KEY ([HoleId]) REFERENCES [dbo].[Hole] ([Id])
);



GO

