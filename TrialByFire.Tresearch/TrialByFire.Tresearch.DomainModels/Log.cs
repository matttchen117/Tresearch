using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrialByFire.Tresearch.DomainModels
{
    public class Log
    {
        public string TimeStamp { get; set; }

        public string Level { get; set; }

        public string Username { get; set; }

        public string Category { get; set; }

        public string Description { get; set; }


        public Log()
        {
        }

        public Log(string timeStamp, string level, string username, string category, string description)
        {
            TimeStamp = timeStamp;
            Level = level;
            Username = username;
            Category = category;
            Description = description;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                Log log = (Log)obj;
                return ((TimeStamp.Equals(log.TimeStamp)) && (Level.Equals(log.Level)) && (Username.Equals(log.Username))
                    && (Category.Equals(log.Category)) && Description.Equals(log.Description));
            }
        }

        public override string ToString()
        {
            return "Timestamp: " + TimeStamp + ", Level: " + Level + ", Username: " + Username + ", Category: " + Category + ", Description: " + Description;
        }
    }
}
