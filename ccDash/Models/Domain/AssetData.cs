using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ccDash.Models.Domain
{
    public class AssetData
    {
        public decimal Price { get; set; }
        public DateTime Time { get; set; }
        public decimal last { get; set; }
        public Volume volume { get; set; }
    }

    public class Volume
    {
        public long timestamp { get; set; }
    }
}