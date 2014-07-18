using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAHcmdInterpreter
{
    class RAHCore
    {
        Parser p;
        Interpreter i;
        public RAHCore()
        {
            p = new Parser();
            i = new Interpreter();
        }

        public String ParseInput(string input)
        {
            //TODO Parse
            //
            p.Parse(input);
            Node root = p.getRoot();
            string response = i.Interpret(root);

            return response;
        }
    }
}
