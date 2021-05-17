// -----------------------------------------------------------------------
// <copyright file="Check.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.Utils
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Defines the <see cref="Check" />.
    /// </summary>
    [System.Diagnostics.DebuggerStepThrough]
    public static class Check
    {
        /// <summary>
        /// The NotNull.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="value">The value<see cref="T"/>.</param>
        /// <param name="parameterName">The parameterName<see cref="string"/>.</param>
        /// <returns>The <see cref="T"/>.</returns>
        public static T NotNull<T>(T value, string parameterName)
        {
            if (value == null)
            {
                throw new ArgumentNullException(parameterName);
            }

            return value;
        }

        /// <summary>
        /// The NotNull.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="value">The value<see cref="T"/>.</param>
        /// <param name="parameterName">The parameterName<see cref="string"/>.</param>
        /// <param name="message">The message<see cref="string"/>.</param>
        /// <returns>The <see cref="T"/>.</returns>
        public static T NotNull<T>(T value, string parameterName, string message)
        {
            if (value == null)
            {
                throw new ArgumentNullException(parameterName, message);
            }

            return value;
        }

        /// <summary>
        /// The NotNull.
        /// </summary>
        /// <param name="value">The value<see cref="string"/>.</param>
        /// <param name="parameterName">The parameterName<see cref="string"/>.</param>
        /// <param name="maxLength">The maxLength<see cref="int"/>.</param>
        /// <param name="minLength">The minLength<see cref="int"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string NotNull(string value, string parameterName, int maxLength = int.MaxValue, int minLength = 0)
        {
            if (value == null)
            {
                throw new ArgumentException($"{parameterName} can not be null!", parameterName);
            }

            if (value.Length > maxLength)
            {
                throw new ArgumentException($"{parameterName} length must be equal to or lower than {maxLength}!",
                    parameterName);
            }

            if (minLength > 0 && value.Length < minLength)
            {
                throw new ArgumentException($"{parameterName} length must be equal to or bigger than {minLength}!",
                    parameterName);
            }

            return value;
        }

        /// <summary>
        /// The NotNullOrWhiteSpace.
        /// </summary>
        /// <param name="value">The value<see cref="string?"/>.</param>
        /// <param name="parameterName">The parameterName<see cref="string"/>.</param>
        /// <param name="maxLength">The maxLength<see cref="int"/>.</param>
        /// <param name="minLength">The minLength<see cref="int"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string NotNullOrWhiteSpace(string value, string parameterName, int maxLength = int.MaxValue, int minLength = 0)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException($"{parameterName} can not be null, empty or white space!",
                    parameterName);
            }

            if (value.Length > maxLength)
            {
                throw new ArgumentException($"{parameterName} length must be equal to or lower than {maxLength}!",
                    parameterName);
            }

            if (minLength > 0 && value.Length < minLength)
            {
                throw new ArgumentException($"{parameterName} length must be equal to or bigger than {minLength}!",
                    parameterName);
            }

            return value;
        }

        /// <summary>
        /// The NotNullOrEmpty.
        /// </summary>
        /// <param name="value">The value<see cref="string"/>.</param>
        /// <param name="parameterName">The parameterName<see cref="string"/>.</param>
        /// <param name="maxLength">The maxLength<see cref="int"/>.</param>
        /// <param name="minLength">The minLength<see cref="int"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string NotNullOrEmpty(string value, string parameterName, int maxLength = int.MaxValue, int minLength = 0)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException($"{parameterName} can not be null or empty!", parameterName);
            }

            if (value.Length > maxLength)
            {
                throw new ArgumentException($"{parameterName} length must be equal to or lower than {maxLength}!",
                    parameterName);
            }

            if (minLength > 0 && value.Length < minLength)
            {
                throw new ArgumentException($"{parameterName} length must be equal to or bigger than {minLength}!",
                    parameterName);
            }

            return value;
        }

        /// <summary>
        /// The NotNullOrEmpty.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="value">The value<see cref="ICollection{T}"/>.</param>
        /// <param name="parameterName">The parameterName<see cref="string"/>.</param>
        /// <returns>The <see cref="ICollection{T}"/>.</returns>
        public static ICollection<T> NotNullOrEmpty<T>(ICollection<T> value, string parameterName)
        {
            if (value is null || value.Count == 0)
            {
                throw new ArgumentException(parameterName + " can not be null or empty!", parameterName);
            }

            return value;
        }

        /// <summary>
        /// The AssignableTo.
        /// </summary>
        /// <typeparam name="TBaseType">.</typeparam>
        /// <param name="type">The type<see cref="Type"/>.</param>
        /// <param name="parameterName">The parameterName<see cref="string"/>.</param>
        /// <returns>The <see cref="Type"/>.</returns>
        public static Type AssignableTo<TBaseType>(Type type, string parameterName)
        {
            NotNull(type, parameterName);

            if (typeof(TBaseType).IsAssignableFrom(type))
            {
                throw new ArgumentException(
                    $"{parameterName} (type of {type.AssemblyQualifiedName}) should be assignable to the {typeof(TBaseType).AssemblyQualifiedName}.");
            }

            return type;
        }

        /// <summary>
        /// The AssignableFrom.
        /// </summary>
        /// <typeparam name="TType">.</typeparam>
        /// <param name="type">The type<see cref="Type"/>.</param>
        /// <param name="parameterName">The parameterName<see cref="string"/>.</param>
        /// <returns>The <see cref="Type"/>.</returns>
        public static Type AssignableFrom<TType>(Type type, string parameterName)
        {
            NotNull(type, parameterName);

            if (type.IsAssignableFrom(typeof(TType)))
            {
                throw new ArgumentException(
                    $"{parameterName} (type of {type.AssemblyQualifiedName}) should be assignable from the {typeof(TType).FullName}.");
            }

            return type;
        }

        /// <summary>
        /// The Length.
        /// </summary>
        /// <param name="value">The value<see cref="string?"/>.</param>
        /// <param name="parameterName">The parameterName<see cref="string"/>.</param>
        /// <param name="maxLength">The maxLength<see cref="int"/>.</param>
        /// <param name="minLength">The minLength<see cref="int"/>.</param>
        /// <returns>The <see cref="string?"/>.</returns>
        public static string Length(string value, string parameterName, int maxLength, int minLength = 0)
        {
            if (minLength > 0)
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException(parameterName + " can not be null or empty!", parameterName);
                }

                if (value.Length < minLength)
                {
                    throw new ArgumentException(
                        $"{parameterName} length must be equal to or bigger than {minLength}!", parameterName);
                }
            }

            if (value != null && value.Length > maxLength)
            {
                throw new ArgumentException($"{parameterName} length must be equal to or lower than {maxLength}!",
                    parameterName);
            }

            return value;
        }

        /// <summary>
        /// The CheckParameter.
        /// </summary>
        /// <param name="startPoint">The startPoint<see cref="long"/>.</param>
        /// <param name="endPoint">The endPoint<see cref="long?"/>.</param>
        /// <param name="length">The length<see cref="long?"/>.</param>
        public static void CheckIndex(long startPoint, long? endPoint, long? length)
        {
            if (startPoint < 0)
            {
                throw new ArgumentException($"{nameof(startPoint)} is invalid, must be large than or equal to 0!");
            }
            NotNull(length, nameof(length));
            var maxSize = (long)length;
            var right = endPoint ?? maxSize;
            if (right > maxSize)
            {
                throw new ArgumentException($"{nameof(endPoint)} is invalid, must be less than or equal to {maxSize}!");
            }
            if (right < startPoint)
            {
                throw new ArgumentException($"{nameof(endPoint)} is invalid, must be less than or equal to {maxSize}!");
            }
        }
    }
}
