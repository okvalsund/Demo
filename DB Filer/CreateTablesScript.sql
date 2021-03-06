USE [Demo]
GO
/****** Object:  Table [dbo].[addresses]    Script Date: 2017-05-14 22.54.26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[addresses](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[personId] [int] NULL,
	[street] [nvarchar](150) NULL,
	[zipCode] [int] NULL,
	[city] [nvarchar](50) NULL,
 CONSTRAINT [PK_addresses] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[changeLog]    Script Date: 2017-05-14 22.54.26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[changeLog](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[verb] [nvarchar](20) NULL,
	[uri] [nvarchar](1000) NULL,
	[changes] [nvarchar](2000) NULL,
 CONSTRAINT [PK_changeLog] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[emails]    Script Date: 2017-05-14 22.54.26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[emails](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[personId] [int] NULL,
	[emailAddress] [nvarchar](100) NULL,
 CONSTRAINT [PK_emails] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[persons]    Script Date: 2017-05-14 22.54.26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[persons](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[firstName] [nvarchar](100) NULL,
	[middleName] [nvarchar](100) NULL,
	[lastName] [nvarchar](100) NULL,
 CONSTRAINT [PK_person] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[users]    Script Date: 2017-05-14 22.54.26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[users](
	[id] [int] NOT NULL,
 CONSTRAINT [PK_users] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[addresses]  WITH CHECK ADD  CONSTRAINT [FK_addresses_persons] FOREIGN KEY([personId])
REFERENCES [dbo].[persons] ([id])
GO
ALTER TABLE [dbo].[addresses] CHECK CONSTRAINT [FK_addresses_persons]
GO
ALTER TABLE [dbo].[emails]  WITH CHECK ADD  CONSTRAINT [FK_emails_persons] FOREIGN KEY([personId])
REFERENCES [dbo].[persons] ([id])
GO
ALTER TABLE [dbo].[emails] CHECK CONSTRAINT [FK_emails_persons]
GO
ALTER TABLE [dbo].[users]  WITH CHECK ADD  CONSTRAINT [FK_users_persons] FOREIGN KEY([id])
REFERENCES [dbo].[persons] ([id])
GO
ALTER TABLE [dbo].[users] CHECK CONSTRAINT [FK_users_persons]
GO
