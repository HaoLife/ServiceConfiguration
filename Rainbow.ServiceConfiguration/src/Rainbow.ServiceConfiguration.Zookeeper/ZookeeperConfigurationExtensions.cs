using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.ServiceConfiguration.Zookeeper
{
    public static class ZookeeperConfigurationExtensions
    {

        public static IConfigurationBuilder AddAzureKeyVault(
            this IConfigurationBuilder configurationBuilder,
            string connection,
            string path)
        {
            if (configurationBuilder == null)
            {
                throw new ArgumentNullException(nameof(configurationBuilder));
            }
            if (connection == null)
            {
                throw new ArgumentNullException(nameof(connection));
            }
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            configurationBuilder.Add(new ZookeeperConfigurationSource()
            {
                Connection = connection,
                Path = path
            });

            return configurationBuilder;
        }
    }
}
