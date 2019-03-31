using System;
using System.Linq;
using System.Collections.Generic;

namespace Trane.Languages
{
    [Serializable]
    public class WordRaiting
    {
        private int hashCellSize;
        private LinkedList<WordRaitingInfo>[] hashTable;
        private int wordCount = 0;

        public WordRaiting(int hashCellSize)
        {
            if (hashCellSize == 0) throw new ArgumentException();
            this.hashCellSize = hashCellSize;
            hashTable = new LinkedList<WordRaitingInfo>[1];
        }

        public WordRaiting(IEnumerable<WordRaitingInfo> collection, int hashCellSize)
            : this(hashCellSize)
        {
            SetWordsInfoFromCollection(collection);
        }

        public void SetWordsInfoFromCollection(IEnumerable<WordRaitingInfo> collection)
        {
            if (collection == null) throw new ArgumentException();
            foreach (var word in collection)
                SetWordInfo(word, true);
            if (wordCount / hashCellSize != 0 && hashTable.Length <= wordCount / hashCellSize)
            {
                IncreaseHashTableSize();
            }
        }

        public void SetWordInfo(WordRaitingInfo word)
        {
            SetWordInfo(word, false);
        }

        private void SetWordInfo(WordRaitingInfo word, bool withoutIncreaseHT)
        {
            if (word == null) throw new ArgumentException();
            if (!withoutIncreaseHT)
            {
                if (wordCount / hashCellSize != 0 && hashTable.Length <= wordCount / hashCellSize)
                {
                    IncreaseHashTableSize();
                }
            }
            int hashCellNumber = GetHashCellNumber(word.Word);
            if (hashTable[hashCellNumber] == null)
            {
                hashTable[hashCellNumber] = new LinkedList<WordRaitingInfo>();
                hashTable[hashCellNumber].AddLast(word);
                ++wordCount;
            }
            else
            {
                bool has = false;
                foreach (var w in hashTable[hashCellNumber])
                {
                    if (word.Word == w.Word)
                    {
                        has = true;
                        break;
                    }
                }
                if (!has)
                {
                    hashTable[hashCellNumber].AddLast(word);
                    ++wordCount;
                }
            }
        }

        public WordRaitingInfo GetWordInfo(string word)
        {
            int hashCellNumber = GetHashCellNumber(word.ToLower());
            foreach (var w in hashTable[hashCellNumber])
                if (w.Word == word.ToLower())
                    return w;
            return null;
        }

        public IEnumerable<WordRaitingInfo> GetAllWordInfo()
        {
            return (from cell in hashTable
                    where cell != null && cell.Count > 0
                    from wordInfo in cell
                    orderby wordInfo.Raiting
                    select wordInfo).ToList();
        }

        private void IncreaseHashTableSize()
        {
            LinkedList<WordRaitingInfo>[] oldHashTable = hashTable;
            int count = wordCount;
            hashTable = new LinkedList<WordRaitingInfo>[oldHashTable.Length + 1];
            foreach (var cell in oldHashTable)
            {
                if (cell == null || cell.Count == 0)
                    continue;
                foreach (var word in cell)
                    SetWordInfo(word);
            }
            wordCount = count;
        }

        private int GetHashCellNumber(string word)
        {
            long hashCode = word.GetHashCode();
            if (hashCode < 0)
                hashCode = Math.Abs(hashCode) * 2;
            return (int)(hashCode % hashTable.Length);
        }
    }
}