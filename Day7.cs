using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC2024_CS_CPP
{
	public class Day7 : AOCSolutionBase
	{
		public override void Run1(string[] inputLines)
		{
			long totalSum = 0;

			for (int lN = 0; lN < inputLines.Length; lN++)
			{
				int aLen = inputLines[lN].IndexOf(':');
				string aStr = inputLines[lN][0..aLen];
				string bStr = inputLines[lN][(aLen + 1)..^0];
				long targetVal = long.Parse(aStr);
				long[] numbers = bStr.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(long.Parse).ToArray();

				// Attempt every possible combination of '+' and '*' operators until the result == target
				bool success = false;
				uint currCombination = 0;
				while (currCombination < Math.Pow(2, numbers.Length - 1))
				{
					string binary = Convert.ToString(currCombination, 2);
					binary = binary.PadLeft(numbers.Length - 1, '0');
					// Obtain then compare result
					long result = numbers[0];
					for (int i = 0; i < binary.Length; i++)
					{
						// 0 means '+', 1 for '*'
						if (binary[i] == '0')
						{
							result += numbers[i + 1];
						}
						else
						{
							result *= numbers[i + 1];
						}
					}

					if (result == targetVal)
					{
						success = true;
						break;
					}

					currCombination++;
				}

				// If and only if success was achieved, add on value
				if (success)
				{
					totalSum += targetVal;
					Console.WriteLine($"SUCCESS: {targetVal}");
				}
			}

			Console.WriteLine($"RESULT: {totalSum}");
		}

		public override void Run2(string[] inputLines)
		{
			long totalSum = 0;

			for (int lN = 0; lN < inputLines.Length; lN++)
			{
				int aLen = inputLines[lN].IndexOf(':');
				string aStr = inputLines[lN][0..aLen];
				string bStr = inputLines[lN][(aLen + 1)..^0];
				long targetVal = long.Parse(aStr);
				long[] numbers = bStr.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(long.Parse).ToArray();

				// Attempt every possible combination of '+' and '*' operators until the result == target
				bool success = false;
				uint currCombination = 0;
				while (currCombination < Math.Pow(3, numbers.Length - 1))
				{
					string trinary = GetBase3(currCombination);
					trinary = trinary.PadLeft(numbers.Length - 1, '0');
					// Obtain then compare result
					long result = numbers[0];
					for (int i = 0; i < trinary.Length; i++)
					{
						// 0 means '+', 1 for '*', 2 is '||'
						switch (trinary[i])
						{
							case '0': // +
								result += numbers[i + 1];
								break;

							case '1': // *
								result *= numbers[i + 1];
								break;

							case '2': // || (concat)
								int maxPVal = (int)Math.Floor(Math.Log10(numbers[i + 1]));
								result *= (int)Math.Pow(10, maxPVal + 1);
								result += numbers[i + 1];
								break;
						}
					}

					if (result == targetVal)
					{
						success = true;
						break;
					}

					currCombination++;
				}

				// If and only if success was achieved, add on value
				if (success)
				{
					totalSum += targetVal;
					Console.WriteLine($"SUCCESS: {targetVal}");
				}
			}

			Console.WriteLine($"RESULT: {totalSum}");
		}

		private static string GetBase3(uint currCombination)
		{
			string result = "";
			while (currCombination > 2)
			{
				result += (currCombination % 3).ToString();
				currCombination /= 3;
			}
			result += currCombination.ToString();
			result = new(result.Reverse().ToArray());

			return result;
		}
	}
}