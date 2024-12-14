using System.Numerics;

namespace AdventOfCode.Utility;

public class Coordinate<T>(T x, T y) : IEquatable<Coordinate<T>> where T : INumberBase<T>
{
    public T X { get; } = x;
    public T Y { get; } = y;

    public static Coordinate<T> operator +(Coordinate<T> a, Coordinate<T> b)
    {
        return new(a.X + b.X, a.Y + b.Y);
    }

    public static Coordinate<T> operator -(Coordinate<T> a, Coordinate<T> b)
    {
        return new(a.X - b.X, a.Y - b.Y);
    }

    public static Coordinate<T> operator *(Coordinate<T> a, Coordinate<T> b)
    {
        return new(a.X * b.X, a.Y * b.Y);
    }

    public static Coordinate<T> operator /(Coordinate<T> a, Coordinate<T> b)
    {
        return new(a.X / b.X, a.Y / b.Y);
    }

    public static Coordinate<T> operator +(Coordinate<T> a, T b)
    {
        return new(a.X + b, a.Y + b);
    }

    public static Coordinate<T> operator -(Coordinate<T> a, T b)
    {
        return new(a.X - b, a.Y - b);
    }

    public static Coordinate<T> operator *(Coordinate<T> a, T b)
    {
        return new(a.X * b, a.Y * b);
    }

    public static Coordinate<T> operator /(Coordinate<T> a, T b)
    {
        return new(a.X / b, a.Y / b);
    }

    public static Coordinate<T> operator +(T a, Coordinate<T> b)
    {
        return new(a + b.X, a + b.Y);
    }

    public static Coordinate<T> operator -(T a, Coordinate<T> b)
    {
        return new(a - b.X, a - b.Y);
    }

    public static Coordinate<T> operator *(T a, Coordinate<T> b)
    {
        return new(a * b.X, a * b.Y);
    }

    public static Coordinate<T> operator /(T a, Coordinate<T> b)
    {
        return new(a / b.X, a / b.Y);
    }

    public override string ToString()
    {
        return $"({X}, {Y})";
    }

    public bool Equals(Coordinate<T>? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }
        return EqualityComparer<T>.Default.Equals(X, other.X)
               && EqualityComparer<T>.Default.Equals(Y, other.Y);
    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        return obj.GetType() == GetType() && Equals((Coordinate<T>)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }
}
public class Coordinate(int x, int y) : Coordinate<int>(x, y);