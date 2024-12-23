using System;
using System.Collections.Generic;
using System.Linq;
using ZUtilLib;

namespace AOC2024_CS_CPP
{
	public class Day21 : AOCSolutionBase
	{
		private static readonly Dictionary<char, Pos2D> LastKeyboardPosMap = new()
		{
			{ '7', new(0,0) },
			{ '8', new(1,0) },
			{ '9', new(2,0) },
			{ '4', new(0,1) },
			{ '5', new(1,1) },
			{ '6', new(2,1) },
			{ '1', new(0,2) },
			{ '2', new(1,2) },
			{ '3', new(2,2) },
			{ '0', new(1,3) },
			{ 'A', new(2,3) },
		};
		private static readonly Pos2D Left = new(-1, 0), Right = new(1, 0), Up = new(0, -1), Down = new(0, 1);
		private static readonly Dictionary<Pos2D, Pos2D> DirKeyboardPosMap = new() // maps direction to pressed button position, except for one which will be null.
		{
			{ Up, new(1,0) },
			{ default, new(2,0) },
			{ Left, new(0,1) },
			{ Down, new(1,1) },
			{ Right, new(2,1) },
		};
		private static readonly Dictionary<Pos2D, char> DirCharMap = new()
		{
			{ Up, '^' },
			{ Down,'v' },
			{ Left, '<' },
			{ Right, '>' },
			{ default, 'A' },
		};

		public override void Run1(string[] inputLines)
		{
			long totalComplexity = 0;
			for (int lN = 0; lN < inputLines.Length; lN++)
			{
				string line = inputLines[lN];
				Pos2D currPos = LastKeyboardPosMap['A'];
				// Convert code into first dir keypad presses
				List<Pos2D> buttonDirSequence = new();
				for (int cN = 0; cN < line.Length; cN++)
				{
					Pos2D destPos = LastKeyboardPosMap[line[cN]];
					if (destPos == currPos) // If same pos, just hit A again
					{
						buttonDirSequence.Add(default);
						continue;
					}

					Pos2D deltaPos = destPos - currPos;
					bool negX = deltaPos._x < 0, negY = deltaPos._y < 0;
					for (int x = 0; x < Math.Abs(deltaPos._x); x++)
					{
						buttonDirSequence.Add(negX ? Left : Right);
					}
					for (int y = 0; y < Math.Abs(deltaPos._y); y++)
					{
						buttonDirSequence.Add(negY ? Up : Down);
					}
					buttonDirSequence.Add(default);
					currPos = destPos;
				}

				// Now convert this keypad sequence into next keypad sequence
				List<Pos2D> nextBackSeq = GetNextDirSeq(buttonDirSequence);
				for (int n = 0; n < 1; n++)
				{
					nextBackSeq = GetNextDirSeq(nextBackSeq);
				}

				// Compute complexity
				long currComplexity = nextBackSeq.Count * int.Parse(line.FilterNumbers(false));
				totalComplexity += currComplexity;

				// Debug
				Console.Write($"COMP: {currComplexity}\t" + line + ": ");
				//buttonDirSequence.ForEach(e => Console.Write(DirCharMap[e]));
				nextBackSeq.ForEach(e => Console.Write(DirCharMap[e]));
				Console.WriteLine();
			}

			Console.WriteLine($"\nTOTAL COMPLEXITY: {totalComplexity}");
		}

		private static List<Pos2D> GetNextDirSeq(List<Pos2D> inputButtonSeq)
		{
			Pos2D currPos = DirKeyboardPosMap[default];
			List<Pos2D> outputSeq = new();
			for (int targActN = 0; targActN < inputButtonSeq.Count; targActN++)
			{
				Pos2D targetKeyPos = DirKeyboardPosMap[inputButtonSeq[targActN]];
				if (targetKeyPos == currPos)
				{
					outputSeq.Add(default);
					continue;
				}

				Pos2D deltaPos = targetKeyPos - currPos;
				bool negX = deltaPos._x < 0, negY = deltaPos._y < 0;
				for (int x = 0; x < Math.Abs(deltaPos._x); x++)
				{
					outputSeq.Add(negX ? Left : Right);
				}
				for (int y = 0; y < Math.Abs(deltaPos._y); y++)
				{
					outputSeq.Add(negY ? Up : Down);
				}
				outputSeq.Add(default);
				currPos = targetKeyPos;
			}

			return outputSeq;
		}

		public override void Run2(string[] inputLines)
		{
		}
	}
}