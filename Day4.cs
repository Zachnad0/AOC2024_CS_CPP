using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZUtilLib;

namespace AOC2024_CS_CPP
{
	public class Day4 : AOCSolutionBase
	{
		//private const string WORD = "XMAS";

		private List<(int col, int row)> GetNeighboursThatMatch(char[][] matrix, int col, int row, char match)
		{
			List<(int, int)> result = new();

			for (int x = -1; x <= 1; x++)
			{
				if (col + x < 0 || col + x >= matrix.Length)
				{
					continue;
				}

				for (int y = -1; y <= 1; y++)
				{
					if ((y == 0 && x == 0) || row + y < 0 || row + y >= matrix[col + x].Length)
					{
						continue;
					}

					if (matrix[col + x][row + y] == match)
					{
						result.Add((col + x, row + y));
					}
				}
			}

			return result;
		}

		public override void Run1(string[] inputLines)
		{
			char[][] searchMatrix = inputLines.LinesToCharMatrix();
			long totalSum = 0;

			// Do a search in each possible direction, at each point
			for (int cN = 0; cN < searchMatrix.Length; cN++)
			{
				int currColLen = searchMatrix[cN].Length;
				for (int rN = 0; rN < currColLen; rN++)
				{
					int localSum = 0;

					if (searchMatrix[cN][rN] != 'X')
					{
						continue;
					}

					// Get direction difference from this to matching neighbours
					// Then we can continue in only those directions
					List<(int col, int row)> nMs = GetNeighboursThatMatch(searchMatrix, cN, rN, 'M');
					List<(int x, int y)> dirs = nMs.Select(p => (p.col - cN, p.row - rN)).ToList();

					// For each direction, look at next two chars along and check for 'A','S" match
					foreach ((int dirX, int dirY) in dirs)
					{
						// First char 'A'
						int currCol = cN + (dirX * 2);
						int currRow = rN + (dirY * 2);
						if (currCol < 0 || currCol >= searchMatrix.Length || currRow < 0 || currRow >= currColLen || searchMatrix[currCol][currRow] != 'A')
						{
							continue;
						}

						// Second char 'S'
						currCol = cN + (dirX * 3);
						currRow = rN + (dirY * 3);
						if (currCol < 0 || currCol >= searchMatrix.Length || currRow < 0 || currRow >= currColLen || searchMatrix[currCol][currRow] != 'S')
						{
							continue;
						}
						localSum++;
					}

					Console.WriteLine(localSum);
					totalSum += localSum;
				}
			}

			Console.WriteLine($"TOTAL: {totalSum}");
		}

		private List<(int col, int row)> GetDiagNeighboursThatMatch(char[][] matrix, int col, int row, char match)
		{
			List<(int, int)> result = new();

			for (int x = -1; x <= 1; x += 2)
			{
				if (col + x < 0 || col + x >= matrix.Length)
				{
					continue;
				}

				for (int y = -1; y <= 1; y += 2)
				{
					if (row + y < 0 || row + y >= matrix[col + x].Length)
					{
						continue;
					}

					if (matrix[col + x][row + y] == match)
					{
						result.Add((col + x, row + y));
					}
				}
			}

			return result;
		}

		public override void Run2(string[] inputLines)
		{
			char[][] searchMatrix = inputLines.LinesToCharMatrix();
			long totalSum = 0;

			// Do a search in each possible direction, at each point
			for (int cN = 0; cN < searchMatrix.Length; cN++)
			{
				int currColLen = searchMatrix[cN].Length;
				for (int rN = 0; rN < currColLen; rN++)
				{
					if (searchMatrix[cN][rN] != 'A')
					{
						continue;
					}

					// Get direction difference from this to matching diagonal neighbours
					// Then we can continue in only those directions
					List<(int col, int row)> nMs = GetDiagNeighboursThatMatch(searchMatrix, cN, rN, 'M');
					List<(int col, int row)> nSs = GetDiagNeighboursThatMatch(searchMatrix, cN, rN, 'S');
					if (nMs.Count != 2 || nSs.Count != 2)
					{
						continue;
					}
					List<(int x, int y)> dirsM = nMs.Select(p => (p.col - cN, p.row - rN)).ToList();
					List<(int x, int y)> dirsS = nSs.Select(p => (p.col - cN, p.row - rN)).ToList();

					// Now check that all M dirs are opposite to S dirs
					bool allMatchAny = dirsM.All(d1 => dirsS.Contains((-d1.x, -d1.y)));

					if (allMatchAny)
					{
						totalSum++;
						Console.WriteLine($"Found at: ({cN},{rN})");
					}
				}
			}

			Console.WriteLine($"TOTAL: {totalSum}");
		}
	}
}