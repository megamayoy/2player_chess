using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TilebasedDragAndDrop
{
    class Knight: piece
    {
        public Knight(string c, string n, Game1 j)
            : base(c, n,j, new Knight_move())
        {
           
        }
    }
}
