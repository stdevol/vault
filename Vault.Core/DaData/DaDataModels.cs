using System;
using Vault.Helpers;

namespace Vault.Core.DaData
{
    #region enums

    public enum PartyBranchType
    {
        Main,
        Branch
    }

    public enum GenderType
    {
        Female,
        Male,
        Unknown
    }

    public enum PartyStatus
    {
        Active,
        Liquidating,
        Liquidated,
        Reorganizing
    }

    public enum PartyType
    {
        Legal,
        Individual
    }

    public enum FounderType
    {
        Legal,
        Physical
    }

    public enum ManagerType
    {
        Employee,
        Foreigner,
        Legal
    }

    #endregion

    [PublicContract]
    public abstract class Suggestion
    {
        public string Value { get; set; }
        public string UnrestrictedValue { get; set; }

        public override string ToString() => Value;
    }


    [PublicContract]
    public class SuggestPartyResponse
    {
        public DaDataOrganization[] Suggestions { get; set; }
    }

    [PublicContract]
    public class DaDataOrganization : Suggestion
    {
        public const int CurrentVersion = 2;
        public int Version { get; set; }
        public DateTime Date { get; protected set; }

        public string Id { get; set; }
        public PartyData Data { get; set; }

        public DaDataOrganization()
        {
            Version = CurrentVersion;
            Date = DateTime.UtcNow;
        }
    }

    [PublicContract]
    public class AddressData
    {
        public string Source { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }

        public string RegionFiasId { get; set; }
        public string RegionKladrId { get; set; }
        public string RegionWithType { get; set; }
        public string RegionType { get; set; }
        public string RegionTypeFull { get; set; }
        public string Region { get; set; }

        public string AreaFiasId { get; set; }
        public string AreaKladrId { get; set; }
        public string AreaWithType { get; set; }
        public string AreaType { get; set; }
        public string AreaTypeFull { get; set; }
        public string Area { get; set; }

        public string CityFiasId { get; set; }
        public string CityKladrId { get; set; }
        public string CityWithType { get; set; }
        public string CityType { get; set; }
        public string CityTypeFull { get; set; }
        public string City { get; set; }

        public string CityArea { get; set; }

        public string CityDistrictFiasId { get; set; }
        public string CityDistrictKladrId { get; set; }
        public string CityDistrictWithType { get; set; }
        public string CityDistrictType { get; set; }
        public string CityDistrictTypeFull { get; set; }
        public string CityDistrict { get; set; }

        public string SettlementFiasId { get; set; }
        public string SettlementKladrId { get; set; }
        public string SettlementWithType { get; set; }
        public string SettlementType { get; set; }
        public string SettlementTypeFull { get; set; }
        public string Settlement { get; set; }

        public string StreetFiasId { get; set; }
        public string StreetKladrId { get; set; }
        public string StreetWithType { get; set; }
        public string StreetType { get; set; }
        public string StreetTypeFull { get; set; }
        public string Street { get; set; }

        public string HouseFiasId { get; set; }
        public string HouseKladrId { get; set; }
        public string HouseType { get; set; }
        public string HouseTypeFull { get; set; }
        public string House { get; set; }

        public string BlockType { get; set; }
        public string BlockTypeFull { get; set; }
        public string Block { get; set; }

        public string FlatType { get; set; }
        public string FlatTypeFull { get; set; }
        public string Flat { get; set; }
        public string FlatArea { get; set; }
        public string SquareMeterPrice { get; set; }
        public string FlatPrice { get; set; }

        public string PostalBox { get; set; }
        public string FiasId { get; set; }
        public string FiasLevel { get; set; }
        public string KladrId { get; set; }
        public string CapitalMarker { get; set; }

        public string Okato { get; set; }
        public string Oktmo { get; set; }
        public string TaxOffice { get; set; }
        public string TaxOfficeLegal { get; set; }

        public string Timezone { get; set; }

        public string GeoLat { get; set; }
        public string GeoLon { get; set; }
        public string QcGeo { get; set; }

        public string BeltwayHit { get; set; }
        public string BeltwayDistance { get; set; }

        public string[] HistoryValues { get; set; }

        public MetroData[] Metro { get; set; }
    }

    [PublicContract]
    public class MetroData
    {
        public string Name { get; set; }
        public string Line { get; set; }
        public decimal Distance { get; set; }
    }

    [PublicContract]
    public class PartyData
    {
        public SuggestAddressResponse.DaDataAddress Address { get; set; }

        public string BranchCount { get; set; }
        public PartyBranchType BranchType { get; set; }

        public string Inn { get; set; }
        public string Kpp { get; set; }
        public string Ogrn { get; set; }
        public long? OgrnDate { get; set; }
        public string Hid { get; set; }

        public PartyManagementData Management { get; set; }
        public PartyNameData Name { get; set; }

        public string Okpo { get; set; }
        public string Okved { get; set; }
        public string OkvedType { get; set; }
        public PartyOpfData Opf { get; set; }
        public PartyStateData State { get; set; }
        public PartyType Type { get; set; }

        public DaDataOkved[] Okveds { get; set; }
        public Authorities Authorities { get; set; }

        public Citizenship Citizenship { get; set; }

        public Founder[] Founders { get; set; }
        public Manager[] Managers { get; set; }
        public Capital Capital { get; set; }

        public Documents Documents { get; set; }
        public License[] Licenses { get; set; }
    }

    [PublicContract]
    public class SuggestAddressResponse
    {
        public class DaDataAddress : Suggestion
        {
            public AddressData Data { get; set; }
        }

        public DaDataAddress[] Suggestions { get; set; }
    }

    [PublicContract]
    public class PartyManagementData
    {
        public string Name { get; set; }
        public string Post { get; set; }
    }

    [PublicContract]
    public class PartyNameData
    {
        public string FullWithOpf { get; set; }
        public string ShortWithOpf { get; set; }
        public string Latin { get; set; }
        public string Full { get; set; }
        public string Short { get; set; }
    }

    [PublicContract]
    public class PartyOpfData
    {
        public string Code { get; set; }
        public string Full { get; set; }
        public string Short { get; set; }
        public string Type { get; set; }

    }

    [PublicContract]
    public class PartyStateData
    {
        public long ActualityDate { get; set; }
        public long? RegistrationDate { get; set; }
        public long? LiquidationDate { get; set; }
        public PartyStatus Status { get; set; }
    }

    [PublicContract]
    public class DaDataOkved
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public bool Main { get; set; }
    }

    [PublicContract]
    public class Authority
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Address { get; set; }
    }

    [PublicContract]
    public class Authorities
    {
        public Authority FtsRegistration { get; set; }
        public Authority FtsReport { get; set; }
        public Authority Pf { get; set; }
        public Authority Sif { get; set; }
    }

    [PublicContract]
    public class Citizenship
    {
        public string Numeric { get; set; }
        public string Alpha3 { get; set; }
        public string Full { get; set; }
        public string Short { get; set; }
    }

    [PublicContract]
    public class Document
    {
        public string Type { get; set; }
        public string Series { get; set; }
        public string Number { get; set; }
        public long IssueDate { get; set; }
        public string IssueAuthority { get; set; }
    }

    [PublicContract]
    public class Documents
    {
        public Document FtsRegistration { get; set; }
        public Document PfRegistration { get; set; }
        public Document SifRegistration { get; set; }
    }

    [PublicContract]
    public class License
    {
        public string Series { get; set; }
        public string Number { get; set; }
        public long IssueDate { get; set; }
        public string IssueAuthority { get; set; }
        public long? SuspendDate { get; set; }
        public string SuspendAuthority { get; set; }
        public long ValidFrom { get; set; }
        public long? ValidTo { get; set; }
        public string[] Activities { get; set; }
        public string[] Addresses { get; set; }
    }

    [PublicContract]
    public class Capital
    {
        public string Type { get; set; }
        public string Value { get; set; }
    }

    [PublicContract]
    public abstract class ManagerBase
    {
        public string Ogrn { get; set; }
        public string Inn { get; set; }
        public string Name { get; set; }
        public Fio Fio { get; set; }
        public string Hid { get; set; }
    }

    [PublicContract]
    public class Founder : ManagerBase
    {
        public FounderType Type { get; set; }
    }

    [PublicContract]
    public class Manager : ManagerBase
    {
        public ManagerType Type { get; set; }

        public string Post { get; set; }
    }

    [PublicContract]

    public class Fio
    {
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Patronymic { get; set; }
        public GenderType Gender { get; set; }
    }
}