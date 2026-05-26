namespace CleanRefactor
{
    public sealed class BuyItemResponse
    {
        public PurchaseStatus Status { get; }
        public bool Success => Status == PurchaseStatus.Purchased;

        public string ItemId { get; }
        public string ItemName { get; }
        public int PlayerCoins { get; }
        public int ItemQuantity { get; }

        public BuyItemResponse(PurchaseStatus i_status, string i_itemId, string i_itemName, int i_playerCoins, int i_itemQuantity)
        {
            Status = i_status;
            ItemId = i_itemId;
            ItemName = i_itemName;
            PlayerCoins = i_playerCoins;
            ItemQuantity = i_itemQuantity;
        }
    }
}
