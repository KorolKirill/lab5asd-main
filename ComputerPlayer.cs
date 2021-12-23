using System;
using System.Collections.Generic;
using System.Linq;
using lab5asd;

public class ComputerPlayer : Player
    {
        public override void WhatToReroll()
        {
            Console.WriteLine("Your current situation");
            ShowAllDices();
            var prevscore = CalculateScrore(dices);
            var indexes = Algo();
            foreach (var ind in indexes)
            {
                dices[ind].Roll();
            }

            var score = CalculateScrore(dices);
        //    this.score += base.score;            
            ShowScore();
            WhatToScore();
        }

        private double CalculateScrore(Dice[] dices)
        {
            var scrore = Game.CheckCombination(dices);
            if (scrore == Game.WinCombinations.None)
            {
                var groups =   dices.GroupBy(x => x.value);
                int a = 1;
                foreach (var gGroup in groups )
                {
                    if (gGroup.Count()*gGroup.Key>a)
                    {
                        a = gGroup.Count()*gGroup.Key;
                    }
                }
                    
                return a;
            }

            return (double) score;
        }

        private class Node
        {
            public Dice[] dices;
            public double score;
            public double chance;
            public string turn;

            public int CalculateScroreForNode()
            {
                var scrore = Game.CheckCombination(dices);
                if (scrore == Game.WinCombinations.None)
                {
                    var groups =   dices.GroupBy(x => x.value);
                    int a = 1;
                    foreach (var gGroup in groups )
                    {
                        if (gGroup.Count()*gGroup.Key>a)
                        {
                            a = gGroup.Count()*gGroup.Key;
                        }
                    }
                    
                    return a;
                }
                return (int) score;
            }
        }

        private Dice[] CopyDiceArr(Dice[] dices)
        {
            var diceList = new List<Dice>();
            foreach (var dice in dices)
            {
                diceList.Add( (Dice) dice.Clone() );
            }

            return diceList.ToArray();
        }
        
        public int[] Algo()
        {
            var listNode = new List<Node>();
            var maxCombinations = 3;
            for (int i = 1; i <= maxCombinations ; i++)
            {
                foreach (var combination in GetAllCombinations(i))
                {
                    listNode.AddRange( Generate(combination) );
                }
            }
            
           var groupedlist = listNode.GroupBy(x => x.turn).ToArray();
           var a = groupedlist.Average(x => x.Average(x => x.score));

           
           
           var previousScore = CalculateScrore(dices);
            double bestAvScore = previousScore+0.1;
            double BestchanceToROll = int.MaxValue;
            string turnIndexes = null;

            foreach (var group in groupedlist)
            {
     
                
                /*
                    // Наиболее нерескуемый вариант, отталкиваемся еще от шанса выпадения.
                     // bestCurrentChance <= BestchanceToROll &&  bestAvScore < averageScore
                    var afterAverageBelowDelete =
                        group.Where(x => x.score > bestAvScore ).ToArray();
                    if (afterAverageBelowDelete.Count() == 0)
                    {
                        continue;
                    }
                    var bestCurrentChance = afterAverageBelowDelete.First().chance * afterAverageBelowDelete.Count();
                    var averageScore = group.Average(x => x.score);
                    if (bestAvScore < averageScore)
                    {
                        bestAvScore = averageScore;
                        BestchanceToROll = bestCurrentChance;
                        turnIndexes = group.Key;
                    }*/
          
                    
                 // Cредний риск , смотрим только на среднее значение.
                //  bestAvScore < averageScore
                var afterAverageBelowDelete =
                    group.Where(x => x.score > bestAvScore ).ToArray();
                if (afterAverageBelowDelete.Count() == 0)
                {
                    continue;
                }
                var bestCurrentChance = afterAverageBelowDelete.First().chance * afterAverageBelowDelete.Count();
                var averageScore = group.Average(x => x.score);


                if (bestAvScore < averageScore)
                {
                    bestAvScore = averageScore;
                    BestchanceToROll = bestCurrentChance;
                    turnIndexes = group.Key;
                }

                
                    /*
                    // Максимальный риск , смотрим только на макс значение.
                    //  bestAvScore < MaxScore
                    var afterAverageBelowDelete =
                        group.Where(x => x.score >= bestAvScore ).ToArray();
                    if (afterAverageBelowDelete.Count() == 0)
                    {
                        continue;
                    }
                    var bestCurrentChance = afterAverageBelowDelete.First().chance * afterAverageBelowDelete.Count();
                    var MaxScore = afterAverageBelowDelete.Average(x=>x.score);
                    
                    if (bestAvScore <= MaxScore)
                    {
                        bestAvScore = MaxScore;
                        BestchanceToROll = bestCurrentChance;
                        turnIndexes = group.Key;
                    }
                    */

            }

            if (turnIndexes == null)
            {
                return new int[] {};
            }

            return GetIndexesFromString(turnIndexes);
        }

        private string[] GetAllCombinations(int numbers)
        {
            if (numbers ==1)
            {
                return  new string[] {"0", "1", "2", "3", "4",};
            }
            var a = new string[] {"0", "1", "2"};

            
            var list22 = new List<string>();
            foreach (var ind in a)
            {
                for (int i = 0; i < 5; i++)
                {
                    if (ind.Contains(i + ""))
                    {
                        continue;
                    }

                    list22.Add(ind + " " + i);
                }
            }

            if (numbers == 2)
            {
                return list22.ToArray();
            }
            

            var list3 = new List<string>();;
            foreach (var ind in list22)
            {
                for (int i = 0; i < 5; i++)
                {
                    if (ind.Contains(i + ""))
                    {
                        continue;
                    }
                    list3.Add( ind + " " + i );
                }
            }

            return list3.ToArray();

        }

        private int[] GetIndexesFromString(string indexes)
        {
            var list = new List<int>();
            var a = indexes.Split(' ');
            foreach (var index in a)
            {
                list.Add( int.Parse(index));    
            }

            return list.ToArray();
        }
        
        private Node[] Generate( string indexes )
        {
            var listOneIndex = new List<Node>();
            var indexesInt = GetIndexesFromString(indexes);
            
            // первый потомок
            for (int i = 1; i < 7; i++)
            {
                var node = new Node();
                node.chance = 1 / Math.Pow(6,indexesInt.Count());
         
                node.dices = CopyDiceArr(dices);
                node.dices[indexesInt[0]].value = i;
                node.turn = indexes;
                node.score = node.CalculateScroreForNode();
                listOneIndex.Add(node);
            }

            if (indexesInt.Length == 1)
            {
                return listOneIndex.ToArray();
            }
            
            var list2 = new List<Node>();

            foreach (var node1 in listOneIndex)
            {
                for (int i = 1; i < 7; i++)
                {
                    var node = new Node();
                    node.chance = 1 / Math.Pow(6, indexesInt.Count());
                    node.dices = CopyDiceArr(node1.dices) ;
                    node.dices[indexesInt[1]].value = i;
                    node.turn = indexes;
                    node.score = node.CalculateScroreForNode();
                    list2.Add(node);
                }

            }

            if (indexesInt.Length == 2)
            {
                return list2.ToArray();
            }
            
            var list3 = new List<Node>();

            foreach (var node1 in list2)
            {
                for (int i = 1; i < 7; i++)
                {
                    var node = new Node();
                    node.chance = 1 / Math.Pow(6, indexesInt.Count());
                    node.dices = CopyDiceArr(node1.dices) ;
                    node.dices[indexesInt[1]].value = i;
                    node.turn = indexes;
                    node.score = node.CalculateScroreForNode();
                    list3.Add(node);
                }

            }
            return list3.ToArray();
        }
        


        public override void WhatToScore()
        {
            
            // Берет кубик, который даст больше всего очков.
            Console.WriteLine("what dice do you want to add to score?");
            ShowAllDices();
           var groups =   dices.GroupBy(x => x.value);
           int a = 0;
            foreach (var gGroup in groups )
            {
                if (gGroup.Count()*gGroup.Key>a)
                {
                    a = gGroup.Count()*gGroup.Key;
                }
            }
            score += a;
            ShowScore();
        }
    }
