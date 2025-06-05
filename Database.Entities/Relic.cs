namespace WFDBAPI.Database.Entities
{
    public class Relic
    {
        // Relic ID for indexing
        public int ID { get; set; }
        // Boolean indicating if the relic is vaulted
        public required bool Vaulted { get; set; }
        // Relic name (Lith A7, Meso B1, Neo C3, Axi Z2, etc.)
        public required string Name { get; set; }
    }
}
