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