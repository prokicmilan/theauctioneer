insert into Role(
	Id,
	[Type],
	[Description]
)
	values
		(newid(), 'Admin', 'System administrator'),
		(newid(), 'User', 'Regular user')