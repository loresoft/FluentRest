namespace FluentRest
{
    public interface IFluentClientFactory {
        IFluentClient CreateClient(string name);
    }
}