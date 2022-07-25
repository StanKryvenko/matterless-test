using System.Numerics;

namespace Helpers
{
    public class Input
    {
        private static bool _isTouched;
        
        public static bool TryGetTouchedPosition(out Vector2 touchPosition)
        {
            if (UnityEngine.Input.GetMouseButtonUp(0))
                _isTouched = false;
            
            if (!_isTouched && UnityEngine.Input.GetMouseButtonDown(0))
            {
                var mousePos = UnityEngine.Input.mousePosition;
                touchPosition = new Vector2(mousePos.x, mousePos.y);
                _isTouched = true;
                return true;
            }
            touchPosition = default;
            return false;
        }
    }
}