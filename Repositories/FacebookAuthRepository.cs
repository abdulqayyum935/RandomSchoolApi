using CrudAPIWithRepositoryPattern.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrudAPIWithRepositoryPattern.Helpers;
using System.Net.Http;
using Newtonsoft.Json;

namespace CrudAPIWithRepositoryPattern.Repositories
{
    public class FacebookAuthRepository : IFacebookAuthRepository
    {
        /*
        private const string TokenValidationURL = "https://graph.facebook.com/debug_token?input_token=EAAEv9JIAO2oBAFAOeTthZCVweG83RexyZAzCJmHX4d6S5LeZCYZC05ApEvsYKdSt5gmjuYPYBwdPjhcr2nidnyCTxEuZA8ZC3n9rSw3iRq7BZAwJy89A5JZC9mQlrJPWfXvKcN7VrWCS8A1Gm6ZBxTjtegnwcVQWnH078fGSZAoW4HgyBbWq80EcOWuXznXaetGFPCOUIlvwczc1W8W2Mvuw8gYiUwVRyjoZAcZD&access_token=334202444725098|2469a79fdbd290d4bde874b5ae4d445a";
        private const string UserInfoUrl = "https://graph.facebook.com/me?fields=name,email&access_token=EAAEv9JIAO2oBAFAOeTthZCVweG83RexyZAzCJmHX4d6S5LeZCYZC05ApEvsYKdSt5gmjuYPYBwdPjhcr2nidnyCTxEuZA8ZC3n9rSw3iRq7BZAwJy89A5JZC9mQlrJPWfXvKcN7VrWCS8A1Gm6ZBxTjtegnwcVQWnH078fGSZAoW4HgyBbWq80EcOWuXznXaetGFPCOUIlvwczc1W8W2Mvuw8gYiUwVRyjoZAcZD";
        */

        private const string TokenValidationURL = "https://graph.facebook.com/debug_token?input_token={0}&access_token={1}|{2}";
        private const string UserInfoUrl = "https://graph.facebook.com/me?fields=name,email&access_token={0}";

        private const string AppId = "334202444725098";
        private const string AppSecret = "2469a79fdbd290d4bde874b5ae4d445a";

        private readonly IHttpClientFactory httpClientFactory;

        public FacebookAuthRepository(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }
        public async Task<FacebookTokenValidationResult> ValidateAccessTokenAsync(string accessToken)
        {
            var formattedURL = string.Format(TokenValidationURL, accessToken, AppId, AppSecret);
            var result = await httpClientFactory.CreateClient().GetAsync(formattedURL);
            result.EnsureSuccessStatusCode();

            var responseAsString = await result.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<FacebookTokenValidationResult>(responseAsString);
        }

        public async Task<FacebookUserInfoResult> GetUserInfoAsync(string accessToken)
        {
            var formattedURL = string.Format(UserInfoUrl, accessToken);
            var result = await httpClientFactory.CreateClient().GetAsync(formattedURL);
            result.EnsureSuccessStatusCode();

            var responseAsString = await result.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<FacebookUserInfoResult>(responseAsString);
        }
    }
}
