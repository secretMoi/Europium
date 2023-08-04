using Europium.Repositories;

namespace Europium.Services.Apis.FlareSolver;

public class FlareSolverService
{
    private readonly FlareSolverRepository _flareSolverRepository;

    public FlareSolverService(FlareSolverRepository flareSolverRepository)
    {
        _flareSolverRepository = flareSolverRepository;
    }

    public async Task<bool> IsUpAsync(string url)
    {
        return await _flareSolverRepository.IsUpAsync(url);
    }
}