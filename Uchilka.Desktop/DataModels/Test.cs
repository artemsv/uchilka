using System.Collections.Generic;

namespace Uchilka.DataModels
{
    internal class Test
    {
        public string Name { get; set; }
        public IEnumerable<Question> Questions { get; set; }
    }
}
