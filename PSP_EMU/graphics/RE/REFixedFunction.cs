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
namespace pspsharp.graphics.RE
{
	using Settings = pspsharp.settings.Settings;

	/// <summary>
	/// @author gid15
	/// 
	/// This RenderingEngine class : the required logic
	/// to use the OpenGL fixed-function pipeline, i.e. without shaders.
	/// 
	/// This class is implemented as a Proxy, forwarding the non-relevant calls
	/// to the proxy.
	/// 
	/// When this class splits one call into multiple calls, they are sent to the
	/// complete RenderingEngine pipeline, not just the proxy, taking advantage
	/// of other RenderingEngines up in the pipeline (e.g. the proxy class removing
	/// redundant calls).
	/// </summary>
	public class REFixedFunction : BaseRenderingEngineFunction
	{
		private ShaderProgram stencilShaderProgram;

		public REFixedFunction(IRenderingEngine proxy) : base(proxy)
		{
		}

		public override void setTextureFunc(int func, bool alphaUsed, bool colorDoubled)
		{
			if (colorDoubled || !alphaUsed)
			{
				// GL_RGB_SCALE is only used in OpenGL when GL_TEXTURE_ENV_MODE is GL_COMBINE
				// See http://www.opengl.org/sdk/docs/man/xhtml/glTexEnv.xml
				switch (func)
				{
					case GeCommands.TFUNC_FRAGMENT_DOUBLE_TEXTURE_EFECT_MODULATE:
						// Cv = Cp * Cs
						// Av = Ap * As
						func = IRenderingEngine_Fields.RE_TEXENV_COMBINE;
						re.setTexEnv(IRenderingEngine_Fields.RE_TEXENV_COMBINE_RGB, IRenderingEngine_Fields.RE_TEXENV_MODULATE);
						re.setTexEnv(IRenderingEngine_Fields.RE_TEXENV_SRC0_RGB, IRenderingEngine_Fields.RE_TEXENV_TEXTURE);
						re.setTexEnv(IRenderingEngine_Fields.RE_TEXENV_OPERAND0_RGB, IRenderingEngine_Fields.RE_TEXENV_SRC_COLOR);
						re.setTexEnv(IRenderingEngine_Fields.RE_TEXENV_SRC1_RGB, IRenderingEngine_Fields.RE_TEXENV_PREVIOUS);
						re.setTexEnv(IRenderingEngine_Fields.RE_TEXENV_OPERAND1_RGB, IRenderingEngine_Fields.RE_TEXENV_SRC_COLOR);

						if (alphaUsed)
						{
							re.setTexEnv(IRenderingEngine_Fields.RE_TEXENV_COMBINE_ALPHA, IRenderingEngine_Fields.RE_TEXENV_MODULATE);
							re.setTexEnv(IRenderingEngine_Fields.RE_TEXENV_SRC0_ALPHA, IRenderingEngine_Fields.RE_TEXENV_TEXTURE);
							re.setTexEnv(IRenderingEngine_Fields.RE_TEXENV_OPERAND0_ALPHA, IRenderingEngine_Fields.RE_TEXENV_SRC_ALPHA);
							re.setTexEnv(IRenderingEngine_Fields.RE_TEXENV_SRC1_ALPHA, IRenderingEngine_Fields.RE_TEXENV_PREVIOUS);
							re.setTexEnv(IRenderingEngine_Fields.RE_TEXENV_OPERAND1_ALPHA, IRenderingEngine_Fields.RE_TEXENV_SRC_ALPHA);
						}
						else
						{
							re.setTexEnv(IRenderingEngine_Fields.RE_TEXENV_COMBINE_ALPHA, IRenderingEngine_Fields.RE_TEXENV_REPLACE);
							re.setTexEnv(IRenderingEngine_Fields.RE_TEXENV_SRC0_ALPHA, IRenderingEngine_Fields.RE_TEXENV_PREVIOUS);
							re.setTexEnv(IRenderingEngine_Fields.RE_TEXENV_OPERAND0_ALPHA, IRenderingEngine_Fields.RE_TEXENV_SRC_ALPHA);
						}
						break;
					case GeCommands.TFUNC_FRAGMENT_DOUBLE_TEXTURE_EFECT_DECAL:
						func = IRenderingEngine_Fields.RE_TEXENV_COMBINE;
						// Cv = Cs * As + Cp * (1 - As)
						// Av = Ap
						if (!alphaUsed)
						{
							// DECAL mode with ignored Alpha is always using
							// the equivalent of Alpha = 1.0 on PSP.
							// Simplified version when As == 1:
							// Cv = Cs
							re.setTexEnv(IRenderingEngine_Fields.RE_TEXENV_COMBINE_RGB, IRenderingEngine_Fields.RE_TEXENV_REPLACE);
							re.setTexEnv(IRenderingEngine_Fields.RE_TEXENV_SRC0_RGB, IRenderingEngine_Fields.RE_TEXENV_TEXTURE);
							re.setTexEnv(IRenderingEngine_Fields.RE_TEXENV_OPERAND0_RGB, IRenderingEngine_Fields.RE_TEXENV_SRC_COLOR);
						}
						else
						{
							re.setTexEnv(IRenderingEngine_Fields.RE_TEXENV_COMBINE_RGB, IRenderingEngine_Fields.RE_TEXENV_INTERPOLATE);
							re.setTexEnv(IRenderingEngine_Fields.RE_TEXENV_SRC0_RGB, IRenderingEngine_Fields.RE_TEXENV_TEXTURE);
							re.setTexEnv(IRenderingEngine_Fields.RE_TEXENV_OPERAND0_RGB, IRenderingEngine_Fields.RE_TEXENV_SRC_COLOR);
							re.setTexEnv(IRenderingEngine_Fields.RE_TEXENV_SRC1_RGB, IRenderingEngine_Fields.RE_TEXENV_PREVIOUS);
							re.setTexEnv(IRenderingEngine_Fields.RE_TEXENV_OPERAND1_RGB, IRenderingEngine_Fields.RE_TEXENV_SRC_COLOR);
							re.setTexEnv(IRenderingEngine_Fields.RE_TEXENV_SRC2_RGB, IRenderingEngine_Fields.RE_TEXENV_TEXTURE);
							re.setTexEnv(IRenderingEngine_Fields.RE_TEXENV_OPERAND2_RGB, IRenderingEngine_Fields.RE_TEXENV_SRC_ALPHA);
						}

						re.setTexEnv(IRenderingEngine_Fields.RE_TEXENV_COMBINE_ALPHA, IRenderingEngine_Fields.RE_TEXENV_REPLACE);
						re.setTexEnv(IRenderingEngine_Fields.RE_TEXENV_SRC0_ALPHA, IRenderingEngine_Fields.RE_TEXENV_PREVIOUS);
						re.setTexEnv(IRenderingEngine_Fields.RE_TEXENV_OPERAND0_ALPHA, IRenderingEngine_Fields.RE_TEXENV_SRC_ALPHA);
						break;
					case GeCommands.TFUNC_FRAGMENT_DOUBLE_TEXTURE_EFECT_BLEND:
						// Cv = Cc * Cs + Cp * (1 - Cs)
						// Av = As * Ap
						func = IRenderingEngine_Fields.RE_TEXENV_COMBINE;
						re.setTexEnv(IRenderingEngine_Fields.RE_TEXENV_COMBINE_RGB, IRenderingEngine_Fields.RE_TEXENV_INTERPOLATE);
						re.setTexEnv(IRenderingEngine_Fields.RE_TEXENV_SRC0_RGB, IRenderingEngine_Fields.RE_TEXENV_CONSTANT);
						re.setTexEnv(IRenderingEngine_Fields.RE_TEXENV_OPERAND0_RGB, IRenderingEngine_Fields.RE_TEXENV_SRC_COLOR);
						re.setTexEnv(IRenderingEngine_Fields.RE_TEXENV_SRC1_RGB, IRenderingEngine_Fields.RE_TEXENV_PREVIOUS);
						re.setTexEnv(IRenderingEngine_Fields.RE_TEXENV_OPERAND1_RGB, IRenderingEngine_Fields.RE_TEXENV_SRC_COLOR);
						re.setTexEnv(IRenderingEngine_Fields.RE_TEXENV_SRC2_RGB, IRenderingEngine_Fields.RE_TEXENV_TEXTURE);
						re.setTexEnv(IRenderingEngine_Fields.RE_TEXENV_OPERAND2_RGB, IRenderingEngine_Fields.RE_TEXENV_SRC_COLOR);

						if (alphaUsed)
						{
							re.setTexEnv(IRenderingEngine_Fields.RE_TEXENV_COMBINE_ALPHA, IRenderingEngine_Fields.RE_TEXENV_MODULATE);
							re.setTexEnv(IRenderingEngine_Fields.RE_TEXENV_SRC0_ALPHA, IRenderingEngine_Fields.RE_TEXENV_TEXTURE);
							re.setTexEnv(IRenderingEngine_Fields.RE_TEXENV_OPERAND0_ALPHA, IRenderingEngine_Fields.RE_TEXENV_SRC_ALPHA);
							re.setTexEnv(IRenderingEngine_Fields.RE_TEXENV_SRC1_ALPHA, IRenderingEngine_Fields.RE_TEXENV_PREVIOUS);
							re.setTexEnv(IRenderingEngine_Fields.RE_TEXENV_OPERAND1_ALPHA, IRenderingEngine_Fields.RE_TEXENV_SRC_ALPHA);
						}
						else
						{
							re.setTexEnv(IRenderingEngine_Fields.RE_TEXENV_COMBINE_ALPHA, IRenderingEngine_Fields.RE_TEXENV_REPLACE);
							re.setTexEnv(IRenderingEngine_Fields.RE_TEXENV_SRC0_ALPHA, IRenderingEngine_Fields.RE_TEXENV_PREVIOUS);
							re.setTexEnv(IRenderingEngine_Fields.RE_TEXENV_OPERAND0_ALPHA, IRenderingEngine_Fields.RE_TEXENV_SRC_ALPHA);
						}
						break;
					case GeCommands.TFUNC_FRAGMENT_DOUBLE_TEXTURE_EFECT_REPLACE:
						// Cv = Cs
						// Av = As
						func = IRenderingEngine_Fields.RE_TEXENV_COMBINE;
						re.setTexEnv(IRenderingEngine_Fields.RE_TEXENV_COMBINE_RGB, IRenderingEngine_Fields.RE_TEXENV_REPLACE);
						re.setTexEnv(IRenderingEngine_Fields.RE_TEXENV_SRC0_RGB, IRenderingEngine_Fields.RE_TEXENV_TEXTURE);
						re.setTexEnv(IRenderingEngine_Fields.RE_TEXENV_OPERAND0_RGB, IRenderingEngine_Fields.RE_TEXENV_SRC_COLOR);

						if (alphaUsed)
						{
							re.setTexEnv(IRenderingEngine_Fields.RE_TEXENV_COMBINE_ALPHA, IRenderingEngine_Fields.RE_TEXENV_REPLACE);
							re.setTexEnv(IRenderingEngine_Fields.RE_TEXENV_SRC0_ALPHA, IRenderingEngine_Fields.RE_TEXENV_TEXTURE);
							re.setTexEnv(IRenderingEngine_Fields.RE_TEXENV_OPERAND0_ALPHA, IRenderingEngine_Fields.RE_TEXENV_SRC_ALPHA);
						}
						else
						{
							re.setTexEnv(IRenderingEngine_Fields.RE_TEXENV_COMBINE_ALPHA, IRenderingEngine_Fields.RE_TEXENV_REPLACE);
							re.setTexEnv(IRenderingEngine_Fields.RE_TEXENV_SRC0_ALPHA, IRenderingEngine_Fields.RE_TEXENV_PREVIOUS);
							re.setTexEnv(IRenderingEngine_Fields.RE_TEXENV_OPERAND0_ALPHA, IRenderingEngine_Fields.RE_TEXENV_SRC_ALPHA);
						}
						break;
					case GeCommands.TFUNC_FRAGMENT_DOUBLE_TEXTURE_EFECT_ADD:
						// Cv = Cp + Cs
						// Av = Ap * As
						func = IRenderingEngine_Fields.RE_TEXENV_COMBINE;
						re.setTexEnv(IRenderingEngine_Fields.RE_TEXENV_COMBINE_RGB, IRenderingEngine_Fields.RE_TEXENV_ADD);
						re.setTexEnv(IRenderingEngine_Fields.RE_TEXENV_SRC0_RGB, IRenderingEngine_Fields.RE_TEXENV_TEXTURE);
						re.setTexEnv(IRenderingEngine_Fields.RE_TEXENV_OPERAND0_RGB, IRenderingEngine_Fields.RE_TEXENV_SRC_COLOR);
						re.setTexEnv(IRenderingEngine_Fields.RE_TEXENV_SRC1_RGB, IRenderingEngine_Fields.RE_TEXENV_PREVIOUS);
						re.setTexEnv(IRenderingEngine_Fields.RE_TEXENV_OPERAND1_RGB, IRenderingEngine_Fields.RE_TEXENV_SRC_COLOR);

						if (alphaUsed)
						{
							re.setTexEnv(IRenderingEngine_Fields.RE_TEXENV_COMBINE_ALPHA, IRenderingEngine_Fields.RE_TEXENV_MODULATE);
							re.setTexEnv(IRenderingEngine_Fields.RE_TEXENV_SRC0_ALPHA, IRenderingEngine_Fields.RE_TEXENV_TEXTURE);
							re.setTexEnv(IRenderingEngine_Fields.RE_TEXENV_OPERAND0_ALPHA, IRenderingEngine_Fields.RE_TEXENV_SRC_ALPHA);
							re.setTexEnv(IRenderingEngine_Fields.RE_TEXENV_SRC1_ALPHA, IRenderingEngine_Fields.RE_TEXENV_PREVIOUS);
							re.setTexEnv(IRenderingEngine_Fields.RE_TEXENV_OPERAND1_ALPHA, IRenderingEngine_Fields.RE_TEXENV_SRC_ALPHA);
						}
						else
						{
							re.setTexEnv(IRenderingEngine_Fields.RE_TEXENV_COMBINE_ALPHA, IRenderingEngine_Fields.RE_TEXENV_REPLACE);
							re.setTexEnv(IRenderingEngine_Fields.RE_TEXENV_SRC0_ALPHA, IRenderingEngine_Fields.RE_TEXENV_PREVIOUS);
							re.setTexEnv(IRenderingEngine_Fields.RE_TEXENV_OPERAND0_ALPHA, IRenderingEngine_Fields.RE_TEXENV_SRC_ALPHA);
						}
						break;
				}
			}

			base.setTextureFunc(func, alphaUsed, colorDoubled);
		}

		public override void disableFlag(int flag)
		{
			if (canUpdateFlag(flag))
			{
				base.disableFlag(flag);
			}
		}

		public override void enableFlag(int flag)
		{
			if (canUpdateFlag(flag))
			{
				base.enableFlag(flag);
			}
		}

		public override void enableVertexAttribArray(int id)
		{
			// This call is used only by Shader
		}

		public override void disableVertexAttribArray(int id)
		{
			// This call is used only by Shader
		}

		public override bool canNativeClut(int textureAddress, int pixelFormat, bool textureSwizzle)
		{
			// Shaders are required for native clut
			return false;
		}

		public override bool setCopyRedToAlpha(bool copyRedToAlpha)
		{
			if (copyRedToAlpha)
			{
				// The stencil index is now available in the red channel of the stencil texture.
				// We need to copy it to the alpha channel of the GE texture.
				// As I've not found how to perform this copy from one channel into another channel
				// using the OpenGL fixed pipeline functionality,
				// we use a small fragment shader program.
				if (stencilShaderProgram == null)
				{
					if (!re.ShaderAvailable)
					{
						System.Console.WriteLine("Shaders are not available on your computer. They are required to save stencil information into the GE texture. Saving of the stencil information has been disabled.");
						return false;
					}

					// The fragment shader is just copying the stencil texture red channel to the GE texture alpha channel
					string fragmentShaderSource = "uniform sampler2D tex;" +
							"void main() {" +
							"    gl_FragColor.a = texture2DProj(tex, gl_TexCoord[0].xyz).r;" +
							"}";
					int shaderId = re.createShader(IRenderingEngine_Fields.RE_FRAGMENT_SHADER);
					bool compiled = re.compilerShader(shaderId, fragmentShaderSource);
					if (!compiled)
					{
						System.Console.WriteLine(string.Format("Cannot compile shader required for storing stencil information into the GE texture: {0}", re.getShaderInfoLog(shaderId)));
						return false;
					}

					int stencilShaderProgramId = re.createProgram();
					re.attachShader(stencilShaderProgramId, shaderId);
					bool linked = re.linkProgram(stencilShaderProgramId);
					if (!linked)
					{
						System.Console.WriteLine(string.Format("Cannot link shader required for storing stencil information into the GE texture: {0}", re.getProgramInfoLog(stencilShaderProgramId)));
						return false;
					}

					Uniforms.tex.allocateId(re, stencilShaderProgramId);

					stencilShaderProgram = new ShaderProgram();
					stencilShaderProgram.setProgramId(re, stencilShaderProgramId);

					if (!Settings.Instance.readBool("emu.useshaders"))
					{
						System.Console.WriteLine("Shaders are disabled in the pspsharp video settings. However a small shader program is required to implement the saving of the Stencil information into the GE texture. This small shader program will still be used even though the shaders are disabled in the settings. This was just for your information, you do not need to take special actions.");
					}
				}

				stencilShaderProgram.use(re);
				re.setUniform(Uniforms.tex.getId(stencilShaderProgram.ProgramId), REShader.ACTIVE_TEXTURE_NORMAL);
				re.checkAndLogErrors("setUniform");
			}
			else
			{
				// Disable the shader program
				re.useProgram(0);
			}

			return base.setCopyRedToAlpha(copyRedToAlpha);
		}

		public override void setFogDist(float end, float scale)
		{
			if (end != 0f && scale != 0f)
			{
				float glEnd = end;
				float glStart = end - (1f / scale);
				if (scale < 0f)
				{
					// OpenGL need positive values for glStart & glEnd
					glEnd = -glEnd;
					glStart = -glStart;
				}
				base.setFogDist(glStart, glEnd);
			}
		}

		public override bool canDiscardVertices()
		{
			// OpenGL fixed-function pipeline cannot discard vertices
			return false;
		}
	}

}