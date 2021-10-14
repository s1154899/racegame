using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Controller;
using Model;

namespace raceWPF
{
    public class VisualizeTrack
    {
        private readonly Bitmap _Corner;

        private readonly Bitmap[] _Corners;
        private readonly Bitmap _finish;
        private readonly Bitmap[] _finishes;
        private readonly Bitmap _start;
        private readonly Bitmap[] _starts;
        private readonly Bitmap _Straight;
        private readonly Bitmap[] _Straights;
        private Dictionary<string, Bitmap[]>[] cars;


        private readonly Dictionary<string, Bitmap[]> carsColored;
        private readonly Dictionary<string, Bitmap[]> carsColoredBroken;
        public int height;


        private readonly List<visualTrackData> map = new List<visualTrackData>();

        public int width;

        public VisualizeTrack()
        {
            carsColored = new Dictionary<string, Bitmap[]>();
            carsColoredBroken = new Dictionary<string, Bitmap[]>();

            cars = new[] {carsColored, carsColoredBroken};

            _Corner = PictureHandler.GetImageBitmap(@"fotos/cornerBottomToLeft.png");
            _Straight = PictureHandler.GetImageBitmap(@"fotos/straightLeftToRight.png");
            _finish = PictureHandler.GetImageBitmap(@"fotos/straightLeftToRightFinish.png");
            _start = PictureHandler.GetImageBitmap(@"fotos/straightLeftToRightStart.png");


            _Corners = new[]
            {
                (Bitmap) _Corner.Clone(), (Bitmap) _Corner.Clone(), (Bitmap) _Corner.Clone(), (Bitmap) _Corner.Clone()
            };

            _Corners[1].RotateFlip(RotateFlipType.Rotate90FlipNone);
            _Corners[2].RotateFlip(RotateFlipType.Rotate180FlipNone);
            _Corners[3].RotateFlip(RotateFlipType.Rotate270FlipNone);

            _Straights = new[] {(Bitmap) _Straight.Clone(), (Bitmap) _Straight.Clone()};
            _Straights[1].RotateFlip(RotateFlipType.Rotate90FlipNone);

            _finishes = new[] {(Bitmap) _finish.Clone(), (Bitmap) _finish.Clone()};
            _finishes[1].RotateFlip(RotateFlipType.Rotate90FlipNone);

            _starts = new[] {(Bitmap) _start.Clone(), (Bitmap) _start.Clone()};
            _starts[1].RotateFlip(RotateFlipType.Rotate90FlipNone);
        }

        private Bitmap[][] GetCar(TeamColors color)
        {
            Bitmap[][] bitmaps;
            Bitmap[] carsRotated;
            Bitmap[] carsBrokenRotated;
            if (!carsColored.TryGetValue(color.ToString(), out carsRotated))
            {
                var car = PictureHandler.GetImageCarsBitmap(color);
                carsRotated = new[]
                    {(Bitmap) car.Clone(), (Bitmap) car.Clone(), (Bitmap) car.Clone(), (Bitmap) car.Clone()};
                carsRotated[1].RotateFlip(RotateFlipType.Rotate270FlipNone);
                carsRotated[2].RotateFlip(RotateFlipType.Rotate180FlipNone);
                carsRotated[3].RotateFlip(RotateFlipType.Rotate90FlipNone);

                carsColored.Add(color.ToString(), carsRotated);
            }

            if (!carsColoredBroken.TryGetValue(color.ToString(), out carsBrokenRotated))
            {
                var carBroken = PictureHandler.GetImageCarsBrokenBitmap(color);
                carsBrokenRotated = new[]
                {
                    (Bitmap) carBroken.Clone(), (Bitmap) carBroken.Clone(), (Bitmap) carBroken.Clone(),
                    (Bitmap) carBroken.Clone()
                };
                carsBrokenRotated[1].RotateFlip(RotateFlipType.Rotate270FlipNone);
                carsBrokenRotated[2].RotateFlip(RotateFlipType.Rotate180FlipNone);
                carsBrokenRotated[3].RotateFlip(RotateFlipType.Rotate90FlipNone);

                carsColoredBroken.Add(color.ToString(), carsBrokenRotated);
            }

            bitmaps = new[] {carsRotated, carsBrokenRotated};
            return bitmaps;
        }

        public Bitmap DrawTrack(Track track)
        {
            var drawFont = new Font("Arial", 8);
            var drawBrush = new SolidBrush(Color.Black);

            var screenMap = (Bitmap) PictureHandler.GetImageBitmap("EmptyGreen").Clone();

            using (var graph = Graphics.FromImage(screenMap))
            {
                foreach (var visualData in map)
                {
                    //Rectangle ImageSize = new Rectangle(mapX.Key * 100, mapY.Key * 100, 100, 100);
                    if (visualData.section.SectionType == SectionTypes.Straight)
                        graph.DrawImage(_Straights[(visualData.Direction.GetHashCode() + 1) % 2], visualData.X * 100, visualData.Y * 100, 100, 100);
                    else if (visualData.section.SectionType == SectionTypes.StartGrid)
                        graph.DrawImage(_starts[(visualData.Direction.GetHashCode() + 1) % 2], visualData.X * 100,
                            visualData.Y * 100, 100, 100);
                    else if (visualData.section.SectionType == SectionTypes.Finish)
                        graph.DrawImage(_finishes[(visualData.Direction.GetHashCode() + 1) % 2], visualData.X * 100,
                            visualData.Y * 100, 100, 100);
                    else
                        graph.DrawImage(
                            _Corners[
                                (visualData.Direction.GetHashCode() + 3 *
                                    (visualData.section.SectionType == SectionTypes.LeftCorner).GetHashCode()) % 4],
                            visualData.X * 100, visualData.Y * 100, 100, 100);

                    if (!Equals(Data.Currentrace, null))
                    {
                        int RightDown =
                            (visualData.Direction == Direction.right || visualData.Direction == Direction.down)
                            .GetHashCode();

                        int LeftUp = (visualData.Direction == Direction.left || visualData.Direction == Direction.up)
                            .GetHashCode();

                        

                        var data = Data.Currentrace.GetSectionData(visualData.section);

                        if (!Equals(data.Left, null))
                        {
                            int CalcRightDownX = visualData.X * 100 + 50 * ((visualData.Direction.GetHashCode() + 1) % 2) +
                                                  data.DistanceLeft / 2 * (visualData.Direction.GetHashCode() % 2);

                            int CalcRightDownY = visualData.Y * 100 + 50 * (visualData.Direction.GetHashCode() % 2) +
                                                 data.DistanceLeft / 2 * ((visualData.Direction.GetHashCode() + 1) % 2);

                            int CalcLeftUpX = visualData.X * 100 + 50 * ((visualData.Direction.GetHashCode() + 1) % 2) +
                                              (75 - data.DistanceLeft / 2) * (visualData.Direction.GetHashCode() % 2);

                            int CalcLeftUpY = visualData.Y * 100 +
                                              (75 - data.DistanceLeft / 2) *
                                              ((visualData.Direction.GetHashCode() + 1) % 2) +
                                              50 * (visualData.Direction.GetHashCode() % 2);

                            graph.DrawImage(
                                GetCar(data.Left.TeamColor)[data.Left.Equipment.isBroken.GetHashCode()][
                                    3 - visualData.Direction.GetHashCode()], (CalcRightDownX * RightDown) + (CalcLeftUpX * LeftUp)
                                , (CalcRightDownY * RightDown) + (CalcLeftUpY * LeftUp)
                                , 25, 25);

                            
                        }

                        if (!Equals(data.Right, null))
                        {

                            int CalcRightDownX = visualData.X * 100 + 25 * ((visualData.Direction.GetHashCode() + 1) % 2) +
                                                 data.DistanceRight / 2 * (visualData.Direction.GetHashCode() % 2);

                            int CalcRightDownY = visualData.Y * 100 + 25 * (visualData.Direction.GetHashCode() % 2) +
                                                 data.DistanceRight / 2 * ((visualData.Direction.GetHashCode() + 1) % 2);

                            int CalcLeftUpX =
                                visualData.X * 100 + 25 * ((visualData.Direction.GetHashCode() + 1) % 2) +
                                (75 - data.DistanceRight / 2) * (visualData.Direction.GetHashCode() % 2);

                            int CalcLeftUpY = visualData.Y * 100 +
                                              (75 - data.DistanceRight / 2) * ((visualData.Direction.GetHashCode() + 1) % 2) +
                                              25 * (visualData.Direction.GetHashCode() % 2);

                            graph.DrawImage(
                                GetCar(data.Right.TeamColor)[data.Right.Equipment.isBroken.GetHashCode()][
                                    3 - visualData.Direction.GetHashCode()], (CalcRightDownX * RightDown) + (CalcLeftUpX * LeftUp)
                                , (CalcRightDownY * RightDown) + (CalcLeftUpY * LeftUp)
                                , 25, 25);

                        }

                    }

//graph.FillRectangle(System.Drawing.Brushes.Aqua, ImageSize);
                    graph.DrawString($"{visualData.Direction.ToString()} : {visualData.section.SectionType.ToString()}",
                        drawFont, drawBrush, visualData.X * 100 + 50, visualData.Y * 100 + 50);
                }
            }

            return screenMap;
        }

        public void mapTrack(Track track)
        {
            var direction = Direction.right;
            var x = 0;
            var y = 0;


            foreach (var trackParts in track.Sections)
            {
                map.Add(new visualTrackData(trackParts, direction, x, y));
                if (trackParts.SectionType == SectionTypes.LeftCorner ||
                    trackParts.SectionType == SectionTypes.RightCorner)
                    _findDirectionAfterTurn(trackParts, ref direction);


                switch (direction)
                {
                    case Direction.down:
                        y++;
                        break;
                    case Direction.up:
                        y--;
                        break;
                    case Direction.left:
                        x--;
                        break;
                    case Direction.right:
                        x++;
                        break;
                }
            }


            width = (map.Min(I => I.X) - map.Max(I => I.X)) * -1 * 100;
            height = (map.Min(I => I.Y) - map.Max(I => I.Y)) * -1 * 100;

            var lowestY = map.Min(I => I.Y) * -1;
            var lowestX = map.Min(I => I.X) * -1;

            foreach (var t in map)
            {
                t.X = t.X + lowestX;
                t.Y = t.Y + lowestY;
            }

            var b = map.Count;
        }

        private static void _findDirectionAfterTurn(Section sec, ref Direction direction)
        {
            switch (sec.SectionType)
            {
                case SectionTypes.RightCorner:
                    direction = (Direction) ((direction.GetHashCode() - 1 + 4) % 4);
                    break;
                case SectionTypes.LeftCorner:
                    direction = (Direction) ((direction.GetHashCode() + 1 + 4) % 4);
                    break;
            }
        }
    }

    internal class visualTrackData
    {
        public Direction Direction;
        public Section section;

        public int X;
        public int Y;


        public visualTrackData()
        {
        }

        public visualTrackData(Section section, Direction direction, int x, int y) : this(section, direction)
        {
            X = x;
            Y = y;
        }

        public visualTrackData(Section section, Direction direction)
        {
            this.section = section;
            Direction = direction;
        }
    }
}