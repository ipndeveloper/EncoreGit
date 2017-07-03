#NetSteps.Diagnostics.Utilities
##Installation
###Configure Event Log Listener
If configuring for use with the custom event log listener you must perform the following steps with elevated permission.

1. Open an elevated command prompt in the desired bitness 
2. Find the location of the NetSteps.Diagnostics.Utilities assembly
3. Run the the .net installation utility [installutil.exe](http://msdn.microsoft.com/en-us/library/50614e95(v=vs.110).aspx) against the assembly

##Configuration

Example

	<system.diagnostics>
		<sources>
			<!--
				@name should match desired namespace
				Sources are evaluated in order of namespace and will expand to switch with most relaxed value
			-->
			<source name="NetSteps" switchValue="All" >
				<listeners>
					<add name="EventLog" />
				</listeners>
			</source>
			<source name="nsDistributor" switchValue="All" >
				<listeners>
					<add name="EventLog" />
				</listeners>
			</source>
		</sources>
		<sharedListeners>
			<!--Can use any compatible trace listener-->
			<add type="NetSteps.Diagnostics.Utilities.Listeners.CustomEventLogTraceListener" name="EventLog" />
		</sharedListeners>
		<trace autoflush="true" />
	</system.diagnostics>

