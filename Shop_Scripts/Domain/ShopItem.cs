using System;

namespace CleanRefactor
{
    public sealed class ShopItem
    {
        public string Id { get; }
        public string Name { get; }
        public int Cost { get; }
        public int MaxQuantity { get; }
        public int RequiredLevel { get; }
        public bool IsUnique => MaxQuantity == 1;

        public ShopItem(string i_id, string i_name, int i_cost, int i_maxQuantity, int i_requiredLevel)
        {
            if (string.IsNullOrEmpty(i_id))
                throw new ArgumentException("Id cannot be null or empty.", nameof(i_id));

            if (string.IsNullOrEmpty(i_name))
                throw new ArgumentException("Name cannot be null or empty.", nameof(i_name));

            if (i_cost <= 0)
                throw new ArgumentOutOfRangeException(nameof(i_cost), "Cost must be greater than zero.");

            if (i_maxQuantity <= 0)
                throw new ArgumentOutOfRangeException(nameof(i_maxQuantity), "MaxQuantity must be greater than zero.");

            if (i_requiredLevel < 0)
                throw new ArgumentOutOfRangeException(nameof(i_requiredLevel), "RequiredLevel cannot be negative.");

            Id = i_id;
            Name = i_name;
            Cost = i_cost;
            MaxQuantity = i_maxQuantity;
            RequiredLevel = i_requiredLevel;
        }
    }
}
