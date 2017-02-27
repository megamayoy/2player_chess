using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TilebasedDragAndDrop
{
    public class Queen:piece
    {
        public Queen(string c, string n, Game1 j)
            : base(c, n,j, new Queen_move())
        { }

    }
}
