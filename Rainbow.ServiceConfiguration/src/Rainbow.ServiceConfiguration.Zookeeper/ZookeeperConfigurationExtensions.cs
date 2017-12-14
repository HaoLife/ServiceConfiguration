using Microsoft.Extensions.Configuration;
using Rainbow.ServiceConfiguration.Zookeeper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.Configuration
{
    public static class ZookeeperConfigurationExtensions
    {

        public static IConfigurationBuilder AddZookeeper(
            this IConfigurationBuilder configurationBuilder,
            string connection,
            string serviceName)
        {
            if (configurationBuilder == null)
            {
                throw new ArgumentNullException(nameof(configurationBuilder));
            }
            if (connection == null)
            {
                throw new ArgumentNullException(nameof(connection));
            }
            if (serviceName == null)
            {
                throw new ArgumentNullException(nameof(serviceName));
            }

            configurationBuilder.Add(new ZookeeperConfigurationSource()
            {
                Connection = connection,
                ServiceName = serviceName
            });

            return configurationBuilder;
        }
    }
}
