namespace FloodGate.SDK
{
    public class DefaultClientConfig : ClientConfigBase, IClientConfig
    {
        public override async void InitializeConfig()
        {
            await FetchFlagsServerAsync();
        }
    }
}
