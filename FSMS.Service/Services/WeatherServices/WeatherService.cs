using AutoMapper;
using FSMS.Entity.Models;
using FSMS.Entity.Repositories.FruitHistoryRepositories;
using FSMS.Entity.Repositories.WeatherRepositories;
using FSMS.Service.Enums;
using FSMS.Service.ViewModels.FruitHistories;
using FSMS.Service.ViewModels.Weather;
using Google.Api.Gax.ResourceNames;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FSMS.Service.Services.WeatherServices
{
    public class WeatherService : IWeatherService
    {
        private IWeatherRepository _weatherRepository;
        private IMapper _mapper;
        public WeatherService(IWeatherRepository weatherRepository, IMapper mapper)
        {
            _weatherRepository = weatherRepository;
            _mapper = mapper;
        }
        public async Task<List<GetWeather>> GetAllAsync()
        {
            try
            {
                IEnumerable<FSMS.Entity.Models.Weather> weathers = await _weatherRepository.GetAsync();

                List<GetWeather> result = weathers
                    .Select(weather => _mapper.Map<GetWeather>(weather))
                    .ToList();

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while fetching fruit histories.", ex);
            }
        }
        public async Task ScrapeWeatherAreaAndSaveToDatabaseAsync(int userId)
        {
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    string url = "https://nchmf.gov.vn/Kttvsite/vi-VN/1/thoi-tiet-dat-lien-24h-12h2-15.html";
                    HttpResponseMessage response = await httpClient.GetAsync(url);
                    string htmlContent = await response.Content.ReadAsStringAsync();

                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(htmlContent);

                    var weatherNodes = doc.DocumentNode.SelectNodes("//ul[@class='uk-list']/li");

                    if (weatherNodes != null)
                    {
                        foreach (var node in weatherNodes)
                        {
                            var locationNode = node.SelectSingleNode(".//a");
                            string location = locationNode?.InnerText.Trim();

                            var imageNode = node.SelectSingleNode(".//div[@class='icon-list-item uk-flex']/img");
                            string imageSrc = imageNode?.GetAttributeValue("src", null);

                            var descriptionNode = node.SelectSingleNode(".//div[@class='text-weather-location fix-weather-location']/p[1]");
                            string description = CleanDescription(descriptionNode?.InnerText);

                            var weatherNameNode = doc.DocumentNode.SelectSingleNode("//h1[@class='tt-news']");
                            string weatherName = weatherNameNode?.InnerText.Trim();

                            Weather weather = new Weather()
                            {
                                WeatherName = weatherName,
                                UserId = userId,
                                Location = location,
                                Image = imageSrc,
                                Description = description,
                                Status = StatusEnums.Active.ToString(),
                                CreatedDate = DateTime.Now,
                            };

                            await _weatherRepository.InsertAsync(weather);
                        }
                        await _weatherRepository.CommitAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while saving the entity changes.", ex);
            }
        }




        public async Task ScrapeCityWeatherAndSaveToDatabaseAsync(int userId)
        {
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    string url = "http://vnmha.gov.vn/nchmf-new/show-city-weather";
                    HttpResponseMessage response = await httpClient.GetAsync(url);
                    string htmlContent = await response.Content.ReadAsStringAsync();

                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(htmlContent);

                    // Find the <h2> element that contains the weather name
                    var weatherNameNode = doc.DocumentNode.SelectSingleNode("//h2");

                    // Extract the text content within the <h2> tag
                    string weatherName = weatherNameNode?.InnerText.Trim();

                    var weatherNodes = doc.DocumentNode.SelectNodes("//div[@class='col-md-4']");

                    if (weatherNodes != null)
                    {
                        foreach (var node in weatherNodes)
                        {
                            var locationNode = node.SelectSingleNode(".//h3");
                            string location = locationNode?.InnerText.Trim();

                            var imageNode = node.SelectSingleNode(".//div[@class='weather-icon']/img");
                            string imageSrc = imageNode?.GetAttributeValue("src", null);

                            var descriptionNode = node.SelectSingleNode(".//p[contains(text(), 'Nhiệt độ')]");
                            string description = CleanDescription(descriptionNode?.InnerText);

                            // Check if any of the required elements is null and skip adding if so
                            if (location != null && imageSrc != null && description != null)
                            {
                                Weather weather = new Weather()
                                {
                                    WeatherName = weatherName,
                                    UserId = userId,
                                    Location = location,
                                    Image = imageSrc,
                                    Description = description,
                                    Status = StatusEnums.Active.ToString(),
                                    CreatedDate = DateTime.Now,
                                };

                                await _weatherRepository.InsertAsync(weather);
                            }
                        }
                        await _weatherRepository.CommitAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while saving the entity changes.", ex);
            }
        }













        private string CleanDescription(string description)
        {
            if (description != null)
            {
                // Remove all extra spaces, newlines, and tabs
                description = Regex.Replace(description, @"\s+", " ").Trim();
            }
            return description;
        }




    }
}
