public static class MiyuTooltips
{
    // Formatting
    public static readonly string FormatDanger  = "<color=red>";
    public static readonly string FormatWarning = "<color=yellow>";
    public static readonly string FormatSpecial = "<color=blue>";

    // General
    public static readonly string TipWindowReset =
        FormatDanger + "Reset all variables in the current window.";

    // Tool Window

    // Core
    public static readonly string TipCompile = 
        "Recompile all assembly scripts in the Unity project.";
    public static readonly string TipStatics = 
        "Reset static classes to their default values.";

    // Play Mode
    public static readonly string TipScene = 
        "Load the current active scene.";
    public static readonly string TipSpawn =
        "Instantiates the object set in the instantiate settings window.";

    // Instantiate Window
    public static readonly string TipRandomPos =
        "Randomize vectors of the spawn <u>position</u>.";
    public static readonly string TipRandomRot =
        "Randomize vectors of the spawn <u>rotation</u>.";
    
}
