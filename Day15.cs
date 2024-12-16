using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using ZUtilLib;
using static AOC2024_CS_CPP.Day13;

namespace AOC2024_CS_CPP
{
	public class Day15 : AOCSolutionBase
	{
		private const char UP = '^', DOWN = 'v', LEFT = '<', RIGHT = '>', ROBOT = '@', BOX = 'O', WALL = '#', SPACE = '.';
		private static readonly Dictionary<char, Pos2DL> DirMap = new() {
			{ UP, new(0, -1) },
			{ DOWN, new(0, 1)},
			{ LEFT, new(-1, 0)},
			{ RIGHT, new(1, 0) }
		};

		public override void Run1(string[] inputLines)
		{
			int mapHeight = Array.IndexOf(inputLines, "");
			int mapWidth = inputLines[0].Length;

			// Parse map and instructions
			char[][] mapMatrix = inputLines[0..mapHeight].LinesToCharMatrix();
			char[][] workingMatrix = mapMatrix.CloneJaggedMatrix();
			char[] instructions = inputLines[(mapHeight + 1)..^0].SelectMany(l => l.ToCharArray()).ToArray();

			// Iterate through and perform all instructions for robot
			(int x, int y) robotStartPos = mapMatrix.IndexesOf(ROBOT);
			Pos2DL currRobotPos = new(robotStartPos.x, robotStartPos.y);
			for (int iN = 0; iN < instructions.Length; iN++)
			{
				// Try to perform movement
				Pos2DL currDir = DirMap[instructions[iN]];
				Pos2DL nextRobotPos = currRobotPos + currDir;
				switch (workingMatrix[nextRobotPos._x][nextRobotPos._y])
				{
					case WALL:
						// Skip movement, thus don't advance robot position
						continue;

					case SPACE:
						// Do movement, nothing special
						break;

					case BOX:
						// Move all adjacent boxes in dir if possible, otherwise don't move
						if (!TryMoveBoxes(nextRobotPos, currDir, workingMatrix)) continue;
						break;
				}

				// Move robot
				workingMatrix[currRobotPos._x][currRobotPos._y] = SPACE;
				workingMatrix[nextRobotPos._x][nextRobotPos._y] = ROBOT;
				currRobotPos = nextRobotPos;
			}

			// Calculate scores from pos
		}

		private static bool TryMoveBoxes(Pos2DL pos, Pos2DL dir, char[][] map)
		{
			// If can move OR this space is free, return true
			if (map[pos._x][pos._y] == SPACE) return true;
			Pos2DL movToPos = pos + dir;
			// If it can't move and free this space, return false
			if (map[pos._x][pos._y] == WALL || !TryMoveBoxes(movToPos, dir, map)) return false;

			// Otherwise move this box, as all ahead were moved successfully/space is free
			map[pos._x][pos._y] = SPACE;
			map[movToPos._x][movToPos._y] = BOX;
			return true;
		}

		public override void Run2(string[] inputLines)
		{
		}
	}

	public static class TempUtils
	{
		// TODO Implement into ZUtilLib
		public static (int col, int row) IndexesOf<T>(this T[][] matrix, T? match) /*where T : IEquatable<T>*/
		{
			if (match == null || matrix == null) return default;
			int w = matrix.Length;
			for (int cN = 0; cN < w; cN++)
			{
				if (matrix[cN] == null) break;
				int h = matrix[cN].Length;
				for (int rN = 0; rN < h; rN++)
				{
					T? val = matrix[cN][rN];
					if (val == null) break;
					if (val.Equals(match)) return (cN, rN);
				}
			}
			return default;
		}
	}
}