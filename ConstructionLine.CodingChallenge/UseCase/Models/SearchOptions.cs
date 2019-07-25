using System.Collections.Generic;
using ConstructionLine.CodingChallenge.Domain;

namespace ConstructionLine.CodingChallenge.UseCase.Models
{
    public class SearchOptions
    {
        public List<Size> Sizes { get; set; } = new List<Size>();

        public List<Color> Colors { get; set; } = new List<Color>();
    }
}