namespace ProxySearch.Console.Code.GoogleAnalytics.Timing
{
    public class TimingDictionaryKey
    {
        public TimingCategory Category { get; set; }
        public TimingVariable Variable { get; set; }

        public override bool Equals(object obj)
        {
            TimingDictionaryKey data = obj as TimingDictionaryKey;

            if (data == null)
                return false;

            return data.Category == Category && data.Variable == Variable;
        }

        public override int GetHashCode()
        {
            return Category.GetHashCode() ^ Variable.GetHashCode();
        }
    }
}
