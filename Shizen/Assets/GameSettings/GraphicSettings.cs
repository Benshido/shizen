using System.Collections.Generic;
using System.Collections.ObjectModel;

public static class GraphicSettings
{
    public static IReadOnlyList<int> FramerateOptions => _framerateOptions;
    private static List<int> _framerateOptions = new () {7, 30, 60, 90, 120 };

    public static int maxFPS = FramerateOptions[1];

    public static bool VSync = false;
}
