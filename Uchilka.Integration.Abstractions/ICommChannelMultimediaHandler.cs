namespace Uchilka.Integration.Abstractions
{
    public interface ICommChannelMultimediaHandler : ICommChannelHandler
    {
        void HandleVoice(string path);
        void HandlePicture(string path);
        void HandleVideo(string path);
        void HandleAudio(string path);
    }
}
