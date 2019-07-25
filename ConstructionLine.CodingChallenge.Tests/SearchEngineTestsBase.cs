﻿using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using ConstructionLine.CodingChallenge.Domain;
using ConstructionLine.CodingChallenge.UseCase.Models;
using NUnit.Framework;

namespace ConstructionLine.CodingChallenge.Tests
{
    public class SearchEngineTestsBase
    {
        protected static void AssertResults(List<Shirt> shirts, SearchOptions options)
        {
            Assert.That(shirts, Is.Not.Null);

            var resultingShirtIds = shirts.Select(s => s.Id).ToList();
            var sizeIds = options.Sizes.Select(s => s.Id).ToList();
            var colorIds = options.Colors.Select(c => c.Id).ToList();

            foreach (var shirt in shirts)
            {
                if (sizeIds.Contains(shirt.Size.Id)
                    && colorIds.Contains(shirt.Color.Id)
                    && !resultingShirtIds.Contains(shirt.Id))
                {
                    Assert.Fail($"'{shirt.Name}' with Size '{shirt.Size.Name}' and Color '{shirt.Color.Name}' not found in results, " +
                                $"when selected sizes where '{string.Join(",", options.Sizes.Select(s => s.Name))}' " +
                                $"and colors '{string.Join(",", options.Colors.Select(c => c.Name))}'");
                }
            }
        }


        protected static void AssertSizeCounts(List<Shirt> shirts, SearchOptions searchOptions, List<SizeCount> sizeCounts)
        {
            Assert.That(sizeCounts, Is.Not.Null);

            foreach (var size in searchOptions.Sizes)
            {
                var sizeCount = sizeCounts.SingleOrDefault(s => s.Size.Id == size.Id);

                IQueryable<Shirt> sizeQueryable = shirts.Where(w => w.Size == size).AsQueryable();
                if (searchOptions.Colors.Any())
                    sizeQueryable = sizeQueryable.Where(w => searchOptions.Colors.Contains(w.Color));

                if (sizeQueryable.Any())
                {

                    Assert.That(sizeCount, Is.Not.Null, $"Size count for '{size.Name}' not found in results");

                    var expectedSizeCount = shirts
                        .Count(s => s.Size.Id == size.Id
                                    && (!searchOptions.Colors.Any() ||
                                        searchOptions.Colors.Select(c => c.Id).Contains(s.Color.Id)));

                    Assert.That(sizeCount.Count, Is.EqualTo(expectedSizeCount),
                        $"Size count for '{sizeCount.Size.Name}' showing '{sizeCount.Count}' should be '{expectedSizeCount}'");
                }
                else
                {
                    Assert.That(sizeCount, Is.Null, $"Size count for '{size.Name}' found in results");
                }
            }
        }


        protected static void AssertColorCounts(List<Shirt> shirts, SearchOptions searchOptions, List<ColorCount> colorCounts)
        {
            Assert.That(colorCounts, Is.Not.Null);
            
            foreach (var color in searchOptions.Colors)
            {
                var colorCount = colorCounts.SingleOrDefault(s => s.Color.Id == color.Id);

                IQueryable<Shirt> colourQueryable = shirts.Where(w => w.Color == color).AsQueryable();
                if (searchOptions.Sizes.Any())
                    colourQueryable = colourQueryable.Where(w => searchOptions.Sizes.Contains(w.Size));

                if (colourQueryable.Any())
                {
                    Assert.That(colorCount, Is.Not.Null, $"Color count for '{color.Name}' not found in results");
                    var expectedColorCount = shirts
                        .Count(shirt => shirt.Color.Id == color.Id
                                        && (!searchOptions.Sizes.Any() || searchOptions.Sizes.Select(s => s.Id).Contains(shirt.Size.Id)));

                    Assert.That(colorCount.Count, Is.EqualTo(expectedColorCount),
                        $"Color count for '{colorCount.Color.Name}' showing '{colorCount.Count}' should be '{expectedColorCount}'");
                }
                else
                {
                    Assert.That(colorCount, Is.Null, $"Color count for '{color.Name}' found in results");
                }
            }
        }
    }
}