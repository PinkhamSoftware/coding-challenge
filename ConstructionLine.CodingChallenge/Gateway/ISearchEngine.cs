using System.Threading;
using System.Threading.Tasks;
using ConstructionLine.CodingChallenge.Gateway.Models;

namespace ConstructionLine.CodingChallenge.Gateway
{
    public interface ISearchEngine
    {
        Task<SearchResults> SearchAsync(SearchOptions options, CancellationToken cancellationToken);
    }
}