using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Lab4
{
    class Program
    {
        static int[,] connections, links;
        static int size;

        static void print_array(int[,] matrix) 
        {
            Console.WriteLine("   " + string.Join(" ", Enumerable.Range(1, size)));
            Console.WriteLine("  " + string.Concat(Enumerable.Repeat("__", size)));

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                Console.Write((i + 1) + " |");
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    Console.Write(matrix[i, j].ToString() + ' ');
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        static void hand_init(string message)
        {
            //reads one line in a row like
            //1 2 3
            //4 5 6

            Console.WriteLine("Enter {0}", message);
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Console.WriteLine(string.Format("Enter [{0}, {1}]:", i, j));
                    connections[i, j] = Convert.ToInt32(Console.ReadLine());
                }
            }
        }

        static void calculate_links()
        {
            List<Task> tasks = new List<Task>();
            for (int i = 0; i < size; i++)
            {
                int index = i;
                var task = Task.Factory.StartNew(() =>
                {
                    calculate_row(index);
                });
                tasks.Add(task);
            }
            Task.WaitAll(tasks.ToArray());
        }

        static void calculate_row(int i) 
        {
            bool[] is_visited = new bool[size];
            
            for (int j = 0; j < size; j++)
            {
                
                links[i, j] = check_link(i, j, is_visited) ? 1 : 0; 
                
            }
        }

        static bool check_link(int start_node, int end_node, bool[] is_visited, int iter=0)
        {
            //check if start node have loop
            if (iter == 0 && start_node == end_node) 
            {
                return connections[start_node, end_node] == 1;
            }
            //check if we arrived to end node and this isn't 0 iter
            if (start_node == end_node && iter != 0)
            {
                return true;
            }
            for (int i = 0; i < size; i++)
            {
                if (connections[start_node, i] == 1 && start_node != i)
                {
                    if (!is_visited[i])
                    {
                        bool[] localVisited = is_visited;
                        localVisited[start_node] = true;
                        //moving to next recursion with next possible node
                        if (check_link(i, end_node, localVisited, iter++)) 
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Enter the number of graph nodes:");
            size = Convert.ToInt32(Console.ReadLine());

            connections = new int[size, size];
            links = new int[size, size];

            Console.WriteLine("Use random values? y/n");
            ConsoleKey answer = Console.ReadKey(true).Key;

            if (answer == ConsoleKey.Y)
            {

                Random rand = new Random();

                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        connections[i, j] = rand.Next(0, 2);
                    }
                }
            }
            else 
            {
                hand_init("Graph connections");
            }

            Console.WriteLine("\nConnections:");
            print_array(connections);

            calculate_links();
            Console.WriteLine("All possible links:");
            print_array(links);

        }

    }
}
