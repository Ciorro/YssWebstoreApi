namespace YssWebstoreApi.Models.DTOs.Accounts
{
    public class PublicAccount
    {
        public required uint Id { get; set; }
        public required DateTimeOffset CreatedAt { get; set; }
        public required string UniqueName { get; set; }
        public required string DisplayName { get; set; }

        public static readonly PublicAccount Empty = new PublicAccount { 
            Id = 0, 
            CreatedAt = DateTimeOffset.MinValue, 
            UniqueName = "", 
            DisplayName = "" 
        };
    }
}
