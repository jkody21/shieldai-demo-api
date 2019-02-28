USE [ShieldAI]
GO

/****** Object:  Table [dbo].[FlightLog]    Script Date: 2/26/2019 8:55:56 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FlightLog](
	[FlightLogId] [bigint] IDENTITY(1,1) NOT NULL,
	[DroneId] [int] NOT NULL,
	[DroneGeneration] [int] NOT NULL,
	[BeginOn] [datetime] NOT NULL,
	[EndOn] [datetime] NULL,
	[Latitude] [float] NOT NULL,
	[Longitude] [float] NOT NULL,
	[MapPath] [varchar](512) NULL,
 CONSTRAINT [PK_FlightLog] PRIMARY KEY CLUSTERED 
(
	[FlightLogId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Index [IX_FlightLog_DroneId]    Script Date: 2/26/2019 8:56:25 PM ******/
CREATE NONCLUSTERED INDEX [IX_FlightLog_DroneId] ON [dbo].[FlightLog]
(
	[DroneId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO


/****** Object:  Index [IX_FlightLog_Latitude]    Script Date: 2/26/2019 8:56:46 PM ******/
CREATE NONCLUSTERED INDEX [IX_FlightLog_Latitude] ON [dbo].[FlightLog]
(
	[Latitude] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

/****** Object:  Index [IX_FlightLog_Longitude]    Script Date: 2/26/2019 8:57:14 PM ******/
CREATE NONCLUSTERED INDEX [IX_FlightLog_Longitude] ON [dbo].[FlightLog]
(
	[Longitude] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

