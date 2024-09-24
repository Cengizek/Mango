namespace Mango.Services.EmailAPI.Message
{
    public class RewardsMessage
    {
        public string UserId { get; set; }
        public string RewardsActivity { get; set; }
        public int OrderId { get; set; }
    }
}
