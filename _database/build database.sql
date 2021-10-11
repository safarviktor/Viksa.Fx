DROP PROCEDURE IF EXISTS dbo.spAddCurrencyRates
DROP TYPE IF EXISTS dbo.KeyValuePairChar3Decimal
DROP TABLE IF EXISTS dbo.CurrencyRate
DROP TABLE IF EXISTS dbo.Currency

CREATE TABLE dbo.Currency
(
	Id INT NOT NULL
		CONSTRAINT PK_Currency PRIMARY KEY CLUSTERED,
	CurrencyCode CHAR(3) NOT NULL,
	Unit INT NOT NULL
		CONSTRAINT DF_Currency_Unit DEFAULT 1
)

INSERT INTO dbo.Currency (CurrencyCode)
SELECT 'EUR'

CREATE TABLE dbo.CurrencyRate
(
	Id INT NOT NULL
		CONSTRAINT PK_CurrencyRate PRIMARY KEY CLUSTERED,
	FromCurrencyId INT NOT NULL
		CONSTRAINT FK_CurrencyRate_Currency_From FOREIGN KEY REFERENCES dbo.Currency (Id),
	ToCurrencyId INT NOT NULL
		CONSTRAINT FK_CurrencyRate_Currency_To FOREIGN KEY REFERENCES dbo.Currency (Id),
	AsAt DATE NOT NULL,
	Rate DECIMAL(18,8) NOT NULL,
	DateTimeCreated DATETIME NOT NULL
		CONSTRAINT DF_CurrencyRate_DateTimeCreated DEFAULT GETDATE()
)


CREATE TYPE dbo.KeyValuePairChar3Decimal AS TABLE 
(
	[Key] CHAR(3) NOT NULL,
	[Value] DECIMAL(18,8) NOT NULL
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

	INSERT INTO dbo.CurrencyRate
	(FromCurrencyId, ToCurrencyId, Rate, AsAt)
	SELECT @fromCurrencyId, C.Id, R.[Value], @asat
	FROM @rates R
	JOIN dbo.Currency C ON C.CurrencyCode = R.[Key]

	/* use a MERGE statement if we're expecting updates, not just inserts, or if we want to keep one value per date */

END