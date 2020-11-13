using NUnit.Framework;
using TricomBowling.CoreVS2017;

namespace TricomBowling.CoreVS2017_Test
{
    public class Tests
    {
        Game game;
        
        [SetUp]
        public void Setup()
        {
            game = new Game();
        }

        void RollMany(int rolls, int pins)
        {
            for (int i = 0; i < rolls; i++)
            {
                game.Roll(pins);
            }

        }

        [Test]
        public void RollGame()
        {
            RollMany(20, 0);
            Assert.That(game.Score(), Is.EqualTo(0));
        }

        [Test]
        public void RollOnes()
        {
            RollMany(20, 1);
            Assert.That(game.Score(), Is.EqualTo(20));
        }

        [Test]
        public void RollSpareFirstFrame()
        {
            game.Roll(9);
            game.Roll(1);
            RollMany(18, 1);

            Assert.That(game.Score(), Is.EqualTo(29));
        }

        [Test]
        public void RollSparesAllFrame()
        {
            RollMany(21, 5);

            Assert.That(game.Score(), Is.EqualTo(150));
        }
    }
}