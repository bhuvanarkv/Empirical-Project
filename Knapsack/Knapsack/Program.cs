using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Knapsack
{
    class KnapSack
    {
        public int BestValue;
        public int KnapSackBruteForce(int number, int Capacity, int[] value, int[] wt)
        {
            var bestValue = 0;
            var bestPosition = 0;
            var size = number;

            if (size == 0 || Capacity == 0)
                return 0;

            var n = (long)Math.Pow(2, size);
            for (var i = 0; i < n; i++)
            {
                var total = 0;
                var weight = 0;
                for (var j = 0; j < size; j++)
                {
                    // if bit not included then skip
                    if (((i >> j) & 1) != 1) continue;

                    total += value[j];
                    weight += wt[j];
                }

                if (weight <= Capacity && total > bestValue)
                {
                    bestPosition = i;
                    bestValue = total;
                }
            }
            BestValue = bestValue;

            return BestValue;
        }
        public int KnapSackBottomUp(int number, int Capacity, int[] value, int[] wt)
        {
            var size = number;
            int[,] Value = new int[size+1,Capacity+1];            
            if (size == 0 || Capacity == 0)
                return 0;
            for (int i = 0; i <= size; i++)
            {
                for (int j = 0; j <=Capacity; j++)
                {
                    if (i == 0 || j == 0)
                        Value[i, j] = 0;
                    else if (wt[i-1] > j)
                        Value[i, j] = Value[i - 1, j];
                    else
                        Value[i, j] = Math.Max(Value[i - 1, j], Value[i - 1, j - wt[i-1]] + value[i-1]);
                }
            }            
            return Value[size,Capacity];
        }
    }

    
    class Program
    {
        static void Main(string[] args)
        {
            var ks_bruteforce = new KnapSack();
            var ks_bottomup = new KnapSack();
            Console.WriteLine("Enter Capacity of KnapSack(W):");
            int Capacity = Int32.Parse(Console.ReadLine());
            Console.WriteLine("Enter Value of n:");
            int n = Int32.Parse(Console.ReadLine());
            int[] val = new int[n];
            int[] wt = new int[n];
            Random rand = new Random();            
            for (int i = 0; i < n; i++)
            {
                val[i] = rand.Next(1,20);
                wt[i] = rand.Next(1, Capacity / 2);
                Console.WriteLine("Value of" + i + "=" + val[i] + "," + "Weight of" + i + "=" + wt[i]);
            }
            Console.WriteLine("\nCapacity of KnapSack(W): {0}\n", Capacity);
            int result;
            var s_bruteforce = new Stopwatch();
            s_bruteforce.Start();
            result = ks_bruteforce.KnapSackBruteForce(n,Capacity,val,wt);
            s_bruteforce.Stop();
            Console.WriteLine(string.Format("Brute Force - Best value: {0}", result));
            var s_bottomup = new Stopwatch();
            s_bottomup.Start();
            result = ks_bottomup.KnapSackBottomUp(n, Capacity, val, wt);
            Console.WriteLine(string.Format("Bottom Up - Best value: {0}\n", result));            
            Console.WriteLine("Time taken for the BruteForce algorithm: {0}", s_bruteforce.Elapsed.ToString());
            Console.WriteLine("Time taken for the Bottomup algorithm: {0}\n", s_bottomup.Elapsed.ToString());
            Console.ReadKey();
        }
    }

}
