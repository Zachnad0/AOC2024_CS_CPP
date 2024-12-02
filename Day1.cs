using System;
using System.Collections.Generic;
using System.Linq;
using ZUtilLib;

namespace AOC2024_CS_CPP
{
	public class Day1 : AOCSolutionBase
	{
		public override void Run1(string[] inputLines)
		{
			// Take each side as lists, sort them, then take sum of differences per index
			List<int> left = new(), right = new();
			for (int lN = 0; lN < inputLines.Length; lN++)
			{
				IEnumerable<int> nPair = inputLines[lN].Trim().Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).Select(int.Parse);

				left.Add(nPair.First());
				right.Add(nPair.Last());
			}

			left.Sort();
			right.Sort();

			long sumOfDiffs = 0;
			for (int lN = 0; lN < inputLines.Length; lN++)
			{
				sumOfDiffs += Math.Abs(left[lN] - right[lN]);
			}

			Console.WriteLine($"Result: {sumOfDiffs}");
		}

		public override void Run2(string[] inputLines)
		{
			// Take each side as lists, sort them, then take sum of differences per index
			List<int> left = new(), right = new();
			for (int lN = 0; lN < inputLines.Length; lN++)
			{
				IEnumerable<int> nPair = inputLines[lN].Trim().Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).Select(int.Parse);

				left.Add(nPair.First());
				right.Add(nPair.Last());
			}

			long simScore = 0;
			foreach (int n in left)
			{
				simScore += n * right.Count(n.Equals);
			}

			Console.WriteLine($"Result: {simScore}");
		}
	}
}