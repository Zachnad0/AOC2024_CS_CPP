using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZUtilLib;

namespace AOC2024_CS_CPP
{
	public readonly struct Pos2D
	{
		public readonly int _x, _y;

		public Pos2D(int x, int y)
		{
			_x = x;
			_y = y;
		}

		public readonly Pos2D GetIncr(int x, int y) => new(_x + x, _y + y);
	}

	public class Day10 : AOCSolutionBase
	{
		public override void Run1(string[] inputLines)
		{
			// Input is heightmap
			byte[][] heightMap = inputLines.Select(l => l.ToCharArray().Select(c => byte.Parse(c.ToString())).ToArray()).ToArray();
			int mWdith = heightMap.Length, mHeight = heightMap[0].Length;

			long nOfTrails = 0;

			// Iterate through every possible starting point if it's a zero
			heightMap.Foreach((col, row, val) =>
			{
				if (val != 0)
				{
					return;
				}

				HashSet<Pos2D> foundValidEnds = new();
				List<Pos2D> visitedPositions = new();
				Queue<Pos2D> toVisitQueue = new();
				toVisitQueue.Enqueue(new(col, row));

				// Count number of valid paths, via a breadth-first search
				while (toVisitQueue.Count > 0)
				{
					Pos2D currPos = toVisitQueue.Dequeue();
					visitedPositions.Add(currPos);
					byte currValue = heightMap[currPos._x][currPos._y];

					// Check if 9 for end, then add no neighbours
					if (currValue == 9)
					{
						foundValidEnds.Add(currPos);
						continue;
					}

					// Add all neighbours that are exactly one value above this
					Pos2D[] adjPositions = [currPos.GetIncr(0, -1), currPos.GetIncr(0, 1), currPos.GetIncr(-1, 0), currPos.GetIncr(1, 0)];
					foreach (Pos2D pos in adjPositions)
					{
						if (!visitedPositions.Contains(pos) && IsPosInBounds(pos, mWdith, mHeight) && heightMap[pos._x][pos._y] == currValue + 1)
						{
							toVisitQueue.Enqueue(pos);
						}
					}
				}

				nOfTrails += foundValidEnds.Count;

			});

			Console.WriteLine($"RESULT: {nOfTrails}");
		}

		private static bool IsPosInBounds(Pos2D pos, int width, int height) => pos._x >= 0 && pos._x < width && pos._y >= 0 && pos._y < height;

		public override void Run2(string[] inputLines)
		{
			// Input is heightmap
			byte[][] heightMap = inputLines.Select(l => l.ToCharArray().Select(c => byte.Parse(c.ToString())).ToArray()).ToArray();
			int mWdith = heightMap.Length, mHeight = heightMap[0].Length;

			long nOfTrails = 0;

			// Iterate through every possible starting point if it's a zero
			heightMap.Foreach((col, row, val) =>
			{
				if (val != 0)
				{
					return;
				}

				//HashSet<Pos2D> foundValidEnds = new();
				//List<Pos2D> visitedPositions = new();
				Queue<Pos2D> toVisitQueue = new();
				toVisitQueue.Enqueue(new(col, row));

				// Count number of valid paths, via a breadth-first search
				while (toVisitQueue.Count > 0)
				{
					Pos2D currPos = toVisitQueue.Dequeue();
					//visitedPositions.Add(currPos);
					byte currValue = heightMap[currPos._x][currPos._y];

					// Simply the number of times we hit a 9, that is a trail
					// This works because we are never visiting anything !!!
					if (currValue == 9)
					{
						//foundValidEnds.Add(currPos);
						nOfTrails++;
						continue;
					}

					// Add all neighbours that are exactly one value above this
					Pos2D[] adjPositions = [currPos.GetIncr(0, -1), currPos.GetIncr(0, 1), currPos.GetIncr(-1, 0), currPos.GetIncr(1, 0)];
					foreach (Pos2D pos in adjPositions)
					{
						if (IsPosInBounds(pos, mWdith, mHeight) && heightMap[pos._x][pos._y] == currValue + 1)
						{
							toVisitQueue.Enqueue(pos);
						}
					}
				}

				//nOfTrails += foundValidEnds.Count;

			});

			Console.WriteLine($"RESULT: {nOfTrails}");
		}
	}
}