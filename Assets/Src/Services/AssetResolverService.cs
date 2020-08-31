using GameLovers.AddressablesExtensions;


namespace Services
{
	/// <summary>
	/// Service to extend the behaviour to load assets into the project based on it's own custom needs
	/// </summary>
	public interface IAssetResolverService : IAssetLoader, ISceneLoader
	{
	}
	
	/// <inheritdoc cref="IAssetResolverService" />
	public class AssetResolverService : AddressablesAssetLoader, IAssetResolverService
	{
	}
}