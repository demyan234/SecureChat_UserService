using Google.Protobuf.WellKnownTypes;
using NodaTime;
using NodaTime.Extensions;

namespace SecureChatUserMicroService.Application.Application.Extensions
{
    public static class ProtobufExtensions
    {
        public static Instant ToInstant(this Timestamp timestamp) => 
            timestamp?.ToDateTimeOffset().ToInstant() ?? Instant.FromUnixTimeSeconds(0);
    
        public static Timestamp ToTimestamp(this Instant instant) => 
            instant.ToDateTimeUtc().ToTimestamp();
        
        public static Guid ToGuid(this String id) =>
            Guid.Parse(id);
    }
}