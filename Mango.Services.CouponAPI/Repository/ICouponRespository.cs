using Mango.Services.CouponAPI.Models.Dto;

namespace Mango.Services.CouponAPI.Repository
{
    public interface ICouponRespository
    {
        Task<CouponDto> GetCoupnByCode(string coupnCode);
    }
}
