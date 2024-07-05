using Catalog.API.Resources;
using Catalog.Domain.Aggregates;
using DomainHelper.Interfaces;
using FamilieLaissMassTransitDefinitions.Commands;
using FamilieLaissMassTransitDefinitions.Contracts.Commands;
using FamilieLaissSharedObjects.Constants;
using FamilieLaissSharedObjects.Enums;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using ServiceLayerHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BackgroundService = ServiceLayerHelper.BackgroundService;

namespace Catalog.API.BackgroundServices
{
    /// <summary>
    /// Background Service - Generating messages for added media items 
    /// </summary>
    public class MessageForAddedMediaItemsBackgroundService : BackgroundService
    {
        #region Private Members
        private readonly IServiceScopeFactory _ScopeFactory;
        private readonly IBus _ServiceBus;
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="scopeFactory">Service Scope Factory. Will be injected by DI-Container</param>
        /// <param name="serviceBus">Masstransit service bus. Will be injected by DI-Container</param>
        public MessageForAddedMediaItemsBackgroundService(IServiceScopeFactory scopeFactory, IBus serviceBus)
        {
            _ScopeFactory = scopeFactory;
            _ServiceBus = serviceBus;
        }
        #endregion

        #region Protected Overrides
        /// <inheritdoc/>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //Solange kein beenden der Applikation ansteht, wird die Abfrage im Loop ausgeführt
            while (!stoppingToken.IsCancellationRequested)
            {
                //Erstellen des Scopes
                using (var scope = _ScopeFactory.CreateScope())
                {
                    //Ermitteln der Unit of Work
                    using (var _UnitOfWork = scope.ServiceProvider.GetRequiredService<iUnitOfWork>())
                    {
                        try
                        {
                            //Ermitteln des Repositories
                            var Repo = _UnitOfWork.GetReadOnlyRepository<MediaItem>();

                            //Ermitteln der Daten
                            //Dabei werden alle Media-Items ermittelt die in den Letzen
                            //10 Minuten erstellt worden sind
                            DateTimeOffset CheckDateTime = DateTimeOffset. Now.Subtract(new TimeSpan(0, 10, 0));
                            var ItemListe = await Repo.GetAll(x => x.CreateDate >= CheckDateTime, "", "MediaGroup");

                            //Ermitteln der Gruppen
                            List<MediaGroup> ListeGruppen = new List<MediaGroup>();
                            foreach (var Item in ItemListe)
                            {
                                if (!ListeGruppen.Exists(x => x.Id == Item.Id))
                                {
                                    ListeGruppen.Add(Item.MediaGroup);
                                }
                            }

                            //Die Liste der Gruppen durchgehen und die Anzahl aller Items ermitteln
                            //Und dann für die Gruppe eine Message erstellen
                            int CountItems = 0;
                            string GermanText = "";
                            string EnglishText = "";
                            CreateMessageForUserGroupCmd CmdOne;
                            CreateMessageForUserGroupCmd CmdTwo;
                            foreach (var ItemGroup in ListeGruppen)
                            {
                                //Ermitteln der Anzahl Items
                                CountItems = ItemListe.Count(x => x.MediaGroup.Id == ItemGroup.Id);

                                //Ermitteln des deutschen und englischen Textes
                                //if (CountItems == 1)
                                //{
                                //    GermanText = ItemGroup.MediaType == EnumMediaType.Picture ? 
                                //        string.Format(MessageText.MediaItems_Photo_Added_Single_German, ItemGroup.NameGerman) : 
                                //        string.Format(MessageText.MediaItems_Video_Added_Single_German, ItemGroup.NameGerman);
                                //    EnglishText = ItemGroup.MediaType == EnumMediaType.Picture ?
                                //        string.Format(MessageText.MediaItems_Photo_Added_Single_English, ItemGroup.NameEnglish) :
                                //        string.Format(MessageText.MediaItems_Video_Added_Single_English, ItemGroup.NameEnglish);
                                //}
                                //else
                                //{
                                //    GermanText = ItemGroup.MediaType == EnumMediaType.Picture ?
                                //        string.Format(MessageText.MediaItems_Photo_Added_Plural_German, CountItems.ToString(), ItemGroup.NameGerman) :
                                //        string.Format(MessageText.MediaItems_Video_Added_Plural_German, CountItems.ToString(), ItemGroup.NameGerman);
                                //    EnglishText = ItemGroup.MediaType == EnumMediaType.Picture ?
                                //        string.Format(MessageText.MediaItems_Photo_Added_Plural_English, CountItems.ToString(), ItemGroup.NameEnglish) :
                                //        string.Format(MessageText.MediaItems_Video_Added_Plural_English, CountItems.ToString(), ItemGroup.NameEnglish);
                                //}

                                //Erstellen des Commands
                                CmdOne = new CreateMessageForUserGroupCmd(enMessagePrio.Info, UserRoleConstants.User, GermanText, EnglishText, "");
                                CmdTwo = new CreateMessageForUserGroupCmd(enMessagePrio.Info, UserRoleConstants.User, GermanText, EnglishText, "");

                                //Erstellen der Nachricht
                                await _ServiceBus.Send<iCreateMessageForUserGroupCmd>(CmdOne);
                                await _ServiceBus.Send<iCreateMessageForUserGroupCmd>(CmdTwo);
                            }
                        }
                        catch
                        {
                            //Wenn eine Exception aufgetreten ist, dann kann das ignoriert werden
                            //da es sich hier nur um eine Zusatzinfo handelt und das System nicht direkt beinflusst
                        }
                    }
                }

                //10 Minuten bis zur nächsten Abfrage warten
                await Task.Delay(600000);
            }
        }
        #endregion
    }
}
