using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Controller;
using Model;

namespace race
{
    public static class Visual
    {
        #region graphics

        private static string[] _StartHorizontal = {"--S-", "LLLL", "RRRR", "--S-"};
        private static string[] _StraightHorizontal = {"----", "LLLL", "RRRR", "----"};
        private static string[] _finishHorizontal = {"--F-", "LLLL", "RRRR", "--F-"};
        private static string[] _LeftCornerHorizontal = { "|LR|", "/LR|", "###/", "--/#" };
        private static string[] _RightCornerHorizontal = { "--\\#", "LR\\#", "\\LR|", "|LR|" };

        private static string[][] Horizontal =
            {_StraightHorizontal, _LeftCornerHorizontal, _RightCornerHorizontal, _StartHorizontal, _finishHorizontal};

        private static string[] _StartVertical = {"|LR|", "|LR|", "SLRS", "|LR|"};
        private static string[] _StraightVertical = {"|LR|", "|LR|", "|LR|", "|LR|"};
        private static string[] _finishVertical = {"|LR|", "|LR|", "FLRF", "|LR|"};
        private static string[] _LeftCornerVertical = { "|LR|", "\\LR|", "#\\##", "##\\-" };
        private static string[] _RightCornerVertical = { "##/-", "#/LR", "/LR|", "|LR|" };

        private static string[][] Vertical =
            {_StraightVertical, _LeftCornerVertical,  _RightCornerVertical, _StartVertical,  _finishVertical};

        #endregion


        public static void DrawTrack(Track track)
        {
            LinkedListNode<Section> trackPart = track.Sections.First;
            int x = 16;
            int y = 16;
            Direction direction = Direction.right;

            while (!object.Equals(trackPart, null))
            {
                Section sec = trackPart.Value;

                char[] fillLR = _getParticipantChar(sec);
                Console.SetCursorPosition(x, y);

                string[][] HOV = _HorizontalOrVerticalTrack(direction);

                if (direction == Direction.up || direction == Direction.left)
                {
                    y = y + 3;
                    for (int i = 3; i >= 0; i--)
                    {

                        Console.SetCursorPosition(x, y - i);
                        for (int j = HOV[sec.SectionType.GetHashCode()][i].Length - 1; j >= 0; j--)
                        {

                            Console.Write(HOV[sec.SectionType.GetHashCode()][i][j].ToString().Replace('L',fillLR[0]).Replace('R',fillLR[1]));
                        }
                    }

                    y = y - 3;
                }
                else
                {
                    for (int i = 0; i < 4; i++)
                    {

                        Console.SetCursorPosition(x, y + i);
                        Console.Write(HOV[sec.SectionType.GetHashCode()][i].Replace('L', fillLR[0]).Replace('R', fillLR[1]));
                    }
                }

                _findDirectionAfterTurn(sec, ref direction);

                _newTrackLocation(direction, ref x, ref y);

                
                trackPart = trackPart.Next;
            }



        }

        private static void _findDirectionAfterTurn(Section sec,ref Direction direction)
        {
            switch (sec.SectionType)
            {
                case SectionTypes.RightCorner:
                    direction = (Direction)((direction.GetHashCode() - 1 + 4) % 4);
                    break;
                case SectionTypes.LeftCorner:
                    direction = (Direction)((direction.GetHashCode() + 1 + 4) % 4);
                    break;
            }

        }

        private static void _newTrackLocation(Direction direction, ref int x, ref int y)
        {
            switch (direction)
            {
                case Direction.up:
                    y = y + 4;
                    break;
                case Direction.right:
                    x = x + 4;
                    break;
                case Direction.down:
                    y = y - 4;
                    break;
                case Direction.left:
                    x = x - 4;
                    break;
            }
        }

        private static string[][] _HorizontalOrVerticalTrack(Direction direction)
        {
            if (direction == Direction.up || direction == Direction.down)
            {
                return Vertical;
            }

            return Horizontal;
        }

        private static char[] _getParticipantChar(Section section)
        {
            SectionData data = Data.Currentrace.GetSectionData(section);
            char[] returnValue = { '#', '#' };
            if (!object.Equals(data.Left,null))
            {
                returnValue[0] = data.Left.Name.ToCharArray()[0];
            }

            if (!object.Equals(data.Right, null))
            {
                returnValue[1] = data.Right.Name.ToCharArray()[0];
            }

            return returnValue;

        }
    }

}
