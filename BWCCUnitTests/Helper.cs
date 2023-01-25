using BuilderWireCodingChallenge;
using System.Security.Cryptography;

namespace BWCCUnitTests
{
    internal static class Helper
    {
        public static bool ValidateFiles(string expectedFile, string actualFile)
        {
            try
            {
                string expectedFilePath = FilePath.ExpectedOutputFilesPath + expectedFile;
                string actualFilePath = FilePath.ActualOutputFilesPath + actualFile;
                string expectedFileHash, actualFileHash;

                using (var md5 = MD5.Create())
                {
                    using (var stream = File.OpenRead(expectedFilePath))
                    {
                        expectedFileHash = BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", "").ToLower();
                    }
                    using (var stream = File.OpenRead(actualFilePath))
                    {
                        actualFileHash = BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", "").ToLower();
                    }
                }

                if (expectedFileHash == actualFileHash)
                    return true;

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}