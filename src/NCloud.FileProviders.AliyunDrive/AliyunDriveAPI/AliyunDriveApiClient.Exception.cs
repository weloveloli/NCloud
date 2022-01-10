// -----------------------------------------------------------------------
// <copyright file="AliyunDriveApiClient.Exception.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.AliyunDrive.AliyunDriveAPI
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Text.Json.Nodes;
    using System.Threading.Tasks;
    using NCloud.FileProviders.AliyunDrive.AliyunDriveAPI.Exceptions;

    /// <summary>
    /// Defines the <see cref="AliyunDriveApiClient" />.
    /// </summary>
    public partial class AliyunDriveApiClient
    {
        /// <summary>
        /// Defines the _exceptionTypesDic.
        /// </summary>
        private Dictionary<string, Type> _exceptionTypesDic;

        /// <summary>
        /// The TryThrowExceptionAndReadContentAsync.
        /// </summary>
        /// <param name="url">The url<see cref="string"/>.</param>
        /// <param name="resp">The resp<see cref="HttpResponseMessage"/>.</param>
        /// <returns>The <see cref="Task{string}"/>.</returns>
        private async Task<string> TryThrowExceptionAndReadContentAsync(string url, HttpResponseMessage resp)
        {
            if (resp.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return string.Empty;
            }

            var json = await resp.Content.ReadAsStringAsync();
            var obj = JsonNode.Parse(json);
            var code = (string)obj["code"];
            var message = (string)obj["message"];
            if (string.IsNullOrEmpty(code) && string.IsNullOrEmpty(message))
            {
                return json;
            }

            var ex = new APIException(message)
            {
                Url = url,
                Headers = new(resp.Headers.ToDictionary(x => x.Key, x => x.Value.First())),
                StatusCode = resp.StatusCode,
                Code = code,
                ResponseContent = json
            };
            throw GetTypedException(ex);
        }

        /// <summary>
        /// The GetTypedException.
        /// </summary>
        /// <param name="ex">The ex<see cref="APIException"/>.</param>
        /// <returns>The <see cref="APIException"/>.</returns>
        private APIException GetTypedException(APIException ex)
        {
            if (_exceptionTypesDic == null)
            {
                _exceptionTypesDic = new(new KeyValuePair<string, Type>[]
                {
                new ("NotFound.", typeof(NotFoundException)),
                new ("JsonParseException", typeof(JsonParseException)),
                new ("BadRequest", typeof(BadRequestException)),
                new ("InvalidParameter.", typeof(BadRequestException)),
                new ("ForbiddenNoPermission.", typeof(ForbiddenNoPermissionException)),
                new ("AccessTokenInvalid", typeof(AccessTokenInvalidException)),
                new ("InvalidResource.", typeof(InvalidResourceException)),
                new ("AlreadyExist.", typeof(AlreadyExistException))
                });
            }

            if (ex.Code == null)
            {
                return ex;
            }

            var type = _exceptionTypesDic.Where(o => ex.Code.StartsWith(o.Key)).Select(o => o.Value).FirstOrDefault();
            if (type != null)
            {
                var newEx = (APIException)type.Assembly.CreateInstance(type.FullName, true, System.Reflection.BindingFlags.Default, null, new object[] { ex }, null, null);
                return newEx;
            }

            return ex;
        }
    }
}
