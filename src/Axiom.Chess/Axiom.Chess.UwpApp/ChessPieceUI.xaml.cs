using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using SilverChess.Model;

namespace SilverChess
{
    public partial class ChessPieceUI : UserControl
    {
        public double pieceWidth = 50;
        public double pieceHeight = 50;

        public XiangQiUI XiangQi { get { return xiangQiUI; } }
        public PiecePoint PiecePoint { get { return piecePoint; } set { piecePoint = value; ResetPiecePoint(); } }
        public ChessPiece Piece { get { return piece; } }

        protected XiangQiUI xiangQiUI;
        protected PiecePoint piecePoint;
        protected ChessPiece piece;

        //用于鼠标事件的变量
        protected bool trackingMouseMove = false;
        protected PiecePoint piecePointOnMouseDown;
        protected Point mousePosition;
        protected int zIndexOnDragging = 999;
        protected int originalZIndex;

        public ChessPieceUI(XiangQiUI xiangQiUI, ChessPiece piece)
        {
            InitializeComponent();
            this.xiangQiUI = xiangQiUI;
            this.piece = piece;

            //set piece text
            PieceText.Text = piece.Name;

            //place piece at point
            PiecePoint = xiangQiUI.GetPiecePoint(this);  
        }

        public void ApplyVisual(PieceVisual visual)
        {
            if (visual == null) return;

            SetForeground(visual.ForeColor);
        }

        /// <summary>
        /// 移动到新的棋子位置
        /// </summary>
        /// <param name="newPiecePoint"></param>
        public void MoveTo(PiecePoint newPiecePoint)
        {
            if (newPiecePoint == null) { ResetPiecePoint(); return; }
            if (newPiecePoint.Position == piecePoint.Position) { ResetPiecePoint(); return; }

            Step s = piece.MoveTo(newPiecePoint.Position);//不想再这里使用piece,有更好的办法吗？

            if (s != null)
            {
                if (s.KilledPiece != null)
                    xiangQiUI.KickoutPiece(s.KilledPiece);
                PiecePoint = newPiecePoint;
            }
            else
                ResetPiecePoint();
        }

        #region private method

        private void ResetPiecePoint()
        {
            this.SetValue(Canvas.LeftProperty, PiecePoint.GetLeft(pieceWidth));        
            this.SetValue(Canvas.TopProperty, PiecePoint.GetTop(pieceHeight));
        }

        private void SetForeground(Color c)
        {
            PieceEllipse.Stroke = new SolidColorBrush(c);
            PieceText.Foreground = new SolidColorBrush(c);
        }

        private void UserControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement element = sender as FrameworkElement;
            piecePointOnMouseDown = PiecePoint;
            mousePosition = e.GetPosition(null);
            trackingMouseMove = true;
            if (null != element)
            {
                element.CaptureMouse();
                element.Cursor = Cursors.Hand;
                originalZIndex = (int)element.GetValue(Canvas.ZIndexProperty);
                element.SetValue(Canvas.ZIndexProperty, zIndexOnDragging);
            }
        }

        private void UserControl_MouseMove(object sender, MouseEventArgs e)
        {
            FrameworkElement element = sender as FrameworkElement;
            if (trackingMouseMove)
            {
                double deltaV = e.GetPosition(null).Y - mousePosition.Y;
                double deltaH = e.GetPosition(null).X - mousePosition.X;
                double newTop = deltaV + (double)element.GetValue(Canvas.TopProperty);
                double newLeft = deltaH + (double)element.GetValue(Canvas.LeftProperty);

                element.SetValue(Canvas.TopProperty, newTop);
                element.SetValue(Canvas.LeftProperty, newLeft);

                mousePosition = e.GetPosition(null);
            }

        }

        private void UserControl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement element = sender as FrameworkElement;

            double centerX = (double)element.GetValue(Canvas.LeftProperty) + pieceWidth / 2;
            double centerY = (double)element.GetValue(Canvas.TopProperty) + pieceHeight / 2;
            PiecePoint newPiecePoint = xiangQiUI.GetNearestPiecePointFrom(centerX, centerY);

            MoveTo(newPiecePoint);            

            trackingMouseMove = false;
            element.ReleaseMouseCapture();
            element.Cursor = null;
            element.SetValue(Canvas.ZIndexProperty, originalZIndex);
            mousePosition.X = mousePosition.Y = 0;

        }

        #endregion
    }


    public class PieceVisual
    {
        public Color ForeColor { get; set; }

        public PieceVisual(Color foreColor)
        {
            this.ForeColor = foreColor;
        }
    }
}
