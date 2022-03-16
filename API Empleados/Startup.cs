using API_Empleados.EmpleadosMapper;
using API_Empleados.Repository;
using API_Empleados.Repository.IRepository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace API_Empleados
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Crear configuracion de conexion
            services.AddDbContext<ApplicationDbContext>(Options => Options.UseSqlServer(Configuration.GetConnectionString("EmpleadosApiConexion")));


            services.AddScoped<IDepartamentoRepository, DepartmentoRepository>();
            services.AddScoped<ICargoRepository, CargoRepository>();
            //services.AddScoped<IEmpleadoRepository, EmpleadoRepository>();
            //services.AddScoped<IUsuarioRepository, UsuarioRepository>();


            //Agregar Dependencia del token
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection("AppSettings:Token").Value)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });


            //Configurar AutoMapper
            services.AddAutoMapper(typeof(EmpleadosMappers));


            //De aqui en adelante configuracion de documentacion de Nuestra API
            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("ApiEmpleados", new Microsoft.OpenApi.Models.OpenApiInfo()
                {
                    Title = "API Empleados",
                    Version = "1",
                    Description = "Backend Empleados",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                    {
                        Email = "example@gmail.com",
                        Name = "Eleikel",
                        Url = new Uri("https://www.google.com")
                    },
                    License = new Microsoft.OpenApi.Models.OpenApiLicense()
                    {
                        Name = "MIT License",
                        Url = new Uri("https://es.wikipedia.org/wiki/Licencia_MIT")
                    }
                });


                option.SwaggerDoc("ApiCargos", new Microsoft.OpenApi.Models.OpenApiInfo()
                {
                    Title = "API Cargos",
                    Version = "1",
                    Description = "Backend Empleados",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                    {
                        Email = "example@gmail.com",
                        Name = "Eleikel",
                        Url = new Uri("https://www.google.com")
                    },
                    License = new Microsoft.OpenApi.Models.OpenApiLicense()
                    {
                        Name = "MIT License",
                        Url = new Uri("https://es.wikipedia.org/wiki/Licencia_MIT")
                    }
                });


                option.SwaggerDoc("ApiDepartamentos", new Microsoft.OpenApi.Models.OpenApiInfo()
                {
                    Title = "API Departamentos",
                    Version = "1",
                    Description = "Backend Empleados",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                    {
                        Email = "example@gmail.com",
                        Name = "Eleikel",
                        Url = new Uri("https://www.google.com")
                    },
                    License = new Microsoft.OpenApi.Models.OpenApiLicense()
                    {
                        Name = "MIT License",
                        Url = new Uri("https://es.wikipedia.org/wiki/Licencia_MIT")
                    }
                });


                option.SwaggerDoc("ApiUsuarios", new Microsoft.OpenApi.Models.OpenApiInfo()
                {
                    Title = "API Usuarios",
                    Version = "1",
                    Description = "Backend Empleados",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                    {
                        Email = "example@gmail.com",
                        Name = "Eleikel",
                        Url = new Uri("https://www.google.com")
                    },
                    License = new Microsoft.OpenApi.Models.OpenApiLicense()
                    {
                        Name = "MIT License",
                        Url = new Uri("https://es.wikipedia.org/wiki/Licencia_MIT")
                    }
                });




                // Hacer que los comentarios sirvan como detalles en la documentacion del API en Swagger
                //Comentarios XML
                var archivoXmlComentarios = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var rutaApiComentarios = Path.Combine(AppContext.BaseDirectory, archivoXmlComentarios);
                option.IncludeXmlComments(rutaApiComentarios);



                //AUTENTICACION EN LA DOCUMENTACION
                //ESTO ES PARA PODER USAR EL TOKEN DEL USUARIO Y VALIDARLO PARA INGRESAR A METODOS NO AUTORIZADOS
                //Primero definir el esquema de seguridad
                option.AddSecurityDefinition("Bearer",
                     new OpenApiSecurityScheme
                     {
                         Description = "Autenticación JWT (Bearer)",
                         Type = SecuritySchemeType.Http,
                         Scheme = "Bearer"
                     });

                option.AddSecurityRequirement(new OpenApiSecurityRequirement{
                    {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Id = "Bearer",
                            Type = ReferenceType.SecurityScheme
                        }
                    }, new List<string>()
                    }

                });


            });


            services.AddControllers();

            //CORS

            services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            //Linea para documentacion API **********************************************************************
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/ApiEmpleados/swagger.json", "API Empleados");
                options.SwaggerEndpoint("/swagger/ApiCargos/swagger.json", "API Cargos");
                options.SwaggerEndpoint("/swagger/ApiDepartamentos/swagger.json", "API Departamentos");
                options.SwaggerEndpoint("/swagger/ApiUsuarios/swagger.json", "API Usuarios");

                options.RoutePrefix = ""; //Con esto nos redirecciona al Swagger Interface directamente :)
            });



            app.UseRouting();

            // Estos dos son para la Autenticacion y autorizacion ***********************

            app.UseAuthorization();

            app.UseAuthentication();
            // *******************************************************

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //CORS
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
        }
    }
}
