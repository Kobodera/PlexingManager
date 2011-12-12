--USE [PlexManager]
--GO

/****** Object:  Table [dbo].[Fleets]    Script Date: 12/10/2011 20:37:51 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Fleets](
	[FcId] [int] NOT NULL,
	[Participants] [nvarchar](max) NULL,
	[Comments] [nvarchar](2000) NULL,
 CONSTRAINT [PK_Fleets_1] PRIMARY KEY CLUSTERED 
(
	[FcId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


/****** Object:  Table [dbo].[Plexes]    Script Date: 12/10/2011 20:38:09 ******/

CREATE TABLE [dbo].[Plexes](
	[PlexId] [int] IDENTITY(1,1) NOT NULL,
	[FCId] [int] NOT NULL,
	[PlexInfoId] [int] NOT NULL,
	[PlexingPeriodId] [int] NOT NULL,
	[Participants] [nvarchar](max) NULL,
	[PlexingDate] [datetime] NULL,
 CONSTRAINT [PK_Plexes] PRIMARY KEY CLUSTERED 
(
	[PlexId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


/****** Object:  Table [dbo].[PlexInfos]    Script Date: 12/10/2011 20:38:25 ******/

CREATE TABLE [dbo].[PlexInfos](
	[PlexId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](200) NULL,
	[Points] [int] NULL,
 CONSTRAINT [PK_PlexInfos] PRIMARY KEY CLUSTERED 
(
	[PlexId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


/****** Object:  Table [dbo].[PlexingPeriods]    Script Date: 12/10/2011 20:38:44 ******/

CREATE TABLE [dbo].[PlexingPeriods](
	[PlexingPeriodId] [int] IDENTITY(1,1) NOT NULL,
	[FromDate] [datetime] NULL,
	[ToDate] [datetime] NULL,
	[PayoutSum] [float] NULL,
	[CorpTax] [float] NULL,
	[Payed] [bit] NULL,
	[IskPerPoint] [float] NULL,
	[IskPerPointAfterTax] [float] NULL,
	[Points] [float] NULL,
 CONSTRAINT [PK_PlexingPeriods] PRIMARY KEY CLUSTERED 
(
	[PlexingPeriodId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


/****** Object:  Table [dbo].[PlexUserRoles]    Script Date: 12/10/2011 20:38:56 ******/

CREATE TABLE [dbo].[PlexUserRoles](
	[CharacterId] [int] NOT NULL,
	[Roles] [nvarchar](2000) NOT NULL,
 CONSTRAINT [PK_PlexUserRoles] PRIMARY KEY CLUSTERED 
(
	[CharacterId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


/****** Object:  Table [dbo].[PlexUsers]    Script Date: 12/10/2011 20:39:08 ******/

CREATE TABLE [dbo].[PlexUsers](
	[CharacterId] [int] NOT NULL,
	[CharacterName] [nvarchar](200) NULL,
	[Password] [nvarchar](200) NULL,
	[CorpId] [int] NULL,
	[CorpName] [nvarchar](200) NULL,
	[AllianceId] [int] NULL,
	[AllianceName] [nvarchar](200) NULL,
	[Enabled] [bit] NULL,
 CONSTRAINT [PK_PlexUsers] PRIMARY KEY CLUSTERED 
(
	[CharacterId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


/****** Object:  Table [dbo].[Rules]    Script Date: 12/10/2011 20:39:19 ******/

CREATE TABLE [dbo].[Rules](
	[RuleId] [int] IDENTITY(1,1) NOT NULL,
	[RuleName] [nvarchar](200) NULL,
	[Id] [int] NULL,
	[Allowed] [bit] NULL,
 CONSTRAINT [PK_Rules] PRIMARY KEY CLUSTERED 
(
	[RuleId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

