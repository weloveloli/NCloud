// -----------------------------------------------------------------------
// <copyright file="VirtualFileProviderTests.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.Virtual.Tests
{
    using System.IO;
    using System.Linq;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using NCloud.FileProviders.Abstractions;
    using NCloud.FileProviders.Support;
    using NCloud.Utils;

    /// <summary>
    /// Defines the <see cref="VirtualFileProviderTests" />.
    /// </summary>
    [TestClass()]
    public class VirtualFileProviderTests
    {
        /// <summary>
        /// Defines the provider.
        /// </summary>
        private ServiceProvider provider;

        /// <summary>
        /// The Init.
        /// </summary>
        [TestInitialize]
        public void Init()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<INCloudFileProviderFactory, DefaultNCloudFileProviderFactory>();
            serviceCollection.AddSingleton(new Mock<ILogger<EmbeddableCompositeNCloudFileProvider>> ().Object);
            this.provider = serviceCollection.BuildServiceProvider();
        }

        /// <summary>
        /// The VirtualFileProviderTest.
        /// </summary>
        [TestMethod()]
        public void VirtualFileProviderTest()
        {
            var config = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "samples", "sample1.yml"));
            config = $"virtual:{config.EncodeBase64()}";
            var v = new VirtualFileProvider(provider, config, "/v");

            var parent = v.GetDirectoryContents("/");
            Assert.IsTrue(parent.Exists);
            Assert.AreEqual(1, parent.Count());
            var parentFileInfo = parent.First();
            Assert.AreEqual("v", parentFileInfo.Name);
            Assert.IsTrue(parentFileInfo.Exists);
            Assert.IsTrue(parentFileInfo.IsDirectory);

            var root = v.GetDirectoryContents("/v");
            Assert.IsTrue(root.Exists);
            Assert.AreEqual(2, root.Count());
            var rootFileInfo = root.First();
            Assert.AreEqual("os", rootFileInfo.Name);
            Assert.IsTrue(rootFileInfo.Exists);
            Assert.IsTrue(rootFileInfo.IsDirectory);

            var ubuntu = v.GetDirectoryContents("/v/os/linux/ubuntu");
            Assert.IsTrue(ubuntu.Exists);
            Assert.AreEqual(4, ubuntu.Count());
            var ubuntuFileInfo = ubuntu.First();
            Assert.AreEqual("ubuntu-18.04.1-desktop-amd64.iso", ubuntuFileInfo.Name);
            Assert.IsTrue(ubuntuFileInfo.Exists);
            Assert.IsFalse(ubuntuFileInfo.IsDirectory);
            Assert.IsFalse(ubuntuFileInfo is HttpRemoteFileInfo);

            var notfound = v.GetDirectoryContents("/v/os/notfound");
            Assert.IsFalse(notfound.Exists);
        }

        /// <summary>
        /// The VirtualFileProviderTest.
        /// </summary>
        [TestMethod()]
        public void VirtualFileProviderTest2()
        {
            var config = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "samples", "sample2.yml"));
            config = $"virtual:{config.EncodeBase64()}";
            var v = new VirtualFileProvider(provider, config, "/v");

            var parent = v.GetDirectoryContents("/");
            Assert.IsTrue(parent.Exists);
            Assert.AreEqual(1, parent.Count());
            var parentFileInfo = parent.First();
            Assert.AreEqual("v", parentFileInfo.Name);
            Assert.IsTrue(parentFileInfo.Exists);
            Assert.IsTrue(parentFileInfo.IsDirectory);

            var root = v.GetDirectoryContents("/v");
            Assert.IsTrue(root.Exists);
            Assert.AreEqual(2, root.Count());

            var ubuntu = v.GetDirectoryContents("/v/os/linux/ubuntu");
            Assert.IsTrue(ubuntu.Exists);
            Assert.AreEqual(4, ubuntu.Count());
            var ubuntuFileInfo = ubuntu.First();
            Assert.AreEqual("ubuntu-18.04.1-desktop-amd64.iso", ubuntuFileInfo.Name);
            Assert.IsTrue(ubuntuFileInfo.Exists);
            Assert.IsFalse(ubuntuFileInfo.IsDirectory);
            Assert.IsFalse(ubuntuFileInfo is HttpRemoteFileInfo);

            var notfound = v.GetDirectoryContents("/v/os/notfound");
            Assert.IsFalse(notfound.Exists);

            var test = v.GetDirectoryContents("/v/test");
            Assert.IsTrue(test.Exists);

            var fileInfo = v.GetFileInfo("/v/test/abc/124.txt");
            Assert.IsTrue(fileInfo.Exists);
            Assert.IsFalse(fileInfo.IsDirectory);
            Assert.IsTrue(fileInfo is NCloudFileInfo);
            var nCloudFileInfo = (NCloudFileInfo)fileInfo;
            Assert.AreEqual("/v/test/abc/124.txt", nCloudFileInfo.Path);
        }
    }
}
