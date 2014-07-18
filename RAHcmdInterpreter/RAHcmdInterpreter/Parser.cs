using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAHcmdInterpreter
{
    class Parser
    {
        int index;
        Node root = new Node();

        public Node getRoot()
        {
            return root;
        }

        public void Parse(String input)
        {
            Node current = root;
            index = 0;
            input += " ";
            while (index < input.Length)
            {
                String word = getNextWord(input);
                current.child = getNode(word);
                current = current.child;
            }

        }

        Node getNode(String word)
        {
            Token type;

            switch (word)
            {
                case "Parse":
                    type = Token.Parse;
                    break;
                case "Tabs":
                    type = Token.Tabs;
                    break;
                case "Graph":
                    type = Token.Graph;
                    break;
                case "Raw":
                    type = Token.Raw;
                    break;
                case "CloseAll":
                    type = Token.CloseAll;
                    break;
                case "CloseAllBut":
                    type = Token.CloseAllBut;
                    break;
                default:
                    type = Token.Data;
                    break;
            }
            Node n;

            if (type == Token.Data) n = new DataNode(word, type);
            else n = new Node(type);

            return n;
        }

        String getNextWord(String input)
        {
            String word = "";
            char c;
            do
            {
                c = input[index];
                word += c;
                index++;
                c = input[index];
                
            }
            while (c != ' ');
            index++;

            return word;
        }
    }

    enum Token
    {
        None,
        Parse,
        Tabs,
        Graph,
        Raw,
        CloseAll,
        CloseAllBut,
        Data
    }

    class Node
    {
        public Token type;
        public Node child;
        public Node()
        {
            type = Token.None;
        }

        public Node(Token t)
        {
            type = t;
        }
    }

    class DataNode : Node
    {
        String value;
        public DataNode(String v, Token t)
        {
            value = v;
            type = t;
        }

        public String getValue()
        {
            return value;
        }
    }
}
