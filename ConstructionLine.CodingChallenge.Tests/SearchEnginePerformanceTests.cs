using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using ConstructionLine.CodingChallenge.Domain;
using ConstructionLine.CodingChallenge.Tests.SampleData;
using ConstructionLine.CodingChallenge.UseCase;
using ConstructionLine.CodingChallenge.UseCase.Models;
using NUnit.Framework;

namespace ConstructionLine.CodingChallenge.Tests
{
    [TestFixture]
    public class SearchEnginePerformanceTests : SearchEngineTestsBase
    {
        private List<Shirt> _shirts;
        private ISearchEngine _searchEngine;

        [SetUp]
        public void Setup()
        {
            
            var dataBuilder = new SampleDataBuilder(50000);

            _shirts = dataBuilder.CreateShirts();

            _searchEngine = new SearchEngine(_shirts);
        }


        [Test]
        public async Task PerformanceTest()
        {
            var sw = new Stopwatch();
            sw.Start();

            var options = new SearchOptions
            {
                Colors = new List<Color> { Color.Red }
            };

            var results = await  _searchEngine.SearchAsync(options, CancellationToken.None).ConfigureAwait(false);

            sw.Stop();
            Console.WriteLine($"Test fixture finished in {sw.ElapsedMilliseconds} milliseconds");

            AssertResults(results.Shirts, options);
            AssertSizeCounts(_shirts, options, results.SizeCounts);
            AssertColorCounts(_shirts, options, results.ColorCounts);
        }
    }
}
