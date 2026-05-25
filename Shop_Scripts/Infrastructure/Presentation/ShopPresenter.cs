using System;

namespace CleanRefactor
{
    public sealed class ShopPresenter : IDisposable
    {
        private readonly IShopView m_view;
        private readonly GetShopStatusUseCase m_getShopStatus;
        private readonly BuyItemUseCase m_buyItem;
        private readonly AddCoinsUseCase m_addCoins;
        private readonly LevelUpUseCase m_levelUp;
        private readonly ResetPlayerUseCase m_resetPlayer;

        public ShopPresenter(
            IShopView i_view,
            GetShopStatusUseCase i_getShopStatus,
            BuyItemUseCase i_buyItem,
            AddCoinsUseCase i_addCoins,
            LevelUpUseCase i_levelUp,
            ResetPlayerUseCase i_resetPlayer)
        {
            m_view = i_view ?? throw new ArgumentNullException(nameof(i_view));
            m_getShopStatus = i_getShopStatus ?? throw new ArgumentNullException(nameof(i_getShopStatus));
            m_buyItem = i_buyItem ?? throw new ArgumentNullException(nameof(i_buyItem));
            m_addCoins = i_addCoins ?? throw new ArgumentNullException(nameof(i_addCoins));
            m_levelUp = i_levelUp ?? throw new ArgumentNullException(nameof(i_levelUp));
            m_resetPlayer = i_resetPlayer ?? throw new ArgumentNullException(nameof(i_resetPlayer));
        }

        public void Initialize()
        {
            m_view.BuyItemClicked += OnBuyItemClicked;
            m_view.AddCoinsClicked += OnAddCoinsClicked;
            m_view.LevelUpClicked += OnLevelUpClicked;
            m_view.ResetClicked += OnResetClicked;

            RefreshStatus();
            m_view.SetFeedback("Select an item to buy.");
        }

        public void Dispose()
        {
            m_view.BuyItemClicked -= OnBuyItemClicked;
            m_view.AddCoinsClicked -= OnAddCoinsClicked;
            m_view.LevelUpClicked -= OnLevelUpClicked;
            m_view.ResetClicked -= OnResetClicked;
        }

        private void RefreshStatus()
        {
            ShopStatusResponse response = m_getShopStatus.Execute();

            m_view.SetCoins(response.Coins);
            m_view.SetLevel(response.Level);

            foreach (ShopItemStatus item in response.Items)
            {
                m_view.SetItemStatus(
                    item.ItemId,
                    item.Name,
                    item.Cost,
                    item.CurrentQuantity,
                    item.MaxQuantity,
                    item.IsUnique,
                    item.CanBuy);
            }
        }

        private void OnBuyItemClicked(string i_itemId)
        {
            BuyItemResponse response = m_buyItem.Execute(i_itemId);
            m_view.SetFeedback(GetFeedbackFor(response));

            if (response.Success)
                m_view.PlayPurchaseSound();
            else
                m_view.PlayFailSound();

            RefreshStatus();
        }

        private void OnAddCoinsClicked()
        {
            m_addCoins.Execute();
            m_view.SetFeedback("Coins added.");
            RefreshStatus();
        }

        private void OnLevelUpClicked()
        {
            m_levelUp.Execute();
            m_view.SetFeedback("Level up!");
            RefreshStatus();
        }

        private void OnResetClicked()
        {
            m_resetPlayer.Execute();
            m_view.SetFeedback("Player reset.");
            RefreshStatus();
        }

        private static string GetFeedbackFor(BuyItemResponse i_response)
        {
            switch (i_response.Status)
            {
                case PurchaseStatus.Purchased:
                    return i_response.ItemName + " purchased!";
                case PurchaseStatus.NotEnoughCoins:
                    return "Not enough coins for " + i_response.ItemName;
                case PurchaseStatus.MaxUsesReached:
                    return i_response.ItemName + " already at max uses";
                case PurchaseStatus.RequiredLevelNotReached:
                    return "Required level not reached for " + i_response.ItemName;
                case PurchaseStatus.AlreadyOwned:
                    return i_response.ItemName + " already owned";
                default:
                    throw new ArgumentOutOfRangeException(nameof(i_response.Status), i_response.Status, null);
            }
        }
    }
}
