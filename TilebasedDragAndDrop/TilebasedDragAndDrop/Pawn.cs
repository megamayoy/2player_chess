using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TilebasedDragAndDrop
{
  public  class Pawn:piece
    {
      public Pawn(string c, string n, Game1 j)
          : base(c, n,j, new Pawn_move())
      { 

      }
    }
}
