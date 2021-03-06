CREATE TABLE [dbo].[GeoObjectType](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[GeoObjectType] [varchar](40) NOT NULL,
	[GeoObjectDescription] [varchar](MAX) NOT NULL,
 CONSTRAINT [PK__GeoObjec__3214EC270CBAE877] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
