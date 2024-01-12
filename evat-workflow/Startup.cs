using evat_workflow.Helpers;
using IdentityServer4.AccessTokenValidation;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

namespace evat_workflow
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public CustomSettings Option { get; set; }



        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Option= new CustomSettings();
        }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();

            Configuration.Bind(nameof(CustomSettings), Option);
            services.AddSingleton(Option);

            services.AddSingleton(Configuration);


            services.AddControllers();
            services.AddEndpointsApiExplorer();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = bool.Parse(Option.SaveToken);
                options.Authority = Option.Authority;
                options.RequireHttpsMetadata = bool.Parse(Option.RequireHttpsMetadata);
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes("BBDC7B95-AC0C-4EBA-8086-A8E4B426F861")),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    RequireExpirationTime = true,
                    ClockSkew = TimeSpan.FromSeconds(0)
                };
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "eVAT Workflow Sample",
                    Version = "v1.0",
                    Description = "eVAT Workflow Sample v1.0",
                    TermsOfService = new Uri("https://evat.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "SWD API Developer",
                        Email = "michael.ameyaw@persol.net",
                        Url = new Uri("http://www.evat.net/")
                    },
                    License = new OpenApiLicense { Name = "Evat", Url = new Uri("http://www.evat.net/") }
                });

                var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                var commentsFileName = Assembly.GetExecutingAssembly().GetName().Name + ".XML";

                var commentsFile = Path.Combine(baseDirectory, commentsFileName);
                c.IncludeXmlComments(commentsFile);

                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        AuthorizationCode = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri(Option.AuthorityURL),
                            TokenUrl = new Uri(Option.TokenUrl),
                            RefreshUrl = new Uri(Option.TokenUrl),
                            Scopes = new Dictionary<string, string>
                            {
                                {"evat_modular_api", "eVAT Workflow Sample - full access"}
                            }
                        }
                    },

                });

                c.OperationFilter<EvatFilter>();
            });
        }
        public void Configure(WebApplication app, IWebHostEnvironment env)
        {

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"{Option.Folder}/swagger/v1/swagger.json",
                    "eVAT Workflow Sample v1");
                c.OAuthClientId(Option.ClientId);
                c.OAuthClientSecret(Option.ClientSecret);
                c.OAuthAppName("eVAT Workflow Sample v1");
                c.OAuthScopeSeparator(" ");
                c.OAuthUsePkce();
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
