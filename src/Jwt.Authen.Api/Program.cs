using Asp.Versioning;
using Jwt.Authen.Api.Commons;
using Jwt.Authen.Api.Scalar;
using Jwt.Authen.Api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
IConfiguration configuration = builder.Configuration;

// 添加认证服务
builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = configuration["Jwt:Issuer"] ?? string.Empty,
        ValidAudience = configuration["Jwt:Audience"] ?? string.Empty,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Secret"] ?? string.Empty))
    };
});
builder.Services.AddSingleton<TokenService>();


builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader(),
        new HeaderApiVersionReader("X-API-VERSION"),
        new MediaTypeApiVersionReader("VER")
        );
}).AddMvc().AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

foreach (var version in ApiVersionHelper.GetApiVersions(typeof(ApiVersionConsts)))
{
    builder.Services.AddOpenApi($"v{version}", option =>
    {
        //option.AddDocumentTransformer<AuthenApiDocumentTransformer>();
        option.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
        option.AddDocumentTransformer((document, context, cancellationToken) =>
        {
            document.Info = new() { Title = $"微服务 - V{version.ToUpper()}", Version = $"v{version}", Description = $"微服务相关接口" };
            return Task.CompletedTask;
        });
    });
}

builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference();
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
