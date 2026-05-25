namespace CleanRefactor
{
    public sealed class ShopItemStatus
    {
        public string ItemId { get; }
        public string Name { get; }
        public int Cost { get; }
        public int CurrentQuantity { get; }
        public int MaxQuantity { get; }
        public int RequiredLevel { get; }
        public bool IsUnique { get; }
        public bool CanBuy { get; }

        public ShopItemStatus(
            string i_itemId,
            string i_name,
            int i_cost,
            int i_currentQuantity,
            int i_maxQuantity,
            int i_requiredLevel,
            bool i_isUnique,
            bool i_canBuy)
        {
            ItemId = i_itemId;
            Name = i_name;
            Cost = i_cost;
            CurrentQuantity = i_currentQuantity;
            MaxQuantity = i_maxQuantity;
            RequiredLevel = i_requiredLevel;
            IsUnique = i_isUnique;
            CanBuy = i_canBuy;
        }
    }
}
