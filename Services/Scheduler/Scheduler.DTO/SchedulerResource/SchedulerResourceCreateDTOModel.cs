using System;
using System.Collections.Generic;
using System.Text;

namespace Scheduler.DTO.SchedulerResource
{
    public class SchedulerResourceCreateDTOModel
    {
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
    }
}
