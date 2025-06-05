namespace WFDBAPI.Database.Entities
{
    public class Warframe
    {
        // Warframe ID for indexing
        public int ID { get; set; }
        // Warframe's name
        public required string Name { get; set; }
    }
}
