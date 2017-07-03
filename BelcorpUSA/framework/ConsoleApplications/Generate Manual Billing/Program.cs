using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using NetSteps.Common.Configuration;
using NetSteps.Common.Events;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Interfaces;
using NetSteps.Data.Entities.Processors;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Diagnostics.Utilities;
using Generate_Manual_Billing;

namespace AutoshipProcessor
{
	public class Program
	{
		public static void Main(string[] args)
		{
			try
			{
                GenerateBatch generar = new GenerateBatch();

                Console.WriteLine("Iniciando Proceso Batch .......");

                string resultado = generar.ProcesarBatch();

                Console.WriteLine(resultado);

                Thread.Sleep(3000);

                Console.WriteLine("Fin del Proceso Batch");
			}
			catch (Exception ex)
			{
                Console.WriteLine("Error : " + ex.Message);
				
			}
		}
	}
}