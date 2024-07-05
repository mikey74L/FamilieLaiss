class VideoJsOperations {
    ///Initialisiert ein HTML-Video-Element als VideoJs-Element
    initializePlayer(idPlayer, currentLanguage, videoUrl, videoType, filenameVtt, userSettings) {
        //Video.js Player initialisieren und Lokalisierung hinzufügen
        var player = videojs(idPlayer, {
            //autoplay: userSettings.VideoAutoPlay,   
            controls: true,
            //loop: userSettings.VideoLoop,        
            muted: false,
            preload: 'auto',
            sources: [{
                src: videoUrl,
                type: videoType
            }],
            language: currentLanguage
        });

        //Verdrahten des Event-Handlers
        player.on('nuevoReady', function () {
            //Setzen der Metadaten für das VTT-File
            var track = ({ kind: 'metadata', src: filenameVtt });

            //Laden des VTT-Files
            player.loadTracks(track);
        });

        //Initialisieren des Nuevo-Plugins
        player.nuevo({
            //Optionen-Menu
            pipButton: false,                                                 //Ausblenden des Picture in Picture Buttons
            relatedMenu: false,                                               //Anzeigen der Liste der ähnlichen Videos
            shareMenu: false,                                                 //Anzeigen der Share-Option (Facebook, etc)
//            rateMenu: userSettings.ShowPlayRateMenu,                          //Anzeigen des Menüs für die Abspielgeschwindigkeiten
//            qualityMenu: userSettings.ShowQualityMenu,                        //Soll die Auswahl der Videoqualität in den Optionen angezeigt werden (true) oder direkt im Player (false) 
            //Zeit-Tooltip in Zeitleiste
//            timetooltip: userSettings.ShowTooltipForCurrentPlaytime,          //Soll ein Tooltip für die aktuelle Abspielzeit angezeigt werden
//            mousedisplay: userSettings.ShowTooltipForPlaytimeOnMouseCursor,   //Soll ein Tooltip für die Abspielzeit für den aktuellen Mauscursor angezeigt werden
//            buttonRewind: userSettings.ShowButtonRewind,                      //Soll der Button für das Rückwärtsspringen angezeigt werden 
//            buttonForward: userSettings.ShowButtonForward,                    //Soll der Button für das Vorwärtsspringen angezeigt werden 
            //Zoom-Optionen
//            zoomMenu: userSettings.ShowZoomMenu,                              //Anzeigen der Zoom-Optionen im Player 
 //           zoomInfo: userSettings.ShowZoomInfo,                              //Anzeigen der Zoom-Info in der oberen linken Ecke des Players
  //          zoomWheel: userSettings.AllowZoomingWithMouseWheel,               //Zoomen mit Mausrad erlauben
            //Mirror-Option
   //         mirrorButton: userSettings.ShowMirrorButton,                      //Soll der Button zum horizontalen Spiegeln des Videos angezeigt werden 
            //Context-Menü-Optionen
  //          contextMenu: userSettings.ShowContextMenu                         //Soll das Context-Menü angezeigt werden 


            //TODO Chapter Handling mit Editing währe noch ein geiles Feature
            //TODO Subtitles und vielleicht automatisches Übersetzen über Azure könnte auch noch ein Feature sein
            //TODO Chromecast währe auch noch eine tolles Feature
            //TODO Und dann brauchen wir noch die Playlist
        });

        //Initialisieren des Thumbnail-Plugins
        player.thumbnails();

        //Setzen der initialen Lautstärke des Players aus den User-Settings
        //player.volume(userSettings.VideoVolume / 100);
    }
}

const videoJsOperations = new VideoJsOperations();

export { videoJsOperations }
