var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<CrudMVC.Services.ClienteService>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

// 👇 ESTA LÍNEA ES LA CORRECTA PARA ARCHIVOS ESTÁTICOS
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Clientes}/{action=Index}/{id?}");

app.Run();