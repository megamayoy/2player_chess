using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TilebasedDragAndDrop
{
    public class Bishop_move:Move_piece
    {
       override public bool legal_move(int oldX, int oldY, int newX, int newY)
        {
            Tuple<bool, int, int> res = new Tuple<bool,int,int>(false,-1,-1);
           //check if the new move is diagonal 
            if (Math.Abs(newX - oldX ) == Math.Abs(newY - oldY))
            {

                //check if it's up left move
                if (newX < oldX && newY < oldY)
                {

                    for (int x = newX + 70, y = newY + 70; x < oldX && y < oldY; x += 70, y += 70)
                    {
                        res = game.Piece_on_tile(x, y);

                        if (res.Item1 == true)
                            return false;
                    }

                    return true;
                }
                //check if it's up right
                else if (newX > oldX && newY < oldY)
                {

                    for (int x = newX - 70, y = newY + 70; x > oldX && y < oldY; x -= 70, y += 70)
                    {
                        res = game.Piece_on_tile(x, y);

                        if (res.Item1 == true)
                            return false;
                    }

                    return true;

                }
                    //check if it's down left
                else if (newX < oldX && newY > oldY)
                {

                    for (int x = newX + 70, y = newY - 70; x < oldX && y > oldY; x += 70, y -= 70)
                    {
                        res = game.Piece_on_tile(x, y);

                        if (res.Item1 == true)
                            return false;
                    }

                    return true;
                
                }
                    //check if down right
                else if(newX > oldX && newY > oldY)
                {
                
                    for (int x = newX - 70, y = newY - 70; x > oldX && y > oldY; x -= 70, y -= 70)
                    {
                        res = game.Piece_on_tile(x, y);

                        if (res.Item1 == true)
                            return false;
                    }

                    return true;
                
                }
                else
                    return false;
              
                
            }
            else
            {
                return false;
            }
        }

    }
}
