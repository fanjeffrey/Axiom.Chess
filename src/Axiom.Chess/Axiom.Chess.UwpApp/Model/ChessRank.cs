namespace SilverChess.Model
{
    public class ChessRank
    {
        public int Y { get; set; }
        public ChessBoard Board { get; set; }
        public PiecePosition[] PiecePositions { get { return piecePositions; } }
        public PiecePosition PositionAtFirstFile { get { return PositionAtX(PositionView.FirstFileNum - 1); } }
        public PiecePosition PositionAtNinthFile { get { return PositionAtX(PositionView.NinthFileNum - 1); } }

        protected PiecePosition[] piecePositions;

        public ChessRank(ChessBoard board, int y)
        {
            Board = board;
            Y = y;

            //得到属于本行的所有棋子位置
            piecePositions = new PiecePosition[PositionView.NinthFileNum];
            for (int i = 0; i < PositionView.NinthFileNum; i++)
            {
                piecePositions[i] = board.Positions[i, y];
            }
        }

        public PiecePosition PositionAtX(int x)
        {
            return piecePositions[x];
        }

        public bool ExistPieceBetween(PiecePosition p1, PiecePosition p2)
        {
            if (p1.X == p2.X) return false;
            else if (p1.X < p2.X) return ExistPieceBetween(p1.X, p2.X);
            else return ExistPieceBetween(p2.X, p1.X);
        }

        private bool ExistPieceBetween(int x1, int x2)
        {
            return CountPiece(x1, x2) > 0;
        }

        public int CountPiece()
        {
            return CountPiece(PositionAtFirstFile, PositionAtNinthFile);
        }

        public int CountPiece(PiecePosition p1, PiecePosition p2)
        {
            if (p1.X == p2.X) return 0;
            else if (p1.X < p2.X) return CountPiece(p1.X, p2.X);
            else return CountPiece(p2.X, p1.X);
        }

        private int CountPiece(int x1, int x2)
        {
            int count = 0;

            for (int x = x1 + 1; x < x2; x++)
            {
                if (piecePositions[x].ItsPiece != null) count++;
            }

            return count;
        }
    }
}
