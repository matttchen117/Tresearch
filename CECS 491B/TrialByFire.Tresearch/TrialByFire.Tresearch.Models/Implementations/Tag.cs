using System;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Models.Implementations
{
    public class Tag : ITag
    {
        public string tagName { get; set; }

        public Tag(string tagName)
        {
            this.tagName = tagName;
        }
    }
}