using System.Configuration;
using System.Net;
using Domain.Constants;
using Infrastructure;
using Microsoft.AspNetCore.HttpOverrides;
using WebUI;
using WebUI.CodeMegaSignalR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddWebDependency(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.UseStatusCodePages(context =>
{
    var response = context.HttpContext.Response;
    var method = context.HttpContext.Request.Method;

    if (!method.ToUpper().Equals("GET")) return Task.CompletedTask;
    
    switch (response.StatusCode)
    {
        case (int)HttpStatusCode.NotFound:
            response.Redirect("/page-not-found");
            break;
        case (int)HttpStatusCode.Unauthorized:
            response.Redirect("/sign-in");
            break;
        case (int)HttpStatusCode.Forbidden:
            response.Redirect("/no-permission");
            break;
    }

    return Task.CompletedTask;
});

// Configure the HTTP request pipeline.
app.UseExceptionHandler("/error");
// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
    app.Use((context, next) =>
    {
        context.Request.Scheme = "https";
        return next(context);
    });
}

app.UseSession();

app.Use(async (context, next) =>
{
    var jwtToken = context.Session.GetString(AppConst.SessionJwtKey);
    // var jwtToken = context.Request.GetCookie(SystemConst.AppSecure);
    if (!string.IsNullOrEmpty(jwtToken))
        context.Request.Headers.Add("Authorization", "Bearer " + jwtToken);
    await next();
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedProto
});

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    endpoints.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller=Home}/{action=Index}");
    
    endpoints.MapHub<CodeMegaSignalR>("/realtime");
});

app.Run();