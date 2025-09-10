﻿#region copyright
// ------------------------------------------------------
// Copyright (C) Dmitriy Yukhanov [https://codestage.net]
// ------------------------------------------------------
#endregion

#if UNITY_EDITOR

using System.Runtime.InteropServices;
using CodeStage.AntiCheat.Utils;
using UnityEngine;

namespace CodeStage.AntiCheat.ObscuredTypes
{
	[StructLayout(LayoutKind.Auto)]
    public sealed partial class ObscuredString : ISerializableObscuredType
    {
		internal const int Version = 1;
		
		// ReSharper disable once NotAccessedField.Global - used explicitly
		[SerializeField] internal byte version;

		bool ISerializableObscuredType.IsDataValid => IsDefault() || hash == HashUtils.CalculateHash(InternalEncryptDecrypt(hiddenChars, cryptoKey));
    }
}
#endif