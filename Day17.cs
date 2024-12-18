using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZUtilLib;

namespace AOC2024_CS_CPP
{
	public class Day17 : AOCSolutionBase
	{
		public override void Run1(string[] inputLines)
		{
			long regA, regB = 0, regC = 0;
			regA = long.Parse(inputLines[0].FilterNumbers(false));
			long[] program = inputLines[4].Split(',', StringSplitOptions.RemoveEmptyEntries).Select(str => long.Parse(str.FilterNumbers(false))).ToArray();

			for (long iPtr = 0; iPtr < program.Length; iPtr += 2)
			{
				long opcode = program[iPtr];
				long operand = program[iPtr + 1];
				long comboOperand = operand switch
				{
					>= 0 and <= 3 => operand,
					4 => regA,
					5 => regB,
					6 => regC,
					_ => 7,
				};

				// Switch depending on opcode
				switch (opcode)
				{
					case 0: // adv - division
						regA = (long)(regA / Math.Pow(2, comboOperand));
						break;

					case 1: // bxl - bitwise XOR 1
						regB ^= operand;
						break;

					case 2: // bst - mod
						regB = comboOperand % 8;
						break;

					case 3: // jnz - jump
						if (regA != 0) iPtr = operand - 2;
						break;

					case 4: // bxc - bitwise XOR 2
						regB ^= regC;
						break;

					case 5: // out - writes to console
						Console.Write($"{comboOperand % 8},");
						break;

					case 6: // bdv - adv 2
						regB = (long)(regA / Math.Pow(2, comboOperand));
						break;

					case 7: // cdv
						regC = (long)(regA / Math.Pow(2, comboOperand));
						break;
				}
			}

			Console.WriteLine($"\n\nREG A: {regA}\nREG B: {regB}\nREG C: {regC}");
		}

		public override void Run2(string[] inputLines)
		{
			long[] program = inputLines[4].Split(',', StringSplitOptions.RemoveEmptyEntries).Select(str => long.Parse(str.FilterNumbers(false))).ToArray();

			// While current output isn't same as program, try increment
			const int INCREMENT = 2000000;
			List<long> suitableAValues = new();
			long minVal = 499070000, maxVal = minVal + INCREMENT;
			while (suitableAValues.Count == 0)
			{
				minVal += INCREMENT;
				maxVal += INCREMENT;

				//for (iterN = 499070000; !program.SequenceEqual(currOutput); iterN++)
				Parallel.For(minVal, maxVal, iterN =>
				{
					long regA = iterN, regB = 0, regC = 0;
					List<long> currOutput = new();

					bool excessOutLength = false;
					for (long iPtr = 0; iPtr < program.Length && !excessOutLength; iPtr += 2)
					{
						long opcode = program[iPtr];
						long operand = program[iPtr + 1];
						long comboOperand = operand switch
						{
							>= 0 and <= 3 => operand,
							4 => regA,
							5 => regB,
							6 => regC,
							_ => 7,
						};

						// Switch depending on opcode
						switch (opcode)
						{
							case 0: // adv - division
								regA = (long)(regA / Math.Pow(2, comboOperand));
								break;

							case 1: // bxl - bitwise XOR 1
								regB ^= operand;
								break;

							case 2: // bst - mod
								regB = comboOperand % 8;
								break;

							case 3: // jnz - jump
								if (regA != 0) iPtr = operand - 2;
								break;

							case 4: // bxc - bitwise XOR 2
								regB ^= regC;
								break;

							case 5: // out - writes to output
									//Console.Write($"{comboOperand % 8},");
								currOutput.Add(comboOperand % 8);
								if (currOutput.Count > program.Length)
								{
									excessOutLength = true;
									Console.WriteLine($"MAX HIT: {iterN}");
								}
								break;

							case 6: // bdv - adv 2
								regB = (long)(regA / Math.Pow(2, comboOperand));
								break;

							case 7: // cdv
								regC = (long)(regA / Math.Pow(2, comboOperand));
								break;
						}

					}

					if (currOutput.SequenceEqual(program))
					{
						lock (suitableAValues) suitableAValues.Add(iterN);
						Console.WriteLine($"SUITABLE A VAL: {iterN}");
					}
				});

				Console.WriteLine($"Now trying: {minVal} - {maxVal}");
			}

			/*if (suitableAValues.Count > 0)*/
			Console.WriteLine($"REG A VAL: {suitableAValues.Min()}");
			//else Console.WriteLine("NO SUITABLE A VALUES");
		}
	}
}