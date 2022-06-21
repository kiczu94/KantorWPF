using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

namespace KalkulatorWalut
{
    class KantorWalutModelView : INotifyPropertyChanged
    {
        public bool currencySelectedIGetForeignCurrency { get; set; }
        public bool currencySelectedIHaveForeignCurrency { get; set; }

        private Rate _PLN;
        public Rate PLN
        {
            get { return _PLN; }
            set { _PLN = value; }
        }

        private string _input;

        public string input
        {
            get { return _input; }
            set { _input = value;
                OnPropertyChanged("input");
            }
        }
        public bool inputCorrect { get; set; }
        public decimal _inputDecimal;
        private List<string> _currencyCodes;
        private List<string> _currencyNames;
        private List<Rate> _rates;
        public List<Rate> rates
        {
            get { return _rates; }
            set { _rates = value; }
        }

        private string _output;

        public string output
        {
            get { return _output; }
            set { _output = value;
                OnPropertyChanged("output");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public List<string> currencyNames
        {
            get { return _currencyNames; }
            set { _currencyNames = value; }
        }
        public List<string> currencyCodes
        {
            get { return _currencyCodes; }
            set { _currencyCodes = value; }
        }

        public KantorWalutModelView()
        {
            currencySelectedIGetForeignCurrency = true;
            currencySelectedIHaveForeignCurrency = false;
            GetRates();
        }
        public async void GetCodes()
        {
            currencyCodes = await KantorWalutModel.GetCurrenciesCodes();
            OnPropertyChanged("currencyCodes");
        }
        public async void GetRates()
        {
                rates = await KantorWalutModel.GetRatesAsync();
                PLN = new Rate() { code = "PLN", currency = "Polski złoty", mid = "1" };
                OnPropertyChanged("rates");
                OnPropertyChanged("PLN");
            

        }
        private void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
        public void CheckInputValue(string value)
        {
            string stringToParse;
            if (value.Contains('.'))
            {
                stringToParse = value.Replace('.', ',');
                inputCorrect= decimal.TryParse(stringToParse, out _inputDecimal);
            }
            else
            {
                inputCorrect= decimal.TryParse(value, out _inputDecimal);
            }
        }
        public async void CalculateOutput(object selectedItem,string name)
        {
            Rate currency = (Rate)selectedItem;
            output = await KantorWalutModel.GetValue(currency.code,_inputDecimal,name);
            OnPropertyChanged("output");
        }
    }
}
