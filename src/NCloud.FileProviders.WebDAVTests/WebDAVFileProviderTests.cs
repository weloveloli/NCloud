// -----------------------------------------------------------------------
// <copyright file="WebDAVFileProviderTests.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.WebDAV.Tests
{
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using NCloud.FileProviders.WebDAV;

    /// <summary>
    /// Defines the <see cref="WebDAVFileProviderTests" />.
    /// </summary>
    [TestClass()]
    public class WebDAVFileProviderTests
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
            serviceCollection.AddSingleton(new Mock<ILogger<WebDAVFileProvider>>().Object);
            this.provider = serviceCollection.BuildServiceProvider();
        }

        /// <summary>
        /// The WebDAVFileProviderTest.
        /// </summary>
        [TestMethod()]
        [Ignore]
        public void WebDAVFileProviderTest()
        {
           var fileProvider = new WebDAVFileProvider(provider, new WebDAVConfig { Url = "http://127.0.0.1:6666", BasePath = "/", User = "admin", Password = "admin", Prefix = "/webdav" });
           var contents = fileProvider.GetDirectoryContents("/webdav");
        }
    }
}
