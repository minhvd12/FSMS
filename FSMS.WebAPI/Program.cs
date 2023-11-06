using FirebaseAdmin;
using FluentValidation;
using FluentValidation.AspNetCore;
using FSMS.Entity.Repositories.CategoryFruitRepositories;
using FSMS.Entity.Repositories.CommentRepositories;
using FSMS.Entity.Repositories.CropVarietyGrowthTaskRepositories;
using FSMS.Entity.Repositories.CropVarietyRepositories;
using FSMS.Entity.Repositories.CropVarietyStageRepositories;
using FSMS.Entity.Repositories.FruitDiscountRepositories;
using FSMS.Entity.Repositories.FruitHistoryRepositories;
using FSMS.Entity.Repositories.FruitImageRepositories;
using FSMS.Entity.Repositories.FruitRepositories;
using FSMS.Entity.Repositories.GardenRepositories;
using FSMS.Entity.Repositories.GardenTaskRepositories;
using FSMS.Entity.Repositories.NotificationRepositories;
using FSMS.Entity.Repositories.OrderDetailRepositories;
using FSMS.Entity.Repositories.OrderRepositories;
using FSMS.Entity.Repositories.PaymentRepositories;
using FSMS.Entity.Repositories.PlantRepositories;
using FSMS.Entity.Repositories.PostRepositories;
using FSMS.Entity.Repositories.ReviewFruitRepositories;
using FSMS.Entity.Repositories.RoleRepositories;
using FSMS.Entity.Repositories.SeasonRepositories;
using FSMS.Entity.Repositories.UserRepositories;
using FSMS.Entity.Repositories.WeatherRepositories;
using FSMS.Service.Configs;
using FSMS.Service.Services.AuthServices;
using FSMS.Service.Services.CategoryFruitServices;
using FSMS.Service.Services.CommentServices;
using FSMS.Service.Services.CropVarietyGrowthTaskServices;
using FSMS.Service.Services.CropVarietyServices;
using FSMS.Service.Services.CropVarietyStageServices;
using FSMS.Service.Services.FileServices;
using FSMS.Service.Services.FruitDiscountSevices;
using FSMS.Service.Services.FruitHistoryServices;
using FSMS.Service.Services.FruitImageServices;
using FSMS.Service.Services.FruitServices;
using FSMS.Service.Services.GardenServices;
using FSMS.Service.Services.GardenTaskServices;
using FSMS.Service.Services.NotificationServices;
using FSMS.Service.Services.OrderServices;
using FSMS.Service.Services.PaymentServices;
using FSMS.Service.Services.PlantServices;
using FSMS.Service.Services.PostServices;
using FSMS.Service.Services.ReviewFruitServices;
using FSMS.Service.Services.RoleServices;
using FSMS.Service.Services.SeasonServices;
using FSMS.Service.Services.UserServices;
using FSMS.Service.Services.WeatherServices;
using FSMS.Service.ViewModels;
using FSMS.Service.ViewModels.Authentications;
using FSMS.Service.ViewModels.Gardens;
using FSMS.Service.ViewModels.Users;
using FSMS.WebAPI.Extensions;
using FSMS.WebAPI.Middlewares;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

namespace FSMS.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers().
                AddFluentValidation(c => c.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly()));

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();


            #region JWT 
            builder.Services.AddSwaggerGen(options =>
            {
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "FSMMS Application API",
                    Description = "JWT Authentication API"
                });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtAuth:Key"])),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });
            #endregion

            //ODATA
            //var modelBuilder = new ODataConventionModelBuilder();
            //modelBuilder.EntitySet<GetCustomer>("Users");
            //modelBuilder.EntitySet<GetSupplier>("Suppliers");
            //modelBuilder.EntitySet<GetFarmer>("Farmers");
            //modelBuilder.EntitySet<GetExpert>("Users");
            //modelBuilder.EntitySet<GetDocument>("Documents");




            //add CORS
            builder.Services.AddCors(cors => cors.AddPolicy(
                                        name: "WebPolicy",
                                        build =>
                                        {
                                            build.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                                        }
                                    ));


            /*  builder.Services.AddScoped<IValidator<CreateGarden>, GardenValidator>();*/

            //DI

            builder.Services.Configure<JwtAuth>(builder.Configuration.GetSection("JwtAuth"));
            builder.Services.AddScoped<IRoleRepository, RoleRepository>();
            builder.Services.AddScoped<IRoleService, RoleService>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IGardenService, GardenService>();
            builder.Services.AddScoped<IGardenRepository, GardenRepository>();
            builder.Services.AddScoped<ISeasonService, SeasonService>();
            builder.Services.AddScoped<ISeasonRepository, SeasonRepository>();
            builder.Services.AddScoped<IPlantService, PlantService>();
            builder.Services.AddScoped<IPlantRepository, PlantRepository>();
            builder.Services.AddScoped<ICropVarietyService, CropVarietyService>();
            builder.Services.AddScoped<ICropVarietyRepository, CropVarietyRepository>();
            builder.Services.AddScoped<ICropVarietyGrowthTaskService, CropVarietyGrowthTaskService>();
            builder.Services.AddScoped<ICropVarietyGrowthTaskRepository, CropVarietyGrowthTaskRepository>();
            builder.Services.AddScoped<ICropVarietyStageService, CropVarietyStageService>();
            builder.Services.AddScoped<ICropVarietyStageRepository, CropVarietyStageRepository>();
            builder.Services.AddScoped<IGardenTaskService, GardenTaskService>();
            builder.Services.AddScoped<IGardenTaskRepository, GardenTaskRepository>();
            builder.Services.AddScoped<IPostService, PostService>();
            builder.Services.AddScoped<IPostRepository, PostRepository>();
            builder.Services.AddScoped<ICommentService, CommentService>();
            builder.Services.AddScoped<ICommentRepository, CommentRepository>();
            builder.Services.AddScoped<IFruitService, FruitService>();
            builder.Services.AddScoped<IFruitRepository, FruitRepository>();
            builder.Services.AddScoped<ICategoryFruitRepository, CategoryFruitRepository>();
            builder.Services.AddScoped<ICategoryFruitService, CategoryFruitService>();
            builder.Services.AddScoped<IFruitImageRepository, FruitImageRepository>();
            builder.Services.AddScoped<IFruitImageService, FruitImageService>();
            builder.Services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IReviewFruitRepository, ReviewFruitRepository>();
            builder.Services.AddScoped<IReviewFruitService, ReviewFruitService>();
            builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
            builder.Services.AddScoped<INotificationService, NotificationService>();
            builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
            builder.Services.AddScoped<IPaymentService, PaymentService>();
            builder.Services.AddScoped<IFileService, FileService>();
            builder.Services.AddScoped<IFruitHistoryService, FruitHistoryService>();
            builder.Services.AddScoped<IFruitHistoryRepository, FruitHistoryRepository>();
            builder.Services.AddScoped<IWeatherService, WeatherService>();
            builder.Services.AddScoped<IWeatherRepository, WeatherRepository>();
            builder.Services.AddScoped<IFruitDiscountService, FruitDiscountService>();
            builder.Services.AddScoped<IFruitDiscountRepository, FruitDiscountRepository>();


            //builder.Services.ConfigureAutoMapper();


            builder.Services.AddAutoMapper(typeof(UserProfile),
                                           typeof(GardenProfile),
                                           typeof(SeasonProfile),
                                           typeof(PlantProfile),
                                           typeof(CropVarietyProfile),
                                           typeof(GardenTaskProfile),
                                           typeof(PostProfile),
                                           typeof(CommentProfile),
                                           typeof(FruitProfile),
                                           typeof(FruitImageProfile),
                                           typeof(CategoryFruitProfile),
                                           typeof(RoleProfile),
                                           typeof(OrderProfile),
                                           typeof(OrderDetailProfile),
                                           typeof(ReviewFruitProfile),
                                           typeof(NotificationProfile),
                                           typeof(PaymentProfile),
                                           typeof(FruitHistoryProfile),
                                           typeof(FruitDiscountProfile),
                                           typeof(CropVarietyGrowthTaskProfile),
                                           typeof(CropVarietyStageProfile),
                                           typeof(WeatherProfile));


            //Add middleware extentions
            builder.Services.AddTransient<ExceptionMiddleware>();
            //Add firebase config
            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile("Configurations/capstone-firebase.json")
            });

            var app = builder.Build();
            // Configure the HTTP request pipeline.

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //app.UseSwagger();
            //app.UseSwaggerUI();

            app.UseCors("WebPolicy");
            app.UseAuthentication();
            app.UseAuthorization();
            app.UsePathBase("/api");

            app.ConfigureExceptionMiddleware();
            app.MapControllers();

            app.Run();
        }
    }
}

