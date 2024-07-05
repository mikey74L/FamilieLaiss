namespace DomainHelper.DomainEvents
{
    public abstract class DomainEventMultiple : DomainEventBase
    {
        #region C'tor
        public DomainEventMultiple(string id) : base(id, true)
        {

        }
        #endregion
    }
}
