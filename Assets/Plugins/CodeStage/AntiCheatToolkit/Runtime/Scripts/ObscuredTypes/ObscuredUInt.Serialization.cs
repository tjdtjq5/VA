﻿#region copyright
// -------------------------------------------------------
// Copyright (C) Dmitriy Yukhanov [https://codestage.net]
// -------------------------------------------------------
#endregion

using CodeStage.AntiCheat.Detectors;
using UnityEngine;

namespace CodeStage.AntiCheat.ObscuredTypes
{
	public partial struct ObscuredUInt : ISerializationCallbackReceiver
	{
		public void OnBeforeSerialize() { /* not used */ }
		public void OnAfterDeserialize()
		{
			if (IsDefault())
				fakeValue = default;
			else if (ObscuredCheatingDetector.IsRunningInHoneypotMode)
				fakeValue = Decrypt(hiddenValue, currentCryptoKey);
		}
	}
}