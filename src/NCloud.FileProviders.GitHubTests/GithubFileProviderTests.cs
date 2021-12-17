// -----------------------------------------------------------------------
// <copyright file="GithubFileProviderTests.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.GitHub.Tests
{
    using System.Linq;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Defines the <see cref="GithubFileProviderTests" />.
    /// </summary>
    [TestClass()]
    public class GithubFileProviderTests
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
        /// The GithubFileProviderTest.
        /// </summary>
        [TestMethod()]
        public void GithubFileProviderTest()
        {
            var github = new GithubFileProvider(provider, new GitProviderConfig { Owner = "weloveloli", Project = "NCloud", Prefix = "/github" });
            var contents = github.GetDirectoryContents("/github");
            Assert.IsTrue(contents.Exists);
            Assert.IsTrue(contents.Any());
        }
    }
}
