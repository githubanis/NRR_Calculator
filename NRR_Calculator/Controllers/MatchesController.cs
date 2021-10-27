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

            int[] m1 = _context.Matches.Where(p => p.Team1Id == match.Team1Id).OrderBy(a => a.Team1Id).Select(x => x.Scores1).ToArray();
            double[] m2 = _context.Matches.Where(p => p.Team1Id == match.Team1Id).OrderBy(a => a.Team1Id).Select(x => x.Over1).ToArray();
            //IEnumerable<Match> m2 = _context.Matches.Where(p => p.Team2Id == match.Team2Id).OrderBy(a => a.Team2Id).ToArray();
            int[] m3 = _context.Matches.Where(p => p.Team2Id == match.Team2Id).OrderBy(a => a.Team2Id).Select(x => x.Scores2).ToArray();
            double[] m4 = _context.Matches.Where(p => p.Team2Id == match.Team2Id).OrderBy(a => a.Team2Id).Select(x => x.Over2).ToArray();

            var team1TotalScore = 0;
            var team1TotalOver = 0.0;
            var team2TotalScore = 0;
            var team2TotalOver = 0.0;
            for (int i = 0; i < m1.Length; i++)
            {
                team1TotalScore += m1[i];
                team1TotalOver += m2[i];
            }
            for (int i = 0; i < m3.Length; i++)
            {
                team2TotalScore += m3[i];
                team2TotalOver += m4[i];
            }

            team1.TeamNRR = (team1TotalScore / team1TotalOver) - (team2TotalScore / team2TotalOver);
            team2.TeamNRR = (team2TotalScore / team2TotalOver) - (team1TotalScore / team1TotalOver);

            
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
