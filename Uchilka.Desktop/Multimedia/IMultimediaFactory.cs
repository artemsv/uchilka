namespace Uchilka.Multimedia
{
    public interface IPlayer
    {
        void Play(string path);
    }

    public interface IMultimediaFactory
    {
        IPlayer GetPlayer();
    }
}
