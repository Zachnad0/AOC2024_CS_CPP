using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Text.RegularExpressions;
using System.Reflection.Metadata.Ecma335;

namespace AOC2024_CS_CPP
{
	public class Day3 : AOCSolutionBase
	{
		public override void Run1(string[] inputLines)
		{
			string input = "";
			for (int i = 0; i < inputLines.Length; i++)
			{
				input += inputLines[i].Replace(" ", "").ToLower(); // Flatten
			}

			// For each character, advance FSM
			long totalSum = 0;
			for (int cN = 0; cN < input.Length - 3; cN++)
			{
				if (input.Substring(cN, 4) != "mul(")
				{
					continue;
				}

				int firstVal = 0, secondVal = 0;
				int a = 0, b = 0;

				while (int.TryParse(input[cN + 4 + a].ToString(), out int outN))
				{
					firstVal += outN;
					firstVal *= 10;
					a++;
				}
				firstVal /= 10;

				// Comma, then right character set
				if (input[cN + 4 + a] != ',')
				{
					continue;
				}

				while (int.TryParse(input[cN + 5 + a + b].ToString(), out int outN))
				{
					secondVal += outN;
					secondVal *= 10;
					b++;
				}
				secondVal /= 10;

				if (input[cN + 5 + a + b] != ')')
				{
					continue;
				}

				totalSum += firstVal * secondVal;
			}

			Console.WriteLine($"Total: {totalSum}");
		}


		private bool _readMulOn = true;
		public override void Run2(string[] inputLines)
		{
			string input = "";
			for (int i = 0; i < inputLines.Length; i++)
			{
				input += inputLines[i].Replace(" ", "").ToLower(); // Flatten
			}

			// For each character, advance FSM
			long totalSum = 0;
			for (int cN = 0; cN < input.Length - 3; cN++)
			{
				if (!_readMulOn)
				{
					Console.WriteLine("DON'T");
					// Check for read mul on
					if (input.Substring(cN, 4) == "do()")
					{
						_readMulOn = true;
					}
					continue;
				}

				// Else check for read mul off
				if (cN < input.Length - 6 && input.Substring(cN, 7) == "don't()")
				{
					Console.WriteLine("DO");
					_readMulOn = false;
					continue;
				}

				// Else do standard reading
				if (input.Substring(cN, 4) != "mul(")
				{
					continue;
				}

				int firstVal = 0, secondVal = 0;
				int a = 0, b = 0;

				while (int.TryParse(input[cN + 4 + a].ToString(), out int outN))
				{
					firstVal += outN;
					firstVal *= 10;
					a++;
				}
				firstVal /= 10;

				// Comma, then right character set
				if (input[cN + 4 + a] != ',')
				{
					continue;
				}

				while (int.TryParse(input[cN + 5 + a + b].ToString(), out int outN))
				{
					secondVal += outN;
					secondVal *= 10;
					b++;
				}
				secondVal /= 10;

				if (input[cN + 5 + a + b] != ')')
				{
					continue;
				}

				totalSum += firstVal * secondVal;
			}

			Console.WriteLine($"Total: {totalSum}");
		}
	}
}