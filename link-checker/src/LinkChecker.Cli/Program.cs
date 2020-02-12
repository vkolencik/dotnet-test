using System;
using System.IO;

using LinkChecker.Domain;

namespace LinkChecker.Cli
{
    class Program
    {
        private const string Usage = @"
LinkChecker.

Usage:
    LinkChecker <input_file> <output_file>";

        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.Write(Usage);
            }

            var input = args[0];
            using (var linkReader = new LinkReader(
                input == "-"
                    ? Console.OpenStandardInput()
                    : new FileStream(input, FileMode.Open, FileAccess.Read)))
            {
                var output = args[1];
                using (var outputStream = new StreamWriter(
                    output == "-"
                        ? Console.OpenStandardOutput()
                        : new FileStream(output, FileMode.Create, FileAccess.Write)))
                {
                    var links = linkReader.Read();
                    var linkCheckerInput = new LinkCheckerInput(links);

                }
            }
        }
    }
}
