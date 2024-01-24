namespace Infrastructure.Data.ViewModel
{
    public class AccountViewModel
    {
        public Guid AccountId { get; set; }
        public string Username { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int ProfileCode { get; set; }
        public bool? isSysadmin { get; set; }
    }
}