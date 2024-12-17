using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
		private char[][]? _maze;
		//private static readonly ParallelOptions ParOpt = new() { MaxDegreeOfParallelism = 2 };

		public override void Run1(string[] inputLines)
		{
			_maze = inputLines.LinesToCharMatrix();
			(int x, int y) startPosTuple = _maze.IndexesOf(START);

			// Use DFS to search for ALL possible routes and costs
			Pos2D startPos = new(startPosTuple.x, startPosTuple.y), startDir = new(1, 0);
			SearchForAllPaths(startPos, startDir, 0, 0).Wait();

			if (_pathCosts.Count == 0) Console.WriteLine("NO PATHS FOUND (err)");
			else Console.WriteLine($"LEAST PATH COST: {_pathCosts.Min()}");
		}

		private const int DEPTH_LIMIT = 2000, PARALLEL_DEPTH = 20;
		private async Task SearchForAllPaths(Pos2D origin, Pos2D prevDir, int currCost, int depth)
		{
			if (depth > DEPTH_LIMIT || _visited.Contains(origin) || _maze == null) return;
			if (_maze[origin._x][origin._y] == END)
			{
				// If at end, add current cost to path costs

				lock (_pathCosts) _pathCosts.Add(currCost);
				Console.WriteLine($"PATH FOUND: {currCost}\tDEPTH: {depth}");
				return;
			}

			lock (_visited) _visited.Add(origin);

			// Search all neighbours
			List<Task> pathSearchTasks = new();
			//foreach (Pos2D dir in Dirs)
			if (depth < PARALLEL_DEPTH)
			{
				await Parallel.ForEachAsync(Dirs, async (dir, a) =>
				{
					Pos2D nextPos = origin + dir;
					if (_maze[nextPos._x][nextPos._y] == WALL || _visited.Contains(nextPos)) return;
					int thisCost = currCost + ((dir == prevDir) ? 1 : 1001);
					//pathSearchTasks.Add(SearchForAllPaths(nextPos, dir, thisCost, depth + 1));
					await SearchForAllPaths(nextPos, dir, thisCost, depth + 1);
				});
			}
			else
			{
				foreach (Pos2D dir in Dirs)
				{
					Pos2D nextPos = origin + dir;
					if (_maze[nextPos._x][nextPos._y] == WALL || _visited.Contains(nextPos)) continue;
					int thisCost = currCost + ((dir == prevDir) ? 1 : 1001);
					await SearchForAllPaths(nextPos, dir, thisCost, depth + 1);
					//pathSearchTasks.Add(SearchForAllPaths(nextPos, dir, thisCost, depth + 1));
				}

				//await Task.WhenAll(pathSearchTasks);
			}


			// After visiting all neighbours, unvisit self
			lock (_visited) _visited.Remove(origin);
			return;
		}

		//private static bool IsInBounds(Pos2D pos, char[][] mat) => pos._x >= 0 && pos._x < mat.Length && pos._y >= 0 && pos._y < mat[0].Length;

		public override void Run2(string[] inputLines)
		{
		}
	}
}