using DomainHelper.AbstractClasses;
using DomainHelper.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.Domain.Entities
{
    public class SchedulerResource: EntityModify<long>
    {
        #region Properties
        /// <summary>
        /// Name for this scheduler resource
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Display color for this scheduler resource
        /// </summary>
        public string Color { get; private set; }

        /// <summary>
        /// Starting hour for working day 
        /// </summary>
        public string StartingTime { get; private set; }

        /// <summary>
        /// Ending hour for working day
        /// </summary>
        public string EndingTime { get; private set; }
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor (Called by EF.Core)
        /// </summary>
        public SchedulerResource()
        {

        }

        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="name">Name for this scheduler resource</param>
        /// <param name="color">Display color for this scheduler resource</param>
        /// <param name="startingTime">Starting hour for working day </param>
        /// <param name="endingTime">Ending hour for working day</param>
        public SchedulerResource(string name, string color, string startingTime, string endingTime)
        {
            //Überprüfen ob ein Name eingegeben wurde
            if (string.IsNullOrEmpty(name)) throw new DomainException("A name is required");

            //Überprüfen ob eine Farbe angegeben wurde
            if (string.IsNullOrEmpty(color)) throw new DomainException("A color is required");

            //Überprüfen ob eine Startzeit festgelegt wurde
            if (string.IsNullOrEmpty(startingTime)) throw new DomainException("A starting time is required");

            //Überprüfen ob eine Endezeit festgelegt wurde
            if (string.IsNullOrEmpty(endingTime)) throw new DomainException("A ending time is required");

            //Übernehmen der Werte
            Name = name;
            Color = color;
            StartingTime = startingTime;
            EndingTime = endingTime;
        }
        #endregion

        #region Domain Methods
        /// <summary>
        /// Update the scheduler resource data
        /// </summary>
        /// <param name="name">Name for this scheduler resource</param>
        /// <param name="color">Display color for this scheduler resource</param>
        /// <param name="startingTime">Starting hour for working day </param>
        /// <param name="endingTime">Ending hour for working day</param>
        public void Update(string name, string color, string startingTime, string endingTime)
        {
            //Überprüfen ob ein Name eingegeben wurde
            if (string.IsNullOrEmpty(name)) throw new DomainException("A name is required");

            //Überprüfen ob eine Farbe angegeben wurde
            if (string.IsNullOrEmpty(color)) throw new DomainException("A color is required");

            //Überprüfen ob eine Startzeit festgelegt wurde
            if (string.IsNullOrEmpty(startingTime)) throw new DomainException("A starting time is required");

            //Überprüfen ob eine Endezeit festgelegt wurde
            if (string.IsNullOrEmpty(endingTime)) throw new DomainException("A ending time is required");

            //Übernehmen der Werte
            Name = name;
            Color = color;
            StartingTime = startingTime;
            EndingTime = endingTime;
        }
        #endregion

        #region Called from Change-Tracker
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
