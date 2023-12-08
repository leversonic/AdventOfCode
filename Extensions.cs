namespace ADL.AdventOfCode2023;

public static class Extensions {
    public static string Reverse(this string str) {
        return new string(str.Reverse<char>().ToArray());
    }
}