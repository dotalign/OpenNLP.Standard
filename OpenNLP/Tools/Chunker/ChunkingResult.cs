namespace OpenNLP.Tools.Chunker
{
    public class ChunkingResult
    {
        public ChunkingResult(string[] tokens, string[] tags)
        {
            Tokens = tokens;
            Tags = tags;
        }

        public string[] Tokens { get; }
        public string[] Tags { get; }
    }
}
