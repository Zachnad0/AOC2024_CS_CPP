using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZUtilLib;

namespace AOC2024_CS_CPP
{
	public class Day16 : AOCSolutionBase
	{
		private const char START = 'S', END = 'E', WALL = '#', SPACE = '.';

		private static readonly Pos2D[] Dirs = [new(0, 1), new(1, 0), new(-1, 0), new(0, -1)];
		private readonly List<int> _pathCosts = new();
		private readonly List<Pos2D> _visited = new();
		private char[][] _maze = null;
		private static readonly ParallelOptions ParOpt = new() { MaxDegreeOfParallelism = 2 };

		public override void Run1(string[] inputLines)
		{
			_maze = inputLines.LinesToCharMatrix();
			(int x, int y) startPosTuple = _maze.IndexesOf(START);

			// Use DFS to search for ALL possible routes and costs
			Pos2D startPos = new(startPosTuple.x, startPosTuple.y), startDir = new(1, 0);
			SearchForAllPaths(startPos, startDir, 0);

			Console.WriteLine($"LEAST PATH COST: {_pathCosts.Min()}");
		}

		private void SearchForAllPaths(Pos2D origin, Pos2D prevDir, int currCost)
		{
			if (_visited.Contains(origin)) return;
			if (_maze[origin._x][origin._y] == END)
			{
				// If at end, add current cost to path costs

				lock (_pathCosts) _pathCosts.Add(currCost);
				Console.WriteLine($"PATH FOUND: {currCost}");
				return;
			}

			lock (_visited) _visited.Add(origin);

			// Search all neighbours
			//foreach (Pos2D dir in Dirs)
			Parallel.ForEach(Dirs, ParOpt, dir =>
		{
			Pos2D nextPos = origin + dir;
			//if (_visited.Contains(nextPos) || !IsInBounds(nextPos, _maze) || _maze[nextPos._x][nextPos._y] == WALL) return;
			if (_maze[nextPos._x][nextPos._y] == WALL || _visited.Contains(nextPos)) return;
			int thisCost = currCost + ((dir == prevDir) ? 1 : 1001);
			SearchForAllPaths(nextPos, dir, thisCost);
		});

			// After visiting all neighbours, unvisit self
			lock (_visited) _visited.Remove(origin);
		}

		//private static bool IsInBounds(Pos2D pos, char[][] mat) => pos._x >= 0 && pos._x < mat.Length && pos._y >= 0 && pos._y < mat[0].Length;

		public override void Run2(string[] inputLines)
		{
		}
	}
}