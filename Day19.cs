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
		private static List<int> _depths = new();

		public override void Run1(string[] inputLines)
		{
			DateTime start = DateTime.Now;
			_availablePatterns = inputLines[0].Split(',', StringSplitOptions.TrimEntries).ToList();
			_availablePatterns.Sort((a, b) => b.Length - a.Length);
			//_availablePatterns.Reverse();

			int validCount = 0;
			//for (int lN = 2; lN < inputLines.Length; lN++)
			Parallel.For(2, inputLines.Length, lN =>
			{
				string currLn = inputLines[lN];
				// Only subtract a towel pattern if there can be another in the remaining substring, use recursion
				if (CanReplace(currLn, 0))
				{
					//validCount++;
					Interlocked.Increment(ref validCount);
					Console.WriteLine($"SUCCESS: {lN - 2}\t\"{currLn}\"");
				}
				else
				{
					Console.WriteLine($"FAILURE: {lN - 2}\t\"{currLn}\"");
				}
			});

			_depths.Sort();
			Console.WriteLine($"\n(fail count: {inputLines.Length - 2 - validCount}\tmin/max/med. depth: {_depths.Min()} / {_depths.Max()} / {_depths[_depths.Count / 2]})\n\nVALID COUNT: {validCount}\nTIME: {(DateTime.Now - start).Milliseconds}ms");
		}

		private static bool CanReplace(string line, ushort depth)
		{
			// Base case is that the string is emptied successfully
			if (string.IsNullOrEmpty(line))
			{
				lock (_depths) _depths.Add(depth);
				return true;
			}
			depth++;

			//foreach (string substr in _availablePatterns)
			bool TestReplacingEach(string substr)
			{
				int substrIndex = line.IndexOf(substr);
				if (substrIndex == -1 || substr.Length > line.Length) return false;
				string subLine = line.Remove(substrIndex, substr.Length);
				//if (CanReplace(subLine, depth))
				//{
				//	return true;
				//}
				return CanReplace(subLine, depth);
			}


			/*if (depth >= 2)*/ return _availablePatterns.Any(TestReplacingEach);
			//return _availablePatterns.AsParallel().Any(TestReplacingEach);
		}

		public override void Run2(string[] inputLines)
		{
		}
	}
}