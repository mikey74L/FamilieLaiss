using DomainHelper.AbstractClasses;
using FamilieLaissSharedObjects.Enums;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Message.Domain.Aggregates
{
    /// <summary>
    /// Entity for message
    /// </summary>
    public class Message: EntityCreation<long>
    {
        #region Private Members
        private ILazyLoader _LazyLoader;
        #endregion

        #region Properties
        /// <summary>
        /// The priority of the message
        /// </summary>
        public enMessagePrio Prio { get; private set; }

        /// <summary>
        /// The german message text
        /// </summary>
        public string Text_German { get; private set; }

        /// <summary>
        /// The english message text
        /// </summary>
        public string Text_English { get; private set; }

        /// <summary>
        /// Additional data for the message
        /// </summary>
        public string AdditionalData { get; private set; }

        /// <summary>
        /// List of related users for this message
        /// </summary>
        private HashSet<MessageUser> _MessageUsers;
        public IEnumerable<MessageUser> MessageUsers => _MessageUsers;
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor without parameters would be used by EF-Core
        /// </summary>
        private Message(ILazyLoader lazyLoader)
        {
            _LazyLoader = lazyLoader;
        }

        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="prio">Priority for the message</param>
        /// <param name="textEnglish">The english message text</param>
        /// <param name="textGerman">The german message text</param>
        public Message(enMessagePrio prio, string textGerman, string textEnglish, string additionalData) : base()
        {
            //Übernehmen der Werte
            Prio = prio;
            Text_English = textEnglish;
            Text_German = textGerman;
            AdditionalData = additionalData;

            //Erstellen der leeren Liste für die User
            _MessageUsers = new HashSet<MessageUser>();
        }
        #endregion

        #region Domain Methods
        /// <summary>
        /// Add a user to a message
        /// </summary>
        /// <param name="userName"></param>
        public void AddMessageUser(string userName)
        {
            //Eine neue User-Entity hinzufügen
            var UserEntity = new MessageUser(this, userName);

            //Hinzufügen der User-Entity zur Collection
            _MessageUsers.Add(UserEntity);
        }

        /// <summary>
        /// Set the message to the state readed for the defined user
        /// </summary>
        /// <param name="userName"></param>
        public async Task SetMessageReaded(string userName)
        {
            //Nachladen der Navigation-Property
            await _LazyLoader.LoadAsync(this, navigationName: nameof(MessageUsers));

            //Ermitteln des entsprechenden User-Eintrages
            var UserEntity = _MessageUsers.First(x => x.UserName == userName);

            //Die entsprechende User-Entity als readed lesen
            UserEntity.SetReaded();
        }

        /// <summary>
        /// Set the message to state readed for all users
        /// </summary>
        public async Task SetMessageReadedAll()
        {
            //Nachladen der Navigation-Property
            await _LazyLoader.LoadAsync(this, navigationName: nameof(MessageUsers));

            //Für alle Items den Status auf readed setzen
            foreach (var Item in _MessageUsers.Where(x => !x.Readed))
            {
                Item.SetReaded();
            }
        }
        #endregion

        #region Called from Change-Tracker
        public override Task EntityAddedAsync()
        {
            return Task.CompletedTask;
        }

        public override async Task EntityDeletedAsync()
        {
            //Nachladen der Navigation Property
            await _LazyLoader.LoadAsync(this, navigationName: nameof(MessageUsers));

            //In jedem zugeordneten Message-User die Delete-Methode aufrufen
            foreach (var Item in MessageUsers)
            {
                await Item.EntityDeletedAsync();
            }
        }
        #endregion
    }
}
