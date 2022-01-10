// -----------------------------------------------------------------------
// <copyright file="FileItem.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.AliyunDrive.AliyunDriveAPI.Models
{
    using System;
    using System.Text.Json.Nodes;
    using System.Text.Json.Serialization;
    using NCloud.FileProviders.AliyunDrive.AliyunDriveAPI.Models.Types;

    /// <summary>
    /// Defines the <see cref="FileItem" />.
    /// </summary>
    public class FileItem
    {
        /// <summary>
        /// Gets or sets the DriveId.
        /// </summary>
        public string DriveId { get; set; }

        /// <summary>
        /// Gets or sets the DomainId.
        /// </summary>
        public string DomainId { get; set; }

        /// <summary>
        /// Gets or sets the FileId.
        /// </summary>
        public string FileId { get; set; }

        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Type.
        /// </summary>
        [JsonConverter(typeof(Converters.JsonStringEnumConverter))]
        public FileType Type { get; set; }

        /// <summary>
        /// Gets a value indicating whether IsFile.
        /// </summary>
        [JsonIgnore]
        public bool IsFile => Type == FileType.File;

        /// <summary>
        /// Gets a value indicating whether IsFolder.
        /// </summary>
        [JsonIgnore]
        public bool IsFolder => Type == FileType.Folder;

        /// <summary>
        /// Gets or sets the CreatedAt.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the UpdatedAt.
        /// </summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Hidden.
        /// </summary>
        public bool Hidden { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Starred.
        /// </summary>
        public bool Starred { get; set; }

        /// <summary>
        /// Gets or sets the Status.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Gets a value indicating whether IsAvailable.
        /// </summary>
        [JsonIgnore]
        public bool IsAvailable => Status == "available";

        /// <summary>
        /// Gets or sets the ParentFileId.
        /// </summary>
        public string ParentFileId { get; set; }

        /// <summary>
        /// Gets or sets the EncryptMode.
        /// </summary>
        public string EncryptMode { get; set; }

        /// <summary>
        /// Gets or sets the RevisionId.
        /// </summary>
        public string RevisionId { get; set; }

        /// <summary>
        /// Gets or sets the UserMeta.
        /// </summary>
        public JsonNode UserMeta { get; set; }

        /// <summary>
        /// Gets or sets the CreatorType.
        /// </summary>
        public string CreatorType { get; set; }

        /// <summary>
        /// Gets or sets the CreatorId.
        /// </summary>
        public string CreatorId { get; set; }

        /// <summary>
        /// Gets or sets the CreatorName.
        /// </summary>
        public string CreatorName { get; set; }

        /// <summary>
        /// Gets or sets the LastModifierType.
        /// </summary>
        public string LastModifierType { get; set; }

        /// <summary>
        /// Gets or sets the LastModifierId.
        /// </summary>
        public string LastModifierId { get; set; }

        /// <summary>
        /// Gets or sets the LastModifierName.
        /// </summary>
        public string LastModifierName { get; set; }

        /// <summary>
        /// Gets or sets the ContentType.
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// Gets or sets the MimeType.
        /// </summary>
        public string MimeType { get; set; }

        /// <summary>
        /// Gets or sets the MimeExtension.
        /// </summary>
        public string MimeExtension { get; set; }

        /// <summary>
        /// Gets or sets the Size.
        /// </summary>
        public long? Size { get; set; }

        /// <summary>
        /// Gets or sets the UploadId.
        /// </summary>
        public string UploadId { get; set; }

        /// <summary>
        /// Gets or sets the Crc64Hash.
        /// </summary>
        public string Crc64Hash { get; set; }

        /// <summary>
        /// Gets or sets the ContentHash.
        /// </summary>
        public string ContentHash { get; set; }

        /// <summary>
        /// Gets or sets the ContentHashName.
        /// </summary>
        public string ContentHashName { get; set; }

        /// <summary>
        /// Gets or sets the DownloadUrl.
        /// </summary>
        public string DownloadUrl { get; set; }

        /// <summary>
        /// Gets or sets the Url.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the Category.
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Gets the CategoryType.
        /// </summary>
        [JsonIgnore]
        public FileCategoryType? CategoryType => Category switch
        {
            null => null,
            "image" => FileCategoryType.Image,
            "video" => FileCategoryType.Video,
            "audio" => FileCategoryType.Audio,
            "app" => FileCategoryType.APP,
            "doc" => FileCategoryType.Doc,
            "zip" => FileCategoryType.Zip,
            _ => FileCategoryType.Others
        };

        /// <summary>
        /// Gets or sets the PunishFlag.
        /// </summary>
        public int? PunishFlag { get; set; }

        /// <summary>
        /// Gets or sets the Labels.
        /// </summary>
        public string[] Labels { get; set; }

        /// <summary>
        /// Gets or sets the Thumbnail.
        /// </summary>
        public string Thumbnail { get; set; }

        /// <summary>
        /// Gets or sets the ImageMediaMetadata.
        /// </summary>
        public ImageMediaMetadata ImageMediaMetadata { get; set; }

        /// <summary>
        /// Gets or sets the VideoMediaMetadata.
        /// </summary>
        public VideoMediaMetadata VideoMediaMetadata { get; set; }

        /// <summary>
        /// Gets or sets the VideoPreviewMetadata.
        /// </summary>
        public VideoPreviewMetadata VideoPreviewMetadata { get; set; }

        /// <summary>
        /// Gets the AudioMediaMetadata.
        /// </summary>
        [JsonIgnore]
        public AudioMediaMetadata AudioMediaMetadata => VideoPreviewMetadata ?? null;
    }

    /// <summary>
    /// Defines the <see cref="ImageMediaMetadata" />.
    /// </summary>
    public class ImageMediaMetadata
    {
        /// <summary>
        /// Gets or sets the Width.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets the Height.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets the ImageTags.
        /// </summary>
        public ImageTag[] ImageTags { get; set; }

        /// <summary>
        /// Gets or sets the Exif.
        /// </summary>
        public JsonNode Exif { get; set; }

        /// <summary>
        /// Gets or sets the ImageQuality.
        /// </summary>
        public ImageQuality ImageQuality { get; set; }

        /// <summary>
        /// Gets or sets the CroppingSuggestion.
        /// </summary>
        public CroppingSuggestion[] CroppingSuggestion { get; set; }
    }

    /// <summary>
    /// Defines the <see cref="ImageTag" />.
    /// </summary>
    public class ImageTag
    {
        /// <summary>
        /// Gets or sets the Confidence.
        /// </summary>
        public decimal Confidence { get; set; }

        /// <summary>
        /// Gets or sets the ParentName.
        /// </summary>
        public string ParentName { get; set; }

        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the TagLevel.
        /// </summary>
        public int TagLevel { get; set; }

        /// <summary>
        /// Gets or sets the CentricScore.
        /// </summary>
        public decimal CentricScore { get; set; }
    }

    /// <summary>
    /// Defines the <see cref="ImageQuality" />.
    /// </summary>
    public class ImageQuality
    {
        /// <summary>
        /// Gets or sets the OverallScore.
        /// </summary>
        public decimal OverallScore { get; set; }
    }

    /// <summary>
    /// Defines the <see cref="CroppingSuggestion" />.
    /// </summary>
    public class CroppingSuggestion
    {
        /// <summary>
        /// Gets or sets the AspectRatio.
        /// </summary>
        public string AspectRatio { get; set; }

        /// <summary>
        /// Gets or sets the Score.
        /// </summary>
        public decimal Score { get; set; }

        /// <summary>
        /// Gets or sets the CroppingBoundary.
        /// </summary>
        public CroppingBoundary CroppingBoundary { get; set; }
    }

    /// <summary>
    /// Defines the <see cref="CroppingBoundary" />.
    /// </summary>
    public class CroppingBoundary
    {
        /// <summary>
        /// Gets or sets the Width.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets the Height.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets the Top.
        /// </summary>
        public int Top { get; set; }

        /// <summary>
        /// Gets or sets the Left.
        /// </summary>
        public int Left { get; set; }
    }

    /// <summary>
    /// Defines the <see cref="VideoPreviewMetadata" />.
    /// </summary>
    public class VideoPreviewMetadata : AudioMediaMetadata
    {
        /// <summary>
        /// Gets or sets the VideoFormat.
        /// </summary>
        public string VideoFormat { get; set; }

        /// <summary>
        /// Gets or sets the FrameRate.
        /// </summary>
        public string FrameRate { get; set; }

        /// <summary>
        /// Gets or sets the Height.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets the Width.
        /// </summary>
        public int Width { get; set; }
    }

    /// <summary>
    /// Defines the <see cref="VideoMediaMetadata" />.
    /// </summary>
    public class VideoMediaMetadata
    {
        /// <summary>
        /// Gets or sets the Width.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets the Height.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets the ImageTags.
        /// </summary>
        public ImageTag[] ImageTags { get; set; }

        /// <summary>
        /// Gets or sets the VideoMediaVideoStream.
        /// </summary>
        public VideoMediaVideoStream[] VideoMediaVideoStream { get; set; }

        /// <summary>
        /// Gets or sets the VideoMediaAudioStream.
        /// </summary>
        public VideoMediaAudioStream[] VideoMediaAudioStream { get; set; }

        /// <summary>
        /// Gets or sets the Duration.
        /// </summary>
        public TimeSpan Duration { get; set; }
    }

    /// <summary>
    /// Defines the <see cref="VideoMediaVideoStream" />.
    /// </summary>
    public class VideoMediaVideoStream
    {
        /// <summary>
        /// Gets or sets the Duration.
        /// </summary>
        public TimeSpan Duration { get; set; }

        /// <summary>
        /// Gets or sets the Clarity.
        /// </summary>
        public string Clarity { get; set; }

        /// <summary>
        /// Gets or sets the Fps.
        /// </summary>
        public string Fps { get; set; }

        /// <summary>
        /// Gets or sets the Bitrate.
        /// </summary>
        public string Bitrate { get; set; }

        /// <summary>
        /// Gets or sets the CodeName.
        /// </summary>
        public string CodeName { get; set; }
    }

    /// <summary>
    /// Defines the <see cref="VideoMediaAudioStream" />.
    /// </summary>
    public class VideoMediaAudioStream
    {
        /// <summary>
        /// Gets or sets the Duration.
        /// </summary>
        public TimeSpan Duration { get; set; }

        /// <summary>
        /// Gets or sets the Channels.
        /// </summary>
        public int Channels { get; set; }

        /// <summary>
        /// Gets or sets the ChannelLayout.
        /// </summary>
        public string ChannelLayout { get; set; }

        /// <summary>
        /// Gets or sets the BitRate.
        /// </summary>
        public string BitRate { get; set; }

        /// <summary>
        /// Gets or sets the CodeName.
        /// </summary>
        public string CodeName { get; set; }

        /// <summary>
        /// Gets or sets the SampleRate.
        /// </summary>
        public string SampleRate { get; set; }
    }

    /// <summary>
    /// Defines the <see cref="AudioMediaMetadata" />.
    /// </summary>
    public class AudioMediaMetadata
    {
        /// <summary>
        /// Gets or sets the Bitrate.
        /// </summary>
        public string Bitrate { get; set; }

        /// <summary>
        /// Gets or sets the Duration.
        /// </summary>
        public string Duration { get; set; }

        /// <summary>
        /// Gets or sets the AudioFormat.
        /// </summary>
        public string AudioFormat { get; set; }

        /// <summary>
        /// Gets or sets the AudioSampleRate.
        /// </summary>
        public string AudioSampleRate { get; set; }

        /// <summary>
        /// Gets or sets the AudioChannels.
        /// </summary>
        public int AudioChannels { get; set; }

        /// <summary>
        /// Gets or sets the AudioTemplateList.
        /// </summary>
        public AudioTemplate[] AudioTemplateList { get; set; }

        /// <summary>
        /// Gets or sets the AudioMeta.
        /// </summary>
        public AudioMeta AudioMeta { get; set; }

        /// <summary>
        /// Gets or sets the AudioMusicMeta.
        /// </summary>
        public AudioMusicMeta AudioMusicMeta { get; set; }
    }

    /// <summary>
    /// Defines the <see cref="AudioTemplate" />.
    /// </summary>
    public class AudioTemplate
    {
        /// <summary>
        /// Gets or sets the TemplateId.
        /// </summary>
        public string TemplateId { get; set; }

        /// <summary>
        /// Gets or sets the Status.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Gets a value indicating whether IsFinished.
        /// </summary>
        [JsonIgnore]
        public bool IsFinished => Status == "finished";
    }

    /// <summary>
    /// Defines the <see cref="AudioMeta" />.
    /// </summary>
    public class AudioMeta
    {
        /// <summary>
        /// Gets or sets the Bitrate.
        /// </summary>
        public long Bitrate { get; set; }

        /// <summary>
        /// Gets or sets the Duration.
        /// </summary>
        public TimeSpan Duration { get; set; }

        /// <summary>
        /// Gets or sets the SampleRate.
        /// </summary>
        public long SampleRate { get; set; }

        /// <summary>
        /// Gets or sets the Channels.
        /// </summary>
        public int Channels { get; set; }
    }

    /// <summary>
    /// Defines the <see cref="AudioMusicMeta" />.
    /// </summary>
    public class AudioMusicMeta
    {
        /// <summary>
        /// Gets or sets the Title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the Artist.
        /// </summary>
        public string Artist { get; set; }

        /// <summary>
        /// Gets or sets the Album.
        /// </summary>
        public string Album { get; set; }

        /// <summary>
        /// Gets or sets the CoverUrl.
        /// </summary>
        public string CoverUrl { get; set; }
    }

    /// <summary>
    /// Defines the <see cref="ExFieldsInfo" />.
    /// </summary>
    public class ExFieldsInfo
    {
        /// <summary>
        /// Gets or sets the VideoMetaProcessed.
        /// </summary>
        public string VideoMetaProcessed { get; set; }

        /// <summary>
        /// Gets a value indicating whether IsVideoMetaProcessed.
        /// </summary>
        [JsonIgnore]
        public bool IsVideoMetaProcessed => VideoMetaProcessed == "y";
    }
}
