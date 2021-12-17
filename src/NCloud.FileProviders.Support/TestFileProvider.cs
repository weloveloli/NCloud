// -----------------------------------------------------------------------
// <copyright file="TestFileProvider.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.Support
{
    using System;

    /// <summary>
    /// Defines the <see cref="TestFileProvider" />.
    /// </summary>
    [FileProvider(Name = "test", Type = "test")]

    public class TestFileProvider : DictionaryBasedFileProvider<TestFileProviderConfig>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestFileProvider"/> class.
        /// </summary>
        /// <param name="provider">The provider<see cref="IServiceProvider"/>.</param>
        /// <param name="config">The config<see cref="string"/>.</param>
        public TestFileProvider(IServiceProvider provider, TestFileProviderConfig config) : base(provider, config)
        {
        }
    }
}
