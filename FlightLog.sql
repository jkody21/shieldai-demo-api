USE [ShieldAI]
GO

/****** Object:  Table [dbo].[Drone]    Script Date: 3/2/2019 2:18:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Drone](
	[DroneId] [int] NOT NULL,
	[Name] [varchar](32) NOT NULL,
	[CurrentGeneration] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_Drone] PRIMARY KEY CLUSTERED 
(
	[DroneId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FlightLog]    Script Date: 3/2/2019 2:18:54 PM ******/
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
	[CreatedOn] [datetime] NOT NULL,
 CONSTRAINT [PK_FlightLog] PRIMARY KEY CLUSTERED 
(
	[FlightLogId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Index [IX_FlightLog_DroneId]    Script Date: 3/2/2019 2:18:54 PM ******/
CREATE NONCLUSTERED INDEX [IX_FlightLog_DroneId] ON [dbo].[FlightLog]
(
	[DroneId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FlightLog_Latitude]    Script Date: 3/2/2019 2:18:54 PM ******/
CREATE NONCLUSTERED INDEX [IX_FlightLog_Latitude] ON [dbo].[FlightLog]
(
	[Latitude] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FlightLog_Longitude]    Script Date: 3/2/2019 2:18:54 PM ******/
CREATE NONCLUSTERED INDEX [IX_FlightLog_Longitude] ON [dbo].[FlightLog]
(
	[Longitude] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Drone] ADD  CONSTRAINT [DF_Drone_CurrentGeneration]  DEFAULT ((0)) FOR [CurrentGeneration]
GO
ALTER TABLE [dbo].[Drone] ADD  CONSTRAINT [DF_Drone_IsActive]  DEFAULT ((0)) FOR [IsActive]
GO
ALTER TABLE [dbo].[FlightLog] ADD  CONSTRAINT [DF_FlightLog_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[FlightLog]  WITH CHECK ADD  CONSTRAINT [FK_FlightLog_Drone] FOREIGN KEY([DroneId])
REFERENCES [dbo].[Drone] ([DroneId])
GO
ALTER TABLE [dbo].[FlightLog] CHECK CONSTRAINT [FK_FlightLog_Drone]
GO
