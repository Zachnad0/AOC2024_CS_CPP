using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZUtilLib;

namespace AOC2024_CS_CPP
{
	public class Day5 : AOCSolutionBase
	{
		public override void Run1(string[] inputLines)
		{
			List<(int lo, int hi)> rulePairs = new();

			for (int lN = 0; inputLines[lN] != ""; lN++)
			{
				string[] strs = inputLines[lN].Split('|', StringSplitOptions.TrimEntries);
				rulePairs.Add((int.Parse(strs[0]), int.Parse(strs[1])));
			}

			int sumOfCenterVals = 0;

			// For each update, determine whether it is valid by checking all rules apply
			for (int lN = rulePairs.Count + 1; lN < inputLines.Length; lN++)
			{
				bool updateIsValid = true;
				int[] currUpdate = inputLines[lN].Split(',', StringSplitOptions.TrimEntries).Select(int.Parse).ToArray();

				// Go through each element, then ensure every rule works for it.
				for (int vN = 0; vN < currUpdate.Length; vN++)
				{
					int val = currUpdate[vN];

					// Find all values that should be lower and higher
					List<int> beforeVals = rulePairs.Where(p => p.hi == val).Select(p => p.lo).ToList();
					List<int> afterVals = rulePairs.Where(p => p.lo == val).Select(p => p.hi).ToList();

					bool beforeInvalid = (vN != 0) && currUpdate[0..vN].Any(afterVals.Contains);
					bool afterInvalid = (vN != currUpdate.Length - 1) && currUpdate[(vN + 1)..^0].Any(beforeVals.Contains);

					if (beforeInvalid || afterInvalid)
					{
						updateIsValid = false;
						break;
					}
				}

				if (updateIsValid)
				{
					int value = currUpdate[currUpdate.Length / 2];
					Console.WriteLine(value);
					sumOfCenterVals += value;
				}
				//else // WHY DID I HAVE THIS HERE???????
				//{
				//	break;
				//}
			}

			Console.WriteLine($"RESULT: {sumOfCenterVals}");
		}

		private static readonly List<(int lo, int hi)> _rulePairs = new();
		public override void Run2(string[] inputLines)
		{

			for (int lN = 0; inputLines[lN] != ""; lN++)
			{
				string[] strs = inputLines[lN].Split('|', StringSplitOptions.TrimEntries);
				_rulePairs.Add((int.Parse(strs[0]), int.Parse(strs[1])));
			}

			int sumOfCenterVals = 0;

			// For each update, determine whether it is valid by checking all rules apply
			for (int lN = _rulePairs.Count + 1; lN < inputLines.Length; lN++)
			{
				bool updateIsValid = true;
				int[] currUpdate = inputLines[lN].Split(',', StringSplitOptions.TrimEntries).Select(int.Parse).ToArray();

				// Go through each element, then ensure every rule works for it.
				for (int vN = 0; vN < currUpdate.Length; vN++)
				{
					int val = currUpdate[vN];

					// Find all values that should be lower and higher
					List<int> beforeVals = _rulePairs.Where(p => p.hi == val).Select(p => p.lo).ToList();
					List<int> afterVals = _rulePairs.Where(p => p.lo == val).Select(p => p.hi).ToList();

					bool beforeInvalid = (vN != 0) && currUpdate[0..vN].Any(afterVals.Contains);
					bool afterInvalid = (vN != currUpdate.Length - 1) && currUpdate[(vN + 1)..^0].Any(beforeVals.Contains);

					if (beforeInvalid || afterInvalid)
					{
						updateIsValid = false;
						break;
					}
				}

				if (updateIsValid)
				{
					continue;
					//int value = currUpdate[currUpdate.Length / 2];
					//Console.WriteLine(value);
					//sumOfCenterVals += value;
				}

				// If the update is invalid, re-organize it
				List<int> sortedUpdate = currUpdate.ToList();
				sortedUpdate.Sort(RuleSortComparer);

				int value2 = sortedUpdate[sortedUpdate.Count / 2];
				Console.WriteLine(value2);
				sumOfCenterVals += value2;
			}

			Console.WriteLine($"RESULT: {sumOfCenterVals}");
		}

		private static int RuleSortComparer(int a, int b)
		{
			//List<int> beforeAVals = _rulePairs.Where(p => p.hi == a).Select(p => p.lo).ToList();
			//List<int> afterAVals = _rulePairs.Where(p => p.lo == a).Select(p => p.hi).ToList();

			if (_rulePairs.Contains((a, b)))
			{
				return -1;
			}
			if (_rulePairs.Contains((b, a)))
			{
				return 1;
			}
			return 0;
		}
	}
}