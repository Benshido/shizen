using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum KeyBindName
{
    ExitUITab,
    Interact,
    Inventory,
    CharacterStats,
    SkillsUI,
    Settings,
    Menu,
    MoveForward,
    MoveBack,
    MoveLeft,
    MoveRight,
    MoveUp,
    MoveDown,
    Jump,
    WalkMode,
    FlyMode,
    NextElement,
    PrevElement,
    NAttk
}

public class KeyBind
{
    public KeyBindName Name { get; private set; }
    public KeyCode MainKeyCode { get; private set; }
    public KeyCode? CombiKeyCode { get; private set; }

    public KeyBind(KeyBindName name, KeyCode code, KeyCode? combiKeyCode = null)
    {
        this.Name = name;
        this.MainKeyCode = code;
        this.CombiKeyCode = combiKeyCode;
    }

    public bool IsPressed
    {
        get
        {
            if (Input.GetKey(MainKeyCode))
            {
                if (CombiKeyCode == null || Input.GetKey((KeyCode)CombiKeyCode))
                {
                    return true;
                }
            }
            return false;
        }
    }

    public bool IsPressedDown
    {
        get
        {
            if (Input.GetKeyDown(MainKeyCode))
            {
                if (CombiKeyCode == null || Input.GetKey((KeyCode)CombiKeyCode))
                {
                    return true;
                }
            }
            return false;
        }
    }
  
    public void AlterKeybind(KeyCode keyCode, KeyCode? combiKey = null)
    {
        this.MainKeyCode = keyCode;
        this.CombiKeyCode = combiKey;
    }

    public float TimePressed = 0;
}
public static class Keybinds
{
    public static List<KeyBind> KeyBinds
    {
        get
        {
            return new List<KeyBind>()
            {
                ExitUITab,
                Interact,
                InventoryUI,
                CharacterStatsUI,
                SkillsUI,
                SettingsUI,
                MenuUI,

                MoveForward,
                MoveBack,
                MoveLeft,
                MoveRight,
                MoveUp,
                MoveDown,
                Jump,
                WalkMode,
                FlyMode,
                NextElement,
                PrevElement,
                NAttk,
            };
        }
    }
    public static KeyBind ExitUITab { get { return _exitUITab; } }
    private static KeyBind _exitUITab = new(KeyBindName.ExitUITab, KeyCode.Escape);
    public static KeyBind Interact { get { return _interact; } }
    private static KeyBind _interact = new(KeyBindName.Interact, KeyCode.F);
    public static KeyBind InventoryUI { get { return _inventoryUI; } }
    private static KeyBind _inventoryUI = new(KeyBindName.Inventory, KeyCode.B);
    public static KeyBind CharacterStatsUI { get { return _characterStatsUI; } }
    private static KeyBind _characterStatsUI = new(KeyBindName.CharacterStats, KeyCode.P);

    public static KeyBind SkillsUI { get { return _skillsUI; } }
    private static KeyBind _skillsUI = new(KeyBindName.SkillsUI, KeyCode.K);
    public static KeyBind SettingsUI { get { return _settingsUI; } }
    private static KeyBind _settingsUI = new(KeyBindName.Settings, KeyCode.O);
    public static KeyBind MenuUI { get { return _menuUI; } }
    private static KeyBind _menuUI = new(KeyBindName.Menu, KeyCode.Escape);


    public static KeyBind MoveForward { get { return _moveForward; } }
    private static KeyBind _moveForward = new(KeyBindName.MoveForward, KeyCode.W);
    public static KeyBind MoveBack { get { return _moveBack; } }
    private static KeyBind _moveBack = new(KeyBindName.MoveBack, KeyCode.S);
    public static KeyBind MoveLeft { get { return _moveLeft; } }
    private static KeyBind _moveLeft = new(KeyBindName.MoveLeft, KeyCode.A);
    public static KeyBind MoveRight { get { return _moveRight; } }
    private static KeyBind _moveRight = new(KeyBindName.MoveRight, KeyCode.D);
    public static KeyBind MoveUp { get { return _moveUp; } }
    private static KeyBind _moveUp = new(KeyBindName.MoveUp, KeyCode.Space);
    public static KeyBind MoveDown { get { return _moveDown; } }
    private static KeyBind _moveDown = new(KeyBindName.MoveDown, KeyCode.Tab);
    public static KeyBind Jump { get { return _jump; } }
    private static KeyBind _jump = new(KeyBindName.Jump, KeyCode.Space);
    public static KeyBind WalkMode { get { return _walkMode; } }
    private static KeyBind _walkMode = new(KeyBindName.WalkMode, KeyCode.Z);
    public static KeyBind FlyMode { get { return _flyMode; } }
    private static KeyBind _flyMode = new(KeyBindName.FlyMode, KeyCode.Space);

    public static KeyBind NextElement { get { return _nextElement; } }
    private static KeyBind _nextElement= new(KeyBindName.NextElement, KeyCode.E);
    public static KeyBind PrevElement { get { return _prevElement; } }
    private static KeyBind _prevElement = new(KeyBindName.PrevElement, KeyCode.Q);
    public static KeyBind NAttk { get { return _nAttk; } }
    private static KeyBind _nAttk = new(KeyBindName.NAttk, KeyCode.Mouse0);

    public static void AlterKeybind(KeyBind newKeybind)
    {
        KeyBinds.FirstOrDefault(x => x.Name == newKeybind.Name).AlterKeybind(newKeybind.MainKeyCode, newKeybind.CombiKeyCode);
    }

}

public static class KeyType
{
    public static List<KeyCode> CombiKeys = new()
         {
        KeyCode.LeftCommand,
        KeyCode.RightCommand,
        KeyCode.LeftControl,
        KeyCode.RightControl,
        KeyCode.LeftAlt,
        KeyCode.RightAlt,
        KeyCode.AltGr,
        KeyCode.LeftShift,
        KeyCode.RightShift,
         };
    public static List<KeyCode> UnavailableKeys = new()
        {
        KeyCode.LeftWindows,
        KeyCode.RightWindows,
        KeyCode.Delete,
        KeyCode.Backspace,
        KeyCode.Return,
        };
}

public static class KeyCodeName
{
    public static string RenameKey(string keycode)
    {
        switch (keycode)
        {
            case nameof(KeyCode.Escape):
                return "Esc";
            case nameof(KeyCode.Alpha0):
                return "A0";
            case nameof(KeyCode.Alpha1):
                return "A1";
            case nameof(KeyCode.Alpha2):
                return "A2";
            case nameof(KeyCode.Alpha3):
                return "A3";
            case nameof(KeyCode.Alpha4):
                return "A4";
            case nameof(KeyCode.Alpha5):
                return "A5";
            case nameof(KeyCode.Alpha6):
                return "A6";
            case nameof(KeyCode.Alpha7):
                return "A7";
            case nameof(KeyCode.Alpha8):
                return "A8";
            case nameof(KeyCode.Alpha9):
                return "A9";
            case nameof(KeyCode.LeftControl):
                return "LCtrl";
            case nameof(KeyCode.RightControl):
                return "RCtrl";
            case nameof(KeyCode.LeftShift):
                return "LShift";
            case nameof(KeyCode.RightShift):
                return "RShift";
            case nameof(KeyCode.LeftAlt):
                return "LAlt";
            case nameof(KeyCode.RightAlt):
                return "RAlt";
            case nameof(KeyCode.UpArrow):
                return "\u2191";
            case nameof(KeyCode.DownArrow):
                return "\u2193";
            case nameof(KeyCode.LeftArrow):
                return "\u2190";
            case nameof(KeyCode.RightArrow):
                return "\u2192";
            case nameof(KeyCode.Mouse0):
                return "LMB";
            case nameof(KeyCode.Mouse1):
                return "MMB";
            case nameof(KeyCode.Mouse2):
                return "RMB";
            case nameof(KeyCode.Minus):
                return "-";
            case nameof(KeyCode.Equals):
                return "=";
            case nameof(KeyCode.Backslash):
                return @"\";
            case nameof(KeyCode.Slash):
                return "/";
            case nameof(KeyCode.KeypadMultiply):
                return "*";
            case nameof(KeyCode.Period):
                return ".";
            case nameof(KeyCode.Comma):
                return ",";
            default:
                return keycode;
        }
    }
}

