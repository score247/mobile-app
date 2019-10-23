﻿// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage(
    "Major Code Smell",
    "S109:Magic numbers should not be used",
    Justification = "MesagePack is used",
    Scope = "namespace",
    Target = "~P:LiveScore.Core")]
