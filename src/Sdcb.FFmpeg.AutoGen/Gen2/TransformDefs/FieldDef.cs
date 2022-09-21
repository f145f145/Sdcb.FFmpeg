﻿using System.Collections.Generic;

namespace Sdcb.FFmpeg.AutoGen.Gen2.TransformDefs
{
    internal record FieldDef
    {
        public string Name { get; init; }

        public string NewName { get; init; }

        public TypeCastDef? TypeCastDef { get; init; }

        public bool? ReadOnly { get; init; }

        public bool Display { get; init; } = true;

        public bool IsRenamed => Name != NewName;

        public bool? Nullable { get; init; } = null;

        public FieldDef(string name, string newName, TypeCastDef? typeCast = null, bool? isReadonly = null, bool display = true, bool? nullable = null)
        {
            Name = name;
            NewName = newName;
            TypeCastDef = typeCast;
            ReadOnly = isReadonly;
            Display = display;
            Nullable = nullable;
        }

        public virtual IEnumerable<string> GetPropertyBody(string fieldName, TypeCastDef typeCastDef, PropStatus propStatus)
        {
            const string _ptr = "_ptr";
            string oldName = StringExtensions.CSharpKeywordTransform(fieldName);
            string newName = propStatus.Name;

            if (propStatus.IsReadonly)
            {
                yield return $"public {typeCastDef.GetReturnType(propStatus)} {newName} => {typeCastDef.GetPropertyGetter($"{_ptr}->{oldName}", propStatus)};";
            }
            else
            {
                yield return $"public {typeCastDef.GetReturnType(propStatus)} {newName}";
                yield return "{";
                yield return $"    get => {typeCastDef.GetPropertyGetter($"{_ptr}->{oldName}", propStatus)};";
                yield return $"    set => {_ptr}->{oldName} = {typeCastDef.GetPropertySetter(propStatus)};";
                yield return "}";
            }
        }

        public bool CalculateIsNullable()
        {
            if (Nullable.HasValue) return Nullable.Value;
            if (TypeCastDef == null) return false;
            return false;
        }

        public static FieldDef CreateTypeCast(string name, TypeCastDef typeCast) => new FieldDef(name, name, typeCast);

        public static FieldDef CreateHide(string name) => new FieldDef(name, name, display: false);

        public static FieldDef CreateRename(string name, string newName) => new FieldDef(name, newName);

        public static FieldDef CreateDefault(string name) => new FieldDef(name, name);

        public static FieldDef CreateNullable(string name) => new FieldDef(name, name, nullable: true);
    }
}
