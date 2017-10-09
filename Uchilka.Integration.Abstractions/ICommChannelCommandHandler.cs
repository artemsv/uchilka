namespace Uchilka.Integration.Abstractions
{
    public interface ICommChannelCommandHandler
    {
        void HandleCommand(CommChannelCommandType cmd);
        void HandleVoice(string path);
        void HandlePhoto(string path);
    }
}
