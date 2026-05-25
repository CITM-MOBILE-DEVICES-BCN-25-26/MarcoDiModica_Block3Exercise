using System.Collections.Generic;
using NUnit.Framework;

namespace CleanRefactor.Tests
{
    public sealed class ShopUseCasesTests
    {
        [Test]
        public void When_BuyBombWithEnoughCoins_Expects_Purchased()
        {
            TestContext context = CreateContext(coins: 500);
            BuyItemResponse response = context.BuyItem.Execute("Bomb");

            Assert.IsTrue(response.Success);
            Assert.AreEqual(PurchaseStatus.Purchased, response.Status);
            Assert.AreEqual(400, response.PlayerCoins);
            Assert.AreEqual(1, response.ItemQuantity);

            Assert.AreEqual(400, context.Repository.Player.Coins);
            Assert.AreEqual(1, context.Repository.Player.Inventory.GetQuantity("Bomb"));
            Assert.AreEqual(1, context.Repository.SaveCallsCount);
        }

        [Test]
        public void When_BuyBombWithoutEnoughCoins_Expects_NotEnoughCoinsAndNoSave()
        {
            TestContext context = CreateContext(coins: 50);
            BuyItemResponse response = context.BuyItem.Execute("Bomb");

            Assert.IsFalse(response.Success);
            Assert.AreEqual(PurchaseStatus.NotEnoughCoins, response.Status);
            Assert.AreEqual(50, context.Repository.Player.Coins);
            Assert.AreEqual(0, context.Repository.SaveCallsCount);
        }

        [Test]
        public void When_BuyBombAtMaxUses_Expects_MaxUsesReachedAndNoSave()
        {
            TestContext context = CreateContext(coins: 500, bombs: 3);
            BuyItemResponse response = context.BuyItem.Execute("Bomb");

            Assert.AreEqual(PurchaseStatus.MaxUsesReached, response.Status);
            Assert.AreEqual(500, context.Repository.Player.Coins);
            Assert.AreEqual(0, context.Repository.SaveCallsCount);
        }

        [Test]
        public void When_BuyShieldWithEnoughCoins_Expects_Purchased()
        {
            TestContext context = CreateContext(coins: 500);
            BuyItemResponse response = context.BuyItem.Execute("Shield");

            Assert.IsTrue(response.Success);
            Assert.AreEqual(PurchaseStatus.Purchased, response.Status);
            Assert.AreEqual(350, response.PlayerCoins);
            Assert.AreEqual(1, context.Repository.Player.Inventory.GetQuantity("Shield"));
            Assert.AreEqual(1, context.Repository.SaveCallsCount);
        }

        [Test]
        public void When_BuyShieldAtMaxUses_Expects_MaxUsesReachedAndNoSave()
        {
            TestContext context = CreateContext(coins: 500, shields: 2);
            BuyItemResponse response = context.BuyItem.Execute("Shield");

            Assert.AreEqual(PurchaseStatus.MaxUsesReached, response.Status);
            Assert.AreEqual(0, context.Repository.SaveCallsCount);
        }

        [Test]
        public void When_BuyDoubleCoinsWithEnoughCoinsAndLevel_Expects_Purchased()
        {
            TestContext context = CreateContext(coins: 500, level: 5);
            BuyItemResponse response = context.BuyItem.Execute("DoubleCoins");

            Assert.IsTrue(response.Success);
            Assert.AreEqual(PurchaseStatus.Purchased, response.Status);
            Assert.AreEqual(200, response.PlayerCoins);
            Assert.AreEqual(1, context.Repository.Player.Inventory.GetQuantity("DoubleCoins"));
            Assert.AreEqual(1, context.Repository.SaveCallsCount);
        }

        [Test]
        public void When_BuyDoubleCoinsWithLowLevel_Expects_RequiredLevelNotReachedAndNoSave()
        {
            TestContext context = CreateContext(coins: 500, level: 4);
            BuyItemResponse response = context.BuyItem.Execute("DoubleCoins");

            Assert.AreEqual(PurchaseStatus.RequiredLevelNotReached, response.Status);
            Assert.AreEqual(0, context.Repository.SaveCallsCount);
        }

        [Test]
        public void When_BuyDoubleCoinsAlreadyOwned_Expects_AlreadyOwnedAndNoSave()
        {
            TestContext context = CreateContext(coins: 500, level: 5, hasDoubleCoins: true);
            BuyItemResponse response = context.BuyItem.Execute("DoubleCoins");

            Assert.AreEqual(PurchaseStatus.AlreadyOwned, response.Status);
            Assert.AreEqual(0, context.Repository.SaveCallsCount);
        }

        [Test]
        public void When_PurchaseSucceeds_Expects_PlayerCoinsAreUpdated()
        {
            TestContext context = CreateContext(coins: 500);
            context.BuyItem.Execute("Bomb");

            Assert.AreEqual(400, context.Repository.Player.Coins);
        }

        [Test]
        public void When_PurchaseFails_Expects_SaveIsNotCalled()
        {
            TestContext context = CreateContext(coins: 50);
            context.BuyItem.Execute("Bomb");

            Assert.AreEqual(0, context.Repository.SaveCallsCount);
        }

        private static TestContext CreateContext(
            int coins = 500,
            int level = 1,
            int bombs = 0,
            int shields = 0,
            bool hasDoubleCoins = false)
        {
            Inventory inventory = new Inventory();
            if (bombs > 0) inventory.Add("Bomb", bombs);
            if (shields > 0) inventory.Add("Shield", shields);
            if (hasDoubleCoins) inventory.Add("DoubleCoins", 1);

            Player player = new Player(coins, level, inventory);
            InMemoryPlayerRepository repository = new InMemoryPlayerRepository(player);
            ShopCatalogue catalogue = CreateCatalogue();
            ShopService shopService = new ShopService(catalogue);

            BuyItemUseCase buyItem = new BuyItemUseCase(repository, shopService);
            GetShopStatusUseCase getShopStatus = new GetShopStatusUseCase(repository, catalogue, shopService);

            return new TestContext(repository, buyItem, getShopStatus);
        }

        private static ShopCatalogue CreateCatalogue()
        {
            return new ShopCatalogue(new List<ShopItem>
            {
                new ShopItem("Bomb",        "Bomb",         100, 3, 0),
                new ShopItem("Shield",      "Shield",       150, 2, 0),
                new ShopItem("DoubleCoins", "Double Coins", 300, 1, 5)
            });
        }

        private sealed class TestContext
        {
            public InMemoryPlayerRepository Repository { get; }
            public BuyItemUseCase BuyItem { get; }
            public GetShopStatusUseCase GetShopStatus { get; }

            public TestContext(
                InMemoryPlayerRepository i_repository,
                BuyItemUseCase i_buyItem,
                GetShopStatusUseCase i_getShopStatus)
            {
                Repository = i_repository;
                BuyItem = i_buyItem;
                GetShopStatus = i_getShopStatus;
            }
        }

        private sealed class InMemoryPlayerRepository : IPlayerRepository
        {
            public Player Player { get; private set; }
            public int SaveCallsCount { get; private set; }

            public InMemoryPlayerRepository(Player i_player) => Player = i_player;

            public Player Load() => Player;

            public void Save(Player i_player)
            {
                Player = i_player;
                SaveCallsCount++;
            }
        }
    }
}
