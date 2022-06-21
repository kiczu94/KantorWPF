using System;
using System.Collections.Generic;
using System.Text;

namespace KalkulatorWalut
{
    class exactCurrencyRateTable
    {
        public string table { get; set; }
        public string  currency { get; set; }
        public string code { get; set; }
        public IList<RateTableC> rates { get; set; }

    }
}
