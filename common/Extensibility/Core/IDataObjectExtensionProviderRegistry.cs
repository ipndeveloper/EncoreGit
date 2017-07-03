
namespace NetSteps.Extensibility.Core
{
    /// <summary>
    /// Interface describing the Extension Provider registry.
    /// </summary>
    public interface IDataObjectExtensionProviderRegistry
    {
        /// <summary>
        /// Registers the provided type for registered provider.
        /// </summary>
        /// <param name="ExtensionTypeName">Name of the extension type.</param>
        /// <param name="providerKey">The provider key.</param>
        void RegisterProvidedTypeForRegisteredProvider(string ExtensionTypeName, string providerKey);

        /// <summary>
        /// Registers the extension provider.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="providerKey">The provider key.</param>
        void RegisterExtensionProvider<T>(string providerKey)
            where T : IDataObjectExtensionProvider;

        /// <summary>
        /// Retrieves the extension provider.
        /// </summary>
        /// <param name="providerKey">The provider key.</param>
        /// <returns></returns>
        IDataObjectExtensionProvider RetrieveExtensionProvider(string providerKey);

        /// <summary>
        /// Retrieves the extension provider.
        /// </summary>
        /// <typeparam name="TProvider">The type of the provider.</typeparam>
        /// <param name="providerKey">The provider key.</param>
        /// <returns></returns>
        TProvider RetrieveExtensionProvider<TProvider>(string providerKey) where TProvider : IDataObjectExtensionProvider;

        /// <summary>
        /// Retrieves the type of the extension provider for registered provided.
        /// </summary>
        /// <param name="ExtensionTypeName">Name of the extension type.</param>
        /// <returns></returns>
        IDataObjectExtensionProvider RetrieveExtensionProviderForRegisteredProvidedType(string ExtensionTypeName);

        /// <summary>
        /// Retrieves the type of the extension provider for registered provided.
        /// </summary>
        /// <typeparam name="TProvider">The type of the provider.</typeparam>
        /// <param name="ExtensionTypeName">Name of the extension type.</param>
        /// <returns></returns>
        TProvider RetrieveExtensionProviderForRegisteredProvidedType<TProvider>(string ExtensionTypeName) where TProvider : IDataObjectExtensionProvider;
    
    }
}
