USE [TheAuctioneer]
GO

/****** Object:  Table [dbo].[SystemParameters]    Script Date: 18-Sep-18 22:38:23 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SystemParameters](
	[Id] [uniqueidentifier] NOT NULL,
	[ParameterName] [nvarchar](50) NOT NULL,
	[ParameterDescription] [nvarchar](512) NOT NULL,
	[ParameterValue] [nvarchar](50) NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_SystemParameters] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


