using System;
using System.ComponentModel;


namespace WexInstaller.Core
{
  /// <summary>
  /// Product (MSI) Action complete event argument
  /// </summary>
  public sealed class ProductMSIActionCompletedEventArgs : AsyncCompletedEventArgs
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="ProductMSIActionCompletedEventArgs" /> class.
    /// </summary>
    /// <param name="error">Error exception</param>
    /// <param name="cancelled">A value that indicates where the MSI action was canceled</param>
    /// <param name="userState">The user state object</param>
    public ProductMSIActionCompletedEventArgs(Exception error, bool cancelled, object userState)
      : base(error, cancelled, userState)
    {
    }

    /// <summary>
    /// Gets or sets the exit code
    /// </summary>
    public int ExitCode { get; set; }
  }
}
