using System.Collections.Generic;
using SilverChess.Model;
using Windows.UI;
using Windows.UI.Xaml.Controls;

namespace SilverChess
{
    public partial class XiangQiUI : UserControl
    {
        protected ChessboardUI boardUI;
        protected List<ChessPieceUI> pieceUIList = new List<ChessPieceUI>();
        
        public XiangQiUI()
        {
            InitializeComponent();

            Load();
        }

        public void Load()
        {
            bool redTeamAtBottom = true;

            Inning aInningOfChess = new Inning(redTeamAtBottom);

            //draw board
            boardUI = new ChessboardUI(this, aInningOfChess.Board);
            BoardCanvas.Children.Add(boardUI);

            ChessPiece piece;
            ChessPieceUI pieceUI;

            //draw pieces of black team
            BlackTeam blackTeam = aInningOfChess.BlackTeam;
            PieceVisual visualOfBlackTeam = new PieceVisual(Colors.Black);
            for (int i = 0; i < blackTeam.Pieces.Count; i++)
            {
                piece = blackTeam.Pieces[i];

                pieceUI = new ChessPieceUI(this, piece);

                AddPieceUI(pieceUI,visualOfBlackTeam);
            }

            //draw piece of red team
            RedTeam redTeam = aInningOfChess.RedTeam;
            PieceVisual visualOfRedTeam = new PieceVisual(Colors.Red);
            for (int i = 0; i < redTeam.Pieces.Count; i++)
            {
                piece = redTeam.Pieces[i];

                pieceUI = new ChessPieceUI(this, piece);

                AddPieceUI(pieceUI,visualOfRedTeam);
            }
        }

        public PiecePoint GetPiecePoint(ChessPieceUI pieceUI)
        {
            return boardUI.GetPiecePointAt(pieceUI.Piece.StartPosition.X, pieceUI.Piece.StartPosition.Y);
        }

        public PiecePoint GetNearestPiecePointFrom(double centerX, double centerY)
        {
            return boardUI.GetNearestPiecePointFrom(centerX, centerY);
        }

        public void KickoutPiece(ChessPiece chessPiece)
        {
            RemovePieceUI(GetPieceUI(chessPiece));                
        }

        private ChessPieceUI GetPieceUI(ChessPiece chessPiece)
        {
            foreach (ChessPieceUI pieceUI in pieceUIList)
            {
                if (pieceUI.Piece == chessPiece) return pieceUI;
            }

            return null;
        }

        private void RemovePieceUI(ChessPieceUI pieceUI)
        {
            if (pieceUI == null) return;

            PieceCanvas.Children.Remove(pieceUI);
            KilledPiecesPanel1.Children.Add(pieceUI);
        }

        private void AddPieceUI(ChessPieceUI pieceUI,PieceVisual visual)
        {
            if (pieceUI == null) return;

            pieceUI.ApplyVisual(visual);

            pieceUIList.Add(pieceUI);
            PieceCanvas.Children.Add(pieceUI);
        }
    }


}
