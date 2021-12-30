using HeadingsMicroservice.Models;

public class HeadingUiRepresentation
{
    public HeadingUiRepresentation(string number, Heading heading)
    {
        Number = number;
        Heading = heading;
    }

    public string Number { get; set; }
    public Heading Heading { get; set; }
}