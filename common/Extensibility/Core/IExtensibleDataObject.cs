
namespace NetSteps.Extensibility.Core
{
    /// <summary>
    /// An interface describing an Extensible Object.
    /// </summary>
    public interface IExtensibleDataObject
    {
        /// <summary>
        /// Gets or sets the extension provider key.
        /// </summary>
        /// <value>
        /// The extension provider key.
        /// </value>
        string ExtensionProviderKey { get; set; }
        /// <summary>
        /// Gets or sets the extension.
        /// </summary>
        /// <value>
        /// The extension.
        /// </value>
        IDataObjectExtension Extension { get; set; }
    }
}
