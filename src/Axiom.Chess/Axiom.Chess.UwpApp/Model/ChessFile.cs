namespace SilverChess.Model
{
    public class ChessFile
    {
        public int X { get { return x; } }
        public ChessBoard Board { get; set; }
        public PiecePosition[] PiecePositions { get { return piecePositions; } }
        public PiecePosition PositionAtTop { get { return PositionAtY(PositionView.FirstRankNum - 1); } }
        public PiecePosition PositionAtTopBank { get { return PositionAtY(PositionView.FifthFileNum - 1); } }
        public PiecePosition PositionAtBottomBank { get { return PositionAtY(PositionView.SixthFileNum - 1); } }
        public PiecePosition PositionAtBottom { get { return PositionAtY(PositionView.TenthRankNum - 1); } }

        protected PiecePosition[] piecePositions;
        protected int x;

        public ChessFile(ChessBoard board, int x)
        {
            Board = board;
            this.x = x;

            //得到属于本列的所有棋子位置
            piecePositions = new PiecePosition[PositionView.TenthRankNum];
            for (int i = 0; i < PositionView.TenthRankNum; i++)
            {
                piecePositions[i] = board.Positions[x, i];
            }
        }

        public PiecePosition PositionAtY(int y)
        {
            return PiecePositions[y];
        }

        public bool ExistPieceBetween(PiecePosition p1, PiecePosition p2)
        {
            if (p1.Y == p2.Y) return false;
            else if (p1.Y < p2.Y) return ExistPieceBetween(p1.Y, p2.Y);
            else return ExistPieceBetween(p2.Y, p1.Y);
        }

        private bool ExistPieceBetween(int y1, int y2)
        {
            return CountPiece(y1, y2) > 0;
        }

        public int CountPiece()
        {
            return CountPiece(PositionAtTop, PositionAtBottom);
        }

        public int CountPiece(PiecePosition p1, PiecePosition p2)
        {
            if (p1.Y == p2.Y) return 0;
            else if (p1.Y < p2.Y) return CountPiece(p1.Y, p2.Y);
            else return CountPiece(p2.Y, p1.Y);
        }

        private int CountPiece(int y1, int y2)
        {
            int count = 0;

            for (int y = y1 + 1; y < y2; y++)
            {
                if (piecePositions[y].ItsPiece != null) count++;
            }

            return count;
        }
    }
}
