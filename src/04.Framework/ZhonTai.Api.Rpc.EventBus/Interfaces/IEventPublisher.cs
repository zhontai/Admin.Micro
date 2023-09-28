namespace ZhonTai.Api.Rpc.EventBus;

public interface IEventPublisher
{
    Task PublishAsync<T>(T eventObj, string? callbackName = null, CancellationToken cancellationToken = default) where T : class;

    Task PublishAsync<T>(T eventObj, IDictionary<string, string?> headers, CancellationToken cancellationToken = default) where T : class;

    void Publish<T>(T eventObj, string? callbackName = null) where T : class;

    void Publish<T>(T eventObj, IDictionary<string, string?> headers) where T : class;
}