using HeadingsMicroservice.Models;

namespace NewsAggregatorConsoleUI;

public class HeadingUiFactory : IHeadingUiFactory
{
    public HeadingUi GetHeadingUi(string number, Heading heading)
    {
        return new HeadingUi(number, heading);
    }

    public List<HeadingUi> GetHeadingUiList(IEnumerable<Heading> headings)
    {
        return headings.ToList().Select((heading, i) => GetHeadingUi((i + 1).ToString(), heading)).ToList();
    }
}