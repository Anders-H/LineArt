using System.Drawing.Imaging;

namespace LineArt;

public partial class Form1 : Form
{
    private readonly Bitmap _bitmap;
    private CircleImage _circleImage;
    private readonly CircleImageList _nextGeneration;
    private int _generationCount = 0;
    private int _savedImagesCounter = 0;
    private const int ImagesPerGeneration = 20;
    public const int NumberOfLinesOnImage = 1000;
    public const int NumberOfChangesPerGeneration = 3;
    public static Random Rnd = new();
    public const int PointCount = 126;
    public const int ImageWidth = 256;
    public const int ImageHeight = 256;
    public Bitmap Target { get; }

    public Form1()
    {
        InitializeComponent();
        _bitmap = new Bitmap(ImageWidth, ImageHeight);
        _circleImage = new CircleImage();
        _nextGeneration = new CircleImageList();
        CreatePoints();
        _circleImage.AddRandomLines(NumberOfLinesOnImage);
        Target = (Bitmap)Image.FromFile("../../../../target.jpg");
    }

    private void Form1_Shown(object sender, EventArgs e)
    {
        timer1.Enabled = true;
    }

    private void Timer1_Tick(object sender, EventArgs e)
    {
        _generationCount++;
        CreateGenerations();
        CheckScore();
        _circleImage = _nextGeneration.GetBest().Copy();

        if (_generationCount % 20 == 0)
        {
            Text = $@"Gen {_generationCount}: {_circleImage.Score} error score";
            _circleImage.Draw(_bitmap);
            _savedImagesCounter++;
            _bitmap.Save($@"D:\Temp\LineArt\{_savedImagesCounter:00000}.png", ImageFormat.Png);
            Invalidate();
        }
    }

    private void CreateGenerations()
    {
        _nextGeneration.Clear();

        for (var i = 0; i < ImagesPerGeneration; i++)
        {
            var n = _circleImage.Copy();
            n.Mutate();
            _nextGeneration.Add(n);
        }
    }

    private void CheckScore()
    {
        foreach (var n in _nextGeneration)
            n.CheckScore(Target);
    }

    private void Form1_Paint(object sender, PaintEventArgs e)
    {
        e.Graphics.Clear(Color.Blue);
        e.Graphics.DrawImage(_bitmap, 0, 0);
    }

    private void CreatePoints()
    {
        _circleImage.Clear();
        var centerX = ImageWidth / 2.0;
        var centerY = ImageHeight / 2.0;
        var radius = centerX - 3;
        for (var i = 0; i < PointCount; i++)
        {
            var x = (int)Math.Round(centerX + Math.Sin(i / 20.05) * radius);
            var y = (int)Math.Round(centerY + Math.Cos(i / 20.05) * radius);
            _circleImage.Add(new CirclePoint(i, x, y));
        }
    }
}