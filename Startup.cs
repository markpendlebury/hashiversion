using System.Text;

namespace hashiversion;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            string[] applicationList = { "terraform", "vault", "consul", "nomad", "packer" };

            endpoints.MapControllers();
            endpoints.MapGet("/", async context =>
            {
                // Create a new message 
                StringBuilder message = new StringBuilder();

                // Build the message
                message.AppendLine($"Usage: /APPLICATION_NAME");
                message.AppendLine();
                message.AppendLine();
                message.AppendLine("Where ApplicationName is one of: ");
                // Add each application from the applicationList
                foreach (var applicationName in applicationList)
                {
                    message.AppendLine($"- {applicationName}");
                }
                // return a 400 status code along with our message
                
                await context.Response.WriteAsync(message.ToString());
            });
        });
    }
}