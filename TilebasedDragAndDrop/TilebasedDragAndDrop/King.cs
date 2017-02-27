using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TilebasedDragAndDrop
{
    public class King : piece
    {
         public King(string c, string n,Game1 j) : base(c,n,j,new King_move())
        {
           
        }  
    }
}
