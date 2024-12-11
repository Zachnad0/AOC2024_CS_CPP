using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Concurrent;

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

		public override void Run2(string[] inputLines)
		{
			// Order doesn't matter!!!
			ConcurrentBag<uint> stones = new(inputLines[0].Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(uint.Parse));
			//stones.Capacity = 1000000;
			//IList syncStones = ArrayList.Synchronized(stones);
			//ParallelOptions parallelOptions = new() { MaxDegreeOfParallelism = 50000 };

			const int BLINK_COUNT = 75;
			// TODO to save on memory, non-concurrently process each stone independently and see how many it results in, then add that count to total
			for (int bN = 0; bN < BLINK_COUNT; bN++)
			{
				Console.WriteLine($"BN: {bN},\tSC: {stones.Count}");
				DateTime preCopy = DateTime.Now;
				ConcurrentBag<uint> pendingAdditions = new(stones);
				Console.WriteLine($"CopyTime: {(DateTime.Now - preCopy).TotalMilliseconds}");
				stones.Clear();

				Parallel.For(0, pendingAdditions.Count, /*parallelOptions,*/ i =>
				{
					// Take a stone from the bag
					if (!pendingAdditions.TryTake(out uint currStoneVal))
					{
						return;
					}

					// Try to apply rules
					// Is 0
					if (currStoneVal == 0)
					{
						//stones[sN] = 1;
						stones.Add(1);
						return;
					}

					// Even # of digits
					string valStr = currStoneVal.ToString();
					if (valStr.Length % 2 == 0)
					{
						//stones.RemoveAt(sN);
						//// Right stone
						//stones.Insert(sN, long.Parse(valStr[(valStr.Length / 2)..^0]));
						//// Left stone
						//stones.Insert(sN, long.Parse(valStr[0..(valStr.Length / 2)]));
						//sN++;

						stones.Add(uint.Parse(valStr[(valStr.Length / 2)..^0]));
						stones.Add(uint.Parse(valStr[0..(valStr.Length / 2)]));
						return;
					}

					// Else multiply value by 2024
					//stones[sN] = currStoneVal * 2024;
					stones.Add(currStoneVal * 2024);
				});

			}

			Console.WriteLine($"STONE COUNT: {stones.Count}");
		}
	}
}