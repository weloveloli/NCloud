// -----------------------------------------------------------------------
// <copyright file="PhysicalNCloudFileProviderTests.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.Physical.Tests
{
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.FileProviders;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using NCloud.FileProviders.Abstractions.Extensions;

    /// <summary>
    /// Defines the <see cref="PhysicalNCloudFileProviderTests" />.
    /// </summary>
    [TestClass()]
    public class PhysicalNCloudFileProviderTests
    {
        private ServiceProvider provider;

        [TestInitialize]
        public void Init()
        {
            var serviceCollection = new ServiceCollection();
            this.provider = serviceCollection.BuildServiceProvider();
        }
        /// <summary>
        /// The PhysicalNCloudFileProviderTest.
        /// </summary>
        [TestMethod()]
        public void PhysicalNCloudFileProviderTest()
        {
            var fileProvider = new PhysicalNCloudFileProvider(provider, "fs:./example", "/test1");
            var contents = fileProvider.GetDirectoryContents("/");
            Assert.AreEqual(NotFoundDirectoryContents.Singleton, contents);
            var contents2 = fileProvider.GetDirectoryContents("/test1");
            Assert.AreNotEqual(NotFoundDirectoryContents.Singleton, contents2);
            var contents3 = fileProvider.GetDirectoryContents("/test1/abc");
            Assert.AreEqual(NotFoundDirectoryContents.Singleton, contents3);
            var contents4 = fileProvider.GetDirectoryContents("/test1/preview");
            Assert.AreNotEqual(NotFoundDirectoryContents.Singleton, contents4);

            var info1 = fileProvider.GetFileInfo("/test1/123.txt");
            Assert.IsTrue(info1.Exists);
            Assert.AreEqual("123", info1.ReadAsString());
        }
    }
}
