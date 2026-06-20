using System;
using System.Drawing;

namespace CodexUsageTray;

internal static class TrayIconRenderer
{
    public static Icon CreateIcon(int remainingPercent, bool warning)
    {
        try
        {
            using var bitmap = new Bitmap(32, 32);
            using var g = Graphics.FromImage(bitmap);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.Clear(Color.Transparent);
            var color = warning ? Color.FromArgb(210, 72, 64) : Color.FromArgb(37, 131, 92);
            using var background = new Pen(Color.FromArgb(180, 188, 200), 4);
            using var progress = new Pen(color, 4) { StartCap = System.Drawing.Drawing2D.LineCap.Round, EndCap = System.Drawing.Drawing2D.LineCap.Round };
            g.DrawEllipse(background, 5, 5, 22, 22);
            g.DrawArc(progress, 5, 5, 22, 22, -90, 360 * Math.Clamp(remainingPercent, 0, 100) / 100f);
            using var textBrush = new SolidBrush(color);
            using var font = new Font("Segoe UI", 7f, FontStyle.Bold, GraphicsUnit.Point);
            var label = Math.Clamp(remainingPercent, 0, 99).ToString();
            var size = g.MeasureString(label, font);
            g.DrawString(label, font, textBrush, (32 - size.Width) / 2, (32 - size.Height) / 2);
            return Icon.FromHandle(bitmap.GetHicon());
        }
        catch
        {
            return SystemIcons.Application;
        }
    }
}
