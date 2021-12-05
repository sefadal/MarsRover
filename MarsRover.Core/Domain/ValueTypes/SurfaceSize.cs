namespace MarsRover.Core.Domain.ValueTypes
{
    public class SurfaceSize
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public SurfaceSize(int width, int height)
        {
            Width = width;
            Height = height;
        }
    }
}
