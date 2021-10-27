using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class Match
    {
        public int MatchId { get; set; }
        public int Team1Id { get; set; }
        [NotMapped]
        [ForeignKey("Team1Id")]
        public virtual Team Team1 { get; set; }
        public int Team2Id { get; set; }
        [NotMapped]
        [ForeignKey("Team2Id")]
        public virtual Team Team2 { get; set; }
        public int Scores1 { get; set; }
        public int Scores2 { get; set; }
        public double Over1 { get; set; }
        public double Over2 { get; set; }
        public int WinId { get; set; }
        public int LoseId { get; set; }        
        public DateTime MatchDate { get; set; }
    }
}
