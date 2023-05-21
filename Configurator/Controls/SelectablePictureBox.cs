/* Copyright (c) 2023, Oracle and/or its affiliates.

 This program is free software; you can redistribute it and/or modify
 it under the terms of the GNU General Public License as published by
 the Free Software Foundation; version 2 of the License.

 This program is distributed in the hope that it will be useful,
 but WITHOUT ANY WARRANTY; without even the implied warranty of
 MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 GNU General Public License for more details.

 You should have received a copy of the GNU General Public License
 along with this program; if not, write to the Free Software
 Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301  USA */

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using MySql.Configurator.Core.Classes;

namespace MySql.Configurator.Controls
{
  /// <summary>
  /// Represents a <see cref="PictureBox"/> that can have a selectable status reflected visually.
  /// </summary>
  public class SelectablePictureBox : PictureBox
  {
    #region Fields

    /// <summary>
    /// Preserves a cached image to represent a selected state.
    /// </summary>
    private Image _cachedSelectedImage;

    /// <summary>
    /// Preserves a cached image to represent an unselected state.
    /// </summary>
    private Image _cachedUnselectedImage;

    /// <summary>
    /// Flag indicating whether the <see cref="PictureBox.Image"/> is automatically set to a version of the image in <seealso cref="PictureBox.ImageLocation"/> corresponding to the selected state.
    /// </summary>
    private bool _changeImageOnSelection;

    /// <summary>
    /// Flag indicating whether the picture is selected or not.
    /// </summary>
    private bool _selected;

    #endregion Fields

    #region Events

    /// <summary>
    /// Occurs when the value of the <see cref="Selected"/> property changes.
    /// </summary>
    public event EventHandler SelectedStateChanged;

    #endregion Events

    /// <summary>
    /// Initializes a new instance of the <see cref="SelectablePictureBox"/> class.
    /// </summary>
    public SelectablePictureBox()
    {
      _cachedSelectedImage = null;
      _cachedUnselectedImage = null;
      _changeImageOnSelection = true;
      _selected = false;
      ChangeImage(_selected);
    }

    #region Properties

    /// <summary>
    /// Gets or sets a value indicating whether the <see cref="PictureBox.Image"/> is automatically set to a version of the image in <seealso cref="PictureBox.ImageLocation"/> corresponding to the selected state.
    /// </summary>
    [Category("MySQL Custom"), Description("A value indicating whether the picture is selected or not."),
     DefaultValue(true)]
    public bool ChangeImageOnSelection
    {
      get => _changeImageOnSelection;
      set
      {
        var oldChangeImageValue = _selected;
        _changeImageOnSelection = value;
        if (oldChangeImageValue != _changeImageOnSelection)
        {
          ChangeImage(_selected);
        }
      }
    }

    /// <summary>
    /// Gets or sets the path or URL for the image to display in the <see cref="SelectablePictureBox"/>.
    /// </summary>
    public new Image InitialImage
    {
      get => base.InitialImage;
      set
      {
        base.InitialImage = value;
        UpdateCachedImages();
        ChangeImage(_selected);
      }
    }

    /// <summary>
    /// Gets or sets the path or URL for the image to display in the <see cref="SelectablePictureBox"/>.
    /// </summary>
    public new string ImageLocation
    {
      get => base.ImageLocation;
      set
      {
        base.ImageLocation = value;
        UpdateCachedImages();
        ChangeImage(_selected);
      }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the picture is selected or not.
    /// </summary>
    /// <remarks>
    /// If <see cref="ChangeImageOnSelection"/> is <c>true</c>, the <seealso cref="PictureBox.Image"/> property will be automatically set to a version of the image in <seealso cref="PictureBox.ImageLocation"/> corresponding to the selected state.
    /// </remarks>
    [Category("MySQL Custom"), Description("A value indicating whether the picture is selected or not."), DefaultValue(false)]
    public bool Selected
    {
      get => _selected;
      set
      {
        var oldSelectedValue = _selected;
        _selected = value;
        if (oldSelectedValue != _selected)
        {
          ChangeImage(_selected);
          OnSelectedStateChanged();
        }
      }
    }

    #endregion Properties

    /// <summary>
    /// Changes the <see cref="Image"/> value to one of the cached images.
    /// </summary>
    public void ChangeImage(bool selected)
    {
      Image = ChangeImageOnSelection
              && selected
        ? _cachedSelectedImage
        : _cachedUnselectedImage;
    }

    /// <summary>
    /// Updates the cached images.
    /// </summary>
    public void UpdateCachedImages()
    {
      if (!string.IsNullOrEmpty(ImageLocation)
          && File.Exists(ImageLocation))
      {
        _cachedSelectedImage = Image.FromFile(ImageLocation);
        _cachedUnselectedImage = Darken(new Bitmap(ImageLocation));
      }
      else if (InitialImage != null)
      {
        _cachedSelectedImage = InitialImage;
        _cachedUnselectedImage = Darken(InitialImage as Bitmap);
      }
    }

    /// <summary>
    /// Raises the <see cref="SelectedStateChanged"/> event.
    /// </summary>
    protected void OnSelectedStateChanged()
    {
      SelectedStateChanged?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Gets a <see cref="ColorMatrix"/> that can change a <see cref="Bitmap"/> brightness, contrast and color saturation.
    /// </summary>
    /// <param name="brightnessFactor">The amount of "sunlight" in the picture. Ranges between -1.0f to 1.0f, -1.0f = pitch-black, 0 = original, 1.0f = total white.</param>
    /// <param name="contrastFactor">The amount of difference between red, green, and blue colors. Must be greater than 0. 0 = complete gray, 1 = original, > 1.0f = glaring white.</param>
    /// <param name="saturationFactor">The amount of "grayscale-ness" in the picture. Must be greater than 0. 0 = grayscale, 1.0f = original colors, > 1.0f = very colorful.</param>
    /// <returns>A <see cref="ColorMatrix"/> that can change a <see cref="Bitmap"/> brightness, contrast and color saturation..</returns>
    private ColorMatrix CreateColorMatrix(float brightnessFactor = 0, float contrastFactor = 1.0f, float saturationFactor = 1.0f)
    {
      const float LUM_RED = 0.3086f;    // or  0.2125f
      const float LUM_GREEN = 0.6094f;  // or  0.7154f
      const float LUM_BLUE = 0.0820f;   // or  0.0721f
      brightnessFactor = Math.Min(1.0f, brightnessFactor);
      brightnessFactor = Math.Max(-1.0f, brightnessFactor);
      contrastFactor = Math.Max(0, contrastFactor);
      saturationFactor = Math.Max(0, saturationFactor);
      var whiteFactor = (1.0f - contrastFactor) / 2.0f + brightnessFactor;
      var redFactor = (1.0f - saturationFactor) * LUM_RED * contrastFactor;
      var greenFactor = (1.0f - saturationFactor) * LUM_GREEN * contrastFactor;
      var blueFactor = (1.0f - saturationFactor) * LUM_BLUE * contrastFactor;
      var redFactor2 = redFactor + saturationFactor * contrastFactor;
      var greenFactor2 = greenFactor + saturationFactor * contrastFactor;
      var blueFactor2 = blueFactor + saturationFactor * contrastFactor;
      return new ColorMatrix(new[]
      {
        new[] { redFactor2, redFactor, redFactor,  0,  0 },
        new[] { greenFactor, greenFactor2, greenFactor,  0,  0 },
        new[] { blueFactor, blueFactor, blueFactor2, 0,  0 },
        new[] { 0,  0,  0,  1.0f,  0 },
        new[] { whiteFactor, whiteFactor, whiteFactor,  0,  1 }
      });
    }

    /// <summary>
    /// Creates a new bitmap based on a given bitmap with its colors darkened.
    /// </summary>
    /// <param name="original">The bitmap to convert.</param>
    /// <param name="factor">A value between 0 and 1.00f, (0 means no change, default is 0.5f).</param>
    /// <returns>A new bitmap based on a given bitmap with its colors darkened.</returns>
    private Bitmap Darken(Bitmap original, float factor = 0.7f)
    {
      return TransformColors(original, 0, 1.0f - factor, 1.0f - factor);
    }

    /// <summary>
    /// Creates a new bitmap based on a given bitmap with its colors transformed by the given factors.
    /// </summary>
    /// <param name="original">The bitmap to change.</param>
    /// <param name="brightnessFactor">The amount of "sunlight" in the picture. Ranges between -1.0f to 1.0f, -1.0f = pitch-black, 0 = original, 1.0f = total white.</param>
    /// <param name="contrastFactor">The amount of difference between red, green, and blue colors. Must be greater than 0. 0 = complete gray, 1 = original, > 1.0f = glaring white.</param>
    /// <param name="saturationFactor">The amount of "grayscale-ness" in the picture. Must be greater than 0. 0 = grayscale, 1.0f = original colors, > 1.0f = very colorful.</param>
    /// <returns>A new bitmap based on a given bitmap with its colors transformed by the given factors.</returns>
    private Bitmap TransformColors(Bitmap original, float brightnessFactor = 0, float contrastFactor = 1.0f, float saturationFactor = 1.0f)
    {
      return original.ChangeColors(CreateColorMatrix(brightnessFactor, contrastFactor, saturationFactor));
    }
  }
}