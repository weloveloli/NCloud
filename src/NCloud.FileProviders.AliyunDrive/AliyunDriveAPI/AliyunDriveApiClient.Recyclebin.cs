// -----------------------------------------------------------------------
// <copyright file="AliyunDriveApiClient.Recyclebin.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.AliyunDrive.AliyunDriveAPI
{
    using System.Text.Json.Nodes;
    using System.Threading.Tasks;
    using NCloud.FileProviders.AliyunDrive.AliyunDriveAPI.Models.Request;
    using NCloud.FileProviders.AliyunDrive.AliyunDriveAPI.Models.Response;

    /// <summary>
    /// Defines the <see cref="AliyunDriveApiClient" />.
    /// </summary>
    public partial class AliyunDriveApiClient
    {
        /// <summary>
        /// The RecyclebinListAsync.
        /// </summary>
        /// <param name="request">The request<see cref="RecyclebinListRequest"/>.</param>
        /// <returns>The <see cref="Task{FileListResponse}"/>.</returns>
        public async Task<FileListResponse> RecyclebinListAsync(RecyclebinListRequest request)
           => await SendJsonPostAsync<FileListResponse>("v2/recyclebin/list", request);

        /// <summary>
        /// The MoveToRecyclebin.
        /// </summary>
        /// <param name="driveId">The driveId<see cref="string"/>.</param>
        /// <param name="fileId">The fileId<see cref="string"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task MoveToRecyclebin(string driveId, string fileId)
            => await MoveToRecyclebin(new() { DriveId = driveId, FileId = fileId });

        /// <summary>
        /// The MoveToRecyclebin.
        /// </summary>
        /// <param name="request">The request<see cref="FileBaseRequest"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task MoveToRecyclebin(FileBaseRequest request)
            => await SendJsonPostAsync("v2/recyclebin/trash", request);

        /// <summary>
        /// The RestoreFromRecyclebin.
        /// </summary>
        /// <param name="driveId">The driveId<see cref="string"/>.</param>
        /// <param name="fileId">The fileId<see cref="string"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task RestoreFromRecyclebin(string driveId, string fileId)
           => await RestoreFromRecyclebin(new() { DriveId = driveId, FileId = fileId });

        /// <summary>
        /// The RestoreFromRecyclebin.
        /// </summary>
        /// <param name="request">The request<see cref="FileBaseRequest"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task RestoreFromRecyclebin(FileBaseRequest request)
            => await SendJsonPostAsync("v2/recyclebin/restore", request);

        /// <summary>
        /// The ClearRecyclebinAsync.
        /// </summary>
        /// <param name="driveId">The driveId<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{ClearRecyclebinResponse}"/>.</returns>
        public async Task<ClearRecyclebinResponse> ClearRecyclebinAsync(string driveId)
            => await SendJsonPostAsync<ClearRecyclebinResponse>("v2/recyclebin/clear", new JsonObject { ["drive_id"] = driveId });
    }
}
