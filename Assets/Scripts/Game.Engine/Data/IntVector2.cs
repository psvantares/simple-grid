using System;
using UnityEngine;

namespace Game.Engine.Data
{
    [Serializable]
    public struct IntVector2 : IEquatable<IntVector2>
    {
        public static readonly IntVector2 One = new IntVector2(1, 1);
        public static readonly IntVector2 Zero = new IntVector2(0, 0);
        public int x;
        public int y;

        public int SqrMagnitude => (x * x) + (y * y);

        public float Magnitude => Mathf.Sqrt(SqrMagnitude);

        public int ManhattanDistance => Mathf.Abs(x) + Mathf.Abs(y);

        public IntVector2(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public bool Equals(IntVector2 other)
        {
            return other.x == x && other.y == y;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return obj is IntVector2 vector2 && Equals(vector2);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (x.GetHashCode() * 92821) ^ (y.GetHashCode() * 31);
            }
        }

        public override string ToString()
        {
            return $"X: {x}, Y: {y}";
        }

        public static bool operator ==(IntVector2 left, IntVector2 right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(IntVector2 left, IntVector2 right)
        {
            return !left.Equals(right);
        }

        public static implicit operator Vector2(IntVector2 vector)
        {
            return new Vector2(vector.x, vector.y);
        }

        public static explicit operator IntVector2(Vector2 vector)
        {
            return new IntVector2((int)vector.x, (int)vector.y);
        }

        public static IntVector2 operator +(IntVector2 left, IntVector2 right)
        {
            return new IntVector2(left.x + right.x, left.y + right.y);
        }

        public static IntVector2 operator -(IntVector2 left, IntVector2 right)
        {
            return new IntVector2(left.x - right.x, left.y - right.y);
        }

        public static IntVector2 operator *(int scale, IntVector2 left)
        {
            return new IntVector2(left.x * scale, left.y * scale);
        }

        public static IntVector2 operator *(IntVector2 left, int scale)
        {
            return new IntVector2(left.x * scale, left.y * scale);
        }

        public static Vector2 operator *(float scale, IntVector2 left)
        {
            return new Vector2(left.x * scale, left.y * scale);
        }

        public static Vector2 operator *(IntVector2 left, float scale)
        {
            return new Vector2(left.x * scale, left.y * scale);
        }

        public static IntVector2 operator -(IntVector2 left)
        {
            return new IntVector2(-left.x, -left.y);
        }
    }
}