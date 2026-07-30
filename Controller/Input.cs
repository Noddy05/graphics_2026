
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Graphics2026.Controller
{
    internal static class Input
    {
        private static Window window;

        static Input()
        {
            window = Program.GetWindow();
        }

        public static Vector2 Direction()
        {
            int x = 0;
            if (window.IsKeyDown(Keys.A) || window.IsKeyDown(Keys.Left))
                x--;
            if (window.IsKeyDown(Keys.D) || window.IsKeyDown(Keys.Right))
                x++;

            int y = 0;
            if (window.IsKeyDown(Keys.W) || window.IsKeyDown(Keys.Up))
                y++;
            if (window.IsKeyDown(Keys.S) || window.IsKeyDown(Keys.Down))
                y--;

            return new Vector2(x, y);
        }

        public static float Scroll() => window.MouseState.ScrollDelta.Y;

        public static Vector2 MouseDelta() => window.MouseState.Position - window.MouseState.PreviousPosition;

        public static bool GetKey(Keys key) => window.IsKeyDown(key);
        public static bool GetKeyDown(Keys key) => window.IsKeyPressed(key);
        public static bool GetKeyUp(Keys key) => window.IsKeyReleased(key);

        public static bool GetNumberKey(int number)
        {
            if (number < 0 || number > 9)
                return false;

            return GetKey((Keys)(48 + number));
        }
        public static bool GetNumberKeyDown(int number)
        {
            if (number < 0 || number > 9)
                return false;

            return GetKeyDown((Keys)(48 + number));
        }
        public static bool GetNumberKeyUp(int number)
        {
            if (number < 0 || number > 9)
                return false;

            return GetKeyUp((Keys)(48 + number));
        }

        public static bool GetMouseButton(MouseButton button) => window.IsMouseButtonDown(button);
        public static bool GetMouseButtonDown(MouseButton button) => window.IsMouseButtonPressed(button);
        public static bool GetMouseButtonUp(MouseButton button) => window.IsMouseButtonReleased(button);

        public static bool GetKeyComboDown(params Keys[] keys)
        {
            bool isPressed = false;
            if (keys.Length == 0)
                return isPressed;

            isPressed = GetKeyDown(keys[keys.Length - 1]);
            for(int i = 0; i < keys.Length - 1; i++) {
                isPressed &= GetKey(keys[i]);
            }

            return isPressed;
        }

        public static bool ToggleProfilerRecord() => GetKeyComboDown(Keys.F3, Keys.R);
        public static bool PrintProfilerStats() => GetKeyComboDown(Keys.F3, Keys.P);
        public static bool Undo() => GetKeyComboDown(Keys.LeftControl, Keys.Z);
        public static bool Redo() => GetKeyComboDown(Keys.LeftControl, Keys.Y);
    }
}
