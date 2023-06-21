using System;
using UnityEngine;

namespace Game.Engine.Data
{
    [Serializable]
    public struct IntVector2 : IEquatable<IntVector2>
    {
        public static readonly IntVector2 One = new(1, 1);
        public static readonly IntVector2 Zero = new(0, 0);
        public int X;
        public int Y;

        public int SqrMagnitude => (X * X) + (Y * Y);

        public float Magnitude => Mathf.Sqrt(SqrMagnitude);

        public int ManhattanDistance => Mathf.Abs(X) + Mathf.Abs(Y);

        public IntVector2(int x, int y)
        {
            X = x;
            Y = y;
        }

        public bool Equals(IntVector2 other)
        {
            return other.X == X && other.Y == Y;
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
                return (X.GetHashCode() * 92821) ^ (Y.GetHashCode() * 31);
            }
        }

        public override string ToString()
        {
            return $"X: {X}, Y: {Y}";
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
            return new Vector2(vector.X, vector.Y);
        }

        public static explicit operator IntVector2(Vector2 vector)
        {
            return new IntVector2((int)vector.x, (int)vector.y);
        }

        public static IntVector2 operator +(IntVector2 left, IntVector2 right)
        {
            return new IntVector2(left.X + right.X, left.Y + right.Y);
        }

        public static IntVector2 operator -(IntVector2 left, IntVector2 right)
        {
            return new IntVector2(left.X - right.X, left.Y - right.Y);
        }

        public static IntVector2 operator *(int scale, IntVector2 left)
        {
            return new IntVector2(left.X * scale, left.Y * scale);
        }

        public static IntVector2 operator *(IntVector2 left, int scale)
        {
            return new IntVector2(left.X * scale, left.Y * scale);
        }

        public static Vector2 operator *(float scale, IntVector2 left)
        {
            return new Vector2(left.X * scale, left.Y * scale);
        }

        public static Vector2 operator *(IntVector2 left, float scale)
        {
            return new Vector2(left.X * scale, left.Y * scale);
        }

        public static IntVector2 operator -(IntVector2 left)
        {
            return new IntVector2(-left.X, -left.Y);
        }
    }
}