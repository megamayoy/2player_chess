using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TilebasedDragAndDrop
{
    public class Knight_move: Move_piece
    {
    
       override public bool legal_move(int oldX, int oldY, int newX, int newY)
        {
            
            //general move
            if ((Math.Abs(oldX - newX) == 70 && Math.Abs(oldY - newY) == 140) || (Math.Abs(oldX - newX) == 140 && Math.Abs(oldY - newY) == 70))
                return true;
            else
                return false;
        }
    }
}
