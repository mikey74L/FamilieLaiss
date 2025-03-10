using FamilieLaissSharedObjects.Attributes;

namespace FamilieLaissSharedObjects.Enums;

/// <summary>
/// Enumeration for the current conversion state of an upload picture
/// </summary>
public enum EnumPictureConvertStatus : byte
{
    /// <summary>
    /// Conversion is in state "Waiting for Conversion"
    /// </summary>
    [DescriptionTranslation("Warte auf Konvertierung", "Waiting for conversion")]
    WaitingForConversion = 0,
    /// <summary>
    /// Conversion is in state "Read Picture Info from file - Begin"
    /// </summary>
    [DescriptionTranslation("Info auslesen - Beginn", "Read info - Begin")]
    ReadInfoBegin = 1,
    /// <summary>
    /// Conversion is in state "Read Picture Info from file - End"
    /// </summary>
    [DescriptionTranslation("Info auslesen - Ende", "Read info - End")]
    ReadInfoEnd = 2,
    /// <summary>
    /// Conversion is in state "Read Exif-Information from file - Begin"
    /// </summary>
    [DescriptionTranslation("Exif auslesen - Beginn", "Read Exif - Begin")]
    ReadExifBegin = 3,
    /// <summary>
    /// Conversion is in state "Read Exif-Information from file - End"
    /// </summary>
    [DescriptionTranslation("Exif auslesen - Ende", "Read Exif - End")]
    ReadExifEnd = 4,
    /// <summary>
    /// Conversion is in state "Convert picture - Begin"
    /// </summary>
    [DescriptionTranslation("Konvertierung - Beginn", "Conversion - Begin")]
    ConvertBegin = 5,
    /// <summary>
    /// Conversion is in state "Convert picture - End"
    /// </summary>
    [DescriptionTranslation("Konvertierung - Ende", "Conversion - End")]
    ConvertEnd = 6,
    /// <summary>
    /// Conversion is in state "Upload to cloud - Begin"
    /// </summary>
    [DescriptionTranslation("Speichern in Cloud - Beginn", "Upload to cloud - Begin")]
    UploadPictureToBlobBegin = 7,
    /// <summary>
    /// Conversion is in state "Upload to cloud - End"
    /// </summary>
    [DescriptionTranslation("Speichern in Cloud - Ende", "Upload to cloud - End")]
    UploadPictureToBlobEnd = 8,
    /// <summary>
    /// Conversion is in state "Delete physical files - Begin"
    /// </summary>
    [DescriptionTranslation("Löschen der physikalischen Dateien - Beginn", "Delete physical files - Begin")]
    DeleteFilesBegin = 9,
    /// <summary>
    /// Conversion is in state "Delete physical files - End"
    /// </summary>
    [DescriptionTranslation("Löschen der physikalischen Dateien - Ende", "Delete physical files - End")]
    DeleteFilesEnd = 10,    /// <summary>
    /// Conversion is in state "Successfully converted upload picture"
    /// </summary>
    [DescriptionTranslation("Erfolgreich konvertiert", "Successfully converted")]
    SucessfullyConverted = 11,
    /// <summary>
    /// Conversion is in state "Upload picture converted with errors"
    /// </summary>
    [DescriptionTranslation("Konvertiert mit Fehlern", "Converted with errors")]
    ConvertedWithErrors = 12,
    /// <summary>
    /// During conversion a transient error occurs
    /// </summary>
    [DescriptionTranslation("Konvertierung wird wiederholt", "")]
    TransientError = 13
}
