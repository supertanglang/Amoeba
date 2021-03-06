using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Amoeba.Rpc;
using Amoeba.Service;
using Omnius.Base;
using Xunit;
using Xunit.Abstractions;

namespace Amoeba.Tests
{
    [Trait("Category", "Amoeba.Rpc")]
    public class RpcTests : TestsBase
    {
        private readonly Random _random = new Random();

        public RpcTests(ITestOutputHelper output) : base(output)
        {

        }

        [Fact]
        public void ConnectTest()
        {
            var bufferManager = new BufferManager(1024 * 1024 * 256, 1024 * 1024 * 256);
            var serviceManager = new ServiceManager("config", "cache.blocks", bufferManager);
            serviceManager.Load();
            serviceManager.Start();

            var server = new AmoebaDaemonManager();
            var client = new AmoebaInterfaceManager();

            var endpoint = new IPEndPoint(IPAddress.Loopback, 4040);

            var task = server.Watch(serviceManager, endpoint);
            client.Connect(endpoint);

            var s = client.State;
            client.Exit();

            task.Wait();
        }
    }
}
