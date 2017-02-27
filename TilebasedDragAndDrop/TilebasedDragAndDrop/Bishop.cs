using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TilebasedDragAndDrop
{
   public class Bishop :piece
    {
       public Bishop(string c, string n, Game1 j)
           : base(c, n,j, new Bishop_move())
        {
           
        }  
    }
}
