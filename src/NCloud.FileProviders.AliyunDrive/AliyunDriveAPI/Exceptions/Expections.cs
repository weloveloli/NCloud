// -----------------------------------------------------------------------
// <copyright file="Expections.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.AliyunDrive.AliyunDriveAPI.Exceptions
{
    /// <summary>
    /// Defines the <see cref="NotFoundException" />.
    /// </summary>
    public class NotFoundException : APIException
    {
        /// <summary>
        /// Gets or sets the FieldName.
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotFoundException"/> class.
        /// </summary>
        /// <param name="ex">The ex<see cref="APIException"/>.</param>
        public NotFoundException(APIException ex) : base(ex)
        {
            FieldName = ex.Code.Split('.')[1];
        }
    }

    /// <summary>
    /// Defines the <see cref="JsonParseException" />.
    /// </summary>
    public class JsonParseException : APIException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonParseException"/> class.
        /// </summary>
        /// <param name="ex">The ex<see cref="APIException"/>.</param>
        public JsonParseException(APIException ex) : base(ex)
        {
        }
    }

    /// <summary>
    /// Defines the <see cref="BadRequestException" />.
    /// </summary>
    public class BadRequestException : APIException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BadRequestException"/> class.
        /// </summary>
        /// <param name="ex">The ex<see cref="APIException"/>.</param>
        public BadRequestException(APIException ex) : base(ex)
        {
        }
    }

    /// <summary>
    /// Defines the <see cref="InvalidParameterException" />.
    /// </summary>
    public class InvalidParameterException : APIException
    {
        /// <summary>
        /// Gets or sets the FieldName.
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidParameterException"/> class.
        /// </summary>
        /// <param name="ex">The ex<see cref="APIException"/>.</param>
        public InvalidParameterException(APIException ex) : base(ex)
        {
            FieldName = ex.Code.Split('.')[1];
        }
    }

    /// <summary>
    /// Defines the <see cref="ForbiddenNoPermissionException" />.
    /// </summary>
    public class ForbiddenNoPermissionException : APIException
    {
        /// <summary>
        /// Gets or sets the ResourceName.
        /// </summary>
        public string ResourceName { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ForbiddenNoPermissionException"/> class.
        /// </summary>
        /// <param name="ex">The ex<see cref="APIException"/>.</param>
        public ForbiddenNoPermissionException(APIException ex) : base(ex)
        {
            ResourceName = ex.Code.Split('.')[1];
        }
    }

    /// <summary>
    /// Defines the <see cref="AccessTokenInvalidException" />.
    /// </summary>
    public class AccessTokenInvalidException : APIException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AccessTokenInvalidException"/> class.
        /// </summary>
        /// <param name="ex">The ex<see cref="APIException"/>.</param>
        public AccessTokenInvalidException(APIException ex) : base(ex)
        {
        }
    }

    /// <summary>
    /// Defines the <see cref="InvalidResourceException" />.
    /// </summary>
    public class InvalidResourceException : APIException
    {
        /// <summary>
        /// Gets or sets the ResourceName.
        /// </summary>
        public string ResourceName { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidResourceException"/> class.
        /// </summary>
        /// <param name="ex">The ex<see cref="APIException"/>.</param>
        public InvalidResourceException(APIException ex) : base(ex)
        {
            ResourceName = ex.Code.Split('.')[1];
        }
    }

    /// <summary>
    /// Defines the <see cref="AlreadyExistException" />.
    /// </summary>
    public class AlreadyExistException : APIException
    {
        /// <summary>
        /// Gets or sets the ResourceName.
        /// </summary>
        public string ResourceName { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AlreadyExistException"/> class.
        /// </summary>
        /// <param name="ex">The ex<see cref="APIException"/>.</param>
        public AlreadyExistException(APIException ex) : base(ex)
        {
            ResourceName = ex.Code.Split('.')[1];
        }
    }
}
