namespace FromAncientGreeksToModernGeeks;

public class UnitTest1
{
    [Fact]
    public void Part01()
    {
        // Item1 = # of Doctor
        // Item2 = # of Stories
        // Item3 = # of Dalek Stories
        var DoctorDalekData = new[]
        {
    (1, 29, 5),
    (2, 21, 2),
    (3, 24, 4),
    (4, 41, 2),
    (5, 20, 1),
    (6, 8, 1),
    (7, 12, 1),
    (8, 2, 0),
    (9, 10, 2),
    (10, 36, 3),
    (11, 39, 3),
    (12, 35, 1),
    (13, 26, 5)
};

        var probabilityOfDalekGivenDoctor = DoctorDalekData.Select(x =>
            (Doctor: x.Item1, Probability: x.Item3 / (decimal)x.Item2)
        );

        var totalStories = DoctorDalekData.Sum(x => x.Item2);
        var totalDalekStories = DoctorDalekData.Sum(x => x.Item3);

        var probabilityOfDoctor =
            DoctorDalekData.Select(x => (Doctor: x.Item1, Probability: (decimal)x.Item2 / totalStories))
                .ToDictionary(x => x.Doctor, x => x.Probability);
        var probabilityOfDaleks = totalDalekStories / (decimal)totalStories;

        var probabilityOfDoctorGivenDaleks = probabilityOfDalekGivenDoctor
            .Select(x => (
                    Doctor: x.Doctor,
                    Probability: x.Probability * probabilityOfDoctor[x.Doctor] / probabilityOfDaleks)
                );

        var reportLines = probabilityOfDoctorGivenDaleks.Select(x =>
            $"{x.Doctor}, {Math.Round(x.Probability, 2)}"
        );

        var reportHeader = "Doctor, Probability";

        var report = reportHeader + Environment.NewLine + string.Join(Environment.NewLine, reportLines);
    }
}
