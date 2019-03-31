using System;

namespace Trane.Languages
{
    [Serializable]
    public class WordRaitingInfo
    {
        public int Raiting { get; }
        public string Word { get; }
        public string Transcription { get; }
        public string Translation { get; }

        public WordRaitingInfo(int raiting, string word, string transcription = null, string translation = null)
        {
            Raiting = raiting;
            Word = word.ToLower();
            Transcription = transcription;
            Translation = translation;
        }
    }
}