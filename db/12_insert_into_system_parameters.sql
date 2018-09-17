INSERT INTO [dbo].[SystemParameters]
           ([ParameterName]
           ,[ParameterDescription]
           ,[ParameterValue]
           ,[IsActive])
     VALUES
           (
				'N',
				'Podrazumevani broj najskorijih aukcija',
				'10',
				1
			),
			(
				'D',
				'Podrazumevano trajanje aukcije u sekundama',
				'180',
				1
			),
			(
				'S',
				'Broj tokena u Silver paketu',
				'30',
				1
			),
			(
				'G',
				'Broj tokena u Gold paketu',
				'70',
				1
			),
			(
				'P',
				'Broj tokena u Platinum paketu',
				'180',
				1
			),
			(
				'C',
				'Valuta portala',
				'RSD',
				1
			),
			(
				'C',
				'Valuta portala',
				'USD',
				0
			),
			(
				'C',
				'Valuta portala',
				'EUR',
				0
			),
			(
				'T',
				'Vrednost tokena u aktivnoj valuti',
				'50',
				1
			)	
				
GO


