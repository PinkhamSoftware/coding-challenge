using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ConstructionLine.CodingChallenge.Domain;
using ConstructionLine.CodingChallenge.Gateway.Models;

namespace ConstructionLine.CodingChallenge.Gateway
{
    public class SearchEngine:ISearchEngine
    {
        private readonly List<Shirt> _shirts;

        public SearchEngine(List<Shirt> shirts)
        {
            _shirts = shirts;

            // TODO: data preparation and initialisation of additional data structures to improve performance goes here.
        }

        public async Task<SearchResults> SearchAsync(SearchOptions options, CancellationToken cancellationToken)
        {
            // TODO: search logic goes here.
            var query = _shirts.AsQueryable();

            var colourQuery = options.Colors?.Any() == true? query.Where(w => options.Colors.Contains(w.Color)): query;

            var shirtQuery = options.Sizes?.Any() == true ? colourQuery.Where(w => options.Sizes.Contains(w.Size)): colourQuery;

            var searchResults = new SearchResults
            {
                Shirts = shirtQuery.ToList(),
                ColorCounts = shirtQuery.GroupBy(g=> g.Color).Select(s=> new ColorCount
                {
                    Color = s.Key,
                    Count = s.Count()
                }).ToList(),
                SizeCounts = shirtQuery.GroupBy(g => g.Size).Select(s => new SizeCount
                {
                    Size = s.Key,
                    Count = s.Count()
                }).ToList(),
            };
            return searchResults;
        }
    }


}