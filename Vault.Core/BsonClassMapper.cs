using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using Vault.Core.DaData;
using Vault.Core.Organizations;

namespace Vault.Core
{
    public static class BsonClassMapper
    {
        public static void Tune()
        {
            BsonSerializer.RegisterSerializer(new EnumSerializer<OrgType>(BsonType.String));
            BsonSerializer.RegisterSerializer(new EnumSerializer<PartyBranchType>(BsonType.String));
            BsonSerializer.RegisterSerializer(new EnumSerializer<PartyStatus>(BsonType.String));
            BsonSerializer.RegisterSerializer(new EnumSerializer<PartyType>(BsonType.String));
            BsonSerializer.RegisterSerializer(new EnumSerializer<FounderType>(BsonType.String));
            BsonSerializer.RegisterSerializer(new EnumSerializer<ManagerType>(BsonType.String));


            BsonClassMap.RegisterClassMap<Organization>(cm =>
            {
                cm.AutoMap();

                cm.MapProperty(c => c.Search);
                cm.MapIdProperty(x => x.Id)
                    .SetIgnoreIfDefault(true)
                    .SetSerializer(new StringSerializer(BsonType.ObjectId));
            });

            BsonClassMap.RegisterClassMap<DaDataOrganization>(cm =>
            {
                cm.AutoMap();

                cm.MapIdProperty(x => x.Id)
                    .SetIgnoreIfDefault(true)
                    .SetSerializer(new StringSerializer(BsonType.ObjectId));
            });
        }
    }
}
