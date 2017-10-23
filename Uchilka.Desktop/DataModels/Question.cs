using System.Collections.Generic;

namespace Uchilka.DataModels
{
    internal class Question
    {
        public string Text { get; set; }
        public IEnumerable<Answer> Answers { get; set; }

        public Content Content { get; set; }
    }
}
