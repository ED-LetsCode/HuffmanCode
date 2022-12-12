using System.Text;

namespace HuffmanCodeLibrary
{
    public class HuffmanCode
    {
        /// <summary>
        /// Reads file and saves it compressed to saving path
        /// </summary>
        public void CompressFile(string filePath, string savingPath, string codeTablePath)
        {
            var text = ReadFile(filePath);
            var dict = GetDictionary(text);
            var binaryDict = CreateTree(dict);
            SaveFile(binaryDict, text, savingPath, codeTablePath);
        }

        public void WriteAndCompressFile(string savingPath, string codeTablePath, string text)
        {
            var dict = GetDictionary(text);
            var binaryDict = CreateTree(dict);
            SaveFile(binaryDict, text, savingPath, codeTablePath);
        }

        /// <summary>
        /// Saves compressed file to saving path
        /// </summary>
        /// <param name="dict">Huffman code dictionary with binary value</param>
        /// <param name="text">Text file that should be compressed</param>
        /// <param name="savingPath">Path where the file will be saved</param>
        /// <exception cref="Exception"> Dictionary not found </exception>
        public void SaveFile(Dictionary<string, string> dict, string text, string savingPath, string codeTablePath)
        {
            try
            {
                //Save huffman code tree to file
                SaveCodeTable(dict, codeTablePath);

                StringBuilder sb = new StringBuilder();

                //Adds the binary code of each character contained in the text to the code variable
                text.ToCharArray().ToList().ForEach(c => sb.Append(dict[c.ToString()])); // <= Accesses the key of dictionary with the character from text and gets the binary code 

                List<byte> byteArray = new();
                List<string> strBytes = new();

                string binaryCode = sb.ToString();

                //Adds the binary code to a list of bytes
                for (int i = 0; i < binaryCode.Length - (binaryCode.Length % 8); i += 8)
                {
                    strBytes.Add(binaryCode.Substring(i, 8));
                }

                //Adds the last byte to the list of bytes
                if (binaryCode.Length % 8 > 0) strBytes.Add(binaryCode.Substring(binaryCode.Length - (binaryCode.Length % 8)));

                //Converts the binary code to bytes
                foreach (string s in strBytes)
                {
                    byteArray.Add(Convert.ToByte(s, 2));
                }

                File.WriteAllBytes(savingPath, byteArray.ToArray());
            }
            catch (DirectoryNotFoundException ex) { throw new DirectoryNotFoundException("Dictionary not found", ex); }
        }

        /// <summary>
        /// Decompresses file and returns the text
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="codeTablePath"></param>
        /// <returns>decompressed text from the file</returns>
        public string DecompressFile(string filePath)
        {
            string codeTablePath = filePath.Remove(filePath.LastIndexOf('\\') + 1) + "HuffmanTree.tree";
            var dict = LoadCodeTable(codeTablePath);

            StringBuilder sb = new StringBuilder();

            byte[] fileBytes = File.ReadAllBytes(filePath);

            //Converts file bytes to string and adds to stringbuilder
            foreach (byte b in fileBytes)
            {
                sb.Append(Convert.ToString(b, 2).PadLeft(8, '0'));
            }

            string fileString = sb.ToString();
            string binaryCode = "";
            sb.Clear();

            foreach (char c in fileString)
            {
                binaryCode += c;
                if (dict.ContainsValue(binaryCode))
                {
                    //Get value from dictionary and add to stringbuilder
                    sb.Append(dict.FirstOrDefault(x => x.Value == binaryCode).Key);
                    binaryCode = "";
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Creates a Huffman Code Binary tree
        /// </summary>
        /// <param name="dict">Dictionary of characters and their frequencys</param>
        /// <returns>Dictionary with characters and binary code</returns>
        public Dictionary<string, string> CreateTree(Dictionary<string, int> dict)
        {
            HuffmanBinaryTree bt = new();
            //Create huffman binary tree
            Node node = bt.CreateBinaryTree(dict);
            //Create binary code and return the return value from the CreateCode function
            return bt.CreateBinaryCode(node);
        }

        /// <summary>
        /// Create dictionary with characters and their frequency
        /// </summary>
        /// <returns>Ascending sorted dictionary with characters and their frequency</returns>
        public Dictionary<string, int> GetDictionary(string text)
        {
            Dictionary<string, int> dict = new();

            //Remove duplicates from text parameter and add every character to dictionary and set frequency to 0
            text.Distinct().ToList().ForEach(character => dict.Add(character.ToString(), 0));

            //Iterate through every character from parameter text and increase frequency by 1 in dictionary
            text.ToCharArray().ToList().ForEach(character => dict[character.ToString()]++);

            //Sort the dictionary by the count of the characters ascending
            return dict.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
        }

        /// <summary>
        /// Reads the file
        /// </summary>
        /// <returns>Returns the text</returns>
        /// <exception cref="FileNotFoundException"></exception>
        public string ReadFile(string filePath)
        {
            try
            {
                return File.ReadAllText(filePath).Replace("\r", "");
            }
            catch (Exception ex) { throw new FileNotFoundException("File not found", ex); }
        }


        /// <summary>
        /// Saves huffman code tree to file
        /// </summary>
        /// <param name="codeTable"></param>
        /// <param name="savingPath"></param>
        public void SaveCodeTable(Dictionary<string, string> codeTable, string savingPath)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var element in codeTable)
            {
                sb.Append(element.Key);
                sb.Append(element.Value + "\n");
            }
            File.WriteAllText(savingPath, sb.ToString());
        }


        /// <summary>
        /// Loads huffman code tree 
        /// </summary>
        /// <returns>Huffman code tree as dictionary </returns>
        public static Dictionary<string, string> LoadCodeTable(string codeTablePath)
        {
            Dictionary<string, string> codeTable = new Dictionary<string, string>();
            var file = File.ReadAllLines(codeTablePath);
            for (int i = 0; i < file.Length; i++)
            {
                if (file[i] == "")
                {
                    codeTable.Add("\n", file[i + 1]);
                    i++;
                }
                else codeTable.Add(file[i].Substring(0, 1), file[i].Substring(1));
            }
            return codeTable;
        }


        /// <summary>
        /// Calculates the percentage between the compared and uncompared file
        /// </summary>
        /// <returns>Percentage of compression</returns>
        public FileData PercentageComparison(string uncompressedFilePath, string compressedFilePath)
        {
            FileInfo compressedFile = new FileInfo(compressedFilePath);
            FileInfo uncompressedFile = new FileInfo(uncompressedFilePath);
            var percentage = Math.Round((uncompressedFile.Length - compressedFile.Length) / (double)uncompressedFile.Length * 100, 2);

            return new FileData()
            {
                UncompressedSize = uncompressedFile.Length,
                CompressedSize = compressedFile.Length,
                Percentage = percentage,
                CompressedFileName = compressedFile.Name,
                UncompressedFileName = uncompressedFile.Name
            };
        }
    }
}
