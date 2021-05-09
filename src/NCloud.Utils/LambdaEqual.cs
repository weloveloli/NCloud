// -----------------------------------------------------------------------
// <copyright file="LamdaEqual.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.Utils
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Defines the <see cref="LambdaEqual" />.
    /// </summary>
    /// <typeparam name="T">.</typeparam>
    public class LambdaEqual<T> : IEqualityComparer<T>
    {
        /// <summary>
        /// Defines the comparer.
        /// </summary>
        public Func<T, T, bool> comparer;

        public Func<T, int> hashCoder;

        /// <summary>
        /// Initializes a new instance of the <see cref="LambdaEqual{T}"/> class.
        /// </summary>
        /// <param name="comparer">The comparer<see cref="Func{T, T, bool}"/>.</param>
        public LambdaEqual(Func<T, T, bool> comparer, Func<T, int> hashCoder)
        {
            this.comparer = comparer;
            this.hashCoder = hashCoder;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LambdaEqual{T}"/> class.
        /// </summary>
        /// <param name="comparer">The comparer<see cref="Func{T, T, bool}"/>.</param>
        public LambdaEqual(Func<T, string> propertySelector)
        {
            this.comparer = (a,b)=>propertySelector(a) == propertySelector(b);
            this.hashCoder = (a) =>propertySelector(a).GetHashCode();
        }

        /// <summary>
        /// The Equals.
        /// </summary>
        /// <param name="a">The a<see cref="T"/>.</param>
        /// <param name="b">The b<see cref="T"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool Equals(T a, T b)
        {
            return comparer(a, b);
        }

        /// <summary>
        /// The GetHashCode.
        /// </summary>
        /// <param name="a">The a<see cref="T"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        public int GetHashCode(T a)
        {
            return hashCoder(a);
        }
    }
}
