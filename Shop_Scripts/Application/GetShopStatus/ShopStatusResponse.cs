using System.Collections.Generic;

namespace CleanRefactor
{
    public sealed class ShopStatusResponse
    {
        public int Coins { get; }
        public int Level { get; }
        public IReadOnlyList<ShopItemStatus> Items { get; }

        public ShopStatusResponse(int i_coins, int i_level, IReadOnlyList<ShopItemStatus> i_items)
        {
            Coins = i_coins;
            Level = i_level;
            Items = i_items;
        }
    }
}
