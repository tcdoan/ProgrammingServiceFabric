﻿using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using System.IO;

namespace HelloWorldService
{
    /// <summary>
    /// An instance of this class is created for each service instance by the Service Fabric runtime.
    /// </summary>
    internal sealed class HelloWorldService : StatelessService
    {
        public HelloWorldService(StatelessServiceContext context)
            : base(context)
        { }

        /// <summary>
        /// Optional override to create listeners (e.g., TCP, HTTP) for this service replica to handle client or user requests.
        /// </summary>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return new ServiceInstanceListener[0];
        }

        /// <summary>
        /// This is the main entry point for your service instance.
        /// </summary>
        /// <param name="cancellationToken">Canceled when Service Fabric needs to shut down this service instance.</param>
        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var configSection = this.Context.CodePackageActivationContext.GetConfigurationPackageObject("Config");
                var parmValue = configSection.Settings.Sections["MyConfigSection"].Parameters["MyParameter"].Value;

                var dataPackage = this.Context.CodePackageActivationContext.GetDataPackageObject("TestData");
                var text = File.ReadAllText(Path.Combine(dataPackage.Path, "Data.txt"));
                ServiceEventSource.Current.ServiceMessage(this.Context, text);
                await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            }
        }
    }
}