namespace MooVC.Utilities.FileSystem.Reconciliation;

using System;
using static System.IO.Path;

internal sealed record Resource(Guid Id, string Location, string Name, string Type)
{
    public string Path => Combine(Location, $"{Name}.{Type}");
}