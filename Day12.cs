using System.Collections.Generic;
using ZUtilLib;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace AOC2024_CS_CPP
{
	public class Day12 : AOCSolutionBase
	{
		public override void Run1(string[] inputLines)
		{
			char[][] gardenMatrix = inputLines.LinesToCharMatrix();

			// Generate map of regions
			Dictionary<char, HashSet<Pos2D>> regionsMap = new();
			gardenMatrix.Foreach((x, y, c) =>
			{
				if (regionsMap.TryGetValue(c, out HashSet<Pos2D>? points))
				{
					points.Add(new(x, y));
				}
				else
				{
					regionsMap.Add(c, [new(x, y)]);
				}
			});

			// Perimeter # = 4 - nOfNeighbours, Cost = perimeter * nOfCells
			long totalCost = 0;
			// Compute regions in parallel
			//foreach (char region in regionsMap.Keys)
			Parallel.ForEach(regionsMap, pair =>
			{
				HashSet<Pos2D> cells = pair.Value;

				// Must identify and get cost of sub-subregions
				HashSet<HashSet<Pos2D>> subsets = new();
				foreach (Pos2D origin in cells)
				{
					HashSet<Pos2D> pendingSet = FindAllConnected(origin, cells, new());
					if (!subsets.Any(set => pendingSet.Intersect(set).Count() == pendingSet.Count))
					{
						subsets.Add(pendingSet);
					}
				}

				foreach (HashSet<Pos2D> sub in subsets)
				{
					long perimeter = 0;
					foreach (Pos2D unit in sub)
					{
						// Calculate number of neighbours
						perimeter += 4 - sub.Count(u => u != unit && Pos2D.Distance(u, unit) <= 1);
					}

					long cost = perimeter * sub.Count;
					Interlocked.Add(ref totalCost, cost);
					Console.WriteLine($"R: {pair.Key}\tP: {perimeter}\tC: {sub.Count}\nCOST: {cost}");
				}
			});

			Console.WriteLine($"TOTAL COST: {totalCost}");
		}

		private static HashSet<Pos2D> FindAllConnected(Pos2D origin, HashSet<Pos2D> superset, HashSet<Pos2D> visited)
		{
			HashSet<Pos2D> result = [origin];
			visited.Add(origin);

			if (superset.Count > 0)
			{
				foreach (Pos2D otherCell in superset)
				{
					if (!visited.Contains(otherCell) && Pos2D.Distance(origin, otherCell) <= 1)
					{
						result = result.Concat(FindAllConnected(otherCell, superset, visited)).ToHashSet();
					}
				}
			}

			return result;
		}

		public override void Run2(string[] inputLines)
		{
			char[][] gardenMatrix = inputLines.LinesToCharMatrix();

			// Generate map of regions
			Dictionary<char, HashSet<Pos2D>> regionsMap = new();
			gardenMatrix.Foreach((x, y, c) =>
			{
				if (regionsMap.TryGetValue(c, out HashSet<Pos2D>? points))
				{
					points.Add(new(x, y));
				}
				else
				{
					regionsMap.Add(c, [new(x, y)]);
				}
			});

			// Perimeter # = 4 - nOfNeighbours, Cost = perimeter * nOfCells
			long totalCost = 0;
			// Compute regions in parallel
			//foreach (char region in regionsMap.Keys)
			Parallel.ForEach(regionsMap, pair =>
			{
				HashSet<Pos2D> cells = pair.Value;

				// Must identify and get cost of sub-subregions
				HashSet<HashSet<Pos2D>> subsets = new();
				foreach (Pos2D origin in cells)
				{
					// Ignore origins already in subsets
					if (subsets.Any(s => s.Contains(origin)))
					{
						continue;
					}

					HashSet<Pos2D> pendingSet = FindAllConnected(origin, cells, new());
					subsets.Add(pendingSet);
				}

				foreach (HashSet<Pos2D> sub in subsets)
				{
					//long perimeter = 0;
					// Get fences
					Dictionary<Pos2D, HashSet<Pos2D>> fences = new();
					foreach (Pos2D unit in sub)
					{
						// Calculate perimeter cout
						//perimeter += 4 - sub.Count(u => u != unit && Pos2D.Distance(u, unit) <= 1);
						Pos2D[] adjUnits = sub.Where(u => u != unit && Pos2D.Distance(u, unit) <= 1).ToArray();
						foreach (Pos2D dir in Directions)
						{
							// If no neighbour in a dir, then add it as fence in that dir
							if (!adjUnits.Contains(unit + dir))
							{
								if (fences.TryGetValue(dir, out HashSet<Pos2D>? already))
								{
									already.Add(unit);
								}
								else
								{
									fences.Add(dir, [unit]);
								}
							}
						}
					}

					// Compute sides, through regions of connection in each dir
					long sideCount = 0;
					foreach (Pos2D dir in Directions)
					{
						if (!fences.TryGetValue(dir, out HashSet<Pos2D>? fencesInDir))
						{
							continue;
						}

						// Get subsets of connected fence regions within this direction
						HashSet<HashSet<Pos2D>> fencesInDirPartitions = new();
						foreach (Pos2D origin in fencesInDir)
						{
							// Ignore origins already in subsets
							if (fencesInDirPartitions.Any(s => s.Contains(origin)))
							{
								continue;
							}

							HashSet<Pos2D> pendingSet = FindAllConnected(origin, fencesInDir, new());
							fencesInDirPartitions.Add(pendingSet);
						}

						sideCount += fencesInDirPartitions.Count;
					}

					//long cost = perimeter * sub.Count;
					long cost = sideCount * sub.Count; // TODO
					Interlocked.Add(ref totalCost, cost);

					Console.WriteLine($"R: {pair.Key}\tP: {sideCount}\tC: {sub.Count}\nCOST: {cost}");
				}
			});

			Console.WriteLine($"TOTAL COST: {totalCost}");
		}

		private static readonly Pos2D[] Directions = [new(0, -1), new(0, 1), new(-1, 0), new(1, 0)];
	}
}