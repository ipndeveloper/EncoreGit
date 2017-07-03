using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace NetSteps.Encore.Core.Tests
{
	[TestClass]
	public class PerformanceBaselineTests
	{
		static Random __rand = new Random(Environment.TickCount);

		[TestMethod]
		public void LoopPerformanceBaseline()
		{
			// The purpose of this test is to prove the strategy that I use for 
			// unrolling loops (I call it "switch unroll") indeed yields a 
			// performance gain as compared to standard loops such as foreach.

			var iterations = int.Parse("10,000,000", NumberStyles.AllowThousands);
			var arrays = new List<int[]>();
			for (int i = 0; i < 20; i++)
			{
				arrays.Add(NewIntArray(__rand.Next(20)));
			}

			var fe = TimeAndRecordIterationStrategy("ForEachOverArrayOfInt", iterations, arrays, ForEachOverArrayOfIntAsClassLevelVariable);
			TimeAndRecordIterationStrategy("ForOverArrayOfIntWithStandardLoopControl", iterations, arrays, ForOverArrayOfIntWithStandardLoopControl);
			TimeAndRecordIterationStrategy("ForOverArrayOfIntWithExternalLoopControl", iterations, arrays, ForOverArrayOfIntWithExternalLoopControl);
			var fee = TimeAndRecordIterationStrategy("ForEachOverEnumerableOfInt", iterations, arrays, ForEachOverEnumerableOfInt);
			var su = TimeAndRecordIterationStrategy("SwitchUnrolled", iterations, arrays, SwitchUnrolled);

			var improvement = ((fe - su) / fe) * 100;

			Console.WriteLine(String.Concat("Switch unrolling yields an improvement of: ", improvement, "% over std foreach over array"));
			Console.WriteLine(String.Concat("Switch unrolling yields an improvement of: ", ((fee - su) / fee) * 100, "% over std foreach over IEnumerable"));
		}

		[TestMethod]
		public void LoopPerformanceBaseline_ElementCountDoesntInvokeLoopInSwitchUnrolled()
		{
			var iterations = int.Parse("10,000,000", NumberStyles.AllowThousands);
			var arrays = new List<int[]>();
			for (int i = 0; i < 20; i++)
			{
				arrays.Add(NewIntArray(__rand.Next(20)));
			}

			var fe = TimeAndRecordIterationStrategy("ForEachOverArrayOfInt", iterations, arrays, ForEachOverArrayOfIntAsClassLevelVariable);
			TimeAndRecordIterationStrategy("ForOverArrayOfIntWithStandardLoopControl", iterations, arrays, ForOverArrayOfIntWithStandardLoopControl);
			TimeAndRecordIterationStrategy("ForOverArrayOfIntWithExternalLoopControl", iterations, arrays, ForOverArrayOfIntWithExternalLoopControl);
			var fee = TimeAndRecordIterationStrategy("ForEachOverEnumerableOfInt", iterations, arrays, ForEachOverEnumerableOfInt);
			var su = TimeAndRecordIterationStrategy("SwitchUnrolled", iterations, arrays, SwitchUnrolled);

			var improvement = ((fe - su) / fe) * 100;

			Console.WriteLine(String.Concat("Switch unrolling yields an improvement of: ", improvement, "% over std foreach over array"));
			Console.WriteLine(String.Concat("Switch unrolling yields an improvement of: ", ((fee - su) / fee) * 100, "% over std foreach over IEnumerable"));
		}

		private int[] NewIntArray(int n)
		{
			var a = new int[n];
			for (int i = 0; i < n; i++)
			{
				a[i] = __rand.Next(100);
			}
			return a;
		}

		double TimeAndRecordIterationStrategy(string name, int iterations, List<int[]> values, Func<int[], int> strategy)
		{
			var ts = Stopwatch.StartNew();
			var c = values.Count;
			for (int i = 0; i < iterations; i++)
			{
				strategy(values[i % c]);
			}
			ts.Stop();
			Console.WriteLine(String.Concat(name, ": ", ((double)(ts.Elapsed.TotalMilliseconds * 1000 * 1000) / iterations).ToString("0.00 ns")));
			return ts.Elapsed.TotalMilliseconds;
		}
		double TimeAndRecordIterationStrategy(string name, int iterations, List<int[]> values, Func<IEnumerable<int>, int> strategy)
		{
			var ts = Stopwatch.StartNew();
			var c = values.Count;
			for (int i = 0; i < iterations; i++)
			{
				strategy(values[i % c]);
			}
			ts.Stop();
			Console.WriteLine(String.Concat(name, ": ", ((double)(ts.Elapsed.TotalMilliseconds * 1000 * 1000) / iterations).ToString("0.00 ns")));
			return ts.Elapsed.TotalMilliseconds;
		}

		int ForEachOverArrayOfIntAsClassLevelVariable(int[] values)
		{
			// Access the field directly in the foreach expression.
			int result = 0;
			foreach (int value in values)
			{
				result += value;
			}
			return result;
		}

		int ForOverArrayOfIntWithStandardLoopControl(int[] values)
		{
			int result = 0;
			for (int i = 0; i < values.Length; i++)
			{
				result += values[i];
			}
			return result;
		}

		int ForOverArrayOfIntWithExternalLoopControl(int[] values)
		{
			int result = 0;
			var vlen = values.Length;
			for (int i = 0; i < vlen; i++)
			{
				result += values[i];
			}
			return result;
		}

		int ForEachOverEnumerableOfInt(IEnumerable<int> values)
		{
			int result = 0;
			foreach (var value in values)
			{
				result += value;
			}
			return result;
		}

		int SwitchUnrolled(int[] values)
		{
			int result = 0;
			var vlen = values.Length;
			switch (vlen)
			{
				case 0:
					break;
				case 1:
					result = values[0];
					break;
				case 2:
					result = values[0] + values[1];
					break;
				case 3:
					result = values[0] + values[1] + values[2];
					break;
				case 4:
					result = values[0] + values[1] + values[2] + values[3];
					break;
				case 5:
					result = values[0] + values[1] + values[2] + values[3] + values[4];
					break;
				case 6:
					result = values[0] + values[1] + values[2] + values[3] + values[4] + values[5];
					break;
				case 7:
					result = values[0] + values[1] + values[2] + values[3] + values[4] + values[5] + values[6];
					break;
				case 8:
					result = values[0] + values[1] + values[2] + values[3] + values[4] + values[5] + values[6] + values[7];
					break;
				case 9:
					result = values[0] + values[1] + values[2] + values[3] + values[4] + values[5] + values[6] + values[7] + values[8];
					break;
				default:
					result = values[0] + values[1] + values[2] + values[3] + values[4] + values[5] + values[6] + values[7] + values[8] + values[9];
					if (vlen > 10)
					{
						for (int i = 10; i < vlen; i++)
						{
							result += values[i];
						}
					}
					break;
			}
			return result;
		}
	}
}
