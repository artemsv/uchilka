using System.Collections.Generic;

namespace Uchilka.DataModels
{
    internal class Question
    {
        public string Text { get; set; }
        public List<Answer> Answers { get; set; }

    }
}
