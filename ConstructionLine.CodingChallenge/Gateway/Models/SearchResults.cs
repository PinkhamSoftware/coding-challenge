﻿using System.Collections.Generic;
using ConstructionLine.CodingChallenge.Domain;

namespace ConstructionLine.CodingChallenge.Gateway.Models
{
    public class SearchResults
    {
        public List<Shirt> Shirts { get; set; }


        public List<SizeCount> SizeCounts { get; set; }


        public List<ColorCount> ColorCounts { get; set; }
    }


    public class SizeCount
    {
        public Size Size { get; set; }

        public int Count { get; set; }
    }


    public class ColorCount
    {
        public Color Color { get; set; }

        public int Count { get; set; }
    }
}