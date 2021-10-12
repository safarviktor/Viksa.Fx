
-- ASUMPTION: we have ONLY 1 set of rates for @dateForWhichWeHaveRates
-- following on that assumption, we delete all rates apart those for @dateForWhichWeHaveRates before we run the inserts


declare @dateForWhichWeHaveRates DATE = GETDATE()
delete dbo.CurrencyRate where asat <> @dateForWhichWeHaveRates

DECLARE @daysBack int = -1
DECLARE @randomMultiplier DECIMAL(18,8)
WHILE @daysBack > -100
BEGIN
	SET @randomMultiplier = RAND()*(1.2-0.9)+0.9;

	insert into dbo.CurrencyRate
	(FromCurrencyId, ToCurrencyId, Rate, AsAt)
	select FromCurrencyId, ToCurrencyId, Rate*@randomMultiplier, dateadd(day, @daysBack, AsAt)
	from dbo.CurrencyRate
	where asat = @dateForWhichWeHaveRates

	SET @daysBack = @daysBack - 1
END

select * from dbo.CurrencyRate where tocurrencyid = (select id from dbo.Currency where currencyCode = 'nok')