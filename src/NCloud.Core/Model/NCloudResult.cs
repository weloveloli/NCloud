// -----------------------------------------------------------------------
// <copyright file="NCloudResult.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.Core.Model
{
    using System;

    /// <summary>
    /// Defines the <see cref="NCloudResult" />.
    /// </summary>
    public class NCloudResult
    {
        /// <summary>
        /// Gets or sets a value indicating whether Success.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Gets or sets the Message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the Code.
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// Gets or sets the Data.
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// The Success.
        /// </summary>
        /// <param name="data">The data<see cref="T"/>.</param>
        /// <returns>The <see cref="NCloudResult{T}"/>.</returns>
        public static NCloudResult OK(object data)
        {
            return new NCloudResult
            {
                Data = data,
                Success = true,
                Message = "Success",
                Code = (int)ResultEnum.Success
            };
        }

        /// <summary>
        /// The Success.
        /// </summary>
        /// <param name="@enum">The enum<see cref="ResultEnum"/>.</param>
        /// <param name="message">The message<see cref="string"/>.</param>
        /// <returns>The <see cref="NCloudResult{T}"/>.</returns>
        public static NCloudResult Oops(ResultEnum @enum, string message = null)
        {
            return new NCloudResult
            {
                Data = null,
                Success = false,
                Message = message ?? @enum.ToString(),
                Code = (int)@enum
            };
        }

        /// <summary>
        /// The Oops.
        /// </summary>
        /// <param name="e">The e<see cref="Exception"/>.</param>
        /// <returns>The <see cref="NCloudResult"/>.</returns>
        public static NCloudResult Error(Exception e)
        {
            if (e is NCloudException)
            {
                var ne = ((NCloudException)e);
                if (ne.Error != null)
                {
                    return Oops(ne.Error.Value);
                }
            }

            return Oops(ResultEnum.Server_Error, e.Message);
        }
    }
}
