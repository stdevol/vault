using Vault.Helpers;

namespace Vault.Core.Organizations
{
    [PublicContract]
    public class Address
    {
        public string Oktmo { get; set; }
        public string Index { get; set; }

        /// <summary>
        /// Район, населенный пункт 
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        /// Улица, номер дома, корпус (строение) 
        /// </summary>
        public string Street { get; set; }
    }
}