using System;

namespace FamilieLaissSharedObjects.Attributes;

public class DescriptionTranslationAttribute : Attribute
{
    public string GermanDescription { get; private set; }
    public string EnglishDescription { get; private set; }

    public DescriptionTranslationAttribute(string germanDescription, string englishDescription)
    {
        GermanDescription = germanDescription;
        EnglishDescription = englishDescription;
    }
}
