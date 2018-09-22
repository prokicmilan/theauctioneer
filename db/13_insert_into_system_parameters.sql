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