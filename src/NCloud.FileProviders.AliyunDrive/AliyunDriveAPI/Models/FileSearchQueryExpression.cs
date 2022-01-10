// -----------------------------------------------------------------------
// <copyright file="FileSearchQueryExpression.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.AliyunDrive.AliyunDriveAPI.Models
{
    using System.Collections.Generic;
    using System.Text;
    using NCloud.FileProviders.AliyunDrive.AliyunDriveAPI.Models.Types;
    using NCloud.FileProviders.AliyunDrive.AliyunDriveAPI.Utils;

    /// <summary>
    /// Defines the <see cref="FileSearchQueryExpression" />.
    /// </summary>
    public class FileSearchQueryExpression : List<FileSearchQuery>
    {
        /// <summary>
        /// Gets or sets the RelationType.
        /// </summary>
        public QueryRelationType RelationType { get; set; } = QueryRelationType.AND;

        /// <summary>
        /// Gets or sets the Children.
        /// </summary>
        public FileSearchQueryExpression Children { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileSearchQueryExpression"/> class.
        /// </summary>
        public FileSearchQueryExpression()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileSearchQueryExpression"/> class.
        /// </summary>
        /// <param name="conditions">The conditions<see cref="IEnumerable{FileSearchQuery}"/>.</param>
        public FileSearchQueryExpression(IEnumerable<FileSearchQuery> conditions) : base(conditions)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileSearchQueryExpression"/> class.
        /// </summary>
        /// <param name="conditions">The conditions<see cref="IEnumerable{FileSearchQuery}"/>.</param>
        /// <param name="relationType">The relationType<see cref="QueryRelationType"/>.</param>
        public FileSearchQueryExpression(IEnumerable<FileSearchQuery> conditions, QueryRelationType relationType) : base(conditions)
        {
            RelationType = relationType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileSearchQueryExpression"/> class.
        /// </summary>
        /// <param name="relationType">The relationType<see cref="QueryRelationType"/>.</param>
        /// <param name="children">The children<see cref="FileSearchQueryExpression"/>.</param>
        public FileSearchQueryExpression(QueryRelationType relationType, FileSearchQueryExpression children)
        {
            RelationType = relationType;
            Children = children;
        }

        /// <summary>
        /// The ToString.
        /// </summary>
        /// <returns>The <see cref="string"/>.</returns>
        public override string ToString()
        {
            var childrenText = string.Empty;
            if (Children != null)
                childrenText = Children.ToString();
            var sb = new StringBuilder();
            for (var i = 0; i < Count; i++)
            {
                var text = this[i].ToString();
                sb.Append(text);
                if (i != Count - 1)
                    sb.Append(" " + ReflectionUtils.GetEnumValueName(this[i].NearRelationType) + " ");
            }
            var thisText = sb.ToString();
            sb = sb.Clear();
            if (!string.IsNullOrEmpty(childrenText))
            {
                if (!string.IsNullOrEmpty(thisText))
                {
                    sb.Append("(" + thisText + ") ");
                    sb.Append(ReflectionUtils.GetEnumValueName(RelationType) + " ");
                    sb.Append("(" + childrenText + ")");
                }
                else
                    sb.Append(childrenText);
            }
            else
                sb.Append(thisText);
            return sb.ToString();
        }
    }
}
