using FamilieLaissMassTransitDefinitions.Contracts.Commands;

namespace FamilieLaissMassTransitDefinitions.Commands;

public class MassSetUploadPictureDimensionsCmd : IMassSetUploadPictureDimensionsCmd
{
    #region Implementation of IMassSetUploadPictureDimensionsCmd

    /// <inheritdoc />
    public long Id { get; set; }

    /// <inheritdoc />
    public int Height { get; set; }

    /// <inheritdoc />
    public int Width { get; set; }

    #endregion
}