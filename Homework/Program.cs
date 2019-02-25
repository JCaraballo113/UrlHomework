using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;

namespace Homework
{
    class Program
    {

        public static void Main(string[] args)
        {
            var urlValidatorInstance = new URLValidator();

            List<string> urls =  new List<string>{
                "google.com",
                "apple.com",
                "xbox.com",
                "pizza.pizza",
                "javascript:alert('Hack me!')"
            };
            
            urlValidatorInstance.GetValidUrls(urls);
            Console.ReadLine();
        }
    }

    class URLValidator
    {
        private static readonly HttpClient client = new HttpClient();

        public async Task<List<string>> GetValidUrls(List<string> urlsToValidate)
        {
            List<string> validUrls = new List<string>();

            // Split up the list of strings

            foreach (string url in urlsToValidate)
            {
                Uri uriResult;

                // If it's actually a valid URL
                if (validURL(url, out uriResult))
                {
                    Console.WriteLine(uriResult.AbsoluteUri);
                    // Send an http GET request to the URL and await the response
                    try
                    {
                        HttpResponseMessage response = await client.GetAsync(uriResult.AbsoluteUri);

                        // If it's a 200 range response I.E OK add it as a validURL
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            validUrls.Add(url);
                        }
                    }
                    catch(InvalidCastException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }

            foreach(string url in validUrls)
            {
                Console.WriteLine(url);
            }
            return validUrls;
        }
        


        private bool validURL(string s, out Uri resultURI)
        {
            if (!Regex.IsMatch(s, @"^https?:\/\/", RegexOptions.IgnoreCase))
                s = "http://" + s;

            if (Uri.TryCreate(s, UriKind.Absolute, out resultURI))
                return (resultURI.Scheme == Uri.UriSchemeHttp ||
                        resultURI.Scheme == Uri.UriSchemeHttps);

            return false;
        }
    }
}
