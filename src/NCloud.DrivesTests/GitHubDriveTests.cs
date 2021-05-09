// -----------------------------------------------------------------------
// <copyright file="GitHubDriveTests.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.Drives.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.FileProviders;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using NCloud.Core;
    using NCloud.Core.Model;
    using NCloud.Drivers;

    /// <summary>
    /// Defines the <see cref="GitHubDriveTests" />.
    /// </summary>
    [TestClass()]
    public class GitHubDriveTests
    {
        /// <summary>
        /// Defines the provider.
        /// </summary>
        private IServiceProvider provider;

        /// <summary>
        /// Defines the systemHelper.
        /// </summary>
        private ISystemHelper systemHelper;

        /// <summary>
        /// The Init.
        /// </summary>
        [TestInitialize]
        public void Init()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton(new Mock<IDriveFactory>().Object);
            serviceCollection.AddSingleton(new HttpClient());
            this.systemHelper = new DefaultSystemHelper();
            serviceCollection.AddSingleton(systemHelper);
            this.provider = serviceCollection.BuildServiceProvider();
        }

        /// <summary>
        /// The GitHubDriveTest.
        /// </summary>
        [TestMethod()]
        public async Task GitHubDriveTest()
        {
            var drive = new GitHubDrive("github:weloveloli/NCloud", "/github", this.provider);
            var result = await drive.GetFileInfosByPathAsync("");
            Assert.IsTrue(result.Data is IEnumerable<FileInfo>);
            var fileInfo = new List<FileInfo> ();
            fileInfo.AddRange((IEnumerable<FileInfo>)result.Data);
            Assert.IsTrue(fileInfo.Where(e => e.Name == "README.md" && e.Path == "/github/README.md").Any());
        }
    }
}
