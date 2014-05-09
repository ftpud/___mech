using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathHelper
{
    public class Vector2
    {
        public float X;
        public float Y;
        public Vector2(float X, float Y)
        {
            this.X = X;
            this.Y = Y;
        }

        public static Vector2 operator +(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.X + v2.X, v1.Y + v2.Y);
        }

        public static Vector2 operator -(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.X - v2.X, v1.Y - v2.Y);
        }

        public static Vector2 operator *(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.X * v2.X, v1.Y * v2.Y);
        }

        public static Vector2 operator *(Vector2 v1, float val)
        {
            return new Vector2(v1.X * val, v1.Y * val);
        }

        public static Vector2 GetForward(float Angle)
        {
            return new Vector2((float)Math.Cos(Angle), (float)Math.Sin(Angle));
        }
    }  
}
