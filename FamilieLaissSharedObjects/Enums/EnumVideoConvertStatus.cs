using FamilieLaissSharedObjects.Attributes;

namespace FamilieLaissSharedObjects.Enums;

public enum EnumVideoConvertStatus : byte
{
    /// <summary>
    /// Conversion is in state "Waiting for conversion"
    /// </summary>
    [DescriptionTranslation("Warte auf Konvertierung", "Waiting for conversion")]
    WaitingForConversion = 0,

    /// <summary>
    /// Conversion is in state "Reading media info from upload file - Begin"
    /// </summary>
    [DescriptionTranslation("Auslesen der Medien-Informationen - Beginn", "Reading media information - Begin")]
    ReadMediaInfoBegin = 1,

    /// <summary>
    /// Conversion is in state "Reading media info from upload file - End"
    /// </summary>
    [DescriptionTranslation("Auslesen der Medien-Informationen - Ende", "Reading media information - End")]
    ReadMediaInfoEnd = 2,

    /// <summary>
    /// Conversion is in state "Converting MP4 video - Begin"
    /// </summary>
    [DescriptionTranslation("Konvertiere MP4 Video - Beginn", "Converting MP4 video - Begin")]
    ConvertMp4VideoBegin = 3,

    /// <summary>
    /// Conversion is in state "Converting MP4 video - End"
    /// </summary>
    [DescriptionTranslation("Konvertiere MP4 Video - Ende", "Converting MP4 video - End")]
    ConvertMp4VideoEnd = 4,

    /// <summary>
    /// Conversion is in state "Converting MP4 video with resolution 640 x 360 - Begin"
    /// </summary>
    [DescriptionTranslation("Konvertiere MP4 Video für 640 x 360 - Beginn",
        "Converting MP4 video for 640 x 360 - Begin")]
    Convert640x360Begin = 5,

    /// <summary>
    /// Conversion is in state "Converting MP4 video with resolution 640 x 360 - End"
    /// </summary>
    [DescriptionTranslation("Konvertiere MP4 Video für 640 x 360 - Ende", "Converting MP4 video for 640 x 360 - End")]
    Convert640x360End = 6,

    /// <summary>
    /// Conversion is in state "Converting MP4 video with resolution 852 x 480 - Begin"
    /// </summary>
    [DescriptionTranslation("Konvertiere MP4 Video für 842 x 480 - Beginn",
        "Converting MP4 video for 842 x 480 - Begin")]
    Convert852x480Begin = 7,

    /// <summary>
    /// Conversion is in state "Converting MP4 video with resolution 852 x 480 - End"
    /// </summary>
    [DescriptionTranslation("Konvertiere MP4 Video für 842 x 480 - Ende", "Converting MP4 video for 842 x 480 - End")]
    Convert852x480End = 8,

    /// <summary>
    /// Conversion is in state "Converting MP4 video with resolution 1280 x 720 - Begin"
    /// </summary>
    [DescriptionTranslation("Konvertiere MP4 Video für 1280 x 720 - Beginn",
        "Converting MP4 video for 1280 x 720 - Begin")]
    Convert1280x720Begin = 9,

    /// <summary>
    /// Conversion is in state "Converting MP4 video with resolution 1280 x 720 - End"
    /// </summary>
    [DescriptionTranslation("Konvertiere MP4 Video für 1280 x 720 - Ende", "Converting MP4 video for 1280 x 720 - End")]
    Convert1280x720End = 10,

    /// <summary>
    /// Conversion is in state "Converting MP4 video with resolution 1920 x 1080 - Begin"
    /// </summary>
    [DescriptionTranslation("Konvertiere MP4 Video für 1920 x 1080 - Beginn",
        "Converting MP4 video for 1920 x 1080 - Begin")]
    Convert1920x1080Begin = 11,

    /// <summary>
    /// Conversion is in state "Converting MP4 video with resolution 1920 x 1080 - End"
    /// </summary>
    [DescriptionTranslation("Konvertiere MP4 Video für 1920 x 1080 - Ende",
        "Converting MP4 video for 1920 x 1080 - End")]
    Convert1920x1080End = 12,

    /// <summary>
    /// Conversion is in state "Converting MP4 video with resolution 3840 x 2160 - Begin"
    /// </summary>
    [DescriptionTranslation("Konvertiere MP4 Video für 3840 x 2160 - Beginn",
        "Converting MP4 video for 3840 x 2160 - Begin")]
    Convert3840x2160Begin = 13,

    /// <summary>
    /// Conversion is in state "Converting MP4 video with resolution 3840 x 2160 - End"
    /// </summary>
    [DescriptionTranslation("Konvertiere MP4 Video für 3840 x 2160 - Ende",
        "Converting MP4 video for 3840 x 2160 - End")]
    Convert3840x2160End = 14,

    /// <summary>
    /// Conversion is in state "Creating preview picture (Thumbnails) - Begin"
    /// </summary>
    [DescriptionTranslation("Erstelle Thumbnail-Bild - Beginn", "Creating thumbnail image - Begin")]
    CreatePreviewImageBegin = 15,

    /// <summary>
    /// Conversion is in state "Creating preview picture (Thumbnails) - End"
    /// </summary>
    [DescriptionTranslation("Erstelle Thumbnail-Bild - Ende", "Creating thumbnail image - End")]
    CreatePreviewImageEnd = 16,

    /// <summary>
    /// Conversion is in state "Creating HLS content - Begin"
    /// </summary>
    [DescriptionTranslation("Erstelle HLS-Inhalt - Beginn", "Creating HLS-Content - Begin")]
    CreateHlsBegin = 17,

    /// <summary>
    /// Conversion is in state "Creating HLS content - End"
    /// </summary>
    [DescriptionTranslation("Erstelle HLS-Inhalt - Ende", "Creating HLS-Content - End")]
    CreateHlsEnd = 18,

    /// <summary>
    /// Conversion is in state "Creating VTT file - Begin"
    /// </summary>
    [DescriptionTranslation("VTT-Datei erstellen - Beginn", "Creating VTT-File - Begin")]
    CreateVttBegin = 19,

    /// <summary>
    /// Conversion is in state "Creating VTT file - End"
    /// </summary>
    [DescriptionTranslation("VTT-Datei erstellen - Ende", "Creating VTT-File - End")]
    CreateVttEnd = 20,

    /// <summary>
    /// Conversion is in state "Copying converted files to video directory - Begin"
    /// </summary>
    [DescriptionTranslation("Kopiere erstellten Inhalt in Zielverzeichnis - Beginn",
        "Copying created content in target directory - Begin")]
    CopyConvertedBegin = 21,

    /// <summary>
    /// Conversion is in state "Copying converted files to video directory - End"
    /// </summary>
    [DescriptionTranslation("Kopiere erstellten Inhalt in Zielverzeichnis - Ende",
        "Copying created content in target directory - End")]
    CopyConvertedEnd = 22,

    /// <summary>
    /// Conversion is in state "Delete original video file - Begin"
    /// </summary>
    [DescriptionTranslation("Lösche Original-Dateien - Beginn", "Deleting original files - Begin")]
    DeleteOriginalBegin = 23,

    /// <summary>
    /// Conversion is in state "Delete original video file - End"
    /// </summary>
    [DescriptionTranslation("Lösche Original-Dateien - Ende", "Deleting original files - End")]
    DeleteOriginalEnd = 24,

    /// <summary>
    /// Conversion is in state "Delete temporary files - Begin"
    /// </summary>
    [DescriptionTranslation("Lösche temporäre Dateien - Beginn", "Deleting temporary files - Begin")]
    DeleteConvertedBegin = 25,

    /// <summary>
    /// Conversion is in state "Delete temporary files - End"
    /// </summary>
    [DescriptionTranslation("Lösche temporäre Dateien - Ende", "Deleting temporary files - End")]
    DeleteConvertedEnd = 26,

    /// <summary>
    /// Conversion is in state "Convert preview picture - Begin"
    /// </summary>
    [DescriptionTranslation("Konvertiere Vorschau-Bild - Beginn", "Converting preview image - Begin")]
    ConvertPictureBegin = 27,

    /// <summary>
    /// Conversion is in state "Convert preview picture - End"
    /// </summary>
    [DescriptionTranslation("Konvertiere Vorschau-Bild - Ende", "Converting preview image - End")]
    ConvertPictureEnd = 28,

    /// <summary>
    /// Conversion is in state "Successfully converted upload video"
    /// </summary>
    [DescriptionTranslation("Erfolgreich konvertiert", "Successfuly converted")]
    SucessfullyConverted = 29,

    /// <summary>
    /// Conversion is in state "Upload video converted with errors"
    /// </summary>
    [DescriptionTranslation("Konvertiert mit Fehlern", "Conversion would be repeated")]
    ConvertedWithErrors = 30,

    /// <summary>
    /// During conversion a transient error occurs
    /// </summary>
    [DescriptionTranslation("Konvertierung wird wiederholt", "Conversion would be repeated")]
    TransientError = 31
}