using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TilebasedDragAndDrop
{
    public class Rook:piece
    {
        public Rook(string c, string n, Game1 j)
            : base(c, n,j, new Rook_move())
        { 
        
        }

    }
}
