/****** Object:  Table [dbo].[Auction]    Script Date: 16-Sep-18 13:43:07 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Auction](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](2048) NOT NULL,
	[Image] [varbinary](max) NULL,
	[Price] [int] NOT NULL,
	[Duration] [bigint] NOT NULL,
	[ExpiresAt] [datetime] NULL,
	[StatusId] [int] NOT NULL,
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


