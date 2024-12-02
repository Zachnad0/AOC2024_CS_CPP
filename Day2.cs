using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZUtilLib;

namespace AOC2024_CS_CPP
{
	public class Day2 : AOCSolutionBase
	{
		public override void Run1(string[] inputLines)
		{
			const uint MAX_DELTA = 3;

			// Each line is a report
			uint nOfSafe = 0;

			for (uint rN = 0; rN < inputLines.Length; rN++)
			{
				// Report is only safe if ALL increasing or ALL decreasing
				// Difference in level must be
				inputLines[rN].Trim();
				int[] levels = inputLines[rN].Split(' ', StringSplitOptions.TrimEntries).Select(int.Parse).ToArray();
				bool allDecreasing = false, isSafe = true;
				for (uint lN = 1; lN < levels.Length; lN++)
				{
					int delta = levels[lN] - levels[lN - 1];
					if (delta == 0 || Math.Abs(delta) > MAX_DELTA)
					{
						isSafe = false;
						break;
					}

					// If first level, determine decreasing or not
					if (lN == 1)
					{
						allDecreasing = delta < 0;
					}
					// Otherwise ensure match
					else
					{
						if (allDecreasing ^ delta < 0)
						{
							// Unsafe report
							isSafe = false;
							break;
						}
					}
				}

				if (isSafe)
				{
					nOfSafe++;
				}
			}

			Console.WriteLine($"Safe N: {nOfSafe}");
		}

		public override void Run2(string[] inputLines)
		{
			const uint MAX_DELTA = 3;

			// Each line is a report
			int nOfSafe = 0;

			for (int rN = 0; rN < inputLines.Length; rN++)
			{
				// Report is only safe if ALL increasing or ALL decreasing
				// Difference in level must be less than MAX_DELTA
				IEnumerable<int> levels = inputLines[rN].Trim().Split(' ', StringSplitOptions.TrimEntries).Select(int.Parse);

				bool allDecreasing, isSafe = false;
				for (int i = -1; i < levels.Count() && !isSafe; i++) // Try removing every single thing from levels and testing for it each time, until success.
				{
					List<int> currLevels = new(levels);
					if (i != -1) // Do first one normally
					{
						currLevels.RemoveAt(i);
					}

					allDecreasing = false;
					isSafe = true;
					for (int lN = 1; lN < currLevels.Count; lN++)
					{
						int delta = currLevels[lN] - currLevels[lN - 1];

						// If first level, determine decreasing or not
						if (lN == 1)
						{
							allDecreasing = delta < 0;
						}

						// Ensure delta is valid and matches
						if (delta == 0 || (allDecreasing ^ delta < 0) || Math.Abs(delta) > MAX_DELTA)
						{
							// Unsafe report
							isSafe = false;
							break;
						}
					}
				}

				if (isSafe)
				{
					nOfSafe++;
				}
			}

			Console.WriteLine($"Safe N: {nOfSafe}");
		}
	}
}
