using System;
using System.Collections.Generic;

namespace SilverChess.Model
{
    public abstract class ChessPiece
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public Team ItsTeam { get; set; }
        public PiecePosition StartPosition { get { return startPosition; } }
        public PiecePosition CurrentPosition { get { return currentPosition; } }


        protected List<PiecePosition> validPositions;
        protected PiecePosition startPosition;
        protected PiecePosition currentPosition;

        private ChessPiece() { }
        public ChessPiece(string code, string name, Team t, int fileNum, int rankNum)
        {
            this.Code = code;
            this.Name = name;
            this.ItsTeam = t;

            SetStartPosition(t.View.GetPiecePositionAt(fileNum, rankNum));

            this.validPositions = new List<PiecePosition>();
        }

        public Step MoveTo(PiecePosition targetPosition)
        {
            if (!IsValidPosition(targetPosition)) return null;

            Step s = new Step(this, CurrentPosition, targetPosition);
            ItsTeam.Steps.Push(s);

            SetCurrentPosition(targetPosition);

            return s;
        }

        public void SetStartPosition(PiecePosition targetPosition)
        {
            targetPosition.ItsPiece = this;
            this.startPosition = targetPosition;
            this.currentPosition = targetPosition;
        }

        private void SetCurrentPosition(PiecePosition targetPosition)
        {
            if (this.currentPosition != null) this.currentPosition.ItsPiece = null;

            targetPosition.ItsPiece = this;
            this.currentPosition = targetPosition;
        }

        public virtual bool IsValidPosition(PiecePosition targetPosition)
        {
            if (targetPosition == null) return false;
            if (!AmongTheValidPositions(targetPosition)) return false;//目标位置不在合法位置集合中，则目标集团无效
            if (ExistAPieceOfTheSameTeamOn(targetPosition)) return false;//目标位置存在已方棋子，则目标位置无效

            return true;
        }

        //目标位置是否存在已方棋子
        protected virtual bool ExistAPieceOfTheSameTeamOn(PiecePosition targetPosition)
        {
            return (targetPosition.ItsPiece != null && targetPosition.ItsPiece.ItsTeam.Name == this.ItsTeam.Name);
        }

        //目标位置是否在有效位置之中
        protected virtual bool AmongTheValidPositions(PiecePosition targetPosition)
        {
            bool amongTheValidPositions = false;

            foreach (PiecePosition p in validPositions)
            {
                if (targetPosition == p) { amongTheValidPositions = true; continue; }
            }

            return amongTheValidPositions;
        }
    }

    public class KingPiece : ChessPiece
    {
        public KingPiece(string name, Team t, int fileNum, int rankNum)
            : base("King", name, t, fileNum, rankNum)
        {
            this.validPositions.Add(t.View.GetPiecePositionAt(4, 1));
            this.validPositions.Add(t.View.GetPiecePositionAt(5, 1));
            this.validPositions.Add(t.View.GetPiecePositionAt(6, 1));
            this.validPositions.Add(t.View.GetPiecePositionAt(4, 2));
            this.validPositions.Add(t.View.GetPiecePositionAt(5, 2));
            this.validPositions.Add(t.View.GetPiecePositionAt(6, 2));
            this.validPositions.Add(t.View.GetPiecePositionAt(4, 3));
            this.validPositions.Add(t.View.GetPiecePositionAt(5, 3));
            this.validPositions.Add(t.View.GetPiecePositionAt(6, 3));
        }

        public override bool IsValidPosition(PiecePosition targetPosition)
        {
            if (!base.IsValidPosition(targetPosition)) return false;

            bool oneStepAlongHorizontal = Math.Abs(targetPosition.X - CurrentPosition.X) == 1;
            bool zeroStepAlongHorizontal = (targetPosition.X - CurrentPosition.X) == 0;
            bool oneStepAlongVertical = Math.Abs(targetPosition.Y - CurrentPosition.Y) == 1;
            bool zeroStepAlongVertical = (targetPosition.Y - CurrentPosition.Y) == 0;

            return (oneStepAlongHorizontal && zeroStepAlongVertical) || (oneStepAlongVertical && zeroStepAlongHorizontal);
        }
    }

    public class GuarderPiece : ChessPiece
    {
        public GuarderPiece(string name, Team t, int fileNum, int rankNum)
            : base("Guard", name, t, fileNum, rankNum)
        {
            this.validPositions.Add(t.View.GetPiecePositionAt(4, 1));
            this.validPositions.Add(t.View.GetPiecePositionAt(6, 1));
            this.validPositions.Add(t.View.GetPiecePositionAt(5, 2));
            this.validPositions.Add(t.View.GetPiecePositionAt(4, 3));
            this.validPositions.Add(t.View.GetPiecePositionAt(6, 3));
        }

        public override bool IsValidPosition(PiecePosition targetPosition)
        {
            if (!base.IsValidPosition(targetPosition)) return false;

            bool oneStepAlongHorizontal = Math.Abs(targetPosition.X - CurrentPosition.X) == 1;
            bool oneStepAlongVertical = Math.Abs(targetPosition.Y - CurrentPosition.Y) == 1;

            return (oneStepAlongHorizontal && oneStepAlongVertical);
        }
    }

    public class MinisterPiece : ChessPiece
    {
        public MinisterPiece(string name, Team t, int fileNum, int rankNum)
            : base("Minister", name, t, fileNum, rankNum)
        {
            this.validPositions.Add(t.View.GetPiecePositionAt(3, 1));
            this.validPositions.Add(t.View.GetPiecePositionAt(7, 1));
            this.validPositions.Add(t.View.GetPiecePositionAt(1, 3));
            this.validPositions.Add(t.View.GetPiecePositionAt(5, 3));
            this.validPositions.Add(t.View.GetPiecePositionAt(9, 3));
            this.validPositions.Add(t.View.GetPiecePositionAt(3, 5));
            this.validPositions.Add(t.View.GetPiecePositionAt(7, 5));
        }

        public override bool IsValidPosition(PiecePosition targetPosition)
        {
            if (!base.IsValidPosition(targetPosition)) return false;

            bool twoStepAlongHorizontal = Math.Abs(targetPosition.X - CurrentPosition.X) == 2;
            bool twoStepAlongVertical = Math.Abs(targetPosition.Y - CurrentPosition.Y) == 2;

            if (!(twoStepAlongHorizontal && twoStepAlongVertical)) return false;

            bool noPieceOnElephantEye = (GetPieceOnCurrentElephantEye(targetPosition) == null);

            return noPieceOnElephantEye;
        }

        private ChessPiece GetPieceOnCurrentElephantEye(PiecePosition targetPosition)
        {
            int offsetAlongHorizontal = ((targetPosition.X - currentPosition.X) < 0) ? -1 : 1;
            int offsetAlongVertical = ((targetPosition.Y - currentPosition.Y) < 0) ? -1 : 1;

            int fileNum = ItsTeam.View.GetFileNum(currentPosition.X + offsetAlongHorizontal);
            int rankNum = ItsTeam.View.GetRankNum(currentPosition.Y + offsetAlongVertical);

            PiecePosition p = ItsTeam.View.GetPiecePositionAt(fileNum, rankNum);

            return (p != null) ? p.ItsPiece : null;
        }
    }

    public class HorsePiece : ChessPiece
    {
        public HorsePiece(string name, Team t, int fileNum, int rankNum)
            : base("Horse", name, t, fileNum, rankNum) { }

        public override bool IsValidPosition(PiecePosition targetPosition)
        {
            if (!base.IsValidPosition(targetPosition)) return false;

            bool twoStepAlongHorizontal = Math.Abs(targetPosition.X - CurrentPosition.X) == 2;
            bool oneStepAlongVertical = Math.Abs(targetPosition.Y - CurrentPosition.Y) == 1;

            bool oneStepAlongHorizontal = Math.Abs(targetPosition.X - currentPosition.X) == 1;
            bool twoStepAlongVertical = Math.Abs(targetPosition.Y - CurrentPosition.Y) == 2;

            if (!(twoStepAlongHorizontal && oneStepAlongVertical || oneStepAlongHorizontal && twoStepAlongVertical)) return false;

            bool noPieceOnHorseLeg = (GetPieceOnCurrentHorseLeg(targetPosition) == null);

            return noPieceOnHorseLeg;
        }

        protected override bool AmongTheValidPositions(PiecePosition targetPosition)
        {
            return true;//由于“马”可以去棋盘上的任意位置，所以不用比较，总是为True
        }

        private ChessPiece GetPieceOnCurrentHorseLeg(PiecePosition targetPosition)
        {
            int fileNum = -1;
            int rankNum = -1;

            if (Math.Abs(targetPosition.X - currentPosition.X) == 2)
            {
                int offsetAlongHorizontal = ((targetPosition.X - currentPosition.X) < 0) ? -1 : 1;
                fileNum = ItsTeam.View.GetFileNum(currentPosition.X + offsetAlongHorizontal);
                rankNum = ItsTeam.View.GetRankNum(currentPosition.Y);
            }
            else if (Math.Abs(targetPosition.Y - currentPosition.Y) == 2)
            {
                int offsetAlongVertical = ((targetPosition.Y - currentPosition.Y) < 0) ? -1 : 1;
                fileNum = ItsTeam.View.GetFileNum(currentPosition.X);
                rankNum = ItsTeam.View.GetRankNum(currentPosition.Y + offsetAlongVertical);
            }

            PiecePosition p = ItsTeam.View.GetPiecePositionAt(fileNum, rankNum);

            return (p != null) ? p.ItsPiece : null;
        }
    }

    public class ChariotPiece : ChessPiece
    {
        public ChariotPiece(string name, Team t, int fileNum, int rankNum) : base("Chariot", name, t, fileNum, rankNum) { }

        public override bool IsValidPosition(PiecePosition targetPosition)
        {
            if (!base.IsValidPosition(targetPosition)) return false;

            if ((targetPosition.X != currentPosition.X) && (targetPosition.Y != currentPosition.Y)) return false;//目标位置与当前位置不在同一条直线，则目标位置无效

            if (ExistPieceBetweenCurrentPositionAnd(targetPosition)) return false;//目标位置与当前位置之间有棋子，则目标位置无效

            return true;
        }

        protected override bool AmongTheValidPositions(PiecePosition targetPosition)
        {
            return true;//由于“车”可以去棋盘上的任意位置，所以不用比较，总是为True
        }

        private bool ExistPieceBetweenCurrentPositionAnd(PiecePosition targetPosition)
        {
            bool exist = false;

            if (targetPosition.X == currentPosition.X)
            {
                ChessFile file = ItsTeam.View.Board.FileAt(currentPosition.X);
                exist = file.ExistPieceBetween(targetPosition, currentPosition);

            }
            else if (targetPosition.Y == currentPosition.Y)
            {
                ChessRank rank = ItsTeam.View.Board.RankAt(currentPosition.Y);
                exist = rank.ExistPieceBetween(targetPosition, currentPosition);
            }

            return exist;
        }
    }

    public class CannonPiece : ChessPiece
    {
        public CannonPiece(string name, Team t, int fileNum, int rankNum)
            : base("Cannon", name, t, fileNum, rankNum) { }

        public override bool IsValidPosition(PiecePosition targetPosition)
        {
            if (!base.IsValidPosition(targetPosition)) return false;

            if ((targetPosition.X != currentPosition.X) && (targetPosition.Y != currentPosition.Y)) return false;//目标位置与当前位置不在同一条直线，则目标位置无效

            if (targetPosition.ItsPiece != null)
            {
                if (!ExistAPieceBetweenCurrentPositionAnd(targetPosition)) return false;//如果目标位置有棋子，但是目标位置与当前位置之间的棋子数不等于一，则目标位置无效
            }
            else
            {
                if (!NoPieceBetweenCurrentPositionAnd(targetPosition)) return false;//如果目标位置无棋子，但是目标位置与当前位置之间的棋子数不等于零，则目标位置无效
            }

            return true;
        }

        protected override bool AmongTheValidPositions(PiecePosition targetPosition)
        {
            return true;//由于“炮”可以去棋盘上的任意位置，所以不用比较，总是为True
        }

        private bool ExistAPieceBetweenCurrentPositionAnd(PiecePosition targetPosition)
        {
            bool exist = false;

            if (targetPosition.X == currentPosition.X)
            {
                ChessFile file = ItsTeam.View.Board.FileAt(currentPosition.X);
                exist = file.CountPiece(targetPosition, currentPosition) == 1;

            }
            else if (targetPosition.Y == currentPosition.Y)
            {
                ChessRank rank = ItsTeam.View.Board.RankAt(currentPosition.Y);
                exist = rank.CountPiece(targetPosition, currentPosition) == 1;
            }

            return exist;
        }

        public bool NoPieceBetweenCurrentPositionAnd(PiecePosition targetPosition)
        {
            bool exist = false;

            if (targetPosition.X == currentPosition.X)
            {
                ChessFile file = ItsTeam.View.Board.FileAt(currentPosition.X);
                exist = file.CountPiece(targetPosition, currentPosition) == 0;

            }
            else if (targetPosition.Y == currentPosition.Y)
            {
                ChessRank rank = ItsTeam.View.Board.RankAt(currentPosition.Y);
                exist = rank.CountPiece(targetPosition, currentPosition) == 0;
            }

            return exist;
        }
    }

    public class SoldierPiece : ChessPiece
    {
        public SoldierPiece(string name, Team t, int fileNum, int rankNum) : base("Soldier", name, t, fileNum, rankNum)
        {
            validPositions.Add(t.View.GetPiecePositionAt(t.View.GetFileNum(startPosition.X), t.View.GetRankNum(startPosition.Y) + 1));

            for (int f = PositionView.NinthFileNum; f >= PositionView.FirstFileNum; f--)
                for (int r = PositionView.TenthRankNum; r >= PositionView.SixthRankNum; r--)
                    validPositions.Add(t.View.GetPiecePositionAt(f, r));
        }

        public override bool IsValidPosition(PiecePosition targetPosition)
        {
            if (!base.IsValidPosition(targetPosition)) return false;

            bool oneStepAlongHorizontal = Math.Abs(targetPosition.X - CurrentPosition.X) == 1;
            bool zeroStepAlongHorizontal = (targetPosition.X - CurrentPosition.X) == 0;
            bool oneStepAlongVertical = Math.Abs(targetPosition.Y - CurrentPosition.Y) == 1;
            bool zeroStepAlongVertical = (targetPosition.Y - CurrentPosition.Y) == 0;

            bool notBack = ItsTeam.View.GetRankNum(targetPosition.Y) - ItsTeam.View.GetRankNum(currentPosition.Y) >= 0;

            if (!((oneStepAlongHorizontal && zeroStepAlongVertical || zeroStepAlongHorizontal && oneStepAlongVertical) && notBack)) return false;

            return true;
        }
    }
}
