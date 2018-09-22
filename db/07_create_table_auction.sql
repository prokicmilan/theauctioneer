USE [TheAuctioneer]
GO

/****** Object:  Table [dbo].[Auction]    Script Date: 22-Sep-18 16:45:23 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Auction](
	[Id] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](2048) NOT NULL,
	[Image] [varbinary](max) NULL,
	[Price] [int] NOT NULL,
	[Duration] [bigint] NOT NULL,
	[ExpiresAt] [datetime2](7) NULL,
	[StatusId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Auction] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Auction]  WITH CHECK ADD  CONSTRAINT [FK_Auction_AuctionStatus] FOREIGN KEY([StatusId])
REFERENCES [dbo].[AuctionStatus] ([Id])
GO

ALTER TABLE [dbo].[Auction] CHECK CONSTRAINT [FK_Auction_AuctionStatus]
GO

ALTER TABLE [dbo].[Auction]  WITH CHECK ADD  CONSTRAINT [FK_Auction_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
GO

ALTER TABLE [dbo].[Auction] CHECK CONSTRAINT [FK_Auction_User]
GO


