// -----------------------------------------------------------------------
// <copyright file="DefaultNCloudFileProviderFactoryTests.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.SupportTests
{
    using System.Linq;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using NCloud.FileProviders.Abstractions;
    using NCloud.FileProviders.Support;

    /// <summary>
    /// Defines the <see cref="DefaultNCloudFileProviderFactoryTests" />.
    /// </summary>
    [TestClass()]
    public class DefaultNCloudFileProviderFactoryTests
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
            this.provider = serviceCollection.BuildServiceProvider();
        }

        /// <summary>
        /// The DefaultNCloudFileProviderFactoryTest.
        /// </summary>
        [TestMethod()]
        public void DefaultNCloudFileProviderFactoryTest()
        {
            var factory = new DefaultNCloudFileProviderFactory(provider);
            var registration = new DefaultNCloudDynamicFileProvider();
            registration.AddProvider( factory.CreateProvider("test:/123.txt;/abc/123.txt;/abc/124.txt;/abc/125.txt;/efd/125.txt;/abc/efd/125.txt", "/test"));
            registration.AddProvider(factory.CreateProvider("test:/124.txt;/abc/124.txt", "/test2"));
            var root = registration.GetDirectoryContents("/");
            Assert.AreEqual(2, root.Count());
            var content = registration.GetDirectoryContents("/test");
            Assert.IsTrue(content.Any());
            Assert.AreEqual(3, content.Count());
            content = registration.GetDirectoryContents("/test/abc");
            Assert.IsTrue(content.Any());
            Assert.AreEqual(4, content.Count());
            var fileInfo = registration.GetFileInfo("/test2/abc/124.txt");
            Assert.IsTrue(fileInfo.Exists);
            Assert.IsFalse(fileInfo.IsDirectory);
            Assert.IsTrue(fileInfo is NCloudFileInfo);
            var nCloudFileInfo = (NCloudFileInfo)fileInfo;
            Assert.AreEqual("/test2/abc/124.txt", nCloudFileInfo.Path);

            content = registration.GetDirectoryContents("/");
            Assert.AreEqual(2, content.Count());
        }
    }
}
