﻿#region copyright
// ------------------------------------------------------
// Copyright (C) Dmitriy Yukhanov [https://codestage.net]
// ------------------------------------------------------
#endregion

using System;
using CodeStage.AntiCheat.Common;
using CodeStage.AntiCheat.Detectors;
using CodeStage.AntiCheat.Utils;
using UnityEngine;

namespace CodeStage.AntiCheat.ObscuredTypes
{
	/// <summary>
	/// Use it instead of regular <c>sbyte</c> for any cheating-sensitive properties, fields and other long-term declarations.
	/// </summary>
	/// <strong><em>Regular type is faster and memory wiser comparing to the obscured one!</em></strong><br/>
	/// Use regular type for all short-term operations and calculations while keeping obscured type only at the long-term declaration (i.e. class field).
	[Serializable]
	public partial struct ObscuredSByte : IObscuredType
	{
		private int hash;
		private sbyte hiddenValue;
		private sbyte currentCryptoKey;
		
		private sbyte fakeValue;

		private ObscuredSByte(sbyte value)
		{
			currentCryptoKey = GenerateKey();
			hiddenValue = Encrypt(value, currentCryptoKey);
			hash = HashUtils.CalculateHash(value);
			fakeValue = ObscuredCheatingDetector.IsRunningInHoneypotMode ? value : default;
		}

		/// <summary>
		/// Encrypts passed value using passed key.
		/// </summary>
		/// Key can be generated automatically using GenerateKey().
		/// \sa Decrypt(), GenerateKey()
		public static sbyte Encrypt(sbyte value, sbyte key)
		{
			unchecked
			{
				return (sbyte)((value ^ key) + key);
			}
		}

		/// <summary>
		/// Decrypts passed value you got from Encrypt() using same key.
		/// </summary>
		/// \sa Encrypt()
		public static sbyte Decrypt(sbyte value, sbyte key)
		{
			unchecked
			{
				return (sbyte)((value - key) ^ key);
			}
		}

		/// <summary>
		/// Creates and fills obscured variable with raw encrypted value previously got from GetEncrypted().
		/// </summary>
		/// Literally does same job as SetEncrypted() but makes new instance instead of filling existing one,
		/// making it easier to initialize new variables from saved encrypted values.
		///
		/// <param name="encrypted">Raw encrypted value you got from GetEncrypted().</param>
		/// <param name="key">Encryption key you've got from GetEncrypted().</param>
		/// <returns>New obscured variable initialized from specified encrypted value.</returns>
		/// \sa GetEncrypted(), SetEncrypted()
		public static ObscuredSByte FromEncrypted(sbyte encrypted, sbyte key)
		{
			var instance = new ObscuredSByte();
			instance.SetEncrypted(encrypted, key);
			return instance;
		}

		/// <summary>
		/// Generates random key. Used internally and can be used to generate key for manual Encrypt() calls.
		/// </summary>
		/// <returns>Key suitable for manual Encrypt() calls.</returns>
		public static sbyte GenerateKey()
		{
			return RandomUtils.GenerateSByteKey();
		}

		/// <summary>
		/// Allows to pick current obscured value as is.
		/// </summary>
		/// <param name="key">Encryption key needed to decrypt returned value.</param>
		/// <returns>Encrypted value as is.</returns>
		/// Use it in conjunction with SetEncrypted().<br/>
		/// Useful for saving data in obscured state.
		/// \sa FromEncrypted(), SetEncrypted()
		public sbyte GetEncrypted(out sbyte key)
		{
			if (IsDefault()) this = new ObscuredSByte(default);
			
			key = currentCryptoKey;
			return hiddenValue;
		}

		/// <summary>
		/// Allows to explicitly set current obscured value. Crypto key should be same as when encrypted value was got with GetEncrypted().
		/// </summary>
		/// Use it in conjunction with GetEncrypted().<br/>
		/// Useful for loading data stored in obscured state.
		/// \sa FromEncrypted()
		public void SetEncrypted(sbyte encrypted, sbyte key)
		{
			currentCryptoKey = key;
			var plain = Decrypt(encrypted, key);
			hiddenValue = encrypted;
			hash = HashUtils.CalculateHash(plain);

			if (ObscuredCheatingDetector.IsRunningInHoneypotMode)
				fakeValue = plain;
		}

		/// <summary>
		/// Alternative to the type cast, use if you wish to get decrypted value
		/// but can't or don't want to use cast to the regular type.
		/// </summary>
		/// <returns>Decrypted value.</returns>
		public sbyte GetDecrypted()
		{
			return InternalDecrypt();
		}

		public void RandomizeCryptoKey()
		{
			hiddenValue = InternalDecrypt();
			currentCryptoKey = GenerateKey();
			HideValue(hiddenValue);
		}
		
		private static bool ValidateHash(sbyte input, int hash)
		{
#if DEBUG
			if (hash == default && HashUtils.CalculateHash(input) != default)
				Debug.LogError(ACTk.LogPrefix + $"{nameof(hash)} is not initialized properly!\n" +
							   "It will produce false positive cheating detection.\n" +
							   "Can happen when migrating from older ACTk versions.\n" +
							   "Please call Tools > Code Stage > Anti-Cheat Toolkit > Migrate > * menu item to try fixing this.");
#endif
			return HashUtils.ValidateHash(input, hash);
		}
		
		private void HideValue(sbyte plain)
		{
			hiddenValue = Encrypt(plain, currentCryptoKey);
			hash = HashUtils.CalculateHash(plain);
		}

		private sbyte InternalDecrypt()
		{
			if (IsDefault()) this = new ObscuredSByte(default);

			var plain = Decrypt(hiddenValue, currentCryptoKey);
			var hashValid = ValidateHash(plain, hash);

			if (hashValid && fakeValue == default)
			{
				// init honeypot if it wasn't initialized yet
				if (plain != default && ObscuredCheatingDetector.IsRunningInHoneypotMode)
					fakeValue = plain;
			}
			
			var honeypotValid = plain == fakeValue;
			ObscuredCheatingDetector.TryDetectCheating(this, hashValid, hash, honeypotValid, plain, fakeValue); 
			
			return plain;
		}
		
		public bool IsDefault()
		{
			return hiddenValue == default &&
				   currentCryptoKey == default &&
				   hash == default;
		}

		//! @cond

		#region obsolete

		[Obsolete("This API is redundant and does not perform any actions. It will be removed in future updates.", true)]
		public static void SetNewCryptoKey(sbyte newKey) {}

		[Obsolete("This API is redundant and does not perform any actions. It will be removed in future updates.", true)]
		public void ApplyNewCryptoKey() {}

		[Obsolete("Please use new Encrypt(value, key) or Decrypt(value, key) APIs instead. " +
		          "This API will be removed in future updates.", true)]
		public static sbyte EncryptDecrypt(sbyte value, sbyte key)
		{
			return (sbyte)(value ^ key);
		}
		
		/// <summary>
		/// Decrypts data encrypted in ACTk 2024.0 or earlier.
		/// </summary>
		public static sbyte DecryptFromV0(sbyte value, sbyte key)
		{
			return (sbyte)(value ^ key);
		}

		#endregion

		//! @endcond
	}
}
