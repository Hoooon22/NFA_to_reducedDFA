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
        public List<string> final_state = new List<string>(); // final states

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
            string[] tmp = Console.ReadLine().Split(' ');
            foreach (var str in tmp)
            {
                final_state.Add(str);
            }
        }

        public void Prinf_nfa()
        {
            Console.WriteLine("\nNFA => ");
            foreach (var pair1 in nfa_dic)
            {
                Console.Write("Key: {0}, Value: ", pair1.Key);

                foreach (var pair2 in pair1.Value)
                {
                    Console.Write("{0}-[{1}] ", pair2.Key, String.Join(", ", pair2.Value));
                }
                Console.WriteLine();
            }
        }
    }

    class DFA
    {
        public Dictionary<List<string>, Dictionary<string, List<string>>> dfa_dic = new Dictionary<List<string>, Dictionary<string, List<string>>>();
        public Dictionary<List<string>, Dictionary<string, List<string>>> min_dfa_dic = new Dictionary<List<string>, Dictionary<string, List<string>>>();
        public string first_state; // first states
        public List<string> final_state = new List<string>(); // final states

        public Dictionary<List<string>, Dictionary<string, List<string>>> GetDFA(List<string> key)
        {
            Dictionary<List<string>, Dictionary<string, List<string>>> dic = new Dictionary<List<string>, Dictionary<string, List<string>>>();
            dic.Add(key, dfa_dic[key]);

            return dic;
        }

        // NFA to DFA
        public void NFAtoDFA(NFA nfa)
        {
            // 1. Q' = 2^Q -> processing in 4

            // 2. q0' = [q0]
            first_state = nfa.first_state;

            // 3. F' = including final state
            foreach (var f_str in nfa.final_state)
            {
                final_state.Add(f_str);
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
                if (String.Join("", value_list) != first_state) // if combined value is not exist in dfa_dic's Key
                {
                    remaining_state.Add(value_list);
                }
            }
            dfa_dic[first_list] = dfa_key; // update key (first state)

            // 4.2 Other State
            while (remaining_state.Count != 0) // 모든 키의 값들이 state로 채워졌을 경우 종료
            {
                Dictionary<string, List<string>> dfa_key2 = new Dictionary<string, List<string>>();
                if (!dfa_dic.ContainsKey(remaining_state[0])) // if combined value is not exist in dfa_dic's Key
                {
                    dfa_dic.Add(remaining_state[0], null); // add state
                }

                foreach (var t in nfa.nfa_dic[nfa.first_state].Keys) // transition
                {
                    List<string> value_list = new List<string>();

                    foreach (var s in remaining_state[0]) // state
                    {
                        if (s != "")
                        {
                            foreach (var str in nfa.nfa_dic[s][t]) // add reaching state
                            {
                                if (!value_list.Contains(str))
                                {
                                    value_list.Add(str);
                                }
                            }
                        }
                    }
                    dfa_key2.Add(t, value_list);

                    //if (!dfa_dic.ContainsKey(value_list)) // if combined value is not exist in dfa_dic's Key
                    //{
                    //    remaining_state.Add(value_list);
                    //}

                    // 위 방법 대체
                    int n_count = 0;
                    foreach (var key in dfa_dic.Keys)
                    {
                        if (String.Join("", value_list) == String.Join("", key)) // if combined value is not exist in dfa_dic's Key
                        {
                            n_count++;
                        }
                    }
                    foreach (var re_state in remaining_state)
                    {
                        if (String.Join("", value_list) == String.Join("", re_state))
                        {
                            n_count++;
                        }
                    }
                    if (n_count == 0)
                    {
                        remaining_state.Add(value_list);
                    }
                }

                dfa_dic[remaining_state[0]] = dfa_key2; // match key
                remaining_state.RemoveAt(0); // remove remaining_state
            }
            Console.WriteLine("NFA to DFA Done.");
        }

        // Minimization of DFA
        public void Minimization_DFA()
        {
            // 1. partition if is final state or not
            Dictionary<List<string>, Dictionary<string, List<string>>> final_dfa = new Dictionary<List<string>, Dictionary<string, List<string>>>();
            Dictionary<List<string>, Dictionary<string, List<string>>> nonfinal_dfa = new Dictionary<List<string>, Dictionary<string, List<string>>>();
            foreach (var key in dfa_dic.Keys)
            {
                if (key == final_state)
                {
                    final_dfa.Add(key, dfa_dic[key]);
                }
                else
                {
                    nonfinal_dfa.Add(key, dfa_dic[key]);
                }
            }

            // 2. Find the minimum state finite automaton
            List<List<string>> remaining_key = new List<List<string>>();
            for (int i = 0; i < dfa_dic.Count() - 1; i++)
            {
                List<string> compare_state_i = new List<string>();
                foreach (var state in nonfinal_dfa[nonfinal_dfa.Keys.ElementAt(i)].Values)
                {
                    foreach (var str in state)
                    {
                        if (!compare_state_i.Contains(str))
                        {
                            compare_state_i.Add(str);
                        }
                    }
                }
                compare_state_i.Sort(); // 정렬해서 비교하기 위하여

                for (int j = i + 1; j < nonfinal_dfa.Count(); j++)
                {
                    List<string> compare_state_j = new List<string>();
                    foreach (var state in nonfinal_dfa[nonfinal_dfa.Keys.ElementAt(j)].Values)
                    {
                        foreach (var str in state)
                        {
                            if (!compare_state_j.Contains(str))
                            {
                                compare_state_j.Add(str);
                            }
                        }
                    }
                    compare_state_j.Sort(); // 정렬해서 비교하기 위하여

                    if (compare_state_i.SequenceEqual(compare_state_j)) // compare
                    {
                        List<string> key = new List<string>();
                        key.AddRange(nonfinal_dfa.Keys.ElementAt(i));
                        remaining_key.Add(nonfinal_dfa.Keys.ElementAt(i));
                        key.AddRange(nonfinal_dfa.Keys.ElementAt(j));
                        remaining_key.Add(nonfinal_dfa.Keys.ElementAt(j));
                        key = key.Distinct().ToList(); // 중복 제거

                        if (!min_dfa_dic.ContainsKey(key))
                        {
                            Dictionary<string, List<string>> low_key = new Dictionary<string, List<string>>();
                            low_key.Add(nonfinal_dfa[nonfinal_dfa.Keys.ElementAt(0)].Keys.ElementAt(0), compare_state_i);

                            min_dfa_dic.Add(key, low_key);
                        }
                        else if (min_dfa_dic[key].Count() != dfa_dic[final_state].Count())
                        {
                            Dictionary<string, List<string>> low_key = new Dictionary<string, List<string>>();
                            min_dfa_dic[key].Add(nonfinal_dfa[nonfinal_dfa.Keys.ElementAt(min_dfa_dic[key].Count())].Keys.ElementAt(0), compare_state_i);
                        }
                    }
                }
            }

            // 3. combined
            foreach (var n_dfa in dfa_dic)
            {
                if (!remaining_key.Contains(n_dfa.Key))
                {
                    min_dfa_dic.Add(n_dfa.Key, n_dfa.Value);
                }
            }

            var list = min_dfa_dic.Keys.ToList();
            //list.Sort();
        }

        public void Prinf_dfa()
        {
            Console.WriteLine("\nDFA => ");
            foreach (var pair1 in dfa_dic)
            {
                Console.Write("Key: ");
                foreach (var key1 in pair1.Key)
                {
                    Console.Write(key1);
                }

                Console.Write(", Value: ");
                foreach (var pair2 in pair1.Value.Keys)
                {
                    Console.Write("{0}-[{1}] ", pair2, String.Join("", pair1.Value[pair2]));
                }
                Console.WriteLine();
            }
        }

        public void Prinf_min_dfa()
        {
            Console.WriteLine("\nMinimization DFA => ");
            foreach (var pair1 in min_dfa_dic)
            {
                Console.Write("Key: ");
                foreach (var key1 in pair1.Key)
                {
                    Console.Write(key1);
                }

                Console.Write(", Value: ");
                foreach (var pair2 in pair1.Value.Keys)
                {
                    Console.Write("{0}-[{1}] ", pair2, String.Join("", pair1.Value[pair2]));
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

            DFA dfa = new DFA();
            dfa.NFAtoDFA(nfa);
            dfa.Prinf_dfa();
            dfa.Minimization_DFA();
            dfa.Prinf_min_dfa();
        }
    }
}