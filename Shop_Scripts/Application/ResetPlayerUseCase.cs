using System;

namespace CleanRefactor
{
    public sealed class ResetPlayerUseCase
    {
        private readonly IPlayerRepository m_playerRepository;
        private readonly int m_initialCoins;
        private readonly int m_initialLevel;

        public ResetPlayerUseCase(IPlayerRepository i_playerRepository, int i_initialCoins, int i_initialLevel)
        {
            m_playerRepository = i_playerRepository ?? throw new ArgumentNullException(nameof(i_playerRepository));
            m_initialCoins = i_initialCoins;
            m_initialLevel = i_initialLevel;
        }

        public void Execute()
        {
            Player freshPlayer = new Player(m_initialCoins, m_initialLevel, new Inventory());
            m_playerRepository.Save(freshPlayer);
        }
    }
}
