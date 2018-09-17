﻿using System.Collections.Generic;

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
namespace pspsharp.HLE.modules
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static pspsharp.HLE.kernel.types.SceKernelThreadInfo.THREAD_CALLBACK_USER_DEFINED;


	//using Logger = org.apache.log4j.Logger;

	using SceUidManager = pspsharp.HLE.kernel.managers.SceUidManager;
	using pspBaseCallback = pspsharp.HLE.kernel.types.pspBaseCallback;
	using SysMemInfo = pspsharp.HLE.modules.SysMemUserForUser.SysMemInfo;
	using Utilities = pspsharp.util.Utilities;

	// Information based on
	// https://github.com/RPCS3/rpcs3/blob/master/rpcs3/Emu/PSP2/Modules/sceNpMatching.cpp
	public class sceNpMatching2 : HLEModule
	{
		//public static Logger log = Modules.getLogger("sceNpMatching2");
		private const string idContextPurpose = "sceNpMatching2Context";
		protected internal int defaultRequestCallbackFunction;
		protected internal int defaultRequestCallbackArgument;
		protected internal int defaultRequestTimeout;
		protected internal int defaultRequestAppReqId;
		protected internal int defaultRoomEventCallbackFunction;
		protected internal int defaultRoomEventCallbackArgument;
		protected internal int defaultRoomMessageCallbackFunction;
		protected internal int defaultRoomMessageCallbackArgument;
		protected internal int signalingCallbackFunction;
		protected internal int signalingCallbackArgument;
		protected internal IDictionary<int, MatchingContext> contextMap = new Dictionary<int, sceNpMatching2.MatchingContext>();
		protected internal SysMemInfo dataBuffer;

		private class MatchingContext
		{
			internal bool started;

			public virtual bool Started
			{
				get
				{
					return started;
				}
				set
				{
					this.started = value;
				}
			}

		}

		private void notifyRequestCallback(int ctxId)
		{
			MatchingContext context = contextMap[ctxId];
			if (context == null || !context.Started || defaultRequestCallbackFunction == 0)
			{
				return;
			}

			pspBaseCallback requestCallback = Modules.ThreadManForUserModule.hleKernelCreateCallback(defaultRequestCallbackFunction, 6);
			if (Modules.ThreadManForUserModule.hleKernelRegisterCallback(THREAD_CALLBACK_USER_DEFINED, requestCallback))
			{
				if (dataBuffer == null)
				{
					dataBuffer = Modules.SysMemUserForUserModule.malloc(SysMemUserForUser.KERNEL_PARTITION_ID, "sceNpMatching2-DataBuffer", SysMemUserForUser.PSP_SMEM_Low, 128, 0);
					//if (log.DebugEnabled)
					{
						Console.WriteLine(string.Format("sceNpMatching2.notifyRequestCallback allocated dataBuffer {0}", dataBuffer));
					}
				}
				Memory mem = Emulator.Memory;
				mem.memset(dataBuffer.addr, (sbyte) 0, dataBuffer.size);

				int reqId = 0x00011111; // Dummy value for debugging
				int @event = 0x2222; // Dummy value for debugging
				int data = dataBuffer.addr;
				int errorCode = 0;

				// Write unknown values to data buffer for debugging
				// The reqId contains a special value in the upper 16-bit
				// which seems to be in range [0..16].
				switch ((int)((uint)reqId >> 16))
				{
					case 0:
					case 6:
					case 7:
					case 9:
					case 10:
					case 11:
					case 13:
					case 16:
					{
						// In those cases, the data buffer doesn't seem to be used
						data = 0;
						break;
					}
					case 1:
					{
						mem.write16(data, (short) 0x3333);
						mem.write8(data + 2, (sbyte) 0x44);
						break;
					}
					case 2:
					{
						int ptr = data + 8;
						mem.write32(data, ptr); // Pointer to 64 bytes
						mem.write32(data + 4, 0);
						mem.write32(ptr, 0); // Pointer to next 64 bytes or NULL
						for (int i = 4; i < 64; i += 4)
						{
							mem.write32(ptr + i, 0x12345600 + i);
						}
						break;
					}
					case 3:
					{
						mem.write32(data + 0, 0x33333333);
						mem.write32(data + 4, 0x44444444);
						mem.write32(data + 8, 0x55555555);
						mem.write32(data + 12, 0x66666666);
						break;
					}
					case 4:
					case 5:
					{
						int ptr = data + 4;
						mem.write32(data, ptr); // Pointer to 64 bytes
						mem.write16(ptr, (short) 0x3333);
						for (int i = 4; i < 64; i += 4)
						{
							mem.write32(ptr + i, 0x12345600 + i);
						}
						int ptr2 = ptr + 64;
						mem.write32(ptr + 44, ptr2); // Pointer to 58 bytes
						mem.write16(ptr2 + 56, (short) 0x4444);
						break;
					}
					case 8:
					{
						mem.write32(data, 1); // Seems to be a flag having value 0 or 1
						break;
					}
					case 12:
					case 14:
					case 15:
					{
						// Two 32-bit values (a 64-bit timestamp maybe?)
						mem.write32(data + 0, 0x12345678);
						mem.write32(data + 4, unchecked((int)0x9ABCDEF0));
						break;
					}
				}

				requestCallback.setArgument(0, ctxId);
				requestCallback.setArgument(1, reqId);
				requestCallback.setArgument(2, @event);
				requestCallback.setArgument(3, errorCode);
				requestCallback.setArgument(4, data);
				requestCallback.setArgument(5, defaultRequestCallbackArgument);
				Modules.ThreadManForUserModule.hleKernelNotifyCallback(THREAD_CALLBACK_USER_DEFINED, requestCallback);
			}
		}

		private void notifySignalingCallback(int ctxId)
		{
			MatchingContext context = contextMap[ctxId];
			if (context == null || !context.Started || signalingCallbackFunction == 0)
			{
				return;
			}

			pspBaseCallback signalingCallback = Modules.ThreadManForUserModule.hleKernelCreateCallback(signalingCallbackFunction, 8);
			if (Modules.ThreadManForUserModule.hleKernelRegisterCallback(THREAD_CALLBACK_USER_DEFINED, signalingCallback))
			{
				long roomId = 0x123456789ABCDEF0L;
				int peerMemberId = 0x1111;
				int @event = 0x5101; // 0x5101 - 0x5106
				int errorCode = 0;
				signalingCallback.setArgument(0, ctxId);
				signalingCallback.setArgument(2, (int) roomId);
				signalingCallback.setArgument(3, (int)((long)((ulong)roomId >> 32)));
				signalingCallback.setArgument(4, peerMemberId);
				signalingCallback.setArgument(5, @event);
				signalingCallback.setArgument(6, errorCode);
				signalingCallback.setArgument(7, signalingCallbackArgument);
				Modules.ThreadManForUserModule.hleKernelNotifyCallback(THREAD_CALLBACK_USER_DEFINED, signalingCallback);
			}
		}

		private void notifyRoomEventCallback(int ctxId)
		{
			MatchingContext context = contextMap[ctxId];
			if (context == null || !context.Started || defaultRoomEventCallbackFunction == 0)
			{
				return;
			}

			pspBaseCallback roomEventCallback = Modules.ThreadManForUserModule.hleKernelCreateCallback(defaultRoomEventCallbackFunction, 7);
			if (Modules.ThreadManForUserModule.hleKernelRegisterCallback(THREAD_CALLBACK_USER_DEFINED, roomEventCallback))
			{
				long roomId = 0x123456789ABCDEF0L;
				int @event = 0x1101; // 0x1101 - 0x1109
				int errorCode = 0;
				roomEventCallback.setArgument(0, ctxId);
				roomEventCallback.setArgument(2, (int) roomId);
				roomEventCallback.setArgument(3, (int)((long)((ulong)roomId >> 32)));
				roomEventCallback.setArgument(4, @event);
				roomEventCallback.setArgument(5, errorCode);
				roomEventCallback.setArgument(6, defaultRoomEventCallbackArgument);
				Modules.ThreadManForUserModule.hleKernelNotifyCallback(THREAD_CALLBACK_USER_DEFINED, roomEventCallback);
			}
		}

		private void notifyRoomMessageCallback(int ctxId)
		{
			MatchingContext context = contextMap[ctxId];
			if (context == null || !context.Started || defaultRoomMessageCallbackFunction == 0)
			{
				return;
			}

			pspBaseCallback roomMessageCallback = Modules.ThreadManForUserModule.hleKernelCreateCallback(defaultRoomMessageCallbackFunction, 8);
			if (Modules.ThreadManForUserModule.hleKernelRegisterCallback(THREAD_CALLBACK_USER_DEFINED, roomMessageCallback))
			{
				if (dataBuffer == null)
				{
					dataBuffer = Modules.SysMemUserForUserModule.malloc(SysMemUserForUser.KERNEL_PARTITION_ID, "sceNpMatching2-DataBuffer", SysMemUserForUser.PSP_SMEM_Low, 128, 0);
					//if (log.DebugEnabled)
					{
						Console.WriteLine(string.Format("sceNpMatching2.notifyRoomMessageCallback allocated dataBuffer {0}", dataBuffer));
					}
				}
				Memory mem = Emulator.Memory;
				mem.memset(dataBuffer.addr, (sbyte) 0, dataBuffer.size);

				long roomId = 0x123456789ABCDEF0L;
				int srcMemberId = 0x1111;
				int @event = 0x2101; // 0x2101, 0x2102
				int data = dataBuffer.addr;
				string dummyString = "Hello, world!";

				int stringData = data + 24;
				mem.write32(data + 12, stringData);
				mem.write32(data + 16, dummyString.Length);
				mem.write32(data + 20, 1); // Seems to be a flag having value 0 or 1
				Utilities.writeStringZ(mem, stringData, dummyString);

				roomMessageCallback.setArgument(0, ctxId);
				roomMessageCallback.setArgument(2, (int) roomId);
				roomMessageCallback.setArgument(3, (int)((long)((ulong)roomId >> 32)));
				roomMessageCallback.setArgument(4, srcMemberId);
				roomMessageCallback.setArgument(5, @event);
				roomMessageCallback.setArgument(6, data);
				roomMessageCallback.setArgument(7, defaultRoomMessageCallbackArgument);
				Modules.ThreadManForUserModule.hleKernelNotifyCallback(THREAD_CALLBACK_USER_DEFINED, roomMessageCallback);
			}
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x2E61F6E1, version = 150) public int sceNpMatching2Init(int poolSize, int threadPriority, int cpuAffinityMask, int threadStackSize)
		[HLEFunction(nid : 0x2E61F6E1, version : 150)]
		public virtual int sceNpMatching2Init(int poolSize, int threadPriority, int cpuAffinityMask, int threadStackSize)
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x8BF37D8C, version = 150) public int sceNpMatching2Term()
		[HLEFunction(nid : 0x8BF37D8C, version : 150)]
		public virtual int sceNpMatching2Term()
		{
			// No parameters
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x12C5A111, version = 150) public int sceNpMatching2GetRoomDataExternalList()
		[HLEFunction(nid : 0x12C5A111, version : 150)]
		public virtual int sceNpMatching2GetRoomDataExternalList()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x1421514B, version = 150) public int sceNpMatching2SetDefaultRoomEventOptParam(int ctxId, pspsharp.HLE.TPointer optParam)
		[HLEFunction(nid : 0x1421514B, version : 150)]
		public virtual int sceNpMatching2SetDefaultRoomEventOptParam(int ctxId, TPointer optParam)
		{
			int callbackFunction = optParam.getValue32(0);
			int callbackArgument = optParam.getValue32(4);
			bool unknownFlag1 = optParam.getValue32(8) != 0;
			bool unknownFlag2 = optParam.getValue32(12) != 0;
			bool unknownFlag3 = optParam.getValue32(16) != 0;
			bool unknownFlag4 = optParam.getValue32(20) != 0;

			//if (log.DebugEnabled)
			{
//JAVA TO C# CONVERTER TODO TASK: The following line has a Java format specifier which cannot be directly translated to .NET:
//ORIGINAL LINE: Console.WriteLine(String.format("sceNpMatching2SetDefaultRoomEventOptParam callbackFunction=0x%08X, callbackArgument=0x%X, unknownFlag1=%b, unknownFlag2=%b, unknownFlag3=%b, unknownFlag4=%b", callbackFunction, callbackArgument, unknownFlag1, unknownFlag2, unknownFlag3, unknownFlag4));
				Console.WriteLine(string.Format("sceNpMatching2SetDefaultRoomEventOptParam callbackFunction=0x%08X, callbackArgument=0x%X, unknownFlag1=%b, unknownFlag2=%b, unknownFlag3=%b, unknownFlag4=%b", callbackFunction, callbackArgument, unknownFlag1, unknownFlag2, unknownFlag3, unknownFlag4));
			}

			defaultRoomEventCallbackFunction = callbackFunction;
			defaultRoomEventCallbackArgument = callbackArgument;

			notifyRoomEventCallback(ctxId);

			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x190FF903, version = 150) public int sceNpMatching2ContextStart(int ctxId)
		[HLEFunction(nid : 0x190FF903, version : 150)]
		public virtual int sceNpMatching2ContextStart(int ctxId)
		{
			MatchingContext context = contextMap[ctxId];
			if (context == null)
			{
				return -1;
			}

			context.Started = true;

			notifyRequestCallback(ctxId);
			notifySignalingCallback(ctxId);
			notifyRoomEventCallback(ctxId);
			notifyRoomMessageCallback(ctxId);

			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x22F38DAF, version = 150) public int sceNpMatching2GetMemoryStat()
		[HLEFunction(nid : 0x22F38DAF, version : 150)]
		public virtual int sceNpMatching2GetMemoryStat()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x2B3892FC, version = 150) public int sceNpMatching2ContextStop(int ctxId)
		[HLEFunction(nid : 0x2B3892FC, version : 150)]
		public virtual int sceNpMatching2ContextStop(int ctxId)
		{
			MatchingContext context = contextMap[ctxId];
			if (context == null)
			{
				return -1;
			}

			context.Started = false;

			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x3892E9A6, version = 150) public int sceNpMatching2SignalingGetConnectionInfo()
		[HLEFunction(nid : 0x3892E9A6, version : 150)]
		public virtual int sceNpMatching2SignalingGetConnectionInfo()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x3DE70241, version = 150) public int sceNpMatching2DestroyContext(int ctxId)
		[HLEFunction(nid : 0x3DE70241, version : 150)]
		public virtual int sceNpMatching2DestroyContext(int ctxId)
		{
			if (dataBuffer != null)
			{
				Modules.SysMemUserForUserModule.free(dataBuffer);
				dataBuffer = null;
			}

			if (!SceUidManager.releaseId(ctxId, idContextPurpose))
			{
				return -1;
			}

			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x495E97BD, version = 150) public int sceNpMatching2GrantRoomOwner()
		[HLEFunction(nid : 0x495E97BD, version : 150)]
		public virtual int sceNpMatching2GrantRoomOwner()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x4EE3A8EC, version = 150) public int sceNpMatching2GetServerInfo(int ctxId, pspsharp.HLE.TPointer16 serverIdAddr, pspsharp.HLE.TPointer32 unknown1, pspsharp.HLE.TPointer32 unknown2)
		[HLEFunction(nid : 0x4EE3A8EC, version : 150)]
		public virtual int sceNpMatching2GetServerInfo(int ctxId, TPointer16 serverIdAddr, TPointer32 unknown1, TPointer32 unknown2)
		{
			int serverId = serverIdAddr.Value;

			//if (log.DebugEnabled)
			{
				Console.WriteLine(string.Format("sceNpMatching2GetServerInfo serverId=0x{0:X}, unknown1: {1}, unknown2: {2}", serverId, Utilities.getMemoryDump(unknown1.Address, 16), Utilities.getMemoryDump(unknown2.Address, 20)));
			}

			unknown2.setValue(0x00010000);

			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x5030CC53, version = 150) public int sceNpMatching2CreateContext(pspsharp.HLE.TPointer communicationId, pspsharp.HLE.TPointer passPhrase, pspsharp.HLE.TPointer16 ctxId, int unknown)
		[HLEFunction(nid : 0x5030CC53, version : 150)]
		public virtual int sceNpMatching2CreateContext(TPointer communicationId, TPointer passPhrase, TPointer16 ctxId, int unknown)
		{
			//if (log.DebugEnabled)
			{
				Console.WriteLine(string.Format("sceNpMatching2CreateContext communicationId={0}, passPhrase={1}", Utilities.getMemoryDump(communicationId.Address, 12), Utilities.getMemoryDump(passPhrase.Address, 128)));
			}

			// Returning a ctxId in range [1..7]
			int uid = SceUidManager.getNewId(idContextPurpose, 1, 7);
			if (uid == SceUidManager.INVALID_ID)
			{
				return -1;
			}

			contextMap[uid] = new MatchingContext();

			//if (log.DebugEnabled)
			{
				Console.WriteLine(string.Format("sceNpMatching2CreateContext returning 0x{0:X}", uid));
			}
			ctxId.Value = uid;

			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x55F7837F, version = 150) public int sceNpMatching2SendRoomChatMessage()
		[HLEFunction(nid : 0x55F7837F, version : 150)]
		public virtual int sceNpMatching2SendRoomChatMessage()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x5C7DB6A4, version = 150) public int sceNpMatching2GetRoomMemberDataInternalList()
		[HLEFunction(nid : 0x5C7DB6A4, version : 150)]
		public virtual int sceNpMatching2GetRoomMemberDataInternalList()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x631682CC, version = 150) public int sceNpMatching2SetDefaultRequestOptParam(int ctxId, pspsharp.HLE.TPointer optParam)
		[HLEFunction(nid : 0x631682CC, version : 150)]
		public virtual int sceNpMatching2SetDefaultRequestOptParam(int ctxId, TPointer optParam)
		{
			int callbackFunction = optParam.getValue32(0);
			int callbackArgument = optParam.getValue32(4);
			int timeout = optParam.getValue32(8);
			int appReqId = optParam.getValue16(12);

			//if (log.DebugEnabled)
			{
				Console.WriteLine(string.Format("sceNpMatching2SetDefaultRequestOptParam callbackFunction=0x{0:X8}, callbackArgument=0x{1:X}, timeout=0x{2:X}, appReqId=0x{3:X}", callbackFunction, callbackArgument, timeout, appReqId));
			}

			defaultRequestCallbackFunction = callbackFunction;
			defaultRequestCallbackArgument = callbackArgument;
			defaultRequestTimeout = timeout;
			defaultRequestAppReqId = appReqId;

			notifyRequestCallback(ctxId);

			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x6D6D0C75, version = 150) public int sceNpMatching2SignalingGetConnectionStatus()
		[HLEFunction(nid : 0x6D6D0C75, version : 150)]
		public virtual int sceNpMatching2SignalingGetConnectionStatus()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x7BBFC427, version = 150) public int sceNpMatching2JoinRoom()
		[HLEFunction(nid : 0x7BBFC427, version : 150)]
		public virtual int sceNpMatching2JoinRoom()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x7D1D5F5E, version = 150) public int sceNpMatching2SetUserInfo()
		[HLEFunction(nid : 0x7D1D5F5E, version : 150)]
		public virtual int sceNpMatching2SetUserInfo()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x7DAA8A90, version = 150) public int sceNpMatching2SetRoomMemberDataInternal()
		[HLEFunction(nid : 0x7DAA8A90, version : 150)]
		public virtual int sceNpMatching2SetRoomMemberDataInternal()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x80F61558, version = 150) public int sceNpMatching2GetRoomMemberIdListLocal()
		[HLEFunction(nid : 0x80F61558, version : 150)]
		public virtual int sceNpMatching2GetRoomMemberIdListLocal()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x81C13E6D, version = 150) public int sceNpMatching2SearchRoom()
		[HLEFunction(nid : 0x81C13E6D, version : 150)]
		public virtual int sceNpMatching2SearchRoom()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x8CD109E7, version = 150) public int sceNpMatching2SignalingGetPeerNetInfo()
		[HLEFunction(nid : 0x8CD109E7, version : 150)]
		public virtual int sceNpMatching2SignalingGetPeerNetInfo()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x9462C05A, version = 150) public int sceNpMatching2SignalingCancelPeerNetInfo()
		[HLEFunction(nid : 0x9462C05A, version : 150)]
		public virtual int sceNpMatching2SignalingCancelPeerNetInfo()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x97529ECC, version = 150) public int sceNpMatching2KickoutRoomMember()
		[HLEFunction(nid : 0x97529ECC, version : 150)]
		public virtual int sceNpMatching2KickoutRoomMember()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0x9A67F5D0, version = 150) public int sceNpMatching2SetSignalingOptParam()
		[HLEFunction(nid : 0x9A67F5D0, version : 150)]
		public virtual int sceNpMatching2SetSignalingOptParam()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0xA3C298D1, version = 150) public int sceNpMatching2RegisterSignalingCallback(int ctxId, pspsharp.HLE.TPointer callbackFunction, int callbackArgument)
		[HLEFunction(nid : 0xA3C298D1, version : 150)]
		public virtual int sceNpMatching2RegisterSignalingCallback(int ctxId, TPointer callbackFunction, int callbackArgument)
		{
			signalingCallbackFunction = callbackFunction.Address;
			signalingCallbackArgument = callbackArgument;

			notifySignalingCallback(ctxId);

			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0xA53E7C69, version = 150) public int sceNpMatching2GetWorldInfoList(int ctxId, pspsharp.HLE.TPointer16 serverIdAddr, @CanBeNull pspsharp.HLE.TPointer optParam, pspsharp.HLE.TPointer32 assignedReqId)
		[HLEFunction(nid : 0xA53E7C69, version : 150)]
		public virtual int sceNpMatching2GetWorldInfoList(int ctxId, TPointer16 serverIdAddr, TPointer optParam, TPointer32 assignedReqId)
		{
			if (optParam.NotNull)
			{
				int callbackFunction = optParam.getValue32(0);
				int callbackArgument = optParam.getValue32(4);
				int timeout = optParam.getValue32(8);
				int appReqId = optParam.getValue16(12);

				//if (log.DebugEnabled)
				{
					Console.WriteLine(string.Format("sceNpMatching2GetWorldInfoList callbackFunction=0x{0:X8}, callbackArgument=0x{1:X}, timeout=0x{2:X}, appReqId=0x{3:X}", callbackFunction, callbackArgument, timeout, appReqId));
				}
			}

			int serverId = serverIdAddr.Value;

			//if (log.DebugEnabled)
			{
				Console.WriteLine(string.Format("sceNpMatching2GetWorldInfoList serverId=0x{0:X}", serverId));
			}

			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0xA5775DBF, version = 150) public int sceNpMatching2GetRoomMemberDataInternal()
		[HLEFunction(nid : 0xA5775DBF, version : 150)]
		public virtual int sceNpMatching2GetRoomMemberDataInternal()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0xAAD0946A, version = 150) public int sceNpMatching2CreateJoinRoom()
		[HLEFunction(nid : 0xAAD0946A, version : 150)]
		public virtual int sceNpMatching2CreateJoinRoom()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0xC7E72EC5, version = 150) public int sceNpMatching2GetSignalingOptParamLocal()
		[HLEFunction(nid : 0xC7E72EC5, version : 150)]
		public virtual int sceNpMatching2GetSignalingOptParamLocal()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0xC870535A, version = 150) public int sceNpMatching2LeaveRoom()
		[HLEFunction(nid : 0xC870535A, version : 150)]
		public virtual int sceNpMatching2LeaveRoom()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0xC8FC5D41, version = 150) public int sceNpMatching2GetUserInfoList()
		[HLEFunction(nid : 0xC8FC5D41, version : 150)]
		public virtual int sceNpMatching2GetUserInfoList()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0xD13491AB, version = 150) public int sceNpMatching2SetDefaultRoomMessageOptParam(int ctxId, pspsharp.HLE.TPointer optParam)
		[HLEFunction(nid : 0xD13491AB, version : 150)]
		public virtual int sceNpMatching2SetDefaultRoomMessageOptParam(int ctxId, TPointer optParam)
		{
			int callbackFunction = optParam.getValue32(0);
			int callbackArgument = optParam.getValue32(4);

			//if (log.DebugEnabled)
			{
				Console.WriteLine(string.Format("sceNpMatching2SetDefaultRoomMessageOptParam callbackFunction=0x{0:X8}, callbackArgument=0x{1:X}", callbackFunction, callbackArgument));
			}

			defaultRoomMessageCallbackFunction = callbackFunction;
			defaultRoomMessageCallbackArgument = callbackArgument;

			notifyRoomMessageCallback(ctxId);

			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0xD7D4AEB2, version = 150) public int sceNpMatching2SetRoomDataExternal()
		[HLEFunction(nid : 0xD7D4AEB2, version : 150)]
		public virtual int sceNpMatching2SetRoomDataExternal()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0xDFEDB642, version = 150) public int sceNpMatching2SignalingGetPeerNetInfoResult()
		[HLEFunction(nid : 0xDFEDB642, version : 150)]
		public virtual int sceNpMatching2SignalingGetPeerNetInfoResult()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0xE313E586, version = 150) public int sceNpMatching2GetRoomDataInternal()
		[HLEFunction(nid : 0xE313E586, version : 150)]
		public virtual int sceNpMatching2GetRoomDataInternal()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0xE6C93DBD, version = 150) public int sceNpMatching2SetRoomDataInternal()
		[HLEFunction(nid : 0xE6C93DBD, version : 150)]
		public virtual int sceNpMatching2SetRoomDataInternal()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0xEF683F4F, version = 150) public int sceNpMatching2GetRoomDataInternalLocal()
		[HLEFunction(nid : 0xEF683F4F, version : 150)]
		public virtual int sceNpMatching2GetRoomDataInternalLocal()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0xF22C7ADC, version = 150) public int sceNpMatching2GetRoomMemberDataInternalLocal()
		[HLEFunction(nid : 0xF22C7ADC, version : 150)]
		public virtual int sceNpMatching2GetRoomMemberDataInternalLocal()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0xF47342FC, version = 150) public int sceNpMatching2GetServerIdListLocal(int ctxId, pspsharp.HLE.TPointer16 serverIds, int maxServerIds)
		[HLEFunction(nid : 0xF47342FC, version : 150)]
		public virtual int sceNpMatching2GetServerIdListLocal(int ctxId, TPointer16 serverIds, int maxServerIds)
		{
			// Return dummy values for debugging
			for (int i = 0; i < maxServerIds; i++)
			{
				serverIds.setValue(i * 2, i + 0x1234);
			}

			return maxServerIds;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0xF739BE92, version = 150) public int sceNpMatching2GetRoomPasswordLocal()
		[HLEFunction(nid : 0xF739BE92, version : 150)]
		public virtual int sceNpMatching2GetRoomPasswordLocal()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0xF940D9AD, version = 150) public int sceNpMatching2SendRoomMessage()
		[HLEFunction(nid : 0xF940D9AD, version : 150)]
		public virtual int sceNpMatching2SendRoomMessage()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0xFADBA9DB, version = 150) public int sceNpMatching2AbortRequest()
		[HLEFunction(nid : 0xFADBA9DB, version : 150)]
		public virtual int sceNpMatching2AbortRequest()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0xFBF494C0, version = 150) public int sceNpMatching2GetRoomMemberDataExternalList()
		[HLEFunction(nid : 0xFBF494C0, version : 150)]
		public virtual int sceNpMatching2GetRoomMemberDataExternalList()
		{
			return 0;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @HLEUnimplemented @HLEFunction(nid = 0xFF32EA05, version = 150) public int sceNpMatching2SignalingGetLocalNetInfo()
		[HLEFunction(nid : 0xFF32EA05, version : 150)]
		public virtual int sceNpMatching2SignalingGetLocalNetInfo()
		{
			return 0;
		}
	}

}