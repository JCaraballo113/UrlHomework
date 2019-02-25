using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Net.Http;
using System.Threading.Tasks;

namespace Homework
{
    class Program
    {

        public static void Main(string[] args)
        {
            var urlValidatorInstance = new URLValidator();

            List<string> urls = new List<string>
            {
                "google.com",
                "apple.com",
                "xbox.com",
                "pizza.pizza"
            };
            
             urlValidatorInstance.ValidURLS(urls);
            Console.ReadLine();
        }
    }

    class URLValidator
    {
        private static readonly HttpClient client = new HttpClient();

        public async Task<List<string>> ValidURLS(List<string> urlsToValidate)
        {
            List<string> validUrls = new List<string>();

            foreach (string url in urlsToValidate)
            {
                Uri uriResult;

                if (validURL(url, out uriResult))
                {
                    HttpResponseMessage response = await client.GetAsync(uriResult.AbsoluteUri);
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        validUrls.Add(url);
                    }
                }
            }

            foreach(string url in validUrls)
            {
                Console.WriteLine(url);
            }
            return validUrls;
        }

        private bool validURL(string s, out Uri resultUri)
        {
            if (!Regex.IsMatch(s, @"^https?:\/\/", RegexOptions.IgnoreCase))
                s = "http://" + s;

            if (Uri.TryCreate(s, UriKind.Absolute, out resultUri))
                return (resultUri.Scheme == Uri.UriSchemeHttp ||
                        resultUri.Scheme == Uri.UriSchemeHttps);

            return false;
        }
    }
}
