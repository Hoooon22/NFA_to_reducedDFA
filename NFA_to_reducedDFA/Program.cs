using System;
using System.Collections.Generic;
using System.Linq;

namespace NFA_to_reducedDFA
{
    class NFA
    {
        public Dictionary<string, Dictionary<string, string[]>> nfa_dic = new Dictionary<string, Dictionary<string, string[]>>();
        public int s; // number of states
        public int t; // number of transitions
        public string first_state; // first state
        public string[] final_state; // final states

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
                    Console.Write("next states from state " + state + " through " + transition + ": ");
                    string next_state = Console.ReadLine();
                    t_dic.Add(transition, next_state.Split(' '));
                }

                nfa_dic.Add(state, t_dic); // add nfa
            }
            // Enter First state
            Console.Write("first states: ");
            first_state = Console.ReadLine();

            // Enter Final state
            Console.Write("final states: ");
            final_state = Console.ReadLine().Split(' ');
        }

        public void Prinf_nfa()
        {
            Console.WriteLine("\nNFA => ");
            foreach (var pair1 in nfa_dic)
            {
                Console.Write("Key: {0}, Value: ", pair1.Key);

                foreach (var pair2 in pair1.Value)
                {
                    Console.Write("{0}-[{1}] ", pair2.Key, String.Join(", ", pair2.Value)); ;
                }
                Console.WriteLine();
            }
        }
    }

    class DFA
    {
        public Dictionary<List<string>, Dictionary<string, List<string>>> dfa_dic = new Dictionary<List<string>, Dictionary<string, List<string>>>();
        public string first_state; // first states
        public List<string[]> final_state = new List<string[]>(); // final states

        // NFA to DFA
        public void NFAtoDFA(NFA nfa)
        {
            // 1. Q' = 2^Q -> processing in 4

            // 2. q0' = [q0]
            first_state = nfa.first_state;

            // 3. F' = including final state
            foreach (var nfa_s in nfa.nfa_dic)
            {
                foreach (var value_dic in nfa_s.Value.Values)
                {
                    if (Array.IndexOf(value_dic, nfa.final_state) > -1) // if contain nfa final state
                    {
                        final_state.Add(value_dic); // add final states
                    }
                }
            }

            // 4. reconstruct fa + 1. Q' = 2^Q
            List<List<string>> remaining_state = new List<List<string>>(); // list of remaining state
            // 4.1. First State
            List<string> first_list = new List<string>();
            first_list.Add(first_state);
            dfa_dic.Add(first_list, null); // 1. Q' = 2^Q
            
            Dictionary<string, List<string>> dfa_key = new Dictionary<string, List<string>>();
            foreach (var nfa_value in nfa.nfa_dic[first_state]) // 첫 번째 state 구성
            {
                List<string> value_list = new List<string>();
                foreach(var v in nfa_value.Value)
                {
                    value_list.Add(v);
                }
                dfa_key.Add(nfa_value.Key, value_list);
                if (!dfa_dic.ContainsKey(value_list)) // if combined value is not exist in dfa_dic's Key
                {
                    remaining_state.Add(value_list);
                    dfa_dic.Add(value_list, null); // add state
                }
            }
            dfa_dic[first_list] = dfa_key; // update key (first state)

            // 4.2 Other State
            while (remaining_state.Any()) // 모든 키의 값들이 state로 채워졌을 경우 종료
            {
                foreach (var s in remaining_state[0]) // A, B
                {

                }
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
