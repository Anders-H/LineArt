namespace LineArt;

public class CircleImageList : List<CircleImage>
{
    public CircleImage GetBest() =>
        this.OrderBy(x => x.Score).First();
}