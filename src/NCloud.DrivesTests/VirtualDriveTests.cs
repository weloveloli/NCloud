// -----------------------------------------------------------------------
// <copyright file="VirtualDriveTests.cs" company="Weloveloli">
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
    using Moq;
    using NCloud.Core;
    using NCloud.Drivers;

    /// <summary>
    /// Defines the <see cref="VirtualDriveTests" />.
    /// </summary>
    [TestClass()]
    public class VirtualDriveTests
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
            this.systemHelper = new DefaultSystemHelper();
            serviceCollection.AddSingleton(systemHelper);
            this.provider = serviceCollection.BuildServiceProvider();
        }

        /// <summary>
        /// The VirtualDriveTest.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [TestMethod()]
        public async Task VirtualDriveTest()
        {
            var config = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "samples", "sample1.yml"));
            config = $"virtual:{systemHelper.EncodeBase64(config)}";
            var drive = new VirtualDrive(config, "/test1", this.provider);
            var osinfos = await drive.GetFileInfosByPathAsync("os");
            Assert.IsTrue(osinfos.Data is IEnumerable<Core.Model.FileInfo>);
            var osInfoData = (IEnumerable<Core.Model.FileInfo>)osinfos.Data;
            Assert.AreEqual(1, osInfoData.Count());
            Assert.AreEqual("/test1/os/linux", osInfoData.First().Path);

            var linuxInfos = await drive.GetFileInfosByPathAsync("os/linux");
            Assert.IsTrue(linuxInfos.Data is IEnumerable<Core.Model.FileInfo>);
            var linuxInfosData = (IEnumerable<Core.Model.FileInfo>)linuxInfos.Data;
            Assert.AreEqual(2, linuxInfosData.Count());
            Assert.AreEqual("/test1/os/linux/debian", linuxInfosData.First().Path);

            var debianInfos = await drive.GetFileInfosByPathAsync("os/linux/debian");
            Assert.IsTrue(debianInfos.Data is IEnumerable<Core.Model.FileInfo>);
            var debianInfosData = (IEnumerable<Core.Model.FileInfo>)debianInfos.Data;
            Assert.AreEqual(2, debianInfosData.Count());

            var ubuntuInfos = await drive.GetFileInfosByPathAsync("os/linux/ubuntu");
            Assert.IsTrue(ubuntuInfos.Data is IEnumerable<Core.Model.FileInfo>);
            var ubuntuInfosData = (IEnumerable<Core.Model.FileInfo>)ubuntuInfos.Data;
            Assert.AreEqual(4, ubuntuInfosData.Count());


            var ubuntuFile = await drive.GetFileInfosByPathAsync("os/linux/ubuntu/ubuntu-18.04.1-desktop-amd64.iso");
            Assert.IsTrue(ubuntuFile.Data is Core.Model.FileInfo);
            var ubuntuFileInfo = (Core.Model.FileInfo)ubuntuFile.Data;
            Assert.AreEqual("http://releases.ubuntu.com/18.04.1/ubuntu-18.04.1-desktop-amd64.iso", ubuntuFileInfo.RemoteUrl);
            Assert.AreEqual("/test1/os/linux/ubuntu/ubuntu-18.04.1-desktop-amd64.iso", ubuntuFileInfo.Path);
        }
        
    }
}
