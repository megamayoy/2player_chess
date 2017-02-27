using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TilebasedDragAndDrop
{
    public class Pawn_move:Move_piece
    {
        
      override public  bool legal_move(int oldX, int oldY, int newX, int newY)
        {
            Tuple<bool, int, int> result = game.Piece_on_tile(newX, newY);
        
            if (piece_colour == "White")
            { 
               if(First_move)
               {
                                      //normal move (straight) 
                   if ((newX == oldX && (newY - oldY == -70 || newY - oldY == -140) && result.Item1 == false) || (((newX - oldX == 70 && newY - oldY == -70) || (newX - oldX == -70 && newY - oldY == -70))&& result.Item1 == true && game.PieceType[result.Item2,result.Item3].color == "Black"))
                   { 
                       
                     return true;
                   }
                   else
                       return false;
                   
               }
               else
               {
                   
                   return ((newX == oldX && newY - oldY == -70 && result.Item1 == false) || (((newX - oldX == 70 && newY - oldY == -70) || (newX - oldX == -70 && newY - oldY == -70)) && result.Item1 == true && game.PieceType[result.Item2,result.Item3].color == "Black"));
               }
            
            }
            else if (piece_colour == "Black")
            {

                if (First_move)
                {
                    
                    if ((newX == oldX && (newY - oldY == 70 || newY - oldY == 140) &&  result.Item1 == false) || (((newX - oldX == -70 && newY - oldY == 70) || (newX - oldX == 70 && newY - oldY == 70)) && result.Item1 == true && game.PieceType[result.Item2, result.Item3].color == "White"))
                    {
                        
                        return true;
                    }
                    else
                        return false;
                }
                else
                {
                    
                    return ((newX == oldX && newY - oldY == 70 && result.Item1 == false) ||  (((newX - oldX == -70 && newY - oldY == 70) || (newX - oldX == 70 && newY - oldY == 70)) && result.Item1 == true && game.PieceType[result.Item2, result.Item3].color == "White"));
                }
            
            }

            return false; 
               
        }
        

    }

}
