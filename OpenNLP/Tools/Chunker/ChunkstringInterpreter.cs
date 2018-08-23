namespace OpenNLP.Tools.Chunker
{
    public class ChunkstringInterpreter
    {
        /// <summary>
        /// Gets formatted chunk information for a specified sentence.
        /// </summary>
        /// <param name="data">
        /// a string containing a list of tokens and tags, separated by / characters. For example:
        /// Battle-tested/JJ Japanese/NNP industrial/JJ managers/NNS 
        /// </param>
        /// <returns>
        /// A string containing the formatted chunked sentence
        /// </returns>
        public ChunkingResult GetChunks(string data)
        {
            string[] tokenAndTags = data.Split(' ');
            var tokens = new string[tokenAndTags.Length];
            var tags = new string[tokenAndTags.Length];
            for (int currentTokenAndTag = 0, tokenAndTagCount = tokenAndTags.Length; currentTokenAndTag < tokenAndTagCount; currentTokenAndTag++)
            {
                string[] tokenAndTag = tokenAndTags[currentTokenAndTag].Split('/');
                tokens[currentTokenAndTag] = tokenAndTag[0];
                tags[currentTokenAndTag] = tokenAndTag.Length > 1 ? tokenAndTag[1] : PartsOfSpeech.SentenceFinalPunctuation;
            }

            return new ChunkingResult(tokens, tags);
        }
    }
}
