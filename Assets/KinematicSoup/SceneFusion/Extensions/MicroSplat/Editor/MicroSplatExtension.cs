using System;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using KS.SceneFusion.API;

namespace KS.SceneFusion.Extensions
{
    /**
     * Makes Scene Fusion and MicroSplat play nice together.
     * 
     * NOTE: If you get compilation errors because MicroSplatObject is not defined, go to
     * Project Settings > Player > Scripting Define Symbols, remove SF_MICROSPLAT and press enter.
     */
    [InitializeOnLoad]
    class MicroSplatExtension
    {
#if SF_MICROSPLAT
        /**
         * Initialization
         */
        static MicroSplatExtension()
        {
            sfUtility.SyncHiddenProperties<MicroSplatObject>("templateMaterial");
            sfUtility.SuppressAssetDatabaseWarningFor<Material, MicroSplatObject>();
            sfUtility.OnLoadingComplete += BuildLighting;
        }

        /**
         * Builds lighting if auto-generate lightmaps is turned off.
         */
        private static void BuildLighting()
        {
            if (Lightmapping.giWorkflowMode == Lightmapping.GIWorkflowMode.OnDemand)
            {
                Lightmapping.BakeAsync();
            }
        }
#else
        /**
         * Initialization
         */
        static MicroSplatExtension()
        {
            if (DetectMicroSplat())
            {
                sfUtility.SetDefineSymbol("SF_MICROSPLAT");
            }
        }

        private static bool DetectMicroSplat()
        {
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (assembly.GetType("MicroSplatObject") != null)
                {
                    return true;
                }
            }
            return false;
        }
#endif
    }
}
