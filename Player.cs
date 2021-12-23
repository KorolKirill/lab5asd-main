using System;
using System.Collections.Generic;
using System.Linq;

namespace lab5asd
{
    public class Player
    {
        public Dice[] dices;
        public int score;
        public string name;
        public Player()
        {
            score = 0;
            dices = new[] {new Dice(), new Dice(), new Dice(), new Dice(), new Dice()};
        }
        public void RollAllDice()
        {
            foreach (var dice in dices)
            {
                dice.Roll();
            }

            Console.WriteLine("You have rolled");
            ShowAllDices();
        }

        protected void ShowAllDices()
        {
            //Console.WriteLine("Your dices:");
            foreach (var dice in dices)
            {
                Console.Write(dice.value+ " ");
            }
        }

        public void AddScore(Game.WinCombinations combinations)
        {
            score += (int) combinations;
        }

        public virtual void WhatToReroll()
        {
            Console.WriteLine("Your current situation");
            ShowAllDices();
            Console.WriteLine("What dices or dice you want to reroll?");
            foreach (var index in AskForIndexesOfDices())
            {
                dices[index].Roll();
            }
            Console.WriteLine("You rolled: ");
            ShowAllDices();
            ShowScore();
            if (Game.CheckCombination(dices) == Game.WinCombinations.None)
            {
                WhatToScore();
                return;
            }
            
            score += (int) Game.CheckCombination(dices);
            ShowScore();
        }

        public virtual void WhatToScore()
        {
            Console.WriteLine("what dice do you want to add to score?");
            var a = int.Parse(Console.ReadLine());
            score += dices.Count(x => x.value == a) * a;
            ShowScore();
        }

        public void ShowScore()
        {
            Console.WriteLine("Your current score: " + score);
        }
         
        private IEnumerable<int> AskForIndexesOfDices() {
            Console.WriteLine("pls write index or indexes of dices");
            var a = Console.ReadLine();
            var indexes = a.Split(',');
            for (int i = 0; i < indexes.Length; i++)
            {
                yield return int.Parse(indexes[i]);
            }
        }
    }
}