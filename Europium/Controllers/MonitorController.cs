using AutoMapper;
using Europium.Dtos;
using Europium.Models;
using Europium.Repositories;
using Europium.Repositories.Models;
using Europium.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Europium.Controllers;

[ApiController]
[Route("[controller]")]
public class MonitorController : ControllerBase
{
	private readonly MonitorService _monitorService;
	private readonly IMapper  _mapper;
	
	private readonly EuropiumContext _europiumContext;
	private readonly AppConfig AppConfig;
	
	public MonitorController(EuropiumContext europiumContext, IOptions<AppConfig> optionsSnapshot, MonitorService monitorService, IMapper mapper)
	{
		_europiumContext = europiumContext;
		_monitorService = monitorService;
		_mapper = mapper;
		AppConfig = optionsSnapshot.Value;
	}
	
	[HttpGet("apis")]
	public async Task<IActionResult> GetApisToMonitor()
	{
		var apis = await _europiumContext.ApisToMonitor.Include(p => p.ApiUrls).ToListAsync();

		foreach (var api in apis)
		{
			api.Logo = $"{Request.Scheme}://{Request.Host.Value}/{AppConfig.ApiToMonitorImagePath}/{api.Logo}";
		}
		
		var monitoredApiDto = _mapper.Map<List<MonitoredApiDto>>(apis);
		
		_monitorService.VerifyAllApisState(monitoredApiDto);
		
		return Ok(monitoredApiDto);
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