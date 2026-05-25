using System;
using System.Collections.Generic;

namespace CleanRefactor
{
    public sealed class GetShopStatusUseCase
    {
        private readonly IPlayerRepository m_playerRepository;
        private readonly ShopCatalogue m_catalogue;
        private readonly ShopService m_shopService;

        public GetShopStatusUseCase(
            IPlayerRepository i_playerRepository,
            ShopCatalogue i_catalogue,
            ShopService i_shopService)
        {
            m_playerRepository = i_playerRepository ?? throw new ArgumentNullException(nameof(i_playerRepository));
            m_catalogue = i_catalogue ?? throw new ArgumentNullException(nameof(i_catalogue));
            m_shopService = i_shopService ?? throw new ArgumentNullException(nameof(i_shopService));
        }

        public ShopStatusResponse Execute()
        {
            Player player = m_playerRepository.Load();
            List<ShopItemStatus> items = new List<ShopItemStatus>();

            foreach (ShopItem item in m_catalogue.GetAll())
            {
                items.Add(new ShopItemStatus(
                    i_itemId: item.Id,
                    i_name: item.Name,
                    i_cost: item.Cost,
                    i_currentQuantity: player.Inventory.GetQuantity(item.Id),
                    i_maxQuantity: item.MaxQuantity,
                    i_requiredLevel: item.RequiredLevel,
                    i_isUnique: item.IsUnique,
                    i_canBuy: m_shopService.CanBuy(player, item)));
            }

            return new ShopStatusResponse(player.Coins, player.Level, items);
        }
    }
}
