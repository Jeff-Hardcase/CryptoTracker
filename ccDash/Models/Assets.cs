using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ccDash.Models
{
    public enum AssetType
    {
        None,
        Bitcoin,
        Ethereum,
        USD
    }

    public enum AssetPriceSource
    {
        GDAX,
        Gemini
    }

    public class Assets
    {
        public AssetType AssetType { get; set; }
        public string AssetSymbol { get; set; }
        public decimal AssetBalance { get; set; }
        public decimal AssetTotalCost { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal AssetTotal { get { return AssetBalance * UnitPrice; } }
        public decimal AssetReturn { get { return (AssetTotal - AssetTotalCost) / AssetTotalCost; } }
    }
}