using System;

namespace TricomBowling.CoreVS2017
{
    public class Game
    {
        int _score = 0;
        int[] pinFalls = new int[21];
        int rollsCounter;

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Console.ReadKey();
        }

        public void Roll(int pins)
        {
            //_score += pins;
            pinFalls[rollsCounter] = pins;
            rollsCounter++;
        }

        public int Score()
        {
            int score = 0;
            int i = 0;

            for (int frame = 0; frame < 10; frame++)
            {
                if (pinFalls[i] + pinFalls[i + 1] == 10)
                {
                    score += 10 + pinFalls[i + 2];
                    i++;
                }
                else
                {
                    score += pinFalls[i] + pinFalls[i+1];
                    i++;
                }
            }
            return score;
        }
    }
}
