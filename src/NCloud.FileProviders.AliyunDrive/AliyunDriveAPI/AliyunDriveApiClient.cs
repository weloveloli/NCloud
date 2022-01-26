// -----------------------------------------------------------------------
// <copyright file="AliyunDriveApiClient.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.AliyunDrive.AliyunDriveAPI
{
    using System;
    using System.Net.Http;
    using System.Text.Encodings.Web;
    using System.Text.Json;
    using System.Text.Json.Nodes;
    using System.Text.Json.Serialization;
    using System.Threading.Tasks;
    using NCloud.FileProviders.AliyunDrive.AliyunDriveAPI.Models.Converters;
    using NCloud.FileProviders.AliyunDrive.AliyunDriveAPI.Models.Response;

    /// <summary>
    /// Defines the <see cref="AliyunDriveApiClient" />.
    /// </summary>
    public partial class AliyunDriveApiClient
    {
        /// <summary>
        /// Defines the _refreshToken.
        /// </summary>
        private string _refreshToken;

        /// <summary>
        /// Defines the _token.
        /// </summary>
        private string _token;

        /// <summary>
        /// Defines the _tokenExpireTime.
        /// </summary>
        private DateTime? _tokenExpireTime;

        /// <summary>
        /// Defines the _httpClient.
        /// </summary>
        private readonly HttpClient _httpClient;


        private readonly Action<RefreshTokenResponse> refreshTokenAction;

        /// <summary>
        /// Gets the JsonSerializerOptions.
        /// </summary>
        public static JsonSerializerOptions JsonSerializerOptions => new()
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNamingPolicy = new SnakeCaseNamingPolicy(),
            Converters =
                {
                new Models.Converters.JsonStringEnumConverter(),
                new JsonNodeConverter(),
                new TimeSpanSecondConverter(),
                new NullableTimeSpanSecondConverter(),
                new DatetimeConverter(),
                }
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="AliyunDriveApiClient"/> class.
        /// </summary>
        /// <param name="refreshToken">The refreshToken<see cref="string"/>.</param>
        public AliyunDriveApiClient(string refreshToken)
        {
            _refreshToken = refreshToken;
            _httpClient = new() { BaseAddress = new Uri("https://api.aliyundrive.com/") };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AliyunDriveApiClient"/> class.
        /// </summary>
        /// <param name="refreshToken">The refreshToken<see cref="string"/>.</param>
        /// <param name="expiredTime">The expiredTime<see cref="DateTime"/>.</param>
        public AliyunDriveApiClient(string refreshToken, DateTime dateTime, Action<RefreshTokenResponse> refreshTokenAction)
        {
            _refreshToken = refreshToken;
            _httpClient = new() { BaseAddress = new Uri("https://api.aliyundrive.com/") };
            this.refreshTokenAction = refreshTokenAction;
        }

        /// <summary>
        /// The RefreshTokenAsync.
        /// </summary>
        /// <returns>The <see cref="Task{RefreshTokenResponse}"/>.</returns>
        public async Task<RefreshTokenResponse> RefreshTokenAsync()
        {
            var obj = new JsonObject
            {
                ["refresh_token"] = _refreshToken
            };
            var res = await SendJsonPostAsync<RefreshTokenResponse>("https://websv.aliyundrive.com/token/refresh", obj, false);
            _token = res.AccessToken;
            _refreshToken = res.RefreshToken;
            _tokenExpireTime = res.ExpireTime;
            if (refreshTokenAction != null)
            {
                this.refreshTokenAction.Invoke(res);
            }
            if (_httpClient.DefaultRequestHeaders.Contains("Authorization"))
            {
                _httpClient.DefaultRequestHeaders.Remove("Authorization");
            }
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _token);
            return res;
        }

        /// <summary>
        /// The IsTokenExpire.
        /// </summary>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool IsTokenExpire()
            => _tokenExpireTime == null || _tokenExpireTime.Value > DateTime.UtcNow;

        /// <summary>
        /// The PrepareTokenAsync.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task<string> PrepareTokenAsync()
        {
            if (!IsTokenExpire())
            {
                return _token;
            }
            await RefreshTokenAsync();
            return _token;
        }
    }
}
