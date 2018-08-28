using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNLP.Tools.Chunker
{
    public class ChunksetInterpreter
    {
        /// <summary>
        /// Gets formatted chunk information for a specified sentence.
        /// </summary>
        /// <param name="tokens">
        /// string array of tokens in the sentence
        /// </param>
        /// <param name="tags">
        /// string array of POS tags for the tokens in the sentence
        /// </param>
        /// <param name="chunks">
        /// already chunked
        /// </param>
        /// <returns>
        /// A string containing the formatted chunked sentence
        /// </returns>
        public List<SentenceChunk> GetChunks(string[] tokens, string[] tags, string[] chunks)
        {
            var results = new List<SentenceChunk>();

            SentenceChunk currentSentenceChunk = null;
            for (int currentChunk = 0, chunkCount = chunks.Length; currentChunk < chunkCount; currentChunk++)
            {
                if (
                    // Per https://opennlp.apache.org/docs/1.5.3/manual/opennlp.html
                    // it seems like B- is expected when it's the first chunk. 
                    // But in practice with "Awesome!" it returns "I-NP as the first chunk". 
                    (currentChunk == 0 && chunks[currentChunk].StartsWith("I-")) ||
                    chunks[currentChunk].StartsWith("B-") || 
                    chunks[currentChunk] == "O")
                {
                    if (currentSentenceChunk != null)
                    {
                        results.Add(currentSentenceChunk);
                    }

                    var index = results.Count;
                    if (chunks[currentChunk].Length > 2)
                    {
                        var tag = chunks[currentChunk].Substring(2);
                        currentSentenceChunk = new SentenceChunk(tag, index);
                    }
                    else
                    {
                        currentSentenceChunk = new SentenceChunk(index);
                    }
                }

                // in all cases add the tagged word
                var word = tokens[currentChunk];
                var wTag = tags[currentChunk];
                var wIndex = currentSentenceChunk.TaggedWords.Count;
                var taggedWord = new TaggedWord(word, wTag, wIndex);
                currentSentenceChunk.TaggedWords.Add(taggedWord);
            }
            // add last chunk
            results.Add(currentSentenceChunk);

            return results;
        }
    }
}
