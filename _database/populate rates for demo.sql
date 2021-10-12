select * from dbo.Currency
select * from dbo.CurrencyRate 

-- ASUMPTION: we have ONLY 1 set of rates for @dateForWhichWeHaveRates
-- following on that assumption, we delete all rates apart those for @dateForWhichWeHaveRates before we run the inserts


declare @dateForWhichWeHaveRates DATE = GETDATE()
delete dbo.CurrencyRate where asat <> @dateForWhichWeHaveRates

insert into dbo.CurrencyRate
(FromCurrencyId, ToCurrencyId, Rate, AsAt)
select FromCurrencyId, ToCurrencyId, Rate*0.95, dateadd(day, -1, AsAt)
from dbo.CurrencyRate
where asat = @dateForWhichWeHaveRates

insert into dbo.CurrencyRate
(FromCurrencyId, ToCurrencyId, Rate, AsAt)
select FromCurrencyId, ToCurrencyId, Rate*0.99, dateadd(day, -2, AsAt)
from dbo.CurrencyRate
where asat = @dateForWhichWeHaveRates

insert into dbo.CurrencyRate
(FromCurrencyId, ToCurrencyId, Rate, AsAt)
select FromCurrencyId, ToCurrencyId, Rate*1.03, dateadd(day, -3, AsAt)
from dbo.CurrencyRate
where asat = @dateForWhichWeHaveRates

