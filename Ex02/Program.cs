using Ex02.Model;

namespace Ex02
{
    public class Program
    {
        public static void Main()
        {
            GameLogic gameLogic = new GameLogic();
            GameUI gameUI = new GameUI();
            GameManager gameManager = new GameManager(gameLogic, gameUI);

            gameManager.InitiateGame();
        }
    }
}
