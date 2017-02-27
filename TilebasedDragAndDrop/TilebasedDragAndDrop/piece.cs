using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TilebasedDragAndDrop
{
    public class piece
    {
        // color of players(player and opponent)
        public string color, name;
        // a variable for the AI part 
       public Move_piece move_behaviour;
        public piece()
        {
            color = "none";

        }

        public piece(string c, string n,Game1 h, Move_piece m)
        {
            color = c;
            name = n;
            move_behaviour = m;
            move_behaviour.piece_colour = c;
            move_behaviour.game = h;
        }  
    }
}
