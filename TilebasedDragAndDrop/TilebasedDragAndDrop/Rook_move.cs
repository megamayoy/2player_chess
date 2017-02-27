using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TilebasedDragAndDrop
{
    public class Rook_move:Move_piece
    {
       override public bool legal_move(int oldX, int oldY, int newX, int newY)
        {
            Tuple<bool, int, int> res;
            // check if horizontal(left or right)
            if (newY == oldY && newX != oldX)
            {
                // check if right
                if (newX > oldX)
                {
                    for (int i = newX-70; i > oldX; i-=70)
                    { 
                         //check if there's a block in the route 
                        res = game.Piece_on_tile(i, newY);
                        if (res.Item1 == true)
                            return false;                  
                    }
                    return true;
                
                }
                //check if left
                else
                {
                    for (int i = newX + 70; i < oldX; i += 70)
                    {
                        //check if there's a block in the route 
                        res = game.Piece_on_tile(i, newY);
                        if (res.Item1 == true)
                            return false;
                    }
                    return true;
                
                }


            }
            // check if vertical(up or down)
            else if (newX == oldX && newY != oldY)
            {
                //check if up
                if (newY < oldY)
                {

                    for (int i = newY + 70; i < oldY; i += 70)
                    {
                        res = game.Piece_on_tile(newX,i);
                        //if block found
                        if (res.Item1 == true)
                            return false;     
                    
                    }
                    return true;
                     
                }
                //check if down
                else
                {
                    for (int i = newY - 70; i > oldY; i -= 70)
                    {
                        res = game.Piece_on_tile(newX, i);
                        //if block found
                        if (res.Item1 == true)
                            return false;

                    }
                    return true;
                
                }


            }
            else
                return false;
        }
    }
}
