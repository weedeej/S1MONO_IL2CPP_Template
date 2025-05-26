using MelonLoader;
using UnityEngine;

namespace MONO_IL2CPP_Template.TemplateUtils
{
    public static class AssetBundleUtils
    {
        static Core mod = MelonAssembly.FindMelonInstance<Core>();
        static MelonAssembly melonAssembly = mod.MelonAssembly;
#if IL2CPP
        public static Il2CppAssetBundle LoadAssetBundle(string bundleFileName)
#elif MONO
        public static AssetBundle LoadAssetBundle(string bundleFileName)
#endif
        {
#if IL2CPP
            try
            {
                Stream bundleStream = melonAssembly.Assembly.GetManifestResourceStream($"{typeof(Core).Namespace}.Assets.{bundleFileName}");
                if (bundleStream == null)
                {
                    mod.Unregister($"AssetBundle stream not found");
                    return null;
                }
                byte[] bundleData;
                using (MemoryStream ms = new MemoryStream())
                {
                    bundleStream.CopyTo(ms);
                    bundleData = ms.ToArray();
                }
                Il2CppSystem.IO.Stream stream = new Il2CppSystem.IO.MemoryStream(bundleData);
                return Il2CppAssetBundleManager.LoadFromStream(stream);
            }
            catch (Exception e)
            {
                mod.Unregister($"Failed to load AssetBundle. Please report to dev: {e}");
                return null;
            }
#elif MONO
            try
            {
                var stream = melonAssembly.Assembly.GetManifestResourceStream($"{typeof(Core).Namespace}.Assets.{bundleFileName}");

                if (stream == null)
                {

                    mod.Unregister($"AssetBundle stream not found");
                    return null;
                }
                return AssetBundle.LoadFromStream(stream);
            }
            catch (Exception e)
            {
                mod.Unregister($"Failed to load AssetBundle. Please report to dev: {e}");
                return null;
            }
#endif
        }

        public static
#if IL2CPP
            Il2CppAssetBundle
#elif MONO
            AssetBundle
#endif
            GetLoadedAssetBundle(string asset_name_flag)
        {
#if IL2CPP

            Il2CppAssetBundle[] loadedBundles = Il2CppAssetBundleManager.GetAllLoadedAssetBundles();
#elif MONO
            AssetBundle[] loadedBundles = AssetBundle.GetAllLoadedAssetBundles().ToArray();
#endif
            if (loadedBundles.Length == 1) return loadedBundles[0];
            try
            {
                foreach (var bundle in loadedBundles)
                {
                    if (bundle.Contains(asset_name_flag)) return bundle;
                }
                throw new Exception($"Asset '{asset_name_flag}' not found in {loadedBundles.Length} bundle(s)");
            }
            catch (Exception e)
            {
                mod.Unregister($"Failed to get loaded AssetBundle. Please report to dev: {e}");
                return null;
            }

        }

        public static GameObject LoadAssetFromBundle(string asset_name)
        {
            var bundle = GetLoadedAssetBundle(asset_name);
            return bundle.LoadAsset<GameObject>(asset_name);
        }
    }
}
