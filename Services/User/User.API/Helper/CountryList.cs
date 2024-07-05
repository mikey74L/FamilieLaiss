﻿using System.Collections.Generic;
using User.Domain.Aggregates;

namespace User.API.Helper
{
    public static class CountryList
    {
        public static IEnumerable<Country> GetCountryList()
        {
            yield return new Country("AD", "Andorra", "Andorra");
            yield return new Country("AE", "Vereinigte Arabische Emirate", "United Arab Emirates");
            yield return new Country("AF", "Afghanistan", "Afghanistan");
            yield return new Country("AG", "Antigua und Barbuda", "Antigua and Barbuda");
            yield return new Country("AI", "Anguilla", "Anguilla");
            yield return new Country("AL", "Albanien", "Albania");
            yield return new Country("AM", "Armenien", "Armenia");
            yield return new Country("AO", "Angola", "Angola");
            yield return new Country("AR", "Argentinien", "Argentina");
            yield return new Country("AT", "Österreich", "Austria");
            yield return new Country("AU", "Australien", "Australia");
            yield return new Country("AW", "Aruba", "Aruba");
            yield return new Country("AZ", "Aserbaidschan", "Azerbaijan");
            yield return new Country("BA", "Bosnien und Herzegowina", "Bosnia and Herzegovina");
            yield return new Country("BB", "Barbados", "Barbados");
            yield return new Country("BD", "Bangladesch", "Bangladesh");
            yield return new Country("BE", "Belgien", "Belgium");
            yield return new Country("BF", "Burkina Faso", "Burkina Faso");
            yield return new Country("BG", "Bulgarien", "Bulgaria");
            yield return new Country("BH", "Bahrain", "Bahrain");
            yield return new Country("BI", "Burundi", "Burundi");
            yield return new Country("BJ", "Benin", "Benin");
            yield return new Country("BM", "Bermudas", "Bermuda");
            yield return new Country("BN", "Brunei", "Brunei");
            yield return new Country("BO", "Bolivien", "Bolivia");
            yield return new Country("BQ", "Bonaire", "Bonaire");
            yield return new Country("BR", "Brasilien", "Brazil");
            yield return new Country("BS", "Bahamas", "Bahamas");
            yield return new Country("BT", "Bhutan", "Bhutan");
            yield return new Country("BV", "Bouvetinsel", "Bouvet Island");
            yield return new Country("BW", "Botswana", "Botswana");
            yield return new Country("BY", "Weißrussland", "Belarus");
            yield return new Country("BZ", "Belize", "Belize");
            yield return new Country("CA", "Kanada", "Canada");
            yield return new Country("CD", "Demokratische Republik Kongo", "Democratic Republic Of The Congo");
            yield return new Country("CF", "Zentralafrikanische Republik", "Central African Republic");
            yield return new Country("CG", "Kongo", "Congo");
            yield return new Country("CH", "Schweiz", "Switzerland");
            yield return new Country("CI", "Elfenbeinküste", "Côte d’Ivoire");
            yield return new Country("CK", "Cookinseln", "Cook Islands");
            yield return new Country("CL", "Chile", "Chile");
            yield return new Country("CM", "Kamerun", "Cameroon");
            yield return new Country("CN", "China", "China");
            yield return new Country("CO", "Kolumbien", "Colombia");
            yield return new Country("CR", "Costa Rica", "Costa Rica");
            yield return new Country("CU", "Kuba", "Cuba");
            yield return new Country("CV", "Kap Verde", "Cabo Verde");
            yield return new Country("CW", "Curaçao", "Curaçao");
            yield return new Country("CY", "Zypern", "Cyprus");
            yield return new Country("CZ", "Tschechische Republik", "Czech Republic");
            yield return new Country("DE", "Deutschland", "Germany");
            yield return new Country("DJ", "Dschibuti", "Djibouti");
            yield return new Country("DK", "Dänemark", "Denmark");
            yield return new Country("DM", "Dominica", "Dominica");
            yield return new Country("DO", "Dominikanische Republik", "Dominican Republic");
            yield return new Country("DZ", "Algerien", "Algeria");
            yield return new Country("EC", "Ecuador", "Ecuador");
            yield return new Country("EE", "Estland", "Estonia");
            yield return new Country("EG", "Ägypten", "Egypt");
            yield return new Country("ER", "Eritrea", "Eritrea");
            yield return new Country("ES", "Spanien", "Spain");
            yield return new Country("ET", "Äthiopien", "Ethiopia");
            yield return new Country("FI", "Finnland", "Finland");
            yield return new Country("FJ", "Fidschi", "Fiji");
            yield return new Country("FK", "Falklandinseln", "Falkland Islands");
            yield return new Country("FO", "Färöer", "Faroe Islands");
            yield return new Country("FR", "Frankreich", "France");
            yield return new Country("GA", "Gabun", "Gabon");
            yield return new Country("GB", "Vereinigtes Königreich", "United Kingdom");
            yield return new Country("GD", "Grenada", "Grenada");
            yield return new Country("GE", "Georgien", "Georgia");
            yield return new Country("GG", "Guernsey", "Guernsey");
            yield return new Country("GH", "Ghana", "Ghana");
            yield return new Country("GI", "Gibraltar", "Gibraltar");
            yield return new Country("GL", "Grönland", "Greenland");
            yield return new Country("GM", "Gambia", "Gambia");
            yield return new Country("GN", "Guinea", "Guinea");
            yield return new Country("GQ", "Äquatorialguinea", "Equatorial Guinea");
            yield return new Country("GR", "Griechenland", "Greece");
            yield return new Country("GS", "Südgeorgien und die Südlichen Sandwichinseln", "South Georgia And The South Sandwich Islands");
            yield return new Country("GT", "Guatemala", "Guatemala");
            yield return new Country("GW", "Guinea-Bissau", "Guinea–Bissau");
            yield return new Country("GY", "Guyana", "Guyana");
            yield return new Country("HK", "Hongkong", "The Hong Kong Special Administrative Region Of The People’s Republic Of China");
            yield return new Country("HN", "Honduras", "Honduras");
            yield return new Country("HR", "Kroatien", "Croatia");
            yield return new Country("HT", "Haiti", "Haiti");
            yield return new Country("HU", "Ungarn", "Hungary");
            yield return new Country("ID", "Indonesien", "Indonesia");
            yield return new Country("IE", "Irland", "Ireland");
            yield return new Country("IL", "Israel", "Israel");
            yield return new Country("IM", "Isle of Man", "Isle of Man");
            yield return new Country("IN", "Indien", "India");
            yield return new Country("IQ", "Irak", "Iraq");
            yield return new Country("IR", "Iran", "Islamic Republic Of Iran");
            yield return new Country("IS", "Island", "Iceland");
            yield return new Country("IT", "Italien", "Italy");
            yield return new Country("JE", "Jersey", "Jersey");
            yield return new Country("JM", "Jamaika", "Jamaica");
            yield return new Country("JO", "Jordanien", "Jordan");
            yield return new Country("JP", "Japan", "Japan");
            yield return new Country("KE", "Kenia", "Kenya");
            yield return new Country("KG", "Kirgisistan", "Kyrgyzstan");
            yield return new Country("KH", "Kambodscha", "Cambodia");
            yield return new Country("KI", "Kiribati", "Kiribati");
            yield return new Country("KM", "Komoren", "Comoros");
            yield return new Country("KN", "St. Kitts und Nevis", "Saint Kitts And Nevis");
            yield return new Country("KP", "Nordkorea", "Democratic Poeple’s Republic Of Korea");
            yield return new Country("KR", "Südkorea", "Republic Of Korea");
            yield return new Country("KW", "Kuwait", "Kuwait");
            yield return new Country("KY", "Kaimaninseln", "Cayman Islands");
            yield return new Country("KZ", "Kasachstan", "Kazakhstan");
            yield return new Country("LA", "Laos", "Laos");
            yield return new Country("LB", "Libanon", "Lebanon");
            yield return new Country("LC", "St. Lucia", "Saint Lucia");
            yield return new Country("LI", "Liechtenstein", "Liechtenstein");
            yield return new Country("LK", "Sri Lanka", "Sri Lanka");
            yield return new Country("LR", "Liberia", "Liberia");
            yield return new Country("LS", "Lesotho", "Lesotho");
            yield return new Country("LT", "Litauen", "Lithuania");
            yield return new Country("LU", "Luxemburg", "Luxembourg");
            yield return new Country("LV", "Lettland", "Latvia");
            yield return new Country("LY", "Libyen", "Libya");
            yield return new Country("MA", "Marokko", "Morocco");
            yield return new Country("MC", "Monaco", "Monaco");
            yield return new Country("MD", "Republik Moldau", "Republic Of Moldova");
            yield return new Country("ME", "Montenegro", "Montenegro");
            yield return new Country("MG", "Madagaskar", "Madagascar");
            yield return new Country("MK", "Mazedonien", "Macedonia");
            yield return new Country("ML", "Mali", "Mali");
            yield return new Country("MM", "Myanmar", "Myanmar");
            yield return new Country("MN", "Mongolei", "Mongolia");
            yield return new Country("MO", "Macau", "Macao");
            yield return new Country("MP", "Nördliche Marianen", "Northern Mariana Islands");
            yield return new Country("MR", "Mauretanien", "Mauritania");
            yield return new Country("MS", "Montserrat", "Montserrat");
            yield return new Country("MT", "Malta", "Malta");
            yield return new Country("MU", "Mauritius", "Mauritius");
            yield return new Country("MV", "Malediven", "Maldives");
            yield return new Country("MW", "Malawi", "Malawi");
            yield return new Country("MX", "Mexiko", "Mexico");
            yield return new Country("MY", "Malaysia", "Malaysia");
            yield return new Country("MZ", "Mosambik", "Mozambique");
            yield return new Country("NA", "Namibia", "Namibia");
            yield return new Country("NE", "Niger", "Niger");
            yield return new Country("NG", "Nigeria", "Nigeria");
            yield return new Country("NI", "Nicaragua", "Nicaragua");
            yield return new Country("NL", "Niederlande", "Netherlands");
            yield return new Country("NO", "Norwegen", "Norway");
            yield return new Country("NP", "Nepal", "Nepal");
            yield return new Country("NR", "Nauru", "Nauru");
            yield return new Country("NZ", "Neuseeland", "New Zealand");
            yield return new Country("OM", "Oman", "Oman");
            yield return new Country("PA", "Panama", "Panama");
            yield return new Country("PE", "Peru", "Peru");
            yield return new Country("PG", "Papua-Neuguinea", "Papua New Guinea");
            yield return new Country("PH", "Philippinen", "Philippines");
            yield return new Country("PK", "Pakistan", "Pakistan");
            yield return new Country("PL", "Polen", "Poland");
            yield return new Country("PT", "Portugal", "Portugal");
            yield return new Country("PW", "Palau", "Palau");
            yield return new Country("PY", "Paraguay", "Paraguay");
            yield return new Country("QA", "Katar", "Qatar");
            yield return new Country("RO", "Rumänien", "Romania");
            yield return new Country("RS", "Serbien", "Serbia");
            yield return new Country("RU", "Russland", "Russian Federation");
            yield return new Country("RW", "Ruanda", "Rwanda");
            yield return new Country("SA", "Saudi-Arabien", "Saudi Arabia");
            yield return new Country("SB", "Salomonen", "Solomon Islands");
            yield return new Country("SC", "Seychellen", "Seychelles");
            yield return new Country("SD", "Sudan", "Sudan");
            yield return new Country("SE", "Schweden", "Sweden");
            yield return new Country("SG", "Singapur", "Singapore");
            yield return new Country("SH", "St. Helena, Ascension und Tristan da Cunha", "Saint Helena, Ascension And Tristan Da Cunha");
            yield return new Country("SI", "Slowenien", "Slovenia");
            yield return new Country("SK", "Slowakei", "Slovakia");
            yield return new Country("SL", "Sierra Leone", "Sierra Leone");
            yield return new Country("SM", "San Marino", "San Marino");
            yield return new Country("SN", "Senegal", "Senegal");
            yield return new Country("SO", "Somalia", "Somalia");
            yield return new Country("SR", "Suriname", "Suriname");
            yield return new Country("SS", "Südsudan", "South Sudan");
            yield return new Country("SV", "El Salvador", "El Salvador");
            yield return new Country("SX", "Sint Maarten", "Sint Maarten (Dutch part)");
            yield return new Country("SY", "Syrien", "Syria");
            yield return new Country("TC", "Turks- und Caicosinseln", "Turks And Caicos Islands");
            yield return new Country("TD", "Tschad", "Chad");
            yield return new Country("TG", "Togo", "Togo");
            yield return new Country("TH", "Thailand", "Thailand");
            yield return new Country("TJ", "Tadschikistan", "Tajikistan");
            yield return new Country("TL", "Osttimor", "Timor–Leste");
            yield return new Country("TM", "Turkmenistan", "Turkmenistan");
            yield return new Country("TN", "Tunesien", "Tunisia");
            yield return new Country("TO", "Tonga", "Tonga");
            yield return new Country("TR", "Türkei", "Turkey");
            yield return new Country("TT", "Trinidad und Tobago", "Trinidad And Tobago");
            yield return new Country("TV", "Tuvalu", "Tuvalu");
            yield return new Country("TW", "Taiwan", "Taiwan Province Of China");
            yield return new Country("TZ", "Tansania", "Tanzania");
            yield return new Country("UA", "Ukraine", "Ukraine");
            yield return new Country("UG", "Uganda", "Uganda");
            yield return new Country("US", "Vereinigte Staaten von Amerika", "United States");
            yield return new Country("UY", "Uruguay", "Uruguay");
            yield return new Country("UZ", "Usbekistan", "Uzbekistan");
            yield return new Country("VA", "Vatikanstadt", "Vatican City");
            yield return new Country("VC", "St. Vincent und die Grenadinen", "Saint Vincent and the Grenadines");
            yield return new Country("VE", "Venezuela", "Venezuela");
            yield return new Country("VG", "Britische Jungferninseln", "British Virgin Islands");
            yield return new Country("VN", "Vietnam", "Vietnam");
            yield return new Country("VU", "Vanuatu", "Vanuatu");
            yield return new Country("WS", "Samoa", "Samoa");
            yield return new Country("YE", "Jemen", "Yemen");
            yield return new Country("ZA", "Südafrika", "South Africa");
            yield return new Country("ZM", "Sambia", "Zambia");
            yield return new Country("ZW", "Simbabwe", "Zimbabwe");
        }
    }
}