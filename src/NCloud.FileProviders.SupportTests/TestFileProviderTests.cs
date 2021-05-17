// -----------------------------------------------------------------------
// <copyright file="TestFileProviderTests.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.Support.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Defines the <see cref="TestFileProviderTests" />.
    /// </summary>
    [TestClass()]
    public class TestFileProviderTests
    {
        /// <summary>
        /// The TestFileProviderTest.
        /// </summary>
        [TestMethod()]
        public void TestFileProviderTest()
        {
            var attr = typeof(TestFileProvider).GetCustomAttributes(typeof(FileProviderAttribute),false);
            Assert.AreEqual(1, attr.Length);
        }
    }
}
