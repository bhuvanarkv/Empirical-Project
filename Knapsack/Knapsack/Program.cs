using System;
using System.Collections;
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
        public ArrayList opt = new ArrayList();
        /*
         * Implementation of the brute-force knapsack algorithm.
         * n is the number of items.
         * W is the capacity of the knapsack.
         * value is the values of the items.
         * wt is the weights of the items.
         * Return the value of the best optimal subset.
         * */
        public int KnapSackBruteForce(int n, int W, int[] value, int[] wt)
        {
            var bestValue = 0;
            var bestPosition = 0;

            if (n == 0 || W == 0)
                return 0;

            var n_2 = (long)Math.Pow(2, n);
            for (var i = 0; i < n_2; i++)
            {
                var total = 0;
                var weight = 0;
                for (var j = 0; j < n; j++)
                {
                    // if bit not included then skip
                    if (((i >> j) & 1) != 1) continue;

                    total += value[j];
                    weight += wt[j];
                }

                if (weight <= W && total > bestValue)
                {
                    bestPosition = i;
                    bestValue = total;
                }
            }
            BestValue = bestValue;

            return BestValue;
        }

        /*
         * Implementation of bottom-up knapsack algorithm.
         * n is the number of items.
         * W is the capacity of the knapsack.
         * value is the value of the items.
         * wt is the weight of the items.
         * Return the maximum value for this set of items and this capacity.
         * */
        public int KnapSackBottomUp(int n, int W, int[] value, int[] wt)
        {
            //Result array
            int[,] Value = new int[n + 1, W + 1];
            //Base Case
            if (n == 0 || W == 0)
                return 0;
            //Iterative Case
            for (int i = 0; i <= n; i++)
            {
                for (int j = 0; j <= W; j++)
                {
                    //Set first row and column to 0
                    if (i == 0 || j == 0)
                        Value[i, j] = 0;
                    //item i is not included
                    else if (wt[i - 1] > j)
                        Value[i, j] = Value[i - 1, j];
                    //item i is included, take max of either taking i or not taking i
                    else
                        Value[i, j] = Math.Max(Value[i - 1, j], Value[i - 1, j - wt[i - 1]] + value[i - 1]);
                }
            }
            RecoverOptimalSubset(Value, value, wt, n, W);
            return Value[n, W];
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
            //base case
            if (n == 0 || W == 0) {
                return 0;
            }
            //recursive case
            if (T[n, W] < 0)
            {
                if (W < weight[n])
                {
                    //item is not included in optimal set
                    T[n, W] = KnapSackTopDown(n - 1, W, T, weight, value);
                }
                else
                {
                    //either don't take current item or take current item
                    T[n, W] = Math.Max(KnapSackTopDown(n - 1, W, T, weight, value), value[n] + KnapSackTopDown(n - 1, W - weight[n], T, weight, value));
                }
            }
            return T[n, W];
        }

        public void RecoverOptimalSubset(int[,] T, int[] value, int[] wt, int n, int W) {
            int result = T[n, W];
            int w = W;
            for (int i = n; i > 0 && result > 0; i--) {
                if (result == T[i - 1, w])
                    continue;
                else {
                    opt.Add(i - 1);
                    result = result - value[i - 1];
                    w = w - wt[i - 1];
                }
            }
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
            Console.WriteLine(string.Format("Bottom Up - Best value: {0}", result));
            foreach ( Object obj in knapSack.opt )
                Console.Write( "   {0}", obj );
            Console.WriteLine();

            //Fill T array with -1, except first row and column which are filled with 0
            int[,] T = new int[n, W];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < W; j++)
                {
                    if (i == 0 || j == 0)
                    {
                        T[i,j] = 0;
                    }
                    else
                    {
                        T[i,j] = -1;
                    }
                }
            }
            var s_topdown = new Stopwatch();
            s_topdown.Start();
            result = knapSack.KnapSackTopDown(n - 1, W - 1, T, wt, val);
            s_topdown.Stop();
            Console.WriteLine(string.Format("Top Down - Best value: {0}\n", result));
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < W; j++)
                {
                    Console.Write(string.Format("{0} ", T[i, j]));
                }
                Console.Write(Environment.NewLine + Environment.NewLine);
            }

            //Print times for every algorithm
            Console.WriteLine("Time taken for the BruteForce algorithm: {0}", s_bruteforce.Elapsed.ToString());
            Console.WriteLine("Time taken for the Bottomup algorithm: {0}\n", s_bottomup.Elapsed.ToString());
            Console.WriteLine("Time taken for the Top Down algorithm: {0}\n", s_topdown.Elapsed.ToString());
            Console.ReadKey();
        }
    }

}
