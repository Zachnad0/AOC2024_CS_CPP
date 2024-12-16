using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using static AOC2024_CS_CPP.Day13; // Well, TIL
using ZUtilLib;
using System.Drawing;
using System.IO;

namespace AOC2024_CS_CPP
{
	public class Day14 : AOCSolutionBase
	{
		private class Robot
		{
			public const int ZONE_WIDTH = 101 /*11*/, ZONE_HEIGHT = 103 /*7*/;
			public Pos2DL Position { get; private set; }
			public Pos2DL Velocity { get; init; }

			public Robot(int startLeft, int startTop, int velX, int velY)
			{
				Position = new(startLeft, startTop);
				Velocity = new(velX, velY);
			}

			public void MoveRobot()
			{
				Pos2DL movingTo = Position + Velocity;
				if (movingTo._x < 0)
				{
					movingTo += new Pos2DL(ZONE_WIDTH, 0);
				}
				else if (movingTo._x >= ZONE_WIDTH)
				{
					movingTo -= new Pos2DL(ZONE_WIDTH, 0);
				}

				if (movingTo._y < 0)
				{
					movingTo += new Pos2DL(0, ZONE_HEIGHT);
				}
				else if (movingTo._y >= ZONE_HEIGHT)
				{
					movingTo -= new Pos2DL(0, ZONE_HEIGHT);
				}

				Position = movingTo;
			}
		}

		public override void Run1(string[] inputLines)
		{
			// Acquire and parse robots
			List<Robot> allRobots = new();
			for (int lN = 0; lN < inputLines.Length; lN++)
			{
				string[] valStrs = inputLines[lN].Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
				int val1 = int.Parse(valStrs[0].Remove(0, 2));
				int val2 = int.Parse(valStrs[1]);
				int val3 = int.Parse(valStrs[2].Remove(0, 2));
				int val4 = int.Parse(valStrs[3]);

				Robot newRob = new(val1, val2, val3, val4);
				allRobots.Add(newRob);
			}

			// Move robots in ticks
			const int TICK_MAX = 100;
			for (int t = 0; t < TICK_MAX; t++)
			{
				allRobots.ForEach(r => r.MoveRobot());
			}

			// Count quadrants
			long topLefC = 0, botLefC = 0, topRigC = 0, botRigC = 0;
			foreach (Robot robot in allRobots)
			{
				Pos2DL robPos = robot.Position;
				// Left
				if (robPos._x < Robot.ZONE_WIDTH / 2)
				{
					if (robPos._y < Robot.ZONE_HEIGHT / 2)
					{
						topLefC++;
					}
					else if (robPos._y > Robot.ZONE_HEIGHT / 2)
					{
						botLefC++;
					}
				}
				else if (robPos._x > Robot.ZONE_WIDTH / 2)
				{
					if (robPos._y > Robot.ZONE_HEIGHT / 2)
					{
						botRigC++;
					}
					else if (robPos._y < Robot.ZONE_HEIGHT / 2)
					{
						topRigC++;
					}
				}
			}

			long safetyFactor = topLefC * botLefC * topRigC * botRigC;
			Console.WriteLine($"RESULT: {safetyFactor}");
		}

		public override void Run2(string[] inputLines)
		{
			// Acquire and parse robots
			List<Robot> allRobots = new();
			for (int lN = 0; lN < inputLines.Length; lN++)
			{
				string[] valStrs = inputLines[lN].Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
				int val1 = int.Parse(valStrs[0].Remove(0, 2));
				int val2 = int.Parse(valStrs[1]);
				int val3 = int.Parse(valStrs[2].Remove(0, 2));
				int val4 = int.Parse(valStrs[3]);

				Robot newRob = new(val1, val2, val3, val4);
				allRobots.Add(newRob);
			}

			// Move robots in ticks
			long ticks = 0;
			Directory.CreateDirectory("../inputs/temp/");
			//while (!RobotsInPosition(allRobots))
			for (int i = 0; i < 10000; i++)
			{
				allRobots.ForEach(r => r.MoveRobot());
				ticks++;

				// Write bitmap
				//Bitmap image = new(Robot.ZONE_WIDTH, Robot.ZONE_HEIGHT);
				//foreach (Robot robot in allRobots)
				//{
				//	image.SetPixel((int)robot.Position._x, (int)robot.Position._y, Color.White);
				//}

				//image.Save($"../inputs/temp/img-{ticks}.bmp");

				if (i % 1000 == 0)
				{
					Console.WriteLine(i.ToString());
				}
			}
			/*
			// Count quadrants
			//long topLefC = 0, botLefC = 0, topRigC = 0, botRigC = 0;
			foreach (Robot robot in allRobots)
			{
				Pos2DL robPos = robot.Position;
				// Left
				if (robPos._x < Robot.ZONE_WIDTH / 2)
				{
					if (robPos._y < Robot.ZONE_HEIGHT / 2)
					{
						topLefC++;
					}
					else if (robPos._y > Robot.ZONE_HEIGHT / 2)
					{
						botLefC++;
					}
				}
				else if (robPos._x > Robot.ZONE_WIDTH / 2)
				{
					if (robPos._y > Robot.ZONE_HEIGHT / 2)
					{
						botRigC++;
					}
					else if (robPos._y < Robot.ZONE_HEIGHT / 2)
					{
						topRigC++;
					}
				}
			}

			long safetyFactor = topLefC * botLefC * topRigC * botRigC;
			Console.WriteLine($"RESULT: {safetyFactor}");
			*/

			//Console.WriteLine($"COUNT: {tick}");
		}

		//private static bool RobotsInPosition(List<Robot> robots)
		//{
		//}
	}
}