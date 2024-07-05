namespace DomainHelper.DomainEvents
{
    public abstract class DomainEventSingle : DomainEventBase
    {
        #region C'tor
        public DomainEventSingle(string id) : base(id, false)
        {

        }
        #endregion
    }
}
