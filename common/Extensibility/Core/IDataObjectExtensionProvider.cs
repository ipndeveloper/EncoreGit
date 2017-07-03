
namespace NetSteps.Extensibility.Core
{
    public interface IDataObjectExtensionProvider
    {
        /// <summary>
        /// Gets the provider key for the provider.
        /// </summary>
        string GetProviderKey();

        /// <summary>
        /// Saves the data object extension to the persistence layer.
        /// </summary>
        /// <param name="dataObject">The data object.</param>
        /// <returns></returns>
        IDataObjectExtension SaveDataObjectExtension(IExtensibleDataObject dataObject);

        /// <summary>
        /// Retrieves or creates and then attaches the entity extension to the extensible data object.
        /// </summary>
        /// <param name="dataObject">The data object.</param>
        /// <returns></returns>
        IDataObjectExtension GetDataObjectExtension(IExtensibleDataObject dataObject);

        /// <summary>
        /// Updates the entity extension with values from the extensible data object (i.e. primary key ID, etc.).
        /// </summary>
        /// <param name="dataObject">The data object.</param>
        void UpdateDataObjectExtension(IExtensibleDataObject dataObject);

        /// <summary>
        /// Deletes the data object extension.
        /// </summary>
        /// <param name="x">The x.</param>
        void DeleteDataObjectExtension(IExtensibleDataObject x);
    }
}
