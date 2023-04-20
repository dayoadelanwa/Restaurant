using AutoMapper;
using Mango.Services.CouponAPI.DbContexts;
using Mango.Services.CouponAPI.Models.Dto;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace Mango.Services.CouponAPI.Repository
{
    public class CouponRepository : ICouponRespository
    {
        private readonly ApplicationDbContext _context;
        private IMapper _mapper;

        public CouponRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CouponDto> GetCoupnByCode(string coupnCode)
        {
            var couponFromBd = await _context.Coupons.FirstOrDefaultAsync(u=> u.CouponCode== coupnCode);
            return _mapper.Map<CouponDto>(couponFromBd); 
        }
    }
}
