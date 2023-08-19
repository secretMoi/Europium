using Microsoft.Extensions.FileProviders;

namespace Europium.Helpers.Extensions;

public static class WebApplicationExtension
{
	public static void SetupPipeline(this WebApplication app, WebApplicationBuilder builder)
	{
		// Configure the HTTP request pipeline.
		if (app.Environment.IsDevelopment())
		{
			app.UseSwagger();
			app.UseSwaggerUI();
		}

		//app.UseHttpsRedirection();

		var configProgram = app.Services.GetService<ConfigProgram>()!;
		app.UseStaticFiles(new StaticFileOptions
		{
			FileProvider = new PhysicalFileProvider(
				Path.Combine(builder.Environment.ContentRootPath, configProgram.RessourcesPath
				)),
			RequestPath = "/Ressources"
		});

		app.UseCors();

		app.UseAuthentication();
		app.UseAuthorization();

		app.MapControllers();

		app.Run();
	}
}