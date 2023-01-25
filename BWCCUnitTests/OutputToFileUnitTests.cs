namespace BWCCUnitTests
{
    [TestClass]
    public class OutputToFileUnitTests
    {
        [TestMethod]
        public void File_Create_Success()
        {
            string wordsFile = "Words.txt";
            string articleFile = "Article.txt";
            FileService fileService = new FileService(wordsFile, articleFile);

            bool expected = true;
            bool actual = false;

            var isSuccess = fileService.OutputToFile().Success;

            if (isSuccess)
                actual = Helper.ValidateFiles("Output.txt", "Output.txt");

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void File_Create_FileNotFound()
        {
            string wordsFile = "Notfound.txt";
            string articleFile = "Notfound.txt";
            FileService fileService = new FileService(wordsFile, articleFile);

            bool expected = false;

            var actual = fileService.OutputToFile().Success;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void File_Create_ArticleNotFound()
        {
            string wordsFile = "Words.txt";
            string articleFile = "Notfound.txt";
            FileService fileService = new FileService(wordsFile, articleFile);

            bool expected = false;

            var actual = fileService.OutputToFile().Success;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void File_Create_WordsNotFound()
        {
            string wordsFile = "Notfound.txt";
            string articleFile = "Article.txt";
            FileService fileService = new FileService(wordsFile, articleFile);

            bool expected = false;

            var actual = fileService.OutputToFile().Success;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void File_Create_EmptyName()
        {
            string wordsFile = "";
            string articleFile = "";
            FileService fileService = new FileService(wordsFile, articleFile);

            bool expected = false;

            var actual = fileService.OutputToFile().Success;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void File_Create_EmptyWords()
        {
            string wordsFile = "EmptyWords.txt";
            string articleFile = "Article.txt";
            FileService fileService = new FileService(wordsFile, articleFile);

            bool expected = false;

            var actual = fileService.OutputToFile().Success;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void File_Create_EmptyArticle()
        {
            string wordsFile = "Words.txt";
            string articleFile = "EmptyArticle.txt";
            FileService fileService = new FileService(wordsFile, articleFile);

            bool expected = false;

            var actual = fileService.OutputToFile().Success;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void File_Create_NoExtensionWords()
        {
            string wordsFile = "Words";
            string articleFile = "Article.txt";
            FileService fileService = new FileService(wordsFile, articleFile);

            bool expected = false;

            var actual = fileService.OutputToFile().Success;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void File_Create_NoExtensionArticle()
        {
            string wordsFile = "Words.txt";
            string articleFile = "Article";
            FileService fileService = new FileService(wordsFile, articleFile);

            bool expected = false;

            var actual = fileService.OutputToFile().Success;

            Assert.AreEqual(expected, actual);
        }
    }
}