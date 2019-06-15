using System;
using SilverChess.Model;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace SilverChess
{
    public partial class ChessboardUI : UserControl
    {
        #region Property
        //--------------------
        public double TopMargin { get { return topMargin; } }
        public double LeftMargin { get { return leftMargin; } }
        public double FileSpace { get { return fileSpace; } }
        public double RankSpace { get { return rankSpace; } }
        public Color LineColor { get { return lineColor; } }

        public XiangQiUI XiangQi { get { return xiangQi; } }
        public PiecePoint[,] PiecePoints { get { return piecePoints; } }
        public ChessBoard Board { get { return board; } }
        //-------------
        #endregion

        #region Member
        //-------------------------
        protected double topMargin;
        protected double leftMargin;
        protected double fileSpace;
        protected double rankSpace;
        protected Color lineColor;

        protected PiecePoint[,] piecePoints;
        protected ChessBoard board;
        protected XiangQiUI xiangQi;
        //-----------------------------
        #endregion

        public ChessboardUI(XiangQiUI xiangQi, ChessBoard board)
            : this(xiangQi, board, 60, 50, 60, 50, Colors.Black)
        {
        }

        public ChessboardUI(XiangQiUI xiangQi, ChessBoard board, double leftMargin, double topMargin, double fileSpace, double rankSpace, Color lineColor)
        {
            InitializeComponent();

            this.xiangQi = xiangQi;

            this.topMargin = topMargin;
            this.leftMargin = leftMargin;
            this.fileSpace = fileSpace;
            this.rankSpace = rankSpace;
            this.lineColor = lineColor;

            this.board = board;

            BuildPiecePoints();

            DrawGrid();
            DrawDecoration();
        }

        #region private method

        private void BuildPiecePoints()
        {
            piecePoints = new PiecePoint[PositionView.NinthFileNum, PositionView.TenthRankNum];

            PiecePosition position;
            Point centerPoint;
            double hotspotRadius = (Math.Min(fileSpace, rankSpace) - 10) / 2;

            for (int i = 0; i < PositionView.NinthFileNum; i++)
            {
                for (int j = 0; j < PositionView.TenthRankNum; j++)
                {
                    position = board.PositionAt(i, j);
                    centerPoint = GetCenterPoint(position);

                    piecePoints[i, j] = new PiecePoint(position, centerPoint, hotspotRadius);
                }
            }
        }

        //画棋盘网格
        private void DrawGrid()
        {
            DrawRanks();
            DrawFiles();
        }

        //画修饰性的外围边框，士行交叉线，炮位标记，兵卒位标记
        private void DrawDecoration()
        {
            DrawBorder();
            DrawCrossLines();
            DrawCannonFlag();
            DrawSoldierFlag();
        }

        private void DrawBorder() { }
        private void DrawCannonFlag() { }
        private void DrawSoldierFlag() { }

        //画士行交叉线
        private void DrawCrossLines()
        {
            this.DecorationCanvas.Children.Add(GetLine(board.PositionAt(3, 0), board.PositionAt(5, 2)));
            this.DecorationCanvas.Children.Add(GetLine(board.PositionAt(3, 2), board.PositionAt(5, 0)));

            this.DecorationCanvas.Children.Add(GetLine(board.PositionAt(3, 7), board.PositionAt(5, 9)));
            this.DecorationCanvas.Children.Add(GetLine(board.PositionAt(3, 9), board.PositionAt(5, 7)));
        }

        //画所有行线
        private void DrawRanks()
        {
            for (int i = 0; i < board.Ranks.Length; i++)
            {
                DrawRank(board.Ranks[i]);
            }
        }

        //画所有路线
        private void DrawFiles()
        {
            for (int i = 0; i < board.Files.Length; i++)
            {
                DrawFile(board.Files[i]);
            }
        }

        private void DrawRank(ChessRank rank)
        {
            this.GridCanvas.Children.Add(GetLine(rank.PositionAtFirstFile, rank.PositionAtNinthFile));
        }

        private void DrawFile(ChessFile file)
        {
            if (file.X == PositionView.FirstFileNum - 1 || file.X == PositionView.NinthFileNum - 1)
            {
                this.GridCanvas.Children.Add(GetLine(file.PositionAtTop, file.PositionAtBottom));
            }
            else
            {
                this.GridCanvas.Children.Add(GetLine(file.PositionAtTop, file.PositionAtTopBank));
                this.GridCanvas.Children.Add(GetLine(file.PositionAtBottomBank, file.PositionAtBottom));
            }
        }

        //使用两个棋子位置构造一条线
        private Line GetLine(PiecePosition p1, PiecePosition p2)
        {
            double x1 = GetX(p1);
            double y1 = GetY(p1);
            double x2 = GetX(p2);
            double y2 = GetY(p2);

            return GetLine(x1, y1, x2, y2);
        }

        private Line GetLine(double x1, double y1, double x2, double y2)
        {
            Line line = new Line();

            line.X1 = x1;
            line.Y1 = y1;
            line.X2 = x2;
            line.Y2 = y2;

            line.Stroke = new SolidColorBrush(lineColor);
            return line;
        }

        private Point GetCenterPoint(PiecePosition p)
        {
            return new Point(GetX(p), GetY(p));
        }

        private double GetX(PiecePosition p)
        {
            return (p.X) * fileSpace + leftMargin;
        }

        private double GetY(PiecePosition p)
        {
            return (p.Y) * rankSpace + topMargin;
        }

        #endregion

        /// <summary>
        /// 取离点(x,y)最近的棋子位置
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public PiecePoint GetNearestPiecePointFrom(double x, double y)
        {
            for (int i = 0; i < PositionView.NinthFileNum; i++)
            {
                for (int j = 0; j < PositionView.TenthRankNum; j++)
                {
                    if (piecePoints[i, j].InHotspot(x, y))
                        return piecePoints[i, j];
                }
            }

            return null;
        }

        /// <summary>
        /// 取指定坐标（x,y）处的棋子位置
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public PiecePoint GetPiecePointAt(int x, int y)
        {
            return piecePoints[x, y];
        }
    }

    public class PiecePoint
    {
        public PiecePosition Position { get; set; }
        public double HotspotRadius { get; set; }
        public Point CenterPoint { get; set; }

        public PiecePoint(PiecePosition p, Point centerPoint, double hotspotRadius)
        {
            this.Position = p;
            this.CenterPoint = centerPoint;
            this.HotspotRadius = hotspotRadius;
        }

        public double GetLeft(double pieceWidth)
        {
            return CenterPoint.X - pieceWidth / 2;
        }

        public double GetTop(double pieceHeight)
        {
            return CenterPoint.Y - pieceHeight / 2;
        }

        public bool InHotspot(Point p)
        {
            return InHotspot(p.X, p.Y);
        }

        public bool InHotspot(double x, double y)
        {
            double x2 = Math.Pow((x - CenterPoint.X), 2);
            double y2 = Math.Pow((y - CenterPoint.Y), 2);

            double r = Math.Sqrt(x2 + y2);

            return r < HotspotRadius;
        }
    }
}
