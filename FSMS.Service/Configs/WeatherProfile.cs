using AutoMapper;
using FSMS.Entity.Models;
using FSMS.Service.ViewModels.FruitHistories;
using FSMS.Service.ViewModels.Weather;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMS.Service.Configs
{
    public class WeatherProfile : Profile
    {
        public WeatherProfile()
        {
            CreateMap<Weather, GetWeather>();
        }
    }
}
