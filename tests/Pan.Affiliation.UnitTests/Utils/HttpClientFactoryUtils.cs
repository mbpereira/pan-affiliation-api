using NSubstitute;
using Pan.Affiliation.UnitTests.Utils.Mocks;
using System.Net;
using System.Text;

namespace Pan.Affiliation.UnitTests.Utils
{
    public static class HttpClientFactoryUtils
    {
        public static T CreateMockedHttpClientFactory<T>(
            Func<IHttpClientFactory, T> createService,
            string httpClientName = "Default",
            HttpStatusCode statusCode = HttpStatusCode.OK,
            string responseContent = "{}") =>
                
                CreateMockedHttpClientFactory(
                    createService,
                    httpClientName,
                    new HttpResponseMessage(statusCode)
                    {
                        Content = new StringContent(responseContent, Encoding.UTF8, "application/json")
                    });

        public static T CreateMockedHttpClientFactory<T>(
            Func<IHttpClientFactory, T> createService,
            string httpClientName = "Default",
            HttpResponseMessage? httpResponseMessage = null) =>

                CreateMockedHttpClientFactory(
                    createService,
                    httpClientName,
                    (_, _) => httpResponseMessage ?? GetDefaultResponseMessage());

        public static T CreateMockedHttpClientFactory<T>(
            Func<IHttpClientFactory, T> createService,
            string httpClientName = "Default",
            Func<HttpRequestMessage, CancellationToken, HttpResponseMessage>? getResponse = null)
        {
            var httpMessageHandler = new HttpMessageHandlerMock(getResponse != null
                ? getResponse
                : (_, _) => GetDefaultResponseMessage());

            var httpClientFactory = Substitute.For<IHttpClientFactory>();

            httpClientFactory.CreateClient(httpClientName)
                .Returns(new HttpClient(httpMessageHandler));

            return createService(httpClientFactory);
        }

        private static HttpResponseMessage GetDefaultResponseMessage()
        {
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}
