using System.Drawing.Drawing2D;

namespace LineArt;

public class CircleImage : List<CirclePoint>
{
    public int Score { get; set; }

    public CircleImage()
    {
        Score = 0;
    }

    public void Draw(Bitmap bitmap)
    {
        using var g = Graphics.FromImage(bitmap);
        g.SmoothingMode = SmoothingMode.HighQuality;

        g.Clear(Color.White);

        using var pen = new Pen(Color.FromArgb(60, 0, 0, 0));

        foreach (var p in this)
        {
            var x = p.PhysicalX;
            var y = p.PhysicalY;

            foreach (var l in p.LinesTo)
            {
                var targetX = this[l].PhysicalX;
                var targetY = this[l].PhysicalY;
                g.DrawLine(pen, x, y, targetX, targetY);
            }
        }
    }

    public CircleImage Copy()
    {
        var res = new CircleImage
        {
            Score = Score
        };
        res.AddRange(this.Select(c => c.Copy()));

        return res;
    }

    public void AddRandomLines(int count)
    {
        for (var i = 0; i < count; i++)
        {
            bool again;

            do
            {
                var start = Form1.Rnd.Next(Form1.PointCount);
                var end = Form1.Rnd.Next(Form1.PointCount);

                while (end == start)
                    end = Form1.Rnd.Next(Form1.PointCount);

                if (AreConnected(start, end))
                {
                    again = true;
                }
                else
                {
                    AddLine(start, end);
                    again = false;
                }

            } while (again);
        }
    }

    private bool AreConnected(int start, int end) =>
        CheckConnection(start, end) || CheckConnection(end, start);

    private bool CheckConnection(int start, int end) =>
        this[start].IsConnectedTo(end);

    private void AddLine(int start, int end) =>
        this[start].LinesTo.Add(end);

    public void Mutate()
    {
        RemoveRandomLines(Form1.NumberOfChangesPerGeneration);
        AddRandomLines(Form1.NumberOfChangesPerGeneration);
    }

    private void RemoveRandomLines(int count)
    {
        var removed = 0;

        while (removed < count)
            if (TryRemoveRandomLine())
                removed++;
    }

    private bool TryRemoveRandomLine()
    {
        var index = Form1.Rnd.Next(Form1.PointCount);
        return this[index].RemoveRandomLine();
    }

    public void CheckScore(Bitmap target)
    {
        using var current = new Bitmap(target.Width, target.Height);
        Draw(current);
        var score = 0;

        for (var y = 0; y < target.Height; y++)
            for (var x = 0; x < target.Width; x++)
                score += Math.Abs(target.GetPixel(x, y).R - current.GetPixel(x, y).R);

        Score = score;
    }
}