namespace MessengerApi.DAL.Entities
{
    public class Group
    {
        public string Id { get; set; }

        public string ApplicationUserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
    }
}
