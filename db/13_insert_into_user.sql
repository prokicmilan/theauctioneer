set identity_insert [User] on
GO
insert into [User] (
			Id,
			[Name],
			Surname,
			Gender,
			Email,
			Username,
			[Password],
			TokenCount,
			RoleId)
		values (
			1,
			'Admin',
			'Admin',
			'A',
			'admin@test.com',
			'admin',
			'irDim0DPcFg+eIvU6tzc/4afxGWv8RvUwuhFlMBvDzsTnyY1ohc8dg==',
			99999999,
			1
		)
GO
set identity_insert [User] off
GO