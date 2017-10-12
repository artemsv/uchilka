namespace Uchilka.Integration.Abstractions
{
    public interface ICommChannelMultimediaHandler : ICommChannelHandler
    {
        void HandleVoice(string path);
        void HandlePhoto(string path);
    }
}
