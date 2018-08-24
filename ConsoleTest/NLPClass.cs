using System.IO;
using OpenNLP.Tools.SentenceDetect;

namespace OpenNLP.ConsoleApp2
{
    public class NLPClass
    {
        private MaximumEntropySentenceDetector _sentenceDetector;
        private readonly string _modelPath;

        public NLPClass(DirectoryInfo modelPath)
        {
            _modelPath = modelPath.FullName;
            
        }

        public string[] Split(string paragraph)
        {
            return SplitSentences(paragraph);
        }

        private string[] SplitSentences(string paragraph)
        {
            if (_sentenceDetector == null)
            {
                _sentenceDetector = new EnglishMaximumEntropySentenceDetector(Path.Combine(_modelPath, "EnglishSD.nbin"));
            }

            return _sentenceDetector.SentenceDetect(paragraph);
        }
    }
}
