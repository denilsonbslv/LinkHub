using LinkHub.Api.Dtos;
using LinkHub.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LinkHub.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class LinksController : ControllerBase
    {
        private readonly ILinkService _linkService;

        public LinksController(ILinkService linkService)
        {
            _linkService = linkService;
        }

        /// <summary>
        /// Cria e encurta um novo link para o usuário autenticado.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(LinkDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateLink(CreateLinkDto createLinkDto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            try
            {
                var newLink = await _linkService.CreateAsync(createLinkDto, userId);

                var linkDto = new LinkDto
                {
                    Id = newLink.Id,
                    OriginalUrl = newLink.OriginalUrl,
                    ShortCode = newLink.ShortCode,
                    ShortUrl = $"{Request.Scheme}://{Request.Host}/{newLink.ShortCode}",
                    Title = newLink.Title,
                    Clicks = newLink.Clicks,
                    CreatedAt = newLink.CreatedAt
                };

                return CreatedAtAction(nameof(CreateLink), new { id = linkDto.Id }, linkDto);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
