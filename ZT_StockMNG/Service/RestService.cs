using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZT_StockMNG.Model;

namespace ZT_StockMNG.Service
{
    public class RestService
    {
        private HttpClient client;

        public RestService()
        {
            client = new HttpClient();
            client.Timeout = new TimeSpan(0, 3, 0);
            client.MaxResponseContentBufferSize = 256000;
        }

        public async Task<List<Article>> GetArticlesAsync()
        {
            Uri uri = new Uri($"{Constants.APIBaseUrl}Article/GetArticles");

            try
            {
                var response = await client.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    List<Article> result = JsonConvert.DeserializeObject<List<Article>>(content);

                    return result;
                }
                else
                    throw new Exception(response.ReasonPhrase);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<Article>> GetArticlesByAsync(string articleCode, string description, decimal qty)
        {
            Uri uri = new Uri($"{Constants.APIBaseUrl}Article/GetArticlesBy?articleCode={articleCode}&description={description}&qty={qty}");

            try
            {
                var response = await client.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    List<Article> result = JsonConvert.DeserializeObject<List<Article>>(content);

                    return result;
                }
                else
                    throw new Exception(response.ReasonPhrase);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task ModifyArticleAsync(Article article)
        {
            Uri uri = new Uri($"{Constants.APIBaseUrl}Article/PostModifyArticle?&articleDTO={article}");

            try
            {
                HttpContent reqContent = new StringContent(JsonConvert.SerializeObject(article), Encoding.UTF8, "application/json");
                var response = await client.PostAsync(uri, reqContent);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    List<Article> result = JsonConvert.DeserializeObject<List<Article>>(content);
                }
                else
                    throw new Exception(response.ReasonPhrase);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
