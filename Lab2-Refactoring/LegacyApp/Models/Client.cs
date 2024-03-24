using LegacyApp.Enums;

namespace LegacyApp
{
    public class Client : Person
    {
        public string Name { get; internal set; }
        public int ClientId { get; internal set; }
        public string Address { get; internal set; }
        public ClientType Type { get; set; }
    }
}