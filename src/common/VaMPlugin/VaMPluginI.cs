using SimpleJSON;

namespace Vs1Plugin
{
    public interface VaMPluginI
    {
        JSONClass GetJSONBase(bool includePhysical = true, bool includeAppearance = true, bool forceStore = false);

        void RestoreFromJSONBase(JSONClass jc, bool restorePhysical = true, bool restoreAppearance = true, JSONArray presetAtoms = null, bool setMissingToDefault = true);
        void LateRestoreFromJSONBase(JSONClass jc, bool restorePhysical = true, bool restoreAppearance = true, bool setMissingToDefault = true);
    }
}