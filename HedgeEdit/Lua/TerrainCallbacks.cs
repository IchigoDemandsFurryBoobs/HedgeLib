﻿using HedgeLib.Models;
using HedgeLib.Terrain;
using System;
using System.IO;

namespace HedgeEdit.Lua
{
    public partial class LuaScript
    {
        // Methods
        protected void InitTerrainCallbacks()
        {
            script.Globals["LoadTerrainList"] = (Func<string, string,
                string, GensTerrainList>)LoadTerrainList;

            script.Globals["LoadTerrain"] = (Func<string, string,
                string, bool, VPModel>)LoadTerrain;

            script.Globals["LoadTerrainInstance"] = (Func<string, string,
                string, string, bool, VPModel>)LoadTerrainInstance;
        }

        // Lua Callbacks
        public GensTerrainList LoadTerrainList(string path, string groupsDir, string resDir)
        {
            // Format path strings, return if the given files/directories don't exist
            path = FormatCacheDir(path);
            groupsDir = FormatCacheDir(groupsDir);
            resDir = FormatCacheDir(resDir);

            if (!File.Exists(path) || !Directory.Exists(groupsDir) ||
                !Directory.Exists(resDir))
                return null;

            return Data.LoadTerrainList(path, groupsDir, resDir);
        }

        public VPModel LoadTerrain(string path, string resDir,
            string group = null, bool loadMats = true)
        {
            // Format path strings, return if the given files/directories don't exist
            path = FormatCacheDir(path);
            resDir = FormatCacheDir(resDir);

            if (!File.Exists(path) || !Directory.Exists(resDir))
                return null;

            // Terrain Model
            return Data.LoadModel(path, resDir,
                true, loadMats, group, false);
        }

        public VPModel LoadTerrainInstance(string path, string resDir, string dir,
            string group = null, bool loadMats = true)
        {
            // Format path strings, return if the given files/directories don't exist
            path = FormatCacheDir(path);
            resDir = FormatCacheDir(resDir);
            dir = FormatCacheDir(dir);

            if (!File.Exists(path) || !Directory.Exists(resDir) || !Directory.Exists(dir))
                return null;

            // Load Terrain Instance Info
            var instInfo = new GensTerrainInstanceInfo();
            instInfo.Load(path);

            string name = string.Format("{0}{1}", instInfo.ModelFileName,
                GensModel.TerrainExtension);

            return Data.LoadModel(Path.Combine(dir, name), resDir,
                true, loadMats, group, false, instInfo);
        }
    }
}