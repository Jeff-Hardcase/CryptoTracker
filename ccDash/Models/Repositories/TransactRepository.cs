using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ccDash.Models.Contexts;
using ccDash.Models.Domain;
using ccDash.Models.View;

namespace ccDash.Models.Repositories
{
    public class TransactRepository
    {
        private TransactContext db = new TransactContext();

        public DashboardSummary GetCurrentSummary()
        {
            var result = new DashboardSummary();
            var source = AssetPriceSource.Gemini;       //Assume Gemini for now

            var balances = GetAssetList();
            var totalDeposit = GetDepositsUSD();

            result.Balances = balances;
            result.Deposits = new Assets { AssetBalance = totalDeposit, AssetType = AssetType.USD, AssetSymbol = "USD", UnitPrice = 1 };

            //need price for each asset
            foreach(var entry in result.Balances)
            {
                entry.UnitPrice = GetPrice(entry.AssetType, source).Price;
            }

            return result;
        }

        public List<Assets> GetAssetList()
        {
            var _sql = @"  Select	A.AssetType, 
			                        A.AssetSymbol, 
			                        A.AssetName, 
			                        CASE 
				                        WHEN A.AssetSymbol='BTC' THEN Cast([BTC Balance] as decimal(15,10))
				                        WHEN A.AssetSymbol='ETH' THEN Cast([ETH Balance] as decimal(15,10))
				                        WHEN A.AssetSymbol='USD' THEN [USD Balance]
			                        END as AssetBalance, 
			                        (Select ISNULL(-Sum([usd amount] + [trading fee (usd)]), T.[USD Balance]) 
				                        from transactions where Symbol = (A.AssetSymbol + 'USD')) as AssetTotalCost
	                        From Transactions T, Assets A 
	                        Where T.[Date] is null";

            var result = db.Database.SqlQuery<Assets>(_sql);

            return result.ToList();
        }

        public decimal GetDepositsUSD()
        {
            var _sql = @"Select Sum([USD Amount]) as TotalDeposits
                            From Transactions
                            Where Type = 'Credit'";

            var result = db.Database.SqlQuery<decimal>(_sql).FirstOrDefault();

            return result;
        }

        public AssetBalance GetAssetBalances()
        {
            var result = new AssetBalance();

            var _sql = @"Select Top 1   Cast([BTC Balance] as decimal(15,10)) as BTC_Balance, 
                                        Cast([ETH Balance] as decimal(15,10)) as ETH_Balance, 
                                        [USD Balance] as USD_Balance
                            From Transactions
                            Order by [Date] Desc";

            result = db.Database.SqlQuery<AssetBalance>(_sql).FirstOrDefault();

            return result;
        }

        public AssetView GetBTCPriceGDAX()
        {
            var result = GetPrice(AssetType.Bitcoin, AssetPriceSource.GDAX);

            return result;
        }

        public AssetView GetPrice(AssetType assetType, AssetPriceSource source)
        {
            var _returnData = new AssetView();

            if (assetType == AssetType.USD)
                _returnData.Price = 1;
            else
            {
                var _wsRepo = new WebServiceRepository();
                var _headerData = new System.Collections.Specialized.NameValueCollection();

                _headerData.Set("User-Agent", ".NET service");

                var _wsURL = GetAssetPriceURL(assetType, source);
                var _response = _wsRepo.wsCall(_wsURL, HttpVerb.Get, null, _headerData);
                var _result = ServiceStack.Text.JsonSerializer.DeserializeFromString<AssetData>(_response);

                _returnData = MapDomainToAssetView(_result, assetType, source);
            }

            return _returnData;
        }

        private string GetAssetPriceURL(AssetType assetType, AssetPriceSource source)
        {
            //default to empty string
            var result = string.Empty;

            //for now, just brute force it, since there are only 4 permutations
            //later could load a dictionary with compound key, then cache the dict
            var _settings = Properties.Settings.Default;
            var _index = assetType.ToString() + source.ToString();

            switch(_index)
            {
                case "BitcoinGDAX":
                    result = _settings.url_BtcPrice_GDAX;
                    break;
                case "BitcoinGemini":
                    result = _settings.url_BtcPrice_Gemini;
                    break;
                case "EthereumGDAX":
                    result = _settings.url_EthPrice_GDAX;
                    break;
                case "EthereumGemini":
                    result = _settings.url_EthPrice_Gemini;
                    break;
            }
            
            return result;
        }

        private AssetView MapDomainToAssetView(AssetData asset, AssetType type, AssetPriceSource source)
        {
            var result = new AssetView();

            result.AssetSource = source;
            result.AssetType = type;

            //may need a better approach if this expands to more sources
            if (source == AssetPriceSource.GDAX)
            {
                result.Price = asset.Price;
                result.QuoteDateLocal = asset.Time;
                result.QuoteDateUTC = result.QuoteDateLocal.ToUniversalTime();
            }

            if (source == AssetPriceSource.Gemini)
            {
                var priceDate = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

                result.Price = asset.last;
                result.QuoteDateUTC = priceDate.AddMilliseconds(asset.volume.timestamp);
                result.QuoteDateLocal = result.QuoteDateUTC.ToLocalTime();
            }

            return result;
        }
    }
}