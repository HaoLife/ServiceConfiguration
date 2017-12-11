using org.apache.zookeeper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Rainbow.ServiceConfiguration.Zookeeper
{
    public class ZookeeperSubscribeWatcher : Watcher
    {
        private readonly ZookeeperConfigurationProvider _provider;

        public ZookeeperSubscribeWatcher(ZookeeperConfigurationProvider provider)
        {
            this._provider = provider;
        }

        public override Task process(WatchedEvent @event)
        {
            return Task.Run(() =>
            {
                this.NodeChange(@event);
            });
        }
        
        protected virtual void NodeChange(WatchedEvent @event)
        {
            switch (@event.getState())
            {
                case Watcher.Event.KeeperState.Expired:
                case Watcher.Event.KeeperState.SyncConnected:
                    //Connection(@event);
                    //todo:重新获取
                    this._provider.Load();
                    break;
            }
        }

    }
}
