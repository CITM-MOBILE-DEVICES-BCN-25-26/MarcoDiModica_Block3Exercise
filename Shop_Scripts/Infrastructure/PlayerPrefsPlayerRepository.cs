using System;
using UnityEngine;

namespace CleanRefactor
{
    public sealed class PlayerPrefsPlayerRepository : IPlayerRepository
    {
        private const string KEY_COINS = "Coins";
        private const string KEY_LEVEL = "PlayerLevel";
        private const string KEY_INVENTORY_PREFIX = "Inventory_";

        private readonly ShopCatalogue m_catalogue;
        private readonly int m_defaultCoins;
        private readonly int m_defaultLevel;

        public PlayerPrefsPlayerRepository(ShopCatalogue i_catalogue, int i_defaultCoins, int i_defaultLevel)
        {
            m_catalogue = i_catalogue ?? throw new ArgumentNullException(nameof(i_catalogue));
            m_defaultCoins = i_defaultCoins;
            m_defaultLevel = i_defaultLevel;
        }

        public Player Load()
        {
            int coins = PlayerPrefs.GetInt(KEY_COINS, m_defaultCoins);
            int level = PlayerPrefs.GetInt(KEY_LEVEL, m_defaultLevel);

            Inventory inventory = new Inventory();

            foreach (ShopItem item in m_catalogue.GetAll())
            {
                int quantity = PlayerPrefs.GetInt(KEY_INVENTORY_PREFIX + item.Id, 0);
                if (quantity > 0)
                {
                    inventory.Add(item.Id, quantity);
                }
            }

            return new Player(coins, level, inventory);
        }

        public void Save(Player i_player)
        {
            if (i_player == null)
            {
                throw new ArgumentNullException(nameof(i_player));
            }

            PlayerPrefs.SetInt(KEY_COINS, i_player.Coins);
            PlayerPrefs.SetInt(KEY_LEVEL, i_player.Level);

            foreach (ShopItem item in m_catalogue.GetAll())
            {
                int quantity = i_player.Inventory.GetQuantity(item.Id);
                PlayerPrefs.SetInt(KEY_INVENTORY_PREFIX + item.Id, quantity);
            }

            PlayerPrefs.Save();
        }
    }
}
