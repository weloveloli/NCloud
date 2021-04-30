// -----------------------------------------------------------------------
// <copyright file="DefaultSystemHelperTests.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.Drivers.Tests
{
    using System;
    using System.IO;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Defines the <see cref="DefaultSystemHelperTests" />.
    /// </summary>
    [TestClass()]
    public class DefaultSystemHelperTests
    {
        /// <summary>
        /// Defines the helper.
        /// </summary>
        private DefaultSystemHelper helper;

        /// <summary>
        /// The Init.
        /// </summary>
        [TestInitialize]
        public void Init()
        {
            this.helper = new DefaultSystemHelper();
        }

        /// <summary>
        /// The DecodeBase64Test.
        /// </summary>
        [TestMethod()]
        public void DecodeBase64Test()
        {
            Assert.AreEqual("abc", helper.DecodeBase64(helper.EncodeBase64("abc")));
        }

        /// <summary>
        /// The DenormalizePathTest.
        /// </summary>
        [TestMethod()]
        public void DenormalizePathTest()
        {
            if (OperatingSystem.IsWindows())
            {
                Assert.AreEqual(@"c:\abc\edf", helper.DenormalizePath("/c/abc/edf"));
                Assert.AreEqual(Path.Combine(Directory.GetCurrentDirectory(),"abc","edf"), helper.DenormalizePath("./abc/edf"));
            }

        }

        /// <summary>
        /// The NormalizePathTest.
        /// </summary>
        [TestMethod()]
        public void NormalizePathTest()
        {
            if (OperatingSystem.IsWindows())
                Assert.AreEqual("/c/abc", helper.NormalizePath(@"c:\abc"));
        }
    }
}
