namespace NewsAggregatorConsoleUI;

public interface IConsoleManager
{
    string GetNewsCount();
    string GetCategory();
    string GetNewsNumber();
    void PrintHeadings(List<HeadingUi> list);
    string PrintHeadingsAndChooseNumber(List<HeadingUi> headingUiRepresentations);
}