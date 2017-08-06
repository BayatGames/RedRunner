using System;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Linq;

namespace BayatGames.SaveGameFree.Encoders
{

	/// <summary>
	/// Save Game Simple Encoder.
	/// Grabbed from https://stackoverflow.com/questions/10168240/encrypting-decrypting-a-string-in-c-sharp
	/// </summary>
	public class SaveGameSimpleEncoder : ISaveGameEncoder
	{
	
		private const int Keysize = 256;
		private const int DerivationIterations = 1000;

		/// <summary>
		/// Encode the specified input with password.
		/// </summary>
		/// <param name="input">Input.</param>
		/// <param name="password">Password.</param>
		public string Encode ( string input, string password )
		{
			#if !UNITY_WSA || !UNITY_WINRT
			var saltStringBytes = Generate256BitsOfRandomEntropy ();
			var ivStringBytes = Generate256BitsOfRandomEntropy ();
			var plainTextBytes = Encoding.UTF8.GetBytes ( input );
			var passPhrase = new Rfc2898DeriveBytes ( password, saltStringBytes, DerivationIterations );
			var keyBytes = passPhrase.GetBytes ( Keysize / 8 );
			using ( var symmetricKey = new RijndaelManaged () )
			{
				symmetricKey.BlockSize = 256;
				symmetricKey.Mode = CipherMode.CBC;
				symmetricKey.Padding = PaddingMode.PKCS7;
				using ( var encryptor = symmetricKey.CreateEncryptor ( keyBytes, ivStringBytes ) )
				{
					using ( var memoryStream = new MemoryStream () )
					{
						using ( var cryptoStream = new CryptoStream ( memoryStream, encryptor, CryptoStreamMode.Write ) )
						{
							cryptoStream.Write ( plainTextBytes, 0, plainTextBytes.Length );
							cryptoStream.FlushFinalBlock ();
							var cipherTextBytes = saltStringBytes;
							cipherTextBytes = cipherTextBytes.Concat ( ivStringBytes ).ToArray ();
							cipherTextBytes = cipherTextBytes.Concat ( memoryStream.ToArray () ).ToArray ();
							memoryStream.Close ();
							cryptoStream.Close ();
							return Convert.ToBase64String ( cipherTextBytes );
						}
					}
				}
			}
			#else
			return Convert.ToBase64String ( Encoding.UTF8.GetBytes ( input ) );
			#endif
		}

		/// <summary>
		/// Decode the specified input with password.
		/// </summary>
		/// <param name="input">Input.</param>
		/// <param name="password">Password.</param>
		public string Decode ( string input, string password )
		{
			#if !UNITY_WSA || !UNITY_WINRT
			var cipherTextBytesWithSaltAndIv = Convert.FromBase64String ( input );
			var saltStringBytes = cipherTextBytesWithSaltAndIv.Take ( Keysize / 8 ).ToArray ();
			var ivStringBytes = cipherTextBytesWithSaltAndIv.Skip ( Keysize / 8 ).Take ( Keysize / 8 ).ToArray ();
			var cipherTextBytes = cipherTextBytesWithSaltAndIv.Skip ( ( Keysize / 8 ) * 2 ).Take ( cipherTextBytesWithSaltAndIv.Length - ( ( Keysize / 8 ) * 2 ) ).ToArray ();
			var passPhrase = new Rfc2898DeriveBytes ( password, saltStringBytes, DerivationIterations );
			var keyBytes = passPhrase.GetBytes ( Keysize / 8 );
			using ( var symmetricKey = new RijndaelManaged () )
			{
				symmetricKey.BlockSize = 256;
				symmetricKey.Mode = CipherMode.CBC;
				symmetricKey.Padding = PaddingMode.PKCS7;
				using ( var decryptor = symmetricKey.CreateDecryptor ( keyBytes, ivStringBytes ) )
				{
					using ( var memoryStream = new MemoryStream ( cipherTextBytes ) )
					{
						using ( var cryptoStream = new CryptoStream ( memoryStream, decryptor, CryptoStreamMode.Read ) )
						{
							var plainTextBytes = new byte[cipherTextBytes.Length];
							var decryptedByteCount = cryptoStream.Read ( plainTextBytes, 0, plainTextBytes.Length );
							memoryStream.Close ();
							cryptoStream.Close ();
							return Encoding.UTF8.GetString ( plainTextBytes, 0, decryptedByteCount );
						}
					}
				}
			}
			#else
			return Encoding.UTF8.GetString ( Convert.FromBase64String ( input ) );
			#endif
		}

		#if !UNITY_WSA || !UNITY_WINRT
		private static byte[] Generate256BitsOfRandomEntropy ()
		{
			var randomBytes = new byte[32];
			var rngCsp = new RNGCryptoServiceProvider ();
			rngCsp.GetBytes ( randomBytes );
			return randomBytes;
		}
		#endif

	}

}