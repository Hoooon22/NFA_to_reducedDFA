using System;
using System.Collections.Generic;

namespace NFA_to_reducedDFA
{
    class NFA
    {
        private Dictionary<string, Dictionary<string, string[]>> nfa_dic = new Dictionary<string, Dictionary<string, string[]>>();
        private int s; // number of states
        private int t; // number of transitions

        public void Enter_nfa()
        {
            // Input states and transitions
            Console.Write("Enter the number of states: ");
            s = int.Parse(Console.ReadLine());
            Console.Write("Enter the number of transitions: ");
            t = int.Parse(Console.ReadLine());

            // Enter name of state and transition
            for (int i = 0; i < s; i++) // state
            {
                Console.Write("\n" + i + ". state name: ");
                string state = Console.ReadLine();

                Dictionary<string, string[]> t_dic = new Dictionary<string, string[]>(); // 임시 저장
                for (int j = 0; j < t; j++) // transition
                {
                    Console.Write("\n" + i + ". transition name: ");
                    string transition = Console.ReadLine();
                    Console.Write("next state from state " + state + " through " + transition + ": ");
                    string next_state = Console.ReadLine();
                    t_dic.Add(transition, next_state.Split(' '));
                }

                nfa_dic.Add(state, t_dic); // add nfa
            }
        }
        public void Prinf_nfa()
        {
            Console.WriteLine("\nNFA => ");
            foreach (var pair1 in nfa_dic)
            {
                Console.Write("Key: {0}, ", pair1.Key);

                foreach (var pair2 in pair1.Value)
                {
                    Console.Write("Value: {0}: [{1}]", pair2.Key, String.Join(", ", pair2.Value)); ;
                }
                Console.WriteLine();
            }
        }
    }

    class MainClass
    {
        public static void Main(string[] args)
        {
            NFA nfa = new NFA();
            nfa.Enter_nfa();
            nfa.Prinf_nfa();
        }
    }
}
