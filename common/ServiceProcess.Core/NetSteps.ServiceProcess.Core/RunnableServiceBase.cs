using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace NetSteps.ServiceProcess
{
	public abstract class RunnableServiceBase : ServiceBase
	{
		public ServiceState CurrentState { get; set; }

		public void RunService(string[] args)
		{
			this.AutoLog = true;
			this.CurrentState = ServiceState.Running;

			if (Environment.UserInteractive)
			{
				this.OnStart(args);
				WaitPrompt();
				this.OnStop();
			}
			else
			{
				ServiceBase.Run(this);
			}
		}

		private void WaitPrompt()
		{
			ConsoleKeyInfo keypress;
			do
			{
				Console.WriteLine("___ Press key ___");
				if (CurrentState != ServiceState.Paused && this.CanPauseAndContinue) Console.WriteLine("\tP to Pause");
				if (CurrentState != ServiceState.Running && this.CanPauseAndContinue) Console.WriteLine("\tR to Resume");
				if (CurrentState != ServiceState.Stopped) Console.WriteLine("\tS to Stop");
				Console.WriteLine(String.Empty.PadLeft(17, '_'));
				keypress = Console.ReadKey(true);
				switch (keypress.Key)
				{
					case ConsoleKey.P:
						if (this.CanPauseAndContinue)
						{
							Console.WriteLine("User > PAUSE");
							this.CurrentState = ServiceState.Paused;
							this.OnPause();
						}
						else
						{
							Console.WriteLine("User > PAUSE > Unsupported.  Service remains in previous state.");
						}
						break;
					case ConsoleKey.R:
						if (this.CanPauseAndContinue)
						{
							Console.WriteLine("User > RESUME");
							this.CurrentState = ServiceState.Running;
							this.OnContinue();
						}
						else
						{
							Console.WriteLine("User > RESUME > Unsupported.  Service remains in previous state.");
						}
						break;
					case ConsoleKey.S:
						Console.WriteLine("User > STOP");
						this.CurrentState = ServiceState.Stopped;
						break;
					default:
						Console.WriteLine("User > Invalid Key");
						break;
				}
			} while (keypress.Key != ConsoleKey.S);
		}
	}
}
