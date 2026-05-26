namespace CleanRefactor
{
    public interface IPlayerRepository
    {
        Player Load();
        void Save(Player i_player);
    }
}
