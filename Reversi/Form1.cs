using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Reversi
{
    public partial class Form1 : Form
    {
        // used to storage indexes of field with the same color in line
        int up1, up2;
        int rightUp1, rightUp2;
        int right1, right2;
        int rightDown1, rightDown2;
        int down1, down2;
        int downLeft1, downLeft2;
        int left1, left2;
        int leftUp1, leftUp2;

        public int brownFieldWinWindow;
        public int greenFieldWinWindow;
        public int possibleMove;
        public int clickedIndex1;
        public int clickedIndex2;
        public bool pvPGame = false;
        public bool pvAIGame = false;
        public bool aivAIGame = false;
        public bool humanGreen = false;
        public bool humanBrown = false;
        public int whoseMove = 1;
        public int[,] board = new int[10,10];
        public int[] possibleMoveBoard = new int[30];
        public string brownPlayerName, greenPlayerName;
        WinWindow winWindow = new WinWindow();

        void PossibleMoveBoardClean()
        {
            for(int i = 0; i <= 29; i++)
            {
                    possibleMoveBoard[i] = 0;
            }
        }

        bool WinStatement()
        {
            int possibleMove = 0;
            int greenField = 0;
            int brownField = 0;

            for (int i = 1; i <= 8; i++)
            {
                for (int j = 1; j <= 8; j++)
                {
                    if (CorrectMove(i, j, board, 1))
                    {
                        possibleMove++;
                    }
                    if (CorrectMove(i, j, board, 2))
                    {
                        possibleMove++;
                    }
                    if (board[i, j] == 1)
                    {
                        brownField++;
                    }
                    if (board[i, j] == 2)
                    {
                        greenField++;
                    }
                }
            }

            greenFieldWinWindow = greenField;
            brownFieldWinWindow = brownField;

            greenFieldsCounter.Text = greenField.ToString();
            brownFieldsCounter.Text = brownField.ToString();

            if (possibleMove == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
            if (brownField == 0 || greenField == 0)
            {
                return true;
            }
            else
            {
                return false;
            }

            return false;
        }
        bool PossibleMove()
        {
            possibleMove = 0;

            if (whoseMove == 1)
            {
                for (int i = 1; i <= 8; i++)
                {
                    for (int j = 1; j <= 8; j++)
                    {
                        if (CorrectMove(i, j, board, 1))
                        {
                            possibleMoveBoard[possibleMove] = i*10+j;
                            possibleMove++;
                        }
                    }
                }
            }
            if (whoseMove == 2)
            {
                for (int i = 1; i <= 8; i++)
                {
                    for (int j = 1; j <= 8; j++)
                    {
                        if (CorrectMove(i, j, board, 2))
                        {
                            possibleMoveBoard[possibleMove] = i * 10 + j;
                            possibleMove++;
                        }
                    }
                }
            }
            if (possibleMove > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        void AIvAIGame()
        {
            if (whoseMove == 1)
            {
                if (PossibleMove())
                {
                    whoseMoveLabel.Text = "BROWN";

                    Random randomNumber = new Random();
                    int indexRandomMove = randomNumber.Next(0, possibleMove);
                    int possibleMoveNumber = possibleMoveBoard[indexRandomMove];

                    int index1 = possibleMoveNumber / 10;
                    int index2 = possibleMoveNumber % 10;

                    if (CorrectMove(index1, index2, board, whoseMove))
                    {
                        whoseMoveLabel.Text = "GREEN";
                        board[index1, index2] = 1;
                        SwapNumbers(index1, index2, board, whoseMove);
                        ChangeColours();
                        CleanGlobalVariable();
                        PossibleMoveBoardClean();
                        if (WinStatement())
                        {
                            winWindow.playerName.Text = "AI 1";
                            winWindow.g = greenFieldWinWindow;
                            winWindow.b = brownFieldWinWindow;
                            winWindow.Show(this);
                        }
                        else
                        {
                            whoseMove = 2;
                            AIvAIGame();
                        }
                    }
                    else
                    {
                        //wrongMove.Show(this);
                        whoseMove = 1;
                        CleanGlobalVariable();
                        AIvAIGame();
                    }
                }
                else
                {
                    // pass turn info!
                    whoseMove = 2;
                    AIvAIGame();
                }
            }
            else
            {
                if (whoseMove == 2)
                {
                    if (PossibleMove())
                    {
                        whoseMoveLabel.Text = "GREEN";

                        Random randomNumber = new Random();
                        int indexRandomMove = randomNumber.Next(0, possibleMove);
                        int possibleMoveNumber = possibleMoveBoard[indexRandomMove];

                        int index1 = possibleMoveNumber / 10;
                        int index2 = possibleMoveNumber % 10;

                        if (CorrectMove(index1, index2, board, whoseMove))
                        {
                            whoseMoveLabel.Text = "BROWN";
                            board[index1, index2] = 2;
                            SwapNumbers(index1, index2, board, whoseMove);
                            ChangeColours();
                            CleanGlobalVariable();
                            PossibleMoveBoardClean();
                            if (WinStatement())
                            {
                                winWindow.playerName.Text = "AI 2";
                                winWindow.g = greenFieldWinWindow;
                                winWindow.b = brownFieldWinWindow;
                                winWindow.Show(this);
                            }
                            else
                            {
                                whoseMove = 1;
                                AIvAIGame();
                            }
                        }
                        else
                        {
                            //wrongMove.Show(this);
                            whoseMove = 2;
                            CleanGlobalVariable();
                            AIvAIGame();
                        }
                    }
                    else
                    {
                        whoseMove = 1;
                        AIvAIGame();
                    }
                }
            }
        }
        void PvAIGame()
        {
            NameWindowAI nameWindowAI = new NameWindowAI();

            if (humanGreen)
            {
                if (whoseMove == 1)
                {
                    if (PossibleMove())
                    {
                        whoseMoveLabel.Text = "BROWN";
                        if (CorrectMove(clickedIndex1, clickedIndex2, board, whoseMove))
                        {
                            whoseMoveLabel.Text = "GREEN";
                            board[clickedIndex1, clickedIndex2] = 1;
                            SwapNumbers(clickedIndex1, clickedIndex2, board, whoseMove);
                            ChangeColours();
                            CleanGlobalVariable();
                            if (WinStatement())
                            {
                                winWindow.playerName.Text = brownPlayerName;
                                winWindow.Show(this);
                            }
                            else
                            {
                                whoseMove = 2;
                            }
                        }
                        else
                        {
                            //wrongMove.Show(this);
                            whoseMove = 1;
                            CleanGlobalVariable();
                        }
                    }
                    else
                    {
                        // pass turn info!
                        whoseMove = 2;
                    }
                }
                    if (whoseMove == 2)
                    {
                        if (PossibleMove())
                        {
                            whoseMoveLabel.Text = "GREEN";

                            Random randomNumber = new Random();
                            int indexRandomMove = randomNumber.Next(0, possibleMove);
                            int possibleMoveNumber = possibleMoveBoard[indexRandomMove];

                            int index1 = possibleMoveNumber/10;
                            int index2 = possibleMoveNumber%10;

                            if (CorrectMove(index1, index2, board, whoseMove))
                            {
                                whoseMoveLabel.Text = "BROWN";
                                board[index1, index2] = 2;
                                SwapNumbers(index1, index2, board, whoseMove);
                                ChangeColours();
                                CleanGlobalVariable();
                                PossibleMoveBoardClean();
                                if (WinStatement())
                                {
                                    winWindow.playerName.Text = greenPlayerName;
                                    winWindow.Show(this);
                                }
                                else
                                {
                                    whoseMove = 1;
                                }
                            }
                            else
                            {
                                //wrongMove.Show(this);
                                whoseMove = 2;
                                CleanGlobalVariable();
                                PvAIGame();
                            }
                        }
                        else
                        {
                            whoseMove = 1;
                        }
                    }                
            }
            else if(humanBrown)
            {
                if (whoseMove == 1)
                {
                    if (PossibleMove())
                    {
                        whoseMoveLabel.Text = "BROWN";

                        Random randomNumber = new Random();
                        int indexRandomMove = randomNumber.Next(0, possibleMove);
                        int possibleMoveNumber = possibleMoveBoard[indexRandomMove];

                        int index1 = possibleMoveNumber / 10;
                        int index2 = possibleMoveNumber % 10;

                        if (CorrectMove(index1, index2, board, whoseMove))
                        {
                            whoseMoveLabel.Text = "GREEN";
                            board[index1, index2] = 1;
                            SwapNumbers(index1, index2, board, whoseMove);
                            ChangeColours();
                            CleanGlobalVariable();
                            PossibleMoveBoardClean();
                            if (WinStatement())
                            {
                                winWindow.playerName.Text = brownPlayerName;
                                winWindow.Show(this);
                            }
                            else
                            {
                                whoseMove = 2;
                            }
                        }
                        else
                        {
                            //wrongMove.Show(this);
                            whoseMove = 1;
                            CleanGlobalVariable();
                            PvAIGame();
                        }
                    }
                    else
                    {
                        // pass turn info!
                        whoseMove = 2;
                    }
                }
                else
                {
                    if (whoseMove == 2)
                    {
                        if (PossibleMove())
                        {
                            whoseMoveLabel.Text = "GREEN";
                            if (CorrectMove(clickedIndex1, clickedIndex2, board, whoseMove))
                            {
                                whoseMoveLabel.Text = "BROWN";
                                board[clickedIndex1, clickedIndex2] = 2;
                                SwapNumbers(clickedIndex1, clickedIndex2, board, whoseMove);
                                ChangeColours();
                                CleanGlobalVariable();
                                if (WinStatement())
                                {
                                    winWindow.playerName.Text = greenPlayerName;
                                    winWindow.Show(this);
                                }
                                else
                                {
                                    whoseMove = 1;
                                    PvAIGame();
                                }
                            }
                            else
                            {
                                //wrongMove.Show(this);
                                whoseMove = 2;
                                CleanGlobalVariable();
                            }
                        }
                        else
                        {
                            whoseMove = 1;
                            PvAIGame();
                        }
                    }
                }
            }
        }
        void PvPGame()
        {
                if(whoseMove == 1)
                {
                    if (PossibleMove())
                    {
                        whoseMoveLabel.Text = "BROWN";
                        if (CorrectMove(clickedIndex1, clickedIndex2, board, whoseMove))
                        {
                            whoseMoveLabel.Text = "GREEN";
                            board[clickedIndex1, clickedIndex2] = 1;
                            SwapNumbers(clickedIndex1, clickedIndex2, board, whoseMove);
                            ChangeColours();
                            CleanGlobalVariable();
                            if (WinStatement())
                            {
                                winWindow.playerName.Text = brownPlayerName;
                                winWindow.Show(this);
                            }
                            else
                            {
                                whoseMove = 2;
                            }
                        }
                        else
                        {
                            //wrongMove.Show(this);
                            whoseMove = 1;
                            CleanGlobalVariable();
                        }
                    }
                    else
                    {
                        // pass turn info!
                        whoseMove = 2;
                    }
                }
                else
                {
                    if(whoseMove == 2)
                    {
                        if (PossibleMove())
                        {
                            whoseMoveLabel.Text = "GREEN";
                            if (CorrectMove(clickedIndex1, clickedIndex2, board, whoseMove))
                            {
                                whoseMoveLabel.Text = "BROWN";
                                board[clickedIndex1, clickedIndex2] = 2;
                                SwapNumbers(clickedIndex1, clickedIndex2, board, whoseMove);
                                ChangeColours();
                                CleanGlobalVariable();
                                if (WinStatement())
                                {
                                    winWindow.playerName.Text = greenPlayerName;
                                    winWindow.Show(this);
                                }
                                else
                                {
                                    whoseMove = 1;
                                }
                            }
                            else
                            {
                                //wrongMove.Show(this);
                                whoseMove = 2;
                                CleanGlobalVariable();
                            }
                        }
                        else
                        {
                            whoseMove = 1;
                        }
                    }
                }
        }

        // Use this at the end of player move
        void CleanGlobalVariable()
        {
            up1 = 0;
            up2 = 0;
            rightUp1 = 0;
            rightUp2 = 0;
            right1 = 0;
            right2 = 0;
            rightDown1 = 0;
            rightDown2 = 0;
            down1 = 0;
            down2 = 0;
            downLeft1 = 0;
            downLeft2 = 0;
            left1 = 0;
            left2 = 0;
            leftUp1 = 0;
            leftUp2 = 0;
        }

        void SwapNumbers(int index1, int index2, int[,] board, int whoseMove)
        {
            bool upLine = false;
            bool rightUpLine = false;
            bool rightLine = false;
            bool rightDownLine = false;
            bool downLine = false;
            bool downLeftLine = false;
            bool leftLine = false;
            bool leftUpLine = false;

            int tUp = index1;
            int tRightUp1 = index1, tRightUp2 = index2;
            int tRight = index2;
            int tRightDown1 = index1, tRightDown2 = index2;
            int tDown = index1;
            int tDownLeft1 = index1, tDownLeft2 = index2;
            int tLeft = index2;
            int tLeftUp1 = index1, tLeftUp2 = index2;

            while (upLine == false || rightUpLine == false || rightLine == false || rightDownLine == false ||
                downLine == false || downLeftLine == false || leftLine == false || leftUpLine == false)
            {
                if (tUp != up1 && up1 != 0 && tUp > 0)
                {
                    tUp--;
                    if (whoseMove == 1)
                    {
                        board[tUp, index2] = 1;
                    }
                    else
                    {
                        board[tUp, index2] = 2;
                    }
                }
                else
                {
                    upLine = true;
                }
                if (tRightUp1 != rightUp1 && tRightUp2 != rightUp2 && rightUp1 != 0 && rightUp2 != 0 && tRightUp1 > 0 && tRightUp2 < 9)
                {
                    tRightUp1--;
                    tRightUp2++;
                    if (whoseMove == 1)
                    {
                        board[tRightUp1, tRightUp2] = 1;
                    }
                    else
                    {
                        board[tRightUp1, tRightUp2] = 2;
                    }
                }
                else
                {
                    rightUpLine = true;
                }
                if (tRight != right2 && right2 != 0 && tRight < 9)
                {
                    tRight++;
                    if (whoseMove == 1)
                    {
                        board[index1, tRight] = 1;
                    }
                    else
                    {
                        board[index1, tRight] = 2;
                    }
                }
                else
                {
                    rightLine = true;
                }
                if (tRightDown1 != rightDown1 && tRightDown2 != rightDown2 && rightDown1 != 0 && rightDown2 != 0 && tRightDown1 < 9 && tRightDown2 < 9)
                {
                    tRightDown1++;
                    tRightDown2++;
                    if (whoseMove == 1)
                    {
                        board[tRightDown1, tRightDown2] = 1;
                    }
                    else
                    {
                        board[tRightDown1, tRightDown2] = 2;
                    }
                }
                else
                {
                    rightDownLine = true;
                }
                if (tDown != down1 && down1 != 0 && tDown < 9)
                {
                    tDown++;
                    if (whoseMove == 1)
                    {
                        board[tDown, index2] = 1;
                    }
                    else
                    {
                        board[tDown, index2] = 2;
                    }
                }
                else
                {
                    downLine = true;
                }
                if (tDownLeft1 != downLeft1 && tDownLeft2 != downLeft2 && downLeft1 != 0 && downLeft2 != 0 && tDownLeft1 < 9 && tDownLeft2 > 0)
                {
                    tDownLeft1++;
                    tDownLeft2--;
                    if (whoseMove == 1)
                    {
                        board[tDownLeft1, tDownLeft2] = 1;
                    }
                    else
                    {
                        board[tDownLeft1, tDownLeft2] = 2;
                    }
                }
                else
                {
                    downLeftLine = true;
                }
                if (tLeft != left2 && left2 != 0 && tLeft > 0)
                {
                    tLeft--;
                    if (whoseMove == 1)
                    {
                        board[index1, tLeft] = 1;
                    }
                    else
                    {
                        board[index1, tLeft] = 2;
                    }
                }
                else
                {
                    leftLine = true;
                }
                if (tLeftUp1 != leftUp1 && tLeftUp2 != leftUp2 && leftUp1 != 0 && leftUp2 != 0 && tLeftUp1 > 0 && tLeftUp2 > 0)
                {
                    tLeftUp1--;
                    tLeftUp2--;
                    if (whoseMove == 1)
                    {
                        board[tLeftUp1, tLeftUp2] = 1;
                    }
                    else
                    {
                        board[tLeftUp1, tLeftUp2] = 2;
                    }
                }
                else
                {
                    leftUpLine = true;
                }
            }
        }
        // return false if there isnt any field to change color
        // setting global variables to 0 if cannot be used
        bool FieldToChange(int index1, int index2, int[,] board)
        {
            bool upLine = false;
            bool rightUpLine = false;
            bool rightLine = false;
            bool rightDownLine = false;
            bool downLine = false;
            bool downLeftLine = false;
            bool leftLine = false;
            bool leftUpLine = false;

            int totalFieldsToChange = 0;
            int tUp = index1;
            int tRightUp1 = index1, tRightUp2 = index2;
            int tRight = index2;
            int tRightDown1 = index1, tRightDown2 = index2;
            int tDown = index1;
            int tDownLeft1 = index1, tDownLeft2 = index2;
            int tLeft = index2;
            int tLeftUp1 = index1, tLeftUp2 = index2;

            while (upLine == false || rightUpLine == false || rightLine == false || rightDownLine == false ||
                downLine == false || downLeftLine == false || leftLine == false || leftUpLine == false)
            {
                if (board[up1, up2] > 0 && tUp != up1 && tUp > 0)
                {
                    tUp--;
                    if (board[tUp, index2] == 0)
                    {
                        up1 = 0;
                        up2 = 0;
                        upLine = true;
                    }
                    if (board[tUp, index2] > 0 && board[tUp, index2] != whoseMove)
                    {
                        totalFieldsToChange++;
                    }
                }
                else
                {
                    upLine = true;
                }
                if (board[rightUp1, rightUp2] > 0 && tRightUp1 != rightUp1 && tRightUp2 != rightUp2 && tRightUp1 > 0 && tRightUp2 < 9)
                {
                    tRightUp1--;
                    tRightUp2++;
                    if (board[tRightUp1, tRightUp2] == 0)
                    {
                        rightUp1 = 0;
                        rightUp2 = 0;
                        rightUpLine = true;
                    }
                    if (board[tRightUp1, tRightUp2] > 0 && board[tRightUp1, tRightUp2] != whoseMove)
                    {
                        totalFieldsToChange++;
                    }
                }
                else
                {
                    rightUpLine = true;
                }
                if (board[right1, right2] > 0 && tRight != right2 && tRight < 9)
                {
                    tRight++;
                    if (board[index1, tRight] == 0)
                    {
                        right1 = 0;
                        right2 = 0;
                        rightLine = true;
                    }
                    if (board[index1, tRight] > 0 && board[index1, tRight] != whoseMove)
                    {
                        totalFieldsToChange++;
                    }
                }
                else
                {
                    rightLine = true;
                }
                if (board[rightDown1, rightDown2] > 0 && tRightDown1 != rightDown1 && tRightDown2 != rightDown2 && tRightDown1 < 9 && tRightDown2 < 9)
                {
                    tRightDown1++;
                    tRightDown2++;
                    if (board[tRightDown1, tRightDown2] == 0)
                    {
                        rightDown1 = 0;
                        rightDown2 = 0;
                        rightDownLine = true;
                    }
                    if (board[tRightDown1, tRightDown2] > 0 && board[tRightDown1, tRightDown2] != whoseMove)
                    {
                        totalFieldsToChange++;
                    }
                }
                else
                {
                    rightDownLine = true;
                }
                if (board[down1, down2] > 0 && tDown != down1 && tDown < 9)
                {
                    tDown++;
                    if (board[tDown, index2] == 0)
                    {
                        down1 = 0;
                        down2 = 0;
                        downLine = true;
                    }
                    if (board[tDown, index2] > 0 && board[tDown, index2] != whoseMove)
                    {
                        totalFieldsToChange++;
                    }
                }
                else
                {
                    downLine = true;
                }
                if (board[downLeft1, downLeft2] > 0 && tDownLeft1 != downLeft1 && tDownLeft2 != downLeft2 && tDownLeft1 < 9 && tDownLeft2 > 0)
                {
                    tDownLeft1++;
                    tDownLeft2--;
                    if (board[tDownLeft1, tDownLeft2] == 0)
                    {
                        downLeft1 = 0;
                        downLeft2 = 0;
                        downLeftLine = true;
                    }
                    if (board[tDownLeft1, tDownLeft2] > 0 && board[tDownLeft1, tDownLeft2] != whoseMove)
                    {
                        totalFieldsToChange++;
                    }
                }
                else
                {
                    downLeftLine = true;
                }
                if (board[left1, left2] > 0 && tLeft != left2 && tLeft > 0)
                {
                    tLeft--;
                    if (board[index1, tLeft] == 0)
                    {
                        left1 = 0;
                        left2 = 0;
                        leftLine = true;
                    }
                    if (board[index1, tLeft] > 0 && board[index1, tLeft] != whoseMove)
                    {
                        totalFieldsToChange++;
                    }
                }
                else
                {
                    leftLine = true;
                }
                if (board[leftUp1, leftUp2] > 0 && tLeftUp1 != leftUp1 && tLeftUp2 != leftUp2 && tLeftUp1 > 0 && tLeftUp2 > 0)
                {
                    tLeftUp1--;
                    tLeftUp2--;
                    if (board[tLeftUp1, tLeftUp2] == 0)
                    {
                        leftUp1 = 0;
                        leftUp2 = 0;
                        leftUpLine = true;
                    }
                    if (board[tLeftUp1, tLeftUp2] > 0 && board[tLeftUp1, tLeftUp2] != whoseMove)
                    {
                        totalFieldsToChange++;
                    }
                }
                else
                {
                    leftUpLine = true;
                }
            }
            if (board[up1, up2] == -1 && board[rightUp1, rightUp2] == -1 && board[right1, right2] == -1 && board[rightDown1, rightDown2] == -1 &&
                board[down1, down2] == -1 && board[downLeft1, downLeft2] == -1 && board[left1, left2] == -1 && board[leftUp1, leftUp2] == -1 && totalFieldsToChange == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        // Check every line for last field with the same number and return false if there isn't any
        // saved global variables will be used to swap colours at the end of move
        bool CheckLines(int index1, int index2, int[,] board, int whoseMove)
        {
            bool upLine = true;
            bool rightUpLine = true;
            bool rightLine = true;
            bool rightDownLine = true;
            bool downLine = true;
            bool downLeftLine = true;
            bool leftLine = true;
            bool leftUpLine = true;

            int tIndex1 = index1;
            int tIndex2 = index2;
            int tUp = index1;
            int tRightUp1 = index1, tRightUp2 = index2;
            int tRight = index2;
            int tRightDown1 = index1, tRightDown2 = index2;
            int tDown = index1;
            int tDownLeft1 = index1, tDownLeft2 = index2;
            int tLeft = index2;
            int tLeftUp1 = index1, tLeftUp2 = index2;

            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (board[tIndex1 + i, tIndex2 + j] != whoseMove && board[tIndex1 + i, tIndex2 + j] > 0)
                    {
                        int k = 2;
                        if (tIndex1 + i * k >= 0 && tIndex2 + j * k >= 0 && tIndex1 + i * k <= 9 && tIndex2 + j * k <= 9)
                        {
                            try
                            {
                                while (board[tIndex1 + i * k, tIndex2 + j * k] != whoseMove && board[tIndex1 + i * k, tIndex2 + j * k] > 0)
                                {
                                    k++;
                                    // dorobic sprawdzenie pol przeciwnika
                                }
                            }
                            catch
                            {
                                k--;
                            }
                            if (board[tIndex1 + i * k, tIndex2 + j * k] == whoseMove)
                            {
                                if (i == -1 && j == -1)
                                {
                                    leftUpLine = false;
                                }
                                if (i == -1 && j == 0)
                                {
                                    upLine = false;
                                }
                                if (i == -1 && j == 1)
                                {
                                    rightUpLine = false;
                                }
                                if (i == 0 && j == -1)
                                {
                                    leftLine = false;
                                }
                                if (i == 0 && j == 1)
                                {
                                    rightLine = false;
                                }
                                if (i == 1 && j == -1)
                                {
                                    downLeftLine = false;
                                }
                                if (i == 1 && j == 0)
                                {
                                    downLine = false;
                                }
                                if (i == 1 && j == 1)
                                {
                                    rightDownLine = false;
                                }
                            }
                        }
                    }
                }
            }

            while (upLine == false || rightUpLine == false || rightLine == false || rightDownLine == false ||
                downLine == false || downLeftLine == false || leftLine == false || leftUpLine == false)
            {
                if (upLine == false)
                {
                    tUp--;
                    if (board[tUp, index2] == whoseMove)
                    {
                        up1 = tUp;
                        up2 = index2;
                        upLine = true;
                    }
                    if (board[tUp, index2] == -1)
                    {
                        upLine = true;
                    }
                }
                if (rightUpLine == false)
                {
                    tRightUp1--;
                    tRightUp2++;
                    if (board[tRightUp1, tRightUp2] == whoseMove)
                    {
                        rightUp1 = tRightUp1;
                        rightUp2 = tRightUp2;
                        rightUpLine = true;
                    }
                    if (board[tRightUp1, tRightUp2] == -1)
                    {
                        rightUpLine = true;
                    }
                }
                if (rightLine == false)
                {
                    tRight++;
                    if (board[index1, tRight] == whoseMove)
                    {
                        right1 = index1;
                        right2 = tRight;
                        rightLine = true;
                    }
                    if (board[index1, tRight] == -1)
                    {
                        rightLine = true;
                    }
                }
                if (rightDownLine == false)
                {
                    tRightDown1++;
                    tRightDown2++;
                    if (board[tRightDown1, tRightDown2] == whoseMove)
                    {
                        rightDown1 = tRightDown1;
                        rightDown2 = tRightDown2;
                        rightDownLine = true;
                    }
                    if (board[tRightDown1, tRightDown2] == -1)
                    {
                        rightDownLine = true;
                    }
                }
                if (downLine == false)
                {
                    tDown++;
                    if (board[tDown, index2] == whoseMove)
                    {
                        down1 = tDown;
                        down2 = index2;
                        downLine = true;
                    }
                    if (board[tDown, index2] == -1)
                    {
                        downLine = true;
                    }
                }
                if (downLeftLine == false)
                {
                    tDownLeft1++;
                    tDownLeft2--;
                    if (board[tDownLeft1, tDownLeft2] == whoseMove)
                    {
                        downLeft1 = tDownLeft1;
                        downLeft2 = tDownLeft2;
                        downLeftLine = true;
                    }
                    if (board[tDownLeft1, tDownLeft2] == -1)
                    {
                        downLeftLine = true;
                    }
                }
                if (leftLine == false)
                {
                    tLeft--;
                    if (board[index1, tLeft] == whoseMove)
                    {
                        left1 = index1;
                        left2 = tLeft;
                        leftLine = true;
                    }
                    if (board[index1, tLeft] == -1)
                    {
                        leftLine = true;
                    }
                }
                if (leftUpLine == false)
                {
                    tLeftUp1--;
                    tLeftUp2--;
                    if (board[tLeftUp1, tLeftUp2] == whoseMove)
                    {
                        leftUp1 = tLeftUp1;
                        leftUp2 = tLeftUp2;
                        leftUpLine = true;
                    }
                    if (board[tLeftUp1, tLeftUp2] == -1)
                    {
                        leftUpLine = true;
                    }
                }
            }

            if (board[up1, up2] != -1 || board[rightUp1, rightUp2] != -1 || board[right1, right2] != -1 || board[rightDown1, rightDown2] != -1 ||
                board[down1, down2] != -1 || board[downLeft1, downLeft2] != -1 || board[left1, left2] != -1 || board[leftUp1, leftUp2] != -1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        // Check if move is possible
        // WhoseMove is 1 for brown players and 2 for green
        bool CorrectMove(int index1, int index2, int[,] board, int whoseMove)
        {
            int tIndex1 = index1;
            int tIndex2 = index2;
            if (board[index1, index2] == 0)
            {
                if (board[tIndex1 - 1, tIndex2] > 0 || board[tIndex1 - 1, tIndex2 - 1] > 0 || board[tIndex1, tIndex2 - 1] > 0 || board[tIndex1 + 1, tIndex2 - 1] > 0 ||
                    board[tIndex1 + 1, tIndex2] > 0 || board[tIndex1 + 1, tIndex2 + 1] > 0 || board[tIndex1, tIndex2 + 1] > 0 || board[tIndex1 + 1, tIndex2 + 1] > 0)
                {
                    if (CheckLines(index1, index2, board, whoseMove))
                    {
                        if (FieldToChange(index1, index2, board) == true)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Wrong field");
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Field is taken");
                return false;
            }
        }
        // Filling borad array, 0 for empty field, 1 for brown player, 2 for green player, -1 for out of board
        public void SetGame()
        {
            whoseMoveLabel.Text = "BROWN";
            for (int i = 0; i <= 9; i++)
            {
                for (int j = 0; j <= 9; j++)
                {
                    if (i == 0 || j == 9 || i == 9 || j == 0)
                    {
                        board[i, j] = -1;
                    }
                    else
                    {
                        if (i == 4 && j == 4 || i == 5 && j == 5)
                        {
                            board[i, j] = 1;
                        }
                        else
                        {
                            if (i == 5 && j == 4 || i == 4 && j == 5)
                            {
                                board[i, j] = 2;
                            }
                            else
                            {
                                board[i, j] = 0;
                            }
                        }
                    }

                }
            }
        }
        public void ChangeColours()
        {
            int colour = 0;

            colour = board[1, 1];
            if (colour == 1)
            {
                a1.BackColor = Color.Brown;
            }
            else
            {
                if (colour == 2)
                {
                    a1.BackColor = Color.Green;
                }
                else
                {
                    // do nothing
                }
            }
            colour = board[2, 1];
            if (colour == 1)
            {
                a2.BackColor = Color.Brown;
            }
            else
            {
                if (colour == 2)
                {
                    a2.BackColor = Color.Green;
                }
                else
                {
                    // do nothing
                }
            }
            colour = board[3, 1];
            if (colour == 1)
            {
                a3.BackColor = Color.Brown;
            }
            else
            {
                if (colour == 2)
                {
                    a3.BackColor = Color.Green;
                }
                else
                {
                    // do nothing
                }
            }
            colour = board[4, 1];
            if (colour == 1)
            {
                a4.BackColor = Color.Brown;
            }
            else
            {
                if (colour == 2)
                {
                    a4.BackColor = Color.Green;
                }
                else
                {
                    // do nothing
                }
            }
            colour = board[5, 1];
            if (colour == 1)
            {
                a5.BackColor = Color.Brown;
            }
            else
            {
                if (colour == 2)
                {
                    a5.BackColor = Color.Green;
                }
                else
                {
                    // do nothing
                }
            }
            colour = board[6, 1];
            if (colour == 1)
            {
                a6.BackColor = Color.Brown;
            }
            else
            {
                if (colour == 2)
                {
                    a6.BackColor = Color.Green;
                }
                else
                {
                    // do nothing
                }
            }
            colour = board[7, 1];
            if (colour == 1)
            {
                a7.BackColor = Color.Brown;
            }
            else
            {
                if (colour == 2)
                {
                    a7.BackColor = Color.Green;
                }
                else
                {
                    // do nothing
                }
            }
            colour = board[8, 1];
            if (colour == 1)
            {
                a8.BackColor = Color.Brown;
            }
            else
            {
                if (colour == 2)
                {
                    a8.BackColor = Color.Green;
                }
                else
                {
                    // do nothing
                }
            }
            colour = board[1, 2];
            if (colour == 1)
            {
                b1.BackColor = Color.Brown;
            }
            else
            {
                if (colour == 2)
                {
                    b1.BackColor = Color.Green;
                }
                else
                {
                    // do nothing
                }
            }
            colour = board[2, 2];
            if (colour == 1)
            {
                b2.BackColor = Color.Brown;
            }
            else
            {
                if (colour == 2)
                {
                    b2.BackColor = Color.Green;
                }
                else
                {
                    // do nothing
                }
            }
            colour = board[3, 2];
            if (colour == 1)
            {
                b3.BackColor = Color.Brown;
            }
            else
            {
                if (colour == 2)
                {
                    b3.BackColor = Color.Green;
                }
                else
                {
                    // do nothing
                }
            }
            colour = board[4, 2];
            if (colour == 1)
            {
                b4.BackColor = Color.Brown;
            }
            else
            {
                if (colour == 2)
                {
                    b4.BackColor = Color.Green;
                }
                else
                {
                    // do nothing
                }
            }
            colour = board[5, 2];
            if (colour == 1)
            {
                b5.BackColor = Color.Brown;
            }
            else
            {
                if (colour == 2)
                {
                    b5.BackColor = Color.Green;
                }
                else
                {
                    // do nothing
                }
            }
            colour = board[6, 2];
            if (colour == 1)
            {
                b6.BackColor = Color.Brown;
            }
            else
            {
                if (colour == 2)
                {
                    b6.BackColor = Color.Green;
                }
                else
                {
                    // do nothing
                }
            }
            colour = board[7, 2];
            if (colour == 1)
            {
                b7.BackColor = Color.Brown;
            }
            else
            {
                if (colour == 2)
                {
                    b7.BackColor = Color.Green;
                }
                else
                {
                    // do nothing
                }
            }
            colour = board[8, 2];
            if (colour == 1)
            {
                b8.BackColor = Color.Brown;
            }
            else
            {
                if (colour == 2)
                {
                    b8.BackColor = Color.Green;
                }
                else
                {
                    // do nothing
                }
            }
            colour = board[1, 3];
            if (colour == 1)
            {
                c1.BackColor = Color.Brown;
            }
            else
            {
                if (colour == 2)
                {
                    c1.BackColor = Color.Green;
                }
                else
                {
                    // do nothing
                }
            }
            colour = board[2, 3];
            if (colour == 1)
            {
                c2.BackColor = Color.Brown;
            }
            else
            {
                if (colour == 2)
                {
                    c2.BackColor = Color.Green;
                }
                else
                {
                    // do nothing
                }
            }
            colour = board[3, 3];
            if (colour == 1)
            {
                c3.BackColor = Color.Brown;
            }
            else
            {
                if (colour == 2)
                {
                    c3.BackColor = Color.Green;
                }
                else
                {
                    // do nothing
                }
            }
            colour = board[4, 3];
            if (colour == 1)
            {
                c4.BackColor = Color.Brown;
            }
            else
            {
                if (colour == 2)
                {
                    c4.BackColor = Color.Green;
                }
                else
                {
                    // do nothing
                }
            }
            colour = board[5, 3];
            if (colour == 1)
            {
                c5.BackColor = Color.Brown;
            }
            else
            {
                if (colour == 2)
                {
                    c5.BackColor = Color.Green;
                }
                else
                {
                    // do nothing
                }
            }
            colour = board[6, 3];
            if (colour == 1)
            {
                c6.BackColor = Color.Brown;
            }
            else
            {
                if (colour == 2)
                {
                    c6.BackColor = Color.Green;
                }
                else
                {
                    // do nothing
                }
            }
            colour = board[7, 3];
            if (colour == 1)
            {
                c7.BackColor = Color.Brown;
            }
            else
            {
                if (colour == 2)
                {
                    c7.BackColor = Color.Green;
                }
                else
                {
                    // do nothing
                }
            }
            colour = board[8, 3];
            if (colour == 1)
            {
                c8.BackColor = Color.Brown;
            }
            else
            {
                if (colour == 2)
                {
                    c8.BackColor = Color.Green;
                }
                else
                {
                    // do nothing
                }
            }
            colour = board[1, 4];
            if (colour == 1)
            {
                d1.BackColor = Color.Brown;
            }
            else
            {
                if (colour == 2)
                {
                    d1.BackColor = Color.Green;
                }
                else
                {
                    // do nothing
                }
            }
            colour = board[2, 4];
            if (colour == 1)
            {
                d2.BackColor = Color.Brown;
            }
            else
            {
                if (colour == 2)
                {
                    d2.BackColor = Color.Green;
                }
                else
                {
                    // do nothing
                }
            }
            colour = board[3, 4];
            if (colour == 1)
            {
                d3.BackColor = Color.Brown;
            }
            else
            {
                if (colour == 2)
                {
                    d3.BackColor = Color.Green;
                }
                else
                {
                    // do nothing
                }
            }
            colour = board[4, 4];
            if (colour == 1)
            {
                d4.BackColor = Color.Brown;
            }
            else
            {
                if (colour == 2)
                {
                    d4.BackColor = Color.Green;
                }
                else
                {
                    // do nothing
                }
            }
            colour = board[5, 4];
            if (colour == 1)
            {
                d5.BackColor = Color.Brown;
            }
            else
            {
                if (colour == 2)
                {
                    d5.BackColor = Color.Green;
                }
                else
                {
                    // do nothing
                }
            }
            colour = board[6, 4];
            if (colour == 1)
            {
                d6.BackColor = Color.Brown;
            }
            else
            {
                if (colour == 2)
                {
                    d6.BackColor = Color.Green;
                }
                else
                {
                    // do nothing
                }
            }
            colour = board[7, 4];
            if (colour == 1)
            {
                d7.BackColor = Color.Brown;
            }
            else
            {
                if (colour == 2)
                {
                    d7.BackColor = Color.Green;
                }
                else
                {
                    // do nothing
                }
            }
            colour = board[8, 4];
            if (colour == 1)
            {
                d8.BackColor = Color.Brown;
            }
            else
            {
                if (colour == 2)
                {
                    d8.BackColor = Color.Green;
                }
                else
                {
                    // do nothing
                }
            }
            colour = board[1, 5];
            if (colour == 1)
            {
                e1.BackColor = Color.Brown;
            }
            else
            {
                if (colour == 2)
                {
                    e1.BackColor = Color.Green;
                }
                else
                {
                    // do nothing
                }
            }
            colour = board[2, 5];
            if (colour == 1)
            {
                e2.BackColor = Color.Brown;
            }
            else
            {
                if (colour == 2)
                {
                    e2.BackColor = Color.Green;
                }
                else
                {
                    // do nothing
                }
            }
            colour = board[3, 5];
            if (colour == 1)
            {
                e3.BackColor = Color.Brown;
            }
            else
            {
                if (colour == 2)
                {
                    e3.BackColor = Color.Green;
                }
                else
                {
                    // do nothing
                }
            }
            colour = board[4, 5];
            if (colour == 1)
            {
                e4.BackColor = Color.Brown;
            }
            else
            {
                if (colour == 2)
                {
                    e4.BackColor = Color.Green;
                }
                else
                {
                    // do nothing
                }
            }
            colour = board[5, 5];
            if (colour == 1)
            {
                e5.BackColor = Color.Brown;
            }
            else
            {
                if (colour == 2)
                {
                    e5.BackColor = Color.Green;
                }
                else
                {
                    // do nothing
                }
            }
            colour = board[6, 5];
            if (colour == 1)
            {
                e6.BackColor = Color.Brown;
            }
            else
            {
                if (colour == 2)
                {
                    e6.BackColor = Color.Green;
                }
                else
                {
                    // do nothing
                }
            }
            colour = board[7, 5];
            if (colour == 1)
            {
                e7.BackColor = Color.Brown;
            }
            else
            {
                if (colour == 2)
                {
                    e7.BackColor = Color.Green;
                }
                else
                {
                    // do nothing
                }
            }
            colour = board[8, 5];
            if (colour == 1)
            {
                e8.BackColor = Color.Brown;
            }
            else
            {
                if (colour == 2)
                {
                    e8.BackColor = Color.Green;
                }
                else
                {
                    // do nothing
                }
            }
            colour = board[1, 6];
            if (colour == 1)
            {
                f1.BackColor = Color.Brown;
            }
            else
            {
                if (colour == 2)
                {
                    f1.BackColor = Color.Green;
                }
                else
                {
                    // do nothing
                }
            }
            colour = board[2, 6];
            if (colour == 1)
            {
                f2.BackColor = Color.Brown;
            }
            else
            {
                if (colour == 2)
                {
                    f2.BackColor = Color.Green;
                }
                else
                {
                    // do nothing
                }
            }
            colour = board[3, 6];
            if (colour == 1)
            {
                f3.BackColor = Color.Brown;
            }
            else
            {
                if (colour == 2)
                {
                    f3.BackColor = Color.Green;
                }
                else
                {
                    // do nothing
                }
            }
            colour = board[4, 6];
            if (colour == 1)
            {
                f4.BackColor = Color.Brown;
            }
            else
            {
                if (colour == 2)
                {
                    f4.BackColor = Color.Green;
                }
                else
                {
                    // do nothing
                }
            }
            colour = board[5, 6];
            if (colour == 1)
            {
                f5.BackColor = Color.Brown;
            }
            else
            {
                if (colour == 2)
                {
                    f5.BackColor = Color.Green;
                }
                else
                {
                    // do nothing
                }
            }
            colour = board[6, 6];
            if (colour == 1)
            {
                f6.BackColor = Color.Brown;
            }
            else
            {
                if (colour == 2)
                {
                    f6.BackColor = Color.Green;
                }
                else
                {
                    // do nothing
                }
            }
            colour = board[7, 6];
            if (colour == 1)
            {
                f7.BackColor = Color.Brown;
            }
            else
            {
                if (colour == 2)
                {
                    f7.BackColor = Color.Green;
                }
                else
                {
                    // do nothing
                }
            }
            colour = board[8, 6];
            if (colour == 1)
            {
                f8.BackColor = Color.Brown;
            }
            else
            {
                if (colour == 2)
                {
                    f8.BackColor = Color.Green;
                }
                else
                {
                    // do nothing
                }
            }
            colour = board[1, 7];
            if (colour == 1)
            {
                g1.BackColor = Color.Brown;
            }
            else
            {
                if (colour == 2)
                {
                    g1.BackColor = Color.Green;
                }
                else
                {
                    // do nothing
                }
            }
            colour = board[2, 7];
            if (colour == 1)
            {
                g2.BackColor = Color.Brown;
            }
            else
            {
                if (colour == 2)
                {
                    g2.BackColor = Color.Green;
                }
                else
                {
                    // do nothing
                }
            }
            colour = board[3, 7];
            if (colour == 1)
            {
                g3.BackColor = Color.Brown;
            }
            else
            {
                if (colour == 2)
                {
                    g3.BackColor = Color.Green;
                }
                else
                {
                    // do nothing
                }
            }
            colour = board[4, 7];
            if (colour == 1)
            {
                g4.BackColor = Color.Brown;
            }
            else
            {
                if (colour == 2)
                {
                    g4.BackColor = Color.Green;
                }
                else
                {
                    // do nothing
                }
            }
            colour = board[5, 7];
            if (colour == 1)
            {
                g5.BackColor = Color.Brown;
            }
            else
            {
                if (colour == 2)
                {
                    g5.BackColor = Color.Green;
                }
                else
                {
                    // do nothing
                }
            }
            colour = board[6, 7];
            if (colour == 1)
            {
                g6.BackColor = Color.Brown;
            }
            else
            {
                if (colour == 2)
                {
                    g6.BackColor = Color.Green;
                }
                else
                {
                    // do nothing
                }
            }
            colour = board[7, 7];
            if (colour == 1)
            {
                g7.BackColor = Color.Brown;
            }
            else
            {
                if (colour == 2)
                {
                    g7.BackColor = Color.Green;
                }
                else
                {
                    // do nothing
                }
            }
            colour = board[8, 7];
            if (colour == 1)
            {
                g8.BackColor = Color.Brown;
            }
            else
            {
                if (colour == 2)
                {
                    g8.BackColor = Color.Green;
                }
                else
                {
                    // do nothing
                }
            }
            colour = board[1, 8];
            if (colour == 1)
            {
                h1.BackColor = Color.Brown;
            }
            else
            {
                if (colour == 2)
                {
                    h1.BackColor = Color.Green;
                }
                else
                {
                    // do nothing
                }
            }
            colour = board[2, 8];
            if (colour == 1)
            {
                h2.BackColor = Color.Brown;
            }
            else
            {
                if (colour == 2)
                {
                    h2.BackColor = Color.Green;
                }
                else
                {
                    // do nothing
                }
            }
            colour = board[3, 8];
            if (colour == 1)
            {
                h3.BackColor = Color.Brown;
            }
            else
            {
                if (colour == 2)
                {
                    h3.BackColor = Color.Green;
                }
                else
                {
                    // do nothing
                }
            }
            colour = board[4, 8];
            if (colour == 1)
            {
                h4.BackColor = Color.Brown;
            }
            else
            {
                if (colour == 2)
                {
                    h4.BackColor = Color.Green;
                }
                else
                {
                    // do nothing
                }
            }
            colour = board[5, 8];
            if (colour == 1)
            {
                h5.BackColor = Color.Brown;
            }
            else
            {
                if (colour == 2)
                {
                    h5.BackColor = Color.Green;
                }
                else
                {
                    // do nothing
                }
            }
            colour = board[6, 8];
            if (colour == 1)
            {
                h6.BackColor = Color.Brown;
            }
            else
            {
                if (colour == 2)
                {
                    h6.BackColor = Color.Green;
                }
                else
                {
                    // do nothing
                }
            }
            colour = board[7, 8];
            if (colour == 1)
            {
                h7.BackColor = Color.Brown;
            }
            else
            {
                if (colour == 2)
                {
                    h7.BackColor = Color.Green;
                }
                else
                {
                    // do nothing
                }
            }
            colour = board[8, 8];
            if (colour == 1)
            {
                h8.BackColor = Color.Brown;
            }
            else
            {
                if (colour == 2)
                {
                    h8.BackColor = Color.Green;
                }
                else
                {
                    // do nothing
                }
            }
        }
        public Form1()
        {
            InitializeComponent();
        }



        void NewPvPGame_Click(object sender, EventArgs e)
        {
            NameWindow nameWindow = new NameWindow();

            if (nameWindow.ShowDialog(this) == DialogResult.OK)
            {
                greenName.Text = nameWindow.greenPlayerName;
                brownName.Text = nameWindow.brownPlayerName;
                SetGame();
                ChangeColours();
                pvPGame = true;
            }
            
        }

        void Form1_Load(object sender, EventArgs e)
        {

        }

        private void a1_Click(object sender, EventArgs e)
        {
            clickedIndex1 = 1;
            clickedIndex2 = 1;

            if(pvPGame)
            {
                PvPGame();
            }
            else if(pvAIGame)
            {
                PvAIGame();
            }
            else if(aivAIGame)
            {

            }
        }

        private void a2_Click(object sender, EventArgs e)
        {
            clickedIndex1 = 2;
            clickedIndex2 = 1;

            if (pvPGame)
            {
                PvPGame();
            }
            else if (pvAIGame)
            {
                PvAIGame();
            }
            else if (aivAIGame)
            {

            }
        }

        private void a3_Click(object sender, EventArgs e)
        {
            clickedIndex1 = 3;
            clickedIndex2 = 1;

            if (pvPGame)
            {
                PvPGame();
            }
            else if (pvAIGame)
            {
                PvAIGame();
            }
            else if (aivAIGame)
            {

            }
        }

        private void a4_Click(object sender, EventArgs e)
        {
            clickedIndex1 = 4;
            clickedIndex2 = 1;

            if (pvPGame)
            {
                PvPGame();
            }
            else if (pvAIGame)
            {
                PvAIGame();
            }
            else if (aivAIGame)
            {

            }
        }

        private void a5_Click(object sender, EventArgs e)
        {
            clickedIndex1 = 5;
            clickedIndex2 = 1;

            if (pvPGame)
            {
                PvPGame();
            }
            else if (pvAIGame)
            {
                PvAIGame();
            }
            else if (aivAIGame)
            {

            }
        }

        private void a6_Click(object sender, EventArgs e)
        {
            clickedIndex1 = 6;
            clickedIndex2 = 1;

            if (pvPGame)
            {
                PvPGame();
            }
            else if (pvAIGame)
            {
                PvAIGame();
            }
            else if (aivAIGame)
            {

            }
        }

        private void a7_Click(object sender, EventArgs e)
        {
            clickedIndex1 = 7;
            clickedIndex2 = 1;

            if (pvPGame)
            {
                PvPGame();
            }
            else if (pvAIGame)
            {
                PvAIGame();
            }
            else if (aivAIGame)
            {

            }
        }

        private void a8_Click(object sender, EventArgs e)
        {
            clickedIndex1 = 8;
            clickedIndex2 = 1;

            if (pvPGame)
            {
                PvPGame();
            }
            else if (pvAIGame)
            {
                PvAIGame();
            }
            else if (aivAIGame)
            {

            }
        }

        private void b1_Click(object sender, EventArgs e)
        {
            clickedIndex1 = 1;
            clickedIndex2 = 2;

            if (pvPGame)
            {
                PvPGame();
            }
            else if (pvAIGame)
            {
                PvAIGame();
            }
            else if (aivAIGame)
            {

            }
        }

        private void b2_Click(object sender, EventArgs e)
        {
            clickedIndex1 = 2;
            clickedIndex2 = 2;

            if (pvPGame)
            {
                PvPGame();
            }
            else if (pvAIGame)
            {
                PvAIGame();
            }
            else if (aivAIGame)
            {

            }
        }

        private void b3_Click(object sender, EventArgs e)
        {
            clickedIndex1 = 3;
            clickedIndex2 = 2;

            if (pvPGame)
            {
                PvPGame();
            }
            else if (pvAIGame)
            {
                PvAIGame();
            }
            else if (aivAIGame)
            {

            }
        }

        private void b4_Click(object sender, EventArgs e)
        {
            clickedIndex1 = 4;
            clickedIndex2 = 2;

            if (pvPGame)
            {
                PvPGame();
            }
            else if (pvAIGame)
            {
                PvAIGame();
            }
            else if (aivAIGame)
            {

            }
        }

        private void b5_Click(object sender, EventArgs e)
        {
            clickedIndex1 = 5;
            clickedIndex2 = 2;

            if (pvPGame)
            {
                PvPGame();
            }
            else if (pvAIGame)
            {
                PvAIGame();
            }
            else if (aivAIGame)
            {

            }
        }

        private void b6_Click(object sender, EventArgs e)
        {
            clickedIndex1 = 6;
            clickedIndex2 = 2;

            if (pvPGame)
            {
                PvPGame();
            }
            else if (pvAIGame)
            {
                PvAIGame();
            }
            else if (aivAIGame)
            {

            }
        }

        private void b7_Click(object sender, EventArgs e)
        {
            clickedIndex1 = 7;
            clickedIndex2 = 2;

            if (pvPGame)
            {
                PvPGame();
            }
            else if (pvAIGame)
            {
                PvAIGame();
            }
            else if (aivAIGame)
            {

            }
        }

        private void b8_Click(object sender, EventArgs e)
        {
            clickedIndex1 = 8;
            clickedIndex2 = 2;

            if (pvPGame)
            {
                PvPGame();
            }
            else if (pvAIGame)
            {
                PvAIGame();
            }
            else if (aivAIGame)
            {

            }
        }

        private void c1_Click(object sender, EventArgs e)
        {
            clickedIndex1 = 1;
            clickedIndex2 = 3;

            if (pvPGame)
            {
                PvPGame();
            }
            else if (pvAIGame)
            {
                PvAIGame();
            }
            else if (aivAIGame)
            {

            }
        }

        private void c2_Click(object sender, EventArgs e)
        {
            clickedIndex1 = 2;
            clickedIndex2 = 3;

            if (pvPGame)
            {
                PvPGame();
            }
            else if (pvAIGame)
            {
                PvAIGame();
            }
            else if (aivAIGame)
            {

            }
        }

        private void c3_Click(object sender, EventArgs e)
        {
            clickedIndex1 = 3;
            clickedIndex2 = 3;

            if (pvPGame)
            {
                PvPGame();
            }
            else if (pvAIGame)
            {
                PvAIGame();
            }
            else if (aivAIGame)
            {

            }
        }

        private void c4_Click(object sender, EventArgs e)
        {
            clickedIndex1 = 4;
            clickedIndex2 = 3;

            if (pvPGame)
            {
                PvPGame();
            }
            else if (pvAIGame)
            {
                PvAIGame();
            }
            else if (aivAIGame)
            {

            }
        }

        private void c5_Click(object sender, EventArgs e)
        {
            clickedIndex1 = 5;
            clickedIndex2 = 3;

            if (pvPGame)
            {
                PvPGame();
            }
            else if (pvAIGame)
            {
                PvAIGame();
            }
            else if (aivAIGame)
            {

            }
        }

        private void c6_Click(object sender, EventArgs e)
        {
            clickedIndex1 = 6;
            clickedIndex2 = 3;

            if (pvPGame)
            {
                PvPGame();
            }
            else if (pvAIGame)
            {
                PvAIGame();
            }
            else if (aivAIGame)
            {

            }
        }

        private void c7_Click(object sender, EventArgs e)
        {
            clickedIndex1 = 7;
            clickedIndex2 = 3;

            if (pvPGame)
            {
                PvPGame();
            }
            else if (pvAIGame)
            {
                PvAIGame();
            }
            else if (aivAIGame)
            {

            }
        }

        private void c8_Click(object sender, EventArgs e)
        {
            clickedIndex1 = 8;
            clickedIndex2 = 3;

            if (pvPGame)
            {
                PvPGame();
            }
            else if (pvAIGame)
            {
                PvAIGame();
            }
            else if (aivAIGame)
            {

            }
        }

        private void d1_Click(object sender, EventArgs e)
        {
            clickedIndex1 = 1;
            clickedIndex2 = 4;

            if (pvPGame)
            {
                PvPGame();
            }
            else if (pvAIGame)
            {
                PvAIGame();
            }
            else if (aivAIGame)
            {

            }
        }

        private void d2_Click(object sender, EventArgs e)
        {
            clickedIndex1 = 2;
            clickedIndex2 = 4;

            if (pvPGame)
            {
                PvPGame();
            }
            else if (pvAIGame)
            {
                PvAIGame();
            }
            else if (aivAIGame)
            {

            }
        }

        private void d3_Click(object sender, EventArgs e)
        {
            clickedIndex1 = 3;
            clickedIndex2 = 4;

            if (pvPGame)
            {
                PvPGame();
            }
            else if (pvAIGame)
            {
                PvAIGame();
            }
            else if (aivAIGame)
            {

            }
        }

        private void d4_Click(object sender, EventArgs e)
        {
            clickedIndex1 = 4;
            clickedIndex2 = 4;

            if (pvPGame)
            {
                PvPGame();
            }
            else if (pvAIGame)
            {
                PvAIGame();
            }
            else if (aivAIGame)
            {

            }
        }

        private void d5_Click(object sender, EventArgs e)
        {
            clickedIndex1 = 5;
            clickedIndex2 = 4;

            if (pvPGame)
            {
                PvPGame();
            }
            else if (pvAIGame)
            {
                PvAIGame();
            }
            else if (aivAIGame)
            {

            }
        }

        private void d6_Click(object sender, EventArgs e)
        {
            clickedIndex1 = 6;
            clickedIndex2 = 4;

            if (pvPGame)
            {
                PvPGame();
            }
            else if (pvAIGame)
            {
                PvAIGame();
            }
            else if (aivAIGame)
            {

            }
        }

        private void d7_Click(object sender, EventArgs e)
        {
            clickedIndex1 = 7;
            clickedIndex2 = 4;

            if (pvPGame)
            {
                PvPGame();
            }
            else if (pvAIGame)
            {
                PvAIGame();
            }
            else if (aivAIGame)
            {

            }
        }

        private void d8_Click(object sender, EventArgs e)
        {
            clickedIndex1 = 8;
            clickedIndex2 = 4;

            if (pvPGame)
            {
                PvPGame();
            }
            else if (pvAIGame)
            {
                PvAIGame();
            }
            else if (aivAIGame)
            {

            }
        }

        private void e1_Click(object sender, EventArgs e)
        {
            clickedIndex1 = 1;
            clickedIndex2 = 5;

            if (pvPGame)
            {
                PvPGame();
            }
            else if (pvAIGame)
            {
                PvAIGame();
            }
            else if (aivAIGame)
            {

            }
        }

        private void e2_Click(object sender, EventArgs e)
        {
            clickedIndex1 = 2;
            clickedIndex2 = 5;

            if (pvPGame)
            {
                PvPGame();
            }
            else if (pvAIGame)
            {
                PvAIGame();
            }
            else if (aivAIGame)
            {

            }
        }

        private void e3_Click(object sender, EventArgs e)
        {
            clickedIndex1 = 3;
            clickedIndex2 = 5;

            if (pvPGame)
            {
                PvPGame();
            }
            else if (pvAIGame)
            {
                PvAIGame();
            }
            else if (aivAIGame)
            {

            }
        }

        private void e4_Click(object sender, EventArgs e)
        {
            clickedIndex1 = 4;
            clickedIndex2 = 5;

            if (pvPGame)
            {
                PvPGame();
            }
            else if (pvAIGame)
            {
                PvAIGame();
            }
            else if (aivAIGame)
            {

            }
        }

        private void e5_Click(object sender, EventArgs e)
        {
            clickedIndex1 = 5;
            clickedIndex2 = 5;

            if (pvPGame)
            {
                PvPGame();
            }
            else if (pvAIGame)
            {
                PvAIGame();
            }
            else if (aivAIGame)
            {

            }
        }

        private void e6_Click(object sender, EventArgs e)
        {
            clickedIndex1 = 6;
            clickedIndex2 = 5;

            if (pvPGame)
            {
                PvPGame();
            }
            else if (pvAIGame)
            {
                PvAIGame();
            }
            else if (aivAIGame)
            {

            }
        }

        private void e7_Click(object sender, EventArgs e)
        {
            clickedIndex1 = 7;
            clickedIndex2 = 5;

            if (pvPGame)
            {
                PvPGame();
            }
            else if (pvAIGame)
            {
                PvAIGame();
            }
            else if (aivAIGame)
            {

            }
        }

        private void e8_Click(object sender, EventArgs e)
        {
            clickedIndex1 = 8;
            clickedIndex2 = 5;

            if (pvPGame)
            {
                PvPGame();
            }
            else if (pvAIGame)
            {
                PvAIGame();
            }
            else if (aivAIGame)
            {

            }
        }

        private void f1_Click(object sender, EventArgs e)
        {
            clickedIndex1 = 1;
            clickedIndex2 = 6;

            if (pvPGame)
            {
                PvPGame();
            }
            else if (pvAIGame)
            {
                PvAIGame();
            }
            else if (aivAIGame)
            {

            }
        }

        private void f2_Click(object sender, EventArgs e)
        {
            clickedIndex1 = 2;
            clickedIndex2 = 6;

            if (pvPGame)
            {
                PvPGame();
            }
            else if (pvAIGame)
            {
                PvAIGame();
            }
            else if (aivAIGame)
            {

            }
        }

        private void f3_Click(object sender, EventArgs e)
        {
            clickedIndex1 = 3;
            clickedIndex2 = 6;

            if (pvPGame)
            {
                PvPGame();
            }
            else if (pvAIGame)
            {
                PvAIGame();
            }
            else if (aivAIGame)
            {

            }
        }

        private void f4_Click(object sender, EventArgs e)
        {
            clickedIndex1 = 4;
            clickedIndex2 = 6;

            if (pvPGame)
            {
                PvPGame();
            }
            else if (pvAIGame)
            {
                PvAIGame();
            }
            else if (aivAIGame)
            {

            }
        }

        private void f5_Click(object sender, EventArgs e)
        {
            clickedIndex1 = 5;
            clickedIndex2 = 6;

            if (pvPGame)
            {
                PvPGame();
            }
            else if (pvAIGame)
            {
                PvAIGame();
            }
            else if (aivAIGame)
            {

            }
        }

        private void f6_Click(object sender, EventArgs e)
        {
            clickedIndex1 = 6;
            clickedIndex2 = 6;

            if (pvPGame)
            {
                PvPGame();
            }
            else if (pvAIGame)
            {
                PvAIGame();
            }
            else if (aivAIGame)
            {

            }
        }

        private void f7_Click(object sender, EventArgs e)
        {
            clickedIndex1 = 7;
            clickedIndex2 = 6;

            if (pvPGame)
            {
                PvPGame();
            }
            else if (pvAIGame)
            {
                PvAIGame();
            }
            else if (aivAIGame)
            {

            }
        }

        private void f8_Click(object sender, EventArgs e)
        {
            clickedIndex1 = 8;
            clickedIndex2 = 6;

            if (pvPGame)
            {
                PvPGame();
            }
            else if (pvAIGame)
            {
                PvAIGame();
            }
            else if (aivAIGame)
            {

            }
        }

        private void g1_Click(object sender, EventArgs e)
        {
            clickedIndex1 = 1;
            clickedIndex2 = 7;

            if (pvPGame)
            {
                PvPGame();
            }
            else if (pvAIGame)
            {
                PvAIGame();
            }
            else if (aivAIGame)
            {

            }
        }

        private void g2_Click(object sender, EventArgs e)
        {
            clickedIndex1 = 2;
            clickedIndex2 = 7;

            if (pvPGame)
            {
                PvPGame();
            }
            else if (pvAIGame)
            {
                PvAIGame();
            }
            else if (aivAIGame)
            {

            }
        }

        private void g3_Click(object sender, EventArgs e)
        {
            clickedIndex1 = 3;
            clickedIndex2 = 7;

            if (pvPGame)
            {
                PvPGame();
            }
            else if (pvAIGame)
            {
                PvAIGame();
            }
            else if (aivAIGame)
            {

            }
        }

        private void g4_Click(object sender, EventArgs e)
        {
            clickedIndex1 = 4;
            clickedIndex2 = 7;

            if (pvPGame)
            {
                PvPGame();
            }
            else if (pvAIGame)
            {
                PvAIGame();
            }
            else if (aivAIGame)
            {

            }
        }

        private void g5_Click(object sender, EventArgs e)
        {
            clickedIndex1 = 5;
            clickedIndex2 = 7;

            if (pvPGame)
            {
                PvPGame();
            }
            else if (pvAIGame)
            {
                PvAIGame();
            }
            else if (aivAIGame)
            {

            }
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void g6_Click(object sender, EventArgs e)
        {
            clickedIndex1 = 6;
            clickedIndex2 = 7;

            if (pvPGame)
            {
                PvPGame();
            }
            else if (pvAIGame)
            {
                PvAIGame();
            }
            else if (aivAIGame)
            {

            }
        }

        private void g7_Click(object sender, EventArgs e)
        {
            clickedIndex1 = 7;
            clickedIndex2 = 7;

            if (pvPGame)
            {
                PvPGame();
            }
            else if (pvAIGame)
            {
                PvAIGame();
            }
            else if (aivAIGame)
            {

            }
        }

        private void g8_Click(object sender, EventArgs e)
        {
            clickedIndex1 = 8;
            clickedIndex2 = 7;

            if (pvPGame)
            {
                PvPGame();
            }
            else if (pvAIGame)
            {
                PvAIGame();
            }
            else if (aivAIGame)
            {

            }
        }

        private void h1_Click(object sender, EventArgs e)
        {
            clickedIndex1 = 1;
            clickedIndex2 = 8;

            if (pvPGame)
            {
                PvPGame();
            }
            else if (pvAIGame)
            {
                PvAIGame();
            }
            else if (aivAIGame)
            {

            }
        }

        private void h2_Click(object sender, EventArgs e)
        {
            clickedIndex1 = 2;
            clickedIndex2 = 8;

            if (pvPGame)
            {
                PvPGame();
            }
            else if (pvAIGame)
            {
                PvAIGame();
            }
            else if (aivAIGame)
            {

            }
        }

        private void h3_Click(object sender, EventArgs e)
        {
            clickedIndex1 = 3;
            clickedIndex2 = 8;

            if (pvPGame)
            {
                PvPGame();
            }
            else if (pvAIGame)
            {
                PvAIGame();
            }
            else if (aivAIGame)
            {

            }
        }

        private void h4_Click(object sender, EventArgs e)
        {
            clickedIndex1 = 4;
            clickedIndex2 = 8;

            if (pvPGame)
            {
                PvPGame();
            }
            else if (pvAIGame)
            {
                PvAIGame();
            }
            else if (aivAIGame)
            {

            }
        }

        private void h5_Click(object sender, EventArgs e)
        {
            clickedIndex1 = 5;
            clickedIndex2 = 8;

            if (pvPGame)
            {
                PvPGame();
            }
            else if (pvAIGame)
            {
                PvAIGame();
            }
            else if (aivAIGame)
            {

            }
        }

        private void h6_Click(object sender, EventArgs e)
        {
            clickedIndex1 = 6;
            clickedIndex2 = 8;

            if (pvPGame)
            {
                PvPGame();
            }
            else if (pvAIGame)
            {
                PvAIGame();
            }
            else if (aivAIGame)
            {

            }
        }

        private void h7_Click(object sender, EventArgs e)
        {
            clickedIndex1 = 7;
            clickedIndex2 = 8;

            if (pvPGame)
            {
                PvPGame();
            }
            else if (pvAIGame)
            {
                PvAIGame();
            }
            else if (aivAIGame)
            {

            }
        }

        private void h8_Click(object sender, EventArgs e)
        {
            clickedIndex1 = 8;
            clickedIndex2 = 8;

            if (pvPGame)
            {
                PvPGame();
            }
            else if (pvAIGame)
            {
                PvAIGame();
            }
            else if (aivAIGame)
            {

            }
        }

        private void playerVsAIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NameWindowAI nameWindowAI = new NameWindowAI();

            if (nameWindowAI.ShowDialog(this) == DialogResult.OK)
            {
                if(nameWindowAI.greenRadioButton.Checked)
                {
                    greenName.Text = nameWindowAI.Name;
                    brownName.Text = "Random AI";
                    humanGreen = true;
                }
                else if(nameWindowAI.brownRadioButton.Checked)
                {
                    brownName.Text = nameWindowAI.Name;
                    greenName.Text = "Random AI";
                    humanBrown = true;
                }
                SetGame();
                ChangeColours();
                pvAIGame = true;
                PvAIGame();
            }
        }

        private void aIVsAIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetGame();
            ChangeColours();
            aivAIGame = true;
            AIvAIGame();
        }
    }
}
