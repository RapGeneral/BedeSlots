using BedeSlots.ViewModels.Enums;
using System.Collections.Generic;

namespace BedeSlots.Services.Contracts
{
    public interface ISlotGamesServices
    {
        List<List<GameItemChanceOutOf100>> Run(int n, int m);
        decimal Evaluate(List<List<GameItemChanceOutOf100>> slotMatrix);
    }
}
