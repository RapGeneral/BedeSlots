using BedeSlots.Services.Contracts;
using BedeSlots.ViewModels.Enums;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BedeSlots.Services
{
    public class SlotGamesServices : ISlotGamesServices
    {
        private readonly IMemoryCache cache;

        public SlotGamesServices(IMemoryCache cache)
        {
            this.cache = cache;
        }

        public decimal Evaluate(List<List<GameItemChanceOutOf100>> slotMatrix)
        {
            if(slotMatrix.Count < 3 || slotMatrix[0].Count < 3)
            {
                throw new ArgumentException("Slot matrix's dimentions should be atleast 3x3!");
            }
            var wildCard = GetEnumValuesCached().First();
            decimal addedCoefs = 0;
            for (int i = 0; i < slotMatrix.Count; i++)
            {
                bool rowJackpot = true;

                GameItemChanceOutOf100 rowUniqueItem = wildCard;
                for (int j = 0; j < slotMatrix[i].Count; j++)
                {
                    if(rowUniqueItem == wildCard && slotMatrix[i][j] != wildCard)
                    {
                        rowUniqueItem = slotMatrix[i][j];
                    }
                    if(slotMatrix[i][j] != rowUniqueItem && slotMatrix[i][j] != wildCard)
                    {
                        rowJackpot = false;
                        break;
                    }
                }
                if (rowJackpot)
                {
                    addedCoefs += CalculateJackpotRowCoeff(slotMatrix[i]);
                }
            }
            return addedCoefs;
        }

        private decimal CalculateJackpotRowCoeff(List<GameItemChanceOutOf100> winningRow)
        {
            decimal rowCoeff = 0;
            foreach(var item in winningRow)
            {
                rowCoeff += GetCoefficient(item);
            }
            return rowCoeff;
        }

        public List<List<GameItemChanceOutOf100>> Run(int n, int m)
        {
            if(n < 3 || m < 3)
            {
                throw new ArgumentException("Slot matrix's dimentions should be atleast 3x3!");
            }
            var slotMatrix = new List<List<GameItemChanceOutOf100>>();
            var enumValues = GetEnumValuesCached();
            //The chance margine represents a number from 0 to 100.
            //If a number is bigger than it and generated from 0 to 100, it has happend at %of number that the margin represents.
            int[] chanceMargins = cache.GetOrCreate("ChanceMargin", entry => 
            {
                entry.SlidingExpiration = TimeSpan.FromHours(1);
                int[] newChanceMargins = new int[enumValues.Length];
                newChanceMargins[0] = (int)enumValues[0];
                for (int i = 1; i < enumValues.Length; i++)
                {
                    newChanceMargins[i] = newChanceMargins[i - 1] + (int)enumValues[i];
                }
                return newChanceMargins;
            });
            Random rand = new Random();

            for (int i = 0; i < n; i++)
            {
                slotMatrix.Add(new List<GameItemChanceOutOf100>());
                for (int j = 0; j < m; j++)
                {
                    int num = rand.Next(1, 100);
                    for (int s = 0; s < chanceMargins.Length; s++)
                    {
                        if (num <= chanceMargins[s])
                        {
                            slotMatrix[i].Add(enumValues[s]);
                            break;
                        }
                    }
                }
            }

            return slotMatrix;
        }
        private decimal GetCoefficient(GameItemChanceOutOf100 item)
        {
            var cacheChances = cache.GetOrCreate("GameRowWinCoefficients", entry =>
            {
                var coeffsAsJson = File.ReadAllText(@"..\..\..\..\BedeSlots.Services\GameCoefficients.json");
                entry.SlidingExpiration = TimeSpan.FromHours(1);
                return JsonConvert.DeserializeObject<Dictionary<string, decimal>>(coeffsAsJson);
            });
            return cacheChances[item.ToString()];
        }
        private GameItemChanceOutOf100[] GetEnumValuesCached()
        {
            return cache.GetOrCreate("EnumValues", entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromHours(1);
                return Enum.GetValues(typeof(GameItemChanceOutOf100)).Cast<GameItemChanceOutOf100>().ToArray();
            });
        }
    }
}
