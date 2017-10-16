
using Microsoft.Win32;
using System.Diagnostics;

namespace Uchilka.Multimedia.Player
{
    internal class MPC : IPlayer
    {
        private readonly string _path;

        public MPC()
        {
            var key = Registry.CurrentUser.OpenSubKey(@"Software\MPC-HC\MPC-HC");

            if (key != null)
            {
                _path = key.GetValue("ExePath").ToString();
            }
        }

        public void Play(string path)
        {
            if (_path != null)
            {
                Process.Start(_path, string.Format("{0} /play /nofocus /close /minimized", path));
            }
        }
    }
}
