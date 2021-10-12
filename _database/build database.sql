DROP PROCEDURE IF EXISTS dbo.spAddCurrencyRates
DROP TYPE IF EXISTS dbo.KeyValuePairChar3Decimal
DROP TABLE IF EXISTS dbo.CurrencyRate
DROP TABLE IF EXISTS dbo.Currency

CREATE TABLE dbo.Currency
(
	Id INT NOT NULL IDENTITY(1,1)
		CONSTRAINT PK_Currency PRIMARY KEY CLUSTERED,
	CurrencyCode CHAR(3) NOT NULL	
)

INSERT INTO dbo.Currency (CurrencyCode)
SELECT 'EUR'

CREATE TABLE dbo.CurrencyRate
(
	Id INT NOT NULL IDENTITY(1,1)
		CONSTRAINT PK_CurrencyRate PRIMARY KEY CLUSTERED,
	FromCurrencyId INT NOT NULL
		CONSTRAINT FK_CurrencyRate_Currency_From FOREIGN KEY REFERENCES dbo.Currency (Id),
	ToCurrencyId INT NOT NULL
		CONSTRAINT FK_CurrencyRate_Currency_To FOREIGN KEY REFERENCES dbo.Currency (Id),
	AsAt DATE NOT NULL,
	Rate DECIMAL(38,19) NOT NULL,
	DateTimeCreated DATETIME NOT NULL
		CONSTRAINT DF_CurrencyRate_DateTimeCreated DEFAULT GETDATE(),
	DateTimeUpdated DATETIME NOT NULL
		CONSTRAINT DF_CurrencyRate_DateTimeUpdated DEFAULT GETDATE(),

	CONSTRAINT UQ_RateByDate UNIQUE (FromCurrencyId, ToCurrencyId, AsAt)
)


CREATE TYPE dbo.KeyValuePairChar3Decimal AS TABLE 
(
	[Key] CHAR(3) NOT NULL,
	[Value] DECIMAL(38,19) NOT NULL
)
GO

CREATE PROCEDURE dbo.spAddCurrencyRates
	@rates KeyValuePairChar3Decimal READONLY,
	@asat DATE,
	@baseCurrency CHAR(3)
AS
BEGIN

	DECLARE @unknownCurrencies TABLE 
	(
		Code CHAR(3)
	)

	INSERT INTO @unknownCurrencies (Code)
	SELECT R.[Key]
	FROM @rates R
	WHERE NOT EXISTS (select 1 from dbo.Currency EXISTING WHERE EXISTING.CurrencyCode = R.[Key])

	INSERT INTO dbo.Currency (CurrencyCode)
	SELECT Code
	FROM @unknownCurrencies

	DECLARE @fromCurrencyId INT = (SELECT Id FROM dbo.Currency WHERE CurrencyCode = @baseCurrency)

	MERGE dbo.CurrencyRate AS TGT
	USING (
		SELECT 			
			@fromCurrencyId AS FromCurrencyId,
			C.Id AS ToCurrencyId,
			R.[Value] AS NewRate,			
			@asat AS AsAt			
		FROM @rates R
		JOIN dbo.Currency C ON C.CurrencyCode = R.[Key]
	) AS SRC
	ON SRC.ToCurrencyId = TGT.ToCurrencyId
		AND SRC.AsAt = TGT.AsAt
		AND SRC.FromCurrencyId = TGT.FromCurrencyId
	WHEN MATCHED THEN
		UPDATE 
		SET Rate = SRC.NewRate,
			DateTimeUpdated = GETDATE()
	WHEN NOT MATCHED THEN
		INSERT (FromCurrencyId, ToCurrencyId, Rate, AsAt)
		VALUES (SRC.FromCurrencyId, SRC.ToCurrencyId, SRC.NewRate, SRC.AsAt);
	
END