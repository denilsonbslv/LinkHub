using LinkHub.Api.Dtos;
using LinkHub.Api.Infrastructure.Data;
using LinkHub.Api.Services.Interfaces;
using LinkHub.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace LinkHub.Api.Services
{
    public class LinkService : ILinkService
    {
        private readonly ApplicationDbContext _context;
        private const string _shortCodeChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private const int _shortCodeLength = 7;
        private static readonly Random _random = new();

        public LinkService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Link> CreateAsync(CreateLinkDto linkDto, int userId)
        {
            if (linkDto.CategoryId.HasValue)
            {
                var categoryExists = await _context.Categories
                .AnyAsync(c => c.Id == linkDto.CategoryId.Value && c.UserId == userId);

                if (!categoryExists)
                {
                    throw new ArgumentException("A categoria especificada não existe ou não pertence a este usuário.");
                }
            }

            string shortCode;
            do
            {
                shortCode = GenerateShortCode();
            }
            while (await _context.Links.AnyAsync(l => l.ShortCode == shortCode));

            var link = new Link
            {
                OriginalUrl = linkDto.OriginalUrl,
                ShortCode = shortCode,
                Title = linkDto.Title,
                CategoryId = linkDto.CategoryId,
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                Clicks = 0
            };

            _context.Links.Add(link);
            await _context.SaveChangesAsync();

            return link;
        }

        /// <summary>
        /// Gera uma string aleatória com o tamanho definido para ser usada como ShortCode.
        /// </summary>
        /// <returns>O código curto gerado.</returns>
        private string GenerateShortCode()
        {
            return new string(Enumerable.Repeat(_shortCodeChars, _shortCodeLength)
             .Select(s => s[_random.Next(s.Length)]).ToArray());
        }
    }
}
