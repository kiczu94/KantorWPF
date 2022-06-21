using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace KalkulatorWalut
{
    static class KantorWalutModel
    {
        static readonly HttpClient client = new HttpClient();
        static decimal ask;
        static decimal bid;

        public static async Task<List<string>> GetCurrenciesCodes()
        {
            List<string> currencyCodesTableA = await GetDataCodes($"http://api.nbp.pl/api/exchangerates/tables/A/");
            List<string> currencyCodesTableB = await GetDataCodes($"http://api.nbp.pl/api/exchangerates/tables/B/");
            List<string> currencyCodes = new List<string>();

            foreach (var code in currencyCodesTableA)
            {
                currencyCodes.Add(code);
            }
            foreach (var code in currencyCodesTableB)
            {
                currencyCodes.Add(code);
            }
            return currencyCodes;
        }
        private static async Task<List<string>> GetDataCodes(string uri)
        {
            HttpResponseMessage response = await client.GetAsync($"{uri}");
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            List<currencyTable> tables = JsonConvert.DeserializeObject<List<currencyTable>>(responseBody);
            List<string> currencyCodes = new List<string>();
            foreach (var table in tables)
            {
                foreach (var rate in table.rates)
                {
                    currencyCodes.Add(rate.code);
                }
            }
            return currencyCodes;
        }
        private static async Task<List<Rate>> GetRates(string uri)
        {
            HttpResponseMessage response = await client.GetAsync($"{uri}");
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            List<currencyTable> tables = JsonConvert.DeserializeObject<List<currencyTable>>(responseBody);
            List<Rate> rates = new List<Rate>();
            foreach (var table in tables)
            {
                foreach (var rate in table.rates)
                {
                    rates.Add(rate);
                }
            }
            return rates;
        }
        public static async Task<List<Rate>> GetRatesAsync()
        {
            List<Rate> rates1 = await GetRates("http://api.nbp.pl/api/exchangerates/tables/A/");
            List<Rate> rates2 = await GetRates("http://api.nbp.pl/api/exchangerates/tables/B/");
            List<Rate> rates = new List<Rate>();
            foreach (var rate  in rates1)
            {
                rates.Add(rate);
            }
            foreach (var rate in rates2)
            {
                rates.Add(rate);
            }
            
            return rates;
        }
        public static async Task<decimal> GetCurrencyAskRateAsync(string currencyCode)
        {
            exactCurrencyRateTable currencyTableC = new exactCurrencyRateTable();
            currencyTable currencyTable = new currencyTable();
            try
            {
                HttpResponseMessage response = await client.GetAsync($"http://api.nbp.pl/api/exchangerates/rates/C/{currencyCode}/");
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync();
                currencyTableC  = JsonConvert.DeserializeObject<exactCurrencyRateTable>(responseBody);
                return decimal.Parse(currencyTableC.rates[0].ask.Replace('.', ','));
            }
            catch (Exception)
            {
                HttpResponseMessage response = await client.GetAsync($"http://api.nbp.pl/api/exchangerates/rates/A/{currencyCode}/");
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync();
                currencyTable = JsonConvert.DeserializeObject<currencyTable>(responseBody);
                return decimal.Parse(currencyTable.rates[0].mid.Replace('.', ','));
            }


        }
        public static async Task<decimal> GetCurrencyBidRateAsync(string currencyCode)
        {
            exactCurrencyRateTable currencyTableC = new exactCurrencyRateTable();
            currencyTable currencyTable = new currencyTable();
            try
            {
                HttpResponseMessage response = await client.GetAsync($"http://api.nbp.pl/api/exchangerates/rates/C/{currencyCode}/");
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync();
                currencyTableC = JsonConvert.DeserializeObject<exactCurrencyRateTable>(responseBody);
                return decimal.Parse(currencyTableC.rates[0].bid.Replace('.', ','));
            }
            catch (Exception)
            {
                HttpResponseMessage response = await client.GetAsync($"http://api.nbp.pl/api/exchangerates/rates/A/{currencyCode}/");
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync();
                currencyTable = JsonConvert.DeserializeObject<currencyTable>(responseBody);
                return decimal.Parse(currencyTable.rates[0].mid.Replace('.', ','));
            }
        }
        public static async Task<string> GetValue(string currencyCode,decimal input, string name)
        {
            await GetCurrencyRate(currencyCode);
            if (name=="IGetForeignCurrency"&&ask!=0)
            {
                return Math.Round((input / ask), 2).ToString();
            }
            else
            {
                return Math.Round((input * bid), 2).ToString();
            }
            
        }
        public static async Task<bool> GetCurrencyRate(string currencyCode)
        {
            ask = await KantorWalutModel.GetCurrencyAskRateAsync(currencyCode);
            bid = await KantorWalutModel.GetCurrencyBidRateAsync(currencyCode);
            return true;
        }
    }
}
