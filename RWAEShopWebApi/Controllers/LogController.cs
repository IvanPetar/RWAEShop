using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RWAEshopDAL.Services;
using RWAEShopWebApi.DTOs;

namespace RWAEShopWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogController : ControllerBase
    {
        private readonly ILogService _logService;
        private readonly IMapper _mapper;

        public LogController(ILogService logService, IMapper mapper)
        {
            _logService = logService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetAll([FromQuery] int count = 50)
        {
            var logs = _logService.GetLastN(count);
            var result = _mapper.Map<IEnumerable<LogResponseDto>>(logs);
            return Ok(result);
        }

        [HttpGet("count")]
        public IActionResult Count()
        {
            return Ok(_logService.Count());
        }

        [HttpPost]
        public IActionResult CreateLog([FromBody] LogCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var log = _logService.Log(dto.Message, dto.Level);
            var result = _mapper.Map<LogResponseDto>(log);
            return Ok(result);
        }
        [HttpGet("search")]
        public IActionResult Search([FromQuery] string query)
        {
            try
            {
                var logs = _logService.GetAllQueryable()
                    .Where(l => EF.Functions.Like(l.Message, $"%{query}%"))
                    .OrderByDescending(l => l.Timestamp)
                    .ToList();

                var result = _mapper.Map<IEnumerable<LogResponseDto>>(logs);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logService.Log($"Log search failed: {ex.Message}", 3);
                return StatusCode(500, ex.Message);
            }
        }

    }
}
