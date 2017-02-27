using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TilebasedDragAndDrop
{
    public partial class promotion : Form
    {
        public promotion()
        {
            InitializeComponent();

        }
        public int x;
        public Game1 game;
        public promotion(Game1 g)
        {
            InitializeComponent();
            game = g;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string WhitePieces;
            if (game.p1 == 6)
            {
                WhitePieces = "" + 28;
                game._pieces[game.p1, game.p2] = game.Content.Load<Texture2D>(WhitePieces);
                game.PieceType[game.p1, game.p2] = game.PieceType[7, 3];
            }
            else if (game.p1 == 1)
            {
                WhitePieces = "" + 4;
                game._pieces[game.p1, game.p2] = game.Content.Load<Texture2D>(WhitePieces);
                game.PieceType[game.p1, game.p2] = game.PieceType[0, 3];
            }
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string WhitePieces;
            if (game.p1 == 6)
            {
                WhitePieces=""+27;
                game._pieces[game.p1, game.p2] = game.Content.Load<Texture2D>(WhitePieces);
                game.PieceType[game.p1, game.p2] = game.PieceType[7, 2];
            }
            else if (game.p1 == 1)
            {
                WhitePieces = "" + 3;
                game._pieces[game.p1, game.p2] = game.Content.Load<Texture2D>(WhitePieces);
                game.PieceType[game.p1, game.p2] = game.PieceType[0, 2];
            }
            this.Close();
           
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string WhitePieces;
            if (game.p1 == 6)
            {
                WhitePieces = "" + 26;
                game._pieces[game.p1, game.p2] = game.Content.Load<Texture2D>(WhitePieces);
                game.PieceType[game.p1, game.p2] = game.PieceType[7, 1];
            }
            else if (game.p1 == 1)
            {
                WhitePieces = "" + 2;
                game._pieces[game.p1, game.p2] = game.Content.Load<Texture2D>(WhitePieces); 
                game.PieceType[game.p1, game.p2] = game.PieceType[0, 1];
            }
                this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string WhitePieces;
            if (game.p1 == 6)
            {
                WhitePieces = "" + 25;
                game._pieces[game.p1, game.p2] = game.Content.Load<Texture2D>(WhitePieces);
                game.PieceType[game.p1, game.p2] = game.PieceType[7, 0];
            }
            else if (game.p1 == 1)
            {
                WhitePieces = "" + 1;
                game._pieces[game.p1, game.p2] = game.Content.Load<Texture2D>(WhitePieces);
                game.PieceType[game.p1, game.p2] = game.PieceType[0, 0];
            }
            this.Close();
        }

        private void promotion_Load(object sender, EventArgs e)
        {

        }
    }
}
