namespace GameStore.Core.Models
{
    public class Game
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public string? Developer { get; set; }
    }
}