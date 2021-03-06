// -----------------------------------------------------------------------
// <copyright file="StringExtensions.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.Utils
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Extension methods for String class.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Adds a char to end of given string if it does not ends with the char.
        /// </summary>
        /// <param name="str">The str<see cref="string"/>.</param>
        /// <param name="c">The c<see cref="char"/>.</param>
        /// <param name="comparisonType">The comparisonType<see cref="StringComparison"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string EnsureEndsWith(this string str, char c, StringComparison comparisonType = StringComparison.Ordinal)
        {
            CheckNotNull(str, nameof(str));

            if (str.EndsWith(c.ToString(), comparisonType))
            {
                return str;
            }

            return str + c;
        }

        /// <summary>
        /// Adds a char to beginning of given string if it does not starts with the char.
        /// </summary>
        /// <param name="str">The str<see cref="string"/>.</param>
        /// <param name="c">The c<see cref="char"/>.</param>
        /// <param name="comparisonType">The comparisonType<see cref="StringComparison"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string EnsureStartsWith(this string str, char c, StringComparison comparisonType = StringComparison.Ordinal)
        {
            CheckNotNull(str, nameof(str));

            if (str.StartsWith(c.ToString(), comparisonType))
            {
                return str;
            }

            return c + str;
        }

        /// <summary>
        /// Indicates whether this string is null or an System.String.Empty string.
        /// </summary>
        /// <param name="str">The str<see cref="string"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        /// <summary>
        /// indicates whether this string is null, empty, or consists only of white-space characters.
        /// </summary>
        /// <param name="str">The str<see cref="string"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public static bool IsNullOrWhiteSpace(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        /// <summary>
        /// Gets a substring of a string from beginning of the string.
        /// </summary>
        /// <param name="str">The str<see cref="string"/>.</param>
        /// <param name="len">The len<see cref="int"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string Left(this string str, int len)
        {
            CheckNotNull(str, nameof(str));

            if (str.Length < len)
            {
                throw new ArgumentException("len argument can not be bigger than given string's length!");
            }

            return str.Substring(0, len);
        }

        /// <summary>
        /// Converts line endings in the string to <see cref="Environment.NewLine"/>.
        /// </summary>
        /// <param name="str">The str<see cref="string"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string NormalizeLineEndings(this string str)
        {
            return str.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", Environment.NewLine);
        }

        /// <summary>
        /// Gets index of nth occurrence of a char in a string.
        /// </summary>
        /// <param name="str">.</param>
        /// <param name="c">.</param>
        /// <param name="n">Count of the occurrence.</param>
        /// <returns>The <see cref="int"/>.</returns>
        public static int NthIndexOf(this string str, char c, int n)
        {
            CheckNotNull(str, nameof(str));

            var count = 0;
            for (var i = 0; i < str.Length; i++)
            {
                if (str[i] != c)
                {
                    continue;
                }

                if ((++count) == n)
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// Removes first occurrence of the given postfixes from end of the given string.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="postFixes">one or more postfix.</param>
        /// <returns>Modified string or the same string if it has not any of given postfixes.</returns>
        public static string RemovePostFix(this string str, params string[] postFixes)
        {
            return str.RemovePostFix(StringComparison.Ordinal, postFixes);
        }

        /// <summary>
        /// Removes first occurrence of the given postfixes from end of the given string.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="comparisonType">String comparison type.</param>
        /// <param name="postFixes">one or more postfix.</param>
        /// <returns>Modified string or the same string if it has not any of given postfixes.</returns>
        public static string RemovePostFix(this string str, StringComparison comparisonType, params string[] postFixes)
        {
            if (postFixes is null || postFixes.Length == 0)
            {
                return str;
            }

            foreach (var postFix in postFixes)
            {
                if (str.EndsWith(postFix, comparisonType))
                {
                    return str.Left(str.Length - postFix.Length);
                }
            }

            return str;
        }

        /// <summary>
        /// Removes first occurrence of the given prefixes from beginning of the given string.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="preFixes">one or more prefix.</param>
        /// <returns>Modified string or the same string if it has not any of given prefixes.</returns>
        public static string RemovePreFix(this string str, params string[] preFixes)
        {
            return str.RemovePreFix(StringComparison.Ordinal, preFixes);
        }

        /// <summary>
        /// Removes first occurrence of the given prefixes from beginning of the given string.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="comparisonType">String comparison type.</param>
        /// <param name="preFixes">one or more prefix.</param>
        /// <returns>Modified string or the same string if it has not any of given prefixes.</returns>
        public static string RemovePreFix(this string str, StringComparison comparisonType, params string[] preFixes)
        {
            if (preFixes is null || preFixes.Length == 0)
            {
                return str;
            }

            foreach (var preFix in preFixes)
            {
                if (str.StartsWith(preFix, comparisonType))
                {
                    return str.Right(str.Length - preFix.Length);
                }
            }

            return str;
        }

        /// <summary>
        /// The ReplaceFirst.
        /// </summary>
        /// <param name="str">The str<see cref="string"/>.</param>
        /// <param name="search">The search<see cref="string"/>.</param>
        /// <param name="replace">The replace<see cref="string"/>.</param>
        /// <param name="comparisonType">The comparisonType<see cref="StringComparison"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string ReplaceFirst(this string str, string search, string replace, StringComparison comparisonType = StringComparison.Ordinal)
        {
            CheckNotNull(str, nameof(str));

            var pos = str.IndexOf(search, comparisonType);
            if (pos < 0)
            {
                return str;
            }

            return str.Substring(0, pos) + replace + str[(pos + search.Length)..];
        }

        /// <summary>
        /// Gets a substring of a string from end of the string.
        /// </summary>
        /// <param name="str">The str<see cref="string"/>.</param>
        /// <param name="len">The len<see cref="int"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string Right(this string str, int len)
        {
            CheckNotNull(str, nameof(str));

            if (str.Length < len)
            {
                throw new ArgumentException("len argument can not be bigger than given string's length!");
            }

            return str.Substring(str.Length - len, len);
        }

        /// <summary>
        /// Uses string.Split method to split given string by given separator.
        /// </summary>
        /// <param name="str">The str<see cref="string"/>.</param>
        /// <param name="separator">The separator<see cref="string"/>.</param>
        /// <returns>The <see cref="string[]"/>.</returns>
        public static string[] Split(this string str, string separator)
        {
            return str.Split(new[] { separator }, StringSplitOptions.None);
        }

        /// <summary>
        /// Uses string.Split method to split given string by given separator.
        /// </summary>
        /// <param name="str">The str<see cref="string"/>.</param>
        /// <param name="separator">The separator<see cref="string"/>.</param>
        /// <param name="options">The options<see cref="StringSplitOptions"/>.</param>
        /// <returns>The <see cref="string[]"/>.</returns>
        public static string[] Split(this string str, string separator, StringSplitOptions options)
        {
            return str.Split(new[] { separator }, options);
        }

        /// <summary>
        /// Uses string.Split method to split given string by <see cref="Environment.NewLine"/>.
        /// </summary>
        /// <param name="str">The str<see cref="string"/>.</param>
        /// <returns>The <see cref="string[]"/>.</returns>
        public static string[] SplitToLines(this string str)
        {
            return str.Split(Environment.NewLine);
        }

        /// <summary>
        /// Uses string.Split method to split given string by <see cref="Environment.NewLine"/>.
        /// </summary>
        /// <param name="str">The str<see cref="string"/>.</param>
        /// <param name="options">The options<see cref="StringSplitOptions"/>.</param>
        /// <returns>The <see cref="string[]"/>.</returns>
        public static string[] SplitToLines(this string str, StringSplitOptions options)
        {
            return str.Split(Environment.NewLine, options);
        }

        /// <summary>
        /// Converts PascalCase string to camelCase string.
        /// </summary>
        /// <param name="str">String to convert.</param>
        /// <param name="useCurrentCulture">set true to use current culture. Otherwise, invariant culture will be used.</param>
        /// <returns>camelCase of the string.</returns>
        public static string ToCamelCase(this string str, bool useCurrentCulture = false)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return str;
            }

            if (str.Length == 1)
            {
                return useCurrentCulture ? str.ToLower() : str.ToLowerInvariant();
            }

            return (useCurrentCulture ? char.ToLower(str[0]) : char.ToLowerInvariant(str[0])) + str[1..];
        }

        /// <summary>
        /// Converts given PascalCase/camelCase string to sentence (by splitting words by space).
        /// Example: "ThisIsSampleSentence" is converted to "This is a sample sentence".
        /// </summary>
        /// <param name="str">String to convert.</param>
        /// <param name="useCurrentCulture">set true to use current culture. Otherwise, invariant culture will be used.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string ToSentenceCase(this string str, bool useCurrentCulture = false)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return str;
            }

            return useCurrentCulture
                ? Regex.Replace(str, "[a-z][A-Z]", m => m.Value[0] + " " + char.ToLower(m.Value[1]))
                : Regex.Replace(str, "[a-z][A-Z]", m => m.Value[0] + " " + char.ToLowerInvariant(m.Value[1]));
        }

        /// <summary>
        /// Converts given PascalCase/camelCase string to kebab-case.
        /// </summary>
        /// <param name="str">String to convert.</param>
        /// <param name="useCurrentCulture">set true to use current culture. Otherwise, invariant culture will be used.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string ToKebabCase(this string str, bool useCurrentCulture = false)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return str;
            }

            str = str.ToCamelCase();

            return useCurrentCulture
                ? Regex.Replace(str, "[a-z][A-Z]", m => m.Value[0] + "-" + char.ToLower(m.Value[1]))
                : Regex.Replace(str, "[a-z][A-Z]", m => m.Value[0] + "-" + char.ToLowerInvariant(m.Value[1]));
        }

        /// <summary>
        /// Converts given PascalCase/camelCase string to snake case.
        /// Example: "ThisIsSampleSentence" is converted to "this_is_a_sample_sentence".
        /// https://github.com/npgsql/npgsql/blob/dev/src/Npgsql/NameTranslation/NpgsqlSnakeCaseNameTranslator.cs#L51.
        /// </summary>
        /// <param name="str">String to convert.</param>
        /// <returns>.</returns>
        public static string ToSnakeCase(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return str;
            }

            var builder = new StringBuilder(str.Length + Math.Min(2, str.Length / 5));
            var previousCategory = default(UnicodeCategory?);

            for (var currentIndex = 0; currentIndex < str.Length; currentIndex++)
            {
                var currentChar = str[currentIndex];
                if (currentChar == '_')
                {
                    builder.Append('_');
                    previousCategory = null;
                    continue;
                }

                var currentCategory = char.GetUnicodeCategory(currentChar);
                switch (currentCategory)
                {
                    case UnicodeCategory.UppercaseLetter:
                    case UnicodeCategory.TitlecaseLetter:
                        if (previousCategory == UnicodeCategory.SpaceSeparator ||
                            previousCategory == UnicodeCategory.LowercaseLetter ||
                            previousCategory != UnicodeCategory.DecimalDigitNumber &&
                            previousCategory != null &&
                            currentIndex > 0 &&
                            currentIndex + 1 < str.Length &&
                            char.IsLower(str[currentIndex + 1]))
                        {
                            builder.Append('_');
                        }

                        currentChar = char.ToLower(currentChar);
                        break;

                    case UnicodeCategory.LowercaseLetter:
                    case UnicodeCategory.DecimalDigitNumber:
                        if (previousCategory == UnicodeCategory.SpaceSeparator)
                        {
                            builder.Append('_');
                        }
                        break;

                    default:
                        if (previousCategory != null)
                        {
                            previousCategory = UnicodeCategory.SpaceSeparator;
                        }
                        continue;
                }

                builder.Append(currentChar);
                previousCategory = currentCategory;
            }

            return builder.ToString();
        }

        /// <summary>
        /// Converts string to enum value.
        /// </summary>
        /// <typeparam name="T">Type of enum.</typeparam>
        /// <param name="value">String value to convert.</param>
        /// <returns>Returns enum object.</returns>
        public static T ToEnum<T>(this string value)
            where T : struct
        {
            CheckNotNull(value, nameof(value));
            return (T)Enum.Parse(typeof(T), value);
        }

        /// <summary>
        /// Converts string to enum value.
        /// </summary>
        /// <typeparam name="T">Type of enum.</typeparam>
        /// <param name="value">String value to convert.</param>
        /// <param name="ignoreCase">Ignore case.</param>
        /// <returns>Returns enum object.</returns>
        public static T ToEnum<T>(this string value, bool ignoreCase)
            where T : struct
        {
            CheckNotNull(value, nameof(value));
            return (T)Enum.Parse(typeof(T), value, ignoreCase);
        }

        /// <summary>
        /// The ToMd5.
        /// </summary>
        /// <param name="str">The str<see cref="string"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string ToMd5(this string str)
        {
            using var md5 = MD5.Create();
            var inputBytes = Encoding.UTF8.GetBytes(str);
            var hashBytes = md5.ComputeHash(inputBytes);

            var sb = new StringBuilder();
            foreach (var hashByte in hashBytes)
            {
                sb.Append(hashByte.ToString("X2"));
            }

            return sb.ToString();
        }

        /// <summary>
        /// Converts camelCase string to PascalCase string.
        /// </summary>
        /// <param name="str">String to convert.</param>
        /// <param name="useCurrentCulture">set true to use current culture. Otherwise, invariant culture will be used.</param>
        /// <returns>PascalCase of the string.</returns>
        public static string ToPascalCase(this string str, bool useCurrentCulture = false)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return str;
            }

            if (str.Length == 1)
            {
                return useCurrentCulture ? str.ToUpper() : str.ToUpperInvariant();
            }

            return (useCurrentCulture ? char.ToUpper(str[0]) : char.ToUpperInvariant(str[0])) + str[1..];
        }

        /// <summary>
        /// Gets a substring of a string from beginning of the string if it exceeds maximum length.
        /// </summary>
        /// <param name="str">The str<see cref="string"/>.</param>
        /// <param name="maxLength">The maxLength<see cref="int"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string Truncate(this string str, int maxLength)
        {
            return str.Length <= maxLength ? str : str.Left(maxLength);
        }

        /// <summary>
        /// Gets a substring of a string from Ending of the string if it exceeds maximum length.
        /// </summary>
        /// <param name="str">The str<see cref="string"/>.</param>
        /// <param name="maxLength">The maxLength<see cref="int"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string TruncateFromBeginning(this string str, int maxLength)
        {
            return str.Length <= maxLength ? str : str.Right(maxLength);
        }

        /// <summary>
        /// Gets a substring of a string from beginning of the string if it exceeds maximum length.
        /// It adds a "..." postfix to end of the string if it's truncated.
        /// Returning string can not be longer than maxLength.
        /// </summary>
        /// <param name="str">The str<see cref="string"/>.</param>
        /// <param name="maxLength">The maxLength<see cref="int"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string TruncateWithPostfix(this string str, int maxLength)
        {
            return TruncateWithPostfix(str, maxLength, "...");
        }

        /// <summary>
        /// Gets a substring of a string from beginning of the string if it exceeds maximum length.
        /// It adds given <paramref name="postfix"/> to end of the string if it's truncated.
        /// Returning string can not be longer than maxLength.
        /// </summary>
        /// <param name="str">The str<see cref="string"/>.</param>
        /// <param name="maxLength">The maxLength<see cref="int"/>.</param>
        /// <param name="postfix">The postfix<see cref="string"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string TruncateWithPostfix(this string str, int maxLength, string postfix)
        {
            if (str == string.Empty || maxLength == 0)
            {
                return string.Empty;
            }

            if (str.Length <= maxLength)
            {
                return str;
            }

            if (maxLength <= postfix.Length)
            {
                return postfix.Left(maxLength);
            }

            return str.Left(maxLength - postfix.Length) + postfix;
        }

        /// <summary>
        /// Converts given string to a byte array using <see cref="Encoding.UTF8"/> encoding.
        /// </summary>
        /// <param name="str">The str<see cref="string"/>.</param>
        /// <returns>The <see cref="byte[]"/>.</returns>
        public static byte[] GetBytes(this string str)
        {
            return str.GetBytes(Encoding.UTF8);
        }

        /// <summary>
        /// Converts given string to a byte array using the given <paramref name="encoding"/>.
        /// </summary>
        /// <param name="str">The str<see cref="string"/>.</param>
        /// <param name="encoding">The encoding<see cref="Encoding"/>.</param>
        /// <returns>The <see cref="byte[]"/>.</returns>
        public static byte[] GetBytes(this string str, Encoding encoding)
        {
            CheckNotNull(str, nameof(str));
            if (encoding == null)
            {
                throw new ArgumentException($"{nameof(encoding)} can not be null!", nameof(encoding));
            }

            return encoding.GetBytes(str);
        }

        /// <summary>
        /// The CheckNotNull.
        /// </summary>
        /// <param name="value">The value<see cref="string"/>.</param>
        /// <param name="parameterName">The parameterName<see cref="string"/>.</param>
        /// <param name="maxLength">The maxLength<see cref="int"/>.</param>
        /// <param name="minLength">The minLength<see cref="int"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string CheckNotNull(string value, string parameterName, int maxLength = int.MaxValue, int minLength = 0)
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
        /// Decode the str from base64.
        /// </summary>
        /// <param name="encodedString">base64 encoded string.</param>
        /// <returns>.</returns>
        public static string DecodeBase64(this string encodedString)
        {
            if (string.IsNullOrEmpty(encodedString))
            {
                return string.Empty;
            }
            var bytes = Convert.FromBase64String(encodedString);

            var decodedString = Encoding.UTF8.GetString(bytes);

            return decodedString;
        }

        /// <summary>
        /// Encode the str to base64.
        /// </summary>
        /// <param name="originalString">The originalString<see cref="string"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string EncodeBase64(this string originalString)
        {
            if (string.IsNullOrEmpty(originalString))
            {
                return string.Empty;
            }
            var bytes = Encoding.UTF8.GetBytes(originalString);
            var encodedString = Convert.ToBase64String(bytes);
            return encodedString;
        }

        /// <summary>
        /// The FromPosixPath.
        /// </summary>
        /// <param name="path">The path<see cref="string"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public static string FromPosixPath(this string path)
        {
            bool relative = false;
            if (path.StartsWith("./"))
            {
                relative = true;
                path = path.Substring(2);
            }
            if (IsWindows())
            {
                path = Regex.Replace(path, @"^\/([^\/]+?)", @"$1:\\");
                path = Regex.Replace(path, @"\/", "\\");
                path = Regex.Replace(path, @"(?<!\:)\\+$", "");
                path = Regex.Replace(path, @"\\{2,}", "\\");
            }
            if (relative)
            {
                path = Path.Combine(System.Environment.CurrentDirectory, path);
            }
            return path;
        }

        /// <summary>
        /// The ToPosixPath.
        /// </summary>
        /// <param name="path">The path<see cref="string"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public static string ToPosixPath(this string path)
        {
            if (IsWindows())
            {
                path = path.Split(Path.DirectorySeparatorChar).Aggregate((a, b) => a + "/" + b);
                path = Regex.Replace(path, @"^([a-zA-Z])\:", "/$1");
            }
            return path;
        }

        /// <summary>
        /// The IsSubpathOf.
        /// </summary>
        /// <param name="path1">The path1<see cref="string"/>.</param>
        /// <param name="path2">The path2<see cref="string"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public static bool IsSubpathOf(this string path1, string path2)
        {
            if (path1 == null || path2 == null)
            {
                return false;
            }
            path1 = path1.EnsureStartsWith('/');
            path2 = path2.EnsureStartsWith('/');
            if (path1 == path2 || !path1.StartsWith(path2))
            {
                return false;
            }
            if(path2 == "/")
            {
                return path1.LastIndexOf("/") == 0;
            }
            var paths1 = path1.Split("/");
            var paths2 = path2.Split("/");
            if (paths1.Length != paths2.Length + 1)
            {
                return false;
            }

            for(var i = 0; i < paths2.Length; i++)
            {
                if (paths1[i] != paths2[i])
                {
                    return false;
                }
            }
            return true;
        }

        public static bool IsWindows()
        {
            return Environment.OSVersion.Platform == PlatformID.Win32NT;
        }
    }
}
