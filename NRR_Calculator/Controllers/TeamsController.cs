using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data.Models;
using Web_Api.Context;

namespace Web_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : ControllerBase
    {
        private readonly MatchContext _context;

        public TeamsController(MatchContext context)
        {
            _context = context;
        }

        // GET: api/Teams
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Team>>> GetTeams()
        {
            return await _context.Teams.OrderByDescending(k => k.PointsCount).ThenByDescending(l => l.TeamNRR).ThenByDescending(m => m.TeamName).ToListAsync();
        }

        // GET: api/Teams/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Team>> GetTeam(int id)
        {
            var team = await _context.Teams.FindAsync(id);

            if (team == null)
            {
                return NotFound();
            }

            

            return team;
        }

        // PUT: api/Teams/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTeam(int id, Team team)
        {
            if (id != team.TeamId)
            {
                return BadRequest();
            }

            _context.Entry(team).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeamExists(id))
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

        // POST: api/Teams
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Team>> PostTeam(Team team)
        {
            _context.Teams.Add(team);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTeam", new { id = team.TeamId }, team);
        }

        // DELETE: api/Teams/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeam(int id)
        {
            var team = await _context.Teams.FindAsync(id);
            if (team == null)
            {
                return NotFound();
            }

            _context.Teams.Remove(team);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TeamExists(int id)
        {
            return _context.Teams.Any(e => e.TeamId == id);
        }

        [HttpPost("recalculate-nrr")]
        public async Task<ActionResult<IEnumerable<Team>>> RecalculateNRRAndGetTeams()
        {
            List<Team> teams =  await _context.Teams.ToListAsync();
            foreach (var team in teams)
            {
                int[] a1 = _context.Matches.Where(p => p.Team1Id == team.TeamId).Select(x => x.Scores1).ToArray();
                int[] a2 = _context.Matches.Where(p => p.Team2Id == team.TeamId).Select(x => x.Scores2).ToArray();
                int[] team1Runs = a1.Concat(a2).ToArray();

                int[] a3 = _context.Matches.Where(p => p.Team1Id == team.TeamId).Select(x => x.Scores2).ToArray();
                int[] a4 = _context.Matches.Where(p => p.Team2Id == team.TeamId).Select(x => x.Scores1).ToArray();
                int[] team1OppRun = a3.Concat(a4).ToArray();

                double[] a5 = _context.Matches.Where(p => p.Team1Id == team.TeamId).Select(x => x.Over1).ToArray();
                double[] a6 = _context.Matches.Where(p => p.Team2Id == team.TeamId).Select(x => x.Over2).ToArray();
                double[] team1Overs = a5.Concat(a6).ToArray();

                double[] a7 = _context.Matches.Where(p => p.Team1Id == team.TeamId).Select(x => x.Over2).ToArray();
                double[] a8 = _context.Matches.Where(p => p.Team2Id == team.TeamId).Select(x => x.Over1).ToArray();
                double[] team1OppOver = a7.Concat(a8).ToArray();

                var team1TotalScore = 0;
                var team1OppTotalScore = 0;
                var team1TotalOver = 0.0;
                var team1OppTotalOver = 0.0;

                for (int i = 0; i < team1Runs.Length; i++)
                {
                    team1TotalScore += team1Runs[i];
                    team1OppTotalScore += team1OppRun[i];
                    team1TotalOver += team1Overs[i];
                    team1OppTotalOver += team1OppOver[i];
                }

                team.TeamNRR = Math.Round((team1TotalScore / team1TotalOver) - (team1OppTotalScore / team1OppTotalOver), 2);
                
                _context.Teams.Update(team);

                await _context.SaveChangesAsync();

            }
            return await GetTeams();
        }

    }
}
