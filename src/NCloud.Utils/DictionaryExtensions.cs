// -----------------------------------------------------------------------
// <copyright file="DictionaryExtensions.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.Utils
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;

    /// <summary>
    /// Defines the <see cref="DictionaryExtensions" />.
    /// </summary>
    public static class DictionaryExtensions
    {
        /// <summary>
        /// This method is used to try to get a value in a dictionary if it exists.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="dictionary">The collection object.</param>
        /// <param name="key">Key.</param>
        /// <param name="value">Value of the key (or default value if key not exists).</param>
        /// <returns>True if key does exists in the dictionary.</returns>
        internal static bool TryGetValue<T>(this IDictionary<string, object> dictionary, string key, out T value) where T : notnull
        {
            if (dictionary.TryGetValue(key, out var valueObj) && valueObj is T obj)
            {
                value = obj;
                return true;
            }

            value = default!;
            return false;
        }

        /// <summary>
        /// Gets a value from the dictionary with given key. Returns default value if can not find.
        /// </summary>
        /// <typeparam name="TKey">Type of the key.</typeparam>
        /// <typeparam name="TValue">Type of the value.</typeparam>
        /// <param name="dictionary">Dictionary to check and get.</param>
        /// <param name="key">Key to find the value.</param>
        /// <returns>Value if found, default if can not found.</returns>
        public static TValue GetOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key) where TValue : notnull where TKey : notnull
        {
            return (dictionary.TryGetValue(key, out var obj) ? obj : default) ?? throw new NullReferenceException($"A {typeof(TValue)} has null a default");
        }

        /// <summary>
        /// Gets a value from the dictionary with given key. Returns default value if can not find.
        /// </summary>
        /// <typeparam name="TKey">Type of the key.</typeparam>
        /// <typeparam name="TValue">Type of the value.</typeparam>
        /// <param name="dictionary">Dictionary to check and get.</param>
        /// <param name="key">Key to find the value.</param>
        /// <returns>Value if found, default if can not found.</returns>
        public static TValue GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key) where TValue : notnull
        {
            return dictionary.TryGetValue(key, out var obj) ? obj : default!;
        }

        /// <summary>
        /// Gets a value from the dictionary with given key. Returns default value if can not find.
        /// </summary>
        /// <typeparam name="TKey">Type of the key.</typeparam>
        /// <typeparam name="TValue">Type of the value.</typeparam>
        /// <param name="dictionary">Dictionary to check and get.</param>
        /// <param name="key">Key to find the value.</param>
        /// <returns>Value if found, default if can not found.</returns>
        public static TValue GetOrDefault<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dictionary, TKey key) where TValue : notnull
        {
            return dictionary.TryGetValue(key, out var obj) ? obj : default!;
        }

        /// <summary>
        /// Gets a value from the dictionary with given key. Returns default value if can not find.
        /// </summary>
        /// <typeparam name="TKey">Type of the key.</typeparam>
        /// <typeparam name="TValue">Type of the value.</typeparam>
        /// <param name="dictionary">Dictionary to check and get.</param>
        /// <param name="key">Key to find the value.</param>
        /// <returns>Value if found, default if can not found.</returns>
        public static TValue GetOrDefault<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> dictionary, TKey key) where TValue : notnull where TKey : notnull
        {
            return dictionary.TryGetValue(key, out var obj) ? obj : default!;
        }

        /// <summary>
        /// Gets a value from the dictionary with given key. Returns default value if can not find.
        /// </summary>
        /// <typeparam name="TKey">Type of the key.</typeparam>
        /// <typeparam name="TValue">Type of the value.</typeparam>
        /// <param name="dictionary">Dictionary to check and get.</param>
        /// <param name="key">Key to find the value.</param>
        /// <param name="factory">A factory method used to create the value if not found in the dictionary.</param>
        /// <returns>Value if found, default if can not found.</returns>
        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TKey, TValue> factory) where TValue : notnull
        {
            if (dictionary.TryGetValue(key, out var obj))
            {
                return obj;
            }

            return dictionary[key] = factory(key);
        }

        /// <summary>
        /// Gets a value from the dictionary with given key. Returns default value if can not find.
        /// </summary>
        /// <typeparam name="TKey">Type of the key.</typeparam>
        /// <typeparam name="TValue">Type of the value.</typeparam>
        /// <param name="dictionary">Dictionary to check and get.</param>
        /// <param name="key">Key to find the value.</param>
        /// <param name="factory">A factory method used to create the value if not found in the dictionary.</param>
        /// <returns>Value if found, default if can not found.</returns>
        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TValue> factory) where TValue : notnull
        {
            return dictionary.GetOrAdd(key, k => factory());
        }
    }
}
