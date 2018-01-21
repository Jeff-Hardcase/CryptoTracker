using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ccDash.Models.View
{
    public class AssetView
    {
        public AssetType AssetType { get; set; }
        public AssetPriceSource AssetSource { get; set; }
        public decimal Price { get; set; }
        public DateTime QuoteDateLocal { get; set; }
        public DateTime QuoteDateUTC { get; set; }
    }
}