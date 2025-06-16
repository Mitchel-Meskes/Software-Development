using System;
using System.IO;
using System.Threading;

namespace App.Tests.Utils
{
    public static class TestFileHelper
    {
        public static string TestDataFolder => Path.Combine(Path.GetTempPath(), "NonogramTestData");

        public static string GetTestFilePath(string fileName)
        {
            if (!Directory.Exists(TestDataFolder))
                Directory.CreateDirectory(TestDataFolder);

            return Path.Combine(TestDataFolder, fileName);
        }

        public static void EnsureCleanFile(string fileName)
        {
            string fullPath = GetTestFilePath(fileName);

            // Geef het systeem even de tijd na een clean build
            Thread.Sleep(200); // Voorkomt file lock race condition

            const int maxTries = 10;
            for (int i = 0; i < maxTries; i++)
            {
                try
                {
                    if (File.Exists(fullPath))
                    {
                        // Forceer toegang en verbreek eventuele lock
                        using (FileStream stream = new FileStream(fullPath, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                        {
                            // Bestand is nu vrij
                        }

                        File.Delete(fullPath);
                    }

                    return;
                }
                catch (IOException)
                {
                    Thread.Sleep(100);
                }
                catch (UnauthorizedAccessException)
                {
                    Thread.Sleep(100);
                }
            }

            throw new IOException($"Kon testbestand '{fullPath}' niet verwijderen na meerdere pogingen.");
        }
    }
}
