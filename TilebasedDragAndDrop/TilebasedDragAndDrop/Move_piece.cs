using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TilebasedDragAndDrop
{
    
   public class Move_piece
    {
       public  Game1 game;
       public string piece_colour;

       public bool First_move = true;
       public bool Castling=false;
     
        //boolean function takes the coordinates of current location and specified location by user 
      virtual public bool legal_move(int oldX, int oldY, int newX, int newY)
        {
           
            if (oldX == newX || oldY == newY)
            {
                return false;
            }
            else
                return true;
        }
    }
}
