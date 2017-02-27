using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TilebasedDragAndDrop
{
    public class King_move : Move_piece
    {
      override public  bool legal_move(int oldX, int oldY, int newX, int newY)
        {
             //check if it's within range
             Tuple<bool, int, int> collision = game.Piece_on_tile(newX, newY);
            if (newX >= 100 && newX <= 590 && newY >= 75 && newY <= 565)
            {
                //check if it's legal
                if ((Math.Abs(oldX - newX) == 0 && Math.Abs(oldY - newY) == 70) || (Math.Abs(oldX - newX) == 70 && Math.Abs(oldY - newY) == 0) || (Math.Abs(oldX - newX) == 70 && Math.Abs(oldY - newY) == 70))
                {
                    if (!collision.Item1)
                    {
                        return true;
                    }
                    else
                    {
                        if (game.PieceType[collision.Item2, collision.Item3].color != piece_colour)
                        {
                            return true;
                        }
                        else
                            return false;
                    }



                }
                
                else if (oldX == 380 && oldY == 565 && newX == 520 && newY == 565 )
                { 
                    Console.WriteLine("the position you need OldX : " + oldX + " old Y : " + oldY + " New X : " + newX + " New Y : " + newY + " Fist Move : "+this.First_move);
                    if (game.Piece_on_tile(380 + 70, 565).Item2 == -1 && game.Piece_on_tile(380 + 70 + 70, 565).Item2 == -1 && this.First_move && game.PieceType[7, 7].move_behaviour.First_move && game.PieceType[7, 7].name == "Rook")
                    {
                        this.Castling = true;
                        return true;
                    }
                    else
                        return false;
                }
                else if (oldX == 380 && oldY == 565 && newX == 240 && newY == 565 && this.First_move)
                {
                    if (game.Piece_on_tile(310, 565).Item2 == -1 && game.Piece_on_tile(240, 565).Item2 == -1 && game.Piece_on_tile(170, 565).Item2 == -1 && game.PieceType[7, 0].move_behaviour.First_move && game.PieceType[7, 0].name == "Rook")
                    {
                        this.Castling = true;
                        return true;
                    }
                    else
                        return false;
                }
                else if (oldX == 380 && oldY == 75 && newX == 520 && newY == 75)
                {

                    if (game.Piece_on_tile(380 + 70, 75).Item2 == -1 && game.Piece_on_tile(380 + 70 + 70, 75).Item2 == -1 && First_move && game.PieceType[0, 7].move_behaviour.First_move && game.PieceType[0, 7].name == "Rook")
                    {
                        this.Castling = true;
                        return true;
                    }
                    else
                        return false;
                }
                else if (oldX == 380 && oldY == 75 && newX == 240 && newY == 75 && First_move)
                {
                    if (game.Piece_on_tile(380 - 70, 75).Item2 == -1 && game.Piece_on_tile(380 - 70 - 70, 75).Item2 == -1 && game.PieceType[0, 0].move_behaviour.First_move && game.PieceType[0, 0].name == "Rook")
                    {
                        this.Castling = true;
                        return true;
                    }
                    else
                        return false;
                }
                else
                    return false;
            }

           else
                return false;

        }

    }
}
