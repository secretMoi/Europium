using AutoMapper;
using Europium.Repositories.Models;

namespace Europium.Dtos;

public class MapperProfile : Profile
{
	public MapperProfile()
	{
		CreateMap<MonitoredApiDto, ApiToMonitor>().ReverseMap();
	}
}