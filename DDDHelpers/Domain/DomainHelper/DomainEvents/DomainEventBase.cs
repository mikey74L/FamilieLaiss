using MediatR;

namespace DomainHelper.DomainEvents
{
    public abstract class DomainEventBase : INotification
    {
        #region C'tor
        public DomainEventBase(string id, bool multipleEventsAllowed)
        {
            ID = id;
            MultipleEventsAllowed = multipleEventsAllowed;
        }
        #endregion

        #region Public Events
        public bool MultipleEventsAllowed { get; }

        public string ID { get; }
        #endregion
    }
}
