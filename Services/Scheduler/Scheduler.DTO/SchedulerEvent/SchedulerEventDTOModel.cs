using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.DTO.SchedulerEvent
{
    public class SchedulerEventDTOModel
    {
        /// <summary>
        /// Identifier for scheduler event
        /// </summary>
        public long ID { get; set; }

        /// <summary>
        /// Title for this event
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Description for this event
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The name of the location where the event would happen
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// The start time when the event begins
        /// </summary>
        public DateTimeOffset StartTime { get; set; }

        /// <summary>
        /// The name of the timezone for the StartTime
        /// </summary>
        public string StartTimeZone { get; set; }

        /// <summary>
        /// The end time when the event ends
        /// </summary>
        public DateTimeOffset EndTime { get; set; }

        /// <summary>
        /// The name of the timezone for the EndTime
        /// </summary>
        public string EndTimeZone { get; set; }

        /// <summary>
        /// Is the event an all day event?
        /// </summary>
        public bool IsAllDay { get; set; }

        /// <summary>
        /// The recurrence rule for the event if it is an recurrence event
        /// </summary>
        public string RecurrenceRule { get; set; }
    
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
