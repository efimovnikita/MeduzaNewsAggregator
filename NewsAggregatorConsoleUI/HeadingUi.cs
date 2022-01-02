using HeadingsMicroservice.Models;

namespace NewsAggregatorConsoleUI;

public class HeadingUi
{
    public HeadingUi(string number, Heading heading)
    {
        Number = number;
        Heading = heading;
    }

    public string Number { get; set; }
    public Heading Heading { get; set; }
}