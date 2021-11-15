using Amazon.Runtime;
using Amazon.S3;
using Microsoft.EntityFrameworkCore;
using NetFileAPI.AWS;
using NetFileAPI.Database;
using NetFileAPI.Mapper;
using NetFileAPI.Repositories;
using NetFileAPI.Repositories.Interfaces;
using NetFileAPI.Services;
using NetFileAPI.Services.Interfaces;

namespace NetFileAPI
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });

            // Add services to the container.
            services.AddControllers();
            services.AddHealthChecks();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            string mySqlConnectionStr = Configuration.GetConnectionString("MySqlConnection");
            services.AddDbContextPool<MySqlDbContext>(options =>
                options.UseMySql(
                    mySqlConnectionStr,
                    ServerVersion.AutoDetect(mySqlConnectionStr)
                ));



            var awsOptions = Configuration.GetAWSOptions();

            //Env
            awsOptions.Credentials = new EnvironmentVariablesAWSCredentials();
            awsOptions.Region = new EnvironmentVariableAWSRegion().Region;

            services.AddDefaultAWSOptions(awsOptions);
            services.AddAWSService<IAmazonS3>();


            var appSettingsSection = Configuration.GetSection("ServiceConfiguration");
            services.Configure<ServiceConfiguration>(appSettingsSection);

            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IFileRepository, FileRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAwsS3BucketHelper, AwsS3BucketHelper>();

            services.AddAutoMapper(c => c.AddProfile<FileProfile>(), typeof(Startup));
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Configure the HTTP request pipeline.
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // app.UseStaticFiles(new StaticFileOptions
            // {
            //     FileProvider = new PhysicalFileProvider(
            //         Path.Combine(env.ContentRootPath, "Public")),
            //     RequestPath = "/Public"
            // });

            app.UseCors();

            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}