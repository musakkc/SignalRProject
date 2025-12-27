using SignalR.BusinessLayer.ValidationRules.BookingValidations;
using SignalR.DataAccessLayer.Concrete;
using SignalRApi.Hubs;
using System.Reflection;
using System.Text.Json.Serialization;
using FluentValidation;
using SignalR.BusinessLayer.Container;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(opt =>
{
    opt.AddPolicy("CorsPolicy", builder =>
    {
        builder.AllowAnyHeader() // builder arayüzü, tüm baþlýklarý kabul eder
               .AllowAnyMethod() // builder arayüzü, tüm HTTP yöntemlerini (GET, POST, PUT, DELETE vb.) kabul eder
              .SetIsOriginAllowed((host) => true) // builder arayüzü, tüm kaynaklardan gelen istekleri kabul eder
              .AllowCredentials(); // builder arayüzü, kimlik doðrulama bilgilerini (örneðin, çerezler veya HTTP baþlýklarý) içeren istekleri kabul eder
    });
});
builder.Services.AddSignalR(); 

builder.Services.AddDbContext<SignalRContext>();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.ContainerDependencies();

builder.Services.AddValidatorsFromAssemblyContaining<CreateBookingValidation>();

builder.Services.AddControllersWithViews().AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddControllers();
 

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
//app.UseRouting(); // ----
 
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("CorsPolicy");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
//app.MapDefaultControllerRoute(); // ---- // MVC Routing'i tanýmlar

app.MapHub<SignalRHub>("/signalrhub"); // SignalRHub sýnýfý, SignalR ile istemcilerle iletiþim kurmak için kullanýlacak.
//localhost://5000/signalrhub istekte bulunabileceðim (örneðin)

app.Run();
