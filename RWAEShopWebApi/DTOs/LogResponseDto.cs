namespace RWAEShopWebApi.DTOs
{
    public class LogResponseDto
    {
        public int Id { get; set; }
        public string Message { get; set; } = string.Empty;
        public int Level { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
