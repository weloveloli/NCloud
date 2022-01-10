// -----------------------------------------------------------------------
// <copyright file="AliyunDriveApiClient.Preview.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.AliyunDrive.AliyunDriveAPI
{
    using System.Threading.Tasks;
    using NCloud.FileProviders.AliyunDrive.AliyunDriveAPI.Models.Request;
    using NCloud.FileProviders.AliyunDrive.AliyunDriveAPI.Models.Response;
    using NCloud.FileProviders.AliyunDrive.AliyunDriveAPI.Models.Types;

    /// <summary>
    /// Defines the <see cref="AliyunDriveApiClient" />.
    /// </summary>
    public partial class AliyunDriveApiClient
    {
        /// <summary>
        /// The GetVideoPreviewPlayInfoAsync.
        /// </summary>
        /// <param name="driveId">The driveId<see cref="string"/>.</param>
        /// <param name="fileId">The fileId<see cref="string"/>.</param>
        /// <param name="category">The category<see cref="string"/>.</param>
        /// <param name="templateId">The templateId<see cref="VideoPreviewTemplateType"/>.</param>
        /// <returns>The <see cref="Task{VideoPreviewInfoResponse}"/>.</returns>
        public async Task<VideoPreviewInfoResponse> GetVideoPreviewPlayInfoAsync(string driveId, string fileId, string category = "live_transcoding", VideoPreviewTemplateType templateId = VideoPreviewTemplateType.NONE)
            => await GetVideoPreviewPlayInfoAsync(new()
            {
                DriveId = driveId,
                FileId = fileId,
                Category = category,
                TemplateId = templateId
            });

        /// <summary>
        /// The GetVideoPreviewPlayInfoAsync.
        /// </summary>
        /// <param name="request">The request<see cref="VideoPreviewInfoRequest"/>.</param>
        /// <returns>The <see cref="Task{VideoPreviewInfoResponse}"/>.</returns>
        public async Task<VideoPreviewInfoResponse> GetVideoPreviewPlayInfoAsync(VideoPreviewInfoRequest request)
            => await SendJsonPostAsync<VideoPreviewInfoResponse>("v2/file/get_video_preview_play_info", request);

        /// <summary>
        /// The GetAudioPlayInfoAsync.
        /// </summary>
        /// <param name="driveId">The driveId<see cref="string"/>.</param>
        /// <param name="fileId">The fileId<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{AudioPlayInfoResponse}"/>.</returns>
        public async Task<AudioPlayInfoResponse> GetAudioPlayInfoAsync(string driveId, string fileId)
            => await GetAudioPlayInfoAsync(new(driveId, fileId));

        /// <summary>
        /// The GetAudioPlayInfoAsync.
        /// </summary>
        /// <param name="request">The request<see cref="FileBaseRequest"/>.</param>
        /// <returns>The <see cref="Task{AudioPlayInfoResponse}"/>.</returns>
        public async Task<AudioPlayInfoResponse> GetAudioPlayInfoAsync(FileBaseRequest request)
            => await SendJsonPostAsync<AudioPlayInfoResponse>("v2/databox/get_audio_play_info", request);

        /// <summary>
        /// The GetOfficePreviewUrlAsync.
        /// </summary>
        /// <param name="driveId">The driveId<see cref="string"/>.</param>
        /// <param name="fileId">The fileId<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{OfficePreviewUrlResponse}"/>.</returns>
        public async Task<OfficePreviewUrlResponse> GetOfficePreviewUrlAsync(string driveId, string fileId)
            => await GetOfficePreviewUrlAsync(new(driveId, fileId));

        /// <summary>
        /// The GetOfficePreviewUrlAsync.
        /// </summary>
        /// <param name="request">The request<see cref="FileBaseRequest"/>.</param>
        /// <returns>The <see cref="Task{OfficePreviewUrlResponse}"/>.</returns>
        public async Task<OfficePreviewUrlResponse> GetOfficePreviewUrlAsync(FileBaseRequest request)
            => await SendJsonPostAsync<OfficePreviewUrlResponse>("v2/file/get_office_preview_url", request);
    }
}
