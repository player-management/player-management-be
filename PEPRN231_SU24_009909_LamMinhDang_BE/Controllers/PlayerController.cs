using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PEPRN231_SU24_009909_LamMinhDang_BE.ViewModel;
using Repository.Models;
using Repository.UnitOfWork;
using System.Text.RegularExpressions;

namespace PEPRN231_SU24_009909_LamMinhDang_BE.Controllers
{
    [Route("api/player")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public PlayerController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        //Validation
        private static bool ValidatePainting(string paintingName)
        {
            // Check PaintingName format
            if (!Regex.IsMatch(paintingName, @"^([A-Z][a-zA-Z0-9]*\s?)+$"))
            {
                return false;
            }
            // All checks passed
            return true;
        }
        [HttpGet("categories")]
        public IActionResult GetCategories()
        {
            var categories = _unitOfWork.ClubRepository.GetQueryable()
                .Select(c => new { c.FootballClubId, c.ClubName }).ToList();
            return Ok(categories);
        }
        // GET: api/<PaintngsController>
        [HttpGet]
        public IActionResult Get()
        {
            var result = _unitOfWork.PlayerRepository.GetQueryable()
                .Include(x=>x.FootballClub).ToList();

            var list = result.Select(x => new PlayerDTO
            {
                FootballPlayerId = x.FootballPlayerId,
                ClubName = x.FootballClub!.ClubName,
                Achievements = x.Achievements,
                Birthday = x.Birthday,
                FullName = x.FullName,
                Nomination = x.Nomination,
                PlayerExperiences = x.PlayerExperiences,

            });
            return Ok(list.ToList());
        }
        // GET api/<PaintngsController>/5
        [HttpGet("{id}")]
        [Authorize(Roles = "1")]
        public IActionResult GetById(string id)
        {
            var result = _unitOfWork.PlayerRepository.GetQueryable()
                .Include(x => x.FootballClub)
                .FirstOrDefault(x => x.FootballPlayerId.Equals(id));
            if (result == null)
            {
                return NotFound();
            }
            var dto = new PlayerDTO
            {
                FootballPlayerId = result.FootballPlayerId,
                ClubName = result.FootballClub!.ClubName,
                Achievements = result.Achievements,
                Birthday = result.Birthday,
                FullName = result.FullName,
                Nomination = result.Nomination,
                PlayerExperiences = result.PlayerExperiences,
            };
            return Ok(dto);
        }

        [HttpGet("search")]
        [Authorize(Roles = "1,2")]
        public IActionResult Search([FromQuery] string? Achievements, [FromQuery] string? Nomination)
        {
            // Perform the search based on the provided criteria
            IQueryable<FootballPlayer> resultSearch = _unitOfWork.PlayerRepository.GetQueryable()
                .Include(x => x.FootballClub);

            if (!string.IsNullOrEmpty(Achievements))
            {
                resultSearch = resultSearch.Where(x => x.Achievements.Contains(Achievements));
            }

            if (!string.IsNullOrEmpty(Nomination))
            {
                resultSearch = resultSearch.Where(x => x.Nomination.Contains(Nomination));
            }

            var searchList = resultSearch.Select(x => new PlayerDTO
            {
                FootballPlayerId = x.FootballPlayerId,
                ClubName = x.FootballClub!.ClubName,
                Achievements = x.Achievements,
                Birthday = x.Birthday,
                FullName = x.FullName,
                Nomination = x.Nomination,
                PlayerExperiences = x.PlayerExperiences,
            }).ToList();

            return Ok(searchList);
        }

        // POST api/<PaintngsController>
        [HttpPost]
        public IActionResult Post([FromBody] AddDTO request)
        {
            if (string.IsNullOrEmpty(request.FullName) ||
                string.IsNullOrEmpty(request.PlayerExperiences) ||
                string.IsNullOrEmpty(request.Nomination) ||
                string.IsNullOrEmpty(request.Achievements) ||
                string.IsNullOrEmpty(request.FootballClubId)
               )
            {
                return BadRequest("Input Null");
            }
            if (request.Birthday > DateTime.Parse("01-01-2007"))
            {
                return BadRequest("Year not valid");
            }
            // Check Price
            if (request.Achievements.Length < 9 || request.Achievements.Length >100)
            {
                return BadRequest("Achievements invalid");
            }
            if (request.Nomination.Length < 9 || request.Nomination.Length > 100)
            {
                return BadRequest("Nomination invalid");
            }
            if (!ValidatePainting(request.FullName))
            {
                return BadRequest("Validate Wrong");
            }

            var newPainting = new FootballPlayer
            {
                FootballPlayerId = GenerateNewId(),
                FullName = request.FullName,
                PlayerExperiences = request.PlayerExperiences,               
                Nomination = request.Nomination,
                Achievements = request.Achievements,
                Birthday = request.Birthday,
                FootballClubId = request.FootballClubId,
                FootballClub = _unitOfWork.ClubRepository.Get(x => x.FootballClubId.Equals(request.FootballClubId)),

            };
            _unitOfWork.PlayerRepository.Add(newPainting);
            _unitOfWork.Commit();


            return GetById(newPainting.FootballPlayerId);
        }
        private string GenerateNewId()
        {
            // Retrieve all existing IDs from the repository
            var allIds = _unitOfWork.PlayerRepository.GetQueryable()
                .Select(x => x.FootballPlayerId)
                .ToList();

            // Find the highest numeric part from existing IDs
            int maxNumericPart = 0;
            foreach (var id in allIds)
            {
                if (id.Length > 2) // Ensure the ID has the expected format
                {
                    var stringPart = id.Substring(2); // Extract numeric part
                    if (int.TryParse(stringPart, out int numericPart))
                    {
                        if (numericPart > maxNumericPart)
                        {
                            maxNumericPart = numericPart;
                        }
                    }
                }
            }

            // Increment the highest numeric part to generate a new ID
            int newNumericPart = maxNumericPart + 1;

            // Format the new numeric part with leading zeros
            string newId = $"PL{newNumericPart:D5}";

            return newId; // If you need to return the numeric part for further use
        }


        // PUT api/<PaintngsController>/5
        [Authorize(Roles = "1")]
        [HttpPut]
        public IActionResult Put([FromBody] UpdateDTO request)
        {
            if (string.IsNullOrEmpty(request.FootballPlayerId) ||
                string.IsNullOrEmpty(request.FullName) ||
                string.IsNullOrEmpty(request.PlayerExperiences) ||
                string.IsNullOrEmpty(request.Nomination) ||
                string.IsNullOrEmpty(request.Achievements) )
            {
                return BadRequest("Input Null");
            }            
            // Check Price
            if (request.Achievements.Length < 9 && request.Achievements.Length > 100)
            {
                return BadRequest("Achievements invalid");
            }
            if (request.Nomination.Length < 9 && request.Nomination.Length > 100)
            {
                return BadRequest("Achievements invalid");
            }
            if (!ValidatePainting(request.FullName))
            {
                return BadRequest("Validate Wrong");
            }

            var exitedPainting = _unitOfWork.PlayerRepository
                .Get(x => x.FootballPlayerId.Equals(request.FootballPlayerId));
            if (exitedPainting == null)
            {
                return NotFound();
            }

            exitedPainting.FullName = request.FullName;
            exitedPainting.PlayerExperiences = request.PlayerExperiences;
            exitedPainting.Nomination = request.Nomination;
            exitedPainting.Achievements = request.Achievements;
            
            exitedPainting.FootballClubId = request.FootballClubId;
            exitedPainting.FootballClub = _unitOfWork.ClubRepository.Get(x => x.FootballClubId.Equals(request.FootballClubId));

            _unitOfWork.PlayerRepository.Update(exitedPainting);
            _unitOfWork.Commit();


            return GetById(exitedPainting.FootballPlayerId);
        }

        // DELETE api/<PaintngsController>/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "1")]
        public IActionResult Delete(string id)
        {
            var exitedPainting = _unitOfWork.PlayerRepository.Get(x => x.FootballPlayerId.Equals(id));
            if (exitedPainting == null)
            {
                return BadRequest("Cannot find painting");
            }
            _unitOfWork.PlayerRepository.Remove(exitedPainting);
            _unitOfWork.Commit();


            return Ok("Successfull delete");
        }


    }
}
