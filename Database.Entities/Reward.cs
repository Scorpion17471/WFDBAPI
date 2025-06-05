namespace WFDBAPI.Database.Entities
{
    public class Reward
    {
        // Reward ID for indexing
        public int ID { get; set; }
        // String to store part name (neuroptics, chassis, systems, main)
        public required string PartType { get; set; }
        // String to store the rarity of the reward (common, uncommon, rare)
        public required string Rarity { get; set; }
        


        // Foreign Key for Relic.ID
        public required int RelicID { get; set; }
        // Foreign Key for Warframe.ID
        public required int WarframeID { get; set; }
    }
}
