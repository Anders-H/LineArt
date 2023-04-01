namespace LineArt;

public class CirclePoint
{
    public int PointIndex { get; set; }
    public int PhysicalX { get; set; }
    public int PhysicalY { get; set; }
    public List<int> LinesTo { get; }

    public CirclePoint(int pointIndex, int physicalX, int physicalY)
    {
        PointIndex = pointIndex;
        PhysicalX = physicalX;
        PhysicalY = physicalY;
        LinesTo = new List<int>();
    }

    public CirclePoint Copy()
    {
        var res = new CirclePoint(PointIndex, PhysicalX, PhysicalY);

        foreach (var l in LinesTo)
            res.LinesTo.Add(l);

        return res;
    }

    public bool IsConnectedTo(int end) =>
        LinesTo.Any(i => i == end);

    public bool RemoveRandomLine()
    {
        if (LinesTo.Count <= 0)
            return false;

        LinesTo.RemoveAt(Form1.Rnd.Next(LinesTo.Count));
        return true;
    }
}