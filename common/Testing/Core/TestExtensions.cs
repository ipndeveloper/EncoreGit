using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

public static class TestExtensions
{
	public static void AssertEquals<T>(this T actual, T expected)
	{
		Assert.AreEqual(expected, actual);
	}

	public static void AssertEquals<T>(this T actual, T expected, string message)
	{
		Assert.AreEqual(expected, actual, message);
	}

	public static void AssertIsTrue(this bool condition)
	{
		Assert.IsTrue(condition);
	}

	public static void AssertIsTrue(this bool condition, string message)
	{
		Assert.IsTrue(condition, message);
	}

	public static void AssertIsNotNull(this object value)
	{
		Assert.IsNotNull(value);
	}

	public static void AssertIsNotNull(this object value, string message)
	{
		Assert.IsNotNull(value, message);
	}
}
