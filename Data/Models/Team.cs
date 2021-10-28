using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class Team
    {
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public int MatchsCount { get; set; }
        public int WinsCount { get; set; }
        public int LosesCount { get; set; }
        public double TeamNRR { get; set; }
        public int PointsCount { get; set; }
    }
}
