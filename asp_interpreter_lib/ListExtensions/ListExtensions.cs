﻿using System.Text;

namespace asp_interpreter_lib.ListExtensions;

public static class ListExtensions
{
    /// <summary>
    /// Provides a readable string representation for lists.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <returns></returns>
    public static string ListToString<T>(this List<T> list)
    {
        if (list.Count == 0) return string.Empty;

        var sb = new StringBuilder();

        for (int i = 0; i < list.Count - 1; i++)
        {
            sb.Append($"{list[i].ToString()}, ");
        }

        sb.Append($"{list[list.Count - 1].ToString()}");

        return sb.ToString();
    }
}
