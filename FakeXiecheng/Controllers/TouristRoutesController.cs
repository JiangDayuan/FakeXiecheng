using FakeXiecheng.Dtos;
using FakeXiecheng.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using System.Text.RegularExpressions;
using FakeXiecheng.ResourceParameters;
using FakeXiecheng.Models;

namespace FakeXiecheng.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TouristRoutesController : ControllerBase
    {
        private ITouristRouteRepository _touristRouteRepository;
        private readonly IMapper _mapper;

        public TouristRoutesController(ITouristRouteRepository touristRouteRepository, IMapper mapper)
        {
            _touristRouteRepository = touristRouteRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [HttpHead]
        public IActionResult GetTouristRoutes([FromQuery] TouristRouteResourceParameters parameters)
        {
            //Regex regex = new Regex(@"([A-Za-z0-9\-]+)(\d+)");
            //Regex regexDoubleSide = new Regex(@"\[(\s|\d+)\,(\s|\d+)\]");
            //Match match = regex.Match(rating);
            //if (match.Success)
            //{
            //    minRating = match.Groups[1].Value;
            //    maxRating = match.Groups[2].Value;
            //}

            
            
            var touristRoutesFromRepo = _touristRouteRepository.GetTouristRoutes(parameters.title, parameters.minRating, parameters.maxRating);
            if (touristRoutesFromRepo == null || touristRoutesFromRepo.Count() <= 0)
            {
                return NotFound("没有旅游路线");
            }
            var touristRoutesDto = _mapper.Map<IEnumerable<TouristRouteDto>>(touristRoutesFromRepo);
            return Ok(touristRoutesDto);
        }

        //api/touristroutes/{touristRouteId}
        [HttpGet("{touristRouteId:Guid}", Name = "GetTouristRouteById")]
        [HttpHead("{touristRouteId:Guid}")]
        public IActionResult GetTouristRouteById(Guid touristRouteId)
        {
            var touristRouteFromRepo = _touristRouteRepository.GetTouristRoute(touristRouteId);
            if (touristRouteFromRepo == null)
            {
                return NotFound($"旅游路线{touristRouteId}找不到");
            }
            var touristRouteDto = _mapper.Map<TouristRouteDto>(touristRouteFromRepo);
            return Ok(touristRouteDto);

        }

        [HttpPost]
        public IActionResult GreateTouristRoute([FromBody] TouristRouteForCreationDto touristRouteForCreationDto)
        {
            var touristRouteModel = _mapper.Map<TouristRoute>(touristRouteForCreationDto);
            _touristRouteRepository.AddTouristRoute(touristRouteModel);
            _touristRouteRepository.Save();
            var touristRouteForReturn = _mapper.Map<TouristRouteDto>(touristRouteModel);
            return CreatedAtRoute(
                "GetTouristRouteById",
                new { touristRouteId = touristRouteForReturn.Id},
                touristRouteForReturn
            );
        }

        [HttpPut("{touristRouteId:Guid}")]
        public IActionResult UpdateTouristRoute(
            [FromRoute] Guid touristRouteId,
            [FromBody] TouristRouteForUpdateDto touristRouteForUpdateDto)
        {
            if (!_touristRouteRepository.TouristRouteExists(touristRouteId))
            {
                return NotFound("旅游路线找不到");
            }

            var touristRouteFromRepo = _touristRouteRepository.GetTouristRoute(touristRouteId);
            // 1. 映射Dto
            // 2. 更新Dto
            // 3. 更新后的数据映射model
            _mapper.Map(touristRouteForUpdateDto, touristRouteFromRepo);

            _touristRouteRepository.Save();

            return NoContent();
        }
    }
}
