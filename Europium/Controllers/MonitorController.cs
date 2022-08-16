using Europium.Dtos;
using Europium.Repositories;
using Europium.Repositories.Models;
using Europium.Services.Apis;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Europium.Controllers;

[ApiController]
[Route("[controller]")]
public class MonitorController : ControllerBase
{
	private readonly MonitorService _monitorService;
	
	private readonly EuropiumContext _europiumContext;
	
	public MonitorController(EuropiumContext europiumContext, MonitorService monitorService)
	{
		_europiumContext = europiumContext;
		_monitorService = monitorService;
	}
	
	[HttpGet("apis")]
	public async Task<IActionResult> GetApisToMonitor()
	{
		var apis = await _europiumContext.ApisToMonitor.Include(p => p.ApiUrls).ToListAsync();

		return Ok(apis);
	}
	
	[HttpGet("api/{apiCode}")]
	public async Task<IActionResult> GetApiById(string apiCode)
	{
		return Ok(await _monitorService.GetApiByCodeAsync(apiCode));
	}
	
	[HttpPost("api/status")]
	public async Task<IActionResult> GetApiStatus([FromBody] ApiStateDto apiStateDto)
	{
		return Ok(await _monitorService.VerifySingleApiState(apiStateDto.Code, apiStateDto.Url));
	}
	
	[HttpPost("api")]
	public async Task<IActionResult> SaveApi([FromBody] ApiToMonitor api)
	{
		if (await _monitorService.SaveApiAsync(api)) return Ok();
		
		return BadRequest();
	}
	
	[HttpGet("{apiCode}/logo")]
	public async Task<IActionResult> GetApiLogo(string apiCode)
	{
		var api = await _monitorService.GetApiByCodeAsync(apiCode);

		if (api?.Logo is null) return NotFound();

		return Ok(await _monitorService.GetApiLogoAsync(api.Logo));
	}
	
	[HttpPost("apis")]
	public async Task<IActionResult> SaveApisToMonitor([FromBody] ApiToMonitor apiToMonitor)
	{
		var apiToMonitorAdded = (await _europiumContext.ApisToMonitor.AddAsync(apiToMonitor)).Entity;
		await _europiumContext.SaveChangesAsync();
		return Ok(apiToMonitorAdded);
	}
	
	[HttpDelete("apis/{id}")]
	public async Task<IActionResult> DeleteApiById(int id)
	{
		try
		{
			_europiumContext.ApisToMonitor.Remove(new ApiToMonitor { ApiToMonitorId = id});
			await _europiumContext.SaveChangesAsync();
			return Ok();
		}
		catch (Exception)
		{
			return NotFound();
		}
	}
}