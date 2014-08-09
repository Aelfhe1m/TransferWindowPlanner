﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

using KSP;
using UnityEngine;
using KSPPluginFramework;

namespace LaunchWindowPlanner
{
    internal class Resources
    {
        internal static String PathPlugin = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        internal static String PathPluginToolbarIcons = string.Format("{0}/ToolbarIcons", PathPlugin);
        internal static String PathPluginTextures = string.Format("{0}/Textures", PathPlugin);
        //internal static String PathPluginData = string.Format("{0}/Data", PathPlugin);
        //internal static String PathPluginSounds = string.Format("{0}/Sounds", PathPlugin);


        internal static Texture2D texPanel = new Texture2D(16, 16, TextureFormat.ARGB32, false);
        internal static Texture2D texBarBlue = new Texture2D(13, 13, TextureFormat.ARGB32, false);
        internal static Texture2D texBarBlue_Back = new Texture2D(13, 13, TextureFormat.ARGB32, false);

        internal static Texture2D btnSettingsAttention = new Texture2D(17, 16, TextureFormat.ARGB32, false);

        internal static Texture2D btnDropDown = new Texture2D(10, 10, TextureFormat.ARGB32, false);

        internal static Texture2D texBox = new Texture2D(9, 9, TextureFormat.ARGB32, false);
        internal static Texture2D texBoxUnity = new Texture2D(9, 9, TextureFormat.ARGB32, false);

        internal static Texture2D texSeparatorV = new Texture2D(6, 2, TextureFormat.ARGB32, false);
        internal static Texture2D texSeparatorH = new Texture2D(2, 20, TextureFormat.ARGB32, false);


        internal static Texture2D texPorkChopAxis = new Texture2D(306, 305, TextureFormat.ARGB32, false);
        internal static Texture2D texPorkChopLegend = new Texture2D(20, 300, TextureFormat.ARGB32, false);
        internal static Texture2D texPorkChopExample = new Texture2D(300, 300, TextureFormat.ARGB32, false);


        internal static void LoadTextures()
        {
            MonoBehaviourExtended.LogFormatted("Loading Textures");

            LoadImageFromFile(ref texPanel, "img_PanelBack.png");

            LoadImageFromFile(ref texBarBlue, "img_BarBlue.png");
            LoadImageFromFile(ref texBarBlue_Back, "img_BarBlue_Back.png");

            LoadImageFromFile(ref btnSettingsAttention, "img_buttonSettingsAttention.png");

            LoadImageFromFile(ref btnDropDown, "img_DropDown.png");

            LoadImageFromFile(ref texBox, "tex_Box.png");
            LoadImageFromFile(ref texBoxUnity, "tex_BoxUnity.png");

            LoadImageFromFile(ref texSeparatorH, "img_SeparatorHorizontal.png");
            LoadImageFromFile(ref texSeparatorV, "img_SeparatorVertical.png");

            LoadImageFromFile(ref texPorkChopAxis, "img_PorkChopAxis.png");
            LoadImageFromFile(ref texPorkChopLegend, "img_PorkChopLegend.png");
            LoadImageFromFile(ref texPorkChopExample, "img_PorkChopExample.png");

        }


        #region Util Stuff
        /// <summary>
        /// Loads a texture from the file system directly
        /// </summary>
        /// <param name="tex">Unity Texture to Load</param>
        /// <param name="FileName">Image file name</param>
        /// <param name="FolderPath">Optional folder path of image</param>
        /// <returns></returns>
        public static Boolean LoadImageFromFile(ref Texture2D tex, String FileName, String FolderPath = "")
        {
            //DebugLogFormatted("{0},{1}",FileName, FolderPath);
            Boolean blnReturn = false;
            try
            {
                if (FolderPath == "") FolderPath = PathPluginTextures;

                //File Exists check
                if (System.IO.File.Exists(String.Format("{0}/{1}", FolderPath, FileName)))
                {
                    try
                    {
                        MonoBehaviourExtended.LogFormatted_DebugOnly("Loading: {0}", String.Format("{0}/{1}", FolderPath, FileName));
                        tex.LoadImage(System.IO.File.ReadAllBytes(String.Format("{0}/{1}", FolderPath, FileName)));
                        blnReturn = true;
                    }
                    catch (Exception ex)
                    {
                        MonoBehaviourExtended.LogFormatted("Failed to load the texture:{0} ({1})", String.Format("{0}/{1}", FolderPath, FileName), ex.Message);
                    }
                }
                else
                {
                    MonoBehaviourExtended.LogFormatted("Cannot find texture to load:{0}", String.Format("{0}/{1}", FolderPath, FileName));
                }


            }
            catch (Exception ex)
            {
                MonoBehaviourExtended.LogFormatted("Failed to load (are you missing a file):{0} ({1})", String.Format("{0}/{1}", FolderPath, FileName), ex.Message);
            }
            return blnReturn;
        }
        #endregion

    }
}
