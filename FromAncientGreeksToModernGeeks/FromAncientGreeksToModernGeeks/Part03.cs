using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FromAncientGreeksToModernGeeks
{
    public class ColourRow
    {
        public string Name { get; set; }
        public string Colour { get; set; }
    }

    public class Colour
    {
        public string Name { get; set; }
        public int Red { get; set; }
        public int Green { get; set; }
        public int Blue { get; set; }
    }

    public class Part03
    {
        [Fact]
        public void Test03()
        {
            using var tRead = new StreamReader("colours.csv");
            using var csv = new CsvHelper.CsvReader(tRead, CultureInfo.InvariantCulture);
            var colours = csv.GetRecords<ColourRow>()
                .Distinct()
                .Select(x => new Colour
                {
                    Name = x.Name,
                    Red = int.Parse(x.Colour.Split(";")[0]),
                    Green = int.Parse(x.Colour.Split(";")[1]),
                    Blue = int.Parse(x.Colour.Split(";")[2])
                }).ToArray();

            var rnd = new Random();

            var red = rnd.Next(255);
            var green = rnd.Next(255);
            var blue = rnd.Next(255);


            var coloursSorted = colours.Select(x => (
                x.Name,
                x.Red,
                x.Green,
                x.Blue,
                Distance: Math.Sqrt(
                    ((red - x.Red) * (red - x.Red)) +
                    ((green - x.Green) * (green - x.Green)) +
                    ((blue - x.Blue) * (blue - x.Blue))

                )

            )).OrderBy(x => x.Distance)
                .ToArray();
        }
    }
}
