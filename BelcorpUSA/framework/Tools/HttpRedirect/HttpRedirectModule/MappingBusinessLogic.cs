// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MappingBusinessLogic.cs" company="NetSteps">
//   Copyright 2012, NetSteps, LLC
// </copyright>
// <summary>
//   Defines the MappingBusinessLogic type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace HttpRedirectModule
{
    using System;

    using NetSteps.Encore.Core.IoC;
    using System.Diagnostics.Contracts;
    using NetSteps.Encore.Core;

    /// <summary>
    /// The Mapping Business Logic class
    /// </summary>
    [ContainerRegister(typeof(IMappingBusinessLogic), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
    public class MappingBusinessLogic : IMappingBusinessLogic
    {
        IMappingRepository Mappings { get; set; }
                
        /// <summary>
        /// Creates an instance with constructor injected repository.
        /// </summary>
        /// <param name="repo"></param>
        public MappingBusinessLogic(IMappingRepository repo)
        {
            Mappings = repo ?? Create.New<IMappingRepository>();
        }

        /// <summary>
        /// Gets the new site URL based on mapping
        /// </summary>
        /// <param name="oldSiteName">
        /// The old site name.
        /// </param>
        /// <returns>
        /// returns a string with the new site name
        /// </returns>
        public string GetNewSiteUrl(string oldSiteName)
        {
            Contract.Requires<ArgumentNullException>(oldSiteName != null);
            Contract.Requires<ArgumentException>(oldSiteName.Length > 0);

            var repo = this.Mappings;
            return repo.GetNewSiteURL(oldSiteName);
        }


    }
}