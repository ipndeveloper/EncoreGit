namespace NetSteps.EventProcessing.Common
{
	using System;
	using System.Reflection;
	using NetSteps.Encore.Core.Parallel;

	public interface IEventProcessingService
	{
		Future<bool> ExecuteEventAssembly(string typeName, int eventID);
		void BeginMultiThreaded();
		void StopProcessing();
	}

	public static class IEventProcessingServiceExtensions
	{
		public static string GetAssemblyFolderPath(this IEventProcessingService service, string customPath)
		{
			return System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, customPath);
		}
	}
}