namespace SilverChess.Model
{
    public class PiecePosition
    {
        public int X { get { return x; } }
        public int Y { get { return y; } }
        public ChessPiece ItsPiece { get { return itsPiece; } set { itsPiece = value; } }

        protected int x;
        protected int y;
        protected ChessPiece itsPiece;

        private PiecePosition() { }

        public PiecePosition(int x, int y)
        {
            this.x = x;
            this.y = y;

            itsPiece = null;
        }

        public override int GetHashCode()
        {
            return (X * Y).GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != this.GetType()) return false;
            if (!ReferenceEquals(obj, this)) return false;

            PiecePosition other = obj as PiecePosition;

            return (other.X == this.X) && (other.Y == this.Y);
        }

        public static bool operator ==(PiecePosition p1, PiecePosition p2)
        {
            return p1.Equals(p2);
        }

        public static bool operator !=(PiecePosition p1, PiecePosition p2)
        {
            return !(p1 == p2);
        }
    }
}
