using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace AOC2024_CS_CPP
{
	public class Day11 : AOCSolutionBase
	{
		public override void Run1(string[] inputLines)
		{
			List<long> stones = inputLines[0].Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(long.Parse).ToList();

			const int BLINK_COUNT = 25;
			for (int bN = 0; bN < BLINK_COUNT; bN++)
			{
				Console.WriteLine($"BN: {bN},\tSC: {stones.Count}");
				for (int sN = 0; sN < stones.Count; sN++)
				{
					long currStoneVal = stones[sN];
					// Try to apply rules
					// Is 0
					if (currStoneVal == 0)
					{
						stones[sN] = 1;
						continue;
					}
					// Even # of digits
					string valStr = currStoneVal.ToString();
					if (valStr.Length % 2 == 0)
					{
						stones.RemoveAt(sN);
						// Right stone
						stones.Insert(sN, long.Parse(valStr[(valStr.Length / 2)..^0]));
						// Left stone
						stones.Insert(sN, long.Parse(valStr[0..(valStr.Length / 2)]));
						sN++;
						continue;
					}
					// Else multiply value by 2024
					stones[sN] = currStoneVal * 2024;
				}
			}

			Console.WriteLine($"STONE COUNT: {stones.Count}");
		}

		private static List<long> _concurrentStones = new(), _pendingAdditions = new();
		private static readonly object _csLock = new();
		/*
		public override void Run2(string[] inputLines)
		{
			// Order doesn't matter!!!
			List<long> stones = inputLines[0].Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(long.Parse).ToList();

			const int BLINK_COUNT = 75, BLINK_COUNT_A = 30;
			// TODO to save on memory, NON-concurrently process each stone independently and see how many it results in, then add that count to total
			long grandTotal = 0;
			foreach (long stoneStartVal in stones) // Stones don't interact with one-another !!!
			{
				_concurrentStones = [stoneStartVal];

				// Go until first blink count milestone "A"
				for (int bN = 0; bN < BLINK_COUNT_A; bN++)
				{
					//Console.WriteLine($"BN: {bN},\tSC: {_concurrentStones.Count}");

					_pendingAdditions = _concurrentStones;
					_concurrentStones = new();

					Parallel.ForEach(_pendingAdditions, ProcessStones);
				}

				Console.WriteLine($"MILESTONE A FOR {stoneStartVal} =======================================");

				// Then iterate through of those, like initially
				List<long> midpointCollection = _concurrentStones;
				for (int mSN = 0; mSN < midpointCollection.Count; mSN++)
				{
					Console.WriteLine($"STARTING SECTION B FOR {stoneStartVal}::{mSN}/{midpointCollection.Count} =======================================");

					_concurrentStones = [midpointCollection[mSN]];
					for (int bN = BLINK_COUNT_A; bN < BLINK_COUNT; bN++)
					{
						//Console.WriteLine($"BN: {bN},\tSC: {_concurrentStones.Count}");

						_pendingAdditions = _concurrentStones;
						_concurrentStones = new();

						Parallel.ForEach(_pendingAdditions, ProcessStones);
					}

					_concurrentStones.Clear();
					_pendingAdditions.Clear();
					grandTotal += _concurrentStones.Count;
				}

				Console.WriteLine($"COMPLETE FOR {stoneStartVal} =======================================");

				_concurrentStones.Clear();
				_pendingAdditions.Clear();
			}

			Console.WriteLine($"STONE COUNT: {grandTotal}");
		}
		*/

		private static void ProcessStones(long currStoneVal)
		{
			// Try to apply rules
			// Is 0
			if (currStoneVal == 0)
			{
				lock (_csLock)
				{
					_concurrentStones.Add(1);
				}
				return;
			}

			// Even # of digits
			string valStr = currStoneVal.ToString();
			if (valStr.Length % 2 == 0)
			{
				lock (_csLock)
				{
					_concurrentStones.Add(long.Parse(valStr[(valStr.Length / 2)..^0]));
					_concurrentStones.Add(long.Parse(valStr[0..(valStr.Length / 2)]));
				}
				return;
			}

			// Else multiply value by 2024
			lock (_csLock)
			{
				_concurrentStones.Add(currStoneVal * 2024);
			}
			return;
		}


		// TRYING RECURSIVELY
		public override void Run2(string[] inputLines)
		{
			List<long> stones = inputLines[0].Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(long.Parse).ToList();

			const int TOTAL_STEPS = 75;
			long grandTotal = 0;
			foreach (long val in stones)
			{
				Console.WriteLine($"STARTING: {val},\tTOTAL: {grandTotal}");
				grandTotal += CountResultingStones(val, TOTAL_STEPS);
			}

			Console.WriteLine($"TOTAL: {grandTotal}");
		}

		public static long CountResultingStones(long startVal, int steps)
		{
			// Base case, steps is 0
			if (steps == 0)
			{
				//Console.Write("[75-mark]");
				return 1;
			}

			//if (steps == 35)
			//{
			//	Console.Write("[35-mark]");
			//}

			// Otherwise perform action based on startVal, then count from there
			if (startVal == 0)
			{
				return CountResultingStones(1, steps - 1);
			}

			string valStr = startVal.ToString();
			if (valStr.Length % 2 == 0)
			{
				long a = long.Parse(valStr[(valStr.Length / 2)..^0]);
				long b = long.Parse(valStr[0..(valStr.Length / 2)]);
				return CountResultingStones(a, steps - 1) + CountResultingStones(b, steps - 1);
			}

			return CountResultingStones(startVal * 2024, steps - 1);
		}
	}
}