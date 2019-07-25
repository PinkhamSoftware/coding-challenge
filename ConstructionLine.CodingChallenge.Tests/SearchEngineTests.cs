using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ConstructionLine.CodingChallenge.Domain;
using ConstructionLine.CodingChallenge.Gateway;
using ConstructionLine.CodingChallenge.Gateway.Models;
using NUnit.Framework;

namespace ConstructionLine.CodingChallenge.Tests
{
    [TestFixture]
    public class SearchEngineTests : SearchEngineTestsBase
    {
        [Test]
        public async Task can_find_correct_shirt_options()
        {
            var shirts = new List<Shirt>
            {
                new Shirt(Guid.NewGuid(), "Red - Small", Size.Small, Color.Red),
                new Shirt(Guid.NewGuid(), "Black - Medium", Size.Medium, Color.Black),
                new Shirt(Guid.NewGuid(), "Blue - Large", Size.Large, Color.Blue),
            };

            var searchEngine = new SearchEngine(shirts);

            var searchOptions = new SearchOptions
            {
                Colors = new List<Color> {Color.Red}
            };

            var results = await searchEngine.SearchAsync(searchOptions, CancellationToken.None).ConfigureAwait(false);

            AssertResults(results.Shirts, searchOptions);
        }

        [Test]
        public async Task can_find_correct_size_counts()
        {
            var shirts = new List<Shirt>
            {
                new Shirt(Guid.NewGuid(), "Red - Small", Size.Small, Color.Red),
                new Shirt(Guid.NewGuid(), "Black - Medium", Size.Medium, Color.Black),
                new Shirt(Guid.NewGuid(), "Blue - Large", Size.Large, Color.Blue),
            };

            var searchEngine = new SearchEngine(shirts);

            var searchOptions = new SearchOptions
            {
                Sizes = new List<Size> { Size.Small},
                Colors = new List<Color> { Color.Red }
            };

            var results = await searchEngine.SearchAsync(searchOptions, CancellationToken.None).ConfigureAwait(false);

            AssertSizeCounts(shirts, searchOptions, results.SizeCounts);
        }

        [Test]
        public async Task can_find_correct_colour_counts()
        {
            var shirts = new List<Shirt>
            {
                new Shirt(Guid.NewGuid(), "Red - Small", Size.Small, Color.Red),
                new Shirt(Guid.NewGuid(), "Black - Medium", Size.Medium, Color.Black),
                new Shirt(Guid.NewGuid(), "Blue - Large", Size.Large, Color.Blue),
            };

            var searchEngine = new SearchEngine(shirts);

            var searchOptions = new SearchOptions
            {
                Colors = new List<Color> { Color.Red }
            };

            var results = await searchEngine.SearchAsync(searchOptions, CancellationToken.None).ConfigureAwait(false);

            AssertColorCounts(shirts, searchOptions, results.ColorCounts);
        }

        [Test]
        public async Task returns_no_results_when_query_doesnt_match()
        {
            var shirts = new List<Shirt>
            {
                new Shirt(Guid.NewGuid(), "Red - Small", Size.Small, Color.Red),
                new Shirt(Guid.NewGuid(), "Black - Medium", Size.Medium, Color.Black),
                new Shirt(Guid.NewGuid(), "Blue - Large", Size.Large, Color.Blue),
            };

            var searchEngine = new SearchEngine(shirts);

            var searchOptions = new SearchOptions
            {
                Sizes = new List<Size> { Size.Small },
                Colors = new List<Color> { Color.Black }
            };

            var results = await searchEngine.SearchAsync(searchOptions, CancellationToken.None).ConfigureAwait(false);

            AssertResults(results.Shirts, searchOptions);
        }

        [Test]
        public async Task can_find_correct_size_counts_when_size_isnt_in_set()
        {
            var shirts = new List<Shirt>
            {
                new Shirt(Guid.NewGuid(), "Red - Small", Size.Small, Color.Red),
                new Shirt(Guid.NewGuid(), "Black - Medium", Size.Medium, Color.Black),
            };

            var searchEngine = new SearchEngine(shirts);

            var searchOptions = new SearchOptions
            {
                Sizes = new List<Size> { Size.Large}
            };

            var results = await searchEngine.SearchAsync(searchOptions, CancellationToken.None).ConfigureAwait(false);

            AssertSizeCounts(shirts, searchOptions, results.SizeCounts);
        }

        [Test]
        public async Task can_find_correct_colour_counts_when_colour_isnt_in_set()
        {
            var shirts = new List<Shirt>
            {
                new Shirt(Guid.NewGuid(), "Red - Small", Size.Small, Color.Red),
                new Shirt(Guid.NewGuid(), "Black - Medium", Size.Medium, Color.Black),
            };

            var searchEngine = new SearchEngine(shirts);

            var searchOptions = new SearchOptions
            {
                Colors = new List<Color> { Color.White },
            };

            var results = await searchEngine.SearchAsync(searchOptions, CancellationToken.None).ConfigureAwait(false);

            AssertColorCounts(shirts, searchOptions, results.ColorCounts);
        }

        [Test]
        public async Task returns_correct_results_when_colour_isnt_specified()
        {
            var shirts = new List<Shirt>
            {
                new Shirt(Guid.NewGuid(), "Red - Small", Size.Small, Color.Red),
                new Shirt(Guid.NewGuid(), "Black - Medium", Size.Medium, Color.Black),
                new Shirt(Guid.NewGuid(), "Blue - Large", Size.Large, Color.Blue),
                new Shirt(Guid.NewGuid(), "Yellow - Large", Size.Large, Color.Yellow),
            };

            var searchEngine = new SearchEngine(shirts);

            var searchOptions = new SearchOptions
            {
                Sizes = new List<Size> { Size.Large}
            };

            var results = await searchEngine.SearchAsync(searchOptions, CancellationToken.None).ConfigureAwait(false);

            AssertResults(results.Shirts, searchOptions);
            AssertColorCounts(shirts, searchOptions, results.ColorCounts);
            AssertSizeCounts(shirts, searchOptions, results.SizeCounts);
        }

        [Test]
        public async Task returns_correct_results_when_size_isnt_specified()
        {
            var shirts = new List<Shirt>
            {
                new Shirt(Guid.NewGuid(), "Red - Small", Size.Small, Color.Red),
                new Shirt(Guid.NewGuid(), "Black - Medium", Size.Medium, Color.Black),
                new Shirt(Guid.NewGuid(), "Black - Large", Size.Large, Color.Black),
                new Shirt(Guid.NewGuid(), "Blue - Large", Size.Large, Color.Blue),
            };

            var searchEngine = new SearchEngine(shirts);

            var searchOptions = new SearchOptions
            {
                Colors = new List<Color> { Color.Black}
            };

            var results = await searchEngine.SearchAsync(searchOptions, CancellationToken.None).ConfigureAwait(false);

            AssertResults(results.Shirts, searchOptions);
            AssertColorCounts(shirts, searchOptions, results.ColorCounts);
            AssertSizeCounts(shirts, searchOptions, results.SizeCounts);
        }

        [Test]
        public async Task can_find_correct_size_and_colour_counts()
        {
            var shirts = new List<Shirt>
            {
                new Shirt(Guid.NewGuid(), "Red - Small", Size.Small, Color.Red),
                new Shirt(Guid.NewGuid(), "Red - Medium", Size.Medium, Color.Red),
                new Shirt(Guid.NewGuid(), "Black - Medium", Size.Medium, Color.Black),
                new Shirt(Guid.NewGuid(), "Blue - Large", Size.Large, Color.Blue),
            };

            var searchEngine = new SearchEngine(shirts);

            var searchOptions = new SearchOptions
            {
                Sizes = new List<Size> {Size.Small, Size.Medium},
                Colors = new List<Color> {Color.Red}
            };

            var results = await searchEngine.SearchAsync(searchOptions, CancellationToken.None).ConfigureAwait(false);

            AssertResults(results.Shirts, searchOptions);
            AssertColorCounts(shirts, searchOptions, results.ColorCounts);
            AssertSizeCounts(shirts, searchOptions, results.SizeCounts);
        }

        [Test]
        public async Task can_find_correct_size_and_colour_counts_with_multiple_colours_and_sizes()
        {
            var shirts = new List<Shirt>
            {
                new Shirt(Guid.NewGuid(), "Red - Small", Size.Small, Color.Red),
                new Shirt(Guid.NewGuid(), "Red - Medium", Size.Medium, Color.Red),
                new Shirt(Guid.NewGuid(), "Black - Medium", Size.Medium, Color.Black),
                new Shirt(Guid.NewGuid(), "Blue - Large", Size.Large, Color.Blue),
            };

            var searchEngine = new SearchEngine(shirts);

            var searchOptions = new SearchOptions
            {
                Sizes = new List<Size> { Size.Small, Size.Medium },
                Colors = new List<Color> { Color.Black, Color.Red }
            };

            var results = await searchEngine.SearchAsync(searchOptions, CancellationToken.None).ConfigureAwait(false);

            AssertResults(results.Shirts, searchOptions);
            AssertColorCounts(shirts, searchOptions, results.ColorCounts);
            AssertSizeCounts(shirts, searchOptions, results.SizeCounts);
        }


        [Test]
        public async Task can_find_correct_size_and_colour_counts_with_multiple_colours_and_sizes_2()
        {
            var shirts = new List<Shirt>
            {
                new Shirt(Guid.NewGuid(), "Red - Small", Size.Small, Color.Red),
                new Shirt(Guid.NewGuid(), "Red - Medium", Size.Medium, Color.Red),
                new Shirt(Guid.NewGuid(), "Black - Medium", Size.Medium, Color.Black),
                new Shirt(Guid.NewGuid(), "Blue - Large", Size.Large, Color.Blue),
            };

            var searchEngine = new SearchEngine(shirts);

            var searchOptions = new SearchOptions
            {
                Sizes = new List<Size> { Size.Small, Size.Medium },
                Colors = new List<Color> { Color.Blue, Color.Red }
            };

            var results = await searchEngine.SearchAsync(searchOptions, CancellationToken.None).ConfigureAwait(false);

            AssertResults(results.Shirts, searchOptions);
            AssertColorCounts(shirts, searchOptions, results.ColorCounts);
            AssertSizeCounts(shirts, searchOptions, results.SizeCounts);
        }

        [Test]
        public async Task returns_all_results_if_no_search_options_are_specified()
        {
            var shirts = new List<Shirt>
            {
                new Shirt(Guid.NewGuid(), "Red - Small", Size.Small, Color.Red),
                new Shirt(Guid.NewGuid(), "Red - Medium", Size.Medium, Color.Red),
                new Shirt(Guid.NewGuid(), "Black - Medium", Size.Medium, Color.Black),
                new Shirt(Guid.NewGuid(), "Blue - Large", Size.Large, Color.Blue),
            };

            var searchEngine = new SearchEngine(shirts);

            var searchOptions = new SearchOptions
            {

            };

            var results = await searchEngine.SearchAsync(searchOptions, CancellationToken.None).ConfigureAwait(false);

            AssertResults(results.Shirts, searchOptions);
            AssertColorCounts(shirts, searchOptions, results.ColorCounts);
            AssertSizeCounts(shirts, searchOptions, results.SizeCounts);
        }
    }
}