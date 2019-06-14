namespace SilverChess.Model
{
    public abstract class PositionView
    {
        #region ChessFile Num
        public const int FirstFileNum = 1;
        public const int SecondFileNum = 2;
        public const int ThirdFileNum = 3;
        public const int ForthFileNum = 4;
        public const int FifthFileNum = 5;
        public const int SixthFileNum = 6;
        public const int SeventhFileNum = 7;
        public const int EighthFileNum = 8;
        public const int NinthFileNum = 9;
        #endregion

        #region ChessRank Num
        public const int FirstRankNum = 1;
        public const int SecondRankNum = 2;
        public const int ThirdRankNum = 3;
        public const int ForthRankNum = 4;
        public const int FifthRankNum = 5;
        public const int SixthRankNum = 6;
        public const int SeventhRankNum = 7;
        public const int EighthRankNum = 8;
        public const int NinthRankNum = 9;
        public const int TenthRankNum = 10;
        #endregion

        public ChessBoard Board { get { return board; } }
        protected ChessBoard board;

        private PositionView() { }
        public PositionView(ChessBoard board)
        {
            this.board = board;
        }

        public abstract int GetFileNum(PiecePosition position);
        public abstract int GetFileNum(int x);
        public abstract int GetRankNum(PiecePosition position);
        public abstract int GetRankNum(int y);


        /// <summary>
        /// 取与指定的fileNum 和 rankNum相匹配的棋子位置
        /// </summary>
        /// <param name="f">从1到9之间的一位整数，包含1和9</param>
        /// <param name="r">从1到10之间的一位整数，包含1和10</param>
        /// <returns></returns>
        public abstract PiecePosition GetPiecePositionAt(int fileNum, int rankNum);
    }

    public class MyView : PositionView
    {
        public MyView(ChessBoard board) : base(board) { }

        public override PiecePosition GetPiecePositionAt(int fileNum, int rankNum)
        {
            int boardX = PositionView.NinthFileNum - fileNum;
            int boardY = PositionView.TenthRankNum - rankNum;

            return Board.PositionAt(boardX, boardY);
        }

        public override int GetFileNum(PiecePosition position)
        {
            return GetFileNum(position.X);
        }

        public override int GetRankNum(PiecePosition position)
        {
            return GetRankNum(position.Y);
        }

        public override int GetFileNum(int x)
        {
            return PositionView.NinthFileNum - x;
        }

        public override int GetRankNum(int y)
        {
            return PositionView.TenthRankNum - y;
        }
    }

    public class EnemyView : PositionView
    {
        public EnemyView(ChessBoard board) : base(board) { }

        public override PiecePosition GetPiecePositionAt(int fileNum, int rankNum)
        {
            int boardX = fileNum - 1;
            int boardY = rankNum - 1;

            return Board.PositionAt(boardX, boardY);
        }

        public override int GetFileNum(PiecePosition position)
        {
            return GetFileNum(position.X);
        }

        public override int GetRankNum(PiecePosition position)
        {
            return GetRankNum(position.Y);
        }

        public override int GetFileNum(int x)
        {
            return x + 1;
        }

        public override int GetRankNum(int y)
        {
            return y + 1;
        }
    }

}
