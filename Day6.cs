using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using ZUtilLib;

namespace AOC2024_CS_CPP
{
	public class Day6 : AOCSolutionBase
	{
		private const char UP = '^', DOWN = 'v', LEFT = '<', RIGHT = '>', BG = '.', OBSTACLE = '#';

		public override void Run1(string[] inputLines)
		{
			char[][] origMap = inputLines.LinesToCharMatrix();
			int mWidth = origMap.Length, mHeight = origMap[0].Length;
			HashSet<(int x, int y)> visitedPositions = new();

			// Find initial guard pos
			(int col, int row) currGP = (-1, -1), nextGP = (-1, -1);
			origMap.Foreach((x, y, c) => { if (c is UP or DOWN or LEFT or RIGHT) currGP = (x, y); });
			nextGP = currGP;

			// Keep stepping forward guard until next position is offscree
			char[][] newMap = origMap.CloneJaggedMatrix();
			while (PosInBounds(mWidth, mHeight, nextGP))
			{
				// Write and move
				char dirChar = newMap[currGP.col][currGP.row];
				newMap[currGP.col][currGP.row] = BG;
				newMap[nextGP.col][nextGP.row] = dirChar;
				visitedPositions.Add(currGP);
				currGP = nextGP;

				// Figure out guard next position OR direction
				bool turnAgain = false;
				do
				{
					turnAgain = false;
					switch (newMap[currGP.col][currGP.row])
					{
						case UP:
							// Check if it can go there
							if (!PosInBounds(mWidth, mHeight, (currGP.col, currGP.row - 1)))
							{
								nextGP = (currGP.col, currGP.row - 1);
								continue;
							}
							if (origMap[currGP.col][currGP.row - 1] == OBSTACLE)
							{
								turnAgain = true;
								newMap[currGP.col][currGP.row] = RIGHT;
								break;
							}
							nextGP = (currGP.col, currGP.row - 1);
							break;

						case DOWN:
							if (!PosInBounds(mWidth, mHeight, (currGP.col, currGP.row + 1)))
							{
								nextGP = (currGP.col, currGP.row + 1);
								continue;
							}
							if (origMap[currGP.col][currGP.row + 1] == OBSTACLE)
							{
								turnAgain = true;
								newMap[currGP.col][currGP.row] = LEFT;
								break;
							}
							nextGP = (currGP.col, currGP.row + 1);
							break;

						case LEFT:
							if (!PosInBounds(mWidth, mHeight, (currGP.col - 1, currGP.row)))
							{
								nextGP = (currGP.col - 1, currGP.row);
								continue;
							}
							if (origMap[currGP.col - 1][currGP.row] == OBSTACLE)
							{
								turnAgain = true;
								newMap[currGP.col][currGP.row] = UP;
								break;
							}
							nextGP = (currGP.col - 1, currGP.row);
							break;

						case RIGHT:
							if (!PosInBounds(mWidth, mHeight, (currGP.col + 1, currGP.row)))
							{
								nextGP = (currGP.col + 1, currGP.row);
								continue;
							}
							if (origMap[currGP.col + 1][currGP.row] == OBSTACLE)
							{
								turnAgain = true;
								newMap[currGP.col][currGP.row] = DOWN;
								break;
							}
							nextGP = (currGP.col + 1, currGP.row);
							break;
					}
				} while (turnAgain);
			}

			visitedPositions.Add(currGP);
			Console.WriteLine($"RESULT SC: {visitedPositions.Count}");

			static bool PosInBounds(int mWidth, int mHeight, (int col, int row) currGP) => Math.Clamp(currGP.col, 0, mWidth - 1) == currGP.col && Math.Clamp(currGP.row, 0, mHeight - 1) == currGP.row;
		}

		public override void Run2(string[] inputLines)
		{
			// A guard is stuck when it returns to an exact position AND direction it has been to before
			char[][] origMap = inputLines.LinesToCharMatrix();
			int mWidth = origMap.Length, mHeight = origMap[0].Length, nOfLoopsMade = 0;
			object mapLock = new(), incrementLock = new();

			// Find initial guard pos
			(int col, int row) origGP = (-1, -1);
			origMap.Foreach((x, y, c) => { if (c is UP or DOWN or LEFT or RIGHT) origGP = (x, y); });

			Parallel.For(0, mWidth * mHeight, new ParallelOptions() { MaxDegreeOfParallelism = 100 }, (iterNum) =>
			{
				int newObRow = iterNum % mHeight;
				int newObCol = (iterNum / mHeight) % mWidth;
				// Early return to not waste processing time (default config has NO loops)
				if ((newObCol, newObRow) == origGP || origMap[newObCol][newObRow] == OBSTACLE)
				{
					Console.WriteLine($"POINTLESS: ({newObCol}, {newObRow})");
					return;
				}


				char[][] newMap;
				lock (mapLock)
				{
					newMap = origMap.CloneJaggedMatrix();
				}
				// Place obstacle
				newMap[newObCol][newObRow] = OBSTACLE;

				(int col, int row) nextGP, currGP;
				nextGP = currGP = origGP;
				// Keep stepping forward guard until next position is offscreen
				HashSet<(int x, int y, char dir)> visitedPosAndDirs = new();
				while (PosInBounds(mWidth, mHeight, nextGP))
				{
					// Write and move
					char dirChar = newMap[currGP.col][currGP.row];
					newMap[currGP.col][currGP.row] = BG;
					newMap[nextGP.col][nextGP.row] = dirChar;
					currGP = nextGP;

					// If in position and dir it has been in before, we are in a loop for sure
					if (!visitedPosAndDirs.Add((currGP.col, currGP.row, dirChar)))
					{
						Interlocked.Increment(ref nOfLoopsMade);
						Console.WriteLine($"LOOP: ({newObCol}, {newObRow})");
						return;
					}

					// Figure out guard next position OR direction
					bool turnAgain = false;
					do
					{
						turnAgain = false;
						switch (newMap[currGP.col][currGP.row])
						{
							case UP:
								// Check if it can go there
								if (!PosInBounds(mWidth, mHeight, (currGP.col, currGP.row - 1)))
								{
									nextGP = (currGP.col, currGP.row - 1);
									continue;
								}
								if (newMap[currGP.col][currGP.row - 1] == OBSTACLE)
								{
									turnAgain = true;
									newMap[currGP.col][currGP.row] = RIGHT;
									break;
								}
								nextGP = (currGP.col, currGP.row - 1);
								break;

							case DOWN:
								if (!PosInBounds(mWidth, mHeight, (currGP.col, currGP.row + 1)))
								{
									nextGP = (currGP.col, currGP.row + 1);
									continue;
								}
								if (newMap[currGP.col][currGP.row + 1] == OBSTACLE)
								{
									turnAgain = true;
									newMap[currGP.col][currGP.row] = LEFT;
									break;
								}
								nextGP = (currGP.col, currGP.row + 1);
								break;

							case LEFT:
								if (!PosInBounds(mWidth, mHeight, (currGP.col - 1, currGP.row)))
								{
									nextGP = (currGP.col - 1, currGP.row);
									continue;
								}
								if (newMap[currGP.col - 1][currGP.row] == OBSTACLE)
								{
									turnAgain = true;
									newMap[currGP.col][currGP.row] = UP;
									break;
								}
								nextGP = (currGP.col - 1, currGP.row);
								break;

							case RIGHT:
								if (!PosInBounds(mWidth, mHeight, (currGP.col + 1, currGP.row)))
								{
									nextGP = (currGP.col + 1, currGP.row);
									continue;
								}
								if (newMap[currGP.col + 1][currGP.row] == OBSTACLE)
								{
									turnAgain = true;
									newMap[currGP.col][currGP.row] = DOWN;
									break;
								}
								nextGP = (currGP.col + 1, currGP.row);
								break;
						}
					} while (turnAgain);
				}

				// If the guard successfully leaves, then we don't do anything.
				Console.WriteLine($"NOTHING: ({newObCol}, {newObRow})    ESCAPE AT: ({nextGP.col}, {nextGP.row})");
				return;
			});

			Console.WriteLine($"RESULT #ofPos: {nOfLoopsMade}");

			static bool PosInBounds(int mWidth, int mHeight, (int col, int row) currGP) => Math.Clamp(currGP.col, 0, mWidth - 1) == currGP.col && Math.Clamp(currGP.row, 0, mHeight - 1) == currGP.row;
		}
	}
}