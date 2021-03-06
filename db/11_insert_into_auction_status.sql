insert into AuctionStatus (
	Id,
	[Type],
	[Description]
)
	values
		(newid(), 'READY', 'Auction is created and waiting to be opened by the administrator.'),
		(newid(), 'OPENED', 'The auction is open and accepting bids.'),
		(newid(), 'COMPLETED', 'The auction is finished.')