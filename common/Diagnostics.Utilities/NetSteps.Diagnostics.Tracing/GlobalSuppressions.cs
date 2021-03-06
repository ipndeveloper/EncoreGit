// This file is used by Code Analysis to maintain SuppressMessage 
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given 
// a specific target and scoped to a namespace, type, member, etc.
//
// To add a suppression to this file, right-click the message in the 
// Code Analysis results, point to "Suppress Message", and click 
// "In Suppression File".
// You do not need to add suppressions to this file manually.

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Scope = "member", Target = "NetSteps.Diagnostics.Utilities.TraceActivity.#System.IDisposable.Dispose()",Justification="Class makes use of dispose as scoping construct not intended for interface to be used in public methods")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly", Scope = "member", Target = "NetSteps.Diagnostics.Utilities.TraceActivity.#System.IDisposable.Dispose()", Justification = "Class makes use of dispose as scoping construct not intended for interface to be used in public methods")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly", Scope = "type", Target = "NetSteps.Diagnostics.Utilities.TraceActivity", Justification = "Class makes use of dispose as scoping construct not intended for interface to be used in public methods")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors", Scope = "member", Target = "NetSteps.Diagnostics.Utilities.TraceActivity.#.ctor(System.Type,System.String)",Justification="Start and stop are part of design construct of class behavior is by design")]
