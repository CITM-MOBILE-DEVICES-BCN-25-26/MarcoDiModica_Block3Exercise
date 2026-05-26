using System;

namespace CleanRefactor
{
    public sealed class AddCoinsUseCase
    {
        private readonly IPlayerRepository m_playerRepository;
        private readonly int m_amount;

        public AddCoinsUseCase(IPlayerRepository i_playerRepository, int i_amount)
        {
            if (i_amount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(i_amount), "Amount must be greater than zero.");
            }

            m_playerRepository = i_playerRepository ?? throw new ArgumentNullException(nameof(i_playerRepository));
            m_amount = i_amount;
        }

        public void Execute()
        {
            Player player = m_playerRepository.Load();
            player.AddCoins(m_amount);
            m_playerRepository.Save(player);
        }
    }
}
