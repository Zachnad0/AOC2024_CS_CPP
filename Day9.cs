using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AOC2024_CS_CPP
{
	public class Day9 : AOCSolutionBase
	{
		//const char GAP = '.';

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

			// Create processed input list
			//string processedInput = "";
			List<int> processedInput = new();
			for (int i = 0; i < blockLegthsFromIDs.Count; i++)
			{
				// Insert block
				for (int n = 0; n < blockLegthsFromIDs[i]; n++)
				{
					//processedInput += i.ToString();
					//processedInput += Convert.ToChar(i);
					processedInput.Add(i);
				}

				// Insert gaps
				if (i < gapLengths.Count)
				{
					for (int n = 0; n < gapLengths[i]; n++)
					{
						//processedInput += GAP;
						processedInput.Add(-1);
					}
				}
			}
			//processedInput = processedInput.TrimEnd(GAP);
			//Console.WriteLine(processedInput);

			// Shift swap last digit with leftermost GAP, while necessary
			ulong iterC = 0;
			while (/*processedInput.Contains(GAP)*/ processedInput.Contains(-1))
			{
				// Get index of rightermost digit and leftermost gap
				//char[] reverse = processedInput.Reverse().ToArray();
				//int digitIndex = reverse.Length - 1 - Array.FindIndex(reverse, char.IsDigit);
				//int digitIndex = processedInput.Count - 1;
				int gapIndex = processedInput.IndexOf(-1);
				//int digitChar = Convert.ToInt32(processedInput[digitIndex]);
				int digit = processedInput[^1];

				//processedInput = processedInput
				//	.Remove(digitIndex, 1)
				//	.Remove(gapIndex, 1)
				//	.Insert(gapIndex, Convert.ToChar(digitChar) + "")
				//	.TrimEnd(GAP);
				processedInput.Insert(gapIndex, digit);
				processedInput.Remove(-1);
				processedInput.RemoveAt(processedInput.Count - 1);

				if (iterC % 1000 == 0)
				{
					Console.WriteLine(iterC);
					//Console.WriteLine("\n\n");
					//Console.WriteLine(processedInput);
				}
				iterC++;
			}

			// Compute checksum
			Int128 checkSum = 0;
			for (int i = 0; i < processedInput.Count; i++)
			{
				//checkSum += i * Convert.ToInt32(processedInput[i]);
				//Console.WriteLine($"{i} :: {Convert.ToInt32(processedInput[i])}");
				checkSum += i * processedInput[i];
			}

			//Console.WriteLine(processedInput);
			Console.WriteLine($"RESULT: {checkSum}");
		}

		public override void Run2(string[] inputLines)
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

			// Create processed input list
			List<int> processedInput = new();
			for (int i = 0; i < blockLegthsFromIDs.Count; i++)
			{
				// Insert block
				for (int n = 0; n < blockLegthsFromIDs[i]; n++)
				{
					processedInput.Add(i);
				}

				// Insert gaps
				if (i < gapLengths.Count)
				{
					for (int n = 0; n < gapLengths[i]; n++)
					{
						processedInput.Add(-1);
					}
				}
			}

			// Iterate through all file IDs in descending order
			//processedInput.ForEach(v => Console.Write($"{(v == -1 ? "." : v)} "));
			//Console.WriteLine();
			for (int blockID = blockLegthsFromIDs.Count - 1; blockID >= 0; blockID--)
			{
				// Try to move block of this ID
				int blockSize = blockLegthsFromIDs[blockID];
				int blockIndex = processedInput.IndexOf(blockID);

				// Search for first series of gaps that is of suitable length
				int currGapLen = 0;
				for (int i = 0; i < blockIndex; i++)
				{
					if (processedInput[i] == -1)
					{
						currGapLen++;
						// If gap large enough, move file into gap
						if (currGapLen == blockSize)
						{
							int newBlockIndex = i - blockSize + 1;
							processedInput.RemoveRange(newBlockIndex, blockSize);
							for (int n = 0; n < blockSize; n++)
							{
								processedInput.Insert(newBlockIndex, blockID);
							}
							// Replace with gaps
							processedInput.RemoveRange(blockIndex, blockSize);
							for (int n = 0; n < blockSize; n++)
							{
								processedInput.Insert(blockIndex, -1);
							}

							Console.WriteLine($"Moved block #{blockID}");
							break;
						}
					}
					else
					{
						currGapLen = 0;
					}
				}

				//processedInput.ForEach(v => Console.Write($"{(v == -1 ? "." : v)} "));
				//Console.WriteLine();
			}

			// Compute checksum
			Int128 checkSum = 0;
			for (int i = 0; i < processedInput.Count; i++)
			{
				int val = processedInput[i];
				checkSum += i * (val != -1 ? val : 0);
			}

			Console.WriteLine($"RESULT: {checkSum}");
		}
	}
}