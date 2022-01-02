using HeadingsMicroservice.Models;

namespace NewsAggregatorConsoleUI;

public interface IHeadingUiFactory
{
    HeadingUi GetHeadingUi(string number, Heading heading);
    List<HeadingUi> GetHeadingUiList(IEnumerable<Heading> headings);
}