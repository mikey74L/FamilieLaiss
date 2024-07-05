using DomainHelper.AbstractClasses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.Domain.Aggregates
{
    public class SchedulerEvent: EntityModify<long>
    {
        #region Properties
        /// <summary>
        /// Title for this event
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// Description for this event
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// The name of the location where the event would happen
        /// </summary>
        public string Location { get; private set; }

        /// <summary>
        /// The start time when the event begins
        /// </summary>
        public DateTimeOffset StartTime { get; private set; }

        /// <summary>
        /// The name of the timezone for the StartTime
        /// </summary>
        public string StartTimeZone { get; private set; }

        /// <summary>
        /// The end time when the event ends
        /// </summary>
        public DateTimeOffset EndTime { get; private set; }

        /// <summary>
        /// The name of the timezone for the EndTime
        /// </summary>
        public string EndTimeZone { get; private set; }

        /// <summary>
        /// Is the event an all day event?
        /// </summary>
        public bool IsAllDay { get; private set; }

        /// <summary>
        /// The recurrence rule for the event if it is an recurrence event
        /// </summary>
        public string RecurrenceRule { get; private set; }
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor (Called by EF.Core)
        /// </summary>
        public SchedulerEvent()
        {

        }
        #endregion

        #region Domain Methods
        #endregion

        #region Change-Tracker
        public override Task EntityModifiedAsync()
        {
            return Task.CompletedTask;
        }

        public override Task EntityAddedAsync()
        {
            return Task.CompletedTask;
        }

        public override Task EntityDeletedAsync()
        {
            return Task.CompletedTask;
        }
        #endregion
    }
}
