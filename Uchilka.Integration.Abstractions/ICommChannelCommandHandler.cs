namespace Uchilka.Integration.Abstractions
{
    public interface ICommChannelCommandHandler : ICommChannelHandler
    {
        void HandleCommand(CommChannelCommandType cmd);
    }
}
