using System.Threading;
using System.Threading.Tasks;
using ConstructionLine.CodingChallenge.UseCase.Models;

namespace ConstructionLine.CodingChallenge.UseCase
{
    public interface ISearchEngine
    {
        Task<SearchResults> SearchAsync(SearchOptions options, CancellationToken cancellationToken);
    }
}