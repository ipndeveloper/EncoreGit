Additional Steps Needed to Publish this site:
	1. Modify the usersettings.js for the environment you wish to deploy to.
	2. Modify the cache version of the index.html file so yepnope will be forced to reload our resources.

Clients may modify the usersettings.js in the ClientSettings.PreBuild to have their api locations. 

I tried to find a way to transform the usersettings.js based on the solution configuration (similar to the web.config), but after much time trying, I'm giving up and just writing this read me doc to tell people
they need to comment/uncomment the correct lines for the environment you want to publish to. I had a t4 template that worked when manually running it, but running it upon building the project proved problematic.

