using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data.Models;
using Web_Api.Context;
using Microsoft.AspNetCore.Cors;

namespace Web_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchesController : ControllerBase
    {
        private readonly MatchContext _context;

        public MatchesController(MatchContext context)
        {

            _context = context;
        }

        // GET: api/Matches
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Match>>> GetMatches()
        {
            return await _context.Matches.ToListAsync();
        }

        // GET: api/Matches/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Match>> GetMatch(int id)
        {
            var match = await _context.Matches.FindAsync(id);

            if (match == null)
            {
                return NotFound();
            }

            return match;
        }

        // PUT: api/Matches/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMatch(int id, Match match)
        {
            if (id != match.MatchId)
            {
                return BadRequest();
            }

            _context.Entry(match).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MatchExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Matches
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Match>> PostMatch(Match match)
        {            
            Match match1 = new Match
            {
                Team1Id = match.Team1Id,
                Team2Id = match.Team2Id,
                Scores1 = match.Scores1,
                Scores2 = match.Scores2,
                Over1 = match.Over1,
                Over2 = match.Over2,
                WinId = match.Scores1 > match.Scores2 ? match.Team1Id : match.Team2Id,
                LoseId = match.Scores1 > match.Scores2 ? match.Team2Id : match.Team1Id,
                MatchDate = DateTime.UtcNow
            };
            
            Team team1 = _context.Teams.FindAsync(match.Team1Id).Result;
            Team team2 = _context.Teams.FindAsync(match.Team2Id).Result;

            _context.Matches.Add(match1);
            await _context.SaveChangesAsync();

            team1.MatchsCount += 1;
            team1.WinsCount += match.Scores1 > match.Scores2 ? 1 : 0;
            team1.LosesCount += match.Scores1 > match.Scores2 ? 0 : 1;
            team1.PointsCount += match.Scores1 > match.Scores2 ? 2 : 0;

            team2.MatchsCount += 1;
            team2.WinsCount += match.Scores1 > match.Scores2 ? 0 : 1;
            team2.LosesCount += match.Scores1 > match.Scores2 ? 1 : 0;
            team2.PointsCount += match.Scores1 > match.Scores2 ? 0 : 2;

            int[] a1 = _context.Matches.Where(p => p.Team1Id == match.Team1Id).Select(x => x.Scores1).ToArray();
            int[] a2 = _context.Matches.Where(p => p.Team2Id == match.Team1Id).Select(x => x.Scores2).ToArray();
            int[] team1Runs = a1.Concat(a2).ToArray();

            int[] a3 = _context.Matches.Where(p => p.Team1Id == match.Team1Id).Select(x => x.Scores2).ToArray();
            int[] a4 = _context.Matches.Where(p => p.Team2Id == match.Team1Id).Select(x => x.Scores1).ToArray();
            int[] team1OppRun = a3.Concat(a4).ToArray();

            double[] a5 = _context.Matches.Where(p => p.Team1Id == match.Team1Id).Select(x => x.Over1).ToArray();
            double[] a6 = _context.Matches.Where(p => p.Team2Id == match.Team1Id).Select(x => x.Over2).ToArray();
            double[] team1Overs = a5.Concat(a6).ToArray();

            double[] a7 = _context.Matches.Where(p => p.Team1Id == match.Team1Id).Select(x => x.Over2).ToArray();
            double[] a8 = _context.Matches.Where(p => p.Team2Id == match.Team1Id).Select(x => x.Over1).ToArray();
            double[] team1OppOver = a7.Concat(a8).ToArray();


            int[] b1 = _context.Matches.Where(p => p.Team2Id == match.Team2Id).Select(x => x.Scores2).ToArray();
            int[] b2 = _context.Matches.Where(p => p.Team1Id == match.Team2Id).Select(x => x.Scores1).ToArray();
            int[] team2Runs = b1.Concat(b2).ToArray();

            int[] b3 = _context.Matches.Where(p => p.Team2Id == match.Team2Id).Select(x => x.Scores1).ToArray();
            int[] b4 = _context.Matches.Where(p => p.Team1Id == match.Team2Id).Select(x => x.Scores2).ToArray();
            int[] team2OppRuns = b3.Concat(b4).ToArray();

            double[] b5 = _context.Matches.Where(p => p.Team2Id == match.Team2Id).Select(x => x.Over2).ToArray();
            double[] b6 = _context.Matches.Where(p => p.Team1Id == match.Team2Id).Select(x => x.Over1).ToArray();
            double[] team2Overs = b5.Concat(b6).ToArray();

            double[] b7 = _context.Matches.Where(p => p.Team2Id == match.Team2Id).Select(x => x.Over1).ToArray();
            double[] b8 = _context.Matches.Where(p => p.Team1Id == match.Team2Id).Select(x => x.Over2).ToArray();
            double[] team2OppOver = b7.Concat(b8).ToArray();

            var team1TotalScore = 0;
            var team1OppTotalScore = 0;
            var team1TotalOver = 0.0;
            var team1OppTotalOver = 0.0;

            var team2TotalScore = 0;
            var team2OppTotalScore = 0;
            var team2TotalOver = 0.0;
            var team2OppTotalOver = 0.0;

            for (int i = 0; i < team1Runs.Length; i++)
            {
                team1TotalScore += team1Runs[i];
                team1OppTotalScore += team1OppRun[i];
                team1TotalOver += team1Overs[i];
                team1OppTotalOver += team1OppOver[i];
            }
            for (int i = 0; i < team2Runs.Length; i++)
            {
                team2TotalScore += team2Runs[i];
                team2OppTotalScore += team2OppRuns[i];
                team2TotalOver += team2Overs[i];
                team2OppTotalOver += team2OppOver[i];
            }

            team1.TeamNRR = Math.Round((team1TotalScore / team1TotalOver) - (team1OppTotalScore / team1OppTotalOver), 2);
            team2.TeamNRR = Math.Round((team2TotalScore / team2TotalOver) - (team2OppTotalScore / team2OppTotalOver), 2);

            
            _context.Teams.Update(team1);
            _context.Teams.Update(team2);            

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMatch", new { id = match.MatchId }, match);
        }

        // DELETE: api/Matches/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMatch(int id)
        {
            var match = await _context.Matches.FindAsync(id);
            if (match == null)
            {
                return NotFound();
            }

            _context.Matches.Remove(match);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MatchExists(int id)
        {
            return _context.Matches.Any(e => e.MatchId == id);
        }
    }
}
