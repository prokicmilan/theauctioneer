USE [TheAuctioneer]
GO

/****** Object:  Table [dbo].[TokenOrder]    Script Date: 19-Sep-18 21:29:02 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TokenOrder](
	[Id] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[Amount] [int] NOT NULL,
	[Price] [decimal](18, 2) NOT NULL,
	[StatusId] [uniqueidentifier] NOT NULL,
	[TimestampCreated] [datetime2](7) NOT NULL,
	[TimestampChanged] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_TokenOrder] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[TokenOrder]  WITH CHECK ADD  CONSTRAINT [FK_TokenOrder_TokenOrderStatus] FOREIGN KEY([StatusId])
REFERENCES [dbo].[TokenOrderStatus] ([Id])
GO

ALTER TABLE [dbo].[TokenOrder] CHECK CONSTRAINT [FK_TokenOrder_TokenOrderStatus]
GO

ALTER TABLE [dbo].[TokenOrder]  WITH CHECK ADD  CONSTRAINT [FK_TokenOrder_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
GO

ALTER TABLE [dbo].[TokenOrder] CHECK CONSTRAINT [FK_TokenOrder_User]
GO


