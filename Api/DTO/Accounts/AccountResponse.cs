using System.Text.Json.Serialization;

namespace YssWebstoreApi.Api.DTO.Accounts
{
    public class AccountResponse
    {
        public required Guid Id { get; set; }
        public required string UniqueName { get; set; }
        public required string DisplayName { get; set; }
        public string? StatusText { get; set; }
        public string? AvatarUrl { get; set; }
        
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? FollowersCount { get; set; }
       
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? FollowingCount { get; set; }
    }
}
