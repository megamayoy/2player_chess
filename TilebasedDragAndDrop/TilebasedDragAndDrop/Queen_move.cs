using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TilebasedDragAndDrop
{
    public class Queen_move: Move_piece
    {

        // a queen moves like all piece except for the knight
       override public bool legal_move(int oldX, int oldY, int newX, int newY)
        {

           
           
          // we will use the legal move of the rook and bishop

           // using the bishop move 


            Tuple<bool, int, int> res;
            //check if the new move is diagonal 
            if (Math.Abs(newX - oldX) == Math.Abs(newY - oldY))
            {

                //check if it's up left love
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
                else if (newX > oldX && newY > oldY)
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
           //using the rook move 

           // check if horizontal(left or right)
            else if (newY == oldY && newX != oldX)
            {
                // check if right
                if (newX > oldX)
                {
                    for (int i = newX - 70; i > oldX; i -= 70)
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
                        res = game.Piece_on_tile(newX, i);
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
