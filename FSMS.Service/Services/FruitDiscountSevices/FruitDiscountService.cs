using AutoMapper;
using FSMS.Entity.Models;
using FSMS.Entity.Repositories.FruitDiscountRepositories;
using FSMS.Entity.Repositories.FruitRepositories;
using FSMS.Service.Enums;
using FSMS.Service.ViewModels.FruitDiscounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMS.Service.Services.FruitDiscountSevices
{
    public class FruitDiscountService : IFruitDiscountService
    {
        private IFruitRepository _fruitRepository;
        private IFruitDiscountRepository _fruitDiscountRepository;
        private IMapper _mapper;
        public FruitDiscountService(IMapper mapper, IFruitRepository fruitRepository, IFruitDiscountRepository fruitDiscountRepository)
        {
            _mapper = mapper;
            _fruitRepository = fruitRepository;
            _fruitDiscountRepository = fruitDiscountRepository;
        }
        public async Task CreateFruitDiscountAsync(CreateFruitDiscount createFruitDiscount)
        {
            try
            {
                Fruit existedFruit = await _fruitRepository.GetByIDAsync(createFruitDiscount.FruitId);
                if (existedFruit == null)
                {
                    throw new Exception("Fruit Id does not exist in the system.");
                }

                int lastId = (await _fruitDiscountRepository.GetAsync()).Max(x => x.FruitDiscountId);
                FruitDiscount fruitDiscount = new FruitDiscount()
                {
                    DiscountName = createFruitDiscount.DiscountName,
                    DiscountExpiryDate = createFruitDiscount.DiscountExpiryDate,
                    DiscountPercentage = createFruitDiscount.DiscountPercentage,
                    FruitId = createFruitDiscount.FruitId,
                    Status = StatusEnums.Active.ToString(),
                    CreatedDate = DateTime.Now,
                    FruitDiscountId = lastId + 1
                };

                if (existedFruit.OrderType == "PreOrder")
                {
                    fruitDiscount.DiscountThreshold = createFruitDiscount.DiscountThreshold;
                    fruitDiscount.DepositAmount = createFruitDiscount.DepositAmount;
                }

                await _fruitDiscountRepository.InsertAsync(fruitDiscount);
                await _fruitDiscountRepository.CommitAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task DeleteFruitDiscountAsync(int key)
        {
            try
            {
                FruitDiscount existedFruitDiscount = await _fruitDiscountRepository.GetByIDAsync(key);

                if (existedFruitDiscount == null)
                {
                    throw new Exception("FruitDiscount ID does not exist in the system.");
                }
                if (existedFruitDiscount.Status != StatusEnums.Active.ToString())
                {
                    throw new Exception("FruitDiscount is not active.");
                }
                existedFruitDiscount.Status = StatusEnums.InActive.ToString();

                await _fruitDiscountRepository.UpdateAsync(existedFruitDiscount);
                await _fruitDiscountRepository.CommitAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        public async Task<GetFruitDiscount> GetAsync(int key)
        {
            try
            {
                FruitDiscount fruitDiscount = await _fruitDiscountRepository.GetByIDAsync(key);

                if (fruitDiscount == null)
                {
                    throw new Exception("FruitDiscount ID does not exist in the system.");
                }
                if (fruitDiscount.Status != StatusEnums.Active.ToString())
                {
                    throw new Exception("FruitDiscount is not active.");
                }
                List<GetFruitDiscount> fruitDiscounts = _mapper.Map<List<GetFruitDiscount>>(
                  await _fruitDiscountRepository.GetAsync(includeProperties: "Fruit")
              );

                GetFruitDiscount result = _mapper.Map<GetFruitDiscount>(fruitDiscount);

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<List<GetFruitDiscount>> GetAllAsync(string? discountName = null, DateTime? discountExpiryDate = null, bool activeOnly = false)
        {
            try
            {
                List<GetFruitDiscount> fruitDiscounts = _mapper.Map<List<GetFruitDiscount>>(
                    (await _fruitDiscountRepository.GetAsync(includeProperties: "Fruit"))
                    .Where(discount =>
                        (string.IsNullOrEmpty(discountName) || discount.DiscountName.Contains(discountName)) &&
                        (!discountExpiryDate.HasValue || discount.DiscountExpiryDate?.Date == discountExpiryDate.Value.Date) &&
                        (!activeOnly || discount.Status == StatusEnums.Active.ToString())
                    )
                );

                return fruitDiscounts;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        public async Task UpdateFruitDiscountAsync(int key, UpdateFruitDiscount updateFruitDiscount)
        {
            try
            {
                FruitDiscount existedFruitDiscount = await _fruitDiscountRepository.GetByIDAsync(key);

                if (existedFruitDiscount == null)
                {
                    throw new Exception("FruitDiscount ID does not exist in the system.");
                }

                Fruit existedFruit = await _fruitRepository.GetByIDAsync(existedFruitDiscount.FruitId);

                if (existedFruit == null)
                {
                    throw new Exception("Fruit ID does not exist in the system.");
                }

                if (!string.IsNullOrEmpty(updateFruitDiscount.DiscountName))
                {
                    existedFruitDiscount.DiscountName = updateFruitDiscount.DiscountName;
                }

                if (updateFruitDiscount.DiscountThreshold < 0)
                {
                    throw new Exception("DiscountThreshold cannot be negative.");
                }

                existedFruitDiscount.DiscountThreshold = updateFruitDiscount.DiscountThreshold;


                existedFruitDiscount.DiscountPercentage = updateFruitDiscount.DiscountPercentage;
                existedFruitDiscount.DiscountExpiryDate = updateFruitDiscount.DiscountExpiryDate;
                existedFruitDiscount.DepositAmount = updateFruitDiscount.DepositAmount;

                if (!string.IsNullOrEmpty(updateFruitDiscount.Status))
                {
                    if (updateFruitDiscount.Status != "Active" && updateFruitDiscount.Status != "InActive")
                    {
                        throw new Exception("Status must be 'Active' or 'InActive'.");
                    }
                    existedFruitDiscount.Status = updateFruitDiscount.Status;
                }

                existedFruitDiscount.UpdateDate = DateTime.Now;

                await _fruitDiscountRepository.UpdateAsync(existedFruitDiscount);
                await _fruitDiscountRepository.CommitAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
