using BuilderWireCodingChallenge.Services;

namespace BuilderWireCodingChallenge
{
    internal class Program
    {
        /* Change the constants in FilePath.cs accordingly. */
        static void Main(string[] args)
        {
            FileService fileService = new FileService("Words.txt", "Article.txt");
            Console.WriteLine(fileService.OutputToFile().Message);
        }
    }
}