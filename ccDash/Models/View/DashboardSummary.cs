using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ccDash.Models;

namespace ccDash.Models.View
{
    public class DashboardSummary
    {
        public List<Assets> Balances { get; set; }
        public Assets Deposits { get; set; }
    }
}