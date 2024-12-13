﻿using System;
using System.Linq;
using System.Collections.Generic;
using ZUtilLib;
using System.Threading.Tasks;

namespace AOC2024_CS_CPP
{
	public class Day13 : AOCSolutionBase
	{
		public readonly struct Pos2DL
		{
			public readonly long _x, _y;

			public Pos2DL(long x, long y)
			{
				_x = x;
				_y = y;
			}

			public readonly Pos2DL GetIncr(long x, long y) => new(_x + x, _y + y);
			public static double Distance(Pos2DL a, Pos2DL b) => Math.Sqrt(Math.Pow(a._x - b._x, 2) + Math.Pow(a._y - b._y, 2));
			public static bool operator ==(Pos2DL a, Pos2DL b) => (a._x == b._x) && (a._y == b._y);
			public static bool operator !=(Pos2DL a, Pos2DL b) => !(a == b);
			public static Pos2DL operator +(Pos2DL a, Pos2DL b) => new(a._x + b._x, a._y + b._y);
			public static Pos2DL operator -(Pos2DL a, Pos2DL b) => new(a._x - b._x, a._y - b._y);
			public static Pos2DL operator *(long n, Pos2DL p) => new(n * p._x, n * p._y);
			public override string ToString() => $"({_x}, {_y})";
		}

		private const int A_COST = 3, B_COST = 1;

		public override void Run1(string[] inputLines)
		{
			long totalCost = 0;

			// For each starting line of game
			for (int lN = 0; lN < inputLines.Length; lN += 4)
			{
				// Parse data for current game
				string[] buttonAStrs = inputLines[lN].Split(',', StringSplitOptions.RemoveEmptyEntries);
				Pos2D buttonAMov = new(int.Parse(buttonAStrs[0].FilterNumbers(false)), int.Parse(buttonAStrs[1].FilterNumbers(false)));
				string[] buttonBStrs = inputLines[lN + 1].Split(',', StringSplitOptions.RemoveEmptyEntries);
				Pos2D buttonBMov = new(int.Parse(buttonBStrs[0].FilterNumbers(false)), int.Parse(buttonBStrs[1].FilterNumbers(false)));
				string[] prizePosStrs = inputLines[lN + 2].Split(',', StringSplitOptions.RemoveEmptyEntries);
				Pos2D prizePos = new(int.Parse(prizePosStrs[0].FilterNumbers(false)), int.Parse(prizePosStrs[1].FilterNumbers(false)));

				List<long> successCosts = new();
				// Try hitting only B until exceeded/met prize, then try again with 1 of A, 2 of A...
				// Stop when all buttons hit last time were A
				int prevN = 1;
				for (int nOfA = 0; nOfA < prevN; nOfA++)
				{
					int currN = 0, currCost = 0;
					Pos2D clawPos = new(0, 0);

					while (clawPos._x < prizePos._x && clawPos._y < prizePos._y)
					{
						if (currN < nOfA)
						{
							currCost += A_COST;
							clawPos += buttonAMov;
						}
						else
						{
							currCost += B_COST;
							clawPos += buttonBMov;
						}

						currN++;
					}

					if (clawPos == prizePos)
					{
						successCosts.Add(currCost);
					}
					prevN = currN;
				}

				if (successCosts.Count > 0)
				{
					totalCost += successCosts.Min();
				}
				else
				{
					Console.WriteLine($"IMPOSSIBLE LINE: {lN}");
				}
			}

			Console.WriteLine($"MIN TOTAL COST: {totalCost}");
		}

		private const long PART_2_OFFSET = 10000000000000;
		private static readonly object _lock = new();

		public override void Run2(string[] inputLines)
		{
			long totalCost = 0;

			// For each starting line of game
			for (int lN = 0; lN < inputLines.Length; lN += 4)
			{
				// Parse data for current game
				string[] buttonAStrs = inputLines[lN].Split(',', StringSplitOptions.RemoveEmptyEntries);
				Pos2DL buttonAMov = new(int.Parse(buttonAStrs[0].FilterNumbers(false)), int.Parse(buttonAStrs[1].FilterNumbers(false)));
				string[] buttonBStrs = inputLines[lN + 1].Split(',', StringSplitOptions.RemoveEmptyEntries);
				Pos2DL buttonBMov = new(int.Parse(buttonBStrs[0].FilterNumbers(false)), int.Parse(buttonBStrs[1].FilterNumbers(false)));
				string[] prizePosStrs = inputLines[lN + 2].Split(',', StringSplitOptions.RemoveEmptyEntries);
				Pos2DL prizePos = new(long.Parse(prizePosStrs[0].FilterNumbers(false)) /*+ PART_2_OFFSET*/, int.Parse(prizePosStrs[1].FilterNumbers(false)) /*+ PART_2_OFFSET*/);

				List<long> successCosts = new();
				// Try hitting only B until exceeded/met prize, then try again with 1 of A, 2 of A...
				// Stop when all buttons hit last time were A
				long maxNOfA = Math.Max(prizePos._x / buttonAMov._x, prizePos._y / buttonAMov._y) + 1;
				long maxNOfB = Math.Max(prizePos._x / buttonBMov._x, prizePos._y / buttonBMov._y) + 1;
				long maxOfMaxs = Math.Max(maxNOfA, maxNOfB);
				//bool BGTA = maxNOfB > maxNOfA;

				//// Try every combination ???
				//for (long n = 0; n < maxOfMaxs; n++)
				//{
				//	long countUpper = maxOfMaxs - n;
				//	long countLower = n;
				//	if (BGTA)
				//	{
				//		// Use upper for B, lower for A
				//		if ((countUpper * buttonBMov) + (countLower * buttonAMov) == prizePos)
				//		{
				//			// Success for this config, add to costs
				//			successCosts.Add((countUpper * B_COST) + (countLower * A_COST));
				//		}
				//	}
				//	else
				//	{
				//		// Use lower for B, upper for A
				//		if ((countUpper * buttonAMov) + (countLower * buttonBMov) == prizePos)
				//		{
				//			// Success for this config, add to costs
				//			successCosts.Add((countUpper * A_COST) + (countLower * B_COST));
				//		}
				//	}
				//}

				// Literally every single combination
				//for (long aN = 0; aN < maxNOfA; aN++)
				Parallel.For(0, maxNOfA, aN =>
				{
					//for (long bN = 0; bN < maxNOfB; bN++)
					Parallel.For(0, maxNOfB, bN =>
					{
						if (bN + aN > maxOfMaxs)
						{
							return;
						}

						//Console.Write($"[{aN}, {bN}] ");

						if ((bN * buttonBMov) + (aN * buttonAMov) == prizePos)
						{
							lock (_lock)
							{
								successCosts.Add((bN * B_COST) + (aN * A_COST));
							}
						}
					});
				});

				//int prevN = 1;
				//for (int nOfA = 0; nOfA < prevN; nOfA++)
				//Console.WriteLine(maxNOfA);
				//Parallel.For(0, maxNOfA, nOfA =>
				//{
				//	int currN = 0, currCost = 0;
				//	Pos2DL clawPos = new(0, 0);

				//	while (clawPos._x < prizePos._x && clawPos._y < prizePos._y)
				//	{
				//		if (currN < nOfA)
				//		{
				//			currCost += A_COST;
				//			clawPos += buttonAMov;
				//		}
				//		else
				//		{
				//			currCost += B_COST;
				//			clawPos += buttonBMov;
				//		}

				//		currN++;
				//	}

				//	if (clawPos == prizePos)
				//	{
				//		lock (successCosts)
				//		{
				//			successCosts.Add(currCost);
				//		}
				//	}
				//	//prevN = currN;
				//});

				if (successCosts.Count > 0)
				{
					long thisCost = successCosts.Min();
					totalCost += thisCost;
					Console.WriteLine($"SUCCESS LINE: {lN}\tMIN: {thisCost}");
				}
				else
				{
					Console.WriteLine($"IMPOSSIBLE LINE: {lN}");
				}
			}

			Console.WriteLine($"MIN TOTAL COST: {totalCost}");
		}
	}
}