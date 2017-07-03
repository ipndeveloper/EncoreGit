README.txt

Originates with the NetSteps.Encore.Core library.

If you find it in your project, you may safely delete it, but please note
that you MUST install performance counters on any machine that uses the
NetSteps.Encore.Core library's caching classes.

You should run the installer packaged in the library every time you 
pull down an updated version. It installs performance counters onto 
Windoze for tracking using the Performance Monitor:

NetSteps Cache
	hit ratio (overall)         - The ratio of hits to read-attempts. Higher ratios are better.
	resolve ratio (overall)     - For active caches, the ratio of resolves to reads. Lower ratios are better.
	eviction workers (overall)  - The number of threads being used to evict cache items when cache size exceeds its configured cache-depth

NetSteps Cache (ea)
	hit ratio                   - The ratio of hits to read-attempts. Higher ratios are better.
	reads/sec                   - The number of read attempts per second. This is the base used to calculate hit ratio and resolve ratio (for Active Caches)
	writes/sec                  - The number of writes per second. Corresponds to adding an item to the cache.
	removes/sec                 - The number of removes per second. Corresponds to removing an item from the cache.
	expires/sec                 - The number of expirations per second.
	evictions/sec               - The number of evictions per second.

NetSteps Active Cache (ea)    - Includes same counters as NetSteps Cache (ea), as well as a few addl:
	resolve ratio               - The ratio of resolves to reads. Lower ratios are better.
	resolves/sec                - Number of resolves per second.

Use the accompanying powershell script to install the performance counters.