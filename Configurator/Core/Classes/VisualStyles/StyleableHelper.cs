// Copyright (c) 2012, 2019, Oracle and/or its affiliates. All rights reserved.
//
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License as
// published by the Free Software Foundation; version 2 of the
// License.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA
// 02110-1301  USA

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace MySql.Configurator.Core.Classes.VisualStyles
{
  /// <summary>
  /// Defines methods that help with the automatic styling of windows forms and dialogs.
  /// </summary>
  public static class StyleableHelper
  {
    #region Constants

    /// <summary>
    /// The standard value for DPI settings at 100%.
    /// </summary>
    public const float STANDARD_DPI = 96;

    #endregion Constants

    #region Properties

    /// <summary>
    /// Gets a value indicating whether Vista dialog themes are supported.
    /// </summary>
    public static bool AreVistaDialogsThemeSupported => IsWindowsVistaOrLater && VisualStyleRenderer.IsSupported && Application.RenderWithVisualStyles;

    /// <summary>
    /// Gets a <see cref="ColorMatrix"/> to convert colors to a black & white palette.
    /// </summary>
    public static ColorMatrix BlackAndWhiteColorMatrix => new ColorMatrix(
      new[]
      {
        new[] { 1.5f, 1.5f, 1.5f, 0, 0 },
        new[] { 1.5f, 1.5f, 1.5f, 0, 0 },
        new[] { 1.5f, 1.5f, 1.5f, 0, 0 },
        new float[] { 0, 0, 0, 1, 0 },
        new float[] { -1, -1, -1, 0, 1 }
      });

    /// <summary>
    /// Gets a <see cref="ColorMatrix"/> to convert colors to a greyscale palette.
    /// </summary>
    public static ColorMatrix GrayScaleColorMatrix => new ColorMatrix(
      new[]
      {
        new[] { .3f, .3f, .3f, 0, 0 },
        new[] { .59f, .59f, .59f, 0, 0 },
        new[] { .11f, .11f, .11f, 0, 0 },
        new float[] { 0, 0, 0, 1, 0 },
        new float[] { 0, 0, 0, 0, 1 }
      });

    /// <summary>
    /// Gets a <see cref="ColorMatrix"/> to convert colors to an inverted palette.
    /// </summary>
    public static ColorMatrix InvertedColorMatrix => new ColorMatrix(
      new[]
      {
        new float[] { -1, 0, 0, 0, 0 },
        new float[] { 0, -1, 0, 0, 0 },
        new float[] { 0, 0, -1, 0, 0 },
        new float[] { 0, 0, 0, 1, 0 },
        new float[] { 1, 1, 1, 0, 1 }
      });

    /// <summary>
    /// Gets a <see cref="ColorMatrix"/> to convert colors to a polaroid palette.
    /// </summary>
    public static ColorMatrix PolaroidColorMatrix => new ColorMatrix(
      new[]
      {
        new[] { 1.438f, -0.062f, -0.062f, 0, 0 },
        new[] { -0.122f, 1.378f, -0.122f, 0, 0 },
        new[] { -0.016f, -0.016f, 1.483f, 0, 0 },
        new float[] { 0, 0, 0, 1, 0 },
        new[] { -0.03f, 0.05f, -0.02f, 0, 1.0f }
      });

    /// <summary>
    /// Gets a <see cref="ColorMatrix"/> to convert colors to a sepia palette.
    /// </summary>
    public static ColorMatrix SepiaColorMatrix => new ColorMatrix(
      new[]
      {
        new[] { 0.393f, 0.349f, 0.272f, 0, 0 },
        new[] { 0.769f, 0.686f, 0.534f, 0, 0 },
        new[] { 0.189f, 0.168f, 0.131f, 0, 0 },
        new float[] { 0, 0, 0, 1, 0 },
        new float[] { 0, 0, 0, 0, 1 }
      });

    /// <summary>
    /// Gets a <see cref="ColorMatrix"/> to swap RGB colors to BGR.
    /// </summary>
    public static ColorMatrix SwapRgbToBgrColorMatrix => new ColorMatrix(
      new[]
      {
        new float[] { 0, 0, 1, 0, 0 },
        new float[] { 0, 1, 0, 0, 0 },
        new float[] { 1, 0, 0, 0, 0 },
        new float[] { 0, 0, 0, 1, 0 },
        new float[] { 0, 0, 0, 0, 1 }
      });

    /// <summary>
    /// Gets a value indicating whether the OS version is Windows.
    /// </summary>
    public static bool IsWindows
    {
      get
      {
        var platform = Environment.OSVersion.Platform;
        return platform == PlatformID.Win32NT
               || platform == PlatformID.Win32S
               || platform == PlatformID.Win32Windows;
      }
    }

    /// <summary>
    /// Gets a value indicating whether the Windows version is Vista or later (7, 8).
    /// </summary>
    public static bool IsWindowsVistaOrLater => IsWindows && Environment.OSVersion.Version >= new Version(6, 0, 6000);

    /// <summary>
    /// Gets a value indicating whether the Windows version is XP or later (Vista, 7, 8).
    /// </summary>
    public static bool IsWindowsXpOrLater => IsWindows && Environment.OSVersion.Version >= new Version(5, 1, 2600);
  
    #endregion Properties

    /// <summary>
    /// Draws a text string within the given device context on the given location and using a specific visual style element.
    /// </summary>
    /// <param name="deviceContext">Windows device context.</param>
    /// <param name="text">Text string to draw.</param>
    /// <param name="element">Visual style element to draw the text for.</param>
    /// <param name="fallbackFont">Font to use in case Vista dialog themes are not supported.</param>
    /// <param name="location">Top-left coordinates where to start drawing the text.</param>
    /// <param name="measureOnly">Flag indicating if text will not be drawn but only the drawing area will be measured.</param>
    /// <param name="width">Width of the area where text is going to be drawn.</param>
    /// <returns>Bottom-left coordinates of the drawn text rectangle.</returns>
    public static Point DrawText(IDeviceContext deviceContext, string text, VisualStyleElement element, Font fallbackFont, Point location, bool measureOnly, int width)
    {
      var newLocation = location;
      if (string.IsNullOrEmpty(text))
      {
        return newLocation;
      }

      var textRect = new Rectangle(location.X, location.Y, width, (IsWindowsXpOrLater ? int.MaxValue : 100000));
      const TextFormatFlags FLAGS = TextFormatFlags.WordBreak;
      if (AreVistaDialogsThemeSupported)
      {
        var renderer = new VisualStyleRenderer(element);
        var textSize = renderer.GetTextExtent(deviceContext, textRect, text, FLAGS);
        newLocation = location + new Size(0, textSize.Height);
        if (!measureOnly)
        {
          renderer.DrawText(deviceContext, textSize, text, false, FLAGS);
        }
      }
      else
      {
        if (!measureOnly)
        {
          TextRenderer.DrawText(deviceContext, text, fallbackFont, textRect, SystemColors.WindowText, FLAGS);
        }

        var textSize = TextRenderer.MeasureText(deviceContext, text, fallbackFont, new Size(textRect.Width, textRect.Height), FLAGS);
        newLocation = location + new Size(0, textSize.Height);
      }

      return newLocation;
    }

    /// <summary>
    /// Gets a <see cref="ColorMatrix"/> that can change a <see cref="Bitmap"/> brightness, contrast and color saturation.
    /// </summary>
    /// <param name="brightnessFactor">The amount of "sunlight" in the picture. Ranges between -1.0f to 1.0f, -1.0f = pitch-black, 0 = original, 1.0f = total white.</param>
    /// <param name="contrastFactor">The amount of difference between red, green, and blue colors. Must be greater than 0. 0 = complete gray, 1 = original, > 1.0f = glaring white.</param>
    /// <param name="saturationFactor">The amount of "grayscale-ness" in the picture. Must be greater than 0. 0 = grayscale, 1.0f = original colors, > 1.0f = very colorful.</param>
    /// <returns>A <see cref="ColorMatrix"/> that can change a <see cref="Bitmap"/> brightness, contrast and color saturation..</returns>
    public static ColorMatrix CreateColorMatrix(float brightnessFactor = 0, float contrastFactor = 1.0f, float saturationFactor = 1.0f)
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
    /// Gets the multiplying factor to convert physical pixels to DIPs (Device-Independent Pixels).
    /// </summary>
    /// <param name="control">A <see cref="Control"/> instance.</param>
    /// <returns>The multiplying factor to convert physical pixels to DIPs (Device-Independent Pixels).</returns>
    public static float GetDpiScaleX(this Control control)
    {
      if (control == null)
      {
        return 1;
      }

      using (var graphics = control.CreateGraphics())
      {
        return graphics.DpiX / STANDARD_DPI;
      }
    }

    /// <summary>
    /// Gets the multiplying factor to convert physical pixels to DIPs (Device-Independent Pixels).
    /// </summary>
    /// <param name="control">A <see cref="Control"/> instance.</param>
    /// <returns>The multiplying factor to convert physical pixels to DIPs (Device-Independent Pixels).</returns>
    public static float GetDpiScaleY(this Control control)
    {
      if (control == null)
      {
        return 1;
      }

      using (var graphics = control.CreateGraphics())
      {
        return graphics.DpiY / STANDARD_DPI;
      }
    }
  }
}