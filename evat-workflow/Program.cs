using evat_workflow.Helpers;
using IdentityServer4.AccessTokenValidation;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//builder.Configuration
//       .SetBasePath(builder.Environment.ContentRootPath)
//       .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
//       .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
//       .AddEnvironmentVariables().Build();


builder.Configuration.GetSection(nameof(CustomSettings)).Bind(new CustomSettings());


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

var tt = builder.Configuration["IdpSettings:Authority"]; ;

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = bool.Parse(builder.Configuration["IdpSettings:SaveToken"]);
    options.Authority = builder.Configuration["IdpSettings:Authority"];
    options.RequireHttpsMetadata = bool.Parse(builder.Configuration["IdpSettings:RequireHttpsMetadata"]);
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

builder.Services.AddSwaggerGen(c =>
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

    //var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
    //var commentsFileName = Assembly.GetExecutingAssembly().GetName().Name + ".XML";

    //var commentsFile = Path.Combine(baseDirectory, commentsFileName);
    //c.IncludeXmlComments(commentsFile);

    //c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    //{
    //    Type = SecuritySchemeType.OAuth2,
    //    Flows = new OpenApiOAuthFlows
    //    {
    //        AuthorizationCode = new OpenApiOAuthFlow
    //        {
    //            AuthorizationUrl = new Uri(builder.Configuration["IdpSettings:AuthorityURL"]),
    //            TokenUrl = new Uri(builder.Configuration["IdpSettings:TokenUrl"]),
    //            RefreshUrl = new Uri(builder.Configuration["IdpSettings:TokenUrl"]),
    //            Scopes = new Dictionary<string, string>
    //                        {
    //                            {"evat_modular_api", "eVAT Workflow Sample - full access"}
    //                        }
    //        }
    //    },

    //});

    c.OperationFilter<EvatFilter>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseSwagger();

#if DEBUG
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "eVAT Workflow Sample v1");
    c.OAuthClientId(builder.Configuration["IdpSettings:ClientId"]);
    c.OAuthClientSecret(builder.Configuration["IdpSettings:ClientSecret"]);
    c.OAuthAppName("eVAT Workflow Sample v1");
    c.OAuthScopeSeparator(" ");
    c.OAuthUsePkce();
});
#else
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"{builder.Configuration["AppSettings:Folder"]}/swagger/v1/swagger.json",
                    "eVAT Workflow Sample v1");
                c.OAuthClientId(builder.Configuration["IdpSettings:ClientId"]);
                c.OAuthClientSecret(builder.Configuration["IdpSettings:ClientSecret"]);
                c.OAuthAppName("eVAT Workflow Sample v1");
                c.OAuthScopeSeparator(" ");
                c.OAuthUsePkce();
            });
#endif


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
