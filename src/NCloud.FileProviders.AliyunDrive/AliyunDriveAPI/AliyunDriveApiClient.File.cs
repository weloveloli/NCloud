// -----------------------------------------------------------------------
// <copyright file="AliyunDriveApiClient.File.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.AliyunDrive.AliyunDriveAPI
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using NCloud.FileProviders.AliyunDrive.AliyunDriveAPI.Models;
    using NCloud.FileProviders.AliyunDrive.AliyunDriveAPI.Models.Request;
    using NCloud.FileProviders.AliyunDrive.AliyunDriveAPI.Models.Response;
    using NCloud.FileProviders.AliyunDrive.AliyunDriveAPI.Models.Types;

    /// <summary>
    /// Defines the <see cref="AliyunDriveApiClient" />.
    /// </summary>
    public partial class AliyunDriveApiClient
    {
        /// <summary>
        /// The FileGetAsync.
        /// </summary>
        /// <param name="driveId">The driveId<see cref="string"/>.</param>
        /// <param name="fileId">The fileId<see cref="string"/>.</param>
        /// <param name="limit">The limit<see cref="int?"/>.</param>
        /// <param name="urlExpireSec">The urlExpireSec<see cref="int?"/>.</param>
        /// <returns>The <see cref="Task{FileGetResponse}"/>.</returns>
        public async Task<FileGetResponse> FileGetAsync(string driveId, string fileId, int? limit = 100, int? urlExpireSec = 1600)
            => await FileGetAsync(new() { DriveId = driveId, FileId = fileId, UrlExpireSec = urlExpireSec });

        /// <summary>
        /// The FileListAsync.
        /// </summary>
        /// <param name="request">The request<see cref="FileListRequest"/>.</param>
        /// <returns>The <see cref="Task{FileListResponse}"/>.</returns>
        public async Task<FileListResponse> FileListAsync(FileListRequest request)
            => await SendJsonPostAsync<FileListResponse>("adrive/v3/file/list", request);

        /// <summary>
        /// The FileSearchAsync.
        /// </summary>
        /// <param name="driveId">The driveId<see cref="string"/>.</param>
        /// <param name="sampleFileSearchQuery">The sampleFileSearchQuery<see cref="SampleFileSearchQuery"/>.</param>
        /// <param name="limit">The limit<see cref="int?"/>.</param>
        /// <param name="nextMarker">The nextMarker<see cref="string"/>.</param>
        /// <param name="orderBy">The orderBy<see cref="OrderByType?"/>.</param>
        /// <param name="orderDirection">The orderDirection<see cref="OrderDirectionType?"/>.</param>
        /// <returns>The <see cref="Task{FileListResponse}"/>.</returns>
        public async Task<FileListResponse> FileSearchAsync(string driveId, SampleFileSearchQuery sampleFileSearchQuery, int? limit = 100, string nextMarker = null, OrderByType? orderBy = OrderByType.UpdatedAt, OrderDirectionType? orderDirection = OrderDirectionType.DESC)
            => await FileSearchAsync(driveId, sampleFileSearchQuery.GetQueryExpression(), limit, nextMarker, orderBy, orderDirection);

        /// <summary>
        /// The FileSearchAsync.
        /// </summary>
        /// <param name="driveId">The driveId<see cref="string"/>.</param>
        /// <param name="queryExpression">The queryExpression<see cref="FileSearchQueryExpression"/>.</param>
        /// <param name="limit">The limit<see cref="int?"/>.</param>
        /// <param name="nextMarker">The nextMarker<see cref="string"/>.</param>
        /// <param name="orderBy">The orderBy<see cref="OrderByType?"/>.</param>
        /// <param name="orderDirection">The orderDirection<see cref="OrderDirectionType?"/>.</param>
        /// <returns>The <see cref="Task{FileListResponse}"/>.</returns>
        public async Task<FileListResponse> FileSearchAsync(string driveId, FileSearchQueryExpression queryExpression, int? limit = 100, string nextMarker = null, OrderByType? orderBy = OrderByType.UpdatedAt, OrderDirectionType? orderDirection = OrderDirectionType.DESC)
            => await FileSearchAsync(driveId, queryExpression.ToString(), limit, nextMarker, orderBy, orderDirection);

        /// <summary>
        /// The FileSearchAsync.
        /// </summary>
        /// <param name="driveId">The driveId<see cref="string"/>.</param>
        /// <param name="query">The query<see cref="string"/>.</param>
        /// <param name="limit">The limit<see cref="int?"/>.</param>
        /// <param name="nextMarker">The nextMarker<see cref="string"/>.</param>
        /// <param name="orderBy">The orderBy<see cref="OrderByType?"/>.</param>
        /// <param name="orderDirection">The orderDirection<see cref="OrderDirectionType?"/>.</param>
        /// <returns>The <see cref="Task{FileListResponse}"/>.</returns>
        public async Task<FileListResponse> FileSearchAsync(string driveId, string query, int? limit = 100, string nextMarker = null, OrderByType? orderBy = OrderByType.UpdatedAt, OrderDirectionType? orderDirection = OrderDirectionType.DESC)
            => await FileSearchAsync(new() { DriveId = driveId, Query = query, Limit = limit, Marker = nextMarker, OrderByType = orderBy, OrderDirection = orderDirection });

        /// <summary>
        /// The FileSearchAsync.
        /// </summary>
        /// <param name="request">The request<see cref="FileSearchRequest"/>.</param>
        /// <returns>The <see cref="Task{FileListResponse}"/>.</returns>
        public async Task<FileListResponse> FileSearchAsync(FileSearchRequest request)
            => await SendJsonPostAsync<FileListResponse>("adrive/v3/file/search", request);

        /// <summary>
        /// The FileGetAsync.
        /// </summary>
        /// <param name="request">The request<see cref="FileGetRequest"/>.</param>
        /// <returns>The <see cref="Task{FileGetResponse}"/>.</returns>
        public async Task<FileGetResponse> FileGetAsync(FileGetRequest request)
            => await SendJsonPostAsync<FileGetResponse>("v2/file/get", request);

        /// <summary>
        /// The CreateFolderAsync.
        /// </summary>
        /// <param name="driveId">The driveId<see cref="string"/>.</param>
        /// <param name="name">The name<see cref="string"/>.</param>
        /// <param name="parentFileId">The parentFileId<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{CreateFolderResponse}"/>.</returns>
        public async Task<CreateFolderResponse> CreateFolderAsync(string driveId, string name, string parentFileId)
            => await CreateFolderAsync(new() { DriveId = driveId, Name = name, ParentFileId = parentFileId });

        /// <summary>
        /// The CreateFolderAsync.
        /// </summary>
        /// <param name="request">The request<see cref="CreateFolderRequest"/>.</param>
        /// <returns>The <see cref="Task{CreateFolderResponse}"/>.</returns>
        public async Task<CreateFolderResponse> CreateFolderAsync(CreateFolderRequest request)
            => await SendJsonPostAsync<CreateFolderResponse>("adrive/v2/file/createWithFolders", request);

        /// <summary>
        /// The UploadFileAsync.
        /// </summary>
        /// <param name="driveId">The driveId<see cref="string"/>.</param>
        /// <param name="fileName">The fileName<see cref="string"/>.</param>
        /// <param name="bytes">The bytes<see cref="byte[]"/>.</param>
        /// <param name="parentFileId">The parentFileId<see cref="string"/>.</param>
        /// <param name="mode">The mode<see cref="CheckNameModeType"/>.</param>
        /// <param name="chunkSize">The chunkSize<see cref="int"/>.</param>
        /// <returns>The <see cref="Task{FileGetResponse}"/>.</returns>
        public async Task<FileGetResponse> UploadFileAsync(string driveId, string fileName, byte[] bytes, string parentFileId, CheckNameModeType mode, int chunkSize = 1024 * 1024 * 10)
        {
            if (driveId == null)
                throw new ArgumentNullException(nameof(driveId));
            if (fileName == null)
                throw new ArgumentNullException(nameof(fileName));
            if (bytes == null)
                throw new ArgumentNullException(nameof(bytes));
            if (parentFileId == null)
                throw new ArgumentNullException(nameof(parentFileId));
            int chunkCount = (int)Math.Ceiling((double)bytes.Length / chunkSize);
            var preUploadResp = await PreUploadAsync(driveId, fileName, parentFileId, mode, chunkCount);
            for (int i = 0; i < preUploadResp.PartInfoList.Length; i++)
            {
                var partInfo = preUploadResp.PartInfoList[i];
                var partBytes = bytes.Skip(i * chunkSize).Take(chunkSize).ToArray();
                var isOk = await UploadPart(partInfo, partBytes);
                if (!isOk)
                    throw new Exception("上传失败");
            }
            return await CompleteUploadAsync(driveId, preUploadResp.FileId, preUploadResp.UploadId);
        }

        /// <summary>
        /// The UploadFileAsync.
        /// </summary>
        /// <param name="driveId">The driveId<see cref="string"/>.</param>
        /// <param name="fileName">The fileName<see cref="string"/>.</param>
        /// <param name="stream">The stream<see cref="Stream"/>.</param>
        /// <param name="parentFileId">The parentFileId<see cref="string"/>.</param>
        /// <param name="mode">The mode<see cref="CheckNameModeType"/>.</param>
        /// <param name="chunkSize">The chunkSize<see cref="int"/>.</param>
        /// <returns>The <see cref="Task{FileGetResponse}"/>.</returns>
        public async Task<FileGetResponse> UploadFileAsync(string driveId, string fileName, Stream stream, string parentFileId, CheckNameModeType mode, int chunkSize = 1024 * 1024 * 10)
        {
            if (driveId == null)
                throw new ArgumentNullException(nameof(driveId));
            if (fileName == null)
                throw new ArgumentNullException(nameof(fileName));
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));
            if (parentFileId == null)
                throw new ArgumentNullException(nameof(parentFileId));
            int chunkCount = (int)Math.Ceiling((double)stream.Length / chunkSize);
            var preUploadResp = await PreUploadAsync(driveId, fileName, parentFileId, mode, chunkCount);
            var buffer = new byte[chunkSize];
            long remain = stream.Length;
            for (int i = 0; i < preUploadResp.PartInfoList.Length; i++)
            {
                Debug.WriteLine($"正在上传：{i + 1}/{preUploadResp.PartInfoList.Length}");
                var partInfo = preUploadResp.PartInfoList[i];
                stream.Read(buffer, 0, chunkSize);
                var isOk = await UploadPart(partInfo, remain > chunkSize ? buffer : buffer.Take((int)remain).ToArray());
                if (!isOk)
                    throw new Exception("上传失败");
                remain -= chunkSize;
            }
            return await CompleteUploadAsync(driveId, preUploadResp.FileId, preUploadResp.UploadId);
        }

        /// <summary>
        /// The PreUploadAsync.
        /// </summary>
        /// <param name="driveId">The driveId<see cref="string"/>.</param>
        /// <param name="name">The name<see cref="string"/>.</param>
        /// <param name="parentFileId">The parentFileId<see cref="string"/>.</param>
        /// <param name="mode">The mode<see cref="CheckNameModeType"/>.</param>
        /// <param name="partNumber">The partNumber<see cref="int"/>.</param>
        /// <param name="hash">The hash<see cref="string"/>.</param>
        /// <param name="hashName">The hashName<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{PreUploadResponse}"/>.</returns>
        public async Task<PreUploadResponse> PreUploadAsync(string driveId, string name, string parentFileId, CheckNameModeType mode, int partNumber = 1, string hash = null, string hashName = "sha1")
        {
            var partInfos = new List<FileUploadPartInfo>();
            for (int i = 1; i <= partNumber; i++)
                partInfos.Add(new FileUploadPartInfo(i));
            return await PreUploadAsync(new()
            {
                DriveId = driveId,
                Name = name,
                ParentFileId = parentFileId,
                CheckNameMode = mode,
                PartInfoList = partInfos.ToArray(),
                ContentHash = hash,
                ContentHashName = hashName
            });
        }

        /// <summary>
        /// The PreUploadAsync.
        /// </summary>
        /// <param name="request">The request<see cref="PreUploadRequest"/>.</param>
        /// <returns>The <see cref="Task{PreUploadResponse}"/>.</returns>
        public async Task<PreUploadResponse> PreUploadAsync(PreUploadRequest request)
            => await SendJsonPostAsync<PreUploadResponse>("adrive/v2/file/createWithFolders", request);

        /// <summary>
        /// The UploadPart.
        /// </summary>
        /// <param name="partInfo">The partInfo<see cref="FileUploadPartInfoWithUrl"/>.</param>
        /// <param name="stream">The stream<see cref="Stream"/>.</param>
        /// <param name="useInternalUrl">The useInternalUrl<see cref="bool"/>.</param>
        /// <returns>The <see cref="Task{bool}"/>.</returns>
        public async Task<bool> UploadPart(FileUploadPartInfoWithUrl partInfo, Stream stream, bool useInternalUrl = false)
        {
            if (partInfo == null)
                throw new ArgumentNullException(nameof(partInfo));
            var content = new StreamContent(stream);
            var resp = await _httpClient.PutAsync(useInternalUrl ? partInfo.InternalUploadUrl : partInfo.UploadUrl, content);
            if (resp.IsSuccessStatusCode)
                return true;
            return false;
        }

        /// <summary>
        /// The UploadPart.
        /// </summary>
        /// <param name="partInfo">The partInfo<see cref="FileUploadPartInfoWithUrl"/>.</param>
        /// <param name="bytes">The bytes<see cref="byte[]"/>.</param>
        /// <param name="useInternalUrl">The useInternalUrl<see cref="bool"/>.</param>
        /// <returns>The <see cref="Task{bool}"/>.</returns>
        public async Task<bool> UploadPart(FileUploadPartInfoWithUrl partInfo, byte[] bytes, bool useInternalUrl = false)
        {
            if (partInfo == null)
                throw new ArgumentNullException(nameof(partInfo));
            var content = new ByteArrayContent(bytes);
            var resp = await _httpClient.PutAsync(useInternalUrl ? partInfo.InternalUploadUrl : partInfo.UploadUrl, content);
            if (resp.IsSuccessStatusCode)
                return true;
            return false;
        }

        /// <summary>
        /// The CompleteUploadAsync.
        /// </summary>
        /// <param name="driveId">The driveId<see cref="string"/>.</param>
        /// <param name="fileId">The fileId<see cref="string"/>.</param>
        /// <param name="uploadId">The uploadId<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{FileGetResponse}"/>.</returns>
        public async Task<FileGetResponse> CompleteUploadAsync(string driveId, string fileId, string uploadId)
            => await CompleteUploadAsync(new() { DriveId = driveId, FileId = fileId, UploadId = uploadId });

        /// <summary>
        /// The CompleteUploadAsync.
        /// </summary>
        /// <param name="request">The request<see cref="CompleteUploadRequest"/>.</param>
        /// <returns>The <see cref="Task{FileGetResponse}"/>.</returns>
        public async Task<FileGetResponse> CompleteUploadAsync(CompleteUploadRequest request)
            => await SendJsonPostAsync<FileGetResponse>("v2/file/complete", request);

        /// <summary>
        /// The GetDownloadUrlAsync.
        /// </summary>
        /// <param name="driveId">The driveId<see cref="string"/>.</param>
        /// <param name="fileId">The fileId<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{DownloadUrlResponse}"/>.</returns>
        public async Task<DownloadUrlResponse> GetDownloadUrlAsync(string driveId, string fileId)
            => await GetDownloadUrlAsync(new() { DriveId = driveId, FileId = fileId });

        /// <summary>
        /// The GetDownloadUrlAsync.
        /// </summary>
        /// <param name="request">The request<see cref="FileBaseRequest"/>.</param>
        /// <returns>The <see cref="Task{DownloadUrlResponse}"/>.</returns>
        public async Task<DownloadUrlResponse> GetDownloadUrlAsync(FileBaseRequest request)
            => await SendJsonPostAsync<DownloadUrlResponse>("v2/file/get_download_url", request);

        /// <summary>
        /// The DeleteFileAsync.
        /// </summary>
        /// <param name="driveId">The driveId<see cref="string"/>.</param>
        /// <param name="fileId">The fileId<see cref="string"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task DeleteFileAsync(string driveId, string fileId)
            => await DeleteFileAsync(new() { DriveId = driveId, FileId = fileId });

        /// <summary>
        /// The DeleteFileAsync.
        /// </summary>
        /// <param name="request">The request<see cref="FileBaseRequest"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task DeleteFileAsync(FileBaseRequest request)
            => await SendJsonPostAsync("v3/file/delete", request);

        /// <summary>
        /// The FileRenameAsync.
        /// </summary>
        /// <param name="driveId">The driveId<see cref="string"/>.</param>
        /// <param name="fileId">The fileId<see cref="string"/>.</param>
        /// <param name="name">The name<see cref="string"/>.</param>
        /// <param name="mode">The mode<see cref="CheckNameModeType"/>.</param>
        /// <returns>The <see cref="Task{FileItem}"/>.</returns>
        public async Task<FileItem> FileRenameAsync(string driveId, string fileId, string name, CheckNameModeType mode = CheckNameModeType.Refuse)
            => await FileRenameAsync(new() { DriveId = driveId, FileId = fileId, Name = name, CheckNameMode = mode });

        /// <summary>
        /// The FileRenameAsync.
        /// </summary>
        /// <param name="request">The request<see cref="FileRenameRequest"/>.</param>
        /// <returns>The <see cref="Task{FileItem}"/>.</returns>
        public async Task<FileItem> FileRenameAsync(FileRenameRequest request)
            => await SendJsonPostAsync<FileItem>("v3/file/update", request);

        /// <summary>
        /// The FileMoveAsync.
        /// </summary>
        /// <param name="driveId">The driveId<see cref="string"/>.</param>
        /// <param name="fileId">The fileId<see cref="string"/>.</param>
        /// <param name="toDriveId">The toDriveId<see cref="string"/>.</param>
        /// <param name="toParentFileId">The toParentFileId<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{FileItem}"/>.</returns>
        public async Task<FileItem> FileMoveAsync(string driveId, string fileId, string toDriveId, string toParentFileId)
            => await FileMoveAsync(new() { DriveId = driveId, FileId = fileId, ToDriveId = toDriveId, ToParentFileId = toParentFileId });

        /// <summary>
        /// The FileMoveAsync.
        /// </summary>
        /// <param name="request">The request<see cref="FileMoveRequest"/>.</param>
        /// <returns>The <see cref="Task{FileItem}"/>.</returns>
        public async Task<FileItem> FileMoveAsync(FileMoveRequest request)
            => await SendJsonPostAsync<FileItem>("v3/file/update", request);

        /// <summary>
        /// The ShareAsync.
        /// </summary>
        /// <param name="request">The request<see cref="FileShareRequest"/>.</param>
        /// <returns>The <see cref="Task{FileShareResponse}"/>.</returns>
        public async Task<FileShareResponse> ShareAsync(FileShareRequest request)
            => await SendJsonPostAsync<FileShareResponse>("adrive/v2/share_link/create", request);

        /// <summary>
        /// The ShareAsync.
        /// </summary>
        /// <param name="driveId">The driveId<see cref="string"/>.</param>
        /// <param name="fileId">The fileId<see cref="string"/>.</param>
        /// <param name="expiration">The expiration<see cref="TimeSpan?"/>.</param>
        /// <param name="sharePwd">The sharePwd<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{FileShareResponse}"/>.</returns>
        public async Task<FileShareResponse> ShareAsync(string driveId, string fileId, TimeSpan? expiration = null, string sharePwd = null)
            => await ShareAsync(new FileShareRequest(driveId, fileId, expiration, sharePwd));

        /// <summary>
        /// The ShareAsync.
        /// </summary>
        /// <param name="driveId">The driveId<see cref="string"/>.</param>
        /// <param name="fileIdList">The fileIdList<see cref="List{string}"/>.</param>
        /// <param name="expiration">The expiration<see cref="TimeSpan?"/>.</param>
        /// <param name="sharePwd">The sharePwd<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{FileShareResponse}"/>.</returns>
        public async Task<FileShareResponse> ShareAsync(string driveId, List<string> fileIdList, TimeSpan? expiration = null, string sharePwd = null)
            => await ShareAsync(new FileShareRequest(driveId, fileIdList, expiration, sharePwd));
    }
}
