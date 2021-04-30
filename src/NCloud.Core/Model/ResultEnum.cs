// -----------------------------------------------------------------------
// <copyright file="ResultEnum.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.Core.Model
{
    /// <summary>
    /// Defines the <see cref="ResultEnum" />.
    /// </summary>
    public enum ResultEnum
    {
        /// <summary>
        /// Defines the Success.
        /// </summary>
        Success = 10000,
        /// <summary>
        /// Defines the Server_Error.
        /// </summary>
        Server_Error = 20000,
        /// <summary>
        /// Defines the Invalid_Path.
        /// </summary>
        Invalid_Path = 10001,
        /// <summary>
        /// Defines the File_Not_Found.
        /// </summary>
        File_Not_Found = 10002,
        /// <summary>
        /// Defines the Path_Unavilable.
        /// </summary>
        Path_Unavilable = 10003,
        /// <summary>
        /// Defines the File_Opt_Failed.
        /// </summary>
        File_Opt_Failed = 10004,
        /// <summary>
        /// Defines the File_Opt_Forbidden.
        /// </summary>
        File_Opt_Forbidden = 10005,
        /// <summary>
        /// Defines the Invalid_Base16_key.
        /// </summary>
        Invalid_Base16_key = 10006,
        /// <summary>
        /// Defines the Path_Unauthorized.
        /// </summary>
        Path_Unauthorized = 10007,
    }
}
