using System;

namespace CleanRefactor
{
    public sealed class LevelUpUseCase
    {
        private readonly IPlayerRepository m_playerRepository;

        public LevelUpUseCase(IPlayerRepository i_playerRepository)
        {
            m_playerRepository = i_playerRepository ?? throw new ArgumentNullException(nameof(i_playerRepository));
        }

        public void Execute()
        {
            Player player = m_playerRepository.Load();
            player.GainLevel();
            m_playerRepository.Save(player);
        }
    }
}
