using AutoMapper;
using FSMS.Entity.Models;
using FSMS.Entity.Repositories.GardenRepositories;
using FSMS.Entity.Repositories.RoleRepositories;
using FSMS.Entity.Repositories.UserRepositories;
using FSMS.Service.Enums;
using FSMS.Service.Services.FileServices;
using FSMS.Service.Utility.Exceptions;
using FSMS.Service.ViewModels.Gardens;
using FSMS.Service.ViewModels.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMS.Service.Services.GardenServices
{
    public class GardenService : IGardenService
    {
        private IUserRepository _userRepository;
        private IGardenRepository _gardenRepository;
        private readonly IFileService _fileService;
        private IMapper _mapper;
        public GardenService(IUserRepository userRepository, IMapper mapper, IGardenRepository gardenRepository, IFileService fileService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _gardenRepository = gardenRepository;
            _fileService = fileService;
        }

        public async Task CreateGardenAsync(CreateGarden createGarden)
        {
            try
            {
                User existedUser = (await _userRepository.GetByIDAsync(createGarden.UserId));
                if (existedUser == null)
                {
                    throw new Exception("UserId does not exist in the system.");
                }
                int lastId = (await _gardenRepository.GetAsync()).Max(x => x.GardenId);

                bool gardenNameExists = (await _gardenRepository.GetAsync())
                .Any(g => g.UserId == createGarden.UserId && g.GardenName == createGarden.GardenName && g.Status == StatusEnums.Active.ToString());

                if (gardenNameExists)
                {
                    throw new Exception("GardenName already exists for this UserId.");
                }

                if (gardenNameExists)
                {
                    throw new Exception("GardenName already exists for this UserId.");
                }

                Garden garden = new Garden()
                {
                    GardenName = createGarden.GardenName,
                    Description = createGarden.Description,
                    /*Image = createGarden.Image,*/
                    Region = createGarden.Region,
                    UserId = createGarden.UserId,
                    Status = StatusEnums.Active.ToString(),
                    CreatedDate = DateTime.Now,
                    GardenId = lastId + 1
                };
                if (createGarden.UploadFile == null)
                {
                    garden.Image = "";
                }
                else if (createGarden.UploadFile != null) garden.Image = await _fileService.UploadFile(createGarden.UploadFile);

                await _gardenRepository.InsertAsync(garden);
                await _gardenRepository.CommitAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        public async Task DeleteGardenAsync(int key)
        {
            try
            {
                Garden existedGarden = await _gardenRepository.GetByIDAsync(key);

                if (existedGarden == null)
                {
                    throw new Exception("GardenId does not exist in the system.");
                }
                if (existedGarden.Status != StatusEnums.Active.ToString())
                {
                    throw new Exception("Garden is not active.");
                }

                existedGarden.Status = StatusEnums.InActive.ToString();

                await _gardenRepository.UpdateAsync(existedGarden);
                await _gardenRepository.CommitAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        public async Task<GetGarden> GetAsync(int key)
        {
            try
            {
                Garden garden = await _gardenRepository.GetByIDAsync(key);

                if (garden == null)
                {
                    throw new Exception("GardenId does not exist in the system.");
                }
                if (garden.Status != StatusEnums.Active.ToString())
                {
                    throw new Exception("Garden is not active.");
                }

                List<GetGarden> gardens = _mapper.Map<List<GetGarden>>(
                    await _gardenRepository.GetAsync(includeProperties: "User")
                );

                GetGarden result = _mapper.Map<GetGarden>(garden);

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<List<GetGarden>> GetAllAsync(string? gardenName = null, bool activeOnly = false)
        {
            try
            {
                IEnumerable<Garden> gardens = await _gardenRepository.GetAsync(includeProperties: "User");

                if (!string.IsNullOrWhiteSpace(gardenName))
                {
                    gardens = gardens.Where(garden => garden.GardenName.Contains(gardenName, StringComparison.OrdinalIgnoreCase));
                }

                if (activeOnly)
                {
                    gardens = gardens.Where(garden => garden.Status == StatusEnums.Active.ToString());
                }

                List<GetGarden> result = _mapper.Map<List<GetGarden>>(gardens);

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        public async Task UpdateGardenAsync(int key, UpdateGarden updateGarden)
        {
            try
            {
                Garden existedGarden = await _gardenRepository.GetByIDAsync(key);

                if (existedGarden == null)
                {
                    throw new Exception("GardenId does not exist in the system.");
                }




                if (!string.IsNullOrEmpty(updateGarden.GardenName))
                {
                    if (existedGarden.Status == StatusEnums.Active.ToString())
                    {
                        bool gardenNameExists = (await _gardenRepository.GetAsync())
                            .Any(g => g.UserId == existedGarden.UserId && g.GardenName == updateGarden.GardenName);

                        if (gardenNameExists)
                        {
                            throw new Exception("GardenName already exists for this UserId.");
                        }
                    }

                    existedGarden.GardenName = updateGarden.GardenName;
                }


                if (!string.IsNullOrEmpty(updateGarden.Description))
                {
                    existedGarden.Description = updateGarden.Description;
                }
                /*if (!string.IsNullOrEmpty(updateGarden.Image))
                {
                    existedGarden.Image = updateGarden.Image;
                }*/
                if (updateGarden.UploadFile == null)
                {
                    existedGarden.Image = "";
                }
                else if (updateGarden.UploadFile != null) existedGarden.Image = await _fileService.UploadFile(updateGarden.UploadFile);


                if (!string.IsNullOrEmpty(updateGarden.Region))
                {
                    existedGarden.Region = updateGarden.Region;
                }

                if (!string.IsNullOrEmpty(updateGarden.Status))
                {
                    if (updateGarden.Status != "Active" && updateGarden.Status != "InActive")
                    {
                        throw new Exception("Status must be 'Active' or 'InActive'.");
                    }
                    existedGarden.Status = updateGarden.Status;
                }
                existedGarden.UpdateDate = DateTime.Now;


                await _gardenRepository.UpdateAsync(existedGarden);
                await _gardenRepository.CommitAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
