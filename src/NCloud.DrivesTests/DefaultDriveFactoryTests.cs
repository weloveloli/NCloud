// -----------------------------------------------------------------------
// <copyright file="DefaultDriveFactoryTests.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.Drives.Tests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using NCloud.Core;
    using NCloud.Drivers;

    /// <summary>
    /// Defines the <see cref="DefaultDriveFactoryTests" />.
    /// </summary>
    [TestClass()]
    public class DefaultDriveFactoryTests
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
            this.systemHelper = new DefaultSystemHelper();
            serviceCollection.AddSingleton(systemHelper);
            serviceCollection.AddSingleton<IDriveFactory, DefaultDriveFactory>();
            this.provider = serviceCollection.BuildServiceProvider();
        }

        /// <summary>
        /// The DefaultDriveFactoryTest.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [TestMethod()]
        public async Task DefaultDriveFactoryTest()
        {
            IDriveFactory driveFactory = this.provider.GetService<IDriveFactory>();
            var config = File.ReadAllText(Directory.GetCurrentDirectory() + @"\samples\sample1.yml");
            config = $"virtual:{systemHelper.EncodeBase64(config)}";
            driveFactory.TryEnableDrive("/test1", config);
            var osinfos = await driveFactory.GetFileInfosByPathAsync("/test1/os");
            Assert.IsTrue(osinfos.Data is IEnumerable<Core.Model.FileInfo>);
            var osInfoData = (IEnumerable<Core.Model.FileInfo>)osinfos.Data;
            Assert.AreEqual(1, osInfoData.Count());
        }

        /// <summary>
        /// The VirtualDriveTest.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [TestMethod()]
        public async Task DefaultDriveFactoryTest2()
        {
            IDriveFactory driveFactory = this.provider.GetService<IDriveFactory>();
            var config = File.ReadAllText(Directory.GetCurrentDirectory() + @"\samples\sample2.yml");
            config = $"virtual:{systemHelper.EncodeBase64(config)}";
            driveFactory.TryEnableDrive("/test2", config);
            var osinfos = await driveFactory.GetFileInfosByPathAsync("/test2/os");
            Assert.IsTrue(osinfos.Data is IEnumerable<Core.Model.FileInfo>);

            var fsInfo = await driveFactory.GetFileInfosByPathAsync("/test2/telegram");
        }
    }
}
