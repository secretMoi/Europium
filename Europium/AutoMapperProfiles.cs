using AutoMapper;
using Europium.Repositories.Models;

namespace Europium;

public class AutoMapperProfiles : Profile
{
	public AutoMapperProfiles()
	{
		CreateMap<ApiToMonitor, ApiToMonitor>();
	}
	
}