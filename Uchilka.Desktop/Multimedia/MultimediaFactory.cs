using Uchilka.Multimedia.Player;

namespace Uchilka.Multimedia
{
    public class MultimediaFactory : IMultimediaFactory
    {
        public IPlayer GetPlayer()
        {
            return new MPC();
        }
    }
}
