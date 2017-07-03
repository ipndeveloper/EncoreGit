using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;

namespace NetSteps.Encore.Core.Tests
{
	internal class MyDispsable: Disposable {

		public Action<bool> OnDispose { get; set; }
		public int Disposals { get { return _disposals; } }
		int _disposals;
		protected override bool PerformDispose(bool disposing)
		{
			if (OnDispose != null) OnDispose(disposing);
			Assert.AreEqual(0, _disposals);
			_disposals++;
			return disposing;
		}
	}

	[TestClass]
	public class DisposableTests
	{

		[TestMethod]
		public void DoesDispose()
		{
			var numberOfCalls = 0;
			var my = new MyDispsable();
			my.OnDispose = (isDisposing) => {
				numberOfCalls++;
				Assert.IsTrue(isDisposing);
				Assert.IsFalse(my.IsDisposed);
			};

			Assert.IsFalse(my.IsDisposed, "shouldn't be disposed yet");
			my.Dispose();
			try
			{
				my.Dispose();
			}
			catch (ObjectDisposedException)
			{
			}
			Assert.IsTrue(my.IsDisposed, "should be disposed");
			Assert.AreEqual(1, numberOfCalls, "Disposable should have only been called once");
			Assert.AreEqual(1, my.Disposals);
		}				
	}
}
