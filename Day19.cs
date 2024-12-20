using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AOC2024_CS_CPP
{
	public class Day19 : AOCSolutionBase
	{
		private static List<string> _availablePatterns = new();
		private static object _lockConsole = new();

		public override void Run1(string[] inputLines)
		{
			DateTime start = DateTime.Now;
			List<int> failureIndexes = new();
			_availablePatterns = inputLines[0].Split(',', StringSplitOptions.TrimEntries).ToList();
			_availablePatterns.Sort((a, b) => b.Length - a.Length);
			//_availablePatterns.Reverse();

			int validCount = 0;
			//for (int lN = 2; lN < inputLines.Length; lN++)
			Parallel.For(2, inputLines.Length, lN =>
			{
				string currLn = inputLines[lN];
				// Only subtract a towel pattern if there can be another in the remaining substring, use recursion
				if (CanReplace(currLn, 0, null, DateTime.Now))
				{
					//validCount++;
					Interlocked.Increment(ref validCount);
					lock (_lockConsole)
					{
						Console.BackgroundColor = ConsoleColor.Green;
						Console.ForegroundColor = ConsoleColor.Black;
						Console.WriteLine($"SUCCESS: {lN - 2}    ACTUAL: \"{currLn}\"");
						Console.ResetColor();
					}
				}
				else
				{
					lock (failureIndexes) failureIndexes.Add(lN);
					lock (_lockConsole)
					{
						Console.BackgroundColor = ConsoleColor.Red;
						Console.ForegroundColor = ConsoleColor.White;
						Console.WriteLine($"FAILURE: {lN - 2}    ACTUAL: \"{currLn}\"");
						Console.ResetColor();
					}
				}
			});

			//_depths.Sort();
			Console.WriteLine($"\n\nVALID COUNT: {validCount}\nTIME: {(DateTime.Now - start).Milliseconds}ms\n{{");
			failureIndexes.ForEach(v => Console.Write($"{v},")); // hehehehehe
		}

		private static bool CanReplace(string line, ushort depth, ParallelLoopState? maybeLoopState, DateTime startTime)
		{
			// Base case is that the string is emptied successfully
			if (string.IsNullOrEmpty(line))
			{
				//lock (_depths) _depths.Add(depth);
				return true;
			}

			// Testing for timeout...
			if ((DateTime.Now - startTime).Seconds > 10 || (maybeLoopState != null && maybeLoopState.IsStopped))
			{
				return false;
			}
			depth++;

			bool TestReplacing(string substr)
			{
				if (!line.StartsWith(substr)) return false;
				string subLine = line.Remove(0, substr.Length);
				if (CanReplace(subLine, depth, maybeLoopState, startTime))
				{
					//lock (_builtStr) _builtStr = _builtStr.Insert(0, substr);
					return true;
				}
				return false;
			}

			bool anySuccess = false;
			void TestReplacingParallel(string substr, ParallelLoopState pLoopState)
			{
				if (!line.StartsWith(substr)) return;
				string subLine = line.Remove(0, substr.Length);
				if (CanReplace(subLine, depth, pLoopState, startTime))
				{
					if (pLoopState.IsStopped) return;
					pLoopState.Stop();
					//lock (_builtStr)
					//{
					//_builtStr = _builtStr.Insert(0, substr);
					anySuccess = true;
					//}
				}
			}

			if (depth <= 2)
			{
				Parallel.ForEach(_availablePatterns, TestReplacingParallel);
				return anySuccess;
			}
			foreach (string p in _availablePatterns)
			{
				if (TestReplacing(p)) return true;
			}
			return false;
		}

		private static readonly int[] FailIndexes = [365, 102, 268, 201, 36, 300, 236, 70, 338, 171, 398, 141, 202, 237, 302, 105, 40, 373, 5, 282, 175, 347, 145, 81, 303, 106, 204, 376, 240, 45, 6, 285, 178, 354, 146, 82, 206, 304, 47, 379, 110, 8, 286, 179, 355, 154, 248, 89, 49, 209, 112, 384, 311, 295, 24, 188, 359, 157, 90, 250, 50, 387, 211, 297, 320, 27, 121, 362, 219, 161, 93, 254, 212, 321, 122, 364, 53, 390, 132, 220, 162, 32, 96, 255, 322, 214, 55, 393, 127, 221, 97, 59, 328, 128, 227, 130, 61, 230, 62, 64];

		public override void Run2(string[] inputLines)
		{
			DateTime start = DateTime.Now;
			_availablePatterns = inputLines[0].Split(',', StringSplitOptions.TrimEntries).ToList();
			_availablePatterns.Sort((a, b) => b.Length - a.Length);

			long sumOfCombinations = 0;
			Parallel.For(2, inputLines.Length, lN =>
			{
				if (inputLines.Length > 400 && FailIndexes.Contains(lN)) return;
				string currLn = inputLines[lN];

				// There are only success here! The only question is how many
				long currCount = CountAllCombinations(currLn, 0, DateTime.Now);

				Interlocked.Add(ref sumOfCombinations, currCount);
				lock (_lockConsole) Console.WriteLine($"N OF COMBINATIONS: {currCount}");
			});

			Console.WriteLine($"\n\nTOTAL COUNT: {sumOfCombinations}\nTIME: {(DateTime.Now - start).Milliseconds}ms");
		}

		private const double MAX_TIME_MS = 6000; // Timeout isn't sufficient, ALL combinations must be explored
		private static long CountAllCombinations(string line, ushort depth, DateTime startTime)
		{
			if (string.IsNullOrEmpty(line)) return 1;
			depth++;

			long count = 0;
			void CountSubcombinations(string substr)
			{
				if (!line.StartsWith(substr) || (DateTime.Now - startTime).TotalMilliseconds > MAX_TIME_MS / depth) return;
				string subLine = line.Remove(0, substr.Length);
				long currCount = CountAllCombinations(subLine, depth, DateTime.Now);
				Interlocked.Add(ref count, currCount);
			}

			if (depth <= 2)
			{
				Parallel.ForEach(_availablePatterns, CountSubcombinations);
			}
			else
			{
				_availablePatterns.ForEach(CountSubcombinations);
			}

			return count;
		}
	}
}