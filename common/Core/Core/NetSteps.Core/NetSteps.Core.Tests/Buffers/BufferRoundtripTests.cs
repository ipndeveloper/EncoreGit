using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Encore.Core.Buffers;
using NetSteps.Encore.Core.IoC;
using NetSteps.Encore.Core.Wireup;

namespace NetSteps.Encore.Core.Tests.Buffers
{
	[TestClass]
	public class BuffersTests
	{
		[TestInitialize]
		public void Init()
		{
		}

		[TestMethod]
		public void AccessBufferReader_Default()
		{
			IBufferReader reader = BufferReader.Create();
			Assert.IsNotNull(reader);
		}

		[TestMethod]
		public void AccessBufferWriter_Default()
		{
			IBufferWriter writer = BufferWriter.Create();
			Assert.IsNotNull(writer);
		}

		[TestMethod]
		public void BufferRoundtrip_Bool()
		{
			IBufferWriter writer = BufferWriter.Create();
			IBufferReader reader = BufferReader.Create();
			DataGenerator gen = new DataGenerator();
			var test = new
			{
				Items = 200,
				SizeOfItem = sizeof(bool),
				Next = new Func<bool>(() => gen.GetBoolean()),
				Recorded = new Queue<bool>()
			};

			byte[] buffer = new byte[test.Items * test.SizeOfItem];
			var writeCursor = 0;
			for (int i = 0; i < test.Items; i++)
			{
				var captureCursor = writeCursor;
				var value = test.Next();
				test.Recorded.Enqueue(value);

				var written = writer.Write(buffer, ref writeCursor, value);

				Assert.AreEqual<int>(test.SizeOfItem, written, String.Concat("should have written ", test.SizeOfItem));
				Assert.AreEqual<int>(written, writeCursor - captureCursor, "cursor should have incremented equal to the number of bytes written");
			}

			var readCursor = 0;
			while (test.Recorded.Count > 0)
			{
				var captureCursor = readCursor;
				var value = test.Recorded.Dequeue();
				var bufferedValue = reader.ReadBoolean(buffer, ref readCursor);

				Assert.AreEqual<int>(test.SizeOfItem, readCursor - captureCursor, "cursor should have incremented equal to the number of bytes written");
				Assert.AreEqual(value, bufferedValue, "value read from buffer should be the same as the value we recorded");
			}
		}

		[TestMethod]
		public void BufferRoundtrip_Byte()
		{
			IBufferWriter writer = BufferWriter.Create();
			IBufferReader reader = BufferReader.Create();
			DataGenerator gen = new DataGenerator(reader);
			var test = new
			{
				Items = 4000,
				SizeOfItem = sizeof(byte),
				Next = new Func<byte>(() => gen.GetByte()),
				Recorded = new Queue<byte>()
			};

			byte[] buffer = new byte[test.Items * test.SizeOfItem];
			var writeCursor = 0;
			for (int i = 0; i < test.Items; i++)
			{
				var captureCursor = writeCursor;
				var value = test.Next();
				test.Recorded.Enqueue(value);

				var written = writer.Write(buffer, ref writeCursor, value);

				Assert.AreEqual<int>(test.SizeOfItem, written, String.Concat("should have written ", test.SizeOfItem));
				Assert.AreEqual<int>(written, writeCursor - captureCursor, "cursor should have incremented equal to the number of bytes written");
			}

			var readCursor = 0;
			while (test.Recorded.Count > 0)
			{
				var captureCursor = readCursor;
				var value = test.Recorded.Dequeue();
				var bufferedValue = reader.ReadByte(buffer, ref readCursor);

				Assert.AreEqual<int>(test.SizeOfItem, readCursor - captureCursor, "cursor should have incremented equal to the number of bytes written");
				Assert.AreEqual(value, bufferedValue, "value read from buffer should be the same as the value we recorded");
			}
		}

		[TestMethod]
		public void BufferRoundtrip_ByteArray()
		{
			IBufferWriter writer = BufferWriter.Create();
			IBufferReader reader = BufferReader.Create();
			DataGenerator gen = new DataGenerator(reader);
			Random rand = new Random(Environment.TickCount);

			var test = new
			{
				Items = 4000,
				SizeOfItem = 4000,
				Next = new Func<byte[]>(() => gen.GetBytes(rand.Next(4000))),
				Recorded = new Queue<byte[]>()
			};

			byte[] buffer = new byte[test.Items * (test.SizeOfItem * sizeof(Int32))];
			var writeCursor = 0;
			for (int i = 0; i < test.Items; i++)
			{
				var captureCursor = writeCursor;
				var value = test.Next();
				test.Recorded.Enqueue(value);

				// Write a length prefix so we know how big the arrays are comming back out...
				var written = writer.Write(buffer, ref writeCursor, value.Length);
				written += writer.Write(buffer, ref writeCursor, value, 0, value.Length);
				Assert.AreEqual<int>(written, writeCursor - captureCursor, "cursor should have incremented equal to the number of bytes written");
			}

			var readCursor = 0;
			while (test.Recorded.Count > 0)
			{
				var captureCursor = readCursor;
				var value = test.Recorded.Dequeue();
				var byteCount = reader.ReadInt32(buffer, ref readCursor);
				Assert.AreEqual(value.Length, byteCount, "value read from buffer should be the same as the value we recorded");

				var bufferedValue = reader.ReadBytes(buffer, ref readCursor, byteCount);
				Assert.IsTrue(value.EqualsOrItemsEqual(bufferedValue), "value read from buffer should be the same as the value we recorded");
			}
		}

		[TestMethod]
		public void BufferRoundtrip_ByteArrayPart()
		{
			IBufferWriter writer = BufferWriter.Create();
			Assert.IsNotNull(writer);
			IBufferReader reader = BufferReader.Create();
			Assert.IsNotNull(reader);

			Random rand = new Random(Environment.TickCount);
			int count = 2000;
			int cursor = 0;
			byte[] buffer = new byte[200];
			byte[] wvalue = new byte[count];
			rand.NextBytes(wvalue);
			int start = rand.Next(wvalue.Length - buffer.Length);
			writer.Write(buffer, ref cursor, wvalue, start, buffer.Length);
			Assert.AreEqual<int>(cursor, buffer.Length);

			cursor = 0;

			byte[] rvalue = reader.ReadBytes(buffer, ref cursor, buffer.Length);
			Assert.AreEqual<int>(cursor, buffer.Length);
			for (int i = 0; i < buffer.Length; i++)
			{
				Assert.AreEqual<byte>(wvalue[i + start], rvalue[i]);
			}
		}

		[TestMethod]
		public void BufferRoundtrip_Char()
		{
			IBufferWriter writer = BufferWriter.Create();
			IBufferReader reader = BufferReader.Create();
			DataGenerator gen = new DataGenerator(reader);
			var test = new
			{
				Items = 4000,
				SizeOfItem = sizeof(char),
				Next = new Func<char>(() => gen.GetChar()),
				Recorded = new Queue<char>()
			};

			byte[] buffer = new byte[test.Items * test.SizeOfItem];
			var writeCursor = 0;
			for (int i = 0; i < test.Items; i++)
			{
				var captureCursor = writeCursor;
				var value = test.Next();
				test.Recorded.Enqueue(value);

				var written = writer.Write(buffer, ref writeCursor, value);

				Assert.AreEqual<int>(test.SizeOfItem, written, String.Concat("should have written ", test.SizeOfItem));
				Assert.AreEqual<int>(written, writeCursor - captureCursor, "cursor should have incremented equal to the number of bytes written");
			}

			var readCursor = 0;
			while (test.Recorded.Count > 0)
			{
				var captureCursor = readCursor;
				var value = test.Recorded.Dequeue();
				var bufferedValue = reader.ReadChar(buffer, ref readCursor);

				Assert.AreEqual<int>(test.SizeOfItem, readCursor - captureCursor, "cursor should have incremented equal to the number of bytes written");
				Assert.AreEqual(value, bufferedValue, "value read from buffer should be the same as the value we recorded");
			}

		}

		[TestMethod]
		public void BufferRoundtrip_Int16()
		{
			IBufferWriter writer = BufferWriter.Create();
			IBufferReader reader = BufferReader.Create();
			DataGenerator gen = new DataGenerator(reader);
			var test = new
			{
				Items = 4000,
				SizeOfItem = sizeof(Int16),
				Next = new Func<Int16>(() => gen.GetInt16()),
				Recorded = new Queue<Int16>()
			};

			byte[] buffer = new byte[test.Items * test.SizeOfItem];
			var writeCursor = 0;
			for (int i = 0; i < test.Items; i++)
			{
				var captureCursor = writeCursor;
				var value = test.Next();
				test.Recorded.Enqueue(value);

				var written = writer.Write(buffer, ref writeCursor, value);

				Assert.AreEqual<int>(test.SizeOfItem, written, String.Concat("should have written ", test.SizeOfItem));
				Assert.AreEqual<int>(written, writeCursor - captureCursor, "cursor should have incremented equal to the number of bytes written");
			}

			var readCursor = 0;
			while (test.Recorded.Count > 0)
			{
				var captureCursor = readCursor;
				var value = test.Recorded.Dequeue();
				var bufferedValue = reader.ReadInt16(buffer, ref readCursor);

				Assert.AreEqual<int>(test.SizeOfItem, readCursor - captureCursor, "cursor should have incremented equal to the number of bytes written");
				Assert.AreEqual(value, bufferedValue, "value read from buffer should be the same as the value we recorded");
			}
		}

		[TestMethod]
		public void BufferRoundtrip_UInt16()
		{
			IBufferWriter writer = BufferWriter.Create();
			IBufferReader reader = BufferReader.Create();
			DataGenerator gen = new DataGenerator(reader);
			var test = new
			{
				Items = 4000,
				SizeOfItem = sizeof(UInt16),
				Next = new Func<UInt16>(() => gen.GetUInt16()),
				Recorded = new Queue<UInt16>()
			};

			byte[] buffer = new byte[test.Items * test.SizeOfItem];
			var writeCursor = 0;
			for (int i = 0; i < test.Items; i++)
			{
				var captureCursor = writeCursor;
				var value = test.Next();
				test.Recorded.Enqueue(value);

				var written = writer.Write(buffer, ref writeCursor, value);

				Assert.AreEqual<int>(test.SizeOfItem, written, String.Concat("should have written ", test.SizeOfItem));
				Assert.AreEqual<int>(written, writeCursor - captureCursor, "cursor should have incremented equal to the number of bytes written");
			}

			var readCursor = 0;
			while (test.Recorded.Count > 0)
			{
				var captureCursor = readCursor;
				var value = test.Recorded.Dequeue();
				var bufferedValue = reader.ReadUInt16(buffer, ref readCursor);

				Assert.AreEqual<int>(test.SizeOfItem, readCursor - captureCursor, "cursor should have incremented equal to the number of bytes written");
				Assert.AreEqual(value, bufferedValue, "value read from buffer should be the same as the value we recorded");
			}
		}

		[TestMethod]
		public void BufferRoundtrip_Int32()
		{
			IBufferWriter writer = BufferWriter.Create();
			IBufferReader reader = BufferReader.Create();
			DataGenerator gen = new DataGenerator(reader);
			var test = new
			{
				Items = 4000,
				SizeOfItem = sizeof(Int32),
				Next = new Func<Int32>(() => gen.GetInt32()),
				Recorded = new Queue<Int32>()
			};

			byte[] buffer = new byte[test.Items * test.SizeOfItem];
			var writeCursor = 0;
			for (int i = 0; i < test.Items; i++)
			{
				var captureCursor = writeCursor;
				var value = test.Next();
				test.Recorded.Enqueue(value);

				var written = writer.Write(buffer, ref writeCursor, value);

				Assert.AreEqual<int>(test.SizeOfItem, written, String.Concat("should have written ", test.SizeOfItem));
				Assert.AreEqual<int>(written, writeCursor - captureCursor, "cursor should have incremented equal to the number of bytes written");
			}

			var readCursor = 0;
			while (test.Recorded.Count > 0)
			{
				var captureCursor = readCursor;
				var value = test.Recorded.Dequeue();
				var bufferedValue = reader.ReadInt32(buffer, ref readCursor);

				Assert.AreEqual<int>(test.SizeOfItem, readCursor - captureCursor, "cursor should have incremented equal to the number of bytes written");
				Assert.AreEqual(value, bufferedValue, "value read from buffer should be the same as the value we recorded");
			}
		}

		[TestMethod]
		public void BufferRoundtrip_UInt32()
		{
			IBufferWriter writer = BufferWriter.Create();
			IBufferReader reader = BufferReader.Create();
			DataGenerator gen = new DataGenerator(reader);
			var test = new
			{
				Items = 4000,
				SizeOfItem = sizeof(UInt32),
				Next = new Func<UInt32>(() => gen.GetUInt32()),
				Recorded = new Queue<UInt32>()
			};

			byte[] buffer = new byte[test.Items * test.SizeOfItem];
			var writeCursor = 0;
			for (int i = 0; i < test.Items; i++)
			{
				var captureCursor = writeCursor;
				var value = test.Next();
				test.Recorded.Enqueue(value);

				var written = writer.Write(buffer, ref writeCursor, value);

				Assert.AreEqual<int>(test.SizeOfItem, written, String.Concat("should have written ", test.SizeOfItem));
				Assert.AreEqual<int>(written, writeCursor - captureCursor, "cursor should have incremented equal to the number of bytes written");
			}

			var readCursor = 0;
			while (test.Recorded.Count > 0)
			{
				var captureCursor = readCursor;
				var value = test.Recorded.Dequeue();
				var bufferedValue = reader.ReadUInt32(buffer, ref readCursor);

				Assert.AreEqual<int>(test.SizeOfItem, readCursor - captureCursor, "cursor should have incremented equal to the number of bytes written");
				Assert.AreEqual(value, bufferedValue, "value read from buffer should be the same as the value we recorded");
			}
		}

		[TestMethod]
		public void BufferRoundtrip_Int64()
		{
			IBufferWriter writer = BufferWriter.Create();
			IBufferReader reader = BufferReader.Create();
			DataGenerator gen = new DataGenerator(reader);
			var test = new
			{
				Items = 4000,
				SizeOfItem = sizeof(Int64),
				Next = new Func<Int64>(() => gen.GetInt64()),
				Recorded = new Queue<Int64>()
			};

			byte[] buffer = new byte[test.Items * test.SizeOfItem];
			var writeCursor = 0;
			for (int i = 0; i < test.Items; i++)
			{
				var captureCursor = writeCursor;
				var value = test.Next();
				test.Recorded.Enqueue(value);

				var written = writer.Write(buffer, ref writeCursor, value);

				Assert.AreEqual<int>(test.SizeOfItem, written, String.Concat("should have written ", test.SizeOfItem));
				Assert.AreEqual<int>(written, writeCursor - captureCursor, "cursor should have incremented equal to the number of bytes written");
			}

			var readCursor = 0;
			while (test.Recorded.Count > 0)
			{
				var captureCursor = readCursor;
				var value = test.Recorded.Dequeue();
				var bufferedValue = reader.ReadInt64(buffer, ref readCursor);

				Assert.AreEqual<int>(test.SizeOfItem, readCursor - captureCursor, "cursor should have incremented equal to the number of bytes written");
				Assert.AreEqual(value, bufferedValue, "value read from buffer should be the same as the value we recorded");
			}
		}

		[TestMethod]
		public void BufferRoundtrip_UInt64()
		{
			IBufferWriter writer = BufferWriter.Create();
			IBufferReader reader = BufferReader.Create();
			DataGenerator gen = new DataGenerator(reader);
			var test = new
			{
				Items = 4000,
				SizeOfItem = sizeof(UInt64),
				Next = new Func<UInt64>(() => gen.GetUInt64()),
				Recorded = new Queue<UInt64>()
			};

			byte[] buffer = new byte[test.Items * test.SizeOfItem];
			var writeCursor = 0;
			for (int i = 0; i < test.Items; i++)
			{
				var captureCursor = writeCursor;
				var value = test.Next();
				test.Recorded.Enqueue(value);

				var written = writer.Write(buffer, ref writeCursor, value);

				Assert.AreEqual<int>(test.SizeOfItem, written, String.Concat("should have written ", test.SizeOfItem));
				Assert.AreEqual<int>(written, writeCursor - captureCursor, "cursor should have incremented equal to the number of bytes written");
			}

			var readCursor = 0;
			while (test.Recorded.Count > 0)
			{
				var captureCursor = readCursor;
				var value = test.Recorded.Dequeue();
				var bufferedValue = reader.ReadUInt64(buffer, ref readCursor);

				Assert.AreEqual<int>(test.SizeOfItem, readCursor - captureCursor, "cursor should have incremented equal to the number of bytes written");
				Assert.AreEqual(value, bufferedValue, "value read from buffer should be the same as the value we recorded");
			}
		}

		[TestMethod]
		public void BufferRoundtrip_Decimal()
		{
			IBufferWriter writer = BufferWriter.Create();
			IBufferReader reader = BufferReader.Create();
			DataGenerator gen = new DataGenerator(reader);
			var test = new
			{
				Items = 4000,
				SizeOfItem = sizeof(Decimal),
				Next = new Func<Decimal>(() => gen.GetDecimal()),
				Recorded = new Queue<Decimal>()
			};

			byte[] buffer = new byte[test.Items * test.SizeOfItem];
			var writeCursor = 0;
			for (int i = 0; i < test.Items; i++)
			{
				var captureCursor = writeCursor;
				var value = test.Next();
				test.Recorded.Enqueue(value);

				var written = writer.Write(buffer, ref writeCursor, value);

				Assert.AreEqual<int>(test.SizeOfItem, written, String.Concat("should have written ", test.SizeOfItem));
				Assert.AreEqual<int>(written, writeCursor - captureCursor, "cursor should have incremented equal to the number of bytes written");
			}

			var readCursor = 0;
			while (test.Recorded.Count > 0)
			{
				var captureCursor = readCursor;
				var value = test.Recorded.Dequeue();
				var bufferedValue = reader.ReadDecimal(buffer, ref readCursor);

				Assert.AreEqual<int>(test.SizeOfItem, readCursor - captureCursor, "cursor should have incremented equal to the number of bytes written");
				Assert.AreEqual(value, bufferedValue, "value read from buffer should be the same as the value we recorded");
			}
		}

		[TestMethod]
		public void BufferRoundtrip_Double()
		{
			IBufferWriter writer = BufferWriter.Create();
			IBufferReader reader = BufferReader.Create();
			DataGenerator gen = new DataGenerator(reader);
			var test = new
			{
				Items = 4000,
				SizeOfItem = sizeof(Double),
				Next = new Func<Double>(() => gen.GetDouble()),
				Recorded = new Queue<Double>()
			};

			byte[] buffer = new byte[test.Items * test.SizeOfItem];
			var writeCursor = 0;
			for (int i = 0; i < test.Items; i++)
			{
				var captureCursor = writeCursor;
				var value = test.Next();
				test.Recorded.Enqueue(value);

				var written = writer.Write(buffer, ref writeCursor, value);

				Assert.AreEqual<int>(test.SizeOfItem, written, String.Concat("should have written ", test.SizeOfItem));
				Assert.AreEqual<int>(written, writeCursor - captureCursor, "cursor should have incremented equal to the number of bytes written");
			}

			var readCursor = 0;
			while (test.Recorded.Count > 0)
			{
				var captureCursor = readCursor;
				var value = test.Recorded.Dequeue();
				var bufferedValue = reader.ReadDouble(buffer, ref readCursor);

				Assert.AreEqual<int>(test.SizeOfItem, readCursor - captureCursor, "cursor should have incremented equal to the number of bytes written");
				Assert.AreEqual(value, bufferedValue, "value read from buffer should be the same as the value we recorded");
			}
		}

		[TestMethod]
		public void BufferRoundtrip_Single()
		{
			IBufferWriter writer = BufferWriter.Create();
			IBufferReader reader = BufferReader.Create();
			DataGenerator gen = new DataGenerator(reader);
			var test = new
			{
				Items = 4000,
				SizeOfItem = sizeof(Single),
				Next = new Func<Single>(() => gen.GetSingle()),
				Recorded = new Queue<Single>()
			};

			byte[] buffer = new byte[test.Items * test.SizeOfItem];
			var writeCursor = 0;
			for (int i = 0; i < test.Items; i++)
			{
				var captureCursor = writeCursor;
				var value = test.Next();
				test.Recorded.Enqueue(value);

				var written = writer.Write(buffer, ref writeCursor, value);

				Assert.AreEqual<int>(test.SizeOfItem, written, String.Concat("should have written ", test.SizeOfItem));
				Assert.AreEqual<int>(written, writeCursor - captureCursor, "cursor should have incremented equal to the number of bytes written");
			}

			var readCursor = 0;
			while (test.Recorded.Count > 0)
			{
				var captureCursor = readCursor;
				var value = test.Recorded.Dequeue();
				var bufferedValue = reader.ReadSingle(buffer, ref readCursor);

				Assert.AreEqual<int>(test.SizeOfItem, readCursor - captureCursor, "cursor should have incremented equal to the number of bytes written");
				Assert.AreEqual(value, bufferedValue, "value read from buffer should be the same as the value we recorded");
			}
		}

		[TestMethod]
		public void BufferRoundtrip_Guid()
		{
			IBufferWriter writer = BufferWriter.Create();
			IBufferReader reader = BufferReader.Create();
			DataGenerator gen = new DataGenerator(reader);
			var test = new
			{
				Items = 4000,
				SizeOfItem = sizeof(byte) * 16,
				Next = new Func<Guid>(() => gen.GetGuid()),
				Recorded = new Queue<Guid>()
			};

			byte[] buffer = new byte[test.Items * test.SizeOfItem];
			var writeCursor = 0;
			for (int i = 0; i < test.Items; i++)
			{
				var captureCursor = writeCursor;
				var value = test.Next();
				test.Recorded.Enqueue(value);

				var written = writer.Write(buffer, ref writeCursor, value);

				Assert.AreEqual<int>(test.SizeOfItem, written, String.Concat("should have written ", test.SizeOfItem));
				Assert.AreEqual<int>(written, writeCursor - captureCursor, "cursor should have incremented equal to the number of bytes written");
			}

			var readCursor = 0;
			while (test.Recorded.Count > 0)
			{
				var captureCursor = readCursor;
				var value = test.Recorded.Dequeue();
				var bufferedValue = reader.ReadGuid(buffer, ref readCursor);

				Assert.AreEqual<int>(test.SizeOfItem, readCursor - captureCursor, "cursor should have incremented equal to the number of bytes written");
				Assert.AreEqual(value, bufferedValue, "value read from buffer should be the same as the value we recorded");
			}
		}

		[TestMethod]
		public void BufferRoundtrip_LengthPrefixedString()
		{
			IBufferWriter writer = BufferWriter.Create();
			IBufferReader reader = BufferReader.Create();
			DataGenerator gen = new DataGenerator(reader);
			Random rand = new Random(Environment.TickCount);

			var test = new
			{
				Items = 4000,
				SizeOfItem = 4000,
				Next = new Func<string>(() => gen.GetString(rand.Next(4000))),
				Recorded = new Queue<string>()
			};

			// ~ 640M
			byte[] buffer = new byte[test.Items * test.SizeOfItem * 4];
			var writeCursor = 0;
			for (int i = 0; i < test.Items; i++)
			{
				var captureCursor = writeCursor;
				var value = test.Next();
				test.Recorded.Enqueue(value);

				var written = writer.Write(buffer, ref writeCursor, value, true);
				Assert.AreEqual<int>(written, writeCursor - captureCursor, "cursor should have incremented equal to the number of bytes written");
			}

			var readCursor = 0;
			while (test.Recorded.Count > 0)
			{
				var captureCursor = readCursor;
				var value = test.Recorded.Dequeue();
				var bufferedValue = reader.ReadStringWithByteCountPrefix(buffer, ref readCursor);

				Assert.AreEqual(value, bufferedValue, "value read from buffer should be the same as the value we recorded");
			}
		}

	}

}
