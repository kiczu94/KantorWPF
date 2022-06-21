using System;
using System.Collections.Generic;
using System.Text;

namespace KalkulatorWalut
{
    class currencyTable
    {
        public string table { get; set; }
        public string no { get; set; }
        public string effectiveDate { get; set; }
        public IList<Rate> rates { get; set; }
    }
}
