using System.Collections.Generic;

namespace SilverChess.Model
{
    public abstract class Team
    {
        public string Name { get; set; }
        public PositionView View { get; set; }
        public List<ChessPiece> Pieces { get; set; }
        public Stack<Step> Steps { get; set; }

        private Team() { }
        public Team(string name, PositionView pv)
        {
            this.Name = name;
            this.View = pv;
            this.Pieces = new List<ChessPiece>();
            this.Steps = new Stack<Step>();
        }
    }

    public class RedTeam : Team
    {
        public RedTeam(PositionView pv)
            : base("Red Team", pv)
        {
            Pieces.Add(new ChariotPiece("俥", this, 1, 1));
            Pieces.Add(new HorsePiece("傌", this, 2, 1));
            Pieces.Add(new MinisterPiece("相", this, 3, 1));
            Pieces.Add(new GuarderPiece("仕", this, 4, 1));
            Pieces.Add(new KingPiece("帥", this, 5, 1));
            Pieces.Add(new GuarderPiece("仕", this, 6, 1));
            Pieces.Add(new MinisterPiece("相", this, 7, 1));
            Pieces.Add(new HorsePiece("傌", this, 8, 1));
            Pieces.Add(new ChariotPiece("俥", this, 9, 1));

            Pieces.Add(new CannonPiece("炮", this, 2, 3));
            Pieces.Add(new CannonPiece("炮", this, 8, 3));

            Pieces.Add(new SoldierPiece("兵", this, 1, 4));
            Pieces.Add(new SoldierPiece("兵", this, 3, 4));
            Pieces.Add(new SoldierPiece("兵", this, 5, 4));
            Pieces.Add(new SoldierPiece("兵", this, 7, 4));
            Pieces.Add(new SoldierPiece("兵", this, 9, 4));
        }
    }

    public class BlackTeam : Team
    {
        public BlackTeam(PositionView pv)
            : base("Black Team", pv)
        {
            Pieces.Add(new ChariotPiece("車", this, 1, 1));
            Pieces.Add(new HorsePiece("馬", this, 2, 1));
            Pieces.Add(new MinisterPiece("象", this, 3, 1));
            Pieces.Add(new GuarderPiece("士", this, 4, 1));
            Pieces.Add(new KingPiece("將", this, 5, 1));
            Pieces.Add(new GuarderPiece("士", this, 6, 1));
            Pieces.Add(new MinisterPiece("象", this, 7, 1));
            Pieces.Add(new HorsePiece("馬", this, 8, 1));
            Pieces.Add(new ChariotPiece("車", this, 9, 1));

            Pieces.Add(new CannonPiece("砲", this, 2, 3));
            Pieces.Add(new CannonPiece("砲", this, 8, 3));

            Pieces.Add(new SoldierPiece("卒", this, 1, 4));
            Pieces.Add(new SoldierPiece("卒", this, 3, 4));
            Pieces.Add(new SoldierPiece("卒", this, 5, 4));
            Pieces.Add(new SoldierPiece("卒", this, 7, 4));
            Pieces.Add(new SoldierPiece("卒", this, 9, 4));
        }
    }
}
