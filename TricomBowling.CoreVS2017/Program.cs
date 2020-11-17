using System;

namespace TricomBowling.CoreVS2017
{
    public class Game
    {
        /// <summary>
        /// Total score of the game.
        /// </summary>
        public int Score { get; private set; }

        /// <summary>
        /// Array that contains the number of pinFalls for each Roll.
        /// </summary>
        public int[] PinFalls = new int[21];

        /// <summary>
        /// Array that marks the Strike in the corresponding Frame.
        /// </summary>
        public int[] Strikes = new int[10];

        /// <summary>
        /// Array that marks the Spare in the corresponding Frame.
        /// </summary>
        public int[] Spares = new int[10];

        /// <summary>
        /// Rolls counter increasing by one when a Roll action is performed.
        /// </summary>
        public int RollsCounter;

        /// <summary>
        /// Read a file containing a comma separated list of rolls, taking the reference to the file as a command line argument
        /// </summary>
        /// <param name="rollsList"></param>
        /// <returns></returns>
        private static string RollsFromFile(out string[] rollsList)
        {
            var path = Console.ReadLine();
            string rollsFromFile = System.IO.File.ReadAllText(path);
            rollsList = rollsFromFile.Split(",");
            return rollsFromFile;
        }

        /// <summary>
        /// Fill the PinFalls Array for one Roll. 
        /// </summary>
        /// <param name="pins"></param>
        public void Roll(int pins)
        {
            PinFalls[RollsCounter] = pins;
            RollsCounter++;
        }

        /// <summary>
        /// Check if it is a Strike.
        /// </summary>
        /// <param name="frame"></param>
        /// <returns></returns>
        private bool IsStrike(int frame) => PinFalls[frame] == 10;

        /// <summary>
        /// Check if it is a Spare.
        /// </summary>
        /// <param name="frame"></param>
        /// <returns></returns>
        private bool IsSpare(int frame) => (PinFalls[frame] + PinFalls[frame + 1]) == 10;

        /// <summary>
        /// Calculate the Strike Bonus. 
        /// </summary>
        /// <param name="frame"></param>
        /// <returns></returns>
        private int StrikeBonus(int frame) => PinFalls[frame + 1] + PinFalls[frame + 2];

        /// <summary>
        /// Calculate the Spare Bonus. 
        /// </summary>
        /// <param name="frame"></param>
        /// <returns></returns>
        private int SpareBonus(int frame) => PinFalls[frame + 2];

        /// <summary>
        /// Calculate the total score of the game. 
        /// </summary>
        /// <returns></returns>
        public int TotalScore()
        {
            int score = 0;
            int frameNumber = 0;

            for (int frame = 0; frame < 10; frame++)
            {
                //If on his first try in the frame he knocks down all the pins, this is called a “strike”.
                if (IsStrike(frameNumber))
                {
                    score += 10 + StrikeBonus(frameNumber);
                    Strikes[frame] = 1;
                    frameNumber++;
                }//If in two tries he knocks them all down, this is called a “spare”.
                else if (IsSpare(frameNumber))
                {
                    score += 10 + SpareBonus(frameNumber);
                    Spares[frame] = 1;
                    frameNumber += 2;
                }
                else
                {
                    //If in two tries, he fails to knock them all down, his score for that frame is the total number of
                    //pins knocked down in his two tries.
                    score += PinFalls[frameNumber] + PinFalls[frameNumber + 1];
                    frameNumber += 2;
                }
            }
            return score;
        }

        /// <summary>
        /// Format the output according to the exercise specifications. 
        /// </summary>
        /// <param name="rollsFromFile"></param>
        /// <param name="spares"></param>
        /// <returns></returns>
        public string OutputFormat(string rollsFromFile, int[] spares)
        {
            // 1.-  Replace the Strikes with symbol 'X'
            var strikesAndGutterFormat = rollsFromFile.Replace("10", "X");           
            // 2.-  Replace the Gutter balls with symbol '-'
            strikesAndGutterFormat = strikesAndGutterFormat.Replace("0,","-,");

            //3.- Check the Spares Array and format the string with the '/' symbol. 
            var sparesFormat = strikesAndGutterFormat.Split(",");
            for (int s=0; s < spares.Length; s++)
            {
                if (spares[s] == 1) sparesFormat[((s+1)*2)-1] = "/";
            }

            //4-. Insert the fisrt '|' of the output. 
            sparesFormat[0] = "|" + sparesFormat[0];

            for (int s = 0; s < sparesFormat.Length; s++)
            {
                switch (s % 2)
                {
                    case 0 when (sparesFormat[s] == "X"):
                        sparesFormat[s] = sparesFormat[s] + ", " + " |";
                        sparesFormat[s + 1] = sparesFormat[s + 1] + ", ";
                        s++;
                        break;
                    case 0:
                        sparesFormat[s] = sparesFormat[s] + ", ";
                        break;
                    default:
                        sparesFormat[s] = sparesFormat[s] + "|";
                        break;
                }
            }

            string outputString = string.Empty;

            foreach (var s in sparesFormat) outputString += s;

            return outputString;
        }


        static void Main(string[] args)
        {
            Game game = new Game();

            Console.WriteLine("\n ************************************");
            Console.WriteLine("\n              BOWLING GAME           ");
            Console.WriteLine("\n ************************************");
            Console.WriteLine("\n Introduzca la ruta del archivo del juego: / Enter the path of the rolls file: ");

            // Read a file containing a comma separated list of rolls, from the command line.
            var rollsFromFile = RollsFromFile(out var rollsList);

            Console.Write("\n List of rolls of the game:");
            Console.WriteLine(rollsFromFile);

            foreach (var roll in rollsList)
            {
                game.Roll(Convert.ToInt32(roll));
            }

            //Get the total score of the game. 
            game.Score = game.TotalScore();

            // Format the output according to the exercise instructions. 
            var outputString = game.OutputFormat(rollsFromFile, game.Spares);

            Console.WriteLine("\n| f1 | f2 | f3 | f4 | f5 | f6 | f7 | f8 | f9 | f10 | ");
            Console.WriteLine(outputString);
            Console.Write("score: " + game.Score);
            Console.ReadKey();
        }
    }
}
