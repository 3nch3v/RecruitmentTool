using System.Reflection;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Swashbuckle.AspNetCore.SwaggerGen;
using Newtonsoft.Json.Serialization;

using RecruitmentTool.Data;
using RecruitmentTool.Services;
using RecruitmentTool.Services.Contracts;
using RecruitmentTool.WebApi.Infrastructure;
using RecruitmentTool.WebApi.Infrastructure.ServiceExtensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOptions();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<RecruitmentToolDbContext>(
    options => options.UseLazyLoadingProxies()
                     .UseSqlServer(connectionString));

builder.Services.AddCustomCors("AllowAllOrigins");

builder.Services.AddControllers()
                .AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver());

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

builder.Services.AddScoped<IUrlHelper>(x =>
{
    ActionContext actionContext = x.GetRequiredService<IActionContextAccessor>().ActionContext;
    var factory = x.GetRequiredService<IUrlHelperFactory>();
    return factory.GetUrlHelper(actionContext);
});

builder.Services.AddVersioning();

builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

builder.Services.AddSwaggerGen();

builder.Services.AddTransient<ICandidateService, CandidateService>();
builder.Services.AddTransient<ISkillService, SkillService>();
builder.Services.AddTransient<IInterviewService, InterviewService>();
builder.Services.AddTransient<IRecruiterService, RecruiterService>();
builder.Services.AddTransient<IJobService, JobService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

    app.UseSwaggerUI(
        options =>
        {
            foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
            {
                options.SwaggerEndpoint(
                    $"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
            }
        });
}
else
{
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("AllowAllOrigins");

app.MapControllers();

app.Run();





