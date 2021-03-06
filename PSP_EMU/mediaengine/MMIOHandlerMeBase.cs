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
namespace pspsharp.mediaengine
{

	using sceMeCore = pspsharp.HLE.modules.sceMeCore;
	using MMIOHandlerBase = pspsharp.memory.mmio.MMIOHandlerBase;
	using StateInputStream = pspsharp.state.StateInputStream;
	using StateOutputStream = pspsharp.state.StateOutputStream;

	public class MMIOHandlerMeBase : MMIOHandlerBase
	{
		private const int STATE_VERSION = 0;

		public MMIOHandlerMeBase(int baseAddress) : base(baseAddress)
		{

			log = sceMeCore.log;
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: @Override public void read(pspsharp.state.StateInputStream stream) throws java.io.IOException
		public override void read(StateInputStream stream)
		{
			stream.readVersion(STATE_VERSION);
			base.read(stream);
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: @Override public void write(pspsharp.state.StateOutputStream stream) throws java.io.IOException
		public override void write(StateOutputStream stream)
		{
			stream.writeVersion(STATE_VERSION);
			base.write(stream);
		}

		protected internal override Processor Processor
		{
			get
			{
				return MEProcessor.Instance;
			}
		}

		protected internal override Memory Memory
		{
			get
			{
				return MEProcessor.Instance.MEMemory;
			}
		}
	}

}