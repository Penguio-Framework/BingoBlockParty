﻿using System;
using BingoBlockParty.Client.Utils;
using Engine;
using Engine.Interfaces;

namespace BingoBlockParty.Client.BallGame
{
    public class ViewManager
    {
        public ClientGameBoard GameBoard { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public int ViewWidth { get; set; }
        public int ViewHeight { get; set; }
        public Size PaddingBox { get; set; }

        public ViewManager(ClientGameBoard gameBoard)
        {
            GameBoard = gameBoard;
            ViewWidth = gameBoard.GameModel.Client().GameBoardLayout.Width;
            ViewHeight = gameBoard.GameModel.Client().GameBoardLayout.Height;
            PaddingBox = new Size(100, 100);
        }

        public void Set(int x, int y)
        {
            X = x;
            Y = y;
        }
        public void TranslateContext(ILayer layer)
        {
            layer.Translate(-X, -Y);
        }
        private int median(int[] xs)
        {
#if WEB
            xs.Sort((a, b) => a - b);
#else
            Array.Sort(xs);
#endif
            return xs[xs.Length / 2];
        }

        public void Center(int x, int y)
        {
            var proposedX = median(new[] { x - ViewWidth / 2, x - PaddingBox.Width, x + PaddingBox.Width - ViewWidth });
            var proposedY = median(new[] { y - ViewHeight / 2, y - PaddingBox.Height, y + PaddingBox.Height - ViewHeight });

            X = median(new[] { proposedX, 0, GameBoard.GameModel.BoardWidth - ViewWidth });
            Y = median(new[] { proposedY, 0, GameBoard.GameModel.BoardHeight - ViewHeight });
        }

    }
}