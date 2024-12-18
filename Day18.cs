using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC2024_CS_CPP
{
	public class Day18 : AOCSolutionBase
	{
		private const int ENV_LENGTH = /*7*/71;
		private static readonly Pos2D[] Dirs = [new(1, 0), new(-1, 0), new(0, 1), new(0, -1)];

		public override void Run1(string[] inputLines)
		{
			// Get positions of falling bytes
			HashSet<Pos2D> bytePosList = new();
			for (int lN = 0; lN < 1024/*12*/; lN++)
			{
				int[] subVals = inputLines[lN].Split(',').Select(int.Parse).ToArray();
				bytePosList.Add(new(subVals[0], subVals[1]));
			}

			int shortestLen = GetLengthOfShortestPath(new(bytePosList), new(0, 0), new(ENV_LENGTH - 1, ENV_LENGTH - 1));
			Console.WriteLine($"RESULT: {shortestLen}");
		}

		private static int GetLengthOfShortestPath(HashSet<Pos2D> obstaclePosList, Pos2D start, Pos2D target)
		{
			if (obstaclePosList == null) return -1;
			Dictionary<Pos2D, Pos2D> previousPosMap = new(); // Use this to track backwards

			// Use BFS
			HashSet<Pos2D> visited = new();
			Queue<Pos2D> posToVisit = new();
			posToVisit.Enqueue(start);
			visited.Add(start);

			while (posToVisit.Count > 0)
			{
				Pos2D currPos = posToVisit.Dequeue();

				if (currPos == target)
				{
					// Calculate path length via map to track backwards until start
					int pathLength;
					Pos2D nextPosBack = currPos;
					for (pathLength = 0; nextPosBack != start; pathLength++)
					{
						nextPosBack = previousPosMap[nextPosBack];
					}

					return pathLength;
				}

				foreach (Pos2D dir in Dirs)
				{
					Pos2D queryPos = currPos + dir;
					if (visited.Contains(queryPos) || obstaclePosList.Contains(queryPos) || !IsInBounds(queryPos)) continue;
					posToVisit.Enqueue(queryPos);
					previousPosMap.Add(queryPos, currPos);
					visited.Add(queryPos);
				}
			}

			// Failed, then return -1
			return -1;
		}

		private static bool IsInBounds(Pos2D queryPos) => queryPos._x >= 0 && queryPos._x < ENV_LENGTH && queryPos._y >= 0 && queryPos._y < ENV_LENGTH;

		public override void Run2(string[] inputLines)
		{
			// Get positions of falling bytes
			List<Pos2D> bytePosList = new();
			for (int lN = 0; lN < inputLines.Length; lN++)
			{
				int[] subVals = inputLines[lN].Split(',').Select(int.Parse).ToArray();
				bytePosList.Add(new(subVals[0], subVals[1]));
			}

			//int shortestLen = GetLengthOfShortestPath(new(bytePosList), new(0, 0), new(ENV_LENGTH - 1, ENV_LENGTH - 1));
			//Console.WriteLine($"RESULT: {shortestLen}");

			Pos2D startPos = new(0, 0), targetPos = new(ENV_LENGTH - 1, ENV_LENGTH - 1);
			for (int n = 0; n < bytePosList.Count; n++)
			{
				HashSet<Pos2D> bytePosThusFar = new(bytePosList[0..(n + 1)]);
				int len = GetLengthOfShortestPath(bytePosThusFar, startPos, targetPos);
				if (len == -1)
				{
					Console.WriteLine($"SEALING POS: {bytePosList[n]}");
					return;
				}
				//Console.WriteLine($"LEN: {len}");
				if (n % 1000 == 0)
				{
					Console.WriteLine($"N: {n}");
				}
			}

			Console.WriteLine($"NONE FOUND...");
		}
	}
}