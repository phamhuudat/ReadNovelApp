using Newtonsoft.Json;
using NovelApp.Configurations;
using NovelApp.Models.BookGwModels;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovelApp.Services.RequestProvider
{
    public class RequestProvider : IRequestProvider
    {
        private IRestClient _restClient;
        private string _token = "1";
        private string _gatewayBase;
        private string _pathPrefix;
        public RequestProvider()
        {
            _gatewayBase = BookGatewaySettings.Instance.GatewayBase;
            _pathPrefix = BookGatewaySettings.Instance.PathPrefix;
            _restClient = CreateRestClient();
        }
        private IRestClient CreateRestClient()
        {
            var client = new RestClient(_gatewayBase);
            return client;
        }
        private RestRequest CreateRestRequest(string uri, Method method = Method.GET, string token = "")
        {
            var request = new RestRequest($"{_pathPrefix}{uri}", method);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            if (!string.IsNullOrWhiteSpace(token))
                request.AddParameter("token", token);
            request.JsonSerializer = new JsonNetSerializer();
            return request;
        }
        public Task<ResponseObject<TData>> Delete<TData>(string uri, IReadOnlyCollection<RequestParameter> parameters)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseObject<TData>> Get<TData>(string uri, IReadOnlyCollection<RequestParameter> parameters)
        {
            var restRequest = CreateRestRequest(uri, Method.GET, _token);
            if (parameters != null && parameters.Any())
                foreach (var param in parameters)
                    restRequest.AddParameter(param.Name, param.Value);
            var response = await _restClient.ExecuteGetAsync(restRequest);
            var result = response.IsSuccessful ? JsonConvert.DeserializeObject<ResponseObject<TData>>(response.Content) : default;
            return result;
        }

        public async Task<ResponseObject<TData>> Post<TData>(string uri, IReadOnlyCollection<RequestParameter> parameters)
        {
            var restRequest = CreateRestRequest(uri, Method.POST, _token);
            if (parameters != null && parameters.Any())
                foreach (var param in parameters)
                    restRequest.AddParameter(param.Name, param.Value);
            var response = await _restClient.ExecutePostAsync(restRequest);
            var result = response.IsSuccessful ? JsonConvert.DeserializeObject<ResponseObject<TData>>(response.Content) : default;
            return result;
        }

        public Task<ResponseObject<TData>> Put<TData>(string uri, IReadOnlyCollection<RequestParameter> parameters)
        {
            throw new NotImplementedException();
        }
    }
}
