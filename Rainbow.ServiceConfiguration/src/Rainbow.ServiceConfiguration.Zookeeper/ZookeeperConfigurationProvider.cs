using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using org.apache.zookeeper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using static org.apache.zookeeper.ZooDefs;

namespace Rainbow.ServiceConfiguration.Zookeeper
{
    public class ZookeeperConfigurationProvider : ConfigurationProvider
    {
        private ZooKeeper _zkClient;
        private ZookeeperConfigurationSource _source;
        private SortedDictionary<string, IDictionary<string, string>> _cache = new SortedDictionary<string, IDictionary<string, string>>();

        public ZookeeperConfigurationProvider(ZookeeperConfigurationSource source)
        {
            this._source = source;
            this._zkClient = new ZooKeeper(source.Connection, source.SessionTimeout, new ZookeeperSubscribeWatcher(this));
        }

        public override void Load() => LoadAsync().ConfigureAwait(false).GetAwaiter().GetResult();

        private async Task LoadAsync()
        {
            var cache = new SortedDictionary<string, IDictionary<string, string>>();
            var path = $"/{ZookeeperDefaults.ConfigPath}/{_source.ServiceName}";

            try
            {
                var node = await this._zkClient.getChildrenAsync(path, true);

                foreach (var item in node.Children)
                {
                    await this.AddItem(cache, $"{path}/{item}");
                }
                _cache = cache;
            }
            catch (KeeperException.NoNodeException noex)
            {
                CreateNode($"/{ZookeeperDefaults.ConfigPath}");
                CreateNode(path);
                await LoadAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void CreateNode(string node)
        {
            if (this._zkClient.existsAsync(node).GetAwaiter().GetResult() == null)
            {
                this._zkClient.createAsync(node, null, Ids.OPEN_ACL_UNSAFE, CreateMode.PERSISTENT).GetAwaiter().GetResult();
            }
        }




        private async Task AddItem(SortedDictionary<string, IDictionary<string, string>> cache, string path)
        {
            var data = await this._zkClient.getDataAsync(path, true);

            var input = new MemoryStream(data.Data);
            var parser = new JsonConfigurationFileParser();
            var kValue = parser.Parse(input);

            if (cache.ContainsKey(path))
            {
                cache[path] = kValue;
            }
            else
            {
                cache.Add(path, kValue);
            }

        }


    }
}
