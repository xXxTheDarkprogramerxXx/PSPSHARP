﻿using System.Text;

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
namespace pspsharp.graphics.RE
{

	/// <summary>
	/// @author gid15
	/// 
	/// </summary>
	public class ShaderProgram
	{
		private int programId = -1;
		private ShaderProgramKey key;
		private int shaderAttribPosition;
		private int shaderAttribNormal;
		private int shaderAttribColor;
		private int shaderAttribTexture;
		private int shaderAttribWeights1;
		private int shaderAttribWeights2;
		private bool hasGeometryShader;
		private int[] lightType = new int[VideoEngine.NUM_LIGHTS]; // values: [0..2]
		private int[] lightKind = new int[VideoEngine.NUM_LIGHTS]; // values: [0..2]
		private int[] lightEnabled = new int[VideoEngine.NUM_LIGHTS]; // values: [0..1]
		private int[] matFlags = new int[3]; // values: [0..1]
		private int[] texShade = new int[2]; // values: [0..3]
		private int[] texEnvMode = new int[2]; // values: #0:[0..4], #1:[0..1]
		private int ctestFunc; // values: [0..3]
		private int texMapMode; // values: [0..2]
		private int texMapProj; // values: [0..3]
		private int vinfoColor; // values: [0..8]
		private int vinfoPosition; // values: [0..3]
		private int vinfoTexture; // values: [0..3]
		private int vinfoNormal; // values: [0..3]
		private float colorDoubling; // values: [1..2]
		private int texEnable; // values: [0..1]
		private int lightingEnable; // values: [0..1]
		private int vinfoTransform2D; // values: [0..1]
		private int ctestEnable; // values: [0..1]
		private int lightMode; // values: [0..1]
		private int texPixelFormat; // values: [0..14]
		private int numberBones; // values: [0..8]
		private int clutIndexHint; // values: [0..4]
		private int alphaTestEnable; // values: [0..1]
		private int alphaTestFunc; // values: [0..7]
		private int stencilTestEnable; // values: [0..1]
		private int stencilFunc; // values: [0..7]
		private int stencilOpFail; // values: [0..5]
		private int stencilOpZFail; // values: [0..5]
		private int stencilOpZPass; // values: [0..5]
		private int depthTestEnable; // values: [0..1]
		private int depthFunc; // values: [0..7]
		private int blendTestEnable; // values: [0..1]
		private int blendEquation; // values: [0..5]
		private int blendSrc; // values: [0..10];
		private int blendDst; // values: [0..10];
		private int colorMaskEnable; // values: [0..1]
		private int copyRedToAlpha; // values: [0..1]
		private int fogEnable; // values: [0..1]

		public class ShaderProgramKey
		{
			internal long key1;
			internal long key2;

			public ShaderProgramKey(long key1, long key2)
			{
				this.key1 = key1;
				this.key2 = key2;
			}

			public override int GetHashCode()
			{
				int hashCode = (int) key1;
				hashCode ^= (int)(key1 >> 32);
				hashCode ^= (int) key2;
				hashCode ^= (int)(key2 >> 32);
				return hashCode;
			}

			public virtual bool Equals(ShaderProgramKey that)
			{
				return key1 == that.key1 && key2 == that.key2;
			}

			public override bool Equals(object that)
			{
				if (that is ShaderProgramKey)
				{
					return Equals((ShaderProgramKey) that);
				}
				return base.Equals(that);
			}
		}

		public ShaderProgram()
		{
		}

		public ShaderProgram(ShaderContext shaderContext, bool hasGeometryShader)
		{
			this.hasGeometryShader = hasGeometryShader;
			for (int i = 0; i < lightType.Length; i++)
			{
				lightType[i] = shaderContext.getLightType(i);
				lightKind[i] = shaderContext.getLightKind(i);
				lightEnabled[i] = shaderContext.getLightEnabled(i);
			}
			matFlags[0] = shaderContext.getMatFlags(0);
			matFlags[1] = shaderContext.getMatFlags(1);
			matFlags[2] = shaderContext.getMatFlags(2);
			texShade[0] = shaderContext.getTexShade(0);
			texShade[1] = shaderContext.getTexShade(1);
			texEnvMode[0] = shaderContext.getTexEnvMode(0);
			texEnvMode[1] = shaderContext.getTexEnvMode(1);
			ctestFunc = shaderContext.CtestFunc;
			texMapMode = shaderContext.TexMapMode;
			texMapProj = shaderContext.TexMapProj;
			vinfoColor = shaderContext.VinfoColor;
			vinfoPosition = shaderContext.VinfoPosition;
			vinfoTexture = shaderContext.VinfoTexture;
			vinfoNormal = shaderContext.VinfoNormal;
			colorDoubling = shaderContext.ColorDoubling;
			texEnable = shaderContext.TexEnable;
			lightingEnable = shaderContext.LightingEnable;
			vinfoTransform2D = shaderContext.VinfoTransform2D;
			ctestEnable = shaderContext.CtestEnable;
			lightMode = shaderContext.LightMode;
			texPixelFormat = shaderContext.TexPixelFormat;
			numberBones = shaderContext.NumberBones;
			clutIndexHint = shaderContext.ClutIndexHint;
			alphaTestEnable = shaderContext.AlphaTestEnable;
			alphaTestFunc = shaderContext.AlphaTestFunc;
			stencilTestEnable = shaderContext.StencilTestEnable;
			stencilFunc = shaderContext.StencilFunc;
			stencilOpFail = shaderContext.StencilOpFail;
			stencilOpZFail = shaderContext.StencilOpZFail;
			stencilOpZPass = shaderContext.StencilOpZPass;
			depthTestEnable = shaderContext.DepthTestEnable;
			depthFunc = shaderContext.DepthFunc;
			blendTestEnable = shaderContext.BlendTestEnable;
			blendEquation = shaderContext.BlendEquation;
			blendSrc = shaderContext.BlendSrc;
			blendDst = shaderContext.BlendDst;
			colorMaskEnable = shaderContext.ColorMaskEnable;
			copyRedToAlpha = shaderContext.CopyRedToAlpha;
			fogEnable = shaderContext.FogEnable;

			key = getKey(shaderContext, hasGeometryShader);
		}

		public static string DummyDynamicDefines
		{
			get
			{
				StringBuilder defines = new StringBuilder();
				int dummyValue = -1;
    
				for (int i = 0; i < VideoEngine.NUM_LIGHTS; i++)
				{
					// LightType and LightKind are currently not used as defines in the shaders
					//REShader.addDefine(defines, "LIGHT_TYPE" + i, dummyValue);
					//REShader.addDefine(defines, "LIGHT_KIND" + i, dummyValue);
					REShader.addDefine(defines, "LIGHT_ENABLED" + i, dummyValue);
				}
				REShader.addDefine(defines, "MAT_FLAGS0", dummyValue);
				REShader.addDefine(defines, "MAT_FLAGS1", dummyValue);
				REShader.addDefine(defines, "MAT_FLAGS2", dummyValue);
				REShader.addDefine(defines, "TEX_SHADE0", dummyValue);
				REShader.addDefine(defines, "TEX_SHADE1", dummyValue);
				REShader.addDefine(defines, "TEX_ENV_MODE0", dummyValue);
				REShader.addDefine(defines, "TEX_ENV_MODE1", dummyValue);
				REShader.addDefine(defines, "CTEST_FUNC", dummyValue);
				REShader.addDefine(defines, "TEX_MAP_MODE", dummyValue);
				REShader.addDefine(defines, "TEX_MAP_PROJ", dummyValue);
				REShader.addDefine(defines, "VINFO_COLOR", dummyValue);
				REShader.addDefine(defines, "VINFO_POSITION", dummyValue);
				REShader.addDefine(defines, "VINFO_TEXTURE", dummyValue);
				REShader.addDefine(defines, "VINFO_NORMAL", dummyValue);
				REShader.addDefine(defines, "COLOR_DOUBLING", dummyValue);
				REShader.addDefine(defines, "TEX_ENABLE", dummyValue);
				REShader.addDefine(defines, "LIGHTING_ENABLE", dummyValue);
				REShader.addDefine(defines, "VINFO_TRANSFORM_2D", dummyValue);
				REShader.addDefine(defines, "CTEST_ENABLE", dummyValue);
				REShader.addDefine(defines, "LIGHT_MODE", dummyValue);
				REShader.addDefine(defines, "TEX_PIXEL_FORMAT", dummyValue);
				REShader.addDefine(defines, "NUMBER_BONES", dummyValue);
				REShader.addDefine(defines, "CLUT_INDEX_HINT", dummyValue);
				REShader.addDefine(defines, "ALPHA_TEST_ENABLE", dummyValue);
				REShader.addDefine(defines, "ALPHA_TEST_FUNC", dummyValue);
				REShader.addDefine(defines, "STENCIL_TEST_ENABLE", dummyValue);
				REShader.addDefine(defines, "STENCIL_FUNC", dummyValue);
				REShader.addDefine(defines, "STENCIL_OP_FAIL", dummyValue);
				REShader.addDefine(defines, "STENCIL_OP_ZFAIL", dummyValue);
				REShader.addDefine(defines, "STENCIL_OP_ZPASS", dummyValue);
				REShader.addDefine(defines, "DEPTH_TEST_ENABLE", dummyValue);
				REShader.addDefine(defines, "DEPTH_FUNC", dummyValue);
				REShader.addDefine(defines, "BLEND_TEST_ENABLE", dummyValue);
				REShader.addDefine(defines, "BLEND_EQUATION", dummyValue);
				REShader.addDefine(defines, "BLEND_SRC", dummyValue);
				REShader.addDefine(defines, "BLEND_DST", dummyValue);
				REShader.addDefine(defines, "COLOR_MASK_ENABLE", dummyValue);
				REShader.addDefine(defines, "COPY_RED_TO_ALPHA", dummyValue);
				REShader.addDefine(defines, "FOG_ENABLE", dummyValue);
    
				return defines.ToString();
			}
		}

		public virtual string DynamicDefines
		{
			get
			{
				StringBuilder defines = new StringBuilder();
    
				for (int i = 0; i < lightType.Length; i++)
				{
					// LightType and LightKind are currently not used as defines in the shaders
					//REShader.addDefine(defines, "LIGHT_TYPE" + i, lightType[i]);
					//REShader.addDefine(defines, "LIGHT_KIND" + i, lightKind[i]);
					REShader.addDefine(defines, "LIGHT_ENABLED" + i, lightEnabled[i]);
				}
				REShader.addDefine(defines, "MAT_FLAGS0", matFlags[0]);
				REShader.addDefine(defines, "MAT_FLAGS1", matFlags[1]);
				REShader.addDefine(defines, "MAT_FLAGS2", matFlags[2]);
				REShader.addDefine(defines, "TEX_SHADE0", texShade[0]);
				REShader.addDefine(defines, "TEX_SHADE1", texShade[1]);
				REShader.addDefine(defines, "TEX_ENV_MODE0", texEnvMode[0]);
				REShader.addDefine(defines, "TEX_ENV_MODE1", texEnvMode[1]);
				REShader.addDefine(defines, "CTEST_FUNC", ctestFunc);
				REShader.addDefine(defines, "TEX_MAP_MODE", texMapMode);
				REShader.addDefine(defines, "TEX_MAP_PROJ", texMapProj);
				REShader.addDefine(defines, "VINFO_COLOR", vinfoColor);
				REShader.addDefine(defines, "VINFO_POSITION", vinfoPosition);
				REShader.addDefine(defines, "VINFO_TEXTURE", vinfoTexture);
				REShader.addDefine(defines, "VINFO_NORMAL", vinfoNormal);
				REShader.addDefine(defines, "COLOR_DOUBLING", (int) colorDoubling);
				REShader.addDefine(defines, "TEX_ENABLE", texEnable);
				REShader.addDefine(defines, "LIGHTING_ENABLE", lightingEnable);
				REShader.addDefine(defines, "VINFO_TRANSFORM_2D", vinfoTransform2D);
				REShader.addDefine(defines, "CTEST_ENABLE", ctestEnable);
				REShader.addDefine(defines, "LIGHT_MODE", lightMode);
				REShader.addDefine(defines, "TEX_PIXEL_FORMAT", texPixelFormat);
				REShader.addDefine(defines, "NUMBER_BONES", numberBones);
				REShader.addDefine(defines, "CLUT_INDEX_HINT", clutIndexHint);
				REShader.addDefine(defines, "ALPHA_TEST_ENABLE", alphaTestEnable);
				REShader.addDefine(defines, "ALPHA_TEST_FUNC", alphaTestFunc);
				REShader.addDefine(defines, "STENCIL_TEST_ENABLE", stencilTestEnable);
				REShader.addDefine(defines, "STENCIL_FUNC", stencilFunc);
				REShader.addDefine(defines, "STENCIL_OP_FAIL", stencilOpFail);
				REShader.addDefine(defines, "STENCIL_OP_ZFAIL", stencilOpZFail);
				REShader.addDefine(defines, "STENCIL_OP_ZPASS", stencilOpZPass);
				REShader.addDefine(defines, "DEPTH_TEST_ENABLE", depthTestEnable);
				REShader.addDefine(defines, "DEPTH_FUNC", depthFunc);
				REShader.addDefine(defines, "BLEND_TEST_ENABLE", blendTestEnable);
				REShader.addDefine(defines, "BLEND_EQUATION", blendEquation);
				REShader.addDefine(defines, "BLEND_SRC", blendSrc);
				REShader.addDefine(defines, "BLEND_DST", blendDst);
				REShader.addDefine(defines, "COLOR_MASK_ENABLE", colorMaskEnable);
				REShader.addDefine(defines, "COPY_RED_TO_ALPHA", copyRedToAlpha);
				REShader.addDefine(defines, "FOG_ENABLE", fogEnable);
    
				return defines.ToString();
			}
		}

		public static ShaderProgramKey getKey(ShaderContext shaderContext, bool hasGeometryShader)
		{
			long key = 0;
			long key1;
			long key2;
			int shift = 0;

			key += hasGeometryShader ? 1 : 0;
			shift++;
			for (int i = 0; i < VideoEngine.NUM_LIGHTS; i++)
			{
				// LightType and LightKind are currently not used as defines in the shaders
				//key += shaderContext.getLightType(i) << shift;
				//shift += 2;
				//key += shaderContext.getLightKind(i) << shift;
				//shift += 2;
				key += shaderContext.getLightEnabled(i) << shift;
				shift++;
			}
			key += ((long) shaderContext.getMatFlags(0)) << shift;
			shift++;
			key += ((long) shaderContext.getMatFlags(1)) << shift;
			shift++;
			key += ((long) shaderContext.getMatFlags(2)) << shift;
			shift++;
			key += ((long) shaderContext.getTexShade(0)) << shift;
			shift += 2;
			key += ((long) shaderContext.getTexShade(1)) << shift;
			shift += 2;
			key += ((long) shaderContext.getTexEnvMode(0)) << shift;
			shift += 3;
			key += ((long) shaderContext.getTexEnvMode(1)) << shift;
			shift++;
			key += ((long) shaderContext.CtestFunc) << shift;
			shift += 2;
			key += ((long) shaderContext.TexMapMode) << shift;
			shift += 2;
			key += ((long) shaderContext.TexMapProj) << shift;
			shift += 2;
			key += ((long) shaderContext.VinfoColor) << shift;
			shift += 4;
			key += ((long) shaderContext.VinfoPosition) << shift;
			shift += 2;
			key += ((long) shaderContext.VinfoTexture) << shift;
			shift += 2;
			key += ((long) shaderContext.VinfoNormal) << shift;
			shift += 2;
			key += (shaderContext.ColorDoubling == 2.0f ? 1L : 0L) << shift;
			shift++;
			key += ((long) shaderContext.TexEnable) << shift;
			shift++;
			key += ((long) shaderContext.LightingEnable) << shift;
			shift++;
			key += ((long) shaderContext.VinfoTransform2D) << shift;
			shift++;
			key += ((long) shaderContext.CtestEnable) << shift;
			shift++;
			key += ((long) shaderContext.LightMode) << shift;
			shift++;
			key += ((long) shaderContext.TexPixelFormat) << shift;
			shift += 4;
			key += ((long) shaderContext.NumberBones) << shift;
			shift += 4;
			key += ((long) shaderContext.ClutIndexHint) << shift;
			shift += 3;
			key += ((long) shaderContext.AlphaTestEnable) << shift;
			shift++;
			key += ((long) shaderContext.AlphaTestFunc) << shift;
			shift += 3;
			key += ((long) shaderContext.StencilTestEnable) << shift;
			shift++;
			key += ((long) shaderContext.StencilFunc) << shift;
			shift += 3;
			key += ((long) shaderContext.StencilOpFail) << shift;
			shift += 3;
			key += ((long) shaderContext.StencilOpZFail) << shift;
			shift += 3;

			if (shift > (sizeof(long) * 8))
			{
				VideoEngine.log_Renamed.error(string.Format("ShaderProgram: too long key1: {0:D} bits", shift));
			}
			key1 = key;
			key = 0;
			shift = 0;

			key += ((long) shaderContext.StencilOpZPass) << shift;
			shift += 3;
			key += ((long) shaderContext.DepthTestEnable) << shift;
			shift += 3;
			key += ((long) shaderContext.DepthFunc) << shift;
			shift += 3;
			key += ((long) shaderContext.BlendTestEnable) << shift;
			shift++;
			key += ((long) shaderContext.BlendEquation) << shift;
			shift += 3;
			key += ((long) shaderContext.BlendSrc) << shift;
			shift += 4;
			key += ((long) shaderContext.BlendDst) << shift;
			shift += 4;
			key += ((long) shaderContext.ColorMaskEnable) << shift;
			shift++;
			key += ((long) shaderContext.CopyRedToAlpha) << shift;
			shift++;
			key += ((long) shaderContext.FogEnable) << shift;
			shift++;

			if (shift > (sizeof(long) * 8))
			{
				VideoEngine.log_Renamed.error(string.Format("ShaderProgram: too long key2: {0:D} bits", shift));
			}
			key2 = key;

			return new ShaderProgramKey(key1, key2);
		}

		public virtual bool matches(ShaderContext shaderContext, bool hasGeometryShader)
		{
			ShaderProgramKey key = getKey(shaderContext, hasGeometryShader);

			return key.Equals(this.key);
		}

		public virtual void use(IRenderingEngine re)
		{
			re.useProgram(programId);
		}

		public virtual int ProgramId
		{
			get
			{
				return programId;
			}
		}

		public virtual void setProgramId(IRenderingEngine re, int programId)
		{
			this.programId = programId;

			shaderAttribWeights1 = re.getAttribLocation(programId, REShader.attributeNameWeights1);
			shaderAttribWeights2 = re.getAttribLocation(programId, REShader.attributeNameWeights2);
			shaderAttribPosition = re.getAttribLocation(programId, REShader.attributeNamePosition);
			shaderAttribNormal = re.getAttribLocation(programId, REShader.attributeNameNormal);
			shaderAttribColor = re.getAttribLocation(programId, REShader.attributeNameColor);
			shaderAttribTexture = re.getAttribLocation(programId, REShader.attributeNameTexture);
		}

		public virtual int ShaderAttribPosition
		{
			get
			{
				return shaderAttribPosition;
			}
		}

		public virtual int ShaderAttribNormal
		{
			get
			{
				return shaderAttribNormal;
			}
		}

		public virtual int ShaderAttribColor
		{
			get
			{
				return shaderAttribColor;
			}
		}

		public virtual int ShaderAttribTexture
		{
			get
			{
				return shaderAttribTexture;
			}
		}

		public virtual int ShaderAttribWeights1
		{
			get
			{
				return shaderAttribWeights1;
			}
		}

		public virtual int ShaderAttribWeights2
		{
			get
			{
				return shaderAttribWeights2;
			}
		}

		public virtual ShaderProgramKey Key
		{
			get
			{
				return key;
			}
		}

		public override string ToString()
		{
//JAVA TO C# CONVERTER TODO TASK: The following line has a Java format specifier which cannot be directly translated to .NET:
//ORIGINAL LINE: return String.format("ShaderProgram[%d, geometryShader=%b, %s]", programId, hasGeometryShader, getDynamicDefines().replace(System.getProperty("line.separator"), ", "));
			return string.Format("ShaderProgram[%d, geometryShader=%b, %s]", programId, hasGeometryShader, DynamicDefines.Replace(System.getProperty("line.separator"), ", "));
		}
	}
}