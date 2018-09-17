﻿/*
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
namespace pspsharp.format
{
	using SceModule = pspsharp.HLE.kernel.types.SceModule;

	public class DeferredVStubHI16 : DeferredStub
	{
		private bool hasSavedValue;
		private short savedValue;

		public DeferredVStubHI16(SceModule sourceModule, string moduleName, int importAddress, int nid) : base(sourceModule, moduleName, importAddress, nid)
		{
		}

		public override void resolve(Memory mem, int address)
		{
			if (!hasSavedValue)
			{
				savedValue = (short) mem.read16(ImportAddress);
				hasSavedValue = true;
			}

			// Perform a R_MIPS_HI16 relocation

			// Retrieve the current address from hi16
			int hiValue = mem.read16(ImportAddress) << 16;
			int value = hiValue << 16;

			// Relocate the address
			value += address;

			// Write back the relocation address to hi16 and lo16
			short relocatedLoValue = (short) value;
			short relocatedHiValue = (short)((int)((uint)value >> 16));
			if (relocatedLoValue < 0)
			{
				relocatedHiValue++;
			}

			mem.write16(ImportAddress, relocatedHiValue);
		}

		public override void unresolve(Memory mem)
		{
			if (hasSavedValue)
			{
				mem.write16(ImportAddress, savedValue);
			}

			if (sourceModule != null)
			{
				// Add this stub back to the list of unresolved imports from the source module
				sourceModule.unresolvedImports.Add(this);
			}
		}

		public override string ToString()
		{
			return string.Format("HI16 {0}", base.ToString());
		}
	}

}