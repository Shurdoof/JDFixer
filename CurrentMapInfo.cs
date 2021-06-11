using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JDFixer
{
    public static class CurrentMapInfo
    {
        private static float _jumpDistance = 0f;
        public static float JumpDistance
        {
            get => _jumpDistance;
            set
            {
                _jumpDistance = value;
                UI.ModifierUI.instance.UpdateJumpDistance(value);
            }
        }
    }
}
