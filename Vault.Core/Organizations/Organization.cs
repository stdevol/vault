using System;
using System.Linq;
using AutoMapper;
using Vault.Core.DaData;
using Vault.Helpers;

namespace Vault.Core.Organizations
{
    [PublicContract]
    public class Organization
    {
        public const int CurrentVersion = 2;
        public int Version { get; set; }
        public DateTime Date { get; protected set; }

        public string Id { get; set; }
        public string Hid { get; set; }
        public OrgType OrgType { get; set; }
        public string FullName { get; set; }
        public string ShortName { get; set; }
        public string Inn { get; set; }
        public string Ogrn { get; set; }
        public DateTime? RegisterDate { get; set; }
        public string Kpp { get; set; }
        public string Okpo { get; set; }
        public Address JuridicalAddress { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Ceo { get; set; }
        public string CeoPost { get; set; }
        public Okved[] Okveds { get; set; } = Array.Empty<Okved>();

        public PartyBranchType BranchType { get; set; }

        public PartyStatus Status { get; set; }

        public string Search => $"{Inn} {Ogrn} {FullName} {JuridicalAddress?.Area}, {JuridicalAddress?.Street}";

        public Organization()
        {
            Version = CurrentVersion;
            Date = DateTime.UtcNow;
        }
    }

    public class Okved
    {
        public string Code { get; set; }
        public bool IsMain { get; set; }
    }

    internal class OrganizationProfile : Profile
    {
        public OrganizationProfile()
        {
            CreateMap<PartyData, Organization>()
                .ForMember(dest => dest.OrgType, opt => opt.MapFrom(src => src.Type == PartyType.Legal ? OrgType.Juridical : OrgType.IndividualEntrepreneur))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.State.Status))
                .ForMember(dest => dest.Hid, opt => opt.MapFrom(src => src.Hid))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Name.FullWithOpf))
                .ForMember(dest => dest.ShortName, opt => opt.MapFrom(src => src.Name.ShortWithOpf))
                .ForMember(dest => dest.Inn, opt => opt.MapFrom(src => src.Inn))
                .ForMember(dest => dest.Kpp, opt => opt.MapFrom(src => src.Kpp))
                .ForMember(dest => dest.Ogrn, opt => opt.MapFrom(src => src.Ogrn))
                .ForMember(dest => dest.RegisterDate, opt => opt.MapFrom(src => UnixTimeToDate(src.OgrnDate)))
                .ForMember(dest => dest.Okpo, opt => opt.MapFrom(src => src.Okpo))
                .ForMember(dest => dest.Ceo, opt => opt.MapFrom(src => src.Management != null ? src.Management.Name : null))
                .ForMember(dest => dest.CeoPost, opt => opt.MapFrom(src => src.Management != null ? src.Management.Post : null))
                .ForMember(dest => dest.Okveds, opt => opt.MapFrom(src => src.OkvedType == "2014" ? src.Okveds : null))
                .ForMember(dest => dest.JuridicalAddress, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.BranchType, opt => opt.MapFrom(src => src.BranchType));


            CreateMap<SuggestAddressResponse.DaDataAddress, Address>()
                .ForMember(dest => dest.Index, opt => opt.MapFrom(src => src.Data != null ? src.Data.PostalCode : null))
                .ForMember(dest => dest.Oktmo, opt => opt.MapFrom(src => src.Data != null ? src.Data.Oktmo : null))
                .ForMember(
                    dest => dest.Area,
                    opt => opt.MapFrom(src => src.Data != null ? GetAreaAddress(src.Data) : null))
                .ForMember(
                    dest => dest.Street,
                    opt => opt.MapFrom(src => src.Data != null ? GetStreetAddress(src.Data) : src.Value));

            CreateMap<DaDataOkved, Okved>()
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code))
                .ForMember(dest => dest.IsMain, opt => opt.MapFrom(src => src.Main));
        }

        private static string GetAreaAddress(AddressData data)
        {
            var strings = new[]
            {
                data.RegionWithType,
                data.AreaWithType,
                data.CityWithType == data.RegionWithType ? null : data.CityWithType,
                data.SettlementWithType
            };

            return string.Join(", ", strings.Where(s => !string.IsNullOrWhiteSpace(s)));
        }

        private static string GetStreetAddress(AddressData data)
        {
            var strings = new[]
            {
                data.StreetWithType,
                $"{data.HouseType} {data.House}",
                $"{data.BlockType} {data.Block}",
                $"{data.FlatType} {data.Flat}",
            };

            return string.Join(", ", strings.Where(s => !string.IsNullOrWhiteSpace(s)));
        }

        private static DateTime? UnixTimeToDate(long? unixTime)
        {
            return unixTime == null
                ? null
                : (DateTime?)DateTimeOffset.FromUnixTimeMilliseconds((long)unixTime).UtcDateTime;
        }
    }
}