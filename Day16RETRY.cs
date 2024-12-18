using System.Collections.Generic;
using ZUtilLib;

namespace AOC2024_CS_CPP
{
	public class Day16RETRY : AOCSolutionBase
	{
		public override void Run1(string[] inputLines)
		{
		}

		private const char START = 'S', END = 'E', WALL = '#', SPACE = '.';
		private static readonly Pos2D[] Dirs = [new(0, 1), new(1, 0), new(-1, 0), new(0, -1)];

		public override void Run2(string[] inputLines)
		{
			HashSet<Pos2D> wallPosSet = new();
			Pos2D startPos, targetPos;
			char[][] mazeMatrix = inputLines.LinesToCharMatrix();
			mazeMatrix.Foreach((x, y, c) =>
			{
				switch (c)
				{
					case WALL: wallPosSet.Add(new(x, y)); break;
					case START: startPos = new(x, y); break;
					case END: targetPos = new(x, y); break;
				}
			});

			// Using BFS to find and record all possible routes...?
		}
	}
}