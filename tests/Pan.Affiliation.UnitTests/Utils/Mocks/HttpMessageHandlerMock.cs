namespace Pan.Affiliation.UnitTests.Utils.Mocks
{
    public class HttpMessageHandlerMock : HttpMessageHandler
    {
        private readonly Func<HttpRequestMessage, CancellationToken, HttpResponseMessage> _getResponse;

        public HttpMessageHandlerMock(Func<HttpRequestMessage, CancellationToken, HttpResponseMessage> getResponse)
        {
            _getResponse = getResponse;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_getResponse(request, cancellationToken));
        }
    }
}
