using MessagePack;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rating.Models
{
    public class Rate
    {
        public int Id { get; set; }
        [Range(1, 10, ErrorMessage = "Ocena musi być w przedziale od 1 do 10.")]
        public int Value { get; set; }
        public int ItemId { get; set; }
        public Item Item { get; set; }
    }
}
