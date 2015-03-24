using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MissAndCan
{
    public enum BoatState
    {
        Right = 1,
        Left = -1
    }
    public static class EnumHelper
    {
        public static string PositionAsString(this BoatState state)
        {
            switch (state)
            {
                case BoatState.Left:
                    return "<-BOAT LEFT";
                case BoatState.Right:
                    return "BOAT RIGHT->";
            }
            return string.Empty;
        }

        public static string StatusAsString(this BoatState state)
        {
            switch (state)
            {
                case BoatState.Left:
                    return "LEFT";
                case BoatState.Right:
                    return "RIGHT";
            }
            return string.Empty;
        }

        /// <summary>
        /// We use this in problem processing.
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public static int AsMultiplier(this BoatState state)
        {
            switch (state)
            {
                case BoatState.Left:
                    return -1;
                case BoatState.Right:
                    return 1;
            }
            return 0;
        }

        public static BoatState Inverse(this BoatState state)
        {
            switch (state)
            {
                case BoatState.Left:
                    return BoatState.Right;
                case BoatState.Right:
                    return BoatState.Left;
            }
            return BoatState.Left;
        }
    }
}
