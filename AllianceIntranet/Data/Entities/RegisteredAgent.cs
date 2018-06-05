namespace AllianceIntranet.Data.Entities
{
    public class RegisteredAgent
    {

        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        public int CEClassId { get; set; }
        public CEClass CEClass { get; set; }

    }
}
