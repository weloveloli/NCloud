// -----------------------------------------------------------------------
// <copyright file="StringExtensionsTests.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.Utils.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Defines the <see cref="StringExtensionsTests" />.
    /// </summary>
    [TestClass()]
    public class StringExtensionsTests
    {
        /// <summary>
        /// The IsSubpathOfTest.
        /// </summary>
        [TestMethod()] 
        public void IsSubpathOfTest()
        {
            Assert.IsTrue("/test".IsSubpathOf("/"));
            Assert.IsFalse("/test".IsSubpathOf("/test"));
            Assert.IsFalse("/test2".IsSubpathOf("/test"));
        }
    }
}
