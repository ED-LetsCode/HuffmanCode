namespace HuffmanCodeLibrary
{
    public class HuffmanBinaryTree
    {

        /// <summary>
        /// Creates a Huffman binary tree
        /// </summary>
        /// <param name="dict">Dictionary with characters and their frequencys</param>
        /// <returns>Root node with all their child nodes</returns>
        public Node CreateBinaryTree(Dictionary<string, int> dict)
        {
            //The goal is to create a tree with the highest frequency at the top and the lowest frequency at the bottom
            //We create the left and right nodes at the same time and combine them into one root node
            //
            //                       Root
            //                      /    \
            //                     0      0
            //                    / \    / \
            //                   a   b  c   d

            //Create two Dictionarys on for the left and one for the right node
            Dictionary<string, int> dictLeft = new();
            Dictionary<string, int> dictRight = new();

            for (int i = 0; i < dict.Count; i++)
            {
                //If i % 2 == 0 then add to dictLeft
                if (i % 2 == 0)
                    dictLeft.Add(dict.ElementAt(i).Key, dict.ElementAt(i).Value);
                //Else add to dictRight
                else
                    dictRight.Add(dict.ElementAt(i).Key, dict.ElementAt(i).Value);
            }

            //Create a root node with the leftDict and rightDict
            Node rootNode = new(CreateBranch(dictLeft), CreateBranch(dictRight));
            return rootNode;
        }

        /// <summary>
        /// Creates a branch of the tree
        /// </summary>
        /// <param name="dict">Dictionary with characters and their frequencys </param>
        /// <returns>Node</returns>
        private Node CreateBranch(Dictionary<string, int> dict)
        {
            //Create LIFO (Last in First out) list
            var nodeQueue = new Queue<Node>();
            var newNodeQueue = new Queue<Node>();

            //Add all nodes to the nodeQueue list
            dict.ToList().ForEach(value => nodeQueue.Enqueue(new Node(value.Key, value.Value)));

            //Add first element from the nodeQueue list as new node with left and rigth child nodes to the newNodeQueue
            newNodeQueue.Enqueue(new Node(nodeQueue.Dequeue(), nodeQueue.Dequeue()));

            //Build huffman tree
            while (nodeQueue.Count > 1)
            {
                //Add the first element from the nodeQueue list as new node with left and rigth child nodes to the newNodeQueue
                newNodeQueue.Enqueue(new Node(nodeQueue.Dequeue(), nodeQueue.Dequeue()));

                //Add the first 2 elements from the newNodeQueue list as new node with left and rigth child nodes to the newNodeQueue
                newNodeQueue.Enqueue(new Node(newNodeQueue.Dequeue(), newNodeQueue.Dequeue()));
            }

            newNodeQueue.Enqueue(nodeQueue.FirstOrDefault());
            var t = new Node(newNodeQueue.Dequeue(), newNodeQueue.Dequeue());

            return t;
        }


        /// <summary>
        /// Creates binary code from the root node
        /// </summary>
        /// <param name="root">Root node with all their child nodes</param>
        /// <returns>Dictionary with the characters and their binary code</returns>
        public Dictionary<string, string> CreateBinaryCode(Node root)
        {
            var code = new Dictionary<string, string>();
            //Call private overloaded CreateCode() function with root node, dictionary and empty string
            CreateBinaryCode(root, code, "");
            return code;
        }

        /// <summary>
        /// Recursive function to create binary code
        /// </summary>
        private void CreateBinaryCode(Node node, Dictionary<string, string> code, string s)
        {
            //If left and right child nodes are null then add the character and the binary code to the dictionary
            if (node.Left == null && node.Right == null)
            {
                code.Add(node.Character, s);
                return;
            }

            //Else call the function again with left and right child nodes and add 0 for left node and 1 for right node
            if (node.Left != null) CreateBinaryCode(node.Left, code, s + "0");
            if (node.Right != null) CreateBinaryCode(node.Right, code, s + "1");
        }
    }
}