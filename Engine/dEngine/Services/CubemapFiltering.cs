// CubemapFiltering.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using dEngine.Data;
using dEngine.Instances;
using dEngine.Instances.Attributes;
using dEngine.Settings.Global;
using dEngine.Utility;

namespace dEngine.Services
{
    /// <summary>
    /// A service for interfacting with the cubemap filtering tool.
    /// </summary>
    [ExplorerOrder(-1)]
    internal class CubemapFiltering : Service
    {
        private static string _cmft;
        private static string _filteringPath;
        private const string _outputParams = "dds,BGR8,cubemap";
        private static MemoizingMRUCache<Cubemap, Cubemap> _irradianceCache;
        private static MemoizingMRUCache<Cubemap, Cubemap> _radianceCache;

        internal static Service Service;

        public CubemapFiltering()
        {
            Service = this;

            _irradianceCache = new MemoizingMRUCache<Cubemap, Cubemap>((key, c) => LoadCubemap(IrradianceFilter(key.Texture.SaveDds()).Result), int.MaxValue);
            _radianceCache = new MemoizingMRUCache<Cubemap, Cubemap>((key, c) => LoadCubemap(RadianceFilter(key.Texture.SaveDds()).Result), int.MaxValue);

            _filteringPath = Path.Combine(Engine.TempPath, "Filtering");
            Directory.CreateDirectory(_filteringPath);

            Engine.Logger.Info("Unpacking cmft.exe");
            var cmftRaw = ContentProvider.DownloadStream(new Uri("internal://content/NativeLibraries/cmft.exe")).Result;
            _cmft = Path.Combine(Engine.TempPath, "cmft.exe");
            using (var exe = File.Create(_cmft))
            {
                cmftRaw.CopyTo(exe);
            }
        }

        private Cubemap LoadCubemap(string file)
        {
            using (var stream = File.OpenRead(file))
            {
                var cubemap = new Cubemap();
                cubemap.Load(stream);
                return cubemap;
            }
        }

        internal static Cubemap GetIrradianceMap(Cubemap diffuseCubemap)
        {
            return _irradianceCache.Get(diffuseCubemap);
        }

        internal static Cubemap GetRadianceMap(Cubemap diffuseCubemap)
        {
            return _radianceCache.Get(diffuseCubemap);
        }

        public enum LightingModel
        {
            Phong,
            PhongBrdf,
            Blinn,
            BlinnBrdf,
        }

        /// <summary>
        /// Processes the radiance map for a texture.
        /// </summary>
        /// <param name="textureStream">A stream of TGA/DDS data.</param>
        private static async Task<string> RadianceFilter(Stream textureStream)
        {
            const int srcFaceSize = 256;
            const int dstFaceSize = 256;
            const int mipCount = 8;
            const int glossScale = 8;
            const int glossBias = 1;
            const int numCpuProcessingThreads = 4;
            const LightingModel lightingModel = LightingModel.BlinnBrdf; // cmft doesn't support GGX yet
            const float inputGammaNumerator = 1.0f;
            const float inputGammaDenominator = 1.0f;
            const float outputGammaNumerator = 1.0f;
            const float outputGammaDenominator = 1.0f;
            const bool useOpenCL = true;
            const bool excludeBase = false;
            const bool generateMipChain = false;

            return await Task.Run(() =>
            {
                var tempInput = Path.Combine(_filteringPath, $"{Guid.NewGuid():N}.dds");
                var tempOutput = Path.Combine(_filteringPath, $"{Guid.NewGuid():N}");
                using (var inputStream = File.Create(tempInput))
                {
                    textureStream.CopyTo(inputStream);
                }
                var proc = Process.Start(new ProcessStartInfo
                {
                    FileName = _cmft,
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardError = true,
                    Arguments = $"--input {tempInput} --filter radiance --srcFaceSize {srcFaceSize} --dstFaceSize {dstFaceSize} --excludeBase {excludeBase} --generateMipChain {generateMipChain} --mipCount {mipCount} --glossScale {glossScale} --glossBias {glossBias} --lightingModel {lightingModel} --numCpuProcessingThreads {numCpuProcessingThreads} --useOpenCL {useOpenCL} --clVendor anyGpuVendor --deviceType gpu --deviceIndex {RenderSettings.GraphicsAdapter} --inputGammaNumerator {inputGammaNumerator} --inputGammaDenominator {inputGammaDenominator} --outputGammaNumerator {outputGammaNumerator} --outputGammaDenominator {outputGammaDenominator} --outputNum 1 --output0 {tempOutput} --output0params {_outputParams}"
                });
                Debug.Assert(proc != null, "proc != null");
                while (!proc.StandardError.EndOfStream)
                {
                    Engine.Logger.Error(proc.StandardError.ReadLine());
                }
                proc.WaitForExit();
                File.Delete(tempInput);
                return tempOutput + ".dds";
            });
        }

        /// <summary>
        /// Processes the irradiance map for a texture.
        /// </summary>
        /// <param name="textureStream">A stream of TGA/DDS data.</param>
        private static async Task<string> IrradianceFilter(Stream textureStream)
        {
            return await Task.Run(() =>
            {
                var tempInput = Path.Combine(_filteringPath, $"{Guid.NewGuid():N}.dds");
                var tempOutput = Path.Combine(_filteringPath, $"{Guid.NewGuid():N}");
                using (var inputStream = File.Create(tempInput))
                {
                    textureStream.CopyTo(inputStream);
                }
                var proc = Process.Start(new ProcessStartInfo
                {
                    FileName = _cmft,
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardError = true,
                    Arguments = $"--input {tempInput} --filter irradiance --outputNum 1 --output0 {tempOutput} --outputOparams {_outputParams}"
                });
                Debug.Assert(proc != null, "proc != null");
                while (!proc.StandardError.EndOfStream)
                {
                    Engine.Logger.Error(proc.StandardError.ReadLine());
                }
                proc.WaitForExit();
                File.Delete(tempInput);
                return tempOutput + ".dds";
            });
        }

        internal static CubemapFiltering GetExisting()
        {
            return DataModel.GetService<CubemapFiltering>();
        }
    }
}