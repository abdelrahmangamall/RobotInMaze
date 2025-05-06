using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace Problem
{
    // *****************************************
    // DON'T CHANGE CLASS OR FUNCTION NAME
    // YOU CAN ADD FUNCTIONS IF YOU NEED TO
    // *****************************************
    public static class PROBLEM_CLASS
    {
        #region YOUR CODE IS HERE 

        public enum SOLUTION_TYPE { NAIVE, EFFICIENT };
        public static SOLUTION_TYPE solType = SOLUTION_TYPE.EFFICIENT;

        //Your Code is Here:
        //==================
        /// <summary>
        /// find the total number of possible paths to move the robot from the bottm-left corner to the top-right corner
        /// </summary>
        /// <param name="grid">n x m grid wth obstacles marked as 'x'</param>
        /// <returns>total number of possible paths</returns>
        public static long RequiredFunction(char[,] grid)
        {
            int rows = grid.GetLength(0);
            int cols = grid.GetLength(1);

            long[,] dp = new long[rows, cols];

            if (grid[rows - 1, 0] == 'x')
                return 0;

            dp[rows - 1, 0] = 1;

            for (int i = rows - 1; i >= 0; i--)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (grid[i, j] == 'x') continue;

                    if (i - 1 >= 0 && grid[i - 1, j] != 'x')
                        dp[i - 1, j] += dp[i, j];

                    if (j + 1 < cols && grid[i, j + 1] != 'x')
                        dp[i, j + 1] += dp[i, j];

                    if (i - 1 >= 0 && j + 1 < cols && grid[i - 1, j + 1] != 'x')
                        dp[i - 1, j + 1] += dp[i, j];
                }
            }

            return dp[0, cols - 1];
        }
        #endregion
    }
}
