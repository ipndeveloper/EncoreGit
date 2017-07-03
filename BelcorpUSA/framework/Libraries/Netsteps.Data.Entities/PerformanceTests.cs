using System.Collections.Generic;
using NetSteps.Common;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities.Repositories;

namespace NetSteps.Data.Entities
{
	public class PerformanceTests
	{

		public static string UpdateArchivePerformanceTest(int cycleCount)
		{
			string result = string.Empty;
			using (new OperationTimer("UpdateArchivePerformanceTest: ", cycleCount, (resultMessage) => { result = resultMessage; }))
			{
				List<int> randomArchives = new List<int>() { 4, 15, 14, 10, 9, 8, 5, 7, 34, 216, 211, 203, 196, 194 };
				for (int i = 0; i < cycleCount; i++)
				{
					ArchiveRepository archiveRepository = new ArchiveRepository();
					Archive archive = archiveRepository.Load(randomArchives.GetRandom());
					archive.StartTracking();
					//if (archive.Name.EndsWith("TEST"))
					//    archive.Name = archive.Name.TrimEnd("TEST");
					//else
					//    archive.Name = archive.Name + "TEST";
					archiveRepository.Save(archive);
				}
			}
			return result;
		}

	}
}
