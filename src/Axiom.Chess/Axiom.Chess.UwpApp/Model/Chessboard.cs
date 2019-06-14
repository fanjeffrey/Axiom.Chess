namespace SilverChess.Model
{

    public class ChessBoard
    {
        public PiecePosition[,] Positions { get { return piecePositions; } }
        public ChessRank[] Ranks { get { return ranks; } }
        public ChessFile[] Files { get { return files; } }
        //public int MinX { get { return minX; } }
        //public int MaxX { get { return maxX; } }
        //public int MinY { get { return minY; } }
        //public int MaxY { get { return maxY; } }


        protected PiecePosition[,] piecePositions;
        protected ChessRank[] ranks;
        protected ChessFile[] files;

        protected int minX = PositionView.FirstFileNum - 1;
        protected int maxX = PositionView.NinthFileNum - 1;
        protected int minY = PositionView.FirstRankNum - 1;
        protected int maxY = PositionView.TenthRankNum - 1;




        public ChessBoard()
        {
            //构建棋子位置
            piecePositions = new PiecePosition[PositionView.NinthFileNum, PositionView.TenthRankNum];
            for (int x = minX; x <= maxX; x++)
            {
                for (int y = minY; y <= maxY; y++)
                {
                    piecePositions[x, y] = new PiecePosition(x, y);
                }
            }

            //构建棋盘上的所有纵路
            files = new ChessFile[PositionView.NinthFileNum];
            for (int x = minX; x <= maxX; x++)
            {
                files[x] = new ChessFile(this, x);
            }

            //构建棋盘上的所有行线
            ranks = new ChessRank[PositionView.TenthRankNum];
            for (int y = minY; y <= maxY; y++)
            {
                ranks[y] = new ChessRank(this, y);
            }
        }

        /// <summary>
        /// 取指定x，指定y的棋子位置
        /// </summary>
        /// <param name="x">位于从0到8之间的一位整数，包括0和8</param>
        /// <param name="y">位于从0到9之间的一位整数，包括0和9</param>
        /// <returns>棋子位置(PiecePosition)。未找到返回null。</returns>
        public PiecePosition PositionAt(int x, int y)
        {
            if (x < minX || x > maxX) return null;

            return Positions[x, y];
        }

        public ChessFile FileAt(int x)
        {
            if (x < minX || x > maxX) return null;

            return Files[x];
        }

        public ChessRank RankAt(int y)
        {
            if (y < minY || y > maxY) return null;

            return Ranks[y];
        }

    }

}
