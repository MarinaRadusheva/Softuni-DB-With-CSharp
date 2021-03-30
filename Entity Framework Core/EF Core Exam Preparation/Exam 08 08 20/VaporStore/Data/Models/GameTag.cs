namespace VaporStore.Data.Models
{
    public class GameTag
    {
        // ⦁	GameId – integer, Primary Key, foreign key(required)
        public int GameId { get; set; }

        //⦁	Game – Game
        public Game Game { get; set; }

        //⦁	TagId – integer, Primary Key, foreign key(required)
        public int TagId { get; set; }

        //⦁	Tag – Tag
        public Tag Tag { get; set; }

    }
}