using GymPro.Application.Abstractions;
using QRCoder;

namespace GymPro.Infrastructure.Qr;

/// <summary>
/// Generates member QR codes for check-in.
/// </summary>
/// <remarks>
/// Uses QRCoder rather than the original MessagingToolkit.QRCode, which never
/// shipped a .NET Core build. The generator owns native buffers, so this type
/// is disposable; register it as transient or scoped, never singleton.
/// </remarks>
public sealed class QrCodeRenderer : IQrCodeRenderer
{
    private readonly QRCodeGenerator _generator = new();
    private bool _disposed;

    /// <summary>Renders the payload as a PNG byte array.</summary>
    public byte[] RenderPng(string payload, int pixelsPerModule = 4)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        using var qrCode = _generator.CreateQrCode(payload, QRCodeGenerator.ECCLevel.Q);
        var png = new PngByteQRCode(qrCode);
        return png.GetGraphic(pixelsPerModule);
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _generator.Dispose();
        _disposed = true;
    }
}
