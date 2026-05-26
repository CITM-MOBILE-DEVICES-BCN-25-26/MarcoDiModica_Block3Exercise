using System;
using System.Collections.Generic;

namespace CleanRefactor
{
    public sealed class GetShopStatusUseCase
    {
        private readonly IPlayerRepository m_playerRepository;
        private readonly ShopCatalogue m_catalogue;
        private readonly ShopService m_shopService;

        public GetShopStatusUseCase(IPlayerRepository i_playerRepository, ShopCatalogue i_catalogue, ShopService i_shopService)
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
                items.Add(new ShopItemStatus(item.Id, item.Name, item.Cost, player.Inventory.GetQuantity(item.Id), item.MaxQuantity, item.RequiredLevel, item.IsUnique, m_shopService.CanBuy(player, item)));
            }

            return new ShopStatusResponse(player.Coins, player.Level, items);
        }
    }
}
