insert into TokenOrderStatus (
	[Id],
	[Type],
	[Description]
)
	values
		(
			newid(),
			'SUBMITTED',
			'The order has been submitted'
		),
		(
			newid(),
			'CANCELED',
			'The order has been canceled'
		),
		(
			newid(),
			'COMPLETED',
			'The order has been succesfully completed.'
		)
