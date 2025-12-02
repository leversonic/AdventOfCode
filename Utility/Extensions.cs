namespace AdventOfCode.Utility;

public static class Extensions {
    public static string Reverse(this string str) {
        return new(str.Reverse<char>().ToArray());
    }
}