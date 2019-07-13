﻿using System;

/*
 This file is part of pspsharp.

 pspsharp is free software: you can redistribute it and/or modify
 it under the terms of the GNU General Public License as published by
 the Free Software Foundation, either version 3 of the License, or
 (at your option) any later version.

 pspsharp is distributed in the hope that it will be useful,
 but WITHOUT ANY WARRANTY; without even the implied warranty of
 MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 GNU General Public License for more details.

 You should have received a copy of the GNU General Public License
 along with pspsharp.  If not, see <http://www.gnu.org/licenses/>.
 */
namespace pspsharp.crypto
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static pspsharp.HLE.Modules.memlmdModule;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static pspsharp.HLE.Modules.semaphoreModule;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static pspsharp.crypto.KIRK.PSP_KIRK_CMD_MODE_CMD1;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static pspsharp.crypto.KIRK.PSP_KIRK_CMD_MODE_DECRYPT_CBC;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static pspsharp.util.Utilities.read8;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static pspsharp.util.Utilities.readUnaligned16;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static pspsharp.util.Utilities.readUnaligned32;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static pspsharp.util.Utilities.writeUnaligned32;


	//using Logger = org.apache.log4j.Logger;

	using Utilities = pspsharp.util.Utilities;

	public class PRX
	{
		private bool InstanceFieldsInitialized = false;

		private void InitializeInstanceFields()
		{
			g_tagInfo = new TAG_INFO[]
			{
				new TAG_INFO(this, 0x4C949CF0, KeyVault.keys210_vita_k0, 0x43),
				new TAG_INFO(this, 0x4C9494F0, KeyVault.keys660_k1, 0x43),
				new TAG_INFO(this, 0x4C9495F0, KeyVault.keys660_k2, 0x43),
				new TAG_INFO(this, 0x4C9490F0, KeyVault.keys660_k3, 0x43),
				new TAG_INFO(this, 0x4C9491F0, KeyVault.keys660_k8, 0x43),
				new TAG_INFO(this, 0x4C9493F0, KeyVault.keys660_k4, 0x43),
				new TAG_INFO(this, 0x4C9497F0, KeyVault.keys660_k5, 0x43),
				new TAG_INFO(this, 0x4C9492F0, KeyVault.keys660_k6, 0x43),
				new TAG_INFO(this, 0x4C9496F0, KeyVault.keys660_k7, 0x43),
				new TAG_INFO(this, 0x457B90F0, KeyVault.keys660_v1, 0x5B),
				new TAG_INFO(this, 0x457B91F0, KeyVault.keys660_v7, 0x5B),
				new TAG_INFO(this, 0x457B92F0, KeyVault.keys660_v6, 0x5B),
				new TAG_INFO(this, 0x457B93F0, KeyVault.keys660_v3, 0x5B),
				new TAG_INFO(this, 0x380290F0, KeyVault.keys660_v2, 0x5A),
				new TAG_INFO(this, 0x380291F0, KeyVault.keys660_v8, 0x5A),
				new TAG_INFO(this, 0x380292F0, KeyVault.keys660_v4, 0x5A),
				new TAG_INFO(this, 0x380293F0, KeyVault.keys660_v5, 0x5A),
				new TAG_INFO(this, 0x4C948CF0, KeyVault.keys639_k3, 0x43),
				new TAG_INFO(this, 0x4C948DF0, KeyVault.keys638_k4, 0x43),
				new TAG_INFO(this, 0x4C948BF0, KeyVault.keys636_k2, 0x43),
				new TAG_INFO(this, 0x4C948AF0, KeyVault.keys636_k1, 0x43),
				new TAG_INFO(this, 0x457B8AF0, KeyVault.keys636_1, 0x5B),
				new TAG_INFO(this, 0x4C9487F0, KeyVault.keys630_k8, 0x43),
				new TAG_INFO(this, 0x457B83F0, KeyVault.keys630_k7, 0x5B),
				new TAG_INFO(this, 0x4C9486F0, KeyVault.keys630_k6, 0x43),
				new TAG_INFO(this, 0x457B82F0, KeyVault.keys630_k5, 0x5B),
				new TAG_INFO(this, 0x457B81F0, KeyVault.keys630_k4, 0x5B),
				new TAG_INFO(this, 0x4C9485F0, KeyVault.keys630_k3, 0x43),
				new TAG_INFO(this, 0x457B80F0, KeyVault.keys630_k2, 0x5B),
				new TAG_INFO(this, 0x4C9484F0, KeyVault.keys630_k1, 0x43),
				new TAG_INFO(this, 0x457B28F0, KeyVault.keys620_e, 0x5B),
				new TAG_INFO(this, 0x457B0CF0, KeyVault.keys620_a, 0x5B),
				new TAG_INFO(this, 0x380228F0, KeyVault.keys620_5v, 0x5A),
				new TAG_INFO(this, 0x4C942AF0, KeyVault.keys620_5k, 0x43),
				new TAG_INFO(this, 0x4C9428F0, KeyVault.keys620_5, 0x43),
				new TAG_INFO(this, 0x4C941DF0, KeyVault.keys620_1, 0x43),
				new TAG_INFO(this, 0x4C941CF0, KeyVault.keys620_0, 0x43),
				new TAG_INFO(this, 0x4C9422F0, KeyVault.keys600_2, 0x43),
				new TAG_INFO(this, 0x4C941EF0, KeyVault.keys600_1, 0x43),
				new TAG_INFO(this, 0x4C9429F0, KeyVault.keys570_5k, 0x43),
				new TAG_INFO(this, 0x457B0BF0, KeyVault.keys505_a, 0x5B),
				new TAG_INFO(this, 0x4C9419F0, KeyVault.keys505_1, 0x43),
				new TAG_INFO(this, 0x4C9418F0, KeyVault.keys505_0, 0x43),
				new TAG_INFO(this, 0x457B1EF0, KeyVault.keys500_c, 0x5B),
				new TAG_INFO(this, 0x4C941FF0, KeyVault.keys500_2, 0x43),
				new TAG_INFO(this, 0x4C9417F0, KeyVault.keys500_1, 0x43),
				new TAG_INFO(this, 0x4C9416F0, KeyVault.keys500_0, 0x43),
				new TAG_INFO(this, 0x4C9414F0, KeyVault.keys390_0, 0x43),
				new TAG_INFO(this, 0x4C9415F0, KeyVault.keys390_1, 0x43),
				new TAG_INFO(this, 0x4C9412F0, KeyVault.keys370_0, 0x43),
				new TAG_INFO(this, 0x4C9413F0, KeyVault.keys370_1, 0x43),
				new TAG_INFO(this, 0x457B10F0, KeyVault.keys370_2, 0x5B),
				new TAG_INFO(this, 0x4C940DF0, KeyVault.keys360_0, 0x43),
				new TAG_INFO(this, 0x4C9410F0, KeyVault.keys360_1, 0x43),
				new TAG_INFO(this, 0x4C940BF0, KeyVault.keys330_0, 0x43),
				new TAG_INFO(this, 0x457B0AF0, KeyVault.keys330_1, 0x5B),
				new TAG_INFO(this, 0x38020AF0, KeyVault.keys330_2, 0x5A),
				new TAG_INFO(this, 0x4C940AF0, KeyVault.keys330_3, 0x43),
				new TAG_INFO(this, 0x4C940CF0, KeyVault.keys330_4, 0x43),
				new TAG_INFO(this, 0xcfef09f0, KeyVault.keys310_0, 0x62),
				new TAG_INFO(this, 0x457b08f0, KeyVault.keys310_1, 0x5B),
				new TAG_INFO(this, 0x380208F0, KeyVault.keys310_2, 0x5A),
				new TAG_INFO(this, 0xcfef08f0, KeyVault.keys310_3, 0x62),
				new TAG_INFO(this, 0xCFEF07F0, KeyVault.keys303_0, 0x62),
				new TAG_INFO(this, 0xCFEF06F0, KeyVault.keys300_0, 0x62),
				new TAG_INFO(this, 0x457B06F0, KeyVault.keys300_1, 0x5B),
				new TAG_INFO(this, 0x380206F0, KeyVault.keys300_2, 0x5A),
				new TAG_INFO(this, 0xCFEF05F0, KeyVault.keys280_0, 0x62),
				new TAG_INFO(this, 0x457B05F0, KeyVault.keys280_1, 0x5B),
				new TAG_INFO(this, 0x380205F0, KeyVault.keys280_2, 0x5A),
				new TAG_INFO(this, 0x16D59E03, KeyVault.keys260_0, 0x62),
				new TAG_INFO(this, 0x76202403, KeyVault.keys260_1, 0x5B),
				new TAG_INFO(this, 0x0F037303, KeyVault.keys260_2, 0x5A),
				new TAG_INFO(this, 0x4C940FF0, KeyVault.key_2DA8, 0x43),
				new TAG_INFO(this, 0x4467415D, KeyVault.key_22E0, 0x59),
				new TAG_INFO(this, 0x00000000, KeyVault.key_21C0, 0x42),
				new TAG_INFO(this, 0x01000000, KeyVault.key_2250, 0x43),
				new TAG_INFO(this, 0x2E5E10F0, KeyVault.key_2E5E10F0, 0x48),
				new TAG_INFO(this, 0x2E5E12F0, KeyVault.key_2E5E12F0, 0x48),
				new TAG_INFO(this, 0x2E5E13F0, KeyVault.key_2E5E13F0, 0x48),
				new TAG_INFO(this, 0x2FD30BF0, KeyVault.key_2FD30BF0, 0x47),
				new TAG_INFO(this, 0x2FD311F0, KeyVault.key_2FD311F0, 0x47),
				new TAG_INFO(this, 0x2FD312F0, KeyVault.key_2FD312F0, 0x47),
				new TAG_INFO(this, 0x2FD313F0, KeyVault.key_2FD313F0, 0x47),
				new TAG_INFO(this, 0xD91605F0, KeyVault.key_D91605F0, 0x5D),
				new TAG_INFO(this, 0xD91606F0, KeyVault.key_D91606F0, 0x5D),
				new TAG_INFO(this, 0xD91608F0, KeyVault.key_D91608F0, 0x5D),
				new TAG_INFO(this, 0xD91609F0, KeyVault.key_D91609F0, 0x5D),
				new TAG_INFO(this, 0xD9160AF0, KeyVault.key_D9160AF0, 0x5D),
				new TAG_INFO(this, 0xD9160BF0, KeyVault.key_D9160BF0, 0x5D),
				new TAG_INFO(this, 0xD91611F0, KeyVault.key_D91611F0, 0x5D),
				new TAG_INFO(this, 0xD91612F0, KeyVault.key_D91612F0, 0x5D),
				new TAG_INFO(this, 0xD91613F0, KeyVault.key_D91613F0, 0x5D),
				new TAG_INFO(this, 0xD91614F0, KeyVault.key_D91614F0, 0x5D),
				new TAG_INFO(this, 0xD91615F0, KeyVault.key_D91615F0, 0x5D),
				new TAG_INFO(this, 0xD91616F0, KeyVault.key_D91616F0, 0x5D),
				new TAG_INFO(this, 0xD91617F0, KeyVault.key_D91617F0, 0x5D),
				new TAG_INFO(this, 0xD91618F0, KeyVault.key_D91618F0, 0x5D),
				new TAG_INFO(this, 0xD91619F0, KeyVault.key_D91619F0, 0x5D),
				new TAG_INFO(this, 0xD9161AF0, KeyVault.key_D9161AF0, 0x5D),
				new TAG_INFO(this, 0xD91620F0, KeyVault.key_D91620F0, 0x5D),
				new TAG_INFO(this, 0xD91621F0, KeyVault.key_D91621F0, 0x5D),
				new TAG_INFO(this, 0xD91622F0, KeyVault.key_D91622F0, 0x5D),
				new TAG_INFO(this, 0xD91623F0, KeyVault.key_D91623F0, 0x5D),
				new TAG_INFO(this, 0xD91624F0, KeyVault.key_D91624F0, 0x5D),
				new TAG_INFO(this, 0xD91628F0, KeyVault.key_D91628F0, 0x5D),
				new TAG_INFO(this, 0xD91680F0, KeyVault.key_D91680F0, 0x5D),
				new TAG_INFO(this, 0xD91681F0, KeyVault.key_D91681F0, 0x5D),
				new TAG_INFO(this, 0xD82310F0, KeyVault.keys02G_E, 0x51),
				new TAG_INFO(this, 0xD8231EF0, KeyVault.keys03G_E, 0x51),
				new TAG_INFO(this, 0xD82328F0, KeyVault.keys05G_E, 0x51),
				new TAG_INFO(this, 0x279D08F0, KeyVault.oneseg_310, 0x61),
				new TAG_INFO(this, 0x279D06F0, KeyVault.oneseg_300, 0x61),
				new TAG_INFO(this, 0x279D05F0, KeyVault.oneseg_280, 0x61),
				new TAG_INFO(this, 0xD66DF703, KeyVault.oneseg_260_271, 0x61),
				new TAG_INFO(this, 0x279D10F0, KeyVault.oneseg_slim, 0x61),
				new TAG_INFO(this, 0x3C2A08F0, KeyVault.ms_app_main, 0x67),
				new TAG_INFO(this, 0xADF305F0, KeyVault.demokeys_280, 0x60),
				new TAG_INFO(this, 0xADF306F0, KeyVault.demokeys_3XX_1, 0x60),
				new TAG_INFO(this, 0xADF308F0, KeyVault.demokeys_3XX_2, 0x60),
				new TAG_INFO(this, 0x8004FD03, KeyVault.ebootbin_271_new, 0x5D),
				new TAG_INFO(this, 0xD91605F0, KeyVault.ebootbin_280_new, 0x5D),
				new TAG_INFO(this, 0xD91606F0, KeyVault.ebootbin_300_new, 0x5D),
				new TAG_INFO(this, 0xD91608F0, KeyVault.ebootbin_310_new, 0x5D),
				new TAG_INFO(this, 0x0A35EA03, KeyVault.gameshare_260_271, 0x5E),
				new TAG_INFO(this, 0x7B0505F0, KeyVault.gameshare_280, 0x5E),
				new TAG_INFO(this, 0x7B0506F0, KeyVault.gameshare_300, 0x5E),
				new TAG_INFO(this, 0x7B0508F0, KeyVault.gameshare_310, 0x5E),
				new TAG_INFO(this, 0x380210F0, KeyVault.key_380210F0, 0x5A),
				new TAG_INFO(this, 0x380280F0, KeyVault.key_380280F0, 0x5A),
				new TAG_INFO(this, 0x380283F0, KeyVault.key_380283F0, 0x5A),
				new TAG_INFO(this, 0x407810F0, KeyVault.key_407810F0, 0x6A),
				new TAG_INFO(this, 0xE92410F0, KeyVault.drmkeys_6XX_1, 0x40),
				new TAG_INFO(this, 0x692810F0, KeyVault.drmkeys_6XX_2, 0x40),
				new TAG_INFO(this, 0x00000000, KeyVault.g_key00, 0x42, 0x00),
				new TAG_INFO(this, 0x02000000, KeyVault.g_key02, 0x45, 0x00),
				new TAG_INFO(this, 0x03000000, KeyVault.g_key03, 0x46, 0x00),
				new TAG_INFO(this, 0x03000000, KeyVault.g_key04, 0x47, 0x00),
				new TAG_INFO(this, 0x03000000, KeyVault.g_key05, 0x48, 0x00),
				new TAG_INFO(this, 0x03000000, KeyVault.g_key06, 0x49, 0x00),
				new TAG_INFO(this, 0x03000000, KeyVault.g_key0A, 0x4D, 0x00),
				new TAG_INFO(this, 0x03000000, KeyVault.g_key0D, 0x50, 0x00),
				new TAG_INFO(this, 0x03000000, KeyVault.g_key0E, 0x51, 0x00),
				new TAG_INFO(this, 0x4467415d, KeyVault.g_key44, 0x59, 0x59),
				new TAG_INFO(this, 0x207bbf2f, KeyVault.g_key20, 0x5A, 0x5A),
				new TAG_INFO(this, 0x3ace4dce, KeyVault.g_key3A, 0x5B, 0x5B),
				new TAG_INFO(this, 0x07000000, KeyVault.g_key_INDEXDAT1xx, 0x4A, 0x00),
				new TAG_INFO(this, 0x08000000, KeyVault.g_keyEBOOT1xx, 0x4B, 0x00),
				new TAG_INFO(this, unchecked((int)0xC0CB167C), KeyVault.g_keyEBOOT2xx, 0x5D, 0x5D),
				new TAG_INFO(this, 0x0B000000, KeyVault.g_keyUPDATER, 0x4E, 0x00),
				new TAG_INFO(this, 0x0C000000, KeyVault.g_keyDEMOS27X, 0x4F, 0x00),
				new TAG_INFO(this, 0x0F000000, KeyVault.g_keyMEIMG250, 0x52, 0x00),
				new TAG_INFO(this, unchecked((int)0x862648D1), KeyVault.g_keyMEIMG260, 0x52, 0x52),
				new TAG_INFO(this, 0x207BBF2F, KeyVault.g_keyUNK1, 0x5A, 0x5A),
				new TAG_INFO(this, 0x09000000, KeyVault.g_key_GAMESHARE1xx, 0x4C, 0x00),
				new TAG_INFO(this, unchecked((int)0xBB67C59F), KeyVault.g_keyB8, 0x5C, 0x5C),
				new TAG_INFO(this, unchecked((int)0xBB67C59F), KeyVault.g_key_GAMESHARE2xx, 0x5E, 0x5E),
				new TAG_INFO(this, unchecked((int)0xBB67C59F), KeyVault.g_key4C, 0x5F, 0x5F),
				new TAG_INFO(this, unchecked((int)0xBB67C59F), KeyVault.g_key7F, 0x60, 0x60),
				new TAG_INFO(this, unchecked((int)0xBB67C59F), KeyVault.g_key1B, 0x61, 0x61)
			};
		}

		//public static Logger log = CryptoEngine.log;
		// enum SceExecFileAttr
		public const int SCE_EXEC_FILE_COMPRESSED = 0x001;
		public const int SCE_EXEC_FILE_ELF = 0x002;
		public const int SCE_EXEC_FILE_GZIP_OVERLAP = 0x008;
		public const int SCE_EXEC_FILE_KL4E_COMPRESSED = 0x200;
		// enum SceExecFileDecryptMode
		public const int DECRYPT_MODE_NO_EXEC = 0;
		public const int DECRYPT_MODE_BOGUS_MODULE = 1;
		public const int DECRYPT_MODE_KERNEL_MODULE = 2;
		public const int DECRYPT_MODE_VSH_MODULE = 3;
		public const int DECRYPT_MODE_USER_MODULE = 4;
		public const int DECRYPT_MODE_UMD_GAME_EXEC = 9;
		public const int DECRYPT_MODE_GAMESHARING_EXEC = 10;
		public const int DECRYPT_MODE_MS_UPDATER = 12;
		public const int DECRYPT_MODE_DEMO_EXEC = 13;
		public const int DECRYPT_MODE_APP_MODULE = 14;
		public const int DECRYPT_MODE_POPS_EXEC = 20;

		public PRX()
		{
			if (!InstanceFieldsInitialized)
			{
				InitializeInstanceFields();
				InstanceFieldsInitialized = true;
			}
		}

		// PRXDecrypter TAG struct.
		private class TAG_INFO
		{
			private readonly PRX outerInstance;

			internal int tag; // 4 byte value at offset 0xD0 in the PRX file
			internal int[] key; // 144 bytes keys
			internal int code; // code for scramble
			internal int codeExtra; // code extra for scramble (old tags)

			public TAG_INFO(PRX outerInstance, int tag, int[] key, int code, int codeExtra)
			{
				this.outerInstance = outerInstance;
				this.tag = tag;
				this.key = intArrayToTagArray(key);
				this.code = code;
				this.codeExtra = codeExtra;
			}

			public TAG_INFO(PRX outerInstance, int tag, int[] key, int code)
			{
				this.outerInstance = outerInstance;
				this.tag = tag;
				this.key = key;
				this.code = code;
				this.codeExtra = 0;
			}

			internal virtual int[] intArrayToTagArray(int[] array)
			{
				int[] tagArray = new int[144];
				for (int i = 0; i < array.Length; i++)
				{
					tagArray[i * 4 + 3] = ((array[i] >> 24) & 0xFF);
					tagArray[i * 4 + 2] = ((array[i] >> 16) & 0xFF);
					tagArray[i * 4 + 1] = ((array[i] >> 8) & 0xFF);
					tagArray[i * 4 + 0] = (array[i] & 0xFF);
				}
				return tagArray;
			}
		}

		private TAG_INFO[] g_tagInfo;

		private TAG_INFO GetTagInfo(int tag)
		{
			int iTag;
			for (iTag = 0; iTag < g_tagInfo.Length; iTag++)
			{
				if (g_tagInfo[iTag].tag == tag)
				{
					return g_tagInfo[iTag];
				}
			}
			return null;
		}

		private int ScramblePRX(sbyte[] buf, int offset, int size, int code)
		{
			// Set CBC mode.
			writeUnaligned32(buf, offset + 0, PSP_KIRK_CMD_MODE_DECRYPT_CBC);

			// Set unknown parameters to 0.
			writeUnaligned32(buf, offset + 4, 0);
			writeUnaligned32(buf, offset + 8, 0);

			// Set the the key seed to code.
			writeUnaligned32(buf, offset + 12, code & 0xFF);

			// Set the the data size to size.
			writeUnaligned32(buf, offset + 16, size);

			int result = semaphoreModule.hleUtilsBufferCopyWithRange(buf, offset, size, buf, offset, size + 0x14, KIRK.PSP_KIRK_CMD_DECRYPT);
			if (result != 0)
			{
				System.Console.WriteLine(string.Format("ScramblePRX returning 0x{0:X}", result));
			}

			return result;
		}

		private static bool isNullKey(sbyte[] key)
		{
			if (key != null)
			{
				for (int i = 0; i < key.Length; i++)
				{
					if (key[i] != (sbyte) 0)
					{
						return false;
					}
				}
			}
			return true;
		}

		private void MixXOR(sbyte[] outbuf, int size, sbyte[] inbuf, sbyte[] xor)
		{
			for (int i = 0; i < size; i++)
			{
				outbuf[i] = (sbyte)((inbuf[i] & 0xFF) ^ (xor[i] & 0xFF));
			}
		}

		private void RoundXOR(sbyte[] buf, int size, sbyte[] xor1, sbyte[] xor2)
		{
			for (int i = 0; i < size; i++)
			{
				if (!isNullKey(xor1))
				{
					buf[i] ^= xor1[i & 0xf];
				}

				if (!isNullKey(xor2))
				{
					buf[i] ^= xor2[i & 0xf];
				}
			}
		}

		public virtual sbyte[] DecryptAndUncompressPRX(sbyte[] buf, int size)
		{
			int compAttribute = readUnaligned16(buf, 0x6);
			int pspSize = readUnaligned32(buf, 0x2C);
			int elfSize = readUnaligned32(buf, 0x28);
			int decryptMode = read8(buf, 0x7C);

			sbyte[] resultBuffer = new sbyte[size];
			Array.Copy(buf, 0, resultBuffer, 0, size);

			int type;
			switch (decryptMode)
			{
				case DECRYPT_MODE_VSH_MODULE:
				case DECRYPT_MODE_USER_MODULE:
					int result = memlmdModule.hleMemlmd_6192F715(resultBuffer, size);
					//if (log.DebugEnabled)
					{
						System.Console.WriteLine(string.Format("DecryptPRX: memlmd_6192F715 returning 0x{0:X}: {1}", result, Utilities.getMemoryDump(resultBuffer, 0, 0x80 + 0xD0)));
					}
					type = 9;
					break;
				case DECRYPT_MODE_UMD_GAME_EXEC:
					type = 9;
					break;
				case DECRYPT_MODE_GAMESHARING_EXEC:
					type = 2;
					break;
				case DECRYPT_MODE_MS_UPDATER:
					type = 8;
					break;
				case DECRYPT_MODE_DEMO_EXEC:
				case DECRYPT_MODE_APP_MODULE:
					type = 4;
					break;
				case DECRYPT_MODE_POPS_EXEC:
					type = 5;
					break;
				default:
					System.Console.WriteLine(string.Format("DecryptAndUncompressPRX unknown decryptMode={0:D}", decryptMode));
					type = 2;
					break;
			}

			int resultSize = DecryptPRX(resultBuffer, size, type, null, null);
			if (resultSize < 0)
			{
				return null;
			}

			if ((compAttribute & SCE_EXEC_FILE_COMPRESSED) != 0)
			{
				if ((compAttribute & 0xF00) == 0)
				{
					// GZIP compressed
					try
					{
						GZIPInputStream @in = new GZIPInputStream(new System.IO.MemoryStream(resultBuffer, 0, pspSize));
						sbyte[] elfBuffer = new sbyte[elfSize];
						int elfOffset = 0;
						while (elfOffset < elfSize)
						{
							int Length = @in.read(elfBuffer, elfOffset, elfSize - elfOffset);
							if (Length <= 0)
							{
								break;
							}
							elfOffset += Length;
						}
						@in.close();

						// Return the uncompressed ELF file
						resultSize = elfOffset;
						resultBuffer = elfBuffer;
					}
					catch (IOException e)
					{
						System.Console.WriteLine(e);
					}
				}
				else
				{
					System.Console.WriteLine(string.Format("KL4E decompression unimplemented, compAttribute=0x{0:X}", compAttribute));
				}
			}

			// Truncate the resultBuffer if too long
			if (resultBuffer.Length > resultSize)
			{
				sbyte[] newBuffer = new sbyte[resultSize];
				Array.Copy(resultBuffer, 0, newBuffer, 0, resultSize);
				resultBuffer = newBuffer;
			}

			return resultBuffer;
		}

		public virtual int DecryptPRX(sbyte[] buf, int size, int type, sbyte[] xor1, sbyte[] xor2)
		{
			int result = 0;

			// Fetch the PRX tag.
			int tag = readUnaligned32(buf, 0xD0);

			// Get the tag info.
			TAG_INFO pti = GetTagInfo(tag);
			if (pti == null)
			{
				return -1;
			}

			// Fetch the sealed override ELF size.
			int retsize = readUnaligned32(buf, 0xB0);

			// Old encryption method (144 bytes key).
			if (pti.key.Length > 0x10)
			{
				// Setup the buffers.
				sbyte[] oldbuf = new sbyte[size];
				sbyte[] oldbuf1 = new sbyte[0x150];

				Array.Copy(buf, 0, oldbuf, 0, size);

				for (int i = 0; i < 0x150; i++)
				{
					oldbuf[i] = 0;
				}
				for (int i = 0; i < 0x40; i++)
				{
					oldbuf[i] = 0x55;
				}

				writeUnaligned32(oldbuf, 0x2C + 0, PSP_KIRK_CMD_MODE_DECRYPT_CBC);
				writeUnaligned32(oldbuf, 0x2C + 4, 0);
				writeUnaligned32(oldbuf, 0x2C + 8, 0);
				writeUnaligned32(oldbuf, 0x2C + 12, pti.code);
				writeUnaligned32(oldbuf, 0x2C + 16, 0x70);

				Array.Copy(buf, 0xD0, oldbuf1, 0, 0x80);
				Array.Copy(buf, 0x80, oldbuf1, 0x80, 0x50);
				Array.Copy(buf, 0, oldbuf1, 0xD0, 0x80);

				if (pti.codeExtra != 0)
				{
					sbyte[] tmp = new sbyte[0x14 + 0xA0];
					Array.Copy(oldbuf1, 0x10, tmp, 0x14, 0xA0);
					ScramblePRX(tmp, 0, 0xA0, pti.codeExtra);
					Array.Copy(tmp, 0, oldbuf1, 0x10, 0xA0);
				}

				Array.Copy(oldbuf1, 0x40, oldbuf, 0x40, 0x40);

				for (int iXOR = 0; iXOR < 0x70; iXOR++)
				{
					oldbuf[0x40 + iXOR] = (sbyte)(oldbuf[0x40 + iXOR] ^ pti.key[0x14 + iXOR]);
				}

				// Scramble the data by calling CMD7.
				result = semaphoreModule.hleUtilsBufferCopyWithRange(oldbuf, 0x2C, 0x70, oldbuf, 0x2C, 0x70, KIRK.PSP_KIRK_CMD_DECRYPT);
				if (result != 0)
				{
					System.Console.WriteLine(string.Format("DecryptPRX: KIRK command PSP_KIRK_CMD_DECRYPT returned error {0:D}", result));
				}

				for (int iXOR = 0x6F; iXOR >= 0; iXOR--)
				{
					oldbuf[0x40 + iXOR] = (sbyte)(oldbuf[0x2C + iXOR] ^ pti.key[0x20 + iXOR]);
				}

				for (int k = 0; k < 0x30; k++)
				{
					oldbuf[k + 0x80] = 0;
				}

				// Set mode FieldInfo to 1.
				oldbuf[0xA0] = 0x0;
				oldbuf[0xA1] = 0x0;
				oldbuf[0xA2] = 0x0;
				oldbuf[0xA3] = 0x1;

				Array.Copy(buf, 0xB0, oldbuf, 0xB0, 0x20);
				Array.Copy(buf, 0, oldbuf, 0xD0, 0x80);

				// Call KIRK CMD1 for sealed override decryption.
				result = semaphoreModule.hleUtilsBufferCopyWithRange(buf, 0, size, oldbuf, 0x40, size - 0x40, KIRK.PSP_KIRK_CMD_DECRYPT_PRIVATE);
				if (result != 0)
				{
					System.Console.WriteLine(string.Format("DecryptPRX: KIRK command PSP_KIRK_CMD_DECRYPT_PRIVATE returned error {0:D}", result));
				}
			}
			else
			{ // New encryption method (16 bytes key).
				// Setup temporary buffers.
				sbyte[] buf1 = new sbyte[0x150];
				sbyte[] buf2 = new sbyte[0x150];
				sbyte[] buf3 = new sbyte[0x90];
				sbyte[] buf4 = new sbyte[0xB4];
				sbyte[] buf5 = new sbyte[0x20];
				sbyte[] buf6 = new sbyte[0x28];
				sbyte[] sigbuf = new sbyte[0x28];
				sbyte[] sha1buf = new sbyte[0x14];
				int unk_0xD4 = 0;

				// Copy the first header to buf1.
				Array.Copy(buf, 0, buf1, 0, 0x150);

				// Read in the user key and apply scramble.
				if ((type >= 2 && type <= 7) || type == 9 || type == 10)
				{
					for (int i = 0; i < 9; i++)
					{
						for (int j = 0; j < 0x10; j++)
						{
							buf2[0x14 + ((i << 4) + j)] = (sbyte) pti.key[j];
						}
						buf2[0x14 + ((i << 4))] = (sbyte) i;
					}
				}
				else
				{
					for (int i = 0; i < 0x90; i++)
					{
						buf2[0x14 + i] = (sbyte) pti.key[i];
					}
				}

				ScramblePRX(buf2, 0, 0x90, pti.code);

				// Round XOR key for PRX type 3,5,7 and 10.
				if (type == 3 || type == 5 || type == 7 || type == 10)
				{
					if (!isNullKey(xor2))
					{
						RoundXOR(buf2, 0x90, xor2, null);
					}
				}

				Array.Copy(buf2, 0, buf3, 0, 0x90);

				// Type 9 and 10 specific step.
				if (type == 9 || type == 10)
				{
					Array.Copy(buf, 0x104, buf6, 0, buf6.Length);

					for (int i = 0; i < buf6.Length; i++)
					{
						buf[0x104 + i] = 0;
					}

					Array.Copy(buf6, 0, sigbuf, 0, sigbuf.Length);

					writeUnaligned32(buf, 0, size - 4);

					// Generate SHA1 hash.
					result = semaphoreModule.hleUtilsBufferCopyWithRange(buf, 0, size, buf, 0, size, KIRK.PSP_KIRK_CMD_SHA1_HASH);
					if (result != 0)
					{
						System.Console.WriteLine(string.Format("DecryptPRX: KIRK command PSP_KIRK_CMD_SHA1_HASH returned error {0:D}", result));
					}

					Array.Copy(buf, 0, sha1buf, 0, sha1buf.Length);
					Array.Copy(buf1, 0, buf, 0, 0x20);

					int[] pubKey;
					switch ((tag >> 16) & 0xFF)
					{
						case 0x16:
							pubKey = KeyVault.g_pubkey_28752;
							break;
						case 0x5E:
							pubKey = KeyVault.g_pubkey_28712;
							break;
						default:
							pubKey = KeyVault.g_pubkey_28672;
							break;
					}
					for (int i = 0; i < pubKey.Length; i++)
					{
						buf4[i] = (sbyte) pubKey[i];
					}

					Array.Copy(sha1buf, 0, buf4, 0x28, sha1buf.Length);
					Array.Copy(sigbuf, 0, buf4, 0x28 + sha1buf.Length, sigbuf.Length);

					// Verify ECDSA signature.
					result = semaphoreModule.hleUtilsBufferCopyWithRange(null, 0, 0, buf4, 0, 100, KIRK.PSP_KIRK_CMD_ECDSA_VERIFY);
					if (result != 0)
					{
						System.Console.WriteLine(string.Format("DecryptPRX: KIRK command PSP_KIRK_CMD_ECDSA_VERIFY returned error {0:D}", result));
					}
				}

				if (type == 3)
				{
					Array.Copy(buf1, 0xEC, buf2, 0, 0x40);
					for (int i = 0; i < 0x50; i++)
					{
						buf2[0x40 + i] = 0;
					}

					buf2[0x60] = 0x03;
					buf2[0x70] = 0x50;

					Array.Copy(buf1, 0x80, buf2, 0x90, 0x30);
					Array.Copy(buf1, 0xC0, buf2, 0x90 + 0x30, 0x10);
					Array.Copy(buf1, 0x12C, buf2, 0x90 + 0x30 + 0x10, 0x10);

					sbyte[] tmp = new sbyte[0x50];
					for (int i = 0; i < tmp.Length; i++)
					{
						tmp[i] = buf2[0x90 + i];
					}

					// Round XOR with xor1 and xor2.
					RoundXOR(tmp, 0x50, xor1, xor2);

					Array.Copy(tmp, 0, buf2, 0x90, 0x50);

					// Decrypt signature (type 3).
					result = semaphoreModule.hleUtilsBufferCopyWithRange(buf4, 0, 0xB4, buf2, 0, 0x150, KIRK.PSP_KIRK_CMD_DECRYPT_SIGN);
					if (result != 0)
					{
						System.Console.WriteLine(string.Format("DecryptPRX: KIRK command PSP_KIRK_CMD_DECRYPT_SIGN returned error {0:D}", result));
					}

					// Regenerate signature.
					Array.Copy(buf1, 0xD0, buf2, 0, 0x4);
					for (int i = 0; i < 0x58; i++)
					{
						buf2[0x4 + i] = 0;
					}
					Array.Copy(buf1, 0x140, buf2, 0x5C, 0x10);
					Array.Copy(buf1, 0x12C, buf2, 0x6C, 0x14);
					Array.Copy(buf4, 0x40, buf2, 0x6C, 0x10);
					Array.Copy(buf4, 0, buf2, 0x80, 0x30);
					Array.Copy(buf4, 0x30, buf2, 0xB0, 0x10);
					Array.Copy(buf1, 0xB0, buf2, 0xC0, 0x10);
					Array.Copy(buf1, 0, buf2, 0xD0, 0x80);
				}
				else if (type == 5 || type == 7 || type == 10)
				{
					Array.Copy(buf1, 0x80, buf2, 0x14, 0x30);
					Array.Copy(buf1, 0xC0, buf2, 0x44, 0x10);
					Array.Copy(buf1, 0x12C, buf2, 0x54, 0x10);

					sbyte[] tmp = new sbyte[0x50];
					for (int i = 0; i < tmp.Length; i++)
					{
						tmp[i] = buf2[0x14 + i];
					}

					// Round XOR with xor1 and xor2.
					RoundXOR(tmp, 0x50, xor1, xor2);

					Array.Copy(tmp, 0, buf2, 0x14, 0x50);

					// Apply scramble.
					ScramblePRX(buf2, 0, 0x50, pti.code);

					// Copy to buf4.
					Array.Copy(buf2, 0, buf4, 0, 0x50);

					// Regenerate signature.
					Array.Copy(buf1, 0xD0, buf2, 0, 0x4);
					for (int i = 0; i < 0x58; i++)
					{
						buf2[0x4 + i] = 0;
					}
					Array.Copy(buf1, 0x140, buf2, 0x5C, 0x10);
					Array.Copy(buf1, 0x12C, buf2, 0x6C, 0x14);
					Array.Copy(buf4, 0x40, buf2, 0x6C, 0x10);
					Array.Copy(buf4, 0, buf2, 0x80, 0x30);
					Array.Copy(buf4, 0x30, buf2, 0xB0, 0x10);
					Array.Copy(buf1, 0xB0, buf2, 0xC0, 0x10);
					Array.Copy(buf1, 0, buf2, 0xD0, 0x80);
				}
				else if (type == 2 || type == 4 || type == 6 || type == 9)
				{
					// Regenerate sig check.
					Array.Copy(buf1, 0xD0, buf2, 0, 0x5C);
					Array.Copy(buf1, 0x140, buf2, 0x5C, 0x10);
					Array.Copy(buf1, 0x12C, buf2, 0x6C, 0x14);
					Array.Copy(buf1, 0x80, buf2, 0x80, 0x30);
					Array.Copy(buf1, 0xC0, buf2, 0xB0, 0x10);
					Array.Copy(buf1, 0xB0, buf2, 0xC0, 0x10);
					Array.Copy(buf1, 0, buf2, 0xD0, 0x80);
					if (type == 9)
					{
						for (int i = 0; i < 0x28; i++)
						{
							buf2[0x34 + i] = (sbyte) 0;
						}
					}
				}
				else
				{
					// Regenerate sig check.
					Array.Copy(buf1, 0xD0, buf2, 0, 0x80);
					Array.Copy(buf1, 0x80, buf2, 0x80, 0x50);
					Array.Copy(buf1, 0, buf2, 0xD0, 0x80);
				}

				if (type == 1)
				{
					Array.Copy(buf2, 0x10, buf4, 0x14, 0xA0);
					ScramblePRX(buf4, 0, 0xA0, pti.code);
					Array.Copy(buf4, 0, buf2, 0x10, 0xA0);
				}
				else if ((type >= 2 && type <= 7) || type == 9 || type == 10)
				{
					Array.Copy(buf2, 0x5C, buf4, 0x14, 0x60);

					if (type == 3 || type == 5 || type == 7 || type == 10)
					{
						sbyte[] tmp = new sbyte[0x60];
						for (int i = 0; i < tmp.Length; i++)
						{
							tmp[i] = buf4[0x14 + i];
						}
						RoundXOR(tmp, 0x60, xor1, null);
						Array.Copy(tmp, 0, buf4, 0x14, 0x60);
					}
					ScramblePRX(buf4, 0, 0x60, pti.code);
					Array.Copy(buf4, 0, buf2, 0x5C, 0x60);
				}

				if ((type >= 2 && type <= 7) || type == 9 || type == 10)
				{
					Array.Copy(buf2, 0x6C, buf4, 0, 0x14);

					if (type == 4)
					{
						Array.Copy(buf2, 0, buf2, 0x18, 0x67);
						for (int i = 0; i < 0x18; i++)
						{
							buf2[i] = 0;
						}
					}
					else
					{
						Array.Copy(buf2, 0x5C, buf2, 0x70, 0x10);

						if (type == 6 || type == 7)
						{
							Array.Copy(buf2, 0x3C, buf5, 0, 0x20);
							Array.Copy(buf5, 0, buf2, 0x50, 0x20);
							for (int i = 0; i < 0x38; i++)
							{
								buf2[0x18 + i] = 0;
							}
						}
						else
						{
							for (int i = 0; i < 0x58; i++)
							{
								buf2[0x18 + i] = 0;
							}
						}

						if (unk_0xD4 == 0x80)
						{
							buf2[0x18] = unchecked((sbyte) 0x80);
						}
					}

					// Set the SHA1 block size to digest.
					Array.Copy(buf2, 0, buf2, 0x4, 4);
					writeUnaligned32(buf2, 0, 0x14C);
					Array.Copy(buf3, 0, buf2, 0x8, 0x10);
				}
				else
				{
					// Set the SHA1 block size to digest.
					Array.Copy(buf2, 0x4, buf4, 0, 0x14);
					writeUnaligned32(buf2, 0, 0x14C);
					Array.Copy(buf3, 0, buf2, 0x4, 0x14);
				}

				// Generate SHA1 hash.
				result = semaphoreModule.hleUtilsBufferCopyWithRange(buf2, 0, 0x150, buf2, 0, 0x150, KIRK.PSP_KIRK_CMD_SHA1_HASH);
				if (result != 0)
				{
					System.Console.WriteLine(string.Format("DecryptPRX: KIRK command PSP_KIRK_CMD_SHA1_HASH returned error {0:D}", result));
				}

				if ((type >= 2 && type <= 7) || type == 9 || type == 10)
				{
					sbyte[] tmp1 = new sbyte[0x40];
					sbyte[] tmp2 = new sbyte[0x40];
					sbyte[] tmp3 = new sbyte[0x40 + 0x14];
					sbyte[] tmp4 = new sbyte[0x40];
					sbyte[] tmp5 = new sbyte[0x40];
					sbyte[] tmp6 = new sbyte[0x40];

					for (int i = 0; i < 0x40; i++)
					{
						tmp1[i] = buf2[0x80 + i];
						tmp2[i] = buf3[0x10 + i];
					}

					MixXOR(tmp1, 0x40, tmp1, tmp2);
					Array.Copy(tmp1, 0, tmp3, 0x14, 0x40);
					ScramblePRX(tmp3, 0, 0x40, pti.code);
					Array.Copy(tmp3, 0, buf2, 0x80, 0x40);

					for (int i = 0; i < 0x40; i++)
					{
						tmp4[i] = buf[0x40 + i];
						tmp5[i] = buf2[0x80 + i];
						tmp6[i] = buf3[0x50 + i];
					}

					MixXOR(tmp4, 0x40, tmp5, tmp6);
					Array.Copy(tmp4, 0, buf, 0x40, 0x40);

					if (type == 6 || type == 7)
					{
						Array.Copy(buf5, 0, buf, 0x80, 0x20);
						for (int i = 0; i < 0x10; i++)
						{
							buf[0xA0 + i] = 0;
						}
						buf[0xA4] = 0x0;
						buf[0xA5] = 0x0;
						buf[0xA6] = 0x0;
						buf[0xA7] = 0x1;
						writeUnaligned32(buf, 0xA0, PSP_KIRK_CMD_MODE_CMD1);
					}
					else
					{
						for (int i = 0; i < 0x30; i++)
						{
							buf[0x80 + i] = 0;
						}
						writeUnaligned32(buf, 0xA0, PSP_KIRK_CMD_MODE_CMD1);
					}

					Array.Copy(buf2, 0xC0, buf, 0xB0, 0x10);
					for (int i = 0; i < 0x10; i++)
					{
						buf[0xC0 + i] = 0;
					}
					Array.Copy(buf2, 0xD0, buf, 0xD0, 0x80);
				}
				else
				{
					sbyte[] tmp7 = new sbyte[0x70];
					sbyte[] tmp8 = new sbyte[0x70];
					sbyte[] tmp9 = new sbyte[0x70 + 0x14];
					sbyte[] tmp10 = new sbyte[0x70];
					sbyte[] tmp11 = new sbyte[0x70];
					sbyte[] tmp12 = new sbyte[0x70];
					for (int i = 0; i < 0x70; i++)
					{
						tmp7[i] = buf2[0x40 + i];
						tmp8[i] = buf3[0x14 + i];
					}

					MixXOR(tmp7, 0x70, tmp7, tmp8);
					Array.Copy(tmp7, 0, tmp9, 0x14, 0x70);
					ScramblePRX(tmp9, 0, 0x70, pti.code);
					Array.Copy(tmp9, 0, buf2, 0x40, 0x40);

					for (int i = 0; i < 0x70; i++)
					{
						tmp10[i] = buf[0x40 + i];
						tmp11[i] = buf2[0x40 + i];
						tmp12[i] = buf3[0x20 + i];
					}

					MixXOR(tmp10, 0x70, tmp11, tmp12);
					Array.Copy(tmp10, 0, buf, 0x40, 0x70);
					Array.Copy(buf2, 0xB0, buf, 0xB0, 0xA0);
				}

				if (type == 8)
				{
					if ((buf[0xA4] & 0x1) != 0x1)
					{
						return -1;
					}
				}

				if (unk_0xD4 == 0x80)
				{
					if ((buf[0x590] & 0x1) == 0x1)
					{
						return -1;
					}
					buf[0x590] |= unchecked((sbyte)0x80);
				}

				// Call KIRK CMD1 for sealed override decryption.
				result = semaphoreModule.hleUtilsBufferCopyWithRange(buf, 0, size, buf, 0x40, size, KIRK.PSP_KIRK_CMD_DECRYPT_PRIVATE);
				if (result != 0)
				{
					System.Console.WriteLine(string.Format("DecryptPRX: KIRK command PSP_KIRK_CMD_DECRYPT_PRIVATE returned error {0:D}", result));
				}

				if (retsize < 0x150)
				{
					for (int i = 0; i < (0x150 - retsize); i++)
					{
						buf[retsize + i] = 0;
					}
				}
			}

			return retsize;
		}
	}
}