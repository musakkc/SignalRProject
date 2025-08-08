using SignalR.BusinessLayer.Abstract;
using SignalR.BusinessLayer.Concrete;
using SignalR.DataAccessLayer.Abstract;
using SignalR.DataAccessLayer.Concrete;
using SignalR.DataAccessLayer.EntityFramework;
using SignalRApi.Hubs;
using System.Reflection;

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

builder.Services.AddScoped<IAboutService, AboutManager>();
builder.Services.AddScoped<IAboutDal, EfAboutDal>();

builder.Services.AddScoped<IBookingService, BookingManager>();
builder.Services.AddScoped<IBookingDal, EfBookingDal>();

builder.Services.AddScoped<ICategoryService, CategoryManager>();
builder.Services.AddScoped<ICategoryDal, EfCategoryDal>();

builder.Services.AddScoped<IContactService, ContactManager>();
builder.Services.AddScoped<IContactDal, EfContactDal>();

builder.Services.AddScoped<IDiscountService, DiscountManager>();
builder.Services.AddScoped<IDiscountDal, EfDiscountDal>();

builder.Services.AddScoped<IFeatureService, FeatureManager>();
builder.Services.AddScoped<IFeatureDal, EfFeatureDal>();

builder.Services.AddScoped<IProductService, ProductManager>();
builder.Services.AddScoped<IProductDal, EfProductDal>();

builder.Services.AddScoped<ISocialMediaService, SocialMediaManager>();
builder.Services.AddScoped<ISocialMediaDal, EfSocialMediaDal>();

builder.Services.AddScoped<ITestimonialService, TestimonialManager>();
builder.Services.AddScoped<ITestimonialDal, EfTestimonialDal>();

builder.Services.AddScoped<IOrderService, OrderManager>();
builder.Services.AddScoped<IOrderDal, EfOrderDal>();

builder.Services.AddScoped<IOrderDetailService, OrderDetailManager>();
builder.Services.AddScoped<IOrderDetailDal, EfOrderDetailDal>();

builder.Services.AddScoped<IMoneyCaseService, MoneyCaseManager>();
builder.Services.AddScoped<IMoneyCaseDal, EfMoneyCaseDal >();

builder.Services.AddScoped<IMenuTableService, MenuTableManager>();
builder.Services.AddScoped<IMenuTableDal, EfMenuTableDal>();

builder.Services.AddScoped<ISliderService, SliderManager>();
builder.Services.AddScoped<ISliderDal, EfSliderDal>();


builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddControllersWithViews(); // ----

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
//app.UseRouting(); // ----

// Configure the HTTP request pipeline.
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
