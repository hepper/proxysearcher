namespace ProxySearch.Console.Code.Language
{
    public class Language
    {
        public string Name
        {
            get;
            set;
        }

        public string Culture
        {
            get;
            set;
        }

        public override bool Equals(object obj)
        {
            Language language = obj as Language;

            if (language == null)
            {
                return false;
            }

            return Name == language.Name && Culture == language.Culture;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode() ^ Culture.GetHashCode();
        }
    }
}
