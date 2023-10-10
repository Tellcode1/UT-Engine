namespace UnityEngine
{
    /// <summary>
    /// Added functionality to Vector3's which I don't know why Unity hasn't implemented.
    /// </summary>
    public static class Vector3Plus
    {
        public static Vector3 Multiply(Vector3 a, Vector3 b)
        {
            //Vector3 x = a + b;
            return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
        }

        public static Vector3 Clamp(Vector3 a, Vector3 b)
        {
            if (a.x > b.x || a.y > b.y || a.z > b.z) return b;
            else return a;
        }

        public static Vector3 ComplexClamp(Vector3 value, Vector3 max)
        {
            return new()
            {
                x = Mathf.Clamp(value.x, -max.x, max.x),
                y = Mathf.Clamp(value.y, -max.y, max.y),
                z = Mathf.Clamp(value.z, -max.z, max.z)
            };
        } 
        public static Vector2 ComplexClamp(Vector2 value, Vector2 max)
        {
            
            return new Vector2()
            {
                x = Mathf.Clamp(value.x, -max.x, max.x),
                y = Mathf.Clamp(value.y, -max.y, max.y),
            };

        }
        public static Vector3 SubtractFloat(Vector3 a, float b)
        {
            return new Vector3(a.x - b, a.y - b, a.z - b);
        }

        public static Vector3 MoveTo0(Vector3 a, float MaxChangePerFrame)
        {
            Vector3 diff = SubtractFloat(a, MaxChangePerFrame);
            for (; Check0(diff);)
            {
                if (diff.x > 0) diff.x -= MaxChangePerFrame;
                if (diff.y > 0) diff.y -= MaxChangePerFrame;
                if (diff.z > 0) diff.z -= MaxChangePerFrame;
            }
            return diff;
        }

        public static Vector3 Negative(Vector3 value)
        {
            return value * -1;
        }
        public static Vector3 Convert(float a)
        {
            return new Vector3(a, 0, a);
        }
        //public static Vector3 Drop(Vector3 Current, Vector3 Target, float MaxChangePerFrame)
        //{
        //    Vector3 diff = Target - Current;
        //    Vector3 output;
        //    switch(diff.x, diff.y, diff.z)
        //    {
        //        // Case: x > 0 (Current still has way to go) y <= 0, z <= 0 so y and z will be sent as target to avoid overshooting
        //        // > = diff; <= = Target;
        //            case ( > 0, <= 0, <= 0):
        //            {
        //                output.x = diff.x;
        //                output.y = Target.y;
        //                output.z = Target.z;
        //                break;
        //            }

        //            case ( <= 0, > 0, <= 0):
        //                {
        //                   output.x = Target.x;
        //                   output.y = diff.y;
        //                   output.z = Target.z;
        //                   break;
        //              }
        //            case ( <= 0, <= 0, > 0):
        //            {
        //                    output.x = Target.x;
        //                    output.y = Target.y;
        //                    output.z = diff.z;
        //                    break;
        //            }
        //            case ( > 0, <= 0, > 0):
        //            {
        //                output.x = diff.x;
        //                output.y = Target.y;
        //                output.z = diff.z;
        //                break;
        //            }
        //            case (> 0, > 0, <= 0):
        //            {
        //                output.x = diff.x;
        //                output.y = diff.y;
        //                output.z = Target.z;
        //                break;
        //            }
        //            case ( <= 0, <= 0, <= 0):
        //            {
        //                output.x = Target.x;
        //                output.y = Target.y;
        //                output.z = Target.z;
        //                    break;
        //            }
        //            case ( > 0, > 0, > 0):
        //            {
        //                output.x = diff.x;
        //                output.y = diff.y;
        //                output.z = diff.z;
        //                break;
        //            }
        //            default:
        //            {
        //                    output = default;
        //                    break;
        //            }
        //    }
        //    return output;
        //}
        public static bool Compare(Vector3 a, Vector3 b)
        {
            return a.x >= b.x || a.y >= b.y || a.z >= b.z;
        }
        public static bool Check0(Vector3 a)
        {
            return a.x > 0 || a.y > 0 || a.z > 0;
            //Vector3 pips = Vector3.one + Vector3.zero;
            // return Compare(a, Vector3.zero);
        }
    }

    //public class Vector3PlusHelper
    //{
    //    public static implicit operator Vector3(Vector3PlusHelper x)
    //    {
    //        return Vector3.zero;
    //    }
    //    public static implicit operator Vector3PlusHelper(Vector3 x)
    //    {
    //        return null;
    //    }
    //    //[MethodImpl(MethodImplOptions.AggressiveInlining)]
    //    //public static Vector3 operator +(Vector3 a, Vector3 b)
    //    //{
    //    //    return new Vector3(a.x + b.x, a.y + b.y, a.z + b.z);
    //    //}
    //}
}
