using System.Collections.Generic;
using UnityEngine;

namespace CleanRefactor
{
    public sealed class ShopBootstrap : MonoBehaviour
    {
        [Header("View")]
        [SerializeField] private ShopView m_shopView;

        [Header("Bomb")]
        [SerializeField] private int m_bombCost = 100;
        [SerializeField] private int m_bombMaxUses = 3;

        [Header("Shield")]
        [SerializeField] private int m_shieldCost = 150;
        [SerializeField] private int m_shieldMaxUses = 2;

        [Header("Double Coins")]
        [SerializeField] private int m_doubleCoinsCost = 300;
        [SerializeField] private int m_doubleCoinsRequiredLevel = 5;

        [Header("Player Defaults")]
        [SerializeField] private int m_initialCoins = 500;
        [SerializeField] private int m_initialLevel = 1;

        [Header("Debug")]
        [SerializeField] private int m_addCoinsAmount = 500;

        private ShopPresenter m_presenter;

        private void Awake()
        {
            ShopCatalogue catalogue = CreateCatalogue();
            ShopService shopService = new ShopService(catalogue);
            IPlayerRepository repository = new PlayerPrefsPlayerRepository(catalogue, m_initialCoins, m_initialLevel);

            GetShopStatusUseCase getShopStatus = new GetShopStatusUseCase(repository, catalogue, shopService);
            BuyItemUseCase buyItem = new BuyItemUseCase(repository, shopService);
            AddCoinsUseCase addCoins = new AddCoinsUseCase(repository, m_addCoinsAmount);
            LevelUpUseCase levelUp = new LevelUpUseCase(repository);
            ResetPlayerUseCase resetPlayer = new ResetPlayerUseCase(repository, m_initialCoins, m_initialLevel);

            m_presenter = new ShopPresenter(m_shopView, getShopStatus, buyItem, addCoins, levelUp, resetPlayer);
            m_presenter.Initialize();
        }

        private void OnDestroy()
        {
            m_presenter?.Dispose();
        }

        private ShopCatalogue CreateCatalogue()
        {
            return new ShopCatalogue(new List<ShopItem>
            {
                new ShopItem(ShopItemKey.Bomb.ToString(),        "Bomb",         m_bombCost,        m_bombMaxUses,    0),
                new ShopItem(ShopItemKey.Shield.ToString(),      "Shield",       m_shieldCost,      m_shieldMaxUses,  0),
                new ShopItem(ShopItemKey.DoubleCoins.ToString(), "Double Coins", m_doubleCoinsCost, 1,                m_doubleCoinsRequiredLevel)
            });
        }
    }
}
