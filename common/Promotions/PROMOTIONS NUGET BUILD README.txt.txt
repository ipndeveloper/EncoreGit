To build from this branch, and keep your revision numbers the same:

You will need to build each component and place it in the nuget repository successively based upon dependencies.  Build in this order:

Promotions.Common
<copy to nuget folder and update nuget references in the projects>
Promotions.Plugins.Base
Promotions.Caching
<copy to nuget folder and update nuget references in the projects>
Promotions.Service
Promotions.BasePluginTests
Promotions.Plugins.Common
<copy to nuget folder and update nuget references in the projects>
Promotions.Plugins
<copy to nuget folder and update nuget references in the projects>
Promotions.UI.Common
<copy to nuget folder and update nuget references in the projects>
Promotions.UI.Service
<copy to nuget folder>

Don't check in, or the revision number will change.  Always build production ready nugets from a tag.