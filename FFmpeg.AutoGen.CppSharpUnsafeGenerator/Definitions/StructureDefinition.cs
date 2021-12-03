﻿namespace FFmpeg.AutoGen.CppSharpUnsafeGenerator.Definitions
{
    internal record StructureDefinition : NamedDefinition, IDefinition
    {
        public StructureField[] Fields { get; set; } = System.Array.Empty<StructureField>();
        public bool IsComplete { get; set; }
        public bool IsUnion { get; init; }
    }
}