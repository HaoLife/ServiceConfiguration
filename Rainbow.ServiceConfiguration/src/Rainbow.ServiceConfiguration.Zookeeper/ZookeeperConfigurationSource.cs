using Microsoft.Extensions.Configuration;
using org.apache.zookeeper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.ServiceConfiguration.Zookeeper
{
    public class ZookeeperConfigurationSource : IConfigurationSource
    {
        public string Connection { get; set; }
        public string ServiceName { get; set; }
        public int SessionTimeout { get; set; } = 30000;


        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new ZookeeperConfigurationProvider(this);
        }
    }
}
