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
            int[,] Value = new int[size + 1, Capacity + 1];
            if (size == 0 || Capacity == 0)
                return 0;
            for (int i = 0; i <= size; i++)
            {
                for (int j = 0; j <= Capacity; j++)
                {
                    if (i == 0 || j == 0)
                        Value[i, j] = 0;
                    else if (wt[i - 1] > j)
                        Value[i, j] = Value[i - 1, j];
                    else
                        Value[i, j] = Math.Max(Value[i - 1, j], Value[i - 1, j - wt[i - 1]] + value[i - 1]);
                }
            }
            return Value[size, Capacity];
        }

        /*
         * Implementation of top-down knapsack algorithm.
         * n is the number of items
         * W is the capacity of the knapsack
         * T is the table of sentinel values (initialized to -1)
         * weight is the array of weights
         * value is the array of values
         * Return T[n, W] will be the maximum possible value for this instance of the knapsack problem.
         * */
        public int KnapSackTopDown(int n, int W, int[,] T, int[] weight, int[] value)
        {
            int value;
            if (T[n, W] < 0)
            {
                if (W < weight[W])
                {
                    if (W < weight[n])
                    {
                        value = KnapSackTopDown(n - 1, W, T, weight, value);
                    }
                    else
                    {
                        value = Math.Max(KnapSackTopDown(n - 1, W, T, weight, value), value[n] + KnapSackTopDown(n - 1, W - weight[n], T, weight, value));
                    }
                }
                T[n, W] = value;
            }
            return T[n, W];
        }
    }

    
    class Program
    {
        static void Main(string[] args)
        {
            // Generate knapsack objects
            var knapSack = new KnapSack();

            //Read W and n from user.
            Console.WriteLine("Enter Capacity of KnapSack(W):");
            int W = Int32.Parse(Console.ReadLine());
            Console.WriteLine("Enter Value of n:");
            int n = Int32.Parse(Console.ReadLine());

            //Create value and weight arrays
            //Values are random in the range from 1-20, weights are random in the range from 1 to capacity / 2
            int[] val = new int[n];
            int[] wt = new int[n];
            Random rand = new Random();            
            for (int i = 0; i < n; i++)
            {
                val[i] = rand.Next(1,20);
                wt[i] = rand.Next(1, W / 2);
                Console.WriteLine("Value of" + i + "=" + val[i] + "," + "Weight of" + i + "=" + wt[i]);
            }
            Console.WriteLine("\nCapacity of KnapSack(W): {0}\n", W);
            int result;

            //Create timer for bruteforce, call brute force, stop timer, log result
            var s_bruteforce = new Stopwatch();
            s_bruteforce.Start();
            result = knapSack.KnapSackBruteForce(n,W,val,wt);
            s_bruteforce.Stop();
            Console.WriteLine(string.Format("Brute Force - Best value: {0}", result));

            //Create timer for bottom-up, call bottom-up, stop timer, log result
            var s_bottomup = new Stopwatch();
            s_bottomup.Start();
            result = knapSack.KnapSackBottomUp(n, W, val, wt);
            s_bottomup.Stop();
            Console.WriteLine(string.Format("Bottom Up - Best value: {0}\n", result));

            //Fill T array with -1, except first row and column which are filled with 0
            int[,] T = new int[n, W];
            for (int i = 0; i < T.Length; i++)
            {
                T[0][i] = 0;
                T[i][0] = 0;
            }
            for (int i = 1; i < n; i++)
            {
                for (int j = 1; j < W; j++)
                {
                    T[n][W] = -1;
                }
            }
            var s_topdown = new Stopwatch();
            s_topdown.Start();
            result = knapSack.KnapSackTopDown(n, W, T, wt, val);
            s_topdown.Stop();
            Console.WriteLine(string.Format("Top Down - Best value: {0}\n", result));

            //Print times for every algorithm
            Console.WriteLine("Time taken for the BruteForce algorithm: {0}", s_bruteforce.Elapsed.ToString());
            Console.WriteLine("Time taken for the Bottomup algorithm: {0}\n", s_bottomup.Elapsed.ToString());
            Console.WriteLine("Time taken for the Top Down algorithm: {0}\n", s_topdown.Elapsed.ToString());
            Console.ReadKey();
        }
    }

}
