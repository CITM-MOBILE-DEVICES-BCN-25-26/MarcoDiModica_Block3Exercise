using System;

namespace CleanRefactor
{
    public sealed class ShopService
    {
        private readonly ShopCatalogue m_catalogue;

        public ShopService(ShopCatalogue i_catalogue)
        {
            m_catalogue = i_catalogue ?? throw new ArgumentNullException(nameof(i_catalogue));
        }

        public BuyItemResponse Buy(Player i_player, string i_itemId)
        {
            if (i_player == null)
                throw new ArgumentNullException(nameof(i_player));

            ShopItem item = m_catalogue.GetById(i_itemId);
            int currentQuantity = i_player.Inventory.GetQuantity(item.Id);

            if (!i_player.CanAfford(item.Cost))
                return BuildResponse(item, i_player, PurchaseStatus.NotEnoughCoins);

            if (item.RequiredLevel > 0 && i_player.Level < item.RequiredLevel)
                return BuildResponse(item, i_player, PurchaseStatus.RequiredLevelNotReached);

            if (currentQuantity >= item.MaxQuantity)
            {
                PurchaseStatus reason = item.IsUnique
                    ? PurchaseStatus.AlreadyOwned
                    : PurchaseStatus.MaxUsesReached;
                return BuildResponse(item, i_player, reason);
            }

            i_player.Spend(item.Cost);
            i_player.Inventory.Add(item.Id);

            return BuildResponse(item, i_player, PurchaseStatus.Purchased);
        }

        public bool CanBuy(Player i_player, ShopItem i_item)
        {
            if (i_player == null)
                throw new ArgumentNullException(nameof(i_player));

            if (i_item == null)
                throw new ArgumentNullException(nameof(i_item));

            int currentQuantity = i_player.Inventory.GetQuantity(i_item.Id);

            return i_player.CanAfford(i_item.Cost)
                && (i_item.RequiredLevel <= 0 || i_player.Level >= i_item.RequiredLevel)
                && currentQuantity < i_item.MaxQuantity;
        }

        private static BuyItemResponse BuildResponse(ShopItem i_item, Player i_player, PurchaseStatus i_status)
        {
            return new BuyItemResponse(
                i_status,
                i_item.Id,
                i_item.Name,
                i_player.Coins,
                i_player.Inventory.GetQuantity(i_item.Id));
        }
    }
}
