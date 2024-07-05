using System;
using System.Collections.Generic;
using System.Text;

namespace Scheduler.DTO.SchedulerResource
{
    public class SchedulerResourceDTOModel
    {
        /// <summary>
        /// Identifier for this entity
        /// </summary>
        public long ID { get; set; }

        /// <summary>
        /// Name for this scheduler resource
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Display color for this scheduler resource
        /// </summary>
        public string Color { get; set; }

        /// <summary>
        /// Starting hour for working day 
        /// </summary>
        public string StartingTime { get; set; }

        /// <summary>
        /// Ending hour for working day
        /// </summary>
        public string EndingTime { get; set; }

        /// <summary>
        /// When was this entity created
        /// </summary>
        public DateTimeOffset CreateDate { get; set; }

        /// <summary>
        /// When was this entity last changed
        /// </summary>
        public DateTimeOffset? ChangeDate { get; set; }
    }
}
