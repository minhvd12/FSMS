using AutoMapper;
using FSMS.Entity.Models;
using FSMS.Entity.Repositories.GardenRepositories;
using FSMS.Entity.Repositories.GardenTaskRepositories;
using FSMS.Entity.Repositories.PlantRepositories;
using FSMS.Entity.Repositories.SeasonRepositories;
using FSMS.Service.Enums;
using FSMS.Service.Services.FileServices;
using FSMS.Service.Services.Notification;
using FSMS.Service.Utility.ValidationAttributes;
using FSMS.Service.ViewModels.GardenTasks;
using FSMS.Service.ViewModels.Seasons;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMS.Service.Services.GardenTaskServices
{
    public class GardenTaskService : IGardenTaskService
    {
        private IGardenRepository _gardenRepository;
        private IGardenTaskRepository _gardenTaskRepository;
        private IPlantRepository _plantRepository;
        private readonly IFileService _fileService;

        private IMapper _mapper;
        public GardenTaskService(IGardenRepository gardenRepository, IMapper mapper, IGardenTaskRepository gardenTaskRepository,
            IPlantRepository plantRepository, IFileService fileService)
        {
            _gardenRepository = gardenRepository;
            _mapper = mapper;
            _gardenTaskRepository = gardenTaskRepository;
            _plantRepository = plantRepository;
            _fileService = fileService;
        }

        public async Task CreateGardenTaskAsync(CreateGardenTask createGardenTask)
        {
            try
            {

                Garden existedGarden = (await _gardenRepository.GetByIDAsync(createGardenTask.GardenId));
                if (existedGarden == null)
                {
                    throw new Exception("Garden Id does not exist in the system.");
                }

                int lastId = (await _gardenTaskRepository.GetAsync()).Max(x => x.GardenTaskId);
                GardenTask gardenTask = new GardenTask()
                {

                    GardenTaskName = createGardenTask.GardenTaskName,
                    Description = createGardenTask.Description,
                    GardenTaskDate = createGardenTask.GardenTaskDate,
                    GardenId = createGardenTask.GardenId,
                    PlantId = createGardenTask.PlantId,
                    /*Image = createGardenTask.Image,*/
                    Status = GardenTaskEnum.Pending.ToString(),
                    CreatedDate = DateTime.Now,
                    GardenTaskId = lastId + 1
                };
                if (createGardenTask.UploadFile == null)
                {
                    gardenTask.Image = "";
                }
                else if (createGardenTask.UploadFile != null) gardenTask.Image = await _fileService.UploadFile(createGardenTask.UploadFile);

                await _gardenTaskRepository.InsertAsync(gardenTask);
                await _gardenTaskRepository.CommitAsync();

                // Sent noti
                Dictionary<string, string> data = new Dictionary<string, string>()
                {
                    { "type", "admin" },
                    { "GardenId", gardenTask.GardenTaskId.ToString() }
                };
                await PushNotification.SendMessage("eba11598-4e4e-485f-8e94-eeb6a81b8e1f", $"Test đi m .",
                    $"{gardenTask.GardenTaskName}  đang chờ.", data);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
 


        public async Task DeleteGardenTaskAsync(int key)
        {
            try
            {
                GardenTask existedGardenTask = await _gardenTaskRepository.GetByIDAsync(key);

                if (existedGardenTask == null)
                {
                    throw new Exception("GardenTask ID does not exist in the system.");
                }

                existedGardenTask.Status = GardenTaskEnum.Cancelled.ToString();

                await _gardenTaskRepository.UpdateAsync(existedGardenTask);
                await _gardenTaskRepository.CommitAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        public async Task<GetGardenTask> GetAsync(int key)
        {
            try
            {
                GardenTask gardenTask = await _gardenTaskRepository.GetByIDAsync(key);

                if (gardenTask == null)
                {
                    throw new Exception("GardenTask ID does not exist in the system.");
                }
                if (gardenTask.Status == GardenTaskEnum.Cancelled.ToString())
                {
                    throw new Exception("GardenTask is not active.");
                }
                List<GetGardenTask> gardenTasks = _mapper.Map<List<GetGardenTask>>(
                  await _gardenTaskRepository.GetAsync(includeProperties: "Garden,Plant")
              );

                GetGardenTask result = _mapper.Map<GetGardenTask>(gardenTask);

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<List<GetGardenTask>> GetAllAsync(string? gardenTaskName = null, DateTime? taskDate = null, bool activeOnly = false)
        {
            try
            {
                List<GetGardenTask> gardenTasks = _mapper.Map<List<GetGardenTask>>(
                    (await _gardenTaskRepository.GetAsync(includeProperties: "Garden,Plant"))
                    .Where(task => (string.IsNullOrEmpty(gardenTaskName) || task.GardenTaskName.Contains(gardenTaskName)) &&
                                    (!taskDate.HasValue || task.GardenTaskDate.Date == taskDate.Value.Date) &&
                                    (!activeOnly || task.Status != GardenTaskEnum.Cancelled.ToString())
                    )
                );

                return gardenTasks;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task UpdateGardenTaskAsync(int key, UpdateGardenTask updateGardenTask)
        {
            try
            {
                GardenTask existedGardenTask = await _gardenTaskRepository.GetByIDAsync(key);

                if (existedGardenTask == null)
                {
                    throw new Exception("GardenTask ID does not exist in the system.");
                }

                if (!string.IsNullOrEmpty(updateGardenTask.GardenTaskName))
                {
                    existedGardenTask.GardenTaskName = updateGardenTask.GardenTaskName;
                }

                if (!string.IsNullOrEmpty(updateGardenTask.Description))
                {
                    existedGardenTask.Description = updateGardenTask.Description;
                }
                existedGardenTask.GardenTaskDate = updateGardenTask.GardenTaskDate;

                if (updateGardenTask.UploadFile == null)
                {
                    existedGardenTask.Image = "";
                }
                else if (updateGardenTask.UploadFile != null) existedGardenTask.Image = await _fileService.UploadFile(updateGardenTask.UploadFile);


                if (!string.IsNullOrEmpty(updateGardenTask.Status))
                {
                    if (updateGardenTask.Status != "Pending" && updateGardenTask.Status != "InProgress" && updateGardenTask.Status != "Completed" && updateGardenTask.Status != "Cancelled")
                    {
                        throw new Exception("Status must be 'Pending' or 'InProgress'. or 'Completed' or 'Cancelled'");
                    }
                    existedGardenTask.Status = updateGardenTask.Status;
                }
                existedGardenTask.UpdateDate = DateTime.Now;


                await _gardenTaskRepository.UpdateAsync(existedGardenTask);
                await _gardenTaskRepository.CommitAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
