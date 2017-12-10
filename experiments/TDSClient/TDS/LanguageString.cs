using System;

namespace Microsoft.SqlServer.TDS
{
    /// <summary>
    /// Class that contains langauge names and can convert between enumeration and strings
    /// </summary>
    public static class LanguageString
    {
        private const string English = "us_english";
        private const string German = "Deutsch";
        private const string French = "Français";
        private const string Japanese = "日本語";
        private const string Danish = "Dansk";
        private const string Spanish = "Españo";
        private const string Italian = "Italiano";
        private const string Dutch = "Nederlands";
        private const string Norwegian = "Norsk";
        private const string Portuguese = "Português";
        private const string Finnish = "Suomi";
        private const string Swedish = "Svenska";
        private const string Czech = "čeština";
        private const string Hungarian = "magyar";
        private const string Polish = "polski";
        private const string Romanian = "română";
        private const string Croatian = "hrvatski";
        private const string Slovak = "slovenčina";
        private const string Slovenian = "slovenski";
        private const string Greek = "ελληνικά";
        private const string Bulgarian = "български";
        private const string Russian = "русский";
        private const string Turkish = "Türkçe";
        private const string BritishEnglish = "British";
        private const string Estonian = "eesti";
        private const string Latvian = "latviešu";
        private const string Lithuanian = "lietuvių";
        private const string Brazilian = "Português (Brasil)";
        private const string TraditionalChinese = "繁體中文";
        private const string Korean = "한국어";
        private const string SimplifiedChinese = "简体中文";
        private const string Arabic = "Arabic";
        private const string Thai = "ไทย";
        private const string Bokmal = "norsk (bokmål)";
        
        /// <summary>
        /// Convert a language to enumeration
        /// </summary>
        public static LanguageType ToEnum(string value)
        {
	        // Check every langauge
	        if (string.Compare(value, English, true) == 0)
	        {
		        return LanguageType.English;
	        }
	        else if (string.Compare(value, German, true) == 0)
	        {
		        return LanguageType.German;
	        }
	        else if (string.Compare(value, French, true) == 0)
	        {
		        return LanguageType.French;
	        }
	        else if (string.Compare(value, Japanese, true) == 0)
	        {
		        return LanguageType.Japanese;
	        }
	        else if (string.Compare(value, Danish, true) == 0)
	        {
		        return LanguageType.Danish;
	        }
	        else if (string.Compare(value, Spanish, true) == 0)
	        {
		        return LanguageType.Spanish;
	        }
	        else if (string.Compare(value, Italian, true) == 0)
	        {
		        return LanguageType.Italian;
	        }
	        else if (string.Compare(value, Dutch, true) == 0)
	        {
		        return LanguageType.Dutch;
	        }
	        else if (string.Compare(value, Norwegian, true) == 0)
	        {
		        return LanguageType.Norwegian;
	        }
	        else if (string.Compare(value, Portuguese, true) == 0)
	        {
		        return LanguageType.Portuguese;
	        }
	        else if (string.Compare(value, Finnish, true) == 0)
	        {
		        return LanguageType.Finnish;
	        }
	        else if (string.Compare(value, Swedish, true) == 0)
	        {
		        return LanguageType.Swedish;
	        }
	        else if (string.Compare(value, Czech, true) == 0)
	        {
		        return LanguageType.Czech;
	        }
	        else if (string.Compare(value, Hungarian, true) == 0)
	        {
		        return LanguageType.Hungarian;
	        }
	        else if (string.Compare(value, Polish, true) == 0)
	        {
		        return LanguageType.Polish;
	        }
	        else if (string.Compare(value, Romanian, true) == 0)
	        {
		        return LanguageType.Romanian;
	        }
	        else if (string.Compare(value, Croatian, true) == 0)
	        {
		        return LanguageType.Croatian;
	        }
	        else if (string.Compare(value, Slovak, true) == 0)
	        {
		        return LanguageType.Slovak;
	        }
	        else if (string.Compare(value, Slovenian, true) == 0)
	        {
		        return LanguageType.Slovenian;
	        }
	        else if (string.Compare(value, Greek, true) == 0)
	        {
		        return LanguageType.Greek;
	        }
	        else if (string.Compare(value, Bulgarian, true) == 0)
	        {
		        return LanguageType.Bulgarian;
	        }
	        else if (string.Compare(value, Russian, true) == 0)
	        {
		        return LanguageType.Russian;
	        }
	        else if (string.Compare(value, Turkish, true) == 0)
	        {
		        return LanguageType.Turkish;
	        }
	        else if (string.Compare(value, BritishEnglish, true) == 0)
	        {
		        return LanguageType.BritishEnglish;
	        }
	        else if (string.Compare(value, Estonian, true) == 0)
	        {
		        return LanguageType.Estonian;
	        }
	        else if (string.Compare(value, Latvian, true) == 0)
	        {
		        return LanguageType.Latvian;
	        }
	        else if (string.Compare(value, Lithuanian, true) == 0)
	        {
		        return LanguageType.Lithuanian;
	        }
	        else if (string.Compare(value, Brazilian, true) == 0)
	        {
		        return LanguageType.Brazilian;
	        }
	        else if (string.Compare(value, TraditionalChinese, true) == 0)
	        {
		        return LanguageType.TraditionalChinese;
	        }
	        else if (string.Compare(value, Korean, true) == 0)
	        {
		        return LanguageType.Korean;
	        }
	        else if (string.Compare(value, SimplifiedChinese, true) == 0)
	        {
		        return LanguageType.SimplifiedChinese;
	        }
	        else if (string.Compare(value, Arabic, true) == 0)
	        {
		        return LanguageType.Arabic;
	        }
	        else if (string.Compare(value, Thai, true) == 0)
	        {
		        return LanguageType.Thai;
	        }
	        else if (string.Compare(value, Bokmal, true) == 0)
	        {
		        return LanguageType.Bokmal;
	        }

	        // Unknown value
	        throw new Exception("Unrecognized language string \"" + value + "\"");
        }

        /// <summary>
        /// Convert enumeration to string
        /// </summary>
        public static string ToString(LanguageType value)
        {
	        // Switch through the langauges
	        switch (value)
	        {
	        case LanguageType.English:
		        {
			        return English;
		        }
	        case LanguageType.German:
		        {
			        return German;
		        }
	        case LanguageType.French:
		        {
			        return French;
		        }
	        case LanguageType.Japanese:
		        {
			        return Japanese;
		        }
	        case LanguageType.Danish:
		        {
			        return Danish;
		        }
	        case LanguageType.Spanish:
		        {
			        return Spanish;
		        }
	        case LanguageType.Italian:
		        {
			        return Italian;
		        }
	        case LanguageType.Dutch:
		        {
			        return Dutch;
		        }
	        case LanguageType.Norwegian:
		        {
			        return Norwegian;
		        }
	        case LanguageType.Portuguese:
		        {
			        return Portuguese;
		        }
	        case LanguageType.Finnish:
		        {
			        return Finnish;
		        }
	        case LanguageType.Swedish:
		        {
			        return Swedish;
		        }
	        case LanguageType.Czech:
		        {
			        return Czech;
		        }
	        case LanguageType.Hungarian:
		        {
			        return Hungarian;
		        }
	        case LanguageType.Polish:
		        {
			        return Polish;
		        }
	        case LanguageType.Romanian:
		        {
			        return Romanian;
		        }
	        case LanguageType.Croatian:
		        {
			        return Croatian;
		        }
	        case LanguageType.Slovak:
		        {
			        return Slovak;
		        }
	        case LanguageType.Slovenian:
		        {
			        return Slovenian;
		        }
	        case LanguageType.Greek:
		        {
			        return Greek;
		        }
	        case LanguageType.Bulgarian:
		        {
			        return Bulgarian;
		        }
	        case LanguageType.Russian:
		        {
			        return Russian;
		        }
	        case LanguageType.Turkish:
		        {
			        return Turkish;
		        }
	        case LanguageType.BritishEnglish:
		        {
			        return BritishEnglish;
		        }
	        case LanguageType.Estonian:
		        {
			        return Estonian;
		        }
	        case LanguageType.Latvian:
		        {
			        return Latvian;
		        }
	        case LanguageType.Lithuanian:
		        {
			        return Lithuanian;
		        }
	        case LanguageType.Brazilian:
		        {
			        return Brazilian;
		        }
	        case LanguageType.TraditionalChinese:
		        {
			        return TraditionalChinese;
		        }
	        case LanguageType.Korean:
		        {
			        return Korean;
		        }
	        case LanguageType.SimplifiedChinese:
		        {
			        return SimplifiedChinese;
		        }
	        case LanguageType.Arabic:
		        {
			        return Arabic;
		        }
	        case LanguageType.Thai:
		        {
			        return Thai;
		        }
	        case LanguageType.Bokmal:
		        {
			        return Bokmal;
		        }
	        }

	        // Unknown value
            throw new Exception("Unrecognized language type \"" + value.ToString() + "\"");
        }
    }
}
