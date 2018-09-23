insert into Role(
	Id,
	[Type],
	[Description]
)
	values
		(newid(), 'Admin', 'System administrator'),
		(newid(), 'User', 'Regular user')
go
insert into AuctionStatus (
	Id,
	[Type],
	[Description]
)
	values
		(newid(), 'READY', 'Auction is created and waiting to be opened by the administrator.'),
		(newid(), 'OPENED', 'The auction is open and accepting bids.'),
		(newid(), 'FINISHED', 'The auction is finished.')
go
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
go
insert into SystemParameters
	(
		[Id],
		[ParameterName],
		[ParameterDescription],
		[ParameterValue],
		[IsActive]
	)
	values 
		(
			newid(),
			'C',
			'Valuta portala',
			'RSD',
			1
		),
		(
			newid(),
			'C',
			'Valuta portala',
			'EUR',
			0
		),
		(
			newid(),
			'C',
			'Valuta portala',
			'USD',
			0
		),
		(
			newid(),
			'D',
			'Podrazumevano trajanje aukcije u sekundama',
			'180',
			1
		),
		(
			newid(),
			'N',
			'Podrazumevani broj najskorijih aukcija',
			'10',
			1
		),
		(
			newid(),
			'P',
			'Broj tokena u Platinum paketu',
			'180',
			1
		),
		(
			newid(),
			'G',
			'Broj tokena u Gold paketu',
			'70',
			1
		),
		(
			newid(),
			'S',
			'Broj tokena u Silver paketu',
			'30',
			1
		),
		(
			newid(),
			'T',
			'Vrednost jednog tokena u aktivnoj valuti',
			'50',
			1
		)
  insert into [User] (
	[Id],
	[Name],
	[Surname],
	[Gender],
	[Email],
	[Username],
	[Password],
	[TokenCount],
	[RoleId]
)
	values 
		(
			newid(),
			'Admin',
			'Admin',
			'A',
			'admin@test.com',
			'admin',
			'irDim0DPcFg+eIvU6tzc/4afxGWv8RvUwuhFlMBvDzsTnyY1ohc8dg==',
			99999999,
			(select
				[Id]
			from
		 		[Role]
			where
				[Type] = 'Admin')
		 )        