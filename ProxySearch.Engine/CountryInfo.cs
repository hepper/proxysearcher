namespace ProxySearch.Engine
{
    public class CountryInfo
    {
        public static readonly CountryInfo Empty = new CountryInfo
        {
            Code = string.Empty,
            Name = string.Empty
        };

        public string Code { get; set; }
        public string Name { get; set; }
    }
}