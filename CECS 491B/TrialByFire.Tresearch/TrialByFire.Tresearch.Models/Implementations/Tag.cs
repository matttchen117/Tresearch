using System;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Models.Implementations
{
    /// <summary>
    ///     Tag class. Holds tag name and number of nodes it is tagged with
    /// </summary>
    public class Tag : ITag, IEquatable<Tag>
    {
        /// <summary>
        ///     String holding name of tag
        /// </summary>
        public string tagName { get; set; }

        /// <summary>
        /// Long holding number of nodes that contain this tag
        /// </summary>
        public long tagCount { get; set; }

        /// <summary>
        /// Constructor. Tag name is passed in and count is initialized to 0
        /// </summary>
        /// <param name="tagName"></param>
        public Tag(string tagName)
        {
            this.tagName = tagName;
            tagCount = 0;
        }

        /// <summary>
        /// Constructor. Tag name and count are passed in.
        /// </summary>
        /// <param name="tagName">String tag name</param>
        /// <param name="tagCount">Long count of nodes using tag</param>
        public Tag(string tagName, long tagCount)
        {
            this.tagName = tagName;
            this.tagCount = tagCount;
        }

        /// <summary>
        ///     Checks if tags are equal
        /// </summary>
        /// <param name="obj">Object to be compared</param>
        /// <returns>If tags are equal</returns>
        public override bool Equals(object? obj)
        {
            if (obj != null)
            {
                if (obj is Tag)
                {
                    ITag tag = (ITag)obj;
                    return (tag.tagName == tagName);
                }
            }
            return false;
        }

        /// <summary>
        ///     Checks if tags are equal
        /// </summary>
        /// <param name="t">Tag</param>
        /// <returns>If tags are equal</returns>
        public bool Equals(Tag t)
        {
            return (tagName == t.tagName);
        }
    }
}