using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using org.apache.zookeeper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Rainbow.ServiceConfiguration.Zookeeper
{
    public class ZookeeperConfigurationProvider : ConfigurationProvider
    {
        private ZooKeeper _zkClient;
        private ZookeeperConfigurationSource _source;
        private SortedDictionary<string, string> _cache = new SortedDictionary<string, string>();

        public ZookeeperConfigurationProvider(ZookeeperConfigurationSource source)
        {
            this._source = source;
            this._zkClient = new ZooKeeper(source.Connection, source.SessionTimeout, new ZookeeperSubscribeWatcher(this));
        }

        public override void Load() => LoadAsync().ConfigureAwait(false).GetAwaiter().GetResult();

        private async Task LoadAsync()
        {
            var cache = new SortedDictionary<string, string>();
            var node = await this._zkClient.getChildrenAsync(_source.Path, true);

            foreach (var item in node.Children)
            {
                var data = await this._zkClient.getDataAsync($"{_source.Path}/{item}", true);

                var input = new MemoryStream(data.Data);
                var kValue = _source.Parser.Parse(input);

                AddRange(cache, kValue);
            }
        }


        private void AddRange(IDictionary<string, string> cache, IDictionary<string, string> data)
        {
            foreach(var item in data)
            {
                cache.Add(item.Key, item.Value);
            }
        }


    }
}
