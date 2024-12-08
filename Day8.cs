using System;
using System.Collections.Generic;
using System.Linq;
using ZUtilLib;

namespace AOC2024_CS_CPP
{
	public class Day8 : AOCSolutionBase
	{
		const char BG = '.';

		public override void Run1(string[] inputLines)
		{
			char[][] mapMatrix = inputLines.LinesToCharMatrix();
			int mWidth = mapMatrix.Length, mHeight = mapMatrix[0].Length;
			// List of all antennas and positions
			Dictionary<char, List<(int col, int row)>> antennasByFreq = new();
			mapMatrix.Foreach((x, y, c) =>
			{
				if (c == BG)
				{
					return;
				}
				if (antennasByFreq.ContainsKey(c))
				{
					antennasByFreq[c].Add((x, y));
				}
				else
				{
					antennasByFreq.Add(c, [(x, y)]);
				}
			});

			// Iterate through all frequencies, find pairs and determine antinode positions
			HashSet<(int col, int row)> antinodePositions = new();
			foreach (char key in antennasByFreq.Keys)
			{
				// Iterate through all pairs
				List<(int col, int row)> positions = antennasByFreq[key];
				foreach ((int col, int row) p1 in positions)
				{
					IEnumerable<(int, int)> otherPos = positions.Where(p => p != p1);
					foreach ((int col, int row) p2 in otherPos)
					{
						// Determine direction and distance difference
						int dirX = p2.col - p1.col, dirY = p2.row - p1.row;
						//int distance = (int)Math.Round(Math.Sqrt(Math.Pow(dirX, 2) + Math.Pow(dirY, 2)));

						// Determine antinode positions
						// Positive from p2
						antinodePositions.Add((p2.col + dirX, p2.row + dirY));
						//antinodePositions.Add((p2.col + (dirX * distance), p2.row + (dirY * distance)));
						// Negative from p1
						antinodePositions.Add((p1.col - dirX, p1.row - dirY));
						//antinodePositions.Add((p1.col + (-dirX * distance), p1.row + (-dirY * distance)));
					}
				}
			}

			foreach (var (col, row) in antinodePositions)
			{
				Console.WriteLine($"APOS: ({col}, {row})");
			}

			// remove out of bound antinode positions
			antinodePositions.RemoveWhere((p) => p.col < 0 || p.row < 0 || p.col >= mWidth || p.row >= mHeight);

			Console.WriteLine($"RESULT: {antinodePositions.Count}");
		}

		public override void Run2(string[] inputLines)
		{
			char[][] mapMatrix = inputLines.LinesToCharMatrix();
			int mWidth = mapMatrix.Length, mHeight = mapMatrix[0].Length;
			// List of all antennas and positions
			Dictionary<char, List<(int col, int row)>> antennasByFreq = new();
			mapMatrix.Foreach((x, y, c) =>
			{
				if (c == BG)
				{
					return;
				}
				if (antennasByFreq.ContainsKey(c))
				{
					antennasByFreq[c].Add((x, y));
				}
				else
				{
					antennasByFreq.Add(c, [(x, y)]);
				}
			});

			// Iterate through all frequencies, find pairs and determine antinode positions
			HashSet<(int col, int row)> antinodePositions = new();
			foreach (char key in antennasByFreq.Keys)
			{
				// Iterate through all pairs
				List<(int col, int row)> positions = antennasByFreq[key];
				if (positions.Count <= 1)
				{
					continue;
				}

				foreach ((int col, int row) p1 in positions)
				{
					antinodePositions.Add(p1);

					IEnumerable<(int, int)> otherPos = positions.Where(p => p != p1);
					foreach ((int col, int row) p2 in otherPos)
					{
						// Determine direction and distance difference
						int dirX = p2.col - p1.col, dirY = p2.row - p1.row;

						// Determine antinode positions
						// Positive from p2
						int currX = p2.col + dirX, currY = p2.row + dirY;
						while (currX >= 0 && currX < mWidth && currY >= 0 && currY < mHeight)
						{
							antinodePositions.Add((currX, currY));
							currX += dirX;
							currY += dirY;
						}
						// Negative from p1
						//antinodePositions.Add((p1.col - dirX, p1.row - dirY));
						currX = p1.col - dirX;
						currY = p1.row - dirY;
						while (currX >= 0 && currX < mWidth && currY >= 0 && currY < mHeight)
						{
							antinodePositions.Add((currX, currY));
							currX -= dirX;
							currY -= dirY;
						}
					}
				}
			}

			// remove out of bound antinode positions
			antinodePositions.RemoveWhere((p) => p.col < 0 || p.row < 0 || p.col >= mWidth || p.row >= mHeight);

			char[][] copy = mapMatrix.CloneJaggedMatrix();
			foreach (var (col, row) in antinodePositions)
			{
				//Console.WriteLine($"APOS: ({col}, {row})");
				copy[col][row] = '#';
			}
			Console.WriteLine(copy.ToReadableString(""));

			Console.WriteLine($"RESULT: {antinodePositions.Count}");
		}
	}
}