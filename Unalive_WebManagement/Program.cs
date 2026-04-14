using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Unalive_WebManagement.BLL.Interfaces;
using Unalive_WebManagement.BLL.Services;
using Unalive_WebManagement.DAL.Interfaces;
using Unalive_WebManagement.DAL.Repositories;
using Unalive_WebManagement.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Add DbContext
builder.Services.AddDbContext<UnaliveDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionStringDB")));

// Register Repositories
builder.Services.AddScoped<IItemRepository, ItemRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserItemRepository, UserItemRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<IReportRepository, ReportRepository>();
builder.Services.AddScoped<IStaffRepository, StaffRepository>();
builder.Services.AddScoped<IAbilitiesSetRepository, AbilitiesSetRepository>();
builder.Services.AddScoped<IAnnouncementRepository, AnnouncementRepository>();
builder.Services.AddScoped<IBundleItemRepository, BundleItemRepository>();
builder.Services.AddScoped<ICharacterAttackRepository, CharacterAttackRepository>();
builder.Services.AddScoped<ICharacterPassiveRepository, CharacterPassiveRepository>();
builder.Services.AddScoped<ICharacterPvPRepository, CharacterPvPRepository>();
builder.Services.AddScoped<ICharacterSkillRepository, CharacterSkillRepository>();
builder.Services.AddScoped<ICharacterStatRepository, CharacterStatRepository>();
builder.Services.AddScoped<IGemBundleRepository, GemBundleRepository>();
builder.Services.AddScoped<ISkinAndCharacterBundleRepository, SkinAndCharacterBundleRepository>();
builder.Services.AddScoped<IShopOrderRepository, ShopOrderRepository>();
builder.Services.AddScoped<IShopOrderDetailRepository, ShopOrderDetailRepository>();
builder.Services.AddScoped<ITopUpHistoryRepository, TopUpHistoryRepository>();
builder.Services.AddScoped<IUserBundleRepository, UserBundleRepository>();
builder.Services.AddScoped<IUserBanLogRepository, UserBanLogRepository>();
builder.Services.AddScoped<IBlobService, BlobService>();

// Register Services
builder.Services.AddScoped<IItemService, ItemService>();
builder.Services.AddScoped<ICharacterService, CharacterService>();
builder.Services.AddScoped<IShopService, ShopService>();
builder.Services.AddScoped<IAnnouncementService, AnnouncementService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IFeedbackService, FeedbackService>();
builder.Services.AddScoped<IAuthService, AuthService>();

// Configure JWT authentication
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Unalive Web Management API", Version = "v1" });

    // Add JWT authentication to Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            Array.Empty<string>()
        }
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
