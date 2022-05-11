using System;
using System.Collections.Generic;

namespace NFA_to_reducedDFA
{
    class NFA
    {
        private Dictionary<string, List<string>> nfa = new Dictionary<string, List<string>>();
        private int s; // number of states
        private int t; // number of transitions

        public void Enter_nfa()
        {
            // Input states and transitions
            Console.Write("Enter the number of states: ");
            s = int.Parse(Console.ReadLine());
            Console.Write("Enter the number of transitions: ");
            t = int.Parse(Console.ReadLine());

            for (int i = 0; i < s; i++)
            {
                
            }
        }
    }

    class MainClass
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
