namespace XIV.SaveSystem
{
    public interface ISaveable
    {
        object CaptureState();
        void RestoreState(object state);
    }
}