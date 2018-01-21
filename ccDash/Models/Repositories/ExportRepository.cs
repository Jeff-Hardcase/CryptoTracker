using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ccDash.Models.Contexts;
using ccDash.Models.Domain;

namespace ccDash.Models.Repositories
{
    public class ExportRepository
    {
        private TransactContext db = new TransactContext();

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
    }
}