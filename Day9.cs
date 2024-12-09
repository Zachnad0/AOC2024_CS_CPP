using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC2024_CS_CPP
{
	public class Day9 : AOCSolutionBase
	{
		const char GAP = '.';

		public override void Run1(string[] inputLines)
		{
			// Process input into blocks
			byte[] digits = inputLines[0].Select(c => byte.Parse(c.ToString())).ToArray();
			Dictionary<int, int> blockLegthsFromIDs = new();
			List<int> gapLengths = new();

			for (int dN = 0; dN < digits.Length; dN++)
			{
				if (dN % 2 == 0)
				{
					// Even digits are blocks
					blockLegthsFromIDs.Add(dN / 2, digits[dN]);
				}
				else
				{
					// Odds are gaps
					gapLengths.Add(digits[dN]);
				}
			}

			// Create processed input string
			string processedInput = "";
			for (int i = 0; i < blockLegthsFromIDs.Count; i++)
			{
				// Insert block
				for (int n = 0; n < blockLegthsFromIDs[i]; n++)
				{
					processedInput += i.ToString();
				}

				// Insert gaps
				if (i < gapLengths.Count)
				{
					for (int n = 0; n < gapLengths[i]; n++)
					{
						processedInput += GAP;
					}
				}
			}
			processedInput = processedInput.TrimEnd(GAP);

			// Shift swap last digit with leftermost GAP, while necessary
			ulong iterC = 0;
			while (processedInput.Contains(GAP))
			{
				// Get index of rightermost digit and leftermost gap
				//char[] reverse = processedInput.Reverse().ToArray();
				//int digitIndex = reverse.Length - 1 - Array.FindIndex(reverse, char.IsDigit);
				int digitIndex = processedInput.Length - 1;
				int gapIndex = processedInput.IndexOf(GAP);
				char digitChar = processedInput[digitIndex];

				processedInput = processedInput
					.Remove(digitIndex, 1)
					.Remove(gapIndex, 1)
					.Insert(gapIndex, digitChar.ToString())
					.TrimEnd(GAP);

				if (iterC % 1000 == 0)
				{
					Console.WriteLine(iterC);
					//Console.WriteLine("\n\n");
					//Console.WriteLine(processedInput);
				}
				iterC++;
			}

			// Compute checksum
			UInt128 checkSum = 0;
			for (int i = 0; i < processedInput.Length; i++)
			{
				checkSum += (uint)i * uint.Parse(processedInput[i].ToString());
			}

			Console.WriteLine(processedInput);
			Console.WriteLine($"RESULT: {checkSum}");
		}

		public override void Run2(string[] inputLines)
		{
		}
	}
}