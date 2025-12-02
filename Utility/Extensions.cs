using System.Text.RegularExpressions;

namespace AdventOfCode.Utility;

public static class Extensions {
    public static string Reverse(this string str) {
        return new(str.Reverse<char>().ToArray());
    }

    public static IEnumerable<Tuple<T>> Parse<T>(
        this IEnumerable<string> lines,
        Regex lineRegex,
        Func<string, T> parser
    )
    {
        var lineMatches = lines.Select(line => lineRegex.Match(line));

        return lineMatches.Select(lineMatch => Tuple.Create(
            parser(lineMatch.Groups[1].Value)
        ));
    }

    public static IEnumerable<Tuple<T1, T2>> Parse<T1, T2>(
        this IEnumerable<string> lines,
        Regex lineRegex,
        Func<string, T1> parser1,
        Func<string, T2> parser2
    )
    {
        var lineMatches = lines.Select(line => lineRegex.Match(line));

        return lineMatches.Select(lineMatch => Tuple.Create(
            parser1(lineMatch.Groups[1].Value),
            parser2(lineMatch.Groups[2].Value)
        ));
    }

    public static IEnumerable<Tuple<T1, T2, T3>> Parse<T1, T2, T3>(
        this IEnumerable<string> lines,
        Regex lineRegex,
        Func<string, T1> parser1,
        Func<string, T2> parser2,
        Func<string, T3> parser3
    )
    {
        var lineMatches = lines.Select(line => lineRegex.Match(line));

        return lineMatches.Select(lineMatch => Tuple.Create(
            parser1(lineMatch.Groups[1].Value),
            parser2(lineMatch.Groups[2].Value),
            parser3(lineMatch.Groups[3].Value)
        ));
    }

    public static IEnumerable<Tuple<T1, T2, T3, T4>> Parse<T1, T2, T3, T4>(
        this IEnumerable<string> lines,
        Regex lineRegex,
        Func<string, T1> parser1,
        Func<string, T2> parser2,
        Func<string, T3> parser3,
        Func<string, T4> parser4
    )
    {
        var lineMatches = lines.Select(line => lineRegex.Match(line));

        return lineMatches.Select(lineMatch => Tuple.Create(
            parser1(lineMatch.Groups[1].Value),
            parser2(lineMatch.Groups[2].Value),
            parser3(lineMatch.Groups[3].Value),
            parser4(lineMatch.Groups[4].Value)
        ));
    }

    public static IEnumerable<Tuple<T1, T2, T3, T4, T5>> Parse<T1, T2, T3, T4, T5>(
        this IEnumerable<string> lines,
        Regex lineRegex,
        Func<string, T1> parser1,
        Func<string, T2> parser2,
        Func<string, T3> parser3,
        Func<string, T4> parser4,
        Func<string, T5> parser5
    )
    {
        var lineMatches = lines.Select(line => lineRegex.Match(line));

        return lineMatches.Select(lineMatch => Tuple.Create(
            parser1(lineMatch.Groups[1].Value),
            parser2(lineMatch.Groups[2].Value),
            parser3(lineMatch.Groups[3].Value),
            parser4(lineMatch.Groups[4].Value),
            parser5(lineMatch.Groups[5].Value)
        ));
    }
}