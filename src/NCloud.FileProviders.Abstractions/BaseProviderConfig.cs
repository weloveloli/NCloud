// -----------------------------------------------------------------------
// <copyright file="BaseProviderConfig.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.Abstractions
{
    using System;

    /// <summary>
    /// Defines the <see cref="BaseProviderConfig" />.
    /// </summary>
    public abstract class BaseProviderConfig
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseProviderConfig"/> class.
        /// </summary>
        public BaseProviderConfig()
        {
            this.configTime = DateTime.Now;
        }

        /// <summary>
        /// Gets or sets the Prefix.
        /// </summary>
        public string Prefix { get; set; }

        /// <summary>
        /// Gets or sets the Type.
        /// </summary>
        public abstract string Type { get; }

        /// <summary>
        /// The HashKey.
        /// </summary>
        /// <returns>The <see cref="string"/>.</returns>
        public virtual string HashKey()
        {
            return this.configTime.Ticks.ToString();
        }

        /// <summary>
        /// The Update.
        /// </summary>
        public void Update()
        {
            this.configTime = DateTime.Now;
        }

        /// <summary>
        /// Defines the configTime.
        /// </summary>
        private DateTime configTime;
    }
}
