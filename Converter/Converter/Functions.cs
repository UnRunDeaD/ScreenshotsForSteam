﻿using BespokeFusion;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Converter
{
    public static class Functions
    {
        private static string[] GetScreenshots;
        private static int ConvertedFiles = 1;
        public static void SortScreenshots(string ImpPath)
        {
            try
            {
                GetScreenshots = Directory.EnumerateFiles(ImpPath, "*.*", SearchOption.TopDirectoryOnly).Where(s => s.EndsWith(".png") || s.EndsWith(".jpg")).ToArray();

                for (int i = 1; i < GetScreenshots.Length; i++)
                {
                    string file = GetScreenshots[i];

                    DateTime temp = File.GetCreationTime(file);

                    int j = i - 1;

                    DateTime next = File.GetCreationTime(GetScreenshots[j]);

                    while (j >= 0 && next > temp)
                    {
                        GetScreenshots[j + 1] = GetScreenshots[j];
                        j--;
                    }

                    GetScreenshots[j + 1] = file;
                }
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show(ex.ToString(), "ERROR OCCURED");
            }
        }
        public static void ConvertAndExport(string SteamPath, bool deleteFiles)
        {
            try
            {
                foreach (string file in GetScreenshots)
                {
                    DateTime time = File.GetCreationTime(file);
                    string name = time.ToString("yyyy-MM-dd") + "_0000" + ConvertedFiles.ToString();
                    string Steam = $"{SteamPath}\\{name}.jpg";
                    string SteamVR = $"{SteamPath}\\{name}_vr.jpg";

                    Image png = png = Image.FromFile(file);
                    png.Save(Steam, System.Drawing.Imaging.ImageFormat.Jpeg);
                    png.Save(SteamVR, System.Drawing.Imaging.ImageFormat.Jpeg);
                    png.Dispose();

                    File.SetCreationTime(Steam, File.GetCreationTime(file));
                    File.SetLastWriteTime(Steam, File.GetLastWriteTime(file));

                    File.SetCreationTime(SteamVR, File.GetCreationTime(file));
                    File.SetLastWriteTime(SteamVR, File.GetLastWriteTime(file));

                    ConvertedFiles++;

                    if (deleteFiles == true)
                        File.Delete(file);
                }
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show(ex.ToString(), "ERROR OCCURED");
            }
        }
    }
}
