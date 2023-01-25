using BuilderWireCodingChallenge.Models;

namespace BuilderWireCodingChallenge.Services
{
    public class FileService
    {
        CommonResponseService responseService = new CommonResponseService();

        private string wordsFile, articleFile;

        public FileService(string wordsFileName, string articleFileName)
        {
            wordsFile = wordsFileName;
            articleFile = articleFileName;
        }

        /// <summary>
        /// Gets the list of words to output from a text file.
        /// Gets the article from a text file.
        /// Processes the list of words and the article, format it, then produces a file called "Output.txt"
        /// </summary>
        /// <returns>Service Response</returns>
        public ServiceResponseModel OutputToFile()
        {
            List<string> outputList = new List<string>();

            try
            {
                if (string.IsNullOrEmpty(wordsFile))
                    return responseService.Response(this.GetType().Name, "OutputToFile()", false, "Words file name is required.");

                if (string.IsNullOrEmpty(articleFile))
                    return responseService.Response(this.GetType().Name, "OutputToFile()", false, "Article file name must be provided.");

                var words = GetWordsFromFile(FilePath.InputFilesPath + wordsFile);
                var paragraph = GetArticleFromFile(FilePath.InputFilesPath + articleFile, words);

                foreach (var word in words)
                {
                    var isInParagraph = paragraph.Where(p => p.Word == word.Word).ToList();

                    if (isInParagraph.Count > 0)
                        outputList.Add(string.Format(" {0}. {1} {{{2}:{3}}}", word.Sequence, word.Word, isInParagraph.Count, string.Join(",", isInParagraph.Select(m => m.SentenceNumber).ToArray())));
                }

                if (outputList.Count > 0)
                {
                    File.WriteAllLines("Output.txt", outputList); // Output file is at D:\Github\BuilderWireCodingChallenge\bin\Debug\net6.0
                    return responseService.Response(this.GetType().Name, "OutputToFile()", true, "Output.txt file has been created successfully.");
                }

                return responseService.Response(this.GetType().Name, "OutputToFile()", false, "No data to process.");
            }
            catch (Exception ex)
            { 
                return responseService.Response(this.GetType().Name, "OutputToFile()", false, ex.Message.ToString());
            }
        }

        #region Private

        /// <summary>
        /// Gets the list of words to output from a text file.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns>List of Words Model</returns>
        private List<WordModel> GetWordsFromFile(string filePath)
        {
            List<WordModel> listWords = new List<WordModel>();

            try
            {
                char c = '`'; //26 letters
                int repeat = 1;

                using (StreamReader sr = new StreamReader(filePath))
                {
                    while (sr.Peek() >= 0)
                    {
                        var word = sr.ReadLine();

                        if (!string.IsNullOrEmpty(word))
                        {
                            WordModel currentWord = new WordModel();
                            currentWord.Sequence = c.IterateAlphabetSequence(repeat, out repeat, out c);
                            currentWord.Word = word.ToLower();
                            listWords.Add(currentWord);
                        }
                    }
                }

                return listWords;
            }
            catch (Exception)
            {
                return listWords;
            }
        }

        /// <summary>
        /// Gets the article from a text file.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="words"></param>
        /// <returns>List of Sentences Model</returns>
        private List<SentenceModel> GetArticleFromFile(string filePath, List<WordModel> words)
        {
            List<SentenceModel> listSentence = new List<SentenceModel>();

            try
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    while (sr.Peek() >= 0)
                    {
                        var sentences = sr.ReadLine();

                        if (string.IsNullOrEmpty(sentences))
                            continue;

                        string[] arraySentence = sentences.GetValidSentenceList(words);

                        for (int n = 0; n < arraySentence.Length; n++)
                        {
                            string[] arrayWords = arraySentence[n].Split(' ');

                            foreach (var word in arrayWords)
                            {
                                var trimmed = word.ToLower().Trim().TrimEnd(',');

                                if (string.IsNullOrEmpty(trimmed))
                                    continue;

                                SentenceModel current = new SentenceModel();
                                current.Word = trimmed;
                                current.SentenceNumber = n == 0 ? 1 : n + 1;
                                listSentence.Add(current);
                            }
                        }
                    }
                }

                return listSentence;
            }
            catch (Exception)
            {
                return listSentence;
            }
        }

        #endregion
    }

    internal static class FileProcessExtension
    {
        /// <summary>
        /// Iterates the sequence by alphabet.
        /// </summary>
        /// <param name="c"></param>
        /// <param name="repeat"></param>
        /// <param name="iterateRepeat"></param>
        /// <param name="iterateChar"></param>
        /// <returns>The next letter in the sequence</returns>
        internal static string IterateAlphabetSequence(this char c, int repeat, out int iterateRepeat, out char iterateChar)
        {
            iterateRepeat = repeat;
            iterateChar = c;

            if (c == 'z')
            {
                iterateChar = 'a';
                iterateRepeat++;
            }
            else
            {
                iterateChar++;
            }

            return new string(iterateChar, iterateRepeat);
        }

        /// <summary>
        /// Divides the paragaph into sentences.
        /// </summary>
        /// <param name="sentences"></param>
        /// <param name="words"></param>
        /// <returns>List of sentences</returns>
        internal static string[] GetValidSentenceList(this string sentences, List<WordModel> words)
        {
            List<string> validSentences = new List<string>();

            string[] arraySentence = sentences.Split(new char[] { '.' }).Select(x => x + ".").ToArray();
            bool added = false;

            for (int n = 0; n < arraySentence.Length; n++)
            {
                added = false;
                List<string> combinedSentence = new List<string>();

                while (n + 1 < arraySentence.Length)
                {
                    string[] arrayWords = arraySentence[n].Split(' ');

                    // Process end of sentence. Use the Words.txt file as a reference of words.
                    if (words.Any(w => w.Word == arrayWords[arrayWords.Length - 1].Trim().ToLower()) || arrayWords[arrayWords.Length - 1].Replace(".", "").Length == 1)
                    {
                        var currentSentence = arraySentence[n++].Trim();
                        var nextSentence = arraySentence[n].Trim();

                        if (nextSentence.Replace(".", "").Length == 1)
                        {
                            // If the last word is an abbreviation and can be found from the words list, then the sentence does not stop there
                            combinedSentence.Add(currentSentence + nextSentence);
                            n++;
                        }
                        else
                        {
                            // If the last word is an honorific value and can be found from the words list, then the sentence does not stop there
                            combinedSentence.Add(currentSentence);
                            combinedSentence.Add(nextSentence);
                        }

                        added = true;
                    }
                    else
                    {
                        break;
                    }
                }

                if (added)
                {
                    validSentences.Add(string.Join(" ", combinedSentence).TrimEnd('.'));
                }
                else
                {
                    string[] arrayWords = arraySentence[n].Split(' ');

                    if (arrayWords.Length == 1 && arrayWords[0] == ".")
                        n++; // Skip next line since it is not a valid sentence
                    else
                        validSentences.Add(arraySentence[n].Trim().TrimEnd('.')); // Valid sentence
                }
            }

            return validSentences.ToArray();
        }
    }
}