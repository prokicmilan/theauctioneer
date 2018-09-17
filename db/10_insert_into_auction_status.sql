INSERT INTO [dbo].[AuctionStatus]
           ([Type]
           ,[Description])
     VALUES
		(
			'READY',
			'Auction is ready to be started.'
		),
		(
			'OPENED',
			'Auction is open and accepts bids.'
		),
		(
			'FINISHED',
			'Auction is finished.'
		)
GO


