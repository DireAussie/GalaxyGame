﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityHelpers.Extensions
{
    public static class TextureExtensions
    {
        public static void DrawPoint(this Texture2D tex, Point p, int r, Color col)
        {
            tex.DrawPoint(p.x, p.y, r, col);
        }

        public static void DrawPoint(this Texture2D tex, Vector2 v, int r, Color col)
        {
            tex.DrawPoint((int) v.x, (int) v.y, r, col);
        }

        public static void DrawPoint(this Texture2D tex, int x, int y, int r, Color col)
        {
            if (r == 0)
            {
                tex.SetPixel(x, y, col);
                return;
            }

            for (var i = -r; i < r + 1; i++)
                for (var j = -r; j < r + 1; j++)
                    tex.SetPixel(x + i, y + j, col);
        }

        public static void DrawLine(this Texture2D tex, Point p0, Point p1, Color col)
        {
            tex.DrawLine(p0.x, p0.y, p1.x, p1.y, col);
        }

        public static void DrawLine(this Texture2D tex, Vector2 v0, Vector2 v1, Color col)
        {
            tex.DrawLine((int) v0.x, (int) v0.y, (int) v1.x, (int) v1.y, col);
        }

        public static void DrawLine(this Texture2D tex, int x0, int y0, int x1, int y1, Color col)
        {
            var dy = y1 - y0;
            var dx = x1 - x0;
            int stepx, stepy;

            if (dy < 0)
            {
                dy = -dy;
                stepy = -1;
            }
            else
            {
                stepy = 1;
            }
            if (dx < 0)
            {
                dx = -dx;
                stepx = -1;
            }
            else
            {
                stepx = 1;
            }

            dy <<= 1;
            dx <<= 1;

            float fraction;

            tex.SetPixel(x0, y0, col);
            if (dx > dy)
            {
                fraction = dy - (dx >> 1);
                while (Mathf.Abs(x0 - x1) > 1)
                {
                    if (fraction >= 0)
                    {
                        y0 += stepy;
                        fraction -= dx;
                    }
                    x0 += stepx;
                    fraction += dy;
                    if (x0 < 0 || y0 < 0)
                        tex.SetPixel(x0, y0, col);
                }
            }
            else
            {
                fraction = dx - (dy >> 1);
                while (Mathf.Abs(y0 - y1) > 1)
                {
                    if (fraction >= 0)
                    {
                        x0 += stepx;
                        fraction -= dy;
                    }
                    y0 += stepy;
                    fraction += dx;
                    tex.SetPixel(x0, y0, col);
                }
            }
        }

        public static bool DrawLineConstrained(this Texture2D tex, int x0, int y0, int x1, int y1, Color col)
        {
            var rect = new Rect(0, 0, tex.width, tex.height);

            var point0InRect = Math3d.IsPointInRectangle(new Vector2(x0, y0), rect);
            var point1InRect = Math3d.IsPointInRectangle(new Vector2(x1, y1), rect);

            if (point0InRect && point1InRect)
                DrawLine(tex, x0, y0, x1, y1, col);

            Vector2 intersection;

            var list = new HashSet<Vector2>();

            if (Math3d.LineLineIntersectionPoints(out intersection, rect.RectLeftBottom(), rect.RectLeftTop(), new Vector2(x0, y0), new Vector2(x1, y1)))
                list.Add(intersection);

            if (Math3d.LineLineIntersectionPoints(out intersection, rect.RectLeftTop(), rect.RectRightTop(), new Vector2(x0, y0), new Vector2(x1, y1)))
                list.Add(intersection);

            if (Math3d.LineLineIntersectionPoints(out intersection, rect.RectRightTop(), rect.RectRightBottom(), new Vector2(x0, y0), new Vector2(x1, y1)))
                list.Add(intersection);

            if (Math3d.LineLineIntersectionPoints(out intersection, rect.RectRightBottom(), rect.RectLeftBottom(), new Vector2(x0, y0), new Vector2(x1, y1)))
                list.Add(intersection);

            if (list.Count >= 1)
            {
                if (x0 == x1 && x0 == 0)
                    


            }

            if (list.Count == 1)
            {
                Vector2 inRectPoint;
                //find the point that lies inside
                if (point0InRect)
                {
                    inRectPoint.x = x0;
                    inRectPoint.y = y0;
                }
                else if (point1InRect)
                {
                    inRectPoint.x = x1;
                    inRectPoint.y = y1;
                }
                else
                {
                    inRectPoint.x = (int)list.First().x;
                    inRectPoint.y = (int)list.First().y;
                }

                DrawLine(tex, list.First(), inRectPoint, col);
            }
            if (list.Count == 2)
                DrawLine(tex, list.First(), list.Skip(1).First(), col);
        }

        public static void DrawCircle(this Texture2D tex, Vector2 v, int r, Color col)
        {
            tex.DrawCircle((int) v.x, (int) v.y, r, col);
        }

        public static void DrawCircle(this Texture2D tex, Point p, int r, Color col)
        {
            tex.DrawCircle(p.x, p.y, r, col);
        }

        public static void DrawCircle(this Texture2D tex, int cx, int cy, int r, Color col)
        {
            var y = r;
            var d = 1 / 4 - r;
            var end = Mathf.Ceil(r / Mathf.Sqrt(2));

            for (var x = 0; x <= end; x++)
            {
                tex.SetPixel(cx + x, cy + y, col);
                tex.SetPixel(cx + x, cy - y, col);
                tex.SetPixel(cx - x, cy + y, col);
                tex.SetPixel(cx - x, cy - y, col);
                tex.SetPixel(cx + y, cy + x, col);
                tex.SetPixel(cx - y, cy + x, col);
                tex.SetPixel(cx + y, cy - x, col);
                tex.SetPixel(cx - y, cy - x, col);

                d += 2 * x + 1;
                if (d > 0)
                {
                    d += 2 - 2 * y--;
                }
            }
        }

        public static void FloodFillArea(this Texture2D aTex, int aX, int aY, Color aFillColor)
        {
            var w = aTex.width;
            var h = aTex.height;
            var colors = aTex.GetPixels();
            var refCol = colors[aX + aY * w];
            var nodes = new Queue<Point>();
            nodes.Enqueue(new Point(aX, aY));
            while (nodes.Count > 0)
            {
                var current = nodes.Dequeue();
                for (var i = current.x; i < w; i++)
                {
                    var C = colors[i + current.y * w];
                    if (C != refCol || C == aFillColor)
                        break;
                    colors[i + current.y * w] = aFillColor;
                    if (current.y + 1 < h)
                    {
                        C = colors[i + current.y * w + w];
                        if (C == refCol && C != aFillColor)
                            nodes.Enqueue(new Point(i, current.y + 1));
                    }
                    if (current.y - 1 >= 0)
                    {
                        C = colors[i + current.y * w - w];
                        if (C == refCol && C != aFillColor)
                            nodes.Enqueue(new Point(i, current.y - 1));
                    }
                }
                for (var i = current.x - 1; i >= 0; i--)
                {
                    var C = colors[i + current.y * w];
                    if (C != refCol || C == aFillColor)
                        break;
                    colors[i + current.y * w] = aFillColor;
                    if (current.y + 1 < h)
                    {
                        C = colors[i + current.y * w + w];
                        if (C == refCol && C != aFillColor)
                            nodes.Enqueue(new Point(i, current.y + 1));
                    }
                    if (current.y - 1 >= 0)
                    {
                        C = colors[i + current.y * w - w];
                        if (C == refCol && C != aFillColor)
                            nodes.Enqueue(new Point(i, current.y - 1));
                    }
                }
            }
            aTex.SetPixels(colors);
        }

        public static void FloodFillBorder(this Texture2D aTex, int aX, int aY, Color aFillColor, Color aBorderColor)
        {
            var w = aTex.width;
            var h = aTex.height;
            var colors = aTex.GetPixels();
            var checkedPixels = new byte[colors.Length];
            var refCol = aBorderColor;
            var nodes = new Queue<Point>();
            nodes.Enqueue(new Point(aX, aY));
            while (nodes.Count > 0)
            {
                var current = nodes.Dequeue();

                for (var i = current.x; i < w; i++)
                {
                    if (checkedPixels[i + current.y * w] > 0 || colors[i + current.y * w] == refCol)
                        break;
                    colors[i + current.y * w] = aFillColor;
                    checkedPixels[i + current.y * w] = 1;
                    if (current.y + 1 < h)
                    {
                        if (checkedPixels[i + current.y * w + w] == 0 && colors[i + current.y * w + w] != refCol)
                            nodes.Enqueue(new Point(i, current.y + 1));
                    }
                    if (current.y - 1 >= 0)
                    {
                        if (checkedPixels[i + current.y * w - w] == 0 && colors[i + current.y * w - w] != refCol)
                            nodes.Enqueue(new Point(i, current.y - 1));
                    }
                }
                for (var i = current.x - 1; i >= 0; i--)
                {
                    if (checkedPixels[i + current.y * w] > 0 || colors[i + current.y * w] == refCol)
                        break;
                    colors[i + current.y * w] = aFillColor;
                    checkedPixels[i + current.y * w] = 1;
                    if (current.y + 1 < h)
                    {
                        if (checkedPixels[i + current.y * w + w] == 0 && colors[i + current.y * w + w] != refCol)
                            nodes.Enqueue(new Point(i, current.y + 1));
                    }
                    if (current.y - 1 >= 0)
                    {
                        if (checkedPixels[i + current.y * w - w] == 0 && colors[i + current.y * w - w] != refCol)
                            nodes.Enqueue(new Point(i, current.y - 1));
                    }
                }
            }
            aTex.SetPixels(colors);
        }
    }
}