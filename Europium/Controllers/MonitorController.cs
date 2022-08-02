using Europium.Dtos;
using Europium.Models;
using Europium.Repositories;
using Europium.Repositories.Models;
using Europium.Services.Apis;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Europium.Controllers;

[ApiController]
[Route("[controller]")]
public class MonitorController : ControllerBase
{
	private readonly MonitorService _monitorService;
	
	private readonly EuropiumContext _europiumContext;
	private readonly AppConfig AppConfig;
	
	public MonitorController(EuropiumContext europiumContext, IOptions<AppConfig> optionsSnapshot, MonitorService monitorService)
	{
		_europiumContext = europiumContext;
		_monitorService = monitorService;
		AppConfig = optionsSnapshot.Value;
	}
	
	[HttpGet("apis")]
	public async Task<IActionResult> GetApisToMonitor()
	{
		var apis = await _europiumContext.ApisToMonitor.Include(p => p.ApiUrls).ToListAsync();

		return Ok(apis);
	}
	
	[HttpPost("api/status")]
	public async Task<IActionResult> GetApisToMonitor([FromBody] ApiStateDto apiStateDto)
	{
		return Ok(await _monitorService.VerifySingleApiState(apiStateDto.Code, apiStateDto.Url));
	}
	
	[HttpGet("{apiCode}/logo")]
	public async Task<IActionResult> GetApiLogo(string apiCode)
	{
		var api = await _monitorService.GetApiByCodeAsync(apiCode);

		if (api is null) return NotFound();

		return Ok(await _monitorService.GetApiLogoAsync(api.Logo));
	}
	
	[HttpPost("apis")]
	public async Task<IActionResult> SaveApisToMonitor([FromBody] ApiToMonitor? apiToMonitor)
	{
		var apiToMonitorAdded = (await _europiumContext.ApisToMonitor.AddAsync(apiToMonitor)).Entity;
		await _europiumContext.SaveChangesAsync();
		return Ok(apiToMonitorAdded);
	}
	
	[HttpDelete("apis/{id}")]
	public async Task<IActionResult> SaveApisToMonitor(int id)
	{
		try
		{
			_europiumContext.ApisToMonitor.Remove(new ApiToMonitor() { ApiToMonitorId = id});
			await _europiumContext.SaveChangesAsync();
			return Ok();
		}
		catch (Exception)
		{
			return NotFound();
		}
	}
}