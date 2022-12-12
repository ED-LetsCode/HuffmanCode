using HuffmanCodeLibrary;

namespace HuffmanCodeConsole
{
    internal class Program
    {
        static HuffmanCode huffmanCode = new HuffmanCode();

        static void Main(string[] args)
        {
            bool isRunning = true;
            while (isRunning)
            {
                PrintHeader();
                switch (Console.ReadLine())
                {
                    case "1":
                        CompressFile();
                        break;

                    case "2":
                        DecompressFile();
                        break;

                    case "3":
                        WriteFile();
                        break;

                    case "4":
                        Console.Clear();
                        break;

                    case "5":
                        isRunning = false;
                        break;

                    default:
                        PrintErrorMessage("Please choose a valid option!");
                        break;
                }
            }
        }

        private static void CompressFile()
        {
            Console.WriteLine("Enter the path of the file you want to compress:");
            string uncompressedPath = Console.ReadLine();
            Console.WriteLine();

            Console.WriteLine("Enter the saving path of the compressed file:");
            string compressedPath = Console.ReadLine();
            string codeTablePath = uncompressedPath.Remove(uncompressedPath.LastIndexOf('\\') + 1) + "HuffmanTree.tree";
            try
            {
                Console.WriteLine("Compressing file.....");
                huffmanCode.CompressFile(uncompressedPath, compressedPath, codeTablePath);

                var fileData = huffmanCode.PercentageComparison(uncompressedPath, compressedPath);
                PrintFileInformation(fileData);
            }
            catch (DirectoryNotFoundException ex) { PrintErrorMessage(ex.Message); }
            catch (FileNotFoundException ex) { PrintErrorMessage(ex.Message); }
            catch (Exception ex) { PrintErrorMessage(ex.Message); }
        }

        private static void DecompressFile()
        {
            Console.WriteLine("Enter path to the file you want to decompress:");
            string compressedPath = Console.ReadLine();
            Console.WriteLine();

            try
            {
                string text = huffmanCode.DecompressFile(compressedPath);
                PrintFileText(text);
            }
            catch (DirectoryNotFoundException ex) { PrintErrorMessage(ex.Message); }
            catch (FileNotFoundException ex) { PrintErrorMessage(ex.Message); }
            catch (Exception ex) { PrintErrorMessage(ex.Message); }
        }

        private static void WriteFile()
        {
            Console.WriteLine();
            Console.WriteLine("Enter you're text: ");
            string text = Console.ReadLine();
            Console.WriteLine();

            Console.WriteLine("Enter the saving path for the compressed file:");
            string savingPath = Console.ReadLine();
            Console.WriteLine();
            string codeTablePath = savingPath.Remove(savingPath.LastIndexOf('\\') + 1) + "HuffmanTree.tree";

            try
            {
                huffmanCode.WriteAndCompressFile(savingPath, codeTablePath, text);
                Console.WriteLine("File sucessfully created");
                Console.WriteLine();
            }
            catch (DirectoryNotFoundException ex) { PrintErrorMessage(ex.Message); }
            catch (FileNotFoundException ex) { PrintErrorMessage(ex.Message); }
            catch (Exception ex) { PrintErrorMessage(ex.Message); }

        }

        private static void PrintErrorMessage(string text)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(text);
            Console.ResetColor();
        }
        private static void PrintFileText(string text)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("------------------------------------------------------------------------");
            Console.ResetColor();
            Console.WriteLine(text);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("------------------------------------------------------------------------");
            Console.WriteLine();
            Console.WriteLine();
            Console.ResetColor();
        }

        private static void PrintFileInformation(FileData fileData)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Uncompressed filename:      {fileData.UncompressedFileName}");
            Console.WriteLine($"Uncompressed file size:     {fileData.UncompressedSize} bytes");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Compressed filename:        {fileData.CompressedFileName}");
            Console.WriteLine($"Compressed file size:       {fileData.CompressedSize} bytes");
            Console.WriteLine($"Percentage:                 {fileData.Percentage}%");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.ResetColor();
        }

        private static void PrintHeader()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(" ██████╗ ██████╗ ███╗   ███╗██████╗ ██████╗ ███████╗███████╗███████╗██╗ ██████╗ ███╗   ██╗\n" +
                               "██╔════╝██╔═══██╗████╗ ████║██╔══██╗██╔══██╗██╔════╝██╔════╝██╔════╝██║██╔═══██╗████╗  ██║\n" +
                               "██║     ██║   ██║██╔████╔██║██████╔╝██████╔╝█████╗  ███████╗███████╗██║██║   ██║██╔██╗ ██║\n" +
                               "██║     ██║   ██║██║╚██╔╝██║██╔═══╝ ██╔══██╗██╔══╝  ╚════██║╚════██║██║██║   ██║██║╚██╗██║\n" +
                               "╚██████╗╚██████╔╝██║ ╚═╝ ██║██║     ██║  ██║███████╗███████║███████║██║╚██████╔╝██║ ╚████║\n" +
                               " ╚═════╝ ╚═════╝ ╚═╝     ╚═╝╚═╝     ╚═╝  ╚═╝╚══════╝╚══════╝╚══════╝╚═╝ ╚═════╝ ╚═╝  ╚═══╝\n" +
                               "To compress file select        [1]\n" +
                               "To read compressed file select [2]\n" +
                               "Write compressed file          [3]\n" +
                               "Clear console                  [4]\n" +
                               "To exit                        [5]\n" +
                               "\n");

            Console.ResetColor();
        }
    }
}