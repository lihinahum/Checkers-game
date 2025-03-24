using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ex02
{
    class GameUI
    {
        public void DisplayLastMove(Player i_player,string i_LastMove)
        {
            Console.WriteLine($"{i_player.Name}'s move was ({i_player.PieceShape}) : {i_LastMove}");
        }
        public void DisplayCurrentTurn(Player i_player)
        {
            Console.WriteLine($"{i_player.Name}'s Turn ({i_player.PieceShape}) :");
        }
        public void DisplayMessage(string message)
        {
            Console.WriteLine(message);
        }
        public string GetPlayerMoveInput()
        {
            Console.WriteLine("Enter your move in the next format: Rowcol>Rowcol :");
            return Console.ReadLine();
        }

        //the controller will get the chosen game mode then if it is agains another player, the controller will call 
        // getPlayerName() again and maybe add playerNumber that the function will get as an input param
        public string GetGameModeInput()
        {
            Console.WriteLine("Would you like to play against human player or against the computer?");
            Console.WriteLine("Enter (1) if you would like to play against the computer.");
            Console.WriteLine("Enter (2) if you would like to play against human player.");
            return Console.ReadLine();
        }

        public string GetBoardSizeInput()
        {
            string printMessage = "";
            Console.WriteLine("Please enter the wanted board size for the game:");
            foreach (var option in BoardDefinitions.boardSizeChoiceMapping)
            {
                printMessage = string.Format("Enter ({0}) if you would like to play in {1}X{1} board.", option.Key, option.Value);
                Console.WriteLine(printMessage);
            }
            return Console.ReadLine();
        }

        public string GetPlayerNameInput()
        {
            string printMessage = null;
            printMessage = string.Format("Enter you name: ");
            Console.WriteLine(printMessage);
            return Console.ReadLine();
        }
        
        public void PrintBoard(Board i_Board, int i_BoardSize)
        {
            char colLabel = 'a';
            char rowLabel = 'A';

            Console.Write("  ");
            for (int i = 0; i < i_BoardSize; i++)
            {
                Console.Write($" {colLabel++}  ");
            }
            Console.WriteLine();

            for (int row = 0; row < i_BoardSize; row++)
            {
                printSeparationLine(i_BoardSize);
                Console.Write($"{rowLabel++}|");

                for (int col = 0; col < i_BoardSize; col++)
                {
                    Console.Write($" {i_Board.GetPiece(new Model.Position(row,col)).Owner.PieceShape} |");
                }
                Console.WriteLine();
            }

            printSeparationLine(i_BoardSize);
        }

        private void printSeparationLine(int i_BoardSize)
        {
            Console.Write(" ");
            Console.WriteLine(new string('=', 4 * i_BoardSize + 1));
        }
    }
}
