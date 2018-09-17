﻿using System;
using System.Collections.Generic;

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
namespace pspsharp.graphics
{

	//using Logger = org.apache.log4j.Logger;

	using RuntimeContext = pspsharp.Allegrex.compiler.RuntimeContext;
	using IRenderingEngine = pspsharp.graphics.RE.IRenderingEngine;
	using IMemoryReader = pspsharp.memory.IMemoryReader;
	using MemoryReader = pspsharp.memory.MemoryReader;
	using Utilities = pspsharp.util.Utilities;

	/// <summary>
	/// @author gid15
	/// 
	/// </summary>
	public class VertexBuffer
	{
		private static Logger log = VideoEngine.log_Renamed;
		private int bufferId = -1;
		private int bufferAddress;
		private int bufferLength;
		private int stride;
		private int[] cachedMemory;
		private ByteBuffer cachedBuffer;
		private int cachedBufferOffset;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		private Dictionary<int, int> addressAlreadyChecked_Renamed = new Dictionary<int, int>();
		private AddressRange[] dirtyRanges = new AddressRange[0];
		private int numberDirtyRanges = 0;
		private bool reloadBufferDataPending = false;
		private const int bufferUsage = pspsharp.graphics.RE.IRenderingEngine_Fields.RE_DYNAMIC_DRAW;
		private const int bufferTarget = pspsharp.graphics.RE.IRenderingEngine_Fields.RE_ARRAY_BUFFER;
		private const bool workaRoundBufferDataBug = true;

		private class AddressRange
		{
			public int address;
			public int Length;

			public AddressRange()
			{
			}

			public virtual void setRange(int address, int Length)
			{
				this.address = address;
				this.Length = Length;
			}

			public override string ToString()
			{
				return string.Format("AddressRange[0x{0:X8}-0x{1:X8}, Length {2:D}]", address, address + Length, Length);
			}
		}

		public VertexBuffer(int address, int stride)
		{
			bufferAddress = Memory.normalizeAddress(address);
			bufferLength = 0;
			this.stride = stride;
		}

		public virtual void bind(IRenderingEngine re)
		{
			if (bufferId == -1)
			{
				bufferId = re.genBuffer();
			}
			re.bindBuffer(bufferTarget, bufferId);
		}

		private int getBufferAlignment(Buffer buffer, int address)
		{
			if ((address & 3) == 0)
			{
				return 0;
			}

			if (buffer is IntBuffer || buffer is FloatBuffer)
			{
				return address & 3;
			}
			else if (buffer is ShortBuffer)
			{
				return address & 1;
			}

			return 0;
		}

		private void loadFromMemory(int address, int Length)
		{
			if (Length > 0)
			{
				copyToCachedMemory(address, Length);
				Buffer buffer = Memory.Instance.getBuffer(address, Length);
				int bufferAlignment = getBufferAlignment(buffer, address);
				position(address, bufferAlignment);
				Utilities.putBuffer(cachedBuffer, buffer, ByteOrder.LITTLE_ENDIAN, Length + bufferAlignment);
			}
		}

		private bool extend(Buffer buffer, int address, int Length)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'sealed override':
//ORIGINAL LINE: sealed override bool overflowBottom = address < bufferAddress;
			bool overflowBottom = address < bufferAddress;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'sealed override':
//ORIGINAL LINE: sealed override bool overflowTop = address + Length > bufferAddress + bufferLength;
			bool overflowTop = address + Length > bufferAddress + bufferLength;
			bool extended = false;

			if (!overflowBottom && !overflowTop)
			{
				// Most common case: the buffer is fitting
			}
			else if (bufferLength == 0 || (overflowBottom && overflowTop))
			{
				// Create a new buffer
				cachedBufferOffset = getBufferAlignment(buffer, address);
				// Always allocate 3 additional bytes at the end to allow copy
				// from IntBuffer without running into a buffer overflow
				const int alignmentPaddingEnd = 3;
				cachedBuffer = ByteBuffer.allocateDirect(Length + cachedBufferOffset + alignmentPaddingEnd).order(ByteOrder.LITTLE_ENDIAN);
				bufferAddress = address;
				bufferLength = Length;
				cachedMemory = new int[bufferLength >> 2];
				// The buffer has been resized: its content is lost, reload it
				reloadBufferDataPending = true;
				extended = true;
			}
			else if (overflowBottom)
			{
				// Extend the buffer to the bottom
				cachedBufferOffset = getBufferAlignment(buffer, address);
				int extendLength = bufferAddress - address + cachedBufferOffset;
				ByteBuffer newBuffer = ByteBuffer.allocateDirect(extendLength + cachedBuffer.capacity()).order(ByteOrder.LITTLE_ENDIAN);
				newBuffer.position(extendLength);
				cachedBuffer.clear();
				newBuffer.put(cachedBuffer);
				newBuffer.rewind();
				cachedBuffer = newBuffer;
				bufferLength += extendLength;
				int[] newCachedMemory = new int[bufferLength >> 2];
				Array.Copy(cachedMemory, 0, newCachedMemory, extendLength >> 2, cachedMemory.Length);
				cachedMemory = newCachedMemory;
				bufferAddress = address;
				loadFromMemory(bufferAddress + Length, extendLength - Length);
				// The buffer has been resized: its content is lost, reload it
				reloadBufferDataPending = true;
				extended = true;
			}
			else if (overflowTop)
			{
				// Extend the buffer to the top
				int extendLength = address + Length - (bufferAddress + bufferLength);
				ByteBuffer newBuffer = ByteBuffer.allocateDirect(extendLength + cachedBuffer.capacity()).order(ByteOrder.LITTLE_ENDIAN);
				cachedBuffer.clear();
				newBuffer.put(cachedBuffer);
				newBuffer.rewind();
				cachedBuffer = newBuffer;
				int oldBufferEnd = bufferAddress + bufferLength;
				bufferLength += extendLength;
				int[] newCachedMemory = new int[bufferLength >> 2];
				Array.Copy(cachedMemory, 0, newCachedMemory, 0, cachedMemory.Length);
				cachedMemory = newCachedMemory;
				loadFromMemory(oldBufferEnd, address - oldBufferEnd);
				// The buffer has been resized: its content is lost, reload it
				reloadBufferDataPending = true;
				extended = true;
			}

			return extended;
		}

		private void position(int address)
		{
			cachedBuffer.clear();
			cachedBuffer.position(getBufferOffset(address) + cachedBufferOffset);
		}

		private void position(int address, int bufferAlignment)
		{
			position(address - bufferAlignment);
		}

		private void copyToCachedMemory(int address, int Length)
		{
			int offset = getBufferOffset(address) >> 2;
			int n = Length >> 2;
			if (RuntimeContext.hasMemoryInt())
			{
				Array.Copy(RuntimeContext.MemoryInt, address >> 2, cachedMemory, offset, n);
			}
			else
			{
				IMemoryReader memoryReader = MemoryReader.getMemoryReader(address, Length, 4);
				for (int i = 0; i < n; i++)
				{
					cachedMemory[offset + i] = memoryReader.readNext();
				}
			}
		}

		private void checkDirty(IRenderingEngine re)
		{
			if (reloadBufferDataPending)
			{
				bind(re);
				position(bufferAddress);
				re.setBufferData(bufferTarget, cachedBuffer.remaining(), cachedBuffer, bufferUsage);
				reloadBufferDataPending = false;
				numberDirtyRanges = 0;
			}
			else if (numberDirtyRanges > 0)
			{
				bind(re);
				for (int i = 0; i < numberDirtyRanges; i++)
				{
					position(dirtyRanges[i].address);
					re.setBufferSubData(bufferTarget, cachedBuffer.position(), dirtyRanges[i].Length, cachedBuffer);
				}
				numberDirtyRanges = 0;
			}
		}

		private bool cachedMemoryEquals(int address, int Length)
		{
			IMemoryReader memoryReader = MemoryReader.getMemoryReader(address, Length, 4);
			int n = Length >> 2;
			int offset = getBufferOffset(address) >> 2;
			for (int i = 0; i < n; i++)
			{
				if (cachedMemory[offset + i] != memoryReader.readNext())
				{
					if (log.TraceEnabled)
					{
						log.trace(string.Format("VertexBuffer.cachedMemoryEquals(0x{0:X8}, {1:D}): are not equal", address, Length));
					}
					return false;
				}
			}

			if (log.TraceEnabled)
			{
				log.trace(string.Format("VertexBuffer.cachedMemoryEquals(0x{0:X8}, {1:D}): are equal", address, Length));
			}
			return true;
		}

		private void addDirtyRange(int address, int Length)
		{
			for (int i = 0; i < numberDirtyRanges; i++)
			{
				if (dirtyRanges[i].address == address)
				{
					if (Length > dirtyRanges[i].Length)
					{
						dirtyRanges[i].Length = Length;
					}
					return;
				}
			}

			if (numberDirtyRanges >= dirtyRanges.Length)
			{
				// Extend dirtyRanges array
				AddressRange[] newDirtyRanges = new AddressRange[dirtyRanges.Length + 10];
				Array.Copy(dirtyRanges, 0, newDirtyRanges, 0, dirtyRanges.Length);
				for (int i = dirtyRanges.Length; i < newDirtyRanges.Length; i++)
				{
					newDirtyRanges[i] = new AddressRange();
				}
				dirtyRanges = newDirtyRanges;
			}

			dirtyRanges[numberDirtyRanges].setRange(address, Length);
			numberDirtyRanges++;
		}

		public virtual void preLoad(Buffer buffer, int address, int Length)
		{
			lock (this)
			{
				load(null, buffer, address, Length);
			}
		}

		public virtual void load(IRenderingEngine re, Buffer buffer, int address, int Length)
		{
			lock (this)
			{
				address = Memory.normalizeAddress(address);
        
				if (log.TraceEnabled)
				{
					log.trace(string.Format("VertexBuffer.load(0x{0:X8}, {1:D}) in {2}", address, Length, this.ToString()));
				}
				if (!addressAlreadyChecked(address, Length))
				{
					bool extended = extend(buffer, address, Length);
					// Check if the memory content has changed
					if (extended || !cachedMemoryEquals(address, Length))
					{
						int bufferAlignment = getBufferAlignment(buffer, address);
						position(address, bufferAlignment);
						Utilities.putBuffer(cachedBuffer, buffer, ByteOrder.LITTLE_ENDIAN, Length + bufferAlignment);
						buffer.rewind();
        
						if (re != null)
						{
							if (log.TraceEnabled)
							{
//JAVA TO C# CONVERTER TODO TASK: The following line has a Java format specifier which cannot be directly translated to .NET:
//ORIGINAL LINE: log.trace(String.format("VertexBuffer reload buffer 0x%08X, %d, extended=%b", address, Length, extended));
								log.trace(string.Format("VertexBuffer reload buffer 0x%08X, %d, extended=%b", address, Length, extended));
							}
        
							// No need to update the sub data if the complete buffer has been reloaded...
							bool updateSubData = !reloadBufferDataPending;
        
							checkDirty(re);
        
							if (updateSubData)
							{
								position(address);
								bind(re);
								re.setBufferSubData(bufferTarget, cachedBuffer.position(), Length, cachedBuffer);
							}
						}
						else
						{
							addDirtyRange(address, Length);
						}
        
						copyToCachedMemory(address, Length);
					}
					else if (re != null)
					{
						checkDirty(re);
        
						// Here the buffer data should not need to be reloaded, it is matching
						// the previous data. However, due to a driver bug (?), the buffer data
						// has sometimes been corrupted in the GPU. This problem seems to happen
						// only under some circumstances but I was not able to identify the exact
						// conditions. Just reloading a single byte of the buffer restores
						// the correct data in the buffer. This is why I assumed this is a driver bug.
						// This workaRound could however completely break the performance of the vertex
						// cache!
						if (workaRoundBufferDataBug)
						{
							bind(re);
							position(bufferAddress);
							re.setBufferSubData(bufferTarget, cachedBuffer.position(), 1, cachedBuffer);
						}
					}
        
					setAddressAlreadyChecked(address, Length);
				}
				else if (re != null)
				{
					if (log.TraceEnabled)
					{
						log.trace(string.Format("VertexBuffer address already checked 0x{0:X8}, {1:D}", address, Length));
					}
					checkDirty(re);
				}
			}
		}

		public virtual void delete(IRenderingEngine re)
		{
			lock (this)
			{
				if (bufferId != -1)
				{
					re.deleteBuffer(bufferId);
					bufferId = -1;
				}
				bufferLength = 0;
				bufferAddress = 0;
				stride = 0;
				cachedBuffer = null;
				cachedMemory = null;
			}
		}

		public virtual int getBufferOffset(int address)
		{
			address = Memory.normalizeAddress(address);
			return address - bufferAddress;
		}

		public virtual bool isAddressInside(int address, int Length, int gapSize)
		{
			address = Memory.normalizeAddress(address);
			int endAddress = address + Length;
			int startBuffer = bufferAddress - gapSize;
			int endBuffer = bufferAddress + bufferLength + gapSize;

			// start address inside the buffer
			if (startBuffer <= address && address < endBuffer)
			{
				return true;
			}
			// end address inside the buffer
			if (startBuffer <= endAddress && endAddress < endBuffer)
			{
				return true;
			}
			// start & end address including the buffer
			if (address < startBuffer && endBuffer < endAddress)
			{
				return true;
			}

			return false;
		}

		public virtual void resetAddressAlreadyChecked()
		{
			lock (this)
			{
				addressAlreadyChecked_Renamed.Clear();
			}
		}

		private bool addressAlreadyChecked(int address, int Length)
		{
			int? checkedLength = addressAlreadyChecked_Renamed[address];
			if (checkedLength == null)
			{
				return false;
			}

			return checkedLength.Value >= Length;
		}

		private void setAddressAlreadyChecked(int address, int Length)
		{
			addressAlreadyChecked_Renamed[address] = Length;
		}

		public virtual int Stride
		{
			get
			{
				return stride;
			}
		}

		public virtual int Length
		{
			get
			{
				return bufferLength;
			}
		}

		public virtual int Id
		{
			get
			{
				return bufferId;
			}
		}

		public override string ToString()
		{
			return string.Format("VertexBuffer[0x{0:X8}-0x{1:X8}, Length {2:D}, stride {3:D}, id {4:D}]", bufferAddress, bufferAddress + bufferLength, bufferLength, stride, bufferId);
		}
	}

}