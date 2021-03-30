using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VaporStore.Data.Models.Enums;

namespace VaporStore.Data.Models
{
    public class Purchase
    {
        //⦁	Id – integer, Primary Key
        public int Id { get; set; }

        //⦁	Type – enumeration of type PurchaseType, with possible values(“Retail”, “Digital”) (required) 
        [Required]
        public PurchaseType Type { get; set; }

        //⦁	ProductKey – text, which consists of 3 pairs of 4 uppercase Latin letters and digits, separated by dashes(ex. “ABCD-EFGH-1J3L//”)       (required)
        [Required]
        
        public string ProductKey { get; set; }

        //⦁	Date – Date(required)
        [Required]
        public DateTime Date { get; set; }

        //⦁	CardId – integer, foreign key(required)
        public int CardId { get; set; }

        //⦁	Card – the purchase’s card(required)
        [Required]
        public Card Card { get; set; }

        //⦁	GameId – integer, foreign key(required)
        public int GameId { get; set; }

        //⦁	Game – the purchase’s game(required)
        [Required]
        public Game Game { get; set; }

    }
}