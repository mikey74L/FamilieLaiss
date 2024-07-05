using DomainHelper.AbstractClasses;
using DomainHelper.Exceptions;
using HotChocolate;
using System.Threading.Tasks;

namespace Blog.Domain.Entities;

/// <summary>
/// Entity for blog entry
/// </summary>
// [GraphQLDescription("Blog item")]
// public class BlogEntry : EntityModify<long>
// {
//     #region Properties
//     /// <summary>
//     /// German header text for this Blog-Entry
//     /// </summary>
//     [GraphQLDescription("German header for this blog item")]
//     [GraphQLNonNullType]
//     public string HeaderGerman { get; private set; }
//
//     /// <summary>
//     /// English header text for this Blog-Entry
//     /// </summary>
//     [GraphQLDescription("English header for this blog item")]
//     [GraphQLNonNullType]
//     public string HeaderEnglish { get; private set; }
//
//     /// <summary>
//     /// German text for this Blog-Entry
//     /// </summary>
//     [GraphQLDescription("German text for this blog item")]
//     [GraphQLNonNullType]
//     public string TextGerman { get; private set; }
//
//     /// <summary>
//     /// English text for this Blog-Entry
//     /// </summary>
//     [GraphQLDescription("English text for this blog item")]
//     [GraphQLNonNullType]
//     public string TextEnglish { get; private set; }
//     #endregion
//
//     #region C'tor
//     protected BlogEntry()
//     {
//
//     }
//
//     /// <summary>
//     /// C'tor
//     /// </summary>
//     /// <param name="headerGerman">German header text for this Blog-Entry</param>
//     /// <param name="headerEnglish">English header text for this Blog-Entry</param>
//     /// <param name="textGerman">German text for this Blog-Entry</param>
//     /// <param name="textEnglish">English text for this Blog-Entry</param>
//     public BlogEntry(string headerGerman, string headerEnglish, string textGerman, string textEnglish)
//     {
//         //Überprüfen ob ein deutscher Header vorhanden ist
//         if (string.IsNullOrEmpty(headerGerman)) throw new DomainException("A german header is required");
//
//         //Überprüfen ob ein englischer Header vorhanden ist
//         if (string.IsNullOrEmpty(headerEnglish)) throw new DomainException("A english header is required");
//
//         //Überprüfen ob ein deutscher Header vorhanden ist
//         if (string.IsNullOrEmpty(textGerman)) throw new DomainException("A german text is required");
//
//         //Überprüfen ob ein deutscher Header vorhanden ist
//         if (string.IsNullOrEmpty(textEnglish)) throw new DomainException("A english text is required");
//
//         //Übernehmen der Werte
//         HeaderGerman = headerGerman;
//         HeaderEnglish = headerEnglish;
//         TextGerman = textGerman;
//         TextEnglish = textEnglish;
//     }
//     #endregion
//
//     #region Domain Methods
//     /// <summary>
//     /// Update the blog entry
//     /// </summary>
//     /// <param name="headerGerman">German header text for this Blog-Entry</param>
//     /// <param name="headerEnglish">English header text for this Blog-Entry</param>
//     /// <param name="textGerman">German text for this Blog-Entry</param>
//     /// <param name="textEnglish">English text for this Blog-Entry</param>
//     [GraphQLIgnore]
//     public void Update(string headerGerman, string headerEnglish, string textGerman, string textEnglish)
//     {
//         //Überprüfen ob ein deutscher Header vorhanden ist
//         if (string.IsNullOrEmpty(headerGerman)) throw new DomainException("A german header is required");
//
//         //Überprüfen ob ein englischer Header vorhanden ist
//         if (string.IsNullOrEmpty(headerEnglish)) throw new DomainException("A english header is required");
//
//         //Überprüfen ob ein deutscher Header vorhanden ist
//         if (string.IsNullOrEmpty(textGerman)) throw new DomainException("A german text is required");
//
//         //Überprüfen ob ein deutscher Header vorhanden ist
//         if (string.IsNullOrEmpty(textEnglish)) throw new DomainException("A english text is required");
//
//         //Übernehmen der Werte
//         HeaderGerman = headerGerman;
//         HeaderEnglish = headerEnglish;
//         TextGerman = textGerman;
//         TextEnglish = textEnglish;
//     }
//     #endregion
// }
