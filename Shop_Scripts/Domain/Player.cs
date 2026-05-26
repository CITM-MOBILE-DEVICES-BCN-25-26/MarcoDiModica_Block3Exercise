using System;

namespace CleanRefactor
{
    public sealed class Player
    {
        public int Coins { get; private set; }
        public int Level { get; private set; }
        public Inventory Inventory { get; }

        public Player(int i_coins, int i_level, Inventory i_inventory)
        {
            if (i_coins < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(i_coins), "Coins cannot be negative.");
            }

            if (i_level < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(i_level), "Level must be at least 1.");
            }

            Coins = i_coins;
            Level = i_level;
            Inventory = i_inventory ?? throw new ArgumentNullException(nameof(i_inventory));
        }

        public bool CanAfford(int i_amount) => Coins >= i_amount;

        public void Spend(int i_amount)
        {
            if (i_amount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(i_amount), "Amount must be greater than zero.");
            }

            if (!CanAfford(i_amount))
            {
                throw new InvalidOperationException("Player cannot afford this amount.");
            }

            Coins -= i_amount;
        }

        public void AddCoins(int i_amount)
        {
            if (i_amount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(i_amount), "Amount must be greater than zero.");
            }

            Coins += i_amount;
        }

        public void GainLevel()
        {
            Level++;
        }
    }
}
