using FloodGate.SDK.Events;

namespace FloodGate.SDK
{
    public class DefaultClientConfig : ClientConfigBase, IClientConfig
    {
        public override async void InitializeConfig(IHttpResourceFetcher httpResourceFetcher)
        {
            base.InitializeConfig(httpResourceFetcher);

            await FetchFlagsServerAsync();
        }
    }
}
