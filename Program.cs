using System;
using System.IO;

namespace AOC2024_CS_CPP
{
	public class Program
	{
		public static void Main()
		{
			// Change daily =================

			const string INP_FILENAME = "../inputs/day11.txt";
			AOCSolutionBase currSol = new Day11();
			bool firstOrSecond = false;

			// ==============================

			string[] lines = File.ReadAllLines(INP_FILENAME);
			if (firstOrSecond)
			{
				currSol.Run1(lines);
			}
			else
			{
				currSol.Run2(lines);
			}

			Console.WriteLine("\n==== DONE ====");
			//Console.ReadKey(false);
		}
	}
}
