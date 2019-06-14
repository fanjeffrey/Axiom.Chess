namespace SilverChess.Model
{
    public class Inning
    {
        //public Stack<Step> Steps { get; set; }
        public ChessBoard Board { get { return board; } }
        public RedTeam RedTeam { get { return redTeam; } }
        public BlackTeam BlackTeam { get { return blackTeam; } }

        protected ChessBoard board;
        protected RedTeam redTeam;
        protected BlackTeam blackTeam;

        public Inning(bool redTeamAtBottom)
        {
            board = new ChessBoard();
            ArrangeTeam(redTeamAtBottom);
        }

        private void ArrangeTeam(bool redTeamAtBottom)
        {
            if (redTeamAtBottom)
            {
                redTeam = new RedTeam(new MyView(board));
                blackTeam = new BlackTeam(new EnemyView(board));
            }
            else
            {
                redTeam = new RedTeam(new EnemyView(board));
                blackTeam = new BlackTeam(new MyView(board));
            }
        }
    }



    public class Step
    {
        public ChessPiece Piece { get; set; }
        public PiecePosition FromPosition { get; set; }
        public PiecePosition ToPosition { get; set; }
        public ChessPiece KilledPiece { get { return killedPiece; } }

        protected ChessPiece killedPiece;

        public Step(ChessPiece piece, PiecePosition from, PiecePosition to)
        {
            this.Piece = piece;
            this.FromPosition = from;
            this.ToPosition = to;

            killedPiece = to.ItsPiece;
        }
    }
}
