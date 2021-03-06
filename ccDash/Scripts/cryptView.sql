/****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP 1000 *
  FROM [TransactBit].[dbo].[Transactions]

Delete Transactions
Where Type is Null

Select sum([Trading Fee (USD)]) as Fees
From [Transactions]

/* BTC */
	Select sum([BTC Amount]) as LostBTC
	From Transactions
	Where [Date] > '2017-09-14'
		And [Date] < '2017-09-16'
/* ETH */
  Select sum([ETH Amount]) as LostETH
	From Transactions
	Where [Date] > '2017-09-14'
		And [Date] < '2017-09-16'


/* Totals */
Select Top 1 Cast([BTC Balance] as decimal(15,10)) as BTC_Balance, Cast([ETH Balance] as decimal(15,10)) as ETH_Balance, [USD Balance] as USD_Balance
From Transactions
Order by [Date] Desc
		