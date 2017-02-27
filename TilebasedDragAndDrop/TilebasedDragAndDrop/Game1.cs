
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading;
using System.Windows.Forms;


//finla result
namespace TilebasedDragAndDrop
{
    public class Game1 : Game
    {

        public static Game1 game =  new Game1();

        #region Variables

        enum GameState
        {
            StartMenu,
            Loading,
            Playing,
            Paused
        }
        private Texture2D startButton;
        private Texture2D exitButton;
        private Texture2D loadingScreen;
        private Vector2 startButtonPosition;
        private Vector2 exitButtonPosition;
        private GameState gameState;
        private Thread backgroundThread;
        private bool isLoading = false;
        MouseState mouseState;
        MouseState previousMouseState;
        GraphicsDeviceManager graphics;
        Texture2D BackGround;
        SpriteBatch spriteBatch;
        String TurnString;
        Texture2D _whiteSquare;                 //the white 64x64 pixels bitmap to draw with
        MouseState currentMouseState, lastMouseState, CurrentMouseState2, LastMouseState2;
        Vector2 _currentMousePosition;          //the current position of the mouse
        Vector2 _draggableSquarePosition;       //the draggable tile
        Vector2 _boardPosition;                 //where to position the board
        Vector2 NewPosition;                     // Temp Vector For Changed Position
        bool CheckifFound, gamemode;
        Vector2 _FirstBoxPosition, _LastBoxPosition; // position of start of the baord and the end of the baord
        Vector2 _mouseDownPosition;             //where the mouse was clicked down
        Rectangle _draggableSquareBorder;       //the boundaries of the draggable tile
        bool PieceClicked,Check = false, Check_mate = false;
        string king_color;
        Tuple<int, int, piece> Index;
        int IndX, IndY;                           // New Index for changed Position
        public  piece[,] PieceType;    // the type the box holds Pawn - Knight - Blank 
        public readonly int[,] _board = new int[8, 8];        //stores whether there is something in a square
        public bool turn = false;                          //The turn of which player to play white or black.
        public Texture2D[,] _pieces;  //Store the image of each White piece
        Vector2[,] _PiecesPosition;   //Store the position of each Black piece
        const int _tileSize = 70;               //how wide/tall the tiles are
        bool _isDragging = true;               //remembers whether the mouse is currently dragging something
        SpriteFont _defaultFont;                //font to write info with
        Tuple<bool, int, int, string> king_status;
        public int p1, p2, WDeath_postion=-1, BDeath_postion=-1;
        int x, y;
        Tuple<bool, int, int> undocollion;
        int undoX, undoY, doUNDO;
        int collisionX = 0, collisionY = 0;
        int WDeathpiece = 0, BDeathpiece = 0, WBDeathpiece = 0, BBDeathpiece = 0, WKDeathpiece = 0, BKDeathpiece = 0;
        //stores the previous and current states of the mouse
        //makes it possible to know if a button was just clicked
        //or whether it was up/down previously as well.
        MouseState _oldMouse, _currentMouse;

        #endregion

        #region Constructor and LoadContent

        protected override void Initialize()
        {
            Check = false;
            CheckifFound = false;
            PieceClicked = false;
            gamemode = false;
            TurnString = "";
            Index = Tuple.Create(0, 0, new piece());
            PieceType = new piece[8, 8];
            turn = false;
            _pieces = new Texture2D[8, 8];
            _PiecesPosition = new Vector2[8, 8];
            NewPosition.Y = 75;
            NewPosition.X = 100;
            _FirstBoxPosition.Y = 75;      // Position of first Box in board (100,75

            for (int i = 0; i < 2; i++)   // Set black pieces position
            {
                _FirstBoxPosition.X = 100;
                for (int j = 0; j < 8; j++)
                {
                    _PiecesPosition[i, j].X = _FirstBoxPosition.X;
                    _PiecesPosition[i, j].Y = _FirstBoxPosition.Y;
                    _FirstBoxPosition.X += _tileSize;
                }
                _FirstBoxPosition.Y += _tileSize;
            }
            _LastBoxPosition.Y = 75 + (6 * _tileSize);      // Position of first Box in the row before the last (100,7*80)
            for (int i = 6; i < 8; i++)
            {
                _LastBoxPosition.X = 100;
                for (int j = 0; j < 8; j++)
                {
                    _PiecesPosition[i, j].X = _LastBoxPosition.X;
                    _PiecesPosition[i, j].Y = _LastBoxPosition.Y;
                    
                    _LastBoxPosition.X += _tileSize;
                }
                _LastBoxPosition.Y += _tileSize;
            }


            for (int i = 2; i < 6; i++)
            {
                for (int y = 0; y < 8; y++)
                {
                    _PiecesPosition[i, y].X = -2;
                    _PiecesPosition[i, y].Y = -2;
                }
            }

                Connect_pieces();
                //enable the mousepointer
                IsMouseVisible = true;
                //set the position of the buttons
                startButtonPosition = new Vector2((GraphicsDevice.Viewport.Width / 2) - 50, 210);
                exitButtonPosition = new Vector2((GraphicsDevice.Viewport.Width / 2) - 50, 250);
                //set the gamestate to start menu
                gameState = GameState.StartMenu;
                //get the mouse state
                mouseState = Mouse.GetState();
                previousMouseState = mouseState;
            turn = false;
            base.Initialize();
        }


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            //set the screen size
            graphics.PreferredBackBufferHeight = 700; //700
            graphics.PreferredBackBufferWidth = 750;  //750

            //positions the top left corner of the board - change this to move the board
            _boardPosition = new Vector2(100, 75);

            //positions the square to drag
            _draggableSquarePosition = new Vector2(800, 100);
          


            //show the mouse
            IsMouseVisible = true;
        }


        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            //load the buttonimages into the content pipeline
            startButton = Content.Load<Texture2D>(@"Start");
            exitButton = Content.Load<Texture2D>(@"Exit");
            BackGround = Content.Load<Texture2D>(@"Back");
            //load the loading screen
            loadingScreen = Content.Load<Texture2D>(@"loading");
            //load the textures
            _whiteSquare = Content.Load<Texture2D>("white_64x64");
            string WhitePieces;                             // Draw pieces on board
            int WName = 1;
            for (int i = 0; i < 2; i++)           // Draw White Pieces
            {
                for (int j = 0; j < 8; j++)
                {
                    WhitePieces = "" + WName;
                    _pieces[i, j] = Content.Load<Texture2D>(WhitePieces);
                    WName++;
                }
            }


            string BlackPieces;                             // Draw pieces on board
            int BName = 17;
            for (int i = 6; i < 8; i++)           // Draw Black Pieces
            {
                for (int j = 0; j < 8; j++)
                {
                    BlackPieces = "" + BName;
                    _pieces[i, j] = Content.Load<Texture2D>(BlackPieces);
                    BName++;
                }
            }
            //load the font
            _defaultFont = Content.Load<SpriteFont>("DefaultFont");

        }

        #endregion

        #region Update and related methods

        protected override void Update(GameTime gameTime)
        {
            if (gamemode != true)
            { 
                //load the game when needed
                if (gameState == GameState.Loading && !isLoading) //isLoading bool is to prevent the LoadGame method from being called 60 times a seconds
                {
                    backgroundThread = new Thread(LoadGame);
                    isLoading = true;
                    gamemode = true;
                    //start backgroundthread
                    backgroundThread.Start();
                }
                //wait for mouseclick
                mouseState = Mouse.GetState();
                if (previousMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed &&
                    mouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released)
                {
                    MouseClicked(mouseState.X, mouseState.Y);
                }

                previousMouseState = mouseState;

                if (gameState == GameState.Playing && isLoading)
                {
                    LoadGame();
                    isLoading = false;
                }
            }
            else
            {
                //get the current state of the mouse (position, buttons, etc.)
                _currentMouse = Mouse.GetState();

                // Allows the game to exit on an ESC press
                if (Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape)) { this.Exit(); }

                else if (Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Back) && doUNDO == 1)
                {
                    _PiecesPosition[undoX, undoY].X = x;
                    _PiecesPosition[undoX, undoY].Y = y;
                    doUNDO = 0;
                    if (PieceType[undoX, undoY].name == "Pawn" && PieceType[undoX, undoY].color == "White" && y == 495)
                        PieceType[undoX, undoY].move_behaviour.First_move = true;
                    if (PieceType[undoX, undoY].name == "Pawn" && PieceType[undoX, undoY].color == "Black" && y == 145)
                        PieceType[undoX, undoY].move_behaviour.First_move = true;
                    if (PieceType[undoX, undoY].color == "Black")
                        turn = true;
                    if (PieceType[undoX, undoY].color == "White")
                        turn = false;
                    if (undocollion.Item1 == true && PieceType[undocollion.Item2, undocollion.Item3].color == "Black")
                    {

                        _PiecesPosition[undocollion.Item2, undocollion.Item3].X = collisionX;
                        _PiecesPosition[undocollion.Item2, undocollion.Item3].Y = collisionY;
                        BDeath_postion -= 50;
                        if (undocollion.Item2 == 1 || undocollion.Item3 == 0 && undocollion.Item2 == 0 || undocollion.Item3 == 3 && undocollion.Item2 == 0 || undocollion.Item3 == 7 && undocollion.Item2 == 0)
                        {
                            BDeathpiece--;
                        }
                        else if (undocollion.Item3 == 2 && undocollion.Item2 == 0 || undocollion.Item3 == 5 && undocollion.Item2 == 0)
                        {
                            BBDeathpiece--;
                        }
                        else if (undocollion.Item3 == 1 && undocollion.Item2 == 0 || undocollion.Item3 == 6 && undocollion.Item2 == 0)
                        {
                            BKDeathpiece--;
                        }

                    }
                    if (undocollion.Item1 == true && PieceType[undocollion.Item2, undocollion.Item3].color == "White")
                    {

                        _PiecesPosition[undocollion.Item2, undocollion.Item3].X = collisionX;
                        _PiecesPosition[undocollion.Item2, undocollion.Item3].Y = collisionY;
                        WDeath_postion -=50;
                        if (undocollion.Item2 == 6 || undocollion.Item3 == 0 && undocollion.Item2 == 7 || undocollion.Item3 == 3 && undocollion.Item2 == 7 || undocollion.Item3 == 7 && undocollion.Item2 == 7)
                        {
                            WDeathpiece--;
                        }
                        else if (undocollion.Item3 == 2 && undocollion.Item2 == 7 || undocollion.Item3 == 5 && undocollion.Item2 == 7)
                        {
                            WBDeathpiece--;
                        }
                        else if (undocollion.Item3 == 1 && undocollion.Item2 == 7 || undocollion.Item3 == 6 && undocollion.Item2 == 7)
                        {
                            WKDeathpiece--;
                        }
                    }
                }
                //remember the mouseposition for use in this Update and subsequent Draw
                _currentMousePosition = new Vector2(_currentMouse.X, _currentMouse.Y);

                /* CheckForLeftButtonDown();

                 CheckForLeftButtonRelease();

                 CheckForRightButtonReleaseOverBoard();*/

                if (turn == false)
                    TurnString = "White Player Turn";
                else
                    TurnString = "Black Player Turn";

                // the start of check and check mate function

                // check if king is under threat




                if (PieceClicked == false)
                    Index = ChangePositionAfterDrag();
                else if (PieceClicked == true)
                {
                    Glow(Index.Item1, Index.Item2);
                    ChangePositionAfterRelease(Index.Item1, Index.Item2, Index.Item3);
                    king_status = King_threat();
                    king_color = king_status.Item4;
                    Console.WriteLine("king under threat is "+ king_status.Item4);
                    if (king_status.Item1 && king_status.Item4 != "BOTH")
                    {
                        // check if it's a check mate or not  
                        // check if piece caused threat can be eaten 

                        if (threat_piece_eaten(king_status.Item2, king_status.Item3, king_status.Item4))
                        {
                            Check = true;
                            Console.WriteLine("piece can be eaten");
                        }
                        else if (block_threat(king_status.Item2, king_status.Item3, king_status.Item4))
                        {
                            Check = true;
                            Console.WriteLine("check can be blocked");
                        }
                        else if (king_can_move(king_status.Item4, king_status.Item2, king_status.Item3))
                        {
                            Check = true;
                            Console.WriteLine("king can move");
                        }
                       
                        else
                        {
                            Console.WriteLine("check mate");
                            MessageBox.Show(king_status.Item4 + " LOST");
                            gamemode = false;
                            gameState = GameState.StartMenu;
                            Check = false;
                            this.Initialize();
                            
                        }
                    }
                }


                //store the current state of the mouse as the old
                _oldMouse = _currentMouse;


            }
            //call the Game class' Update 
            base.Update(gameTime);

            
        }

        void Glow(int x, int y)
        {
            bool [,]High = new bool [8,8];
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (PieceType[x, y].move_behaviour.legal_move((int)_PiecesPosition[x, y].X,(int)_PiecesPosition[x, y].Y,(i*80)+100, (j*80)+75) == true)
                    {
                        //Console.WriteLine("here Glow");
                        High[i, j] = true;
                    }
                    else
                        High[i, j] = false;
                }
            }
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (High[i, j] == true)
                        Console.WriteLine(i + " " + j);
                }
            }
        }

        void MouseClicked(int x, int y)
        {
            //creates a rectangle of 10x10 around the place where the mouse was clicked
            Rectangle mouseClickRect = new Rectangle(x, y, 10, 10);

            //check the startmenu
            if (gameState == GameState.StartMenu)
            {
                Rectangle startButtonRect = new Rectangle((int)startButtonPosition.X, (int)startButtonPosition.Y, 100, 20);
                Rectangle exitButtonRect = new Rectangle((int)exitButtonPosition.X, (int)exitButtonPosition.Y, 100, 20);

                if (mouseClickRect.Intersects(startButtonRect)) //player clicked start button
                {
                    gameState = GameState.Loading;
                    isLoading = false;
                }
                else if (mouseClickRect.Intersects(exitButtonRect)) //player clicked exit button
                {
                    Exit();
                }
            }

            //check the pausebutton
            if (gameState == GameState.Playing)
            {
                Rectangle pauseButtonRect = new Rectangle(0, 0, 70, 70);

                if (mouseClickRect.Intersects(pauseButtonRect))
                {
                    gameState = GameState.Paused;
                }
            }
        }
        void LoadGame()
        {
            //start playing
            gameState = GameState.Playing;
            isLoading = false;
        }
        public Tuple<int, int, piece> ChangePositionAfterDrag()  // Pick the selected Piece
        {
            lastMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();
            if (lastMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released && currentMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)         // if the loops hits the _mousedownposition and which kind was in the box 
            {

                for (int k = 0; k < 2; k++)
                {
                    for (int l = 0; l < 8; l++)
                    {
                        if (lastMouseState.X > _PiecesPosition[k, l].X && lastMouseState.X < _PiecesPosition[k, l].X + _tileSize && lastMouseState.Y < _PiecesPosition[k, l].Y + _tileSize && lastMouseState.Y > _PiecesPosition[k, l].Y && ((PieceType[k, l].color == "White" && turn == false) || (PieceType[k, l].color == "Black" && turn == true)) ) 
                        {
                            CheckifFound = true;
                            IndX = k;
                            IndY = l;
                            NewPosition.X = _PiecesPosition[k, l].X;
                            NewPosition.Y = _PiecesPosition[k, l].Y;
                            PieceClicked = true;
                            break;
                        }
                    }
                    if (CheckifFound == true)
                        break;
                }
                for (int k = 6; k < 8; k++)
                {
                    for (int l = 0; l < 8; l++)
                    {
                        if (lastMouseState.X > _PiecesPosition[k, l].X && lastMouseState.X < _PiecesPosition[k, l].X + _tileSize && lastMouseState.Y < _PiecesPosition[k, l].Y + _tileSize && lastMouseState.Y > _PiecesPosition[k, l].Y && ((PieceType[k, l].color == "White" && turn == false) || (PieceType[k, l].color == "Black" && turn == true))) 
                        {
                            CheckifFound = true;
                            IndX = k;
                            IndY = l;
                            NewPosition.X = _PiecesPosition[k, l].X;
                            NewPosition.Y = _PiecesPosition[k, l].Y;
                            PieceClicked = true;
                            break;
                        }
                    }
                    if (CheckifFound == true)
                        break;
                }


            }
            return Tuple.Create(IndX, IndY, PieceType[IndX, IndY]);

        }

        public void ChangePositionAfterRelease(int IndexX, int IndexY, piece p)            // Change the position of the seleceted Piece
        {
            Tuple<bool, int, int, string> before_move = King_threat();
            Console.WriteLine("before move ");
            Console.WriteLine(before_move.Item1);
            Console.WriteLine(before_move.Item4);
            doUNDO = 1;
            //int collisionX=0,collisionY=0;


            bool CheckifFound2 = false;
            LastMouseState2 = CurrentMouseState2;
            CurrentMouseState2 = Mouse.GetState();
            if (LastMouseState2.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released && CurrentMouseState2.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)     // -Trying- to get the the coordinates of the selected box and change positions
            {
                 CheckifFound2 = false;
                for (int i = 100; i < 100 + (_tileSize * 8); i += _tileSize)
                {
                    for (int j = 75; j < 75 + (_tileSize * 8); j += _tileSize)
                    {
                        if (LastMouseState2.X > i && LastMouseState2.X < i + _tileSize && LastMouseState2.Y < j + _tileSize && LastMouseState2.Y > j)
                        {
                            NewPosition.X = i;
                            NewPosition.Y = j;
                            PieceClicked = false;
                            CheckifFound2 = true;
                            CheckifFound = false;
                            break;
                        }
                    }
                    if (CheckifFound2 == true)
                        break;
                }
            }

            Tuple<bool, int, int> collision = Piece_on_tile((int)NewPosition.X, (int)NewPosition.Y);
            

            if (p.color == "White" && turn == false && CheckifFound2 == true && p.move_behaviour.legal_move((int)_PiecesPosition[IndexX, IndexY].X, (int)_PiecesPosition[IndexX, IndexY].Y, (int)NewPosition.X, (int)NewPosition.Y))
            {
                //stroe original posotion of piece
                x =(int) _PiecesPosition[IndexX, IndexY].X;
                y = (int)_PiecesPosition[IndexX, IndexY].Y;
                undoX = IndexX;
                undoY = IndexY;
                undocollion = collision;
                if ( collision.Item1 == false)
                {

                    PieceType[IndexX, IndexY].move_behaviour.First_move = false;
                    if (IndexX == 6 && _PiecesPosition[IndexX, IndexY].Y == 145 && PieceType[IndexX, IndexY].name == "Pawn")
                    {
                        Console.WriteLine("promotion in white pieces");
                        promotion promotionForm = new promotion(this); 
                        promotionForm.Show();
                        p1 = IndexX;
                        p2 = IndexY;
                    }
                    _PiecesPosition[IndexX, IndexY].X = NewPosition.X;
                    _PiecesPosition[IndexX, IndexY].Y = NewPosition.Y;
                    if (PieceType[IndexX, IndexY].move_behaviour.Castling == true /*&& hna al tby2 first move check */)
                    {
                        if (_PiecesPosition[IndexX, IndexY].X == 520 && _PiecesPosition[IndexX, IndexY].Y == 565)
                        {
                            _PiecesPosition[7, 7].X = 450;
                            _PiecesPosition[7, 7].Y = 565;
                        }
                        else if (_PiecesPosition[IndexX, IndexY].X == 240 && _PiecesPosition[IndexX, IndexY].Y == 565)
                        {
                            _PiecesPosition[7, 0].X = 310;
                            _PiecesPosition[7, 0].Y = 565;
                        }
                        PieceType[IndexX, IndexY].move_behaviour.Castling = false;
                    }
                    
                    turn = true;
                }
                else if (collision.Item1 == true && PieceType[collision.Item2, collision.Item3].color == "Black")
                {
                    collisionX = (int)_PiecesPosition[collision.Item2, collision.Item3].X;
                    collisionY = (int)_PiecesPosition[collision.Item2, collision.Item3].Y;
                    if (IndexX == 6 && _PiecesPosition[IndexX, IndexY].Y == 145 && PieceType[IndexX,IndexY].name=="Pawn")
                    {

                        promotion promotionForm = new promotion(this); 
                        Console.WriteLine("promotion in white pieces");
                        p1 = IndexX;
                        p2 = IndexY;
                        promotionForm.Show();
                    }
  
                    _PiecesPosition[collision.Item2, collision.Item3].X = 690;
                    _PiecesPosition[collision.Item2, collision.Item3].Y = BDeath_postion;
                    _PiecesPosition[IndexX, IndexY].X = NewPosition.X;
                    _PiecesPosition[IndexX, IndexY].Y = NewPosition.Y;
                    PieceType[IndexX, IndexY].move_behaviour.First_move = false;
                    turn = true;
                    BDeath_postion += 50;
                    if (collision.Item2 == 1 || collision.Item3 == 0 && collision.Item2 == 0 || collision.Item3 == 3 && collision.Item2 == 0 || collision.Item3 == 7 && collision.Item2 == 0)
                    {
                        BDeathpiece++;
                        Console.WriteLine(BDeathpiece);
                        Console.WriteLine(collision.Item2);
                        Console.WriteLine(collision.Item3);
                    }
                    else if (collision.Item3 == 2 && collision.Item2 == 0 || collision.Item3 == 5 && collision.Item2 == 0)
                    {
                        BBDeathpiece++;
                        Console.WriteLine(BBDeathpiece);
                        Console.WriteLine(collision.Item2);
                        Console.WriteLine(collision.Item3);
                    }
                    else if (collision.Item3 == 1 && collision.Item2 == 0 || collision.Item3 == 6 && collision.Item2 == 0)
                    {
                        BKDeathpiece++;
                        Console.WriteLine(BKDeathpiece);
                        Console.WriteLine(collision.Item2);
                        Console.WriteLine(collision.Item3);
                    }
                }
               //after move 
                Tuple<bool, int, int, string> temp = King_threat();
                Console.WriteLine("after move ");
                Console.WriteLine(temp.Item1);
                Console.WriteLine(temp.Item4);
              
                if ((temp.Item1 && temp.Item4 =="BOTH") || (before_move.Item1 == false && temp.Item1 && temp.Item4 == "White") || (before_move.Item1 && before_move.Item4 == "White" && temp.Item1 && temp.Item4 == "White"))
                {
                    _PiecesPosition[IndexX, IndexY].X = x;
                    _PiecesPosition[IndexX, IndexY].Y = y;
                    turn = false;
                    if (collision.Item1 == true && PieceType[collision.Item2, collision.Item3].color == "Black")
                    { 
                          
                      _PiecesPosition[collision.Item2, collision.Item3].X = collisionX;
                      _PiecesPosition[collision.Item2, collision.Item3].Y = collisionY;
                      BDeath_postion -= 50;
                    }
                }
                //whan draw game 
                if (WDeathpiece >= 11 && BDeathpiece >= 11 && WKDeathpiece + BKDeathpiece + BBDeathpiece + WBDeathpiece >= 7 || WDeathpiece >= 11 && BDeathpiece >= 11 && WKDeathpiece + BKDeathpiece == 4 && BBDeathpiece + WBDeathpiece >= 2 && _PiecesPosition[0, 2].X != 690 && _PiecesPosition[7, 5].X != -1 || WDeathpiece >= 11 && BDeathpiece >= 11 && WKDeathpiece + BKDeathpiece == 4 && BBDeathpiece + WBDeathpiece >= 2 && _PiecesPosition[0, 5].X != 690 && _PiecesPosition[7, 2].X != -1)
                {
                    Console.WriteLine("draw game :(");
                    MessageBox.Show("GAME IS DRAW ** WHY SO SERIOUS ** ");
                    gamemode = false;
                    gameState = GameState.StartMenu;
                    Check = false;
                    this.Initialize();
                    WDeath_postion = -1;
                    BDeath_postion = -1;
                    WDeathpiece = BDeathpiece = BBDeathpiece = WBDeathpiece = BKDeathpiece = WKDeathpiece = 0;
                }
            
            }
            else if (p.color == "Black" && turn == true && CheckifFound2 == true && p.move_behaviour.legal_move((int)_PiecesPosition[IndexX, IndexY].X, (int)_PiecesPosition[IndexX, IndexY].Y, (int)NewPosition.X, (int)NewPosition.Y))
            {
                x = (int)_PiecesPosition[IndexX, IndexY].X;
                y = (int)_PiecesPosition[IndexX, IndexY].Y;
                undoX = IndexX;
                undoY = IndexY;
                undocollion = collision;
                if (collision.Item1 == false)
                {          
                    PieceType[IndexX, IndexY].move_behaviour.First_move = false;
                    if (IndexX == 1 && _PiecesPosition[IndexX, IndexY].Y == 495 && PieceType[IndexX, IndexY].name == "Pawn")
                    {
                        Console.WriteLine("promotion in white pieces");
                        promotion promotionForm = new promotion(this);
                        promotionForm.Show();
                        p1 = IndexX;
                        p2 = IndexY;
                    }
                    _PiecesPosition[IndexX, IndexY].X = NewPosition.X;
                    _PiecesPosition[IndexX, IndexY].Y = NewPosition.Y;
                    if (PieceType[IndexX, IndexY].move_behaviour.Castling == true)
                    {
                        if (_PiecesPosition[IndexX, IndexY].X == 520 && _PiecesPosition[IndexX, IndexY].Y == 75)
                        {
                            _PiecesPosition[0, 7].X = 450;
                            _PiecesPosition[0, 7].Y = 75;
                        }
                        else if (_PiecesPosition[IndexX, IndexY].X == 240 && _PiecesPosition[IndexX, IndexY].Y == 75)
                        {
                            _PiecesPosition[0, 0].X = 310;
                            _PiecesPosition[0, 0].Y = 75;
                        }
                        PieceType[IndexX, IndexY].move_behaviour.Castling = false;
                    }
                    turn = false;
                }
                else if(collision.Item1 == true && PieceType[collision.Item2, collision.Item3].color == "White")
                {
                    collisionX = (int)_PiecesPosition[collision.Item2, collision.Item3].X;
                    collisionY = (int)_PiecesPosition[collision.Item2, collision.Item3].Y;

                    PieceType[IndexX, IndexY].move_behaviour.First_move = false;
                    if (IndexX == 1 && _PiecesPosition[IndexX, IndexY].Y == 495 && PieceType[IndexX, IndexY].name == "Pawn")
                    {
                        Console.WriteLine("promotion in white pieces");
                        promotion promotionForm = new promotion(this);
                        promotionForm.Show();
                        p1 = IndexX;
                        p2 = IndexY;
                    }

                    _PiecesPosition[collision.Item2, collision.Item3].X = -1;
                    _PiecesPosition[collision.Item2, collision.Item3].Y = WDeath_postion;
                    _PiecesPosition[IndexX, IndexY].X = NewPosition.X;
                    _PiecesPosition[IndexX, IndexY].Y = NewPosition.Y;
                    
                    turn = false;
                    WDeath_postion += 50;
                    if (collision.Item2 == 6 || collision.Item3 == 0 && collision.Item2 == 7 || collision.Item3 == 3 && collision.Item2 == 7 || collision.Item3 == 7 && collision.Item2 == 7)
                    {
                        WDeathpiece++;
                        Console.WriteLine(WDeathpiece);
                        Console.WriteLine(collision.Item2);
                        Console.WriteLine(collision.Item3);
                    }
                    else if (collision.Item3 == 2 && collision.Item2 == 7 || collision.Item3 == 5 && collision.Item2 == 7)
                    {
                        WBDeathpiece++;
                        Console.WriteLine(WBDeathpiece);
                        Console.WriteLine(collision.Item2);
                        Console.WriteLine(collision.Item3);
                    }
                    else if (collision.Item3 == 1 && collision.Item2 == 7 || collision.Item3 == 6 && collision.Item2 == 7)
                    {
                        WKDeathpiece++;
                        Console.WriteLine(WKDeathpiece);
                        Console.WriteLine(collision.Item2);
                        Console.WriteLine(collision.Item3);
                    }
                }
                Tuple<bool, int, int, string> temp = King_threat();
                // king wasn't checked until move made 
                Console.WriteLine("after move ");
                Console.WriteLine(temp.Item1);
                Console.WriteLine(temp.Item4);
                if ((temp.Item1 && temp.Item4 == "BOTH")||(before_move.Item1 == false && temp.Item1 && temp.Item4 == "Black")  || (before_move.Item1 && before_move.Item4 == "Black" && temp.Item1 && temp.Item4 == "Black"))
                {
                    _PiecesPosition[IndexX, IndexY].X = x;
                    _PiecesPosition[IndexX, IndexY].Y = y;
                    turn = true;
                    if (collision.Item1 == true && PieceType[collision.Item2, collision.Item3].color == "White")
                    { 
                             _PiecesPosition[collision.Item2, collision.Item3].X = collisionX;
                             _PiecesPosition[collision.Item2, collision.Item3].Y = collisionY;
                             WDeath_postion -= 50;
                    }
                }
                //whan game goes draw
                if (WDeathpiece >= 11 && BDeathpiece >= 11 && WKDeathpiece + BKDeathpiece + BBDeathpiece + WBDeathpiece >= 7 || WDeathpiece >= 11 && BDeathpiece >= 11 && WKDeathpiece + BKDeathpiece == 4 && BBDeathpiece + WBDeathpiece >= 2 && _PiecesPosition[0, 2].X != -1 && _PiecesPosition[7, 5].X != -1 || WDeathpiece >= 11 && BDeathpiece >= 11 && WKDeathpiece + BKDeathpiece == 4 && BBDeathpiece + WBDeathpiece >= 2 && _PiecesPosition[0, 5].X != -1 && _PiecesPosition[7, 2].X != -1)
                {
                    Console.WriteLine("draw game :(");
                    MessageBox.Show("GAME IS DRAW ** WHY SO SERIOUS ** ");
                    gamemode = false;
                    gameState = GameState.StartMenu;
                    Check = false;
                    this.Initialize();
                    WDeath_postion = -1;
                    BDeath_postion = -1;
                    WDeathpiece = BDeathpiece = BBDeathpiece = WBDeathpiece = BKDeathpiece = WKDeathpiece = 0;
                }
                


            }
        }

        public void Connect_pieces()
        { 
                //connect black pieces one by one
              PieceType[0,0] = new Rook("Black","Rook",this);
              PieceType[0, 1] = new Knight("Black", "Knight",this);
              PieceType[0, 2] = new Bishop("Black", "Bishop",this);
              PieceType[0, 3] = new Queen("Black", "Queen", this);
              PieceType[0, 4] = new King("Black", "King",this);
              PieceType[0, 3] = new Queen("Black", "Queen",this);
              PieceType[0, 5] = new Bishop("Black", "Bishop",this);
              PieceType[0, 6] = new Knight("Black", "Knight",this);
              PieceType[0, 7] = new Rook("Black", "Rook",this);
              for(int i = 0 ; i< 8 ;i++)
                  PieceType[1,i] = new Pawn("Black","Pawn",this);

            //connect white pieces one by one

              for (int i = 0; i < 8; i++)
                  PieceType[6, i] = new Pawn("White", "Pawn",this);

              PieceType[7, 0] = new Rook("White", "Rook",this);
              PieceType[7, 1] = new Knight("White", "Knight",this);
              PieceType[7, 2] = new Bishop("White", "Bishop",this);
              PieceType[7, 3] = new Queen("White", "Queen", this);
              PieceType[7, 4] = new King("White", "King",this);       
              PieceType[7, 5] = new Bishop("White", "Bishop",this);
              PieceType[7, 6] = new Knight("White", "Knight",this);
              PieceType[7, 7] = new Rook("White", "Rook",this);
        }

        public Tuple<bool, int, int> Piece_on_tile(int positionx, int positiony)
        {
            int posX = positionx;
            int posY = positiony;
            bool exist = false;
            int indx = -1, indy = -1;

            for (int i = 0; i < 2; i++)
            {
                for (int y = 0; y < 8; y++)
                {

                    if (_PiecesPosition[i, y].X == posX && _PiecesPosition[i, y].Y == posY)
                        {
                            exist = true;
                            indx = i;
                            indy = y;
                            return Tuple.Create(exist, indx, indy);
                        }
                }

            }

            for (int i = 6; i < 8; i++)
            {
                for (int y = 0; y < 8; y++)
                {

                    if (_PiecesPosition[i, y].X == posX && _PiecesPosition[i, y].Y == posY)
                    {
                        exist = true;
                        indx = i;
                        indy = y;
                        return Tuple.Create(exist, indx, indy);
                    }

                }

            }

            return Tuple.Create(exist, indx, indy);
        }

        Tuple<bool, int, int,string> King_threat()
        {
           
            int White_kingx = (int)_PiecesPosition[7, 4].X;
            int White_kingy = (int)_PiecesPosition[7, 4].Y;
            int Black_kingx = (int)_PiecesPosition[0, 4].X;
            int Black_kingy = (int)_PiecesPosition[0, 4].Y;
            bool threat_white = false,threat_black = false;


            int x=-1, h=-1;
            // check if black pieces threat white king
            for (int i = 0; i < 2; i++)
            {
                for (int y = 0; y < 8; y++)
                {

                    if (PieceType[i, y].move_behaviour.legal_move((int)_PiecesPosition[i, y].X, (int)_PiecesPosition[i, y].Y, White_kingx, White_kingy))
                    {
                        threat_white = true;
                        x = i;
                        h = y;
                        Console.WriteLine("white king under threat");
                        break;                   
                      
                    }       
                }
                if (threat_white)
                    break;
            }

            //check if white pieces threat Black king
            for (int i = 6; i < 8; i++)
            {
                for (int y = 0; y < 8; y++)
                {

                    if (PieceType[i, y].move_behaviour.legal_move((int)_PiecesPosition[i, y].X, (int)_PiecesPosition[i, y].Y, Black_kingx, Black_kingy))
                    {
                        threat_black = true;
                        x = i;
                        h = y;
                        Console.WriteLine("black king under threat");
                        break;
                    }
                }
                if (threat_black)
                    break;
            }
           
            if (threat_black && !threat_white)
                return Tuple.Create(threat_black, x, h,"Black");
            else if(!threat_black && threat_white)
                return Tuple.Create(threat_white, x, h,"White");
            else if(threat_white && threat_black)
                return Tuple.Create(threat_black, x, h, "BOTH");
            else    
            return Tuple.Create(false, x, h,"none"); 
        }

       





        // takes the index of threat piece and the color of the king
        bool threat_piece_eaten( int indexX, int indexY,string color_of_king)
        {  // the position of the piece caused threat
            int positionX = (int)_PiecesPosition[indexX, indexY].X;
            int positionY = (int)_PiecesPosition[indexX, indexY].Y;

            if (color_of_king == "White")
            {
                //search in white pieces

                for (int i = 6; i < 8; i++)
                {

                    for (int y = 0; y < 8; y++)
                    {
                        if ((i!=7 || y!=4) && PieceType[i, y].move_behaviour.legal_move((int)_PiecesPosition[i, y].X, (int)_PiecesPosition[i, y].Y, positionX, positionY))
                        {

                            return true;

                        }
                    }
                }

                return false;
            }
            else if (color_of_king == "Black")
            {


                for (int i = 0; i < 2; i++)
                {

                    for (int y = 0; y < 8; y++)
                    {
                        if ((i != 0 || y != 4) && PieceType[i, y].move_behaviour.legal_move((int)_PiecesPosition[i, y].X, (int)_PiecesPosition[i, y].Y, positionX, positionY))
                        {
                            return true;
                        }
                    }
                }
                return false;


            }
            else return false;
   
        }


      public  bool check_threat(int empty_tilex, int empty_tiley,string king_color)
        {
            int tilex = empty_tilex;
            int tiley = empty_tiley;

           // if king color white check in black pieces and vice versa
            if (king_color == "White")
            {


                for (int i = 0; i < 2; i++)
                {
                    for (int y = 0; y < 8; y++)
                    {

                        if (PieceType[i, y].move_behaviour.legal_move((int)_PiecesPosition[i, y].X, (int)_PiecesPosition[i, y].Y, tilex, tiley))
                        {
                            return false;

                        }
                    }
                }
                Console.WriteLine("no piece is a threat in this tile");
                return true;

            }
            else if (king_color == "Black")
            {
                for (int i = 6; i < 8; i++)
                {
                    for (int y = 0; y < 8; y++)
                    {

                        if (PieceType[i, y].move_behaviour.legal_move((int)_PiecesPosition[i, y].X, (int)_PiecesPosition[i, y].Y, tilex, tiley))
                        {
                            return false;
                        }
                    }
                }

                return true;
            }
            else
            {
                return false;
            }

            
             
        
        
        }


        bool king_can_move(string s,int indexX,int indexY)
        {
            Console.WriteLine("entered fucntion");

            int White_kingx = (int)_PiecesPosition[7, 4].X;
            int White_kingy = (int)_PiecesPosition[7, 4].Y;
            int Black_kingx = (int)_PiecesPosition[0, 4].X;
            int Black_kingy = (int)_PiecesPosition[0, 4].Y;
            // posiotion of threat piece
            int positionX = (int)_PiecesPosition[indexX, indexY].X;
            int positionY = (int)_PiecesPosition[indexX, indexY].Y;
            Console.WriteLine(PieceType[indexX, indexY].name);
            if (s == "White")
            {
                _PiecesPosition[7, 4].X = -1;
                _PiecesPosition[7, 4].Y = -1;
                _PiecesPosition[indexX, indexY].X = -1;
                _PiecesPosition[indexX, indexY].Y = -1;
                //check right
                if (!PieceType[indexX, indexY].move_behaviour.legal_move(positionX, positionY, White_kingx + 70, White_kingy) && PieceType[7, 4].move_behaviour.legal_move(White_kingx, White_kingy, White_kingx + 70, White_kingy) && check_threat(White_kingx + 70, White_kingy, "White"))
                {
                    Console.WriteLine("r");
                    _PiecesPosition[7, 4].X = White_kingx;
                    _PiecesPosition[7, 4].Y = White_kingy;
                    _PiecesPosition[indexX, indexY].X = positionX;
                    _PiecesPosition[indexX, indexY].Y = positionY;
                      
                    return true;
                }

                //check left
                if (!PieceType[indexX, indexY].move_behaviour.legal_move(positionX, positionY, White_kingx - 70, White_kingy) && PieceType[7, 4].move_behaviour.legal_move(White_kingx, White_kingy, White_kingx - 70, White_kingy)&&check_threat(White_kingx - 70, White_kingy, "White"))
                {
                    Console.WriteLine("l");
                    _PiecesPosition[7, 4].X = White_kingx;
                    _PiecesPosition[7, 4].Y = White_kingy;
                    _PiecesPosition[indexX, indexY].X = positionX;
                    _PiecesPosition[indexX, indexY].Y = positionY;
                        return true;
                }

                //check up

                if (!PieceType[indexX, indexY].move_behaviour.legal_move(positionX, positionY, White_kingx, White_kingy - 70) && PieceType[7, 4].move_behaviour.legal_move(White_kingx, White_kingy, White_kingx, White_kingy - 70) && check_threat(White_kingx , White_kingy-70, "White"))
                {
                    Console.WriteLine("u");
                    _PiecesPosition[7, 4].X = White_kingx;
                    _PiecesPosition[7, 4].Y = White_kingy;
                    _PiecesPosition[indexX, indexY].X = positionX;
                    _PiecesPosition[indexX, indexY].Y = positionY;
                    return true;
                }
                //check down

                if (!PieceType[indexX, indexY].move_behaviour.legal_move(positionX, positionY, White_kingx, White_kingy + 70) && PieceType[7, 4].move_behaviour.legal_move(White_kingx, White_kingy, White_kingx, White_kingy + 70) && check_threat(White_kingx , White_kingy+70, "White"))
                {
                    Console.WriteLine("d");
                    _PiecesPosition[7, 4].X = White_kingx;
                    _PiecesPosition[7, 4].Y = White_kingy;
                    _PiecesPosition[indexX, indexY].X = positionX;
                    _PiecesPosition[indexX, indexY].Y = positionY;
                    return true;
                }
                //check upleft

                if (!PieceType[indexX, indexY].move_behaviour.legal_move(positionX, positionY, White_kingx - 70, White_kingy - 70) && PieceType[7, 4].move_behaviour.legal_move(White_kingx, White_kingy, White_kingx - 70, White_kingy - 70) && check_threat(White_kingx - 70, White_kingy-70, "White"))
                {
                    Console.WriteLine("ul");
                    _PiecesPosition[7, 4].X = White_kingx;
                    _PiecesPosition[7, 4].Y = White_kingy;
                    _PiecesPosition[indexX, indexY].X = positionX;
                    _PiecesPosition[indexX, indexY].Y = positionY;
                        return true;
                }

                // upright

                if (!PieceType[indexX, indexY].move_behaviour.legal_move(positionX, positionY, White_kingx + 70, White_kingy - 70) && PieceType[7, 4].move_behaviour.legal_move(White_kingx, White_kingy, White_kingx + 70, White_kingy - 70) && check_threat(White_kingx + 70, White_kingy-70, "White"))
                {
                    Console.WriteLine("ur");
                    _PiecesPosition[7, 4].X = White_kingx;
                    _PiecesPosition[7, 4].Y = White_kingy;
                    _PiecesPosition[indexX, indexY].X = positionX;
                    _PiecesPosition[indexX, indexY].Y = positionY;
                        return true;
                }
                //check downleft

                if (!PieceType[indexX, indexY].move_behaviour.legal_move(positionX, positionY, White_kingx - 70, White_kingy + 70) && PieceType[7, 4].move_behaviour.legal_move(White_kingx, White_kingy, White_kingx - 70, White_kingy + 70) && check_threat(White_kingx - 70, White_kingy+70, "White"))
                {
                    Console.WriteLine("dl");
                    _PiecesPosition[7, 4].X = White_kingx;
                    _PiecesPosition[7, 4].Y = White_kingy;
                    _PiecesPosition[indexX, indexY].X = positionX;
                    _PiecesPosition[indexX, indexY].Y = positionY;
                        return true;
                }
                //check downright

                if (!PieceType[indexX, indexY].move_behaviour.legal_move(positionX, positionY, White_kingx + 70, White_kingy + 70) && PieceType[7, 4].move_behaviour.legal_move(White_kingx, White_kingy, White_kingx + 70, White_kingy + 70) && check_threat(White_kingx + 70, White_kingy+70, "White"))
                {
                    Console.WriteLine("dr");
                    _PiecesPosition[7, 4].X = White_kingx;
                    _PiecesPosition[7, 4].Y = White_kingy;
                    _PiecesPosition[indexX, indexY].X = positionX;
                    _PiecesPosition[indexX, indexY].Y = positionY;
                    return true;
                }
                return false;
            }

            else if (s == "Black")
            {
                _PiecesPosition[0, 4].X = -1;
                _PiecesPosition[0, 4].Y = -1;
                _PiecesPosition[indexX, indexY].X = -1;
                _PiecesPosition[indexX, indexY].Y = -1;
                //check right
                if (!PieceType[indexX, indexY].move_behaviour.legal_move(positionX, positionY, Black_kingx + 70, Black_kingy) && PieceType[0, 4].move_behaviour.legal_move(Black_kingx, Black_kingy, Black_kingx + 70, Black_kingy) && check_threat(Black_kingx + 70, Black_kingy, "Black"))
                {
                    Console.WriteLine("r");
                    _PiecesPosition[0, 4].X = Black_kingx;
                    _PiecesPosition[0, 4].Y = Black_kingy;
                    _PiecesPosition[indexX, indexY].X = positionX;
                    _PiecesPosition[indexX, indexY].Y = positionY;
                    return true;
                }

                //check left
                if (!PieceType[indexX, indexY].move_behaviour.legal_move(positionX, positionY, Black_kingx - 70, Black_kingy) && PieceType[0, 4].move_behaviour.legal_move(Black_kingx, Black_kingy, Black_kingx - 70, Black_kingy) && check_threat(Black_kingx - 70, Black_kingy, "Black"))
                {
                    Console.WriteLine("l");
                    _PiecesPosition[0, 4].X = Black_kingx;
                    _PiecesPosition[0, 4].Y = Black_kingy;
                    _PiecesPosition[indexX, indexY].X = positionX;
                    _PiecesPosition[indexX, indexY].Y = positionY;
                    return true;
                }
                //check up

                if (!PieceType[indexX, indexY].move_behaviour.legal_move(positionX, positionY, Black_kingx, Black_kingy - 70) && PieceType[0, 4].move_behaviour.legal_move(Black_kingx, Black_kingy, Black_kingx, Black_kingy - 70) && check_threat(Black_kingx, Black_kingy-70, "Black"))
                {
                    Console.WriteLine("u");
                    _PiecesPosition[0, 4].X = Black_kingx;
                    _PiecesPosition[0, 4].Y = Black_kingy;
                    _PiecesPosition[indexX, indexY].X = positionX;
                    _PiecesPosition[indexX, indexY].Y = positionY;
                    return true;
                }
                //check down

                if (!PieceType[indexX, indexY].move_behaviour.legal_move(positionX, positionY, Black_kingx, Black_kingy + 70) && PieceType[0, 4].move_behaviour.legal_move(Black_kingx, Black_kingy, Black_kingx, Black_kingy + 70) && check_threat(Black_kingx , Black_kingy+70, "Black"))
                {
                    Console.WriteLine("d");
                    _PiecesPosition[0, 4].X = Black_kingx;
                    _PiecesPosition[0, 4].Y = Black_kingy;
                    _PiecesPosition[indexX, indexY].X = positionX;
                    _PiecesPosition[indexX, indexY].Y = positionY;
                    return true;
                }

                //check upleft

                if (!PieceType[indexX, indexY].move_behaviour.legal_move(positionX, positionY, Black_kingx - 70, Black_kingy - 70) && PieceType[0, 4].move_behaviour.legal_move(Black_kingx, Black_kingy, Black_kingx - 70, Black_kingy - 70) && check_threat(Black_kingx - 70, Black_kingy-70, "Black"))
                {
                    Console.WriteLine("ul");
                    _PiecesPosition[0, 4].X = Black_kingx;
                    _PiecesPosition[0, 4].Y = Black_kingy;
                    _PiecesPosition[indexX, indexY].X = positionX;
                    _PiecesPosition[indexX, indexY].Y = positionY;
                    return true;
                }

                // upright

                if (!PieceType[indexX, indexY].move_behaviour.legal_move(positionX, positionY, Black_kingx + 70, Black_kingy - 70) && PieceType[0, 4].move_behaviour.legal_move(Black_kingx, Black_kingy, Black_kingx + 70, Black_kingy - 70) && check_threat(Black_kingx + 70, Black_kingy-70, "Black"))
                {
                    Console.WriteLine("ur");
                    _PiecesPosition[0, 4].X = Black_kingx;
                    _PiecesPosition[0, 4].Y = Black_kingy;
                    _PiecesPosition[indexX, indexY].X = positionX;
                    _PiecesPosition[indexX, indexY].Y = positionY;
                    return true;
                }
                //check downleft

                if (!PieceType[indexX, indexY].move_behaviour.legal_move(positionX, positionY, Black_kingx - 70, Black_kingy + 70) && PieceType[0, 4].move_behaviour.legal_move(Black_kingx, Black_kingy, Black_kingx - 70, Black_kingy + 70) && check_threat(Black_kingx - 70, Black_kingy+70, "Black"))
                {
                    Console.WriteLine("dl");
                    _PiecesPosition[0, 4].X = Black_kingx;
                    _PiecesPosition[0, 4].Y = Black_kingy;
                    _PiecesPosition[indexX, indexY].X = positionX;
                    _PiecesPosition[indexX, indexY].Y = positionY;
                    return true;
                }

                //check downright

                if (!PieceType[indexX, indexY].move_behaviour.legal_move(positionX, positionY, Black_kingx + 70, Black_kingy + 70) && PieceType[7, 4].move_behaviour.legal_move(Black_kingx, Black_kingy, Black_kingx + 70, Black_kingy + 70) && check_threat(Black_kingx + 70, Black_kingy+70, "Black"))
                {
                    Console.WriteLine("dr");
                    _PiecesPosition[0, 4].X = Black_kingx;
                    _PiecesPosition[0, 4].Y = Black_kingy;
                    _PiecesPosition[indexX, indexY].X = positionX;
                    _PiecesPosition[indexX, indexY].Y = positionY;
                    return true;
                }
                return false;      
            }
            else
                return false;
                    
        }

        bool block_threat(int indexX, int indexY, string color_of_king)
        {

            piece threat_piece = PieceType[indexX, indexY];
            // position of piece caused threat
            int positionx = (int)_PiecesPosition[indexX, indexY].X;
            int positiony = (int)_PiecesPosition[indexX, indexY].Y;
            // position of king under threat
            int king_positionx,king_positiony;
            if (color_of_king == "White")
            {
                king_positionx = (int)_PiecesPosition[7, 4].X;
                king_positiony = (int)_PiecesPosition[7, 4].Y;

            }
            else if (color_of_king == "Black")
            {
                king_positionx = (int)_PiecesPosition[0, 4].X;
                king_positiony = (int)_PiecesPosition[0, 4].Y;
            }
            else
                return false;

            if (threat_piece.name != "Pawn" || threat_piece.name != "Knight")
            {
                List<Vector2> tiles = new List<Vector2>();

                if (threat_piece.name == "Rook")
                {

                   //get route from threat piece to king  
                   // so we need to check all the directions that a rook can threat a king
                    tiles = get_Rook_route(positionx, positiony, king_positionx, king_positiony);
                }//end rook

                else if (threat_piece.name == "Bishop")
                {
                    Console.WriteLine("entered as a bishop");
                    //get route from threat piece to king  
                    // so we need to check all the directions that a bishop can threat a king
                    tiles = get_Bishop_route(positionx, positiony, king_positionx, king_positiony);


                }
                else if (threat_piece.name == "Queen")
                {
                    Console.WriteLine("entered as a queen");
                    tiles = get_Queen_route(positionx, positiony, king_positionx, king_positiony);
                }

                //check if a piece can block the way

                if (color_of_king == "White")
                {
                    Console.WriteLine("white loop");
                    //loop through white pieces
                    int posx,posy;

                    for (int i = 6; i < 8; i++)
                    {
                        for (int y = 0; y < 8 ; y++)
                        {
                            if (i != 7 || y != 4)
                            {
                                posx = (int)_PiecesPosition[i, y].X;
                                posy = (int)_PiecesPosition[i, y].Y;

                                for (int w = 0; w < tiles.Count; w++)
                                {


                                    if (PieceType[i, y].move_behaviour.legal_move(posx, posy, (int)tiles[w].X, (int)tiles[w].Y))
                                    {
                                        Console.WriteLine(PieceType[i,y].name);
                                        tiles.Clear();
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                    tiles.Clear();
                    return false;
                }
                else if (color_of_king == "Black")
                {
                    Console.WriteLine("Black loop");
                    //loop through black pieces
                    int posx, posy;

                    for (int i = 0; i < 2; i++)
                    {
                        for (int y = 0; y < 8; y++)
                        {
                            if (i != 0 || y != 4)
                            {
                                posx = (int)_PiecesPosition[i, y].X;
                                posy = (int)_PiecesPosition[i, y].Y;

                                for (int w = 0; w < tiles.Count; w++)
                                {
                                    if (PieceType[i, y].move_behaviour.legal_move(posx, posy, (int)tiles[w].X, (int)tiles[w].Y))
                                    {
                                        Console.WriteLine(PieceType[i, y].name);
                                        tiles.Clear();
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                    tiles.Clear();
                    return false;
                }
                return false;
            }
            else
                //can't block threat if it's a knight or a pawn 
                return false;
        }

        List<Vector2> get_Rook_route(int oldX, int oldY, int newX, int newY)
        {
            List<Vector2> res = new List<Vector2>();
            // check if horizontal(left or right)
            if (newY == oldY && newX != oldX)
            {
                // check if right
                if (newX > oldX)
                {
                    for (int i = newX - 70; i > oldX; i -= 70)
                    {
                        res.Add(new Vector2(i, newY));
                    }
                }
                //check if left
                else
                {
                    for (int i = newX + 70; i < oldX; i += 70)
                    {
                        res.Add(new Vector2(i, newY));
                        
                    }
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
                        res.Add(new Vector2(newX, i));

                    }
                }
                //check if down
                else
                {
                    for (int i = newY - 70; i > oldY; i -= 70)
                    {
                        res.Add(new Vector2(newX,i));
                    }       
                }
            }
            return res;      
        }

        List<Vector2> get_Bishop_route(int oldX, int oldY, int newX, int newY)
        {
            List<Vector2> res = new List<Vector2>();
            //check if the new move is diagonal 
            if (Math.Abs(newX - oldX) == Math.Abs(newY - oldY))
            {
                //check if it's up left move
                if (newX < oldX && newY < oldY)
                {

                    for (int x = newX + 70, y = newY + 70; x < oldX && y < oldY; x += 70, y += 70)
                    {
                        res.Add(new Vector2(x,y));
                    }    
                }
                //check if it's up right
                else if (newX > oldX && newY < oldY)
                {

                    for (int x = newX - 70, y = newY + 70; x > oldX && y < oldY; x -= 70, y += 70)
                    {
                        res.Add(new Vector2(x, y));
                    }
                }
                //check if it's down left
                else if (newX < oldX && newY > oldY)
                {

                    for (int x = newX + 70, y = newY - 70; x < oldX && y > oldY; x += 70, y -= 70)
                    {
                        res.Add(new Vector2(x, y));
                    }
                }
                //check if down right
                else if (newX > oldX && newY > oldY)
                {

                    for (int x = newX - 70, y = newY - 70; x > oldX && y > oldY; x -= 70, y -= 70)
                    {
                        res.Add(new Vector2(x, y));
                    }
                }    
            }
            return res;

        }

        List<Vector2> get_Queen_route(int oldX, int oldY, int newX, int newY)
        {
            List<Vector2> res = new List<Vector2>();

            // if the move is diagonal
            if (Math.Abs(newX - oldX) == Math.Abs(newY - oldY))
            {

                //check if it's up left move
                if (newX < oldX && newY < oldY)
                {

                    for (int x = newX + 70, y = newY + 70; x < oldX && y < oldY; x += 70, y += 70)
                    {
                        res.Add(new Vector2(x, y));
                    }
                }
                //check if it's up right
                else if (newX > oldX && newY < oldY)
                {

                    for (int x = newX - 70, y = newY + 70; x > oldX && y < oldY; x -= 70, y += 70)
                    {
                        res.Add(new Vector2(x, y));
                    }
                }
                //check if it's down left
                else if (newX < oldX && newY > oldY)
                {

                    for (int x = newX + 70, y = newY - 70; x < oldX && y > oldY; x += 70, y -= 70)
                    {
                        res.Add(new Vector2(x, y));
                    }
                }
                //check if down right
                else if (newX > oldX && newY > oldY)
                {

                    for (int x = newX - 70, y = newY - 70; x > oldX && y > oldY; x -= 70, y -= 70)
                    {
                        res.Add(new Vector2(x, y));
                    }
                }
            }
            // check if horizontal(left or right)
            else if (newY == oldY && newX != oldX)
            {
                // check if right
                if (newX > oldX)
                {
                    for (int i = newX - 70; i > oldX; i -= 70)
                    {
                        res.Add(new Vector2(i, newY));
                    }

                }
                //check if left
                else
                {
                    for (int i = newX + 70; i < oldX; i += 70)
                    {
                        res.Add(new Vector2(i, newY));

                    }

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
                        res.Add(new Vector2(newX, i));

                    }


                }
                //check if down
                else
                {
                    for (int i = newY - 70; i > oldY; i -= 70)
                    {
                        res.Add(new Vector2(newX, i));

                    }

                }


            }

            return res;       
        }



        private void CheckForLeftButtonDown()
        {
            if (_currentMouse.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            {
                //if this Update() is a new click - store the mouse-down position
                if (_oldMouse.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released)
                {
                    _mouseDownPosition = _currentMousePosition;
                }

                //if the mousedown was within the draggable tile 
                //and the mouse has been moved more than 10 pixels:
                //start dragging!
                if ((_mouseDownPosition - _currentMousePosition).Length() > 10 && _draggableSquareBorder.Contains((int)_mouseDownPosition.X, (int)_mouseDownPosition.Y))
                {
                    _isDragging = true;
                }
            }
        }

        private void CheckForLeftButtonRelease()
        {
            //if the user just released the mousebutton - set _isDragging to false, and check if we should add the tile to the board
            if (_oldMouse.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && _currentMouse.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released && _isDragging)
            {
                _isDragging = false;

                //if the mousebutton was released inside the board
                if (IsMouseInsideBoard())
                {
                    //find out which square the mouse is over
                    Vector2 tile = GetSquareFromCurrentMousePosition();
                    //and set that square to true (has a piece)
                    _board[(int)tile.X, (int)tile.Y] = 1;
                }
            }
        }
       
        private void CheckForRightButtonReleaseOverBoard()
        {  
            //find out if right button was just clicked over the board - and remove a tile from that square
            if (_oldMouse.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Released && _currentMouse.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && IsMouseInsideBoard())
            {
                Vector2 boardSquare = GetSquareFromCurrentMousePosition();
                _board[(int)boardSquare.X, (int)boardSquare.Y] = 0;
            }
        }

        #endregion

        #region Draw and related methods

        protected override void Draw(GameTime gameTime)
        {

            //add a green background
            GraphicsDevice.Clear(Color.DarkCyan);
            //start drawing
            spriteBatch.Begin();
            if (gamemode != true)
            {
                spriteBatch.Draw(BackGround, new Vector2(0, 0), Color.White);
                //draw the start menu
                if (gameState == GameState.StartMenu)
                {
                    spriteBatch.Draw(startButton, startButtonPosition, Color.White);
                    spriteBatch.Draw(exitButton, exitButtonPosition, Color.White);
                }
            }
            else
            {
                DrawBoard();            //draw the board
                DrawPieces();
                DrawText();             //draw helptext
            }
            //end drawing
            spriteBatch.End();
            base.Draw(gameTime);
        }


        //Draws the text on the screen
        private void DrawText()
        {
            spriteBatch.DrawString(_defaultFont, "Chess Game", new Vector2(100, 20), Color.White);
            if (turn == false)
                spriteBatch.DrawString(_defaultFont, TurnString, new Vector2(500, 20), Color.White);
            else
                spriteBatch.DrawString(_defaultFont, TurnString, new Vector2(500, 20), Color.Black);
            if (Check_mate)
                spriteBatch.DrawString(_defaultFont, "check mate " + king_color, new Vector2(250, 20), Color.White);
            else if (Check && king_color == "White")
                spriteBatch.DrawString(_defaultFont, king_color + " king checked", new Vector2(250, 20), Color.White);
            else if (Check && king_color == "Black")
                spriteBatch.DrawString(_defaultFont, king_color + " king checked", new Vector2(250, 20), Color.Black);
            
        }
        //Draw the Pieces
        public void DrawPieces()
        {
            for (int i = 0; i < 2; i++)            // Draw White Pieces
            {
                for (int j = 0; j < 8; j++)
                {
                    spriteBatch.Draw(_pieces[i, j], _PiecesPosition[i, j], Color.White);
                }
            }

            for (int i = 6; i < 8; i++)            // Draw Black Pieces
            {
                for (int j = 0; j < 8; j++)
                {
                    spriteBatch.Draw(_pieces[i, j], _PiecesPosition[i, j], Color.White);
                }
            }
        }
        // Draws the game board
        private void DrawBoard()
        {
            float opacity;                                      //how opaque/transparent to draw the square
            Color colorToUse = Color.White;                     //background color to use
            Rectangle squareToDrawPosition = new Rectangle();   //the square to draw (local variable to avoid creating a new variable per square)

            //for all columns
            for (int x = 0; x < _board.GetLength(0); x++)
            {
                //for all rows
                for (int y = 0; y < _board.GetLength(1); y++)
                {

                    //figure out where to draw the square
                    squareToDrawPosition = new Rectangle((int)(x * _tileSize + _boardPosition.X), (int)(y * _tileSize + _boardPosition.Y), _tileSize, _tileSize);

                    //the code below will make the board checkered using only a single, white square:

                    //if we add the x and y value of the tile
                    //and it is even, we make it one third opaque
                    if ((x + y) % 2 == 0)
                    {
                        opacity = .33f;
                    }
                    else
                    {
                        //otherwise it is one tenth opaque
                        opacity = .1f;
                    }

                    //make the square the mouse is over red
                    if (IsMouseInsideBoard() && IsMouseOnTile(x, y))
                    {
                        colorToUse = Color.Gold;
                        opacity = 1.5f;
                    }
                    else
                    {
                        colorToUse = Color.White;
                    }


                    if (PieceClicked)
                    {
                        int left = (int)_PiecesPosition[Index.Item1,Index.Item2].X;
                        int right = (int)_PiecesPosition[Index.Item1, Index.Item2].Y;

                        Rectangle colored = new Rectangle(left,right,_tileSize, _tileSize);
                       


                        if (squareToDrawPosition == colored)
                            //draw the white square at the given position, offset by the x- and y-offset, in the opacity desired
                            spriteBatch.Draw(_whiteSquare, squareToDrawPosition, Color.Crimson* 1.5f);
                        else
                            spriteBatch.Draw(_whiteSquare, squareToDrawPosition, colorToUse * opacity);
                    }
                    else
                    {
                        spriteBatch.Draw(_whiteSquare, squareToDrawPosition, colorToUse * opacity);
                    }
                }

            }
        }

        #endregion

        #region Mouse and board helpermethods

        // Checks to see whether a given coordinate is within the board
        private bool IsMouseOnTile(int x, int y)
        {
            //do an integerdivision (whole-number) of the coordinates relative to the board offset with the tilesize in mind
            return (int)(_currentMousePosition.X - _boardPosition.X) / _tileSize == x && (int)(_currentMousePosition.Y - _boardPosition.Y) / _tileSize == y;
        }

        //find out whether the mouse is inside the board
        bool IsMouseInsideBoard()
        {
            if (_currentMousePosition.X >= _boardPosition.X && _currentMousePosition.X <= _boardPosition.X + _board.GetLength(0) * _tileSize && _currentMousePosition.Y >= _boardPosition.Y && _currentMousePosition.Y <= _boardPosition.Y + _board.GetLength(1) * _tileSize)
            {
                return true;
            }
            else
            { return false; }
        }

        //get the column/row on the board for a given coordinate
        Vector2 GetSquareFromCurrentMousePosition()
        {
            //adjust for the boards offset (_boardPosition) and do an integerdivision
            return new Vector2((int)(_currentMousePosition.X - _boardPosition.X) / _tileSize, (int)(_currentMousePosition.Y - _boardPosition.Y) / _tileSize);
        }

        #endregion

    }
}
