using System;

namespace CleanRefactor
{
    public sealed class BuyItemUseCase
    {
        private readonly IPlayerRepository m_playerRepository;
        private readonly ShopService m_shopService;

        public BuyItemUseCase(IPlayerRepository i_playerRepository, ShopService i_shopService)
        {
            m_playerRepository = i_playerRepository ?? throw new ArgumentNullException(nameof(i_playerRepository));
            m_shopService = i_shopService ?? throw new ArgumentNullException(nameof(i_shopService));
        }

        public BuyItemResponse Execute(string i_itemId)
        {
            Player player = m_playerRepository.Load();
            BuyItemResponse response = m_shopService.Buy(player, i_itemId);

            if (response.Success)
            {
                m_playerRepository.Save(player);
            }

            return response;
        }
    }
}
