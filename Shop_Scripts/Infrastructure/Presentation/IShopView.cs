using System;

namespace CleanRefactor
{
    public interface IShopView
    {
        event Action<string> BuyItemClicked;
        event Action AddCoinsClicked;
        event Action LevelUpClicked;
        event Action ResetClicked;

        void SetCoins(int i_coins);
        void SetLevel(int i_level);
        void SetFeedback(string i_message);

        void SetItemStatus(string i_itemId, string i_name, int i_cost, int i_currentQuantity, int i_maxQuantity, bool i_isUnique, bool i_canBuy);

        void PlayPurchaseSound();
        void PlayFailSound();
    }
}
