using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ccDash.Models.Domain
{
    public class AssetBalance
    {
        public decimal BTC_Balance { get; set; }
        public decimal ETH_Balance { get; set; }
        public decimal USD_Balance { get; set; }
    }
}