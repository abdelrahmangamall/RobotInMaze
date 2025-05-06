using Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace Problem
{

    public class Problem : ProblemBase, IProblem
    {
        #region ProblemBase Methods
        public override string ProblemName { get { return "RobotInMaze"; } }

        public override void TryMyCode()
        {
            /* WRITE 4~6 DIFFERENT CASES FOR TRACE, EACH WITH
             * 1) SMALL INPUT SIZE
             * 2) RETURNED indices
             * 4) PRINT THE CASE 
             */
            int N = 0, M = 0;
            long result, expected;

            // No Path
            {
                N = 4;
                M = 4;
                char[,] grid = {
                                { '.', '.', 'x', '.' },
                                { '.', '.', 'x', 'x' },
                                { '.', '.', '.', '.' },
                                { '.', '.', '.', '.' },
                            };
                result = PROBLEM_CLASS.RequiredFunction(grid);
                expected = 0;
                PrintCase(N, M, grid, result, expected);
            }
            // Single Path
            {
                N = 4;
                M = 4;
                char[,] grid = {
                                { 'x', '.', '.', '.' },
                                { '.', 'x', '.', '.' },
                                { '.', 'x', '.', '.' },
                                { '.', 'x', '.', '.' },
                            };
                result = PROBLEM_CLASS.RequiredFunction(grid);
                expected = 1;
                PrintCase(N, M, grid, result, expected);
            }

            // Multiple Paths
            {
                N = 5;
                M = 4;
                char[,] grid = {
                                { '.', 'x', '.', '.' },
                                { 'x', '.', '.', '.' },
                                { '.', 'x', 'x', '.' },
                                { '.', 'x', 'x', '.' },
                                { '.', 'x', 'x', '.' },
                            };
                result = PROBLEM_CLASS.RequiredFunction(grid);
                expected = 4;
                PrintCase(N, M, grid, result, expected);
            }

            // Multiple Paths
            {
                N = 4;
                M = 4;
                char[,] grid = {
                                { '.', '.', '.', '.' },
                                { '.', 'x', 'x', '.' },
                                { '.', '.', 'x', '.' },
                                { '.', '.', '.', '.' },
                            };
                result = PROBLEM_CLASS.RequiredFunction(grid);
                expected = 4;
                PrintCase(N, M, grid, result, expected);
            }
        }

        Thread tstCaseThr;
        bool caseTimedOut ;
        bool caseException;

        protected override void RunOnSpecificFile(string fileName, HardniessLevel level, int timeOutInMillisec)
        {
            /* READ THE TEST CASES FROM THE SPECIFIED FILE, FOR EACH CASE DO:
             * 1) READ ITS INPUT & EXPECTED OUTPUT
             * 2) READ ITS EXPECTED TIMEOUT LIMIT (IF ANY)
             * 3) CALL THE FUNCTION ON THE GIVEN INPUT USING THREAD WITH THE GIVEN TIMEOUT 
             * 4) CHECK THE OUTPUT WITH THE EXPECTED ONE
             */

            int testCases;
            int N = 0, M = 0;
            long expected = -1;
            char[,] grid = null;

            Stream s = new FileStream(fileName, FileMode.Open);
            BinaryReader br = new BinaryReader(s);

            testCases = br.ReadInt32();

            int totalCases = testCases;
            int correctCases = 0;
            int wrongCases = 0;
            int timeLimitCases = 0;
            bool readTimeFromFile = false;
            if (timeOutInMillisec == -1)
            {
                readTimeFromFile = true;
            }
            int i = 1;
            while (testCases-- > 0)
            {
                N = br.ReadInt32();
                M = br.ReadInt32();
                expected = br.ReadInt64();
                grid = new char[N, M];
                for (int k = 0; k < N; k++)
                    for (int j = 0; j < M; j++)
                        grid[k, j] = br.ReadChar();

                //Console.WriteLine("N = {0}, Res = {1}", N, actualResult);
                long result = -1;
                caseTimedOut = true;
                caseException = false;
                Stopwatch sw = null;
                {
                    tstCaseThr = new Thread(() =>
                    {
                        try
                        {
                            sw = Stopwatch.StartNew();
                            result = PROBLEM_CLASS.RequiredFunction(grid);
                            sw.Stop();
                        }
                        catch
                        {
                            caseException = true;
                        }
                        caseTimedOut = false;
                    });

                    if (readTimeFromFile)
                    {
                        timeOutInMillisec = br.ReadInt32();
                    }
                    /*LARGE TIMEOUT FOR SAMPLE CASES TO ENSURE CORRECTNESS ONLY*/
                    if (level == HardniessLevel.Easy)
                    {
                        timeOutInMillisec = 100; //Large Value 
                    }
                    /*=========================================================*/
                    tstCaseThr.Start();
                    tstCaseThr.Join(timeOutInMillisec);
                }
                Console.WriteLine($"N = {N}, M = {M}, result = ({result}), expected = ({expected}), time in ms = {sw.ElapsedMilliseconds}, timeout = {timeOutInMillisec}");

                if (caseTimedOut)       //Timedout
                {
                    Console.WriteLine("Time Limit Exceeded in Case {0}.", i);
                    tstCaseThr.Abort();
                    timeLimitCases++;
                }
                else if (caseException) //Exception 
                {
                    Console.WriteLine("Exception in Case {0}.", i);
                    wrongCases++;
                }
                else if (CheckOutput(result, expected))    //Passed
                {
                    Console.WriteLine("Test Case {0} Passed!", i);
                    correctCases++;
                }
                else                    //WrongAnswer
                {
                    Console.WriteLine("Wrong Answer in Case {0}.", i);
                    wrongCases++;
                }

                i++;
            }
            s.Close();
            br.Close();
            Console.WriteLine();
            Console.WriteLine("# correct = {0}", correctCases);
            Console.WriteLine("# time limit = {0}", timeLimitCases);
            Console.WriteLine("# wrong = {0}", wrongCases);
            Console.WriteLine("\nFINAL EVALUATION (%) = {0}", Math.Round((float)correctCases / totalCases * 100, 0));
        }

       protected override void OnTimeOut(DateTime signalTime)
        {
        }

        /// <summary>
        /// Generate a file of test cases according to the specified params
        /// </summary>
        /// <param name="level">Easy or Hard</param>
        /// <param name="numOfCases">Required number of cases</param>
        /// <param name="includeTimeInFile">specify whether to include the expected time for each case in the file or not</param>
        /// <param name="timeFactor">factor to be multiplied by the actual time</param>
        public override void GenerateTestCases(HardniessLevel level, int numOfCases, bool includeTimeInFile = false, float timeFactor = 1)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Helper Methods
        private static void PrintCase(int N, int M, char[,] matrix, long result, long expected)
        {
            /* PRINT THE FOLLOWING
             * 1) INPUT
             * 2) RETURNED indices
             * 3) WHETHER IT'S CORRECT OR WRONG
             * */
            Console.WriteLine("N: {0}, M: {1}", N, M);

            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < M; j++)
                    Console.Write(matrix[i, j] + "\t");
                Console.WriteLine();
            }
            Console.WriteLine("Output {0}, Expected {1}", result, expected);
            
            Console.WriteLine("Your answer is: {0}", (CheckOutput(result, expected)) ? "CORRECT" : "WRONG");
        }
        private static bool CheckOutput(long result, long expected)
        {
            if (result == expected)
                return true;
            else
                return false;
        }

        #endregion

    }
}
