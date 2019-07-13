﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

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
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static Math.max;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static Math.min;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static pspsharp.HLE.modules.sceGe_user.PSP_GE_LIST_CANCEL_DONE;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static pspsharp.HLE.modules.sceGe_user.PSP_GE_LIST_DONE;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static pspsharp.HLE.modules.sceGe_user.PSP_GE_LIST_DRAWING;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static pspsharp.HLE.modules.sceGe_user.PSP_GE_LIST_END_REACHED;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static pspsharp.HLE.modules.sceGe_user.PSP_GE_LIST_STALL_REACHED;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static pspsharp.HLE.modules.sceGe_user.PSP_GE_MATRIX_BONE0;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static pspsharp.HLE.modules.sceGe_user.PSP_GE_MATRIX_BONE1;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static pspsharp.HLE.modules.sceGe_user.PSP_GE_MATRIX_BONE2;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static pspsharp.HLE.modules.sceGe_user.PSP_GE_MATRIX_BONE3;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static pspsharp.HLE.modules.sceGe_user.PSP_GE_MATRIX_BONE4;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static pspsharp.HLE.modules.sceGe_user.PSP_GE_MATRIX_BONE5;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static pspsharp.HLE.modules.sceGe_user.PSP_GE_MATRIX_BONE6;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static pspsharp.HLE.modules.sceGe_user.PSP_GE_MATRIX_BONE7;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static pspsharp.HLE.modules.sceGe_user.PSP_GE_MATRIX_PROJECTION;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static pspsharp.HLE.modules.sceGe_user.PSP_GE_MATRIX_TEXGEN;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static pspsharp.HLE.modules.sceGe_user.PSP_GE_MATRIX_VIEW;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static pspsharp.HLE.modules.sceGe_user.PSP_GE_MATRIX_WORLD;
	using static pspsharp.graphics.GeCommands;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static pspsharp.util.Utilities.matrixMult;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static pspsharp.util.Utilities.Round4;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static pspsharp.util.Utilities.vectorMult;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static pspsharp.util.Utilities.vectorMult44;


	using RuntimeContext = pspsharp.Allegrex.compiler.RuntimeContext;
	using Modules = pspsharp.HLE.Modules;
	using IAction = pspsharp.HLE.kernel.types.IAction;
	using PspGeList = pspsharp.HLE.kernel.types.PspGeList;
	using sceDisplay = pspsharp.HLE.modules.sceDisplay;
	using sceGe_user = pspsharp.HLE.modules.sceGe_user;
	using EnableDisableFlag = pspsharp.graphics.GeContext.EnableDisableFlag;
	using IRenderingEngine = pspsharp.graphics.RE.IRenderingEngine;
	using IREBufferManager = pspsharp.graphics.RE.buffer.IREBufferManager;
	using ExternalGE = pspsharp.graphics.RE.externalge.ExternalGE;
	using PixelColor = pspsharp.graphics.RE.software.PixelColor;
	using CaptureImage = pspsharp.graphics.capture.CaptureImage;
	using CaptureManager = pspsharp.graphics.capture.CaptureManager;
	using IGraphicsExporter = pspsharp.graphics.export.IGraphicsExporter;
	using WavefrontExporter = pspsharp.graphics.export.WavefrontExporter;
	using GETexture = pspsharp.graphics.textures.GETexture;
	using GETextureManager = pspsharp.graphics.textures.GETextureManager;
	using Texture = pspsharp.graphics.textures.Texture;
	using TextureCache = pspsharp.graphics.textures.TextureCache;
	using Screen = pspsharp.hardware.Screen;
	using IMemoryReader = pspsharp.memory.IMemoryReader;
	using ImageReader = pspsharp.memory.ImageReader;
	using MemoryReader = pspsharp.memory.MemoryReader;
	using AbstractBoolSettingsListener = pspsharp.settings.AbstractBoolSettingsListener;
	using Settings = pspsharp.settings.Settings;
	using CpuDurationStatistics = pspsharp.util.CpuDurationStatistics;
	using DurationStatistics = pspsharp.util.DurationStatistics;
	using Utilities = pspsharp.util.Utilities;

	using Level = org.apache.log4j.Level;
	//using Logger = org.apache.log4j.Logger;

	//
	// Ideas for Optimization:
	// - compile GE lists (or part of it) into OpenGL display list (glNewList/glCallList).
	//   For example, immutable subroutines called using CALL could be compiled into a display list.
	//   A first run of the game using a profiler option could be used to detect which parts
	//   are immutable. This information could be stored in a file for subsequent runs and
	//   used as hints for the next runs.
	// - Unswizzle textures in shader (is this possible?)
	//
	public class VideoEngine
	{

		public const int NUM_LIGHTS = 4;
		public static readonly int SIZEOF_FLOAT = pspsharp.graphics.RE.IRenderingEngine_Fields.sizeOfType[pspsharp.graphics.RE.IRenderingEngine_Fields.RE_FLOAT];
		public static readonly string[] psm_names = new string[]{"PSM_5650", "PSM_5551", "PSM_4444", "PSM_8888", "PSM_4BIT_INDEXED", "PSM_8BIT_INDEXED", "PSM_16BIT_INDEXED", "PSM_32BIT_INDEXED", "PSM_DXT1", "PSM_DXT3", "PSM_DXT5", "RE_PIXEL_STORAGE_16BIT_INDEXED_BGR5650", "RE_PIXEL_STORAGE_16BIT_INDEXED_ABGR5551", "RE_PIXEL_STORAGE_16BIT_INDEXED_ABGR4444", "RE_PIXEL_STORAGE_32BIT_INDEXED_ABGR8888", "RE_DEPTH_COMPONENT", "RE_STENCIL_INDEX", "RE_DEPTH_STENCIL"};
		public static readonly string[] logical_ops_names = new string[]{"LOP_CLEAR", "LOP_AND", "LOP_REVERSE_AND", "LOP_COPY", "LOP_INVERTED_AND", "LOP_NO_OPERATION", "LOP_EXLUSIVE_OR", "LOP_OR", "LOP_NEGATED_OR", "LOP_EQUIVALENCE", "LOP_INVERTED", "LOP_REVERSE_OR", "LOP_INVERTED_COPY", "LOP_INVERTED_OR", "LOP_NEGATED_AND", "LOP_SET"};
		private static readonly int[] textureByteAlignmentMapping = new int[] {2, 2, 2, 4};
		private static readonly int[] minimumNumberOfVertex = new int[] {1, 2, 2, 3, 3, 3, 2};
		private static readonly int[] ditherMatrixValueMapping = new int[] {0, 1, 2, 3, 4, 5, 6, 7, -8, -7, -6, -5, -4, -3, -2, -1};
		private static readonly int[] indexTypes = new int[]{0, pspsharp.graphics.RE.IRenderingEngine_Fields.RE_UNSIGNED_BYTE, pspsharp.graphics.RE.IRenderingEngine_Fields.RE_UNSIGNED_SHORT, pspsharp.graphics.RE.IRenderingEngine_Fields.RE_UNSIGNED_INT};
		private static VideoEngine instance;
		private sceDisplay display;
		private IRenderingEngine re;
		private GeContext context;
		private IREBufferManager bufferManager;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		public static Logger log_Renamed = Logger.getLogger("ge");
		public const bool useTextureCache = true;
		private bool useVertexCache = false;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		private bool useAsyncVertexCache_Renamed = true;
		public bool useOptimisticVertexCache = false;
		private bool useTextureAnisotropicFilter = false;
		private bool usexBRZFilter = false;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		private bool disableOptimizedVertexInfoReading_Renamed = false;
		private bool avoidDrawElementsWithNonZeroIndexOffset = false;
		private bool enableTextureModding = true;
		private static GeCommands helper;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		private int command_Renamed;
		private int normalArgument;
		private int waitForSyncCount;
		private VertexInfoReader vertexInfoReader = new VertexInfoReader();
		private DurationStatistics statistics = new CpuDurationStatistics("VideoEngine Statistics");
		private DurationStatistics vertexStatistics = new CpuDurationStatistics("Vertex");
		private DurationStatistics vertexReadingStatistics = new CpuDurationStatistics("Vertex Reading");
		private DurationStatistics drawArraysStatistics = new CpuDurationStatistics("glDrawArrays");
		private DurationStatistics waitSignalStatistics = new DurationStatistics("Wait for GE Signal completion");
		private DurationStatistics waitStallStatistics = new DurationStatistics("Wait on stall");
		private DurationStatistics textureCacheLookupStatistics = new CpuDurationStatistics("Lookup in TextureCache");
		private DurationStatistics vertexCacheLookupStatistics = new CpuDurationStatistics("Lookup in VertexCache");
		private DurationStatistics[] commandStatistics;
		private int errorCount;
		private const int maxErrorCount = 5; // Abort list processing when detecting more errors
		private bool isLogTraceEnabled;
		private bool isLogDebugEnabled;
		private bool isLogInfoEnabled;
		private bool isLogWarnEnabled;
		private bool isGeProfilerEnabled;
		private int primCount;
		private int nopCount;
		private long listCount;
		private bool viewportChanged;
		public MatrixUpload projectionMatrixUpload;
		public MatrixUpload modelMatrixUpload;
		public MatrixUpload viewMatrixUpload;
		public MatrixUpload textureMatrixUpload;
		private int boneMatrixIndex;
		private int boneMatrixLinearUpdatedMatrix; // number of updated matrix
		private static readonly float[] blackColor = new float[]{0, 0, 0, 0};
		private bool lightingChanged;
		private bool materialChanged;
		private bool textureChanged;
		private int[] patch_prim_types = new int[] {PRIM_TRIANGLE_STRIPS, PRIM_LINES_STRIPS, PRIM_POINT};
		private bool clutIsDirty;
		private bool usingTRXKICK;
		private int maxSpriteHeight;
		private int maxSpriteWidth;
		private bool depthChanged;
		private bool scissorChanged;
		// opengl needed information/buffers
		private int textureId = -1;
		private bool textureFlipped;
		private float textureFlipTranslateY;
		private int[] tmp_texture_buffer32;
		private short[] tmp_texture_buffer16;
		private int[] clut_buffer32 = new int[4096];
		private short[] clut_buffer16 = new short[4096];
		private bool listHasEnded;
		private PspGeList currentList; // The currently executing list
		private const int drawBufferSizeInBytes = 2048 * 1024;
		private const int indexDrawBufferSizeInBytes = 512 * 1024;
		private int bufferId;
		private int nativeBufferId;
		private int indexBufferId;
		private ByteBuffer indexByteBuffer;
		private float[] floatBufferArray;
		private IList<int> buffersToBeDeleted = new LinkedList<int>();
		internal float[][] bboxVertices;
		private readonly LinkedList<PspGeList> drawListQueue = new LinkedList<PspGeList>();
		private bool somethingDisplayed;
		private bool forceLoadGEToScreen;
		private bool geBufChanged;
		private bool fbBufChanged;
		private IAction hleAction;
		private int[] currentCMDValues;
		private int[] currentListCMDValues;
		private int previousPrim;
		private LinkedList<AddressRange> videoTextures;
		private IntBuffer multiDrawFirst;
		private IntBuffer multiDrawCount;
		private const int maxMultiDrawElements = 1000;
		private int multiTrxkickStart;
		private int multiTrxkickEnd;
		private bool multiTrxkickCopyGeToMemoryDone;
		private bool multiTrxkickInvalidateGe;
		private const string name = "VideoEngine";
		private int maxWaitForSyncCount;
		private VertexState v = new VertexState();
		private VertexState v1 = new VertexState();
		private VertexState v2 = new VertexState();
		private bool isBoundingBox;
		private bool skipThisFrame;
		// It is not safe in every application to simply skip the whole list.
		// This could be a compatibility option.
		private bool skipListWhenSkippingFrame = false;
		private bool export3D;
		private bool export3DOnlyVisible = true;
		private string export3DDirectory;
		private IGraphicsExporter exporter;
		private bool hasModdedTextureDirectory;
		private Dictionary<int, int[]> cachedInstructions;
		private long listStartMicroTime;
		private bool wantClearTextureCache;
		private bool wantClearVertexCache;
		// The PSP can handle textures of maximum size 512x512.
		// The PS3 PSP emulator can handle larger textures (for HD Remasters).
		private int maxTextureSizeLog2 = 9;
		private bool doubleTexture2DCoords = false;

		public class MatrixUpload
		{

			internal readonly float[] matrix;
			internal bool changed;
			internal int[] matrixIndex;
			internal int index;
			internal int maxIndex;

			public MatrixUpload(float[] matrix, int matrixWidth, int matrixHeight)
			{
				changed = true;
				this.matrix = matrix;

				for (int y = 0; y < 4; y++)
				{
					for (int x = 0; x < 4; x++)
					{
						matrix[y * 4 + x] = (x == y ? 1 : 0);
					}
				}

				maxIndex = matrixWidth * matrixHeight;
				matrixIndex = new int[maxIndex];
				for (int i = 0; i < maxIndex; i++)
				{
					matrixIndex[i] = (i % matrixWidth) + (i / matrixWidth) * 4;
				}
			}

			public virtual void startUpload(int startIndex)
			{
				index = startIndex;
			}

			public bool uploadValue(float value)
			{
				if (index >= maxIndex)
				{
					if (VideoEngine.Instance.isLogDebugEnabled)
					{
						VideoEngine.log(string.Format("Ignored Matrix upload value (idx={0:X8})", index));
					}
				}
				else
				{
					int i = matrixIndex[index];
					if (matrix[i] != value)
					{
						matrix[i] = value;
						changed = true;
					}
				}
				index++;

				return index >= maxIndex;
			}

			public virtual bool Changed
			{
				get
				{
					return changed;
				}
				set
				{
					this.changed = value;
				}
			}

		}

		private class UseVertexCacheSettingsListerner : AbstractBoolSettingsListener
		{
			private readonly VideoEngine outerInstance;

			public UseVertexCacheSettingsListerner(VideoEngine outerInstance)
			{
				this.outerInstance = outerInstance;
			}


			protected internal override void settingsValueChanged(bool value)
			{
				outerInstance.UseVertexCache = value;
			}
		}

		private class UseTextureAnisotropicFilterSettingsListerner : AbstractBoolSettingsListener
		{
			private readonly VideoEngine outerInstance;

			public UseTextureAnisotropicFilterSettingsListerner(VideoEngine outerInstance)
			{
				this.outerInstance = outerInstance;
			}


			protected internal override void settingsValueChanged(bool value)
			{
				outerInstance.UseTextureAnisotropicFilter = value;
			}
		}

		private class DisableOptimizedVertexInfoReadingListener : AbstractBoolSettingsListener
		{
			private readonly VideoEngine outerInstance;

			public DisableOptimizedVertexInfoReadingListener(VideoEngine outerInstance)
			{
				this.outerInstance = outerInstance;
			}


			protected internal override void settingsValueChanged(bool value)
			{
				outerInstance.DisableOptimizedVertexInfoReading = value;
			}
		}

		private class UsexBRZFilterSettingsListerner : AbstractBoolSettingsListener
		{
			private readonly VideoEngine outerInstance;

			public UsexBRZFilterSettingsListerner(VideoEngine outerInstance)
			{
				this.outerInstance = outerInstance;
			}


			protected internal override void settingsValueChanged(bool value)
			{
				outerInstance.UsexBRZFilter = value;
			}
		}

		private static void log(string msg)
		{
			log_Renamed.debug(msg);
		}

		public static VideoEngine Instance
		{
			get
			{
				if (instance == null)
				{
					helper = new GeCommands();
					instance = new VideoEngine();
				}
				return instance;
			}
		}

		private VideoEngine()
		{
			context = new GeContext();
			modelMatrixUpload = new MatrixUpload(context.model_uploaded_matrix, 3, 4);
			viewMatrixUpload = new MatrixUpload(context.view_uploaded_matrix, 3, 4);
			textureMatrixUpload = new MatrixUpload(context.texture_uploaded_matrix, 3, 4);
			projectionMatrixUpload = new MatrixUpload(context.proj_uploaded_matrix, 4, 4);
			boneMatrixLinearUpdatedMatrix = 8;

			commandStatistics = new DurationStatistics[256];
			for (int i = 0; i < commandStatistics.Length; i++)
			{
				commandStatistics[i] = new DurationStatistics(string.Format("{0,-11}", helper.getCommandString(i)));
			}

//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: bboxVertices = new float[8][3];
			bboxVertices = RectangularArrays.ReturnRectangularFloatArray(8, 3);
			for (int i = 0; i < 8; i++)
			{
				bboxVertices[i] = new float[3];
			}

			currentCMDValues = new int[256];
			currentListCMDValues = new int[256];
			videoTextures = new LinkedList<AddressRange>();

			multiDrawFirst = ByteBuffer.allocateDirect(maxMultiDrawElements * 4).order(ByteOrder.nativeOrder()).asIntBuffer();
			multiDrawCount = ByteBuffer.allocateDirect(maxMultiDrawElements * 4).order(ByteOrder.nativeOrder()).asIntBuffer();
			if (avoidDrawElementsWithNonZeroIndexOffset)
			{
				indexByteBuffer = ByteBuffer.allocateDirect(indexDrawBufferSizeInBytes).order(ByteOrder.nativeOrder());
			}
		}

		/// <summary>
		/// Called from pspge module
		/// </summary>
		public virtual void pushDrawList(PspGeList list)
		{
			lock (drawListQueue)
			{
				drawListQueue.AddLast(list);
			}
		}

		/// <summary>
		/// Called from pspge module
		/// </summary>
		public virtual void pushDrawListHead(PspGeList list)
		{
			lock (drawListQueue)
			{
				drawListQueue.AddFirst(list);
			}
		}

		public virtual int numberDrawLists()
		{
			lock (drawListQueue)
			{
				return drawListQueue.Count;
			}
		}

		public virtual bool hasDrawLists()
		{
			lock (drawListQueue)
			{
				return drawListQueue.Count > 0;
			}
		}

		public virtual bool hasDrawList(int listAddr, int stackAddr)
		{
			bool result = false;
			bool waitAndRetry = false;

			lock (drawListQueue)
			{
				if (currentList != null && currentList.isInUse(listAddr, stackAddr))
				{
					result = true;
					// The current list has already reached the FINISH command,
					// but the list processing is not yet completed.
					// Wait a little for the list to complete.
					if (currentList.Finished)
					{
						waitAndRetry = true;
					}
				}
				else
				{
					foreach (PspGeList list in drawListQueue)
					{
						if (list != null && list.isInUse(listAddr, stackAddr))
						{
							result = true;
							break;
						}
					}
				}
			}

			if (waitAndRetry)
			{
				// The current list is already finished but its processing is not yet
				// completed. Wait a little (100ms) and check again to avoid
				// the "can't enqueue duplicate list address" error.
				for (int i = 0; i < 100; i++)
				{
					if (log_Renamed.DebugEnabled)
					{
						log_Renamed.debug(string.Format("hasDrawList(0x{0:X8}) waiting on finished list {1}", listAddr, currentList));
					}
					Utilities.sleep(1, 0);
					lock (drawListQueue)
					{
						if (currentList == null || currentList.list_addr != listAddr)
						{
							result = false;
							break;
						}
					}
				}
			}

			return result;
		}

		public virtual PspGeList FirstDrawList
		{
			get
			{
				PspGeList firstList;
    
				lock (drawListQueue)
				{
					firstList = currentList;
					if (firstList == null)
					{
						firstList = drawListQueue.First.Value;
					}
				}
    
				return firstList;
			}
		}

		public virtual PspGeList LastDrawList
		{
			get
			{
				PspGeList lastList = null;
    
				lock (drawListQueue)
				{
					foreach (PspGeList list in drawListQueue)
					{
						if (list != null)
						{
							lastList = list;
						}
					}
    
					if (lastList == null)
					{
						lastList = currentList;
					}
				}
    
				return lastList;
			}
		}

		private void deletePendingBuffers()
		{
			while (buffersToBeDeleted.Count > 0)
			{
				int buffer = buffersToBeDeleted.RemoveAt(0);
				bufferManager.deleteBuffer(buffer);
			}
		}

		public virtual void stop()
		{
			// If we are still drawing a list, stop the list processing
			if (currentList != null)
			{
				lock (drawListQueue)
				{
					drawListQueue.Clear();
				}
				listHasEnded = true;
				try
				{
					Thread.Sleep(100);
				}
				catch (InterruptedException)
				{
					// Ignore Exception
				}
			}

			// The buffers have to be deleted from the GUI thread
			buffersToBeDeleted.Add(bufferId);
			buffersToBeDeleted.Add(nativeBufferId);
			buffersToBeDeleted.Add(indexBufferId);
			bufferId = -1;
			nativeBufferId = -1;
			indexBufferId = -1;
			floatBufferArray = null;

			Settings.Instance.removeSettingsListener(name);
		}

		public virtual void start()
		{
			Settings.Instance.registerSettingsListener(name, "emu.useVertexCache", new UseVertexCacheSettingsListerner(this));
			Settings.Instance.registerSettingsListener(name, "emu.graphics.filters.anisotropic", new UseTextureAnisotropicFilterSettingsListerner(this));
			Settings.Instance.registerSettingsListener(name, "emu.plugins.xbrz", new UsexBRZFilterSettingsListerner(this));
			Settings.Instance.registerSettingsListener(name, "emu.disableoptimizedvertexinforeading", new DisableOptimizedVertexInfoReadingListener(this));

			MaxTextureSize = Settings.Instance.readInt("maxTextureSize", 512);
			DoubleTexture2DCoords = Settings.Instance.readBool("doubleTexture2DCoords");

			display = Modules.sceDisplayModule;
			re = display.RenderingEngine;
			re.GeContext = context;
			context.RenderingEngine = re;
			bufferManager = re.BufferManager;

			if (!re.BufferManager.useVBO())
			{
				// VertexCache is relying on VBO
				useVertexCache = false;
			}

			deletePendingBuffers();

			bufferId = bufferManager.genBuffer(pspsharp.graphics.RE.IRenderingEngine_Fields.RE_ARRAY_BUFFER, pspsharp.graphics.RE.IRenderingEngine_Fields.RE_FLOAT, drawBufferSizeInBytes / SIZEOF_FLOAT, pspsharp.graphics.RE.IRenderingEngine_Fields.RE_STREAM_DRAW);
			nativeBufferId = bufferManager.genBuffer(pspsharp.graphics.RE.IRenderingEngine_Fields.RE_ARRAY_BUFFER, pspsharp.graphics.RE.IRenderingEngine_Fields.RE_BYTE, drawBufferSizeInBytes, pspsharp.graphics.RE.IRenderingEngine_Fields.RE_STREAM_DRAW);
			indexBufferId = bufferManager.genBuffer(pspsharp.graphics.RE.IRenderingEngine_Fields.RE_ELEMENT_ARRAY_BUFFER, pspsharp.graphics.RE.IRenderingEngine_Fields.RE_UNSIGNED_BYTE, indexDrawBufferSizeInBytes, pspsharp.graphics.RE.IRenderingEngine_Fields.RE_STREAM_DRAW);
			floatBufferArray = new float[drawBufferSizeInBytes / SIZEOF_FLOAT];

			if (useAsyncVertexCache_Renamed)
			{
				AsyncVertexCache.Instance.UseVertexArray = re.VertexArrayAvailable;
			}

			context.setDirty();
			projectionMatrixUpload.Changed = true;
			modelMatrixUpload.Changed = true;
			viewMatrixUpload.Changed = true;
			textureMatrixUpload.Changed = true;
			lightingChanged = true;
			textureChanged = true;
			geBufChanged = true;
			viewportChanged = true;
			depthChanged = true;
			materialChanged = true;
			previousPrim = PRIM_SPRITES;
			listCount = 0;

			cachedInstructions = new Dictionary<int, int[]>();
		}

		public virtual IRenderingEngine RenderingEngine
		{
			get
			{
				return re;
			}
		}

		public virtual GeContext Context
		{
			get
			{
				return context;
			}
		}

		public static void exit()
		{
			if (instance != null)
			{
				if (instance.re != null)
				{
					instance.re.exit();
				}
				if (DurationStatistics.collectStatistics)
				{
					log_Renamed.info(instance.statistics);
					Array.Sort(instance.commandStatistics);
					const int numberCommands = 20;
					log_Renamed.info(string.Format("{0:D} most time intensive Video commands:", numberCommands));
					for (int i = 0; i < numberCommands; i++)
					{
						log_Renamed.info(string.Format("    {0}", instance.commandStatistics[i]));
					}
					log_Renamed.info(instance.vertexStatistics);
					log_Renamed.info(instance.vertexReadingStatistics);
					log_Renamed.info(instance.drawArraysStatistics);
					log_Renamed.info(instance.waitSignalStatistics);
					log_Renamed.info(instance.waitStallStatistics);
					log_Renamed.info(instance.textureCacheLookupStatistics);
					log_Renamed.info(instance.vertexCacheLookupStatistics);
					VertexBufferManager.exit();
					VertexArrayManager.exit();
				}
			}
		}

		public static DurationStatistics Statistics
		{
			get
			{
				if (instance == null)
				{
					return null;
				}
    
				return instance.statistics;
			}
		}

		/// <summary>
		/// call from GL thread
		/// </summary>
		/// <returns> true if an update was made </returns>
		public virtual bool update()
		{
			int listCount;
			lock (drawListQueue)
			{
				listCount = drawListQueue.Count;
				currentList = drawListQueue.RemoveFirst();
			}
			if (currentList == null)
			{
				return false;
			}

			startUpdate();

			if (State.captureGeNextFrame)
			{
				CaptureManager.startCapture("capture.bin", currentList);
			}

			if (State.replayGeNextFrame)
			{
				// Load the replay list into drawListQueue
				CaptureManager.startReplay("capture.bin");

				// Hijack the current list with the replay list
				// TODO this is assuming there is only 1 list in drawListQueue at this point, only the last list is the replay list
				PspGeList replayList = drawListQueue.RemoveFirst();
				replayList.id = currentList.id;
				replayList.blockedThreadIds.Clear();
				((List<int>)replayList.blockedThreadIds).AddRange(currentList.blockedThreadIds);
				currentList = replayList;
			}

			// Draw only as many lists as currently available in the drawListQueue.
			// Some game add automatically a new list to the queue when the current
			// list is finishing.
			do
			{
				executeList();
				listCount--;
				if (listCount <= 0)
				{
					break;
				}

				lock (drawListQueue)
				{
					currentList = drawListQueue.RemoveFirst();
				}
			} while (currentList != null);

			if (State.captureGeNextFrame)
			{
				// Can't end capture until we get a sceDisplaySetFrameBuf after the list has executed
				CaptureManager.markListExecuted();
			}

			if (State.replayGeNextFrame)
			{
				State.replayGeNextFrame = false;
				CaptureManager.endReplay();
			}

			endUpdate();

			lock (drawListQueue)
			{
				currentList = null;
			}

			return somethingDisplayed;
		}

		private void logLevelUpdated()
		{
			isLogTraceEnabled = log_Renamed.TraceEnabled;
			isLogDebugEnabled = log_Renamed.DebugEnabled;
			isLogInfoEnabled = log_Renamed.InfoEnabled;
			isLogWarnEnabled = log_Renamed.isEnabledFor(Level.WARN);
		}

		public virtual Level LogLevel
		{
			set
			{
				log_Renamed.Level = value;
				logLevelUpdated();
			}
		}

		/// <summary>
		/// The memory used by GE has been updated or changed. Update the caches so
		/// that they see these changes.
		/// </summary>
		private void memoryForGEUpdated()
		{
			if (useTextureCache)
			{
				TextureCache.Instance.resetTextureAlreadyHashed();
			}
			if (useVertexCache)
			{
				VertexCache.Instance.resetVertexAlreadyChecked();
			}
			VertexBufferManager.Instance.resetAddressAlreadyChecked();
		}

		public virtual void hleSetFrameBuf(int topAddr, int bufferWidth, int pixelFormat)
		{
			if (context.fbp != topAddr || context.fbw != bufferWidth || context.psm != pixelFormat)
			{
				// Update the frame buffer parameters at next display start.
				// Do not update the context here, possibly in the middle of a rendering...
				fbBufChanged = true;
			}
		}

		public virtual void resetCurrentListCMDValues()
		{
			// Reset all the values
			for (int i = 0; i < currentListCMDValues.Length; i++)
			{
				currentListCMDValues[i] = -1;
			}
		}

		private void startUpdate()
		{
			// Wait longer for a sync when the compiler is not enabled... pspsharp is then much slower
			maxWaitForSyncCount = RuntimeContext.CompilerEnabled ? 100 : 10000;

			statistics.start();

			logLevelUpdated();
			isGeProfilerEnabled = GEProfiler.ProfilerEnabled;
			memoryForGEUpdated();
			somethingDisplayed = false;
			geBufChanged = true;
			forceLoadGEToScreen = true;
			textureChanged = true;
			projectionMatrixUpload.Changed = true;
			modelMatrixUpload.Changed = true;
			viewMatrixUpload.Changed = true;
			textureMatrixUpload.Changed = true;
			clutIsDirty = true;
			lightingChanged = true;
			viewportChanged = true;
			depthChanged = true;
			materialChanged = true;
			scissorChanged = true;
			errorCount = 0;
			usingTRXKICK = false;
			multiTrxkickStart = -1;
			multiTrxkickEnd = -1;
			maxSpriteHeight = 0;
			maxSpriteWidth = 0;
			primCount = 0;
			nopCount = 0;
			listCount++;

			resetCurrentListCMDValues();

			if (fbBufChanged)
			{
				context.fbp = display.TopAddrFb;
				context.fbw = display.BufferWidthFb;
				context.psm = display.PixelFormatFb;
				geBufChanged = true;
				fbBufChanged = false;
			}

			context.update();

			if (wantClearTextureCache)
			{
				TextureCache.Instance.reset(re);
				GETextureManager.Instance.reset(re);
				resetVideoTextures();
				wantClearTextureCache = false;
			}

			if (wantClearVertexCache)
			{
				VertexCache.Instance.reset(re);
				VertexBufferManager.Instance.reset(re);
				wantClearVertexCache = false;
			}

			deletePendingBuffers();

			hasModdedTextureDirectory = System.IO.Directory.Exists(ModdedTextureDirectory);

			if (videoTextures.Count > 0)
			{
				lock (videoTextures)
				{
					// Check if some video textures are obsolete
//JAVA TO C# CONVERTER WARNING: Unlike Java's ListIterator, enumerators in .NET do not allow altering the collection:
					for (IEnumerator<AddressRange> lit = videoTextures.GetEnumerator(); lit.MoveNext();)
					{
						AddressRange videoTexture = lit.Current;
						if (videoTexture.Obsolete)
						{
//JAVA TO C# CONVERTER TODO TASK: .NET enumerators are read-only:
							lit.remove();
						}
					}
				}
			}

			if (State.exportGeNextFrame)
			{
				startExport3D();
			}
		}

		private static string ExportDirectory
		{
			get
			{
				for (int i = 1; true; i++)
				{
					string directory = string.Format("{0}Export-{1:D}{2}", pspsharp.graphics.export.IGraphicsExporter_Fields.exportDirectory, i, System.IO.Path.DirectorySeparatorChar);
					if (!System.IO.Directory.Exists(directory) || System.IO.File.Exists(directory))
					{
						return directory;
					}
				}
			}
		}

		private void startExport3D()
		{
			export3D = true;
			export3DOnlyVisible = State.exportGeOnlyVisibleElements;
			State.exportGeNextFrame = false;

			export3DDirectory = ExportDirectory;
			if (System.IO.Directory.CreateDirectory(export3DDirectory))
			{
				exporter = new WavefrontExporter();
				exporter.startExport(context, export3DDirectory);
			}
			else
			{
				log_Renamed.error(string.Format("Cannot create export directory '{0}'", export3DDirectory));
			}
		}

		private void endExport3D()
		{
			exporter.endExport();

			exporter = null;
			export3D = false;
		}

		private void endUpdate()
		{
			if (export3D)
			{
				endExport3D();
			}

			if (re.VertexArrayAvailable)
			{
				re.bindVertexArray(0);
			}

			re.waitForRenderingCompletion();

			context.reTextureGenS.setEnabled(false);
			context.reTextureGenT.setEnabled(false);

			if (useVertexCache)
			{
				if (primCount > VertexCache.cacheMaxSize)
				{
					log_Renamed.warn(string.Format("VertexCache size ({0:D}) too small to execute {1:D} PRIM commands", VertexCache.cacheMaxSize, primCount));
				}
			}

			statistics.end();
		}

		public virtual void error(string message)
		{
			errorCount++;
			log_Renamed.error(message);
			if (errorCount >= maxErrorCount)
			{
				if (tryToFallback())
				{
					log_Renamed.error("Aborting current list processing due to too many errors");
				}
			}
		}

		private bool tryToFallback()
		{
			bool abort = false;

			if (!currentList.StackEmpty)
			{
				// When have some CALLs on the stack, try to return from the last CALL
				int oldPc = currentList.Pc;
				currentList.ret();
				int newPc = currentList.Pc;
				if (isLogDebugEnabled)
				{
					log(string.Format("tryToFallback old PC: 0x{0:X8}, new PC: 0x{1:X8}", oldPc, newPc));
				}
			}
			else
			{
				// Finish this list
				currentList.finishList();
				// Trigger a FINISH callback to avoid hanging the application...
				currentList.pushFinishCallback(currentList.id, 0);
				listHasEnded = true;
				abort = true;
			}

			return abort;
		}

		private void checkCurrentListPc()
		{
			Memory mem = Memory.Instance;
			while (!Memory.isAddressGood(currentList.Pc))
			{
				if (!mem.IgnoreInvalidMemoryAccess)
				{
					error("Reading GE list from invalid address 0x" + currentList.Pc.ToString("x"));
					break;
				}
				// Ignoring memory read errors.
				// Try to fall back and continue the list processing.
				log_Renamed.warn("Reading GE list from invalid address 0x" + currentList.Pc.ToString("x"));
				if (tryToFallback())
				{
					break;
				}
			}
		}

		private void executeHleAction()
		{
			if (hleAction != null)
			{
				hleAction.execute();
				hleAction = null;
			}
		}

		private void executeListStalled()
		{
			waitStallStatistics.start();
			if (isLogDebugEnabled)
			{
				log_Renamed.debug(string.Format("Stall address 0x{0:X8} reached, waiting for Sync", currentList.Pc));
			}
			currentList.status = PSP_GE_LIST_STALL_REACHED;
			long startWaitClockMillis = Emulator.Clock.milliTime();
			int waitMillis = 10;
			if (!currentList.waitForSync(waitMillis))
			{
				long endWaitClockMillis = Emulator.Clock.milliTime();
				if (isLogDebugEnabled)
				{
					log_Renamed.debug("Wait for sync while stall reached");
				}
				// Count only when the clock is not paused
				if (endWaitClockMillis - startWaitClockMillis >= waitMillis - 1)
				{
					waitForSyncCount++;
				}

				// Waiting maximum 100 * 10ms (= 1 second) on a stall address.
				// After this timeout, abort the list.
				//
				// When the stall address is at the very beginning of the list
				// (i.e. the list has just been enqueued, but the stall has not yet been updated),
				// allow waiting for a longer time (the CPU might be busy
				// compiling a huge CodeBlock on the first call).
				// This avoids aborting the first list enqueued.
				int maxStallCount = maxWaitForSyncCount;
				if (currentList.Pc == currentList.list_addr)
				{
					maxStallCount *= 60; // Waiting for 60 seconds...
				}
				if (isLogDebugEnabled)
				{
					maxStallCount = int.MaxValue;
				}

				if (waitForSyncCount > maxStallCount)
				{
					error(string.Format("Waiting too long on stall address 0x{0:X8}, aborting the list {1}", currentList.Pc, currentList));
				}
			}
			else
			{
				waitForSyncCount = 0;
			}
			executeHleAction();
			if (!currentList.StallReached)
			{
				currentList.status = PSP_GE_LIST_DRAWING;
			}
			waitStallStatistics.end();
		}

		public virtual bool WaitingOnStall
		{
			get
			{
				return currentList != null && currentList.status == PSP_GE_LIST_STALL_REACHED && waitForSyncCount > 0;
			}
		}

		private bool executeListPaused()
		{
			if (isLogDebugEnabled)
			{
				log_Renamed.debug(string.Format("FINISH / SIGNAL / END reached, waiting for Sync ({0})", currentList.ToString()));
			}
			currentList.status = PSP_GE_LIST_END_REACHED;

			// No need to wait of the END command for a FINISH.
			if (currentList.Finished)
			{
				listHasEnded = true;
				return true;
			}

			waitSignalStatistics.start();
			long startWaitClockMillis = Emulator.Clock.milliTime();
			if (!currentList.waitForSync(10))
			{
				long endWaitClockMillis = Emulator.Clock.milliTime();
				if (isLogDebugEnabled)
				{
					log_Renamed.debug("Wait for sync while END reached");
				}
				// Count only when the clock is not paused
				if (startWaitClockMillis != endWaitClockMillis)
				{
					waitForSyncCount++;
				}

				// Waiting maximum 100 * 10ms (= 1 second) on an END command.
				// After this timeout, abort the list.
				if (waitForSyncCount > maxWaitForSyncCount)
				{
					error(string.Format("Waiting too long on an END command, aborting the list {0}", currentList));
				}
			}
			else
			{
				waitForSyncCount = 0;
			}

			executeHleAction();
			if (currentList.Restarted)
			{
				currentList.clearRestart();
				currentList.clearPaused();
			}
			if (!currentList.Paused)
			{
				if (currentList.Finished)
				{
					listHasEnded = true;
					return true;
				}
				currentList.status = PSP_GE_LIST_DRAWING;
			}
			waitSignalStatistics.end();

			return false;
		}

		// call from GL thread
		// There is an issue here with Emulator.pause
		// - We want to stop on errors
		// - But user may also press pause button
		//   - Either continue drawing to the end of the list (bad if the list contains an infinite loop)
		//   - Or we want to be able to restart drawing when the user presses the run button
		private void executeList()
		{
			if (skipThisFrame && skipListWhenSkippingFrame)
			{
				listHasEnded = true;
				currentList.status = PSP_GE_LIST_DONE;
				Modules.sceGe_userModule.hleGeListSyncDone(currentList);
				executeHleAction();
				return;
			}

			listHasEnded = false;
			currentList.status = PSP_GE_LIST_DRAWING;

			if (isLogDebugEnabled)
			{
				log("executeList " + currentList);
			}

			executeHleAction();

			// Save the context at the beginning of the list processing to the given address (used by sceGu).
			if (currentList.hasSaveContextAddr())
			{
				saveContext(currentList.SaveContextAddr);
			}

			if (isGeProfilerEnabled)
			{
				listStartMicroTime = Emulator.Clock.microTime();
				GEProfiler.startGeList();
			}

			waitForSyncCount = 0;
			while (!listHasEnded && (!Emulator.pause || State.captureGeNextFrame))
			{
				if (currentList.Paused || currentList.Ended)
				{
					if (executeListPaused())
					{
						break;
					}
				}
				else if (currentList.StallReached)
				{
					executeListStalled();
				}
				else
				{
					int ins = currentList.readNextInstruction();
					executeCommand(ins);
				}
			}

			if (Emulator.pause && !listHasEnded)
			{
				if (isLogInfoEnabled)
				{
					log_Renamed.info("Emulator paused - cancelling current list id=" + currentList.id);
				}
				currentList.status = PSP_GE_LIST_CANCEL_DONE;
			}

			// let DONE take priority over STALL_REACHED
			if (listHasEnded)
			{
				currentList.status = PSP_GE_LIST_END_REACHED;

				// Tested on PSP:
				// A list is only DONE after a combination of FINISH + END.
				if (currentList.Ended)
				{
					currentList.status = PSP_GE_LIST_DONE;
				}
			}

			if (currentList.Done)
			{
				if (isGeProfilerEnabled)
				{
					long listEndMicroTime = Emulator.Clock.microTime();
					GEProfiler.geListDuration(listEndMicroTime - listStartMicroTime);
				}
				Modules.sceGe_userModule.hleGeListSyncDone(currentList);
			}

			executeHleAction();

			// Restore the context to the state at the beginning of the list processing (used by sceGu).
			if (currentList.hasSaveContextAddr())
			{
				restoreContext(currentList.SaveContextAddr);
			}
		}

		public virtual PspGeList CurrentList
		{
			get
			{
				return currentList;
			}
		}

		public virtual float[] getMatrix(int mtxtype)
		{
			float[] resmtx;
			switch (mtxtype)
			{
				case PSP_GE_MATRIX_BONE0:
				case PSP_GE_MATRIX_BONE1:
				case PSP_GE_MATRIX_BONE2:
				case PSP_GE_MATRIX_BONE3:
				case PSP_GE_MATRIX_BONE4:
				case PSP_GE_MATRIX_BONE5:
				case PSP_GE_MATRIX_BONE6:
				case PSP_GE_MATRIX_BONE7:
					resmtx = context.bone_uploaded_matrix[mtxtype - PSP_GE_MATRIX_BONE0];
					break;
				case PSP_GE_MATRIX_WORLD:
					resmtx = context.model_uploaded_matrix;
					break;
				case PSP_GE_MATRIX_VIEW:
					resmtx = context.view_uploaded_matrix;
					break;
				case PSP_GE_MATRIX_PROJECTION:
					resmtx = context.proj_uploaded_matrix;
					break;
				case PSP_GE_MATRIX_TEXGEN:
					resmtx = context.texture_uploaded_matrix;
					break;
				default:
					resmtx = null;
					break;
			}

			return resmtx;
		}

		public virtual int getCommandValue(int cmd)
		{
			if (cmd < 0 || cmd >= currentCMDValues.Length)
			{
				return 0;
			}
			return currentCMDValues[cmd];
		}

		public virtual string commandToString(int cmd)
		{
			return GeCommands.Instance.getCommandString(cmd);
		}

		public static int command(int instruction)
		{
			return ((int)((uint)instruction >> 24));
		}

		private static int intArgument(int instruction)
		{
			return (instruction & 0x00FFFFFF);
		}

		private static float floatArgument(int normalArgument)
		{
			return Float.intBitsToFloat(normalArgument << 8);
		}

		private int getClutAddr(int level, int clutNumEntries, int clutEntrySize)
		{
			return context.tex_clut_addr + (context.tex_clut_start << 4) * clutEntrySize;
		}

		private void readClut()
		{
			if (!clutIsDirty || context.tex_clut_addr == 0)
			{
				return;
			}

			if (!Memory.isAddressGood(context.tex_clut_addr))
			{
				if (isLogWarnEnabled)
				{
					log_Renamed.warn(string.Format("Invalid clut address 0x{0:X8}", context.tex_clut_addr));
				}
				return;
			}

			if (context.tex_clut_mode == CMODE_FORMAT_32BIT_ABGR8888)
			{
				readClut32(0);
			}
			else
			{
				readClut16(0);
			}
		}

		public virtual short[] readClut16(int level)
		{
			// Update the clut_buffer only if some clut parameters have been changed
			// since last update.
			if (clutIsDirty)
			{
				int clutNumEntries = context.tex_clut_num_blocks << 4;
				int clutOffset = context.tex_clut_start << 4;
				IMemoryReader memoryReader = MemoryReader.getMemoryReader(getClutAddr(level, clutNumEntries, 2), (clutNumEntries - clutOffset) << 1, 2);
				for (int i = clutOffset; i < clutNumEntries; i++)
				{
					clut_buffer16[i] = (short) memoryReader.readNext();
				}
				clutIsDirty = false;
			}

			if (State.captureGeNextFrame)
			{
				log_Renamed.info("Capture readClut16");
				CaptureManager.captureRAM(context.tex_clut_addr, context.tex_clut_num_blocks * 32);
			}

			return clut_buffer16;
		}

		public virtual int[] readClut32(int level)
		{
			// Update the clut_buffer only if some clut parameters have been changed
			// since last update.
			if (clutIsDirty)
			{
				int clutNumEntries = context.tex_clut_num_blocks << 3;
				int clutOffset = context.tex_clut_start << 4;
				IMemoryReader memoryReader = MemoryReader.getMemoryReader(getClutAddr(level, clutNumEntries, 4), (clutNumEntries - clutOffset) << 2, 4);
				for (int i = clutOffset; i < clutNumEntries; i++)
				{
					clut_buffer32[i] = memoryReader.readNext();
				}
				clutIsDirty = false;
			}

			if (State.captureGeNextFrame)
			{
				log_Renamed.info("Capture readClut32");
				CaptureManager.captureRAM(context.tex_clut_addr, context.tex_clut_num_blocks * 32);
			}

			return clut_buffer32;
		}

		private int getClutIndex(int index)
		{
			return ((index >> context.tex_clut_shift) & context.tex_clut_mask) | (context.tex_clut_start << 4);
		}

		// UnSwizzling based on pspplayer
		private Buffer unswizzleTextureFromMemory(int texaddr, int bytesPerPixel, int level, int textureBufferWidthInPixels)
		{
			int rowWidth = (bytesPerPixel > 0) ? (textureBufferWidthInPixels * bytesPerPixel) : (textureBufferWidthInPixels / 2);
			int pitch = rowWidth / 4;
			int bxc = rowWidth / 16;
			int byc = System.Math.Max((context.texture_height[level] + 7) / 8, 1);

			int ydest = 0;

			IMemoryReader memoryReader = MemoryReader.getMemoryReader(texaddr, 4);
			for (int by = 0; by < byc; by++)
			{
				if (rowWidth >= 16)
				{
					int xdest = ydest;
					for (int bx = 0; bx < bxc; bx++)
					{
						int dest = xdest;
						for (int n = 0; n < 8; n++)
						{
							tmp_texture_buffer32[dest] = memoryReader.readNext();
							tmp_texture_buffer32[dest + 1] = memoryReader.readNext();
							tmp_texture_buffer32[dest + 2] = memoryReader.readNext();
							tmp_texture_buffer32[dest + 3] = memoryReader.readNext();

							dest += pitch;
						}
						xdest += 4;
					}
					ydest += (rowWidth * 8) / 4;
				}
				else if (rowWidth == 8)
				{
					for (int n = 0; n < 8; n++, ydest += 2)
					{
						tmp_texture_buffer32[ydest] = memoryReader.readNext();
						tmp_texture_buffer32[ydest + 1] = memoryReader.readNext();
						memoryReader.skip(2);
					}
				}
				else if (rowWidth == 4)
				{
					for (int n = 0; n < 8; n++, ydest++)
					{
						tmp_texture_buffer32[ydest] = memoryReader.readNext();
						memoryReader.skip(3);
					}
				}
				else if (rowWidth == 2)
				{
					for (int n = 0; n < 4; n++, ydest++)
					{
						int n1 = memoryReader.readNext() & 0xFFFF;
						memoryReader.skip(3);
						int n2 = memoryReader.readNext() & 0xFFFF;
						memoryReader.skip(3);
						tmp_texture_buffer32[ydest] = n1 | (n2 << 16);
					}
				}
				else if (rowWidth == 1)
				{
					for (int n = 0; n < 2; n++, ydest++)
					{
						int n1 = memoryReader.readNext() & 0xFF;
						memoryReader.skip(3);
						int n2 = memoryReader.readNext() & 0xFF;
						memoryReader.skip(3);
						int n3 = memoryReader.readNext() & 0xFF;
						memoryReader.skip(3);
						int n4 = memoryReader.readNext() & 0xFF;
						memoryReader.skip(3);
						tmp_texture_buffer32[ydest] = n1 | (n2 << 8) | (n3 << 16) | (n4 << 24);
					}
				}
			}

			if (State.captureGeNextFrame)
			{
				log_Renamed.info("Capture unswizzleTextureFromMemory");
				CaptureManager.captureRAM(texaddr, rowWidth * context.texture_height[level]);
			}

			return IntBuffer.wrap(tmp_texture_buffer32);
		}

		private string getArgumentLog(int normalArgument)
		{
			if (normalArgument == 0)
			{
				return "(0)"; // a very common case...
			}

			return string.Format("(hex={0:X8},int={1:D},float={2:F})", normalArgument, normalArgument, floatArgument(normalArgument));
		}

		public virtual void executeCommand(int instruction)
		{
			command_Renamed = command(instruction);

			// Quick check: pure state commands can be ignored when they are
			// repeated with the same parameters. These are redundant commands.
			if (pureStateCommands[command_Renamed])
			{
				if (currentListCMDValues[command_Renamed] == instruction)
				{
					if (isLogDebugEnabled)
					{
						log_Renamed.debug(string.Format("{0} 0x{1:X6} redundant pure state cmd ignored", helper.getCommandString(command_Renamed), intArgument(instruction)));
					}
					return;
				}
				currentListCMDValues[command_Renamed] = instruction;
			}

			normalArgument = intArgument(instruction);
			// Compute floatArgument only on demand, most commands do not use it.
			//float floatArgument = floatArgument(instruction);

			currentCMDValues[command_Renamed] = normalArgument;
			if (DurationStatistics.collectStatistics)
			{
				commandStatistics[command_Renamed].start();
			}
			switch (command_Renamed)
			{
				case NOP:
					executeCommandNOP();
					break;
				case VADDR:
					executeCommandVADDR();
					break;
				case IADDR:
					executeCommandIADDR();
					break;
				case PRIM:
					executeCommandPRIM();
					break;
				case BEZIER:
					executeCommandBEZIER();
					break;
				case SPLINE:
					executeCommandSPLINE();
					break;
				case BBOX:
					executeCommandBBOX();
					break;
				case JUMP:
					executeCommandJUMP();
					break;
				case BJUMP:
					executeCommandBJUMP();
					break;
				case CALL:
					executeCommandCALL();
					break;
				case RET:
					executeCommandRET();
					break;
				case END:
					executeCommandEND();
					break;
				case SIGNAL:
					executeCommandSIGNAL();
					break;
				case FINISH:
					executeCommandFINISH();
					break;
				case BASE:
					executeCommandBASE();
					break;
				case VTYPE:
					executeCommandVTYPE();
					break;
				case OFFSET_ADDR:
					executeCommandOFFSET_ADDR();
					break;
				case ORIGIN_ADDR:
					executeCommandORIGIN_ADDR();
					break;
				case REGION1:
					executeCommandREGION1();
					break;
				case REGION2:
					executeCommandREGION2();
					break;
				case LTE:
					executeCommandLTE();
					break;
				case LTE0:
				case LTE1:
				case LTE2:
				case LTE3:
					executeCommandLTEn();
					break;
				case CPE:
					executeCommandCPE();
					break;
				case BCE:
					executeCommandBCE();
					break;
				case TME:
					executeCommandTME();
					break;
				case FGE:
					executeCommandFGE();
					break;
				case DTE:
					executeCommandDTE();
					break;
				case ABE:
					executeCommandABE();
					break;
				case ATE:
					executeCommandATE();
					break;
				case ZTE:
					executeCommandZTE();
					break;
				case STE:
					executeCommandSTE();
					break;
				case AAE:
					executeCommandAAE();
					break;
				case PCE:
					executeCommandPCE();
					break;
				case CTE:
					executeCommandCTE();
					break;
				case LOE:
					executeCommandLOE();
					break;
				case BOFS:
					executeCommandBOFS();
					break;
				case BONE:
					executeCommandBONE();
					break;
				case MW0:
				case MW1:
				case MW2:
				case MW3:
				case MW4:
				case MW5:
				case MW6:
				case MW7:
					executeCommandMWn();
					break;
				case PSUB:
					executeCommandPSUB();
					break;
				case PPRIM:
					executeCommandPPRIM();
					break;
				case PFACE:
					executeCommandPFACE();
					break;
				case MMS:
					executeCommandMMS();
					break;
				case MODEL:
					executeCommandMODEL();
					break;
				case VMS:
					executeCommandVMS();
					break;
				case VIEW:
					executeCommandVIEW();
					break;
				case PMS:
					executeCommandPMS();
					break;
				case PROJ:
					executeCommandPROJ();
					break;
				case TMS:
					executeCommandTMS();
					break;
				case TMATRIX:
					executeCommandTMATRIX();
					break;
				case XSCALE:
					executeCommandXSCALE();
					break;
				case YSCALE:
					executeCommandYSCALE();
					break;
				case ZSCALE:
					executeCommandZSCALE();
					break;
				case XPOS:
					executeCommandXPOS();
					break;
				case YPOS:
					executeCommandYPOS();
					break;
				case ZPOS:
					executeCommandZPOS();
					break;
				case USCALE:
					executeCommandUSCALE();
					break;
				case VSCALE:
					executeCommandVSCALE();
					break;
				case UOFFSET:
					executeCommandUOFFSET();
					break;
				case VOFFSET:
					executeCommandVOFFSET();
					break;
				case OFFSETX:
					executeCommandOFFSETX();
					break;
				case OFFSETY:
					executeCommandOFFSETY();
					break;
				case SHADE:
					executeCommandSHADE();
					break;
				case RNORM:
					executeCommandRNORM();
					break;
				case CMAT:
					executeCommandCMAT();
					break;
				case EMC:
					executeCommandEMC();
					break;
				case AMC:
					executeCommandAMC();
					break;
				case DMC:
					executeCommandDMC();
					break;
				case SMC:
					executeCommandSMC();
					break;
				case AMA:
					executeCommandAMA();
					break;
				case SPOW:
					executeCommandSPOW();
					break;
				case ALC:
					executeCommandALC();
					break;
				case ALA:
					executeCommandALA();
					break;
				case LMODE:
					executeCommandLMODE();
					break;
				case LT0:
				case LT1:
				case LT2:
				case LT3:
					executeCommandLTn();
					break;
				case LXP0:
				case LXP1:
				case LXP2:
				case LXP3:
				case LYP0:
				case LYP1:
				case LYP2:
				case LYP3:
				case LZP0:
				case LZP1:
				case LZP2:
				case LZP3:
					executeCommandLXPn();
					break;
				case LXD0:
				case LXD1:
				case LXD2:
				case LXD3:
				case LYD0:
				case LYD1:
				case LYD2:
				case LYD3:
				case LZD0:
				case LZD1:
				case LZD2:
				case LZD3:
					executeCommandLXDn();
					break;
				case LCA0:
				case LCA1:
				case LCA2:
				case LCA3:
					executeCommandLCAn();
					break;
				case LLA0:
				case LLA1:
				case LLA2:
				case LLA3:
					executeCommandLLAn();
					break;
				case LQA0:
				case LQA1:
				case LQA2:
				case LQA3:
					executeCommandLQAn();
					break;
				case SLE0:
				case SLE1:
				case SLE2:
				case SLE3:
					executeCommandSLEn();
					break;
				case SLF0:
				case SLF1:
				case SLF2:
				case SLF3:
					executeCommandSLFn();
					break;
				case ALC0:
				case ALC1:
				case ALC2:
				case ALC3:
					executeCommandALCn();
					break;
				case DLC0:
				case DLC1:
				case DLC2:
				case DLC3:
					executeCommandDLCn();
					break;
				case SLC0:
				case SLC1:
				case SLC2:
				case SLC3:
					executeCommandSLCn();
					break;
				case FFACE:
					executeCommandFFACE();
					break;
				case FBP:
					executeCommandFBP();
					break;
				case FBW:
					executeCommandFBW();
					break;
				case ZBP:
					executeCommandZBP();
					break;
				case ZBW:
					executeCommandZBW();
					break;
				case TBP0:
				case TBP1:
				case TBP2:
				case TBP3:
				case TBP4:
				case TBP5:
				case TBP6:
				case TBP7:
					executeCommandTBPn();
					break;
				case TBW0:
				case TBW1:
				case TBW2:
				case TBW3:
				case TBW4:
				case TBW5:
				case TBW6:
				case TBW7:
					executeCommandTBWn();
					break;
				case CBP:
					executeCommandCBP();
					break;
				case CBPH:
					executeCommandCBPH();
					break;
				case TRXSBP:
					executeCommandTRXSBP();
					break;
				case TRXSBW:
					executeCommandTRXSBW();
					break;
				case TRXDBP:
					executeCommandTRXDBP();
					break;
				case TRXDBW:
					executeCommandTRXDBW();
					break;
				case TSIZE0:
				case TSIZE1:
				case TSIZE2:
				case TSIZE3:
				case TSIZE4:
				case TSIZE5:
				case TSIZE6:
				case TSIZE7:
					executeCommandTSIZEn();
					break;
				case TMAP:
					executeCommandTMAP();
					break;
				case TEXTURE_ENV_MAP_MATRIX:
					executeCommandTEXTURE_ENV_MAP_MATRIX();
					break;
				case TMODE:
					executeCommandTMODE();
					break;
				case TPSM:
					executeCommandTPSM();
					break;
				case CLOAD:
					executeCommandCLOAD();
					break;
				case CMODE:
					executeCommandCMODE();
					break;
				case TFLT:
					executeCommandTFLT();
					break;
				case TWRAP:
					executeCommandTWRAP();
					break;
				case TBIAS:
					executeCommandTBIAS();
					break;
				case TFUNC:
					executeCommandTFUNC();
					break;
				case TEC:
					executeCommandTEC();
					break;
				case TFLUSH:
					executeCommandTFLUSH();
					break;
				case TSYNC:
					executeCommandTSYNC();
					break;
				case FFAR:
					executeCommandFFAR();
					break;
				case FDIST:
					executeCommandFDIST();
					break;
				case FCOL:
					executeCommandFCOL();
					break;
				case TSLOPE:
					executeCommandTSLOPE();
					break;
				case PSM:
					executeCommandPSM();
					break;
				case CLEAR:
					executeCommandCLEAR();
					break;
				case SCISSOR1:
					executeCommandSCISSOR1();
					break;
				case SCISSOR2:
					executeCommandSCISSOR2();
					break;
				case NEARZ:
					executeCommandNEARZ();
					break;
				case FARZ:
					executeCommandFARZ();
					break;
				case CTST:
					executeCommandCTST();
					break;
				case CREF:
					executeCommandCREF();
					break;
				case CMSK:
					executeCommandCMSK();
					break;
				case ATST:
					executeCommandATST();
					break;
				case STST:
					executeCommandSTST();
					break;
				case SOP:
					executeCommandSOP();
					break;
				case ZTST:
					executeCommandZTST();
					break;
				case ALPHA:
					executeCommandALPHA();
					break;
				case SFIX:
					executeCommandSFIX();
					break;
				case DFIX:
					executeCommandDFIX();
					break;
				case DTH0:
					executeCommandDTH0();
					break;
				case DTH1:
					executeCommandDTH1();
					break;
				case DTH2:
					executeCommandDTH2();
					break;
				case DTH3:
					executeCommandDTH3();
					break;
				case LOP:
					executeCommandLOP();
					break;
				case ZMSK:
					executeCommandZMSK();
					break;
				case PMSKC:
					executeCommandPMSKC();
					break;
				case PMSKA:
					executeCommandPMSKA();
					break;
				case TRXKICK:
					executeCommandTRXKICK();
					break;
				case TRXPOS:
					executeCommandTRXPOS();
					break;
				case TRXDPOS:
					executeCommandTRXDPOS();
					break;
				case TRXSIZE:
					executeCommandTRXSIZE();
					break;
				case VSCX:
					executeCommandVSCX();
					break;
				case VSCY:
					executeCommandVSCY();
					break;
				case VSCZ:
					executeCommandVSCZ();
					break;
				case VTCS:
					executeCommandVTCS();
					break;
				case VTCT:
					executeCommandVTCT();
					break;
				case VTCQ:
					executeCommandVTCQ();
					break;
				case VCV:
					executeCommandVCV();
					break;
				case VAP:
					executeCommandVAP();
					break;
				case VFC:
					executeCommandVFC();
					break;
				case VSCV:
					executeCommandVSCV();
					break;
				case DUMMY:
					executeCommandDUMMY();
					break;
				default:
					executeCommandUNKNOWN();
					break;
			}
			if (DurationStatistics.collectStatistics)
			{
				commandStatistics[command_Renamed].end();
			}
		}

		private void executeCommandUNKNOWN()
		{
			if (isLogWarnEnabled)
			{
				log_Renamed.warn(string.Format("Unknown/unimplemented video command [{0}]{1} at 0x{2:X8}", helper.getCommandString(command_Renamed), getArgumentLog(normalArgument), currentList.Pc - 4));
			}
		}

		private void executeCommandCLEAR()
		{
			if ((normalArgument & 1) == 0)
			{
				if (!context.clearMode)
				{
					return;
				}
				context.clearMode = false;
				re.endClearMode();
				if (isLogDebugEnabled)
				{
					log("clear mode end");
				}
			}
			else
			{
				// TODO Add more disabling in clear mode, we also need to reflect the change to the internal GE registers
				bool color = (normalArgument & 0x100) != 0;
				bool stencil = (normalArgument & 0x200) != 0;
				bool depth = (normalArgument & 0x400) != 0;

				updateGeBuf();
				re.startClearMode(color, stencil, depth);
				context.clearMode = true;
				context.clearModeColor = color;
				context.clearModeStencil = stencil;
				context.clearModeDepth = depth;
				if (isLogDebugEnabled)
				{
					log(string.Format("clear mode: {0:D} ({1}{2}{3})", normalArgument >> 8, color ? "COLOR" : "", stencil ? " STENCIL" : "", depth ? " DEPTH" : ""));
				}
			}

			lightingChanged = true;
			projectionMatrixUpload.Changed = true;
			modelMatrixUpload.Changed = true;
			viewMatrixUpload.Changed = true;
			textureMatrixUpload.Changed = true;
			viewportChanged = true;
			depthChanged = true;
			materialChanged = true;
		}

		private void executeCommandTFUNC()
		{
			context.textureFunc = normalArgument & 0x7;
			if (context.textureFunc >= TFUNC_FRAGMENT_DOUBLE_TEXTURE_EFECT_UNKNOW1)
			{
				// All 3 unknown values have the same function as TFUNC_FRAGMENT_DOUBLE_TEXTURE_EFECT_ADD.
				// Tested on PSP using 3DStudio.
				context.textureFunc = TFUNC_FRAGMENT_DOUBLE_TEXTURE_EFECT_ADD;
			}

			context.textureAlphaUsed = ((normalArgument >> 8) & 0x1) != TFUNC_FRAGMENT_DOUBLE_TEXTURE_COLOR_ALPHA_IS_IGNORED;
			context.textureColorDoubled = ((normalArgument >> 16) & 0x1) != TFUNC_FRAGMENT_DOUBLE_ENABLE_COLOR_UNTOUCHED;

			re.setTextureFunc(context.textureFunc, context.textureAlphaUsed, context.textureColorDoubled);

			if (isLogDebugEnabled)
			{
				log(string.Format("sceGuTexFunc mode {0:X6}", normalArgument) + (((normalArgument & 0x10000) != 0) ? " SCALE" : "") + (((normalArgument & 0x100) != 0) ? " ALPHA" : ""));
			}
		}

		private int fixNativeBufferOffset(Buffer vertexData, int addr, int size)
		{
			// Handle buffer address not aligned with memory Buffer object.
			// E.g. ptr_vertex = 0xNNNNNN2 and vertexData is an IntBuffer
			// starting at 0xNNNNNN0
			int nativeBufferOffset = getBufferOffset(vertexData, addr);
			size += nativeBufferOffset;
			vertexInfoReader.addNativeOffset(nativeBufferOffset);

			return size;
		}

		private int getBufferOffset(Buffer buffer, int addr)
		{
			if (buffer is IntBuffer || buffer is FloatBuffer)
			{
				return addr & 3;
			}
			if (buffer is ShortBuffer)
			{
				return addr & 1;
			}
			return 0;
		}

		private int checkMultiDraw(int currentFirst, int currentType, int currentNumberOfVertex, IntBuffer bufferFirst, IntBuffer bufferCount, bool hasIndex)
		{
			if (avoidDrawElementsWithNonZeroIndexOffset && hasIndex)
			{
				// Multiple drawElements can only mixed into a multi-draw when non-zero index offsets are allowed.
				if (isLogDebugEnabled)
				{
//JAVA TO C# CONVERTER TODO TASK: The following line has a Java format specifier which cannot be directly translated to .NET:
//ORIGINAL LINE: log(String.format("checkMultiDraw hasIndex=%b disabled to avoid non-zero index offsets", hasIndex));
					log(string.Format("checkMultiDraw hasIndex=%b disabled to avoid non-zero index offsets", hasIndex));
				}
				return -1;
			}

			if (isLogDebugEnabled)
			{
				log(string.Format("checkMultiDraw at 0x{0:X8}", currentList.Pc));
			}

			int beforeMultiPc = currentList.Pc;
			int afterMultiPc = currentList.Pc;
			bool hasMultiDraw = false;
			int initialFirst = currentFirst;
			int currentSkip = 0;
			bufferFirst.clear();
			bufferCount.clear();
			int currentPtrVertex = context.vinfo.ptr_vertex + context.vinfo.vertexSize * currentNumberOfVertex;
			bool frontFaceCw = context.frontFaceCw;

			// Leave at least one entry free to put the last item
			while (bufferFirst.remaining() > 1)
			{
				if (currentList.StallReached)
				{
					if (isLogDebugEnabled)
					{
						log_Renamed.debug(string.Format("Stopped integration in MultiDrawArrays at stall address 0x{0:X8}", currentList.Pc));
					}
					break;
				}
				int instruction = currentList.readNextInstruction();

				int cmd = command(instruction);
				if (cmd == PRIM)
				{
					if (context.frontFaceCw != frontFaceCw)
					{
						if (isLogDebugEnabled)
						{
							log_Renamed.debug(string.Format("{0} 0x{1:X6} non matching FFACE has stopped integration in MultiDrawArrays", helper.getCommandString(cmd), intArgument(instruction)));
						}
						break;
					}
					int type = ((instruction >> 16) & 0x7);
					if (type != currentType)
					{
						if (isLogDebugEnabled)
						{
							log_Renamed.debug(string.Format("{0} 0x{1:X6} non matching vertex type has stopped integration in MultiDrawArrays", helper.getCommandString(cmd), intArgument(instruction)));
						}
						break;
					}
					int numberOfVertex = instruction & 0xFFFF;
					bufferFirst.put(currentFirst);
					bufferCount.put(currentNumberOfVertex);
					currentFirst += currentNumberOfVertex + currentSkip;
					currentNumberOfVertex = numberOfVertex;
					currentPtrVertex += context.vinfo.vertexSize * (numberOfVertex + currentSkip);
					currentSkip = 0;
					hasMultiDraw = true;
					afterMultiPc = currentList.Pc;
					if (isLogDebugEnabled)
					{
						log_Renamed.debug(string.Format("{0} type={1:D}, numberOfVertex={2:D} integrated in MultiDrawArrays", helper.getCommandString(cmd), type, numberOfVertex));
					}
				}
				else if (cmd == VADDR)
				{
					int arg = intArgument(instruction);
					int ptr_vertex = currentList.getAddressRelOffset(arg);
					if (ptr_vertex == currentPtrVertex)
					{
						// VADDR in sequence, skip the command
						if (isLogDebugEnabled)
						{
							log_Renamed.debug(string.Format("{0} 0x{1:X8} integrated in MultiDrawArrays", helper.getCommandString(cmd), ptr_vertex));
						}
					}
					else if (ptr_vertex > currentPtrVertex && ((ptr_vertex - currentPtrVertex) % context.vinfo.vertexSize) == 0)
					{
						// VADDR almost in sequence with an aligned hole, skip the command
						currentSkip = (ptr_vertex - currentPtrVertex) / context.vinfo.vertexSize;
						if (isLogDebugEnabled)
						{
							log_Renamed.debug(string.Format("{0} 0x{1:X8} (skip={2:D}) integrated in MultiDrawArrays", helper.getCommandString(cmd), ptr_vertex, currentSkip));
						}
					}
					else
					{
						if (isLogDebugEnabled)
						{
							log_Renamed.debug(string.Format("{0} 0x{1:X8} not integrated in MultiDrawArrays (needed 0x{2:X8})", helper.getCommandString(cmd), ptr_vertex, currentPtrVertex));
						}
						break;
					}
				}
				else if (cmd == TBIAS)
				{
					int tex_mipmap_mode = instruction & 0x3;
					if (context.tex_mipmap_mode == tex_mipmap_mode && tex_mipmap_mode == TBIAS_MODE_AUTO)
					{
						// Skip TBIAS with TBIAS_MODE_AUTO, ignore tex_mipmap_bias parameter
						if (isLogDebugEnabled)
						{
							log_Renamed.debug(string.Format("{0} 0x{1:X6} integrated in MultiDrawArrays", helper.getCommandString(cmd), intArgument(instruction)));
						}
					}
					else
					{
						if (isLogDebugEnabled)
						{
							log_Renamed.debug(string.Format("{0} 0x{1:X6} has stopped integration in MultiDrawArrays", helper.getCommandString(cmd), intArgument(instruction)));
						}
						break;
					}
				}
				else if (cmd == NOP)
				{
					if (isLogDebugEnabled)
					{
						log_Renamed.debug(string.Format("{0} 0x{1:X6} integrated in MultiDrawArrays", helper.getCommandString(cmd), intArgument(instruction)));
					}
				}
				else if (pureStateCommands[cmd])
				{
					if (cmd == FFACE)
					{
						// Some applications generate the following sequence:
						//   FFACE 0
						//   PRIM xxx
						//   FFACE 1
						//   FFACE 0
						//   PRIM xxx
						// Detect such sequences (changing the FFACE with no effect)
						// and integrate them in multiDraw.
						frontFaceCw = intArgument(instruction) != 0;
						if (isLogDebugEnabled)
						{
							log_Renamed.debug(string.Format("{0} 0x{1:X6} trying to integrate in MultiDrawArrays", helper.getCommandString(cmd), intArgument(instruction)));
						}
					}
					else if (currentListCMDValues[cmd] == instruction)
					{
						// The command has been repeated with the same parameters,
						// it can be ignored.
						if (isLogDebugEnabled)
						{
							log_Renamed.debug(string.Format("{0} 0x{1:X6} pure state cmd integrated in MultiDrawArrays", helper.getCommandString(cmd), intArgument(instruction)));
						}
					}
					else
					{
						if (isLogDebugEnabled)
						{
							log_Renamed.debug(string.Format("{0} 0x{1:X6} pure state cmd has stopped integration in MultiDrawArrays", helper.getCommandString(cmd), intArgument(instruction)));
						}
						break;
					}
				}
				else
				{
					if (isLogDebugEnabled)
					{
						log_Renamed.debug(string.Format("{0} 0x{1:X6} has stopped integration in MultiDrawArrays", helper.getCommandString(cmd), intArgument(instruction)));
					}
					break;
				}
			}

			if (!hasMultiDraw)
			{
				currentList.Pc = beforeMultiPc;
				return -1;
			}

			bufferFirst.put(currentFirst);
			bufferCount.put(currentNumberOfVertex);

			bufferFirst.limit(bufferFirst.position());
			bufferFirst.rewind();
			bufferCount.limit(bufferCount.position());
			bufferCount.rewind();

			currentList.Pc = afterMultiPc;

			return currentFirst + currentNumberOfVertex - initialFirst;
		}

		private void checkMultiTrxkick()
		{
			int startPc = currentList.Pc;
			if (isLogDebugEnabled)
			{
				log(string.Format("checkMultiTrxkick from 0x{0:X8}", startPc));
			}

			int lastTrxkick = startPc - 4;
			int countTrxkick = 1; // Count the TRXKICK command that has triggered this check

			while (!currentList.StallReached)
			{
				int instruction = currentList.readNextInstruction();

				int cmd = command(instruction);
				if (cmd == TRXKICK)
				{
					lastTrxkick = currentList.Pc - 4;
					countTrxkick++;
				}
				else if (cmd == TRXPOS || cmd == TRXDPOS || cmd == TRXSIZE || cmd == TRXSBP || cmd == TRXSBW || cmd == TRXDBP || cmd == TRXDBW || cmd == NOP)
				{
					// Continue the check
				}
				else
				{
					// Reaching a non-trxkick command, aborting the check
					if (isLogDebugEnabled)
					{
						log_Renamed.debug(string.Format("{0} 0x{1:X6} has stopped integration in checkMultiTrxkick", helper.getCommandString(cmd), intArgument(instruction)));
					}
					break;
				}
			}

			// Optimize a sequence of TRXKICK commands when there are at least 2 of them
			if (countTrxkick > 2)
			{
				multiTrxkickStart = startPc - 4;
				multiTrxkickEnd = lastTrxkick;
				if (isLogDebugEnabled)
				{
					log_Renamed.debug(string.Format("checkMultiTrxkick start=0x{0:X8}, end=0x{1:X8}, countTrxkick={2:D}", multiTrxkickStart, multiTrxkickEnd, countTrxkick));
				}
			}
			else
			{
				multiTrxkickStart = -1;
				multiTrxkickEnd = -1;
				if (isLogDebugEnabled)
				{
					log_Renamed.debug(string.Format("checkMultiTrxkick failed, countTrxkick={0:D}", countTrxkick));
				}
			}

			// Reset the PC to its initial value
			currentList.Pc = startPc;
		}

		private VertexIndexInfo getVertexIndexInfo(int bytesPerIndex, int numberOfVertex)
		{
			int maxIndex = -1;
			int minIndex = int.MaxValue;
			int indexBufferSize = numberOfVertex * bytesPerIndex;
			bool sequence = true;
			int previousIndex = -1;
			IMemoryReader memoryReader = MemoryReader.getMemoryReader(context.vinfo.ptr_index, indexBufferSize, bytesPerIndex);
			for (int i = 0; i < numberOfVertex; i++)
			{
				int index = memoryReader.readNext();
				maxIndex = max(maxIndex, index);
				minIndex = min(minIndex, index);
				if (i > 0)
				{
					// Are the indices all in sequence?
					if (index != previousIndex + 1)
					{
						sequence = false;
					}
				}
				previousIndex = index;
			}

			if (isLogDebugEnabled)
			{
//JAVA TO C# CONVERTER TODO TASK: The following line has a Java format specifier which cannot be directly translated to .NET:
//ORIGINAL LINE: System.Console.WriteLine(String.format("getIndexedNumberOfVertexInfo %d: [%d..%d], sequence %b", numberOfVertex, minIndex, maxIndex, sequence));
				log_Renamed.debug(string.Format("getIndexedNumberOfVertexInfo %d: [%d..%d], sequence %b", numberOfVertex, minIndex, maxIndex, sequence));
			}

			return new VertexIndexInfo(minIndex, maxIndex, sequence);
		}

		private bool isVertexDiscarded(float[] mvpMatrix, float[] p)
		{
			// Apply the MVP transformation to position coordinates
			float[] mvpPosition = new float[4];
			float[] vertexPosition = new float[4];
			vertexPosition[0] = p[0];
			vertexPosition[1] = p[1];
			vertexPosition[2] = p[2];
			vertexPosition[3] = 1f;
			vectorMult44(mvpPosition, mvpMatrix, vertexPosition);
			float invertedW = 1f / mvpPosition[3];
			float xs = mvpPosition[0] * invertedW * context.viewport_width + context.viewport_cx;
			float ys = mvpPosition[1] * invertedW * context.viewport_height + context.viewport_cy;
			float zs = mvpPosition[2] * invertedW * context.zscale + context.zpos;

			bool discarded = false;
			if (xs < 0f || xs >= 4096f || ys < 0f || ys >= 4096f)
			{
				discarded = true;
			}
			else if (!context.clipPlanesFlag.Enabled)
			{
				if (zs < 0f || zs >= 65536f)
				{
					discarded = true;
				}
			}

			if (isLogDebugEnabled)
			{
//JAVA TO C# CONVERTER TODO TASK: The following line has a Java format specifier which cannot be directly translated to .NET:
//ORIGINAL LINE: System.Console.WriteLine(String.format("isVertexDiscarded (%.0f,%.0f,%.0f) %s=%b returning %b", xs, ys, zs, context.clipPlanesFlag.toString(), context.clipPlanesFlag.isEnabled(), discarded));
				log_Renamed.debug(string.Format("isVertexDiscarded (%.0f,%.0f,%.0f) %s=%b returning %b", xs, ys, zs, context.clipPlanesFlag.ToString(), context.clipPlanesFlag.Enabled, discarded));
			}

			return discarded;
		}

		private void executeCommandPRIM()
		{
			int numberOfVertex = normalArgument & 0xFFFF;
			int type = (normalArgument >> 16) & 0x7;

			if (numberOfVertex == 0)
			{
				return;
			}

			if (!Memory.isAddressGood(context.vinfo.ptr_vertex))
			{
				// Abort here to avoid a lot of useless memory read errors...
				error(string.Format("{0}: Invalid vertex address 0x{1:X8}", helper.getCommandString(PRIM), context.vinfo.ptr_vertex));
				return;
			}

			if (type == PRIM_LINE || type == PRIM_LINES_STRIPS)
			{
				if (context.lineSmoothFlag.Enabled && context.textureFunc == TFUNC_FRAGMENT_DOUBLE_TEXTURE_EFECT_REPLACE)
				{
					if (isLogDebugEnabled)
					{
						log_Renamed.debug(string.Format("The drawing of antialiasing lines is not supported, discarding them"));
					}
					endRendering(numberOfVertex);
					return;
				}
			}

			if (context.textureFlag.Enabled && !context.clearMode)
			{
				int textureAddr = context.texture_base_pointer[0] & Memory.addressMask;
				if (textureAddr > MemoryMap.END_VRAM && textureAddr < MemoryMap.START_VRAM + 0x800000)
				{
					if (isLogWarnEnabled)
					{
						log_Renamed.warn(string.Format("Texture in swizzled VRAM not supported 0x{0:X8}", textureAddr));
					}
					endRendering(numberOfVertex);
					return;
				}
			}

			if (type == PRIM_CONTINUE_PREVIOUS_PRIM)
			{
				// The PSP is continuing the previous PRIM command.
				// If the previous PRIM was a strip or a fan, the strip/fan is
				// continued as it was part of the previous PRIM command.
				switch (previousPrim)
				{
					case PRIM_LINES_STRIPS:
					case PRIM_TRIANGLE_STRIPS:
					case PRIM_TRIANGLE_FANS:
						// Continuing the previous strip/fan is not implemented.
						if (isLogWarnEnabled)
						{
							log_Renamed.warn(string.Format("PRIM type=7 cannot continue previous strip/fan ({0:D})", previousPrim));
						}
						break;
				}
				type = previousPrim;
			}
			previousPrim = type;

			if (numberOfVertex < minimumNumberOfVertex[type])
			{
				if (isLogDebugEnabled)
				{
					log_Renamed.debug(string.Format("{0} type {1:D} unsufficient number of vertex {2:D}", helper.getCommandString(PRIM), type, numberOfVertex));
				}
				endRendering(numberOfVertex);
				return;
			}

			if (skipThisFrame)
			{
				endRendering(numberOfVertex);
				return;
			}

			updateGeBuf();
			somethingDisplayed = true;
			primCount++;
			if (isGeProfilerEnabled)
			{
				GEProfiler.startGeCmd(PRIM);
			}

			loadTexture();

			// Logging
			if (isLogDebugEnabled)
			{
				switch (type)
				{
					case PRIM_POINT:
						log(string.Format("prim {0:D} point ({1:D} vertices)", numberOfVertex, numberOfVertex));
						break;
					case PRIM_LINE:
						log(string.Format("prim {0:D} line ({1:D} vertices)", numberOfVertex / 2, numberOfVertex));
						break;
					case PRIM_LINES_STRIPS:
						log(string.Format("prim {0:D} line strips ({1:D} vertices)", numberOfVertex - 1, numberOfVertex));
						break;
					case PRIM_TRIANGLE:
						log(string.Format("prim {0:D} triangle ({1:D} vertices)", numberOfVertex / 3, numberOfVertex));
						break;
					case PRIM_TRIANGLE_STRIPS:
						log(string.Format("prim {0:D} triangle strips ({1:D} vertices)", numberOfVertex - 2, numberOfVertex));
						break;
					case PRIM_TRIANGLE_FANS:
						log(string.Format("prim {0:D} triangle fans ({1:D} vertices)", numberOfVertex - 2, numberOfVertex));
						break;
					case PRIM_SPRITES:
						log(string.Format("prim {0:D} sprites ({1:D} vertices)", numberOfVertex / 2, numberOfVertex));
						break;
				}
			}

			Memory mem = Memory.Instance;
			initRendering();

			int nTexCoord = 2;
			int nColor = 4;
			int nVertex = 3;

			bool useTexture = false;
			bool useTextureFromNormal = false;
			bool useTextureFromNormalizedNormal = false;
			bool useTextureFromPosition = false;
			// Use the texture from the vertex only is the texture flag is enabled or
			// if the use of VAO is enabled
			// (a VAO depends only on the vinfo.vtype structure, not on the texture flag setting)
			if (context.textureFlag.Enabled || re.VertexArrayAvailable)
			{
				if (context.vinfo.transform2D)
				{
					// 2D is always using UV-mapping
					if (context.vinfo.texture != 0)
					{
						useTexture = true;
					}
				}
				else
				{
					switch (context.tex_map_mode)
					{
						case TMAP_TEXTURE_MAP_MODE_TEXTURE_COORDIATES_UV:
							if (context.vinfo.texture != 0)
							{
								useTexture = true;
							}
							break;

						case TMAP_TEXTURE_MAP_MODE_TEXTURE_MATRIX:
						{
							switch (context.tex_proj_map_mode)
							{
								case TMAP_TEXTURE_PROJECTION_MODE_POSITION:
									if (context.vinfo.position != 0)
									{
										useTexture = true;
										useTextureFromPosition = true;
										nTexCoord = nVertex;
									}
									break;
								case TMAP_TEXTURE_PROJECTION_MODE_TEXTURE_COORDINATES:
									if (context.vinfo.texture != 0)
									{
										useTexture = true;
									}
									break;
								case TMAP_TEXTURE_PROJECTION_MODE_NORMAL:
									if (context.vinfo.normal != 0)
									{
										useTexture = true;
										useTextureFromNormal = true;
										nTexCoord = 3;
									}
									break;
								case TMAP_TEXTURE_PROJECTION_MODE_NORMALIZED_NORMAL:
									if (context.vinfo.normal != 0)
									{
										useTexture = true;
										useTextureFromNormalizedNormal = true;
										nTexCoord = 3;
									}
									break;
							}
							break;
						}

						case TMAP_TEXTURE_MAP_MODE_ENVIRONMENT_MAP:
							break;

						default:
							log_Renamed.warn(string.Format("Unhandled texture matrix mode {0:D}", context.tex_map_mode));
							break;
					}
				}
			}

			vertexStatistics.start();

			context.vinfo.MorphWeights = context.morph_weight;
			context.vinfo.setDirty();

			int numberOfWeightsForBuffer;
			bool mustComputeWeights;
			if (context.vinfo.weight != 0)
			{
				numberOfWeightsForBuffer = re.setBones(context.vinfo.skinningWeightCount, context.boneMatrixLinear);
				mustComputeWeights = (numberOfWeightsForBuffer == 0);
			}
			else
			{
				numberOfWeightsForBuffer = re.setBones(0, null);
				mustComputeWeights = false;
			}

			if (context.scissor_x2 < maxSpriteWidth)
			{
				maxSpriteWidth = context.scissor_x2;
			}
			if (context.scissor_y2 < maxSpriteHeight)
			{
				maxSpriteHeight = context.scissor_y2;
			}

			bool needSetDataPointers = true;

			if (re.canReadAllVertexInfo())
			{
				// The rendering engine can read the vertex infos by himself.
				re.setVertexInfo(context.vinfo, true, context.useVertexColor, useTexture, type);
				re.drawArrays(type, 0, numberOfVertex);
			}
			else
			{
				// In clear mode STENCIL, set the stencil value to the alpha value of the rendered color
				if (context.clearMode && context.clearModeStencil && context.vinfo.color != 0)
				{
					// Retrieve the alpha value of the 1st vertex
					// (2nd vertex for a SPRITE as this is the vertex setting the rendered color for a SPRITE).
					int addr = context.vinfo.getAddress(mem, type == PRIM_SPRITES ? 1 : 0);
					context.vinfo.readVertex(mem, addr, v, false, DoubleTexture2DCoords);
					float alpha = v.c[3];
					int stencilValue = PixelColor.getColor(alpha);
					re.setStencilFunc(STST_FUNCTION_ALWAYS_PASS_STENCIL_TEST, stencilValue, 0xFF);
					re.setStencilOp(SOP_KEEP_STENCIL_VALUE, SOP_KEEP_STENCIL_VALUE, SOP_REPLACE_STENCIL_VALUE);
				}

				// Do not use optimized VertexInfo reading when
				// - disableOptimizedVertexInfoReading is true
				// - using Vertex Cache unless all the vertices are supported natively
				// - the Vertex are indexed
				// - the PRIM_SPRITE primitive is used where it is not supported natively
				// - the normals have to be normalized for the texture mapping
				// - the weights have to be computed and are not supported natively
				// - the vertex address is invalid
				if ((!useVertexCache || re.canAllNativeVertexInfo()) && context.vinfo.morphingVertexCount == 1 && (type != PRIM_SPRITES || re.canNativeSpritesPrimitive()) && !useTextureFromNormalizedNormal && !mustComputeWeights && Memory.isAddressGood(context.vinfo.ptr_vertex) && !disableOptimizedVertexInfoReading())
				{
					//
					// Optimized VertexInfo reading:
					// - do not copy the info already available in the OpenGL format
					//   (native format), load it into nativeBuffer (a direct buffer
					//   is required by OpenGL).
					// - try to keep the info in "int" format when possible, convert
					//   to "float" only when necessary
					// The best case is no reading and no conversion at all when all the
					// vertex info are available in a format usable by OpenGL.
					//
					int numberOfVertexInfo = numberOfVertex;
					int bytesPerIndex = VertexInfo.size_mapping[context.vinfo.index];
					long indicesBufferOffset = 0;
					int firstVertexInfo = 0;
					int firstVertex = 0;
					bool hasIndex = context.vinfo.index != 0;
					if (hasIndex)
					{
						int indexBufferSize = numberOfVertex * bytesPerIndex;
						VertexIndexInfo vertexIndexInfo = getVertexIndexInfo(bytesPerIndex, numberOfVertex);
						numberOfVertexInfo = vertexIndexInfo.NumberOfVertex;
						firstVertexInfo = vertexIndexInfo.MinIndex;

						// No need to use indexed vertices when all the index are in sequence!
						// Disable the index in such cases.
						hasIndex = !vertexIndexInfo.Sequence;

						if (hasIndex)
						{
							Buffer indicesBuffer = mem.getBuffer(context.vinfo.ptr_index, indexBufferSize);
							indicesBufferOffset = getBufferOffset(indicesBuffer, context.vinfo.ptr_index);

							//
							// The AMD/ATI driver seems to have problems using glDrawElements with a non-zero index offset.
							// Provide a work-aRound by copying the indices buffer into a byte buffer where the correct
							// index offset can be set. The index offset passed to glDrawElements can then be set to 0.
							//
							if (avoidDrawElementsWithNonZeroIndexOffset && indicesBufferOffset != 0)
							{
								indexByteBuffer.clear();
								Utilities.putBuffer(indexByteBuffer, indicesBuffer, ByteOrder.LITTLE_ENDIAN);
								indexByteBuffer.limit(indexBufferSize + (int) indicesBufferOffset);
								indexByteBuffer.position((int) indicesBufferOffset);
								indicesBuffer = indexByteBuffer;
								indicesBufferOffset = 0;
							}

							bufferManager.setBufferSubData(pspsharp.graphics.RE.IRenderingEngine_Fields.RE_ELEMENT_ARRAY_BUFFER, indexBufferId, 0, indexBufferSize + (int) indicesBufferOffset, indicesBuffer, pspsharp.graphics.RE.IRenderingEngine_Fields.RE_DYNAMIC_DRAW);
						}
						else
						{
							firstVertex = firstVertexInfo;
						}
					}

					vertexReadingStatistics.start();
					Buffer buffer = vertexInfoReader.read(context.vinfo, context.vinfo.ptr_vertex, firstVertexInfo, numberOfVertexInfo, re.canAllNativeVertexInfo());
					vertexReadingStatistics.end();

					int stride;
					int size = context.vinfo.vertexSize * numberOfVertexInfo;
					bool useBufferManager;
					bool multiDrawArrays = false;
					if (useVertexCache && buffer == null)
					{
						stride = context.vinfo.vertexSize;
						useBufferManager = false;
						int vertexAddress = context.vinfo.ptr_vertex + firstVertexInfo * context.vinfo.vertexSize;
						VertexBuffer vertexBuffer = VertexBufferManager.Instance.getVertexBuffer(re, vertexAddress, size, stride, re.VertexArrayAvailable);
						Buffer vertexData = mem.getBuffer(vertexAddress, size);
						vertexBuffer.load(re, vertexData, vertexAddress, size);
						int multiDrawFirstVertex = 0;
						// Don't try to mix VAO's with indexed vertices...
						if (re.VertexArrayAvailable && firstVertex == 0)
						{
							VertexArray vertexArray = VertexArrayManager.Instance.getVertexArray(re, context.vinfo.vtype, vertexBuffer, vertexAddress, stride);
							needSetDataPointers = vertexArray.bind(re);
							multiDrawFirstVertex = vertexArray.getVertexOffset(vertexAddress);
						}
						else
						{
							// add buffer offset relative to vinfo.ptr_vertex (and not relative to vertexAddress)
							vertexInfoReader.addNativeOffset(vertexBuffer.getBufferOffset(context.vinfo.ptr_vertex));
						}

						// Check if multiple PRIM's are defined in sequence and
						// try to merge them into a single multiDrawArrays call.
						int multiDrawNumberOfVertex = checkMultiDraw(multiDrawFirstVertex, type, numberOfVertex, multiDrawFirst, multiDrawCount, context.vinfo.index != 0);
						if (multiDrawNumberOfVertex > 0)
						{
							firstVertex = multiDrawFirstVertex;
							multiDrawArrays = true;
							numberOfVertex = multiDrawNumberOfVertex;
							if (context.vinfo.index != 0)
							{
								// Reload the now extended buffer for indices
								VertexIndexInfo vertexIndexInfo = getVertexIndexInfo(bytesPerIndex, multiDrawNumberOfVertex);
								numberOfVertexInfo = vertexIndexInfo.NumberOfVertex;
								firstVertexInfo = vertexIndexInfo.MinIndex;

								// No need to use indexed vertices when all the index are in sequence!
								// Disable the index in such cases.
								hasIndex = !vertexIndexInfo.Sequence;

								if (hasIndex)
								{
									int indexBufferSize = multiDrawNumberOfVertex * bytesPerIndex;
									Buffer indicesBuffer = mem.getBuffer(context.vinfo.ptr_index, indexBufferSize);
									indicesBufferOffset = getBufferOffset(indicesBuffer, context.vinfo.ptr_index);
									bufferManager.setBufferSubData(pspsharp.graphics.RE.IRenderingEngine_Fields.RE_ELEMENT_ARRAY_BUFFER, indexBufferId, 0, indexBufferSize + (int) indicesBufferOffset, indicesBuffer, pspsharp.graphics.RE.IRenderingEngine_Fields.RE_DYNAMIC_DRAW);
								}

								vertexAddress = context.vinfo.ptr_vertex + firstVertexInfo * context.vinfo.vertexSize;
								size = context.vinfo.vertexSize * numberOfVertexInfo;
							}
							else
							{
								size = context.vinfo.vertexSize * multiDrawNumberOfVertex;
							}
							vertexData = mem.getBuffer(vertexAddress, size);
							vertexBuffer.load(re, vertexData, vertexAddress, size);
						}
						else if (multiDrawFirstVertex > 0)
						{
							// The VAO requires an updated first vertex
							firstVertex = multiDrawFirstVertex;
						}

						if (needSetDataPointers)
						{
							vertexBuffer.bind(re);
						}
					}
					else
					{
						if (re.VertexArrayAvailable)
						{
							re.bindVertexArray(0);
						}
						stride = vertexInfoReader.Stride;
						useBufferManager = true;

						if (buffer != null)
						{
							bufferManager.setBufferSubData(pspsharp.graphics.RE.IRenderingEngine_Fields.RE_ARRAY_BUFFER, bufferId, firstVertexInfo * stride, stride * numberOfVertexInfo, buffer, pspsharp.graphics.RE.IRenderingEngine_Fields.RE_STREAM_DRAW);
						}

						if (vertexInfoReader.hasNative())
						{
							// Copy the VertexInfo from Memory to the nativeBuffer
							// (a direct buffer is required by glXXXPointer())
							int vertexAddr = context.vinfo.ptr_vertex + firstVertexInfo * context.vinfo.vertexSize;
							Buffer vertexData = mem.getBuffer(vertexAddr, size);
							size = fixNativeBufferOffset(vertexData, vertexAddr, size);
							bufferManager.setBufferSubData(pspsharp.graphics.RE.IRenderingEngine_Fields.RE_ARRAY_BUFFER, nativeBufferId, firstVertexInfo * context.vinfo.vertexSize, size, vertexData, pspsharp.graphics.RE.IRenderingEngine_Fields.RE_STREAM_DRAW);
						}
					}

					re.setVertexInfo(context.vinfo, re.canAllNativeVertexInfo(), context.useVertexColor, useTexture, type);

					if (needSetDataPointers)
					{
						if (hasIndex)
						{
							bufferManager.bindBuffer(pspsharp.graphics.RE.IRenderingEngine_Fields.RE_ELEMENT_ARRAY_BUFFER, indexBufferId);
						}
						if (useTexture)
						{
							bool textureNative;
							int textureOffset;
							int textureType;
							if (useTextureFromNormal)
							{
								textureNative = vertexInfoReader.NormalNative;
								textureOffset = vertexInfoReader.NormalOffset;
								textureType = vertexInfoReader.NormalType;
								nTexCoord = vertexInfoReader.NormalNumberValues;
							}
							else if (useTextureFromPosition)
							{
								textureNative = vertexInfoReader.PositionNative;
								textureOffset = vertexInfoReader.PositionOffset;
								textureType = vertexInfoReader.PositionType;
								nTexCoord = vertexInfoReader.PositionNumberValues;
							}
							else
							{
								textureNative = vertexInfoReader.TextureNative;
								textureOffset = vertexInfoReader.TextureOffset;
								textureType = vertexInfoReader.TextureType;
								nTexCoord = vertexInfoReader.TextureNumberValues;
							}
							setTexCoordPointer(useTexture, nTexCoord, textureType, stride, textureOffset, textureNative, useBufferManager);
						}
						nVertex = vertexInfoReader.PositionNumberValues;
						nColor = vertexInfoReader.ColorNumberValues;
						int nWeight = vertexInfoReader.WeightNumberValues;

						enableClientState(context.useVertexColor, useTexture);
						setColorPointer(context.useVertexColor, nColor, vertexInfoReader.ColorType, stride, vertexInfoReader.ColorOffset, vertexInfoReader.ColorNative, useBufferManager);
						setNormalPointer(vertexInfoReader.NormalType, stride, vertexInfoReader.NormalOffset, vertexInfoReader.NormalNative, useBufferManager);
						setWeightPointer(nWeight, vertexInfoReader.WeightType, stride, vertexInfoReader.WeightOffset, vertexInfoReader.WeightNative, useBufferManager);
						setVertexPointer(nVertex, vertexInfoReader.PositionType, stride, vertexInfoReader.PositionOffset, vertexInfoReader.PositionNative, useBufferManager);
					}

					if (isLogDebugEnabled)
					{
						if (!hasIndex && context.vinfo.index != 0)
						{
							log_Renamed.debug("Indexed vertex has been disabled, all the indices were sequential");
						}
					}

					drawArraysStatistics.start();
					if (hasIndex)
					{
						if (multiDrawArrays)
						{
							re.multiDrawElements(type, multiDrawFirst, multiDrawCount, indexTypes[context.vinfo.index], indicesBufferOffset);
						}
						else
						{
							re.drawElements(type, numberOfVertex, indexTypes[context.vinfo.index], indicesBufferOffset);
						}
					}
					else if (multiDrawArrays)
					{
						re.multiDrawArrays(type, multiDrawFirst, multiDrawCount);
					}
					else
					{
						re.drawArrays(type, firstVertex, numberOfVertex);
					}
					drawArraysStatistics.end();
				}
				else
				{
					// Non-optimized VertexInfo reading
					VertexInfo cachedVertexInfo = null;
					if (useVertexCache)
					{
						vertexCacheLookupStatistics.start();
						cachedVertexInfo = VertexCache.Instance.getVertex(context.vinfo, numberOfVertex, context.bone_uploaded_matrix, numberOfWeightsForBuffer);
						vertexCacheLookupStatistics.end();
					}

					if (!useVertexCache && re.VertexArrayAvailable)
					{
						re.bindVertexArray(0);
					}

					bool readTexture = context.textureFlag.Enabled && !context.clearMode;
					if (useVertexCache)
					{
						// When using the vertex cache, do not try to optimize the texture reading.
						// The cached vertex could be reused in other situations where the texture
						// is required.
						readTexture = true;
					}

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'sealed override':
//ORIGINAL LINE: sealed override bool needToDiscardVertices = !context.vinfo.transform2D && !re.canDiscardVertices();
					bool needToDiscardVertices = !context.vinfo.transform2D && !re.canDiscardVertices();
					// Pre-compute the MVP (Model-View-Projection) matrix.
					// It is used in case we need to discard vertices or with 3D sprites for the texture flip.
					float[] mvpMatrix = null;
					if (cachedVertexInfo == null && (needToDiscardVertices || (!context.vinfo.transform2D && type == PRIM_SPRITES)))
					{
						mvpMatrix = new float[4 * 4];
						matrixMult(mvpMatrix, context.model_uploaded_matrix, context.view_uploaded_matrix);
						matrixMult(mvpMatrix, mvpMatrix, context.proj_uploaded_matrix);
					}

					switch (type)
					{
						case PRIM_POINT:
						case PRIM_LINE:
						case PRIM_LINES_STRIPS:
						case PRIM_TRIANGLE:
						case PRIM_TRIANGLE_STRIPS:
						case PRIM_TRIANGLE_FANS:
							re.setVertexInfo(context.vinfo, false, context.useVertexColor, useTexture, type);

							if (cachedVertexInfo == null)
							{
								vertexReadingStatistics.start();
								int ii = 0;
								for (int i = 0; i < numberOfVertex; i++)
								{
									int addr = context.vinfo.getAddress(mem, i);

									context.vinfo.readVertex(mem, addr, v, readTexture, DoubleTexture2DCoords);

									// Do skinning first as it modifies v.p and v.n
									if (mustComputeWeights && context.vinfo.position != 0)
									{
										doSkinning(context.bone_uploaded_matrix, context.vinfo, v);
									}

									if (needToDiscardVertices)
									{
										// TODO Check if the vertex needs to be discarded. The effect of discarding one vertex depends on the prim type.
										//bool discard = isVertexDiscarded(mvpMatrix, v.p);
										//if (isLogDebugEnabled) {
										//	log(String.format("Vertex#%d discard=%b", i, discard));
										//}
									}

									// Texture
									if (useTextureFromNormal)
									{
										floatBufferArray[ii++] = v.n[0];
										floatBufferArray[ii++] = v.n[1];
										floatBufferArray[ii++] = v.n[2];
									}
									else if (useTextureFromNormalizedNormal)
									{
										float normalLength = (float) System.Math.Sqrt(v.n[0] * v.n[0] + v.n[1] * v.n[1] + v.n[2] * v.n[2]);
										floatBufferArray[ii++] = v.n[0] / normalLength;
										floatBufferArray[ii++] = v.n[1] / normalLength;
										floatBufferArray[ii++] = v.n[2] / normalLength;
									}
									else if (useTextureFromPosition)
									{
										floatBufferArray[ii++] = v.p[0];
										floatBufferArray[ii++] = v.p[1];
										floatBufferArray[ii++] = v.p[2];
									}
									else if (useTexture || context.vinfo.texture != 0)
									{
										floatBufferArray[ii++] = v.t[0];
										floatBufferArray[ii++] = v.t[1];
									}
									// Color
									if (context.useVertexColor)
									{
										floatBufferArray[ii++] = v.c[0];
										floatBufferArray[ii++] = v.c[1];
										floatBufferArray[ii++] = v.c[2];
										floatBufferArray[ii++] = v.c[3];
									}
									// Normal
									if (context.vinfo.normal != 0)
									{
										floatBufferArray[ii++] = v.n[0];
										floatBufferArray[ii++] = v.n[1];
										floatBufferArray[ii++] = v.n[2];
									}
									// Position
									if (context.vinfo.position != 0)
									{
										floatBufferArray[ii++] = v.p[0];
										floatBufferArray[ii++] = v.p[1];
										floatBufferArray[ii++] = v.p[2];
									}
									// Weights
									if (numberOfWeightsForBuffer > 0)
									{
										for (int j = 0; j < numberOfWeightsForBuffer; j++)
										{
											floatBufferArray[ii++] = v.boneWeights[j];
										}
									}

									if (isLogTraceEnabled)
									{
										if (context.vinfo.texture != 0 && context.vinfo.position != 0)
										{
											log_Renamed.trace("  vertex#" + i + " (" + ((int) v.t[0]) + "," + ((int) v.t[1]) + ") at (" + ((int) v.p[0]) + "," + ((int) v.p[1]) + "," + ((int) v.p[2]) + ")");
										}
									}
								}
								int bufferSizeInFloats = ii;
								vertexReadingStatistics.end();

								if (useVertexCache)
								{
									cachedVertexInfo = new VertexInfo(context.vinfo);
									VertexCache.Instance.addVertex(re, cachedVertexInfo, numberOfVertex, context.bone_uploaded_matrix, numberOfWeightsForBuffer);
									needSetDataPointers = cachedVertexInfo.loadVertex(re, floatBufferArray, bufferSizeInFloats);
								}
								else
								{
									ByteBuffer byteBuffer = bufferManager.getBuffer(bufferId);
									byteBuffer.clear();
									byteBuffer.asFloatBuffer().put(floatBufferArray, 0, bufferSizeInFloats);
									bufferManager.setBufferSubData(pspsharp.graphics.RE.IRenderingEngine_Fields.RE_ARRAY_BUFFER, bufferId, 0, bufferSizeInFloats * SIZEOF_FLOAT, byteBuffer, pspsharp.graphics.RE.IRenderingEngine_Fields.RE_STREAM_DRAW);
								}
							}
							else
							{
								if (isLogDebugEnabled)
								{
									log_Renamed.debug("Reusing cached Vertex Data");
								}
								needSetDataPointers = cachedVertexInfo.bindVertex(re);
							}
							if (needSetDataPointers)
							{
								setDataPointers(nVertex, context.useVertexColor, nColor, useTexture, nTexCoord, context.vinfo.normal != 0, numberOfWeightsForBuffer, cachedVertexInfo == null);
							}
							drawArraysStatistics.start();
							re.drawArrays(type, 0, numberOfVertex);
							drawArraysStatistics.end();
							maxSpriteHeight = int.MaxValue;
							maxSpriteWidth = int.MaxValue;
							break;

						case PRIM_SPRITES:
							re.setVertexInfo(context.vinfo, false, context.useVertexColor, useTexture, pspsharp.graphics.RE.IRenderingEngine_Fields.RE_QUADS);
							if (!context.clearMode)
							{
								re.disableFlag(pspsharp.graphics.RE.IRenderingEngine_Fields.GU_CULL_FACE);
							}

							if (cachedVertexInfo == null)
							{
								vertexReadingStatistics.start();
								int ii = 0;
								for (int i = 0; i < numberOfVertex; i += 2)
								{
									int addr1 = context.vinfo.getAddress(mem, i);
									int addr2 = context.vinfo.getAddress(mem, i + 1);
									context.vinfo.readVertex(mem, addr1, v1, readTexture, DoubleTexture2DCoords);
									context.vinfo.readVertex(mem, addr2, v2, readTexture, DoubleTexture2DCoords);

									v1.p[2] = v2.p[2];

									// In 3D, check if one of the vertex is discarded
									if (needToDiscardVertices)
									{
										bool discarded1 = isVertexDiscarded(mvpMatrix, v1.p);
										bool discarded2 = isVertexDiscarded(mvpMatrix, v2.p);

										// Discard the sprite if one of its vertex is discarded
										if (discarded1 || discarded2)
										{
											if (isLogDebugEnabled)
											{
//JAVA TO C# CONVERTER TODO TASK: The following line has a Java format specifier which cannot be directly translated to .NET:
//ORIGINAL LINE: log(String.format("sprite discarded %b, %b", discarded1, discarded2));
												log(string.Format("sprite discarded %b, %b", discarded1, discarded2));
											}
											continue;
										}
									}

									if (v2.p[1] > maxSpriteHeight)
									{
										maxSpriteHeight = (int) v2.p[1];
									}
									if (v2.p[1] > maxSpriteWidth)
									{
										maxSpriteWidth = (int) v2.p[1];
									}

									//
									// Texture flip tested using the GElist application:
									// - it depends on the X and Y coordinates:
									//   GU_TRANSFORM_3D:
									//     X1 < X2 && Y1 < Y2 :     flipped
									//     X1 > X2 && Y1 > Y2 :     flipped
									//     X1 < X2 && Y1 > Y2 : not flipped
									//     X1 > X2 && Y1 < Y2 : not flipped
									//   GU_TRANSFORM_2D: opposite results because
									//                    the Y-Axis is upside-down in 2D
									//     X1 < X2 && Y1 < Y2 : not flipped
									//     X1 > X2 && Y1 > Y2 : not flipped
									//     X1 < X2 && Y1 > Y2 :     flipped
									//     X1 > X2 && Y1 < Y2 :     flipped
									// - the tests for GU_TRANSFORM_3D are based on the coordinates
									//   after the MVP (Model-View-Projection) transformation
									// - texture coordinates are irrelevant
									//
									float x1, y1, x2, y2;
									if (mvpMatrix == null)
									{
										x1 = v1.p[0];
										y1 = -v1.p[1]; // Y-Axis is upside-down in 2D
										x2 = v2.p[0];
										y2 = -v2.p[1]; // Y-Axis is upside-down in 2D
									}
									else
									{
										// Apply the MVP transformation to both position coordinates
										float[] mvpPosition = new float[2];
										vectorMult(mvpPosition, mvpMatrix, v1.p);
										x1 = mvpPosition[0];
										y1 = mvpPosition[1];
										vectorMult(mvpPosition, mvpMatrix, v2.p);
										x2 = mvpPosition[0];
										y2 = mvpPosition[1];
									}
									bool flippedTexture = (y1 < y2 && x1 < x2) || (y1 > y2 && x1 > x2);

									if (isLogDebugEnabled)
									{
										log(string.Format("  sprite ({0:F0},{1:F0})-({2:F0},{3:F0}) at ({4:F0},{5:F0},{6:F0})-({7:F0},{8:F0},{9:F0}){10}", v1.t[0], v1.t[1], v2.t[0], v2.t[1], v1.p[0], v1.p[1], v1.p[2], v2.p[0], v2.p[1], v2.p[2], flippedTexture ? " flipped" : ""));
									}

									// V1
									if (context.vinfo.texture != 0)
									{
										floatBufferArray[ii++] = v1.t[0];
										floatBufferArray[ii++] = v1.t[1];
									}
									if (context.useVertexColor)
									{
										floatBufferArray[ii++] = v2.c[0];
										floatBufferArray[ii++] = v2.c[1];
										floatBufferArray[ii++] = v2.c[2];
										floatBufferArray[ii++] = v2.c[3];
									}
									if (context.vinfo.normal != 0)
									{
										floatBufferArray[ii++] = v2.n[0];
										floatBufferArray[ii++] = v2.n[1];
										floatBufferArray[ii++] = v2.n[2];
									}
									if (context.vinfo.position != 0)
									{
										floatBufferArray[ii++] = v1.p[0];
										floatBufferArray[ii++] = v1.p[1];
										floatBufferArray[ii++] = v1.p[2];
									}

									if (context.vinfo.texture != 0)
									{
										if (flippedTexture)
										{
											floatBufferArray[ii++] = v2.t[0];
											floatBufferArray[ii++] = v1.t[1];
										}
										else
										{
											floatBufferArray[ii++] = v1.t[0];
											floatBufferArray[ii++] = v2.t[1];
										}
									}
									if (context.useVertexColor)
									{
										floatBufferArray[ii++] = v2.c[0];
										floatBufferArray[ii++] = v2.c[1];
										floatBufferArray[ii++] = v2.c[2];
										floatBufferArray[ii++] = v2.c[3];
									}
									if (context.vinfo.normal != 0)
									{
										floatBufferArray[ii++] = v2.n[0];
										floatBufferArray[ii++] = v2.n[1];
										floatBufferArray[ii++] = v2.n[2];
									}
									if (context.vinfo.position != 0)
									{
										floatBufferArray[ii++] = v1.p[0];
										floatBufferArray[ii++] = v2.p[1];
										floatBufferArray[ii++] = v2.p[2];
									}

									// V2
									if (context.vinfo.texture != 0)
									{
										floatBufferArray[ii++] = v2.t[0];
										floatBufferArray[ii++] = v2.t[1];
									}
									if (context.useVertexColor)
									{
										floatBufferArray[ii++] = v2.c[0];
										floatBufferArray[ii++] = v2.c[1];
										floatBufferArray[ii++] = v2.c[2];
										floatBufferArray[ii++] = v2.c[3];
									}
									if (context.vinfo.normal != 0)
									{
										floatBufferArray[ii++] = v2.n[0];
										floatBufferArray[ii++] = v2.n[1];
										floatBufferArray[ii++] = v2.n[2];
									}
									if (context.vinfo.position != 0)
									{
										floatBufferArray[ii++] = v2.p[0];
										floatBufferArray[ii++] = v2.p[1];
										floatBufferArray[ii++] = v2.p[2];
									}

									if (context.vinfo.texture != 0)
									{
										if (flippedTexture)
										{
											floatBufferArray[ii++] = v1.t[0];
											floatBufferArray[ii++] = v2.t[1];
										}
										else
										{
											floatBufferArray[ii++] = v2.t[0];
											floatBufferArray[ii++] = v1.t[1];
										}
									}
									if (context.useVertexColor)
									{
										floatBufferArray[ii++] = v2.c[0];
										floatBufferArray[ii++] = v2.c[1];
										floatBufferArray[ii++] = v2.c[2];
										floatBufferArray[ii++] = v2.c[3];
									}
									if (context.vinfo.normal != 0)
									{
										floatBufferArray[ii++] = v2.n[0];
										floatBufferArray[ii++] = v2.n[1];
										floatBufferArray[ii++] = v2.n[2];
									}
									if (context.vinfo.position != 0)
									{
										floatBufferArray[ii++] = v2.p[0];
										floatBufferArray[ii++] = v1.p[1];
										floatBufferArray[ii++] = v2.p[2];
									}
								}
								int bufferSizeInFloats = ii;
								vertexReadingStatistics.end();

								if (useVertexCache)
								{
									cachedVertexInfo = new VertexInfo(context.vinfo);
									VertexCache.Instance.addVertex(re, cachedVertexInfo, numberOfVertex, context.bone_uploaded_matrix, numberOfWeightsForBuffer);
									needSetDataPointers = cachedVertexInfo.loadVertex(re, floatBufferArray, bufferSizeInFloats);
								}
								else
								{
									ByteBuffer byteBuffer = bufferManager.getBuffer(bufferId);
									byteBuffer.clear();
									byteBuffer.asFloatBuffer().put(floatBufferArray, 0, bufferSizeInFloats);
									bufferManager.setBufferSubData(pspsharp.graphics.RE.IRenderingEngine_Fields.RE_ARRAY_BUFFER, bufferId, 0, bufferSizeInFloats * SIZEOF_FLOAT, byteBuffer, pspsharp.graphics.RE.IRenderingEngine_Fields.RE_STREAM_DRAW);
								}
							}
							else
							{
								if (isLogDebugEnabled)
								{
									log_Renamed.debug("Reusing cached Vertex Data");
								}
								needSetDataPointers = cachedVertexInfo.bindVertex(re);
							}
							if (needSetDataPointers)
							{
								setDataPointers(nVertex, context.useVertexColor, nColor, useTexture, nTexCoord, context.vinfo.normal != 0, 0, cachedVertexInfo == null);
							}
							drawArraysStatistics.start();
							re.drawArrays(pspsharp.graphics.RE.IRenderingEngine_Fields.RE_QUADS, 0, numberOfVertex * 2);
							drawArraysStatistics.end();
							if (!context.clearMode)
							{
								context.cullFaceFlag.updateEnabled();
							}
							break;
					}
				}
			}

			vertexStatistics.end();

			// Don't capture the ram if the vertex list is embedded in the display list. TODO handle stall_addr == 0 better
			// TODO may need to move inside the loop if indices are used, or find the largest index so we can calculate the size of the vertex list
			if (State.captureGeNextFrame)
			{
				if (!VertexBufferEmbedded)
				{
					log_Renamed.info("Capture PRIM");
					CaptureManager.captureRAM(context.vinfo.ptr_vertex, context.vinfo.vertexSize * numberOfVertex);
				}
				display.captureGeImage();
				textureChanged = true;
			}

			if (export3D)
			{
				exportCommandPRIM(numberOfVertex, type);
			}

			endRendering(numberOfVertex);
		}

		private void exportCommandPRIM(int numberOfVertex, int type)
		{
			// Do not export 2D
			if (context.vinfo.transform2D)
			{
				return;
			}
			// Only export triangles and triangle strips.
			if (type != PRIM_TRIANGLE && type != PRIM_TRIANGLE_STRIPS)
			{
				return;
			}

			exporter.startPrimitive(numberOfVertex, type);

			Memory mem = Memory.Instance;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'sealed override':
//ORIGINAL LINE: sealed override float[] position = new float[4];
			float[] position = new float[4];
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'sealed override':
//ORIGINAL LINE: sealed override float[] modelViewMatrix = new float[4 * 4];
			float[] modelViewMatrix = new float[4 * 4];
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'sealed override':
//ORIGINAL LINE: sealed override float[] vertexPosition = new float[4];
			float[] vertexPosition = new float[4];
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'sealed override':
//ORIGINAL LINE: sealed override float[] normalizedN = new float[3];
			float[] normalizedN = new float[3];
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'sealed override':
//ORIGINAL LINE: sealed override VertexState transformedV = new VertexState();
			VertexState transformedV = new VertexState();
			Utilities.matrixMult(modelViewMatrix, context.view_uploaded_matrix, context.model_uploaded_matrix);

			for (int i = 0; i < numberOfVertex; i++)
			{
				int addr = context.vinfo.getAddress(mem, i);
				context.vinfo.readVertex(mem, addr, v, true, DoubleTexture2DCoords);

				if (context.vinfo.weight != 0 && context.vinfo.position != 0)
				{
					doSkinning(context.bone_uploaded_matrix, context.vinfo, v);
				}

				// Multiply the vertex position by the model/view matrix and adjust with W
				vertexPosition[0] = v.p[0];
				vertexPosition[1] = v.p[1];
				vertexPosition[2] = v.p[2];
				vertexPosition[3] = 1f;
				Utilities.vectorMult44(position, modelViewMatrix, vertexPosition);
				float invertedW = 1f / position[3];
				transformedV.p[0] = position[0] * invertedW;
				transformedV.p[1] = position[1] * invertedW;
				transformedV.p[2] = position[2] * invertedW;

				transformedV.c[0] = v.c[0];
				transformedV.c[1] = v.c[1];
				transformedV.c[2] = v.c[2];

				transformedV.n[0] = v.n[0];
				transformedV.n[1] = v.n[1];
				transformedV.n[2] = v.n[2];

				transformedV.t[0] = v.t[0];
				transformedV.t[1] = v.t[1];
				switch (context.tex_map_mode)
				{
					case TMAP_TEXTURE_MAP_MODE_TEXTURE_COORDIATES_UV:
						transformedV.t[0] = v.t[0] * context.tex_scale_x + context.tex_translate_x;
						transformedV.t[1] = v.t[1] * context.tex_scale_y + context.tex_translate_y;
						break;
					case TMAP_TEXTURE_MAP_MODE_TEXTURE_MATRIX:
						float x = v.t[0];
						float y = v.t[1];
						float z = 0f;
						switch (context.tex_proj_map_mode)
						{
							case TMAP_TEXTURE_PROJECTION_MODE_POSITION:
								x = v.p[0];
								y = v.p[1];
								z = v.p[2];
								break;
							case TMAP_TEXTURE_PROJECTION_MODE_TEXTURE_COORDINATES:
								x = v.t[0];
								y = v.t[1];
								z = 0f;
								break;
							case TMAP_TEXTURE_PROJECTION_MODE_NORMALIZED_NORMAL:
								Utilities.normalize3(normalizedN, v.n);
								x = normalizedN[0];
								y = normalizedN[1];
								z = normalizedN[2];
								break;
							case TMAP_TEXTURE_PROJECTION_MODE_NORMAL:
								x = v.n[0];
								y = v.n[1];
								z = v.n[2];
								break;
						}
						transformedV.t[0] = x * context.texture_uploaded_matrix[0] + y * context.texture_uploaded_matrix[4] + z * context.texture_uploaded_matrix[8] + context.texture_uploaded_matrix[12];
						transformedV.t[1] = x * context.texture_uploaded_matrix[1] + y * context.texture_uploaded_matrix[5] + z * context.texture_uploaded_matrix[9] + context.texture_uploaded_matrix[13];
						break;
					case TMAP_TEXTURE_MAP_MODE_ENVIRONMENT_MAP:
						// TODO Implement TMAP_TEXTURE_MAP_MODE_ENVIRONMENT_MAP
						break;
				}

				// Textures on PSP are upside-down as compared to the export format:
				// (0,0) is the upper-left corner of the texture on a PSP,
				// (0,0) is the lower-left corner of the texture in the export format.
				transformedV.t[1] = 1f - transformedV.t[1];

				exporter.exportVertex(v, transformedV);
			}

			exporter.endVertex(numberOfVertex, type);

			// Export the texture
			string textureFileName = null;
			if (context.textureFlag.Enabled)
			{
				const int level = 0;
				int textureAddr = context.texture_base_pointer[level];
				int textureWidth = context.texture_width[level];
				int textureHeight = context.texture_height[level];
				int textureBufferWidth = context.texture_buffer_width[level];
				IMemoryReader imageReader = ImageReader.getImageReader(textureAddr, textureWidth, textureHeight, textureBufferWidth, context.texture_storage, context.texture_swizzle, context.tex_clut_addr, context.tex_clut_mode, context.tex_clut_num_blocks, context.tex_clut_start, context.tex_clut_shift, context.tex_clut_mask, clut_buffer32, clut_buffer16);
				CaptureImage captureImage = new CaptureImage(textureAddr, level, imageReader, textureWidth, textureHeight, textureBufferWidth, false, true, null);
				captureImage.Directory = export3DDirectory;
				captureImage.FileFormat = "png";
				if (pspsharp.graphics.RE.IRenderingEngine_Fields.isTextureTypeIndexed[context.texture_storage])
				{
					// When the image is using a CLUT, add the clut address to the file name.
					// Some games are reusing the same texture with different cluts.
					string fileNameSuffix = string.Format("_{0:X8}", context.tex_clut_addr);
					captureImage.FileNameSuffix = fileNameSuffix;
				}

				try
				{
					if (!captureImage.fileExists())
					{
						captureImage.write();
					}
					textureFileName = captureImage.FileName;
				}
				catch (IOException e)
				{
					log_Renamed.error("Export Texture", e);
				}
			}
			exporter.exportTexture(textureFileName);

			exporter.endPrimitive(numberOfVertex, type);
		}

		private void executeCommandTRXKICK()
		{
			context.textureTx_pixelSize = normalArgument & 0x1;

			context.textureTx_sourceAddress &= Memory.addressMask;
			context.textureTx_destinationAddress &= Memory.addressMask;

			if (isLogDebugEnabled)
			{
				log(string.Format("{0} from 0x{1:X8}({2:D},{3:D}) to 0x{4:X8}({5:D},{6:D}), width={7:D}, height={8:D}, pixelSize={9:D}", helper.getCommandString(TRXKICK), context.textureTx_sourceAddress, context.textureTx_sx, context.textureTx_sy, context.textureTx_destinationAddress, context.textureTx_dx, context.textureTx_dy, context.textureTx_width, context.textureTx_height, context.textureTx_pixelSize));
			}

			if (!Memory.isAddressGood(context.textureTx_sourceAddress))
			{
				error(string.Format("{0} invalid source address 0x{1:X8}", helper.getCommandString(TRXKICK), context.textureTx_sourceAddress));
				return;
			}
			if (!Memory.isAddressGood(context.textureTx_destinationAddress))
			{
				error(string.Format("{0} invalid destination address 0x{1:X8}", helper.getCommandString(TRXKICK), context.textureTx_destinationAddress));
				return;
			}

			if (isGeProfilerEnabled)
			{
				GEProfiler.startGeCmd(TRXKICK);
			}

			int pc = currentList.Pc - 4;
			if (pc < multiTrxkickStart || pc > multiTrxkickEnd)
			{
				checkMultiTrxkick();
			}

			bool insideMultiTrxkick = multiTrxkickStart <= pc && pc <= multiTrxkickEnd;
			if (pc == multiTrxkickStart)
			{
				if (isLogDebugEnabled)
				{
					log_Renamed.debug(string.Format("Start of a TRXKICK sequence at 0x{0:X8}", pc));
				}

				// These actions are only done once at the start of the TRXKICK sequence:
				memoryForGEUpdated();

				// Perform a copy of the GE to memory only when really required
				multiTrxkickCopyGeToMemoryDone = false;
				multiTrxkickInvalidateGe = false;
			}
			else if (insideMultiTrxkick)
			{
				if (isLogDebugEnabled)
				{
					log_Renamed.debug(string.Format("Inside a TRXKICK sequence [0x{0:X8}-0x{1:X8}] at 0x{2:X8}", multiTrxkickStart, multiTrxkickEnd, pc));
				}
			}

			if (!insideMultiTrxkick)
			{
				updateGeBuf();
			}

			int pixelFormatGe = context.psm;
			int bpp = (context.textureTx_pixelSize == TRXKICK_16BIT_TEXEL_SIZE) ? 2 : 4;
			int bppGe = sceDisplay.getPixelFormatBytes(pixelFormatGe);

			bool transferUsingMemcpy = insideMultiTrxkick;
			if (insideMultiTrxkick)
			{
				if (display.isGeAddress(context.textureTx_destinationAddress))
				{
					multiTrxkickInvalidateGe = true;
				}
			}
			else
			{
				memoryForGEUpdated();

				if (!display.isGeAddress(context.textureTx_destinationAddress) || bpp != bppGe || display.UsingSoftwareRenderer)
				{
					transferUsingMemcpy = true;
				}
			}

			if (display.isGeAddress(context.textureTx_sourceAddress))
			{
				if (!insideMultiTrxkick || !multiTrxkickCopyGeToMemoryDone)
				{
					re.waitForRenderingCompletion();
					// Force a copy to the memory if performing the transfer using memcpy
					display.copyGeToMemory(true, transferUsingMemcpy);

					multiTrxkickCopyGeToMemoryDone = true;
				}
			}

			if (transferUsingMemcpy)
			{
				if (!insideMultiTrxkick && isLogDebugEnabled)
				{
					if (bpp != bppGe)
					{
						log(helper.getCommandString(TRXKICK) + " BPP not compatible with GE");
					}
					else
					{
						log(helper.getCommandString(TRXKICK) + " not in Ge Address space");
					}
				}

				usingTRXKICK = true;

				// Remove the destination address from the texture cache
				if (!insideMultiTrxkick && canCacheTexture(context.textureTx_destinationAddress))
				{
					TextureCache textureCache = TextureCache.Instance;
					textureCache.resetTextureAlreadyHashed(context.textureTx_destinationAddress, context.tex_clut_addr, context.tex_clut_start, context.tex_clut_mode);
					textureCache.resetTextureAlreadyHashed(context.textureTx_destinationAddress, 0, 0, 0);
				}
				if (context.textureTx_destinationAddress == (context.texture_base_pointer[0] & Memory.addressMask))
				{
					textureChanged = true;
				}

				int width = context.textureTx_width;
				int height = context.textureTx_height;

				int srcAddress = context.textureTx_sourceAddress + (context.textureTx_sy * context.textureTx_sourceLineWidth + context.textureTx_sx) * bpp;
				int dstAddress = context.textureTx_destinationAddress + (context.textureTx_dy * context.textureTx_destinationLineWidth + context.textureTx_dx) * bpp;
				Memory memory = Memory.Instance;
				if (context.textureTx_sourceLineWidth == width && context.textureTx_destinationLineWidth == width)
				{
					// All the lines are adjacent in memory,
					// copy them all in a single memcpy operation.
					int copyLength = height * width * bpp;
					if (isLogDebugEnabled)
					{
						log(string.Format("{0} memcpy(0x{1:X8}-0x{2:X8}, 0x{3:X8}, 0x{4:X})", helper.getCommandString(TRXKICK), dstAddress, dstAddress + copyLength, srcAddress, copyLength));
					}
					memory.memcpy(dstAddress, srcAddress, copyLength);
				}
				else
				{
					// The lines are not adjacent in memory: copy line by line.
					int copyLength = width * bpp;
					int srcLineLength = context.textureTx_sourceLineWidth * bpp;
					int dstLineLength = context.textureTx_destinationLineWidth * bpp;
					for (int y = 0; y < height; y++)
					{
						if (isLogDebugEnabled)
						{
							log(string.Format("{0} memcpy(0x{1:X8}-0x{2:X8}, 0x{3:X8}, 0x{4:X})", helper.getCommandString(TRXKICK), dstAddress, dstAddress + copyLength, srcAddress, copyLength));
						}
						memory.memcpy(dstAddress, srcAddress, copyLength);
						srcAddress += srcLineLength;
						dstAddress += dstLineLength;
					}
				}

				if (State.captureGeNextFrame)
				{
					log_Renamed.warn("TRXKICK outside of Ge Address space not supported in capture yet");
				}
			}
			else if (!skipThisFrame)
			{ // TRXKICK in GE space can be skipped when skipping this frame
				int width = context.textureTx_width;
				int height = context.textureTx_height;
				int dx = context.textureTx_dx;
				int dy = context.textureTx_dy;
				int lineWidth = context.textureTx_sourceLineWidth;

				int geAddr = display.TopAddrGe;
				dy += (context.textureTx_destinationAddress - geAddr) / (display.BufferWidthGe * bpp);
				dx += ((context.textureTx_destinationAddress - geAddr) % (display.BufferWidthGe * bpp)) / bpp;

				if (isLogDebugEnabled)
				{
					log(helper.getCommandString(TRXKICK) + " in Ge Address space: dx=" + dx + ", dy=" + dy + ", width=" + width + ", height=" + height + ", lineWidth=" + lineWidth + ", bpp=" + bpp);
				}

				if (re.VertexArrayAvailable)
				{
					re.bindVertexArray(0);
				}

				int bufferHeight = Utilities.makePow2(height);
				int textureSize = lineWidth * bufferHeight * bpp;
				int sourceAddr = context.textureTx_sourceAddress + (context.textureTx_sy * lineWidth + context.textureTx_sx) * bpp;
				Buffer buffer = Memory.Instance.getBuffer(sourceAddr, textureSize);

				if (State.captureGeNextFrame)
				{
					log_Renamed.info("Capture TRXKICK");
					CaptureManager.captureRAM(context.textureTx_sourceAddress, lineWidth * height * bpp);
				}

				//
				// glTexImage2D only supports
				//		width = (1 << n)	for some integer n
				//		height = (1 << m)	for some integer m
				//
				// This the reason why we are also using glTexSubImage2D.
				//
				int texture = re.genTexture();
				re.bindTexture(texture);
				re.setTextureFormat(pixelFormatGe, false);
				re.setTexImage(0, pixelFormatGe, lineWidth, bufferHeight, pixelFormatGe, pixelFormatGe, 0, null);
				re.TextureMipmapMinFilter = TFLT_NEAREST;
				re.TextureMipmapMagFilter = TFLT_NEAREST;
				re.TextureMipmapMinLevel = 0;
				re.TextureMipmapMaxLevel = 0;
				re.setTextureWrapMode(TWRAP_WRAP_MODE_CLAMP, TWRAP_WRAP_MODE_CLAMP);
				re.setPixelStore(lineWidth, bpp);
				re.setTexSubImage(0, 0, 0, width, height, pixelFormatGe, pixelFormatGe, textureSize, buffer);

				re.startDirectRendering(true, false, true, true, false, 480, 272);
				re.setTextureFormat(pixelFormatGe, false);

				float texCoordX = width / (float) lineWidth;
				float texCoordY = height / (float) bufferHeight;

				int i = 0;
				floatBufferArray[i++] = texCoordX;
				floatBufferArray[i++] = texCoordY;
				floatBufferArray[i++] = dx + width;
				floatBufferArray[i++] = dy + height;

				floatBufferArray[i++] = 0;
				floatBufferArray[i++] = texCoordY;
				floatBufferArray[i++] = dx;
				floatBufferArray[i++] = dy + height;

				floatBufferArray[i++] = 0;
				floatBufferArray[i++] = 0;
				floatBufferArray[i++] = dx;
				floatBufferArray[i++] = dy;

				floatBufferArray[i++] = texCoordX;
				floatBufferArray[i++] = 0;
				floatBufferArray[i++] = dx + width;
				floatBufferArray[i++] = dy;

				IREBufferManager bufferManager = re.BufferManager;
				ByteBuffer byteBuffer = bufferManager.getBuffer(bufferId);
				byteBuffer.clear();
				int bufferSizeInFloats = i;
				byteBuffer.asFloatBuffer().put(floatBufferArray, 0, bufferSizeInFloats);

				re.setVertexInfo(null, false, false, true, pspsharp.graphics.RE.IRenderingEngine_Fields.RE_QUADS);
				re.enableClientState(pspsharp.graphics.RE.IRenderingEngine_Fields.RE_TEXTURE);
				re.disableClientState(pspsharp.graphics.RE.IRenderingEngine_Fields.RE_COLOR);
				re.disableClientState(pspsharp.graphics.RE.IRenderingEngine_Fields.RE_NORMAL);
				re.enableClientState(pspsharp.graphics.RE.IRenderingEngine_Fields.RE_VERTEX);
				bufferManager.setTexCoordPointer(bufferId, 2, pspsharp.graphics.RE.IRenderingEngine_Fields.RE_FLOAT, 4 * SIZEOF_FLOAT, 0);
				bufferManager.setVertexPointer(bufferId, 2, pspsharp.graphics.RE.IRenderingEngine_Fields.RE_FLOAT, 4 * SIZEOF_FLOAT, 2 * SIZEOF_FLOAT);
				bufferManager.setBufferSubData(pspsharp.graphics.RE.IRenderingEngine_Fields.RE_ARRAY_BUFFER, bufferId, 0, bufferSizeInFloats * SIZEOF_FLOAT, byteBuffer, pspsharp.graphics.RE.IRenderingEngine_Fields.RE_STREAM_DRAW);
				re.drawArrays(pspsharp.graphics.RE.IRenderingEngine_Fields.RE_QUADS, 0, 4);

				re.endDirectRendering();
				re.deleteTexture(texture);

				somethingDisplayed = true;
			}

			if (pc == multiTrxkickEnd)
			{
				if (isLogDebugEnabled)
				{
					log_Renamed.debug(string.Format("End of a TRXKICK sequence at 0x{0:X8}", pc));
				}

				multiTrxkickStart = -1;
				multiTrxkickEnd = -1;

				if (multiTrxkickInvalidateGe)
				{
					// Force a reload of the GE texture
					geBufChanged = true;
				}
			}
		}

		private void executeCommandBBOX()
		{
			Memory mem = Memory.Instance;
			int numberOfVertexBoundingBox = normalArgument & 0xFF;

			if (context.vinfo.ptr_vertex == 0)
			{
				// The GE is initialized with a NULL vertex address, do not log an error
				if (isLogDebugEnabled)
				{
					log_Renamed.debug(string.Format("{0} null vertex address", helper.getCommandString(BBOX)));
				}
				return;
			}
			else if (!Memory.isAddressGood(context.vinfo.ptr_vertex))
			{
				// Abort here to avoid a lot of useless memory read errors...
				error(string.Format("{0} Invalid vertex address 0x{1:X8}", helper.getCommandString(BBOX), context.vinfo.ptr_vertex));
				return;
			}
			else if (context.vinfo.position == 0)
			{
				log_Renamed.warn(helper.getCommandString(BBOX) + " no positions for vertex!");
				return;
			}
			else if ((numberOfVertexBoundingBox % 8) != 0)
			{
				// How to interpret non-multiple of 8?
				log_Renamed.warn(helper.getCommandString(BBOX) + " unsupported numberOfVertex=" + numberOfVertexBoundingBox);
			}
			else if (isLogDebugEnabled)
			{
				log_Renamed.debug(helper.getCommandString(BBOX) + " numberOfVertex=" + numberOfVertexBoundingBox);
			}

			if (skipThisFrame)
			{
				return;
			}

			isBoundingBox = true;
			if (isGeProfilerEnabled)
			{
				GEProfiler.startGeCmd(BBOX);
			}

			initRendering();

			re.beginBoundingBox(numberOfVertexBoundingBox);
			for (int i = 0; i < numberOfVertexBoundingBox; i++)
			{
				int addr = context.vinfo.getAddress(mem, i);

				context.vinfo.readVertex(mem, addr, v, false, DoubleTexture2DCoords);
				if (context.vinfo.weight != 0 && context.vinfo.position != 0)
				{
					doSkinning(context.bone_uploaded_matrix, context.vinfo, v);
				}
				if (isLogDebugEnabled)
				{
					log_Renamed.debug(string.Format("{0} ({1:F},{2:F},{3:F})", helper.getCommandString(BBOX), v.p[0], v.p[1], v.p[2]));
				}

				int vertexIndex = i % 8;
				bboxVertices[vertexIndex][0] = v.p[0];
				bboxVertices[vertexIndex][1] = v.p[1];
				bboxVertices[vertexIndex][2] = v.p[2];

				if (vertexIndex == 7)
				{
					re.drawBoundingBox(bboxVertices);
				}
			}
			re.endBoundingBox(context.vinfo);

			endRendering(numberOfVertexBoundingBox);

			isBoundingBox = false;
		}

		private void executeCommandBJUMP()
		{
			bool takeConditionalJump = false;

			if (skipThisFrame)
			{
				takeConditionalJump = true;
			}
			else if (export3D && !export3DOnlyVisible)
			{
				takeConditionalJump = false;
			}
			else
			{
				takeConditionalJump = !re.BoundingBoxVisible;
			}

			if (takeConditionalJump)
			{
				int oldPc = currentList.Pc;
				currentList.jumpRelativeOffset(normalArgument);
				int newPc = currentList.Pc;
				if (isLogDebugEnabled)
				{
					log(string.Format("{0} old PC: 0x{1:X8}, new PC: 0x{2:X8}", helper.getCommandString(BJUMP), oldPc, newPc));
				}
			}
			else
			{
				if (isLogDebugEnabled)
				{
					log(string.Format("{0} not taking Conditional Jump", helper.getCommandString(BJUMP)));
				}
			}
		}

		private void executeCommandBONE()
		{
			// Multiple BONE matrix can be loaded in sequence
			// without having to issue a BOFS for each matrix.
			int matrixIndex = boneMatrixIndex / 12;
			int elementIndex = boneMatrixIndex % 12;
			if (matrixIndex >= context.bone_uploaded_matrix.Length)
			{
				if (isLogDebugEnabled)
				{
					log_Renamed.debug(string.Format("Ignoring BONE matrix element: boneMatrixIndex={0:D}", boneMatrixIndex));
				}
			}
			else
			{
				float floatArgument = VideoEngine.floatArgument(normalArgument);
				context.bone_uploaded_matrix[matrixIndex][elementIndex] = floatArgument;
				context.boneMatrixLinear[(boneMatrixIndex / 3) * 4 + (boneMatrixIndex % 3)] = floatArgument;
				if (matrixIndex >= boneMatrixLinearUpdatedMatrix)
				{
					boneMatrixLinearUpdatedMatrix = matrixIndex + 1;
				}

				boneMatrixIndex++;

				if (isLogDebugEnabled && (boneMatrixIndex % 12) == 0)
				{
					for (int x = 0; x < 3; x++)
					{
						log_Renamed.debug(string.Format("bone matrix {0:D} {1:F2} {2:F2} {3:F2} {4:F2}", matrixIndex, context.bone_uploaded_matrix[matrixIndex][x + 0], context.bone_uploaded_matrix[matrixIndex][x + 3], context.bone_uploaded_matrix[matrixIndex][x + 6], context.bone_uploaded_matrix[matrixIndex][x + 9]));
					}
				}
			}
		}

		private void executeCommandNOP()
		{
			if (isLogDebugEnabled)
			{
				log(helper.getCommandString(NOP));
			}

			nopCount++;
			// Some application do have more than 5000 NOP instructions inside the first 2 lists
			// (i.e. at application initialization), so exclude these lists.
			// The check on the nopCount is currently disabled as it is causing issues in "Dissidia 012: Duodecim sealed override Fantasy".
			if (listCount > 2 && nopCount > int.MaxValue)
			{
				// More than 5000 NOP instructions executed during this list,
				// something must be wrong...
				error(string.Format("Too many NOP instructions executed ({0:D}) at 0x{1:X8}, list {2}", nopCount, currentList.Pc, currentList));
			}
			else
			{
				// Check if we are not reading from an invalid memory region.
				// Abort the list if this is the case.
				// This is only done in the NOP command to not impact performance.
				checkCurrentListPc();
			}
		}

		private void executeCommandVADDR()
		{
			context.vinfo.ptr_vertex = currentList.getAddressRelOffset(normalArgument);
			if (isLogDebugEnabled)
			{
				log(helper.getCommandString(VADDR) + " " + string.Format("{0:x8}", context.vinfo.ptr_vertex));
			}
		}

		private void executeCommandIADDR()
		{
			context.vinfo.ptr_index = currentList.getAddressRelOffset(normalArgument);
			if (isLogDebugEnabled)
			{
				log(helper.getCommandString(IADDR) + " " + string.Format("{0:x8}", context.vinfo.ptr_index));
			}
		}

		private void executeCommandBEZIER()
		{
			int ucount = normalArgument & 0xFF;
			int vcount = (normalArgument >> 8) & 0xFF;
			if (isLogDebugEnabled)
			{
				log(helper.getCommandString(BEZIER) + " ucount=" + ucount + ", vcount=" + vcount);
			}

			if (skipThisFrame)
			{
				endRendering(ucount * vcount);
				return;
			}

			if (isGeProfilerEnabled)
			{
				GEProfiler.startGeCmd(BEZIER);
			}
			updateGeBuf();
			somethingDisplayed = true;
			loadTexture();

			drawBezier(ucount, vcount);
		}

		private void executeCommandSPLINE()
		{
			// Number of control points.
			int sp_ucount = normalArgument & 0xFF;
			int sp_vcount = (normalArgument >> 8) & 0xFF;
			// Knot types.
			int sp_utype = (normalArgument >> 16) & 0x3;
			int sp_vtype = (normalArgument >> 18) & 0x3;

			if (isLogDebugEnabled)
			{
				log(helper.getCommandString(SPLINE) + " sp_ucount=" + sp_ucount + ", sp_vcount=" + sp_vcount + " sp_utype=" + sp_utype + ", sp_vtype=" + sp_vtype);
			}

			if (skipThisFrame)
			{
				endRendering(sp_ucount * sp_vcount);
				return;
			}

			updateGeBuf();
			somethingDisplayed = true;
			if (isGeProfilerEnabled)
			{
				GEProfiler.startGeCmd(SPLINE);
			}
			loadTexture();

			drawSpline(sp_ucount, sp_vcount, sp_utype, sp_vtype);
		}

		private void executeCommandJUMP()
		{
			int oldPc = currentList.Pc;
			currentList.jumpRelativeOffset(normalArgument);
			int newPc = currentList.Pc;
			if (isLogDebugEnabled)
			{
				log(string.Format("{0} old PC: 0x{1:X8}, new PC: 0x{2:X8}", helper.getCommandString(JUMP), oldPc, newPc));
			}
		}

		private void executeCommandCALL()
		{
			int oldPc = currentList.Pc;
			currentList.callRelativeOffset(normalArgument);
			int newPc = currentList.Pc;
			if (!Memory.isAddressGood(newPc))
			{
				error(string.Format("Call instruction to invalid address 0x{0:X8}", newPc));
				// Return immediately
				currentList.ret();
			}
			else
			{
				if (cachedInstructions.ContainsKey(newPc))
				{
					int[] instructions = cachedInstructions[newPc];
					if (instructions != null)
					{
						int memorySize = instructions.Length << 2;
						if (isLogInfoEnabled)
						{
							log_Renamed.info(string.Format("call using cached instructions 0x{0:X8}-0x{1:X8}", newPc, newPc + memorySize));
						}
						IMemoryReader memoryReader = MemoryReader.getMemoryReader(newPc, instructions, 0, memorySize);
						currentList.MemoryReader = memoryReader;
					}
				}
			}

			if (isLogDebugEnabled)
			{
				log(string.Format("{0} old PC: 0x{1:X8}, new PC: 0x{2:X8}", helper.getCommandString(CALL), oldPc, newPc));
			}
		}

		private void executeCommandRET()
		{
			int oldPc = currentList.Pc;
			currentList.ret();
			int newPc = currentList.Pc;
			if (isLogDebugEnabled)
			{
				log(string.Format("{0} old PC: 0x{1:X8}, new PC: 0x{2:X8}", helper.getCommandString(RET), oldPc, newPc));
			}
		}

		private void executeCommandEND()
		{
			int previousCommand = command(currentList.readPreviousInstruction());
			// Ignore the END command if the command before was not SIGNAL or FINISH
			if (previousCommand == SIGNAL || previousCommand == FINISH)
			{
				// Try to end the current list.
				// The list only ends (isEnded() == true) if FINISH was called previously.
				// In SIGNAL + END cases, isEnded() still remains false.
				currentList.endList();
				currentList.pauseList();
				if (isLogDebugEnabled)
				{
					log(string.Format("{0} pc=0x{1:X8}", helper.getCommandString(END), currentList.Pc));
				}
				updateGeBuf();
			}
			else if (isLogWarnEnabled)
			{
				log_Renamed.warn(string.Format("Ignoring {0} 0x{1:X6} command without {2}/{3} at pc=0x{4:X8}", helper.getCommandString(END), normalArgument, helper.getCommandString(SIGNAL), helper.getCommandString(FINISH), currentList.Pc - 4));
			}
		}

		private void executeCommandSIGNAL()
		{
			int behavior = (normalArgument >> 16) & 0xFF;
			int signal = normalArgument & 0xFFFF;
			if (isLogDebugEnabled)
			{
				log(string.Format("{0} (behavior={1:D}, signal=0x{2:X})", helper.getCommandString(SIGNAL), behavior, signal));
			}

			switch (behavior)
			{
				case sceGe_user.PSP_GE_SIGNAL_SYNC:
				{
					// Skip END / FINISH / END
					if (command(currentList.readNextInstruction()) == END)
					{
						if (command(currentList.readNextInstruction()) == FINISH)
						{
							if (command(currentList.readNextInstruction()) == END)
							{
								// OK, skipped END / FINISH / END sequence
							}
							else
							{
								currentList.undoRead(3);
							}
						}
						else
						{
							currentList.undoRead(2);
						}
					}
					else
					{
						currentList.undoRead(1);
					}

					if (isLogDebugEnabled)
					{
						log(string.Format("PSP_GE_SIGNAL_SYNC ignored PC: 0x{0:X8}", currentList.Pc));
					}
					break;
				}
				case sceGe_user.PSP_GE_SIGNAL_CALL:
				{
					// Call list using absolute address from SIGNAL + END.
					int nextInstruction = currentList.readNextInstruction();
					if (command(nextInstruction) == END)
					{
						int hi16 = signal & 0x0FFF;
						// Read & skip END
						int lo16 = nextInstruction & 0xFFFF;
						int addr = (hi16 << 16) | lo16;
						int oldPc = currentList.Pc;
						currentList.callAbsolute(addr);
						int newPc = currentList.Pc;
						if (isLogDebugEnabled)
						{
							log(string.Format("PSP_GE_SIGNAL_CALL old PC: 0x{0:X8}, new PC: 0x{1:X8}", oldPc, newPc));
						}
					}
					else
					{
						currentList.undoRead();
					}
					break;
				}
				case sceGe_user.PSP_GE_SIGNAL_RETURN:
				{
					// Return from PSP_GE_SIGNAL_CALL.
					int nextInstruction = currentList.readNextInstruction();
					if (command(nextInstruction) == END)
					{
						// Skip END
						int oldPc = currentList.Pc;
						currentList.ret();
						int newPc = currentList.Pc;
						if (isLogDebugEnabled)
						{
							log(string.Format("PSP_GE_SIGNAL_RETURN old PC: 0x{0:X8}, new PC: 0x{1:X8}", oldPc, newPc));
						}
					}
					else
					{
						currentList.undoRead();
					}
					break;
				}
				case sceGe_user.PSP_GE_SIGNAL_TBP0_REL:
				case sceGe_user.PSP_GE_SIGNAL_TBP1_REL:
				case sceGe_user.PSP_GE_SIGNAL_TBP2_REL:
				case sceGe_user.PSP_GE_SIGNAL_TBP3_REL:
				case sceGe_user.PSP_GE_SIGNAL_TBP4_REL:
				case sceGe_user.PSP_GE_SIGNAL_TBP5_REL:
				case sceGe_user.PSP_GE_SIGNAL_TBP6_REL:
				case sceGe_user.PSP_GE_SIGNAL_TBP7_REL:
				{
					// Overwrite TBPn and TBPw with SIGNAL + END (uses relative address only).
					int nextInstruction = currentList.readNextInstruction();
					if (command(nextInstruction) == END)
					{
						int hi16 = signal & 0xFFFF;
						int lo16 = nextInstruction & 0xFFFF;
						int width = (nextInstruction >> 16) & 0xFF;
						int addr = currentList.getAddressRel((hi16 << 16) | lo16);
						context.texture_base_pointer[behavior - sceGe_user.PSP_GE_SIGNAL_TBP0_REL] = addr;
						context.texture_buffer_width[behavior - sceGe_user.PSP_GE_SIGNAL_TBP0_REL] = width;
					}
					else
					{
						currentList.undoRead();
					}
					break;
				}
				case sceGe_user.PSP_GE_SIGNAL_TBP0_REL_OFFSET:
				case sceGe_user.PSP_GE_SIGNAL_TBP1_REL_OFFSET:
				case sceGe_user.PSP_GE_SIGNAL_TBP2_REL_OFFSET:
				case sceGe_user.PSP_GE_SIGNAL_TBP3_REL_OFFSET:
				case sceGe_user.PSP_GE_SIGNAL_TBP4_REL_OFFSET:
				case sceGe_user.PSP_GE_SIGNAL_TBP5_REL_OFFSET:
				case sceGe_user.PSP_GE_SIGNAL_TBP6_REL_OFFSET:
				case sceGe_user.PSP_GE_SIGNAL_TBP7_REL_OFFSET:
				{
					// Overwrite TBPn and TBPw with SIGNAL + END (uses relative address with offset).
					int nextInstruction = currentList.readNextInstruction();
					if (command(nextInstruction) == END)
					{
						int hi16 = signal & 0xFFFF;
						int lo16 = nextInstruction & 0xFFFF;
						int width = (nextInstruction >> 16) & 0xFF;
						int addr = currentList.getAddressRelOffset((hi16 << 16) | lo16);
						context.texture_base_pointer[behavior - sceGe_user.PSP_GE_SIGNAL_TBP0_REL_OFFSET] = addr;
						context.texture_buffer_width[behavior - sceGe_user.PSP_GE_SIGNAL_TBP7_REL_OFFSET] = width;
					}
					else
					{
						currentList.undoRead();
					}
					break;
				}
				case sceGe_user.PSP_GE_SIGNAL_HANDLER_SUSPEND:
				case sceGe_user.PSP_GE_SIGNAL_HANDLER_CONTINUE:
				case sceGe_user.PSP_GE_SIGNAL_HANDLER_PAUSE:
				{
					currentList.clearRestart();
					currentList.pushSignalCallback(currentList.id, behavior, signal);
					break;
				}
				default:
				{
					if (isLogWarnEnabled)
					{
						log_Renamed.warn(string.Format("{0} (behavior={1:D}, signal=0x{2:X}) unknown behavior at 0x{3:X8}", helper.getCommandString(SIGNAL), behavior, signal, currentList.Pc - 4));
					}
				}
			break;
			}
		}

		private void executeCommandFINISH()
		{
			if (isLogDebugEnabled)
			{
				log(helper.getCommandString(FINISH) + " " + getArgumentLog(normalArgument));
			}
			currentList.clearRestart();
			currentList.finishList();
			currentList.pushFinishCallback(currentList.id, normalArgument);
		}

		private void executeCommandBASE()
		{
			context.@base = (normalArgument << 8) & unchecked((int)0xFF000000);
			// Bits of (normalArgument & 0x0000FFFF) are ignored
			// (tested: "Ape Escape On the Loose")
			if (isLogDebugEnabled)
			{
				log(string.Format("{0} 0x{1:X8}", helper.getCommandString(BASE), context.@base));
			}
		}

		private void executeCommandVTYPE()
		{
			int old_transform_mode = context.transform_mode;
			bool old_vertex_hasColor = context.vinfo.color != 0;
			context.vinfo.processType(normalArgument);
			context.transform_mode = (normalArgument >> 23) & 0x1;
			bool vertex_hasColor = context.vinfo.color != 0;

			//Switching from 2D to 3D or 3D to 2D?
			if (old_transform_mode != context.transform_mode)
			{
				projectionMatrixUpload.Changed = true;
				modelMatrixUpload.Changed = true;
				viewMatrixUpload.Changed = true;
				textureMatrixUpload.Changed = true;
				viewportChanged = true;
				depthChanged = true;
				materialChanged = true;
				// Switching from 2D to 3D?
				if (context.transform_mode == VTYPE_TRANSFORM_PIPELINE_TRANS_COORD)
				{
					lightingChanged = true;
				}
			}
			else if (old_vertex_hasColor != vertex_hasColor)
			{
				// Materials have to be reloaded when the vertex color presence is changing
				materialChanged = true;
			}

			if (isLogDebugEnabled)
			{
				log(helper.getCommandString(VTYPE) + " " + context.vinfo.ToString());
			}
		}

		private void executeCommandOFFSET_ADDR()
		{
			context.baseOffset = normalArgument << 8;
			if (isLogDebugEnabled)
			{
				log(string.Format("{0} 0x{1:X8}", helper.getCommandString(OFFSET_ADDR), context.baseOffset));
			}
		}

		private void executeCommandORIGIN_ADDR()
		{
			context.baseOffset = currentList.Pc - 4;
			if (normalArgument != 0)
			{
				log_Renamed.warn(string.Format("{0} unknown argument 0x{1:X8}", helper.getCommandString(ORIGIN_ADDR), normalArgument));
			}
			else if (isLogDebugEnabled)
			{
				log(string.Format("{0} 0x{1:X8} originAddr=0x{2:X8}", helper.getCommandString(ORIGIN_ADDR), normalArgument, context.baseOffset));
			}
		}

		private void executeCommandREGION1()
		{
			int old_region_x1 = context.region_x1;
			int old_region_y1 = context.region_y1;
			context.region_x1 = normalArgument & 0x3ff;
			context.region_y1 = (normalArgument >> 10) & 0x3ff;
			if (old_region_x1 != context.region_x1 || old_region_y1 != context.region_y1)
			{
				scissorChanged = true;
			}
		}

		private void executeCommandREGION2()
		{
			int old_region_x2 = context.region_x2;
			int old_region_y2 = context.region_y2;
			context.region_x2 = normalArgument & 0x3ff;
			context.region_y2 = (normalArgument >> 10) & 0x3ff;
			context.region_width = (context.region_x2 + 1) - context.region_x1;
			context.region_height = (context.region_y2 + 1) - context.region_y1;
			if (isLogDebugEnabled)
			{
				log("drawRegion(" + context.region_x1 + "," + context.region_y1 + "," + context.region_width + "," + context.region_height + ")");
			}
			if (old_region_x2 != context.region_x2 || old_region_y2 != context.region_y2)
			{
				scissorChanged = true;
			}
		}

		private void executeCommandLTE()
		{
			context.lightingFlag.setEnabled(normalArgument);
			if (context.lightingFlag.Enabled)
			{
				lightingChanged = true;
				materialChanged = true;
			}
		}

		private void executeCommandLTEn()
		{
			int lnum = command_Renamed - LTE0;
			EnableDisableFlag lightFlag = context.lightFlags[lnum];
			lightFlag.setEnabled(normalArgument);
			if (lightFlag.Enabled)
			{
				lightingChanged = true;
			}
		}

		private void executeCommandCPE()
		{
			context.clipPlanesFlag.setEnabled(normalArgument);
		}

		private void executeCommandBCE()
		{
			context.cullFaceFlag.setEnabled(normalArgument);
		}

		private void executeCommandTME()
		{
			context.textureFlag.setEnabled(normalArgument);
		}

		private void executeCommandFGE()
		{
			context.fogFlag.setEnabled(normalArgument);
			if (context.fogFlag.Enabled)
			{
				re.setFogHint();
			}
		}

		private void executeCommandDTE()
		{
			context.ditherFlag.setEnabled(normalArgument);
		}

		private void executeCommandABE()
		{
			context.blendFlag.setEnabled(normalArgument);
		}

		private void executeCommandATE()
		{
			context.alphaTestFlag.setEnabled(normalArgument);
		}

		private void executeCommandZTE()
		{
			context.depthTestFlag.setEnabled(normalArgument);
			if (context.depthTestFlag.Enabled)
			{
				// OpenGL requires the Depth parameters to be reloaded
				depthChanged = true;
			}
		}

		private void executeCommandSTE()
		{
			context.stencilTestFlag.setEnabled(normalArgument);
		}

		private void executeCommandAAE()
		{
			context.lineSmoothFlag.setEnabled(normalArgument);
			if (context.lineSmoothFlag.Enabled)
			{
				re.setLineSmoothHint();
			}
		}

		private void executeCommandPCE()
		{
			context.patchCullFaceFlag.setEnabled(normalArgument);
		}

		private void executeCommandCTE()
		{
			context.colorTestFlag.setEnabled(normalArgument);
		}

		private void executeCommandLOE()
		{
			context.colorLogicOpFlag.setEnabled(normalArgument);
		}

		private void executeCommandBOFS()
		{
			boneMatrixIndex = normalArgument;
			if (isLogDebugEnabled)
			{
				log(string.Format("bone matrix offset {0:D}", normalArgument));
			}
		}

		private void executeCommandMWn()
		{
			int index = command_Renamed - MW0;
			float floatArgument = VideoEngine.floatArgument(normalArgument);
			context.morph_weight[index] = floatArgument;
			re.setMorphWeight(index, floatArgument);
			if (isLogDebugEnabled)
			{
				log(string.Format("morph weight {0:D} {1:F}", index, floatArgument));
			}
		}

		private void executeCommandPSUB()
		{
			// A patch division of 0 has the same effect as 1 (checked on PSP using splinesurface demo)
			context.patch_div_s = max(normalArgument & 0xFF, 1);
			context.patch_div_t = max((normalArgument >> 8) & 0xFF, 1);
			re.setPatchDiv(context.patch_div_s, context.patch_div_t);
			if (isLogDebugEnabled)
			{
				log(string.Format("{0} patch_div_s={1:D}, patch_div_t={2:D}", helper.getCommandString(PSUB), context.patch_div_s, context.patch_div_t));
			}
		}

		private void executeCommandPPRIM()
		{
			context.patch_prim = (normalArgument & 0x3);
			// Primitive type to use in patch division:
			// 0 - Triangle.
			// 1 - Line.
			// 2 - Point.
			re.PatchPrim = context.patch_prim;
			if (isLogDebugEnabled)
			{
				log(helper.getCommandString(PPRIM) + " patch_prim=" + context.patch_prim);
			}
		}

		private void executeCommandPFACE()
		{
			// 0 - Clockwise oriented patch / 1 - Counter clockwise oriented patch.
			context.patchFaceFlag.setEnabled(normalArgument);
		}

		private void executeCommandMMS()
		{
			modelMatrixUpload.startUpload(normalArgument);
			if (isLogDebugEnabled)
			{
				log("sceGumMatrixMode GU_MODEL " + normalArgument);
			}
		}

		private void executeCommandMODEL()
		{
			if (modelMatrixUpload.uploadValue(floatArgument(normalArgument)))
			{
				log("glLoadMatrixf", context.model_uploaded_matrix);
			}
		}

		private void executeCommandVMS()
		{
			viewMatrixUpload.startUpload(normalArgument);
			if (isLogDebugEnabled)
			{
				log("sceGumMatrixMode GU_VIEW " + normalArgument);
			}
		}

		private void executeCommandVIEW()
		{
			if (viewMatrixUpload.uploadValue(floatArgument(normalArgument)))
			{
				log("glLoadMatrixf", context.view_uploaded_matrix);
			}
		}

		private void executeCommandPMS()
		{
			projectionMatrixUpload.startUpload(normalArgument);
			if (isLogDebugEnabled)
			{
				log("sceGumMatrixMode GU_PROJECTION " + normalArgument);
			}
		}

		private void executeCommandPROJ()
		{
			if (projectionMatrixUpload.uploadValue(floatArgument(normalArgument)))
			{
				log("glLoadMatrixf", context.proj_uploaded_matrix);
			}
		}

		private void executeCommandTMS()
		{
			textureMatrixUpload.startUpload(normalArgument);
			if (isLogDebugEnabled)
			{
				log("sceGumMatrixMode GU_TEXTURE " + normalArgument);
			}
		}

		private void executeCommandTMATRIX()
		{
			if (textureMatrixUpload.uploadValue(floatArgument(normalArgument)))
			{
				log("glLoadMatrixf", context.texture_uploaded_matrix);
			}
		}

		private void executeCommandXSCALE()
		{
			int old_viewport_width = context.viewport_width;
			context.viewport_width = (int) floatArgument(normalArgument);
			if (old_viewport_width != context.viewport_width)
			{
				viewportChanged = true;
				if ((old_viewport_width < 0 && context.viewport_width > 0) || (old_viewport_width > 0 && context.viewport_width < 0))
				{
					// Projection matrix has to be reloaded when X-axis flipped
					projectionMatrixUpload.Changed = true;
				}
			}
		}

		private void executeCommandYSCALE()
		{
			int old_viewport_height = context.viewport_height;
			context.viewport_height = (int) floatArgument(normalArgument);
			if (old_viewport_height != context.viewport_height)
			{
				viewportChanged = true;
				if ((old_viewport_height < 0 && context.viewport_height > 0) || (old_viewport_height > 0 && context.viewport_height < 0))
				{
					// Projection matrix has to be reloaded when Y-axis flipped
					projectionMatrixUpload.Changed = true;
				}
			}

			if (isLogDebugEnabled)
			{
				log_Renamed.debug("sceGuViewport(cx=" + context.viewport_cx + ", cy=" + context.viewport_cy + ", w=" + context.viewport_width + ", h=" + context.viewport_height + ")");
			}
		}

		private void executeCommandZSCALE()
		{
			float old_zscale = context.zscale;
			float floatArgument = VideoEngine.floatArgument(normalArgument);
			context.zscale = floatArgument;
			if (old_zscale != context.zscale)
			{
				depthChanged = true;
			}

			if (isLogDebugEnabled)
			{
				log(helper.getCommandString(ZSCALE) + " " + floatArgument);
			}
		}

		private void executeCommandXPOS()
		{
			int old_viewport_cx = context.viewport_cx;
			context.viewport_cx = (int) floatArgument(normalArgument);
			if (old_viewport_cx != context.viewport_cx)
			{
				viewportChanged = true;
			}
		}

		private void executeCommandYPOS()
		{
			int old_viewport_cy = context.viewport_cy;
			context.viewport_cy = (int) floatArgument(normalArgument);
			if (old_viewport_cy != context.viewport_cy)
			{
				viewportChanged = true;
			}

			if (isLogDebugEnabled)
			{
				log_Renamed.debug("sceGuViewport(cx=" + context.viewport_cx + ", cy=" + context.viewport_cy + ", w=" + context.viewport_width + ", h=" + context.viewport_height + ")");
			}
		}

		private void executeCommandZPOS()
		{
			float old_zpos = context.zpos;
			float floatArgument = VideoEngine.floatArgument(normalArgument);
			context.zpos = floatArgument;
			if (old_zpos != context.zpos)
			{
				depthChanged = true;
			}

			if (isLogDebugEnabled)
			{
				log(string.Format("{0} {1:F}", helper.getCommandString(ZPOS), floatArgument));
			}
		}

		private void executeCommandUSCALE()
		{
			float old_tex_scale_x = context.tex_scale_x;
			context.tex_scale_x = floatArgument(normalArgument);

			if (old_tex_scale_x != context.tex_scale_x)
			{
				textureMatrixUpload.Changed = true;
			}

			if (isLogDebugEnabled)
			{
				log(string.Format("sceGuTexScale(u={0:F}, X)", context.tex_scale_x));
			}
		}

		private void executeCommandVSCALE()
		{
			float old_tex_scale_y = context.tex_scale_y;
			context.tex_scale_y = floatArgument(normalArgument);

			if (old_tex_scale_y != context.tex_scale_y)
			{
				textureMatrixUpload.Changed = true;
			}

			if (isLogDebugEnabled)
			{
				log(string.Format("sceGuTexScale(X, v={0:F})", context.tex_scale_y));
			}
		}

		private void executeCommandUOFFSET()
		{
			float old_tex_translate_x = context.tex_translate_x;
			context.tex_translate_x = floatArgument(normalArgument);

			if (old_tex_translate_x != context.tex_translate_x)
			{
				textureMatrixUpload.Changed = true;
			}

			if (isLogDebugEnabled)
			{
				log(string.Format("sceGuTexOffset(u={0:F}, X)", context.tex_translate_x));
			}
		}

		private void executeCommandVOFFSET()
		{
			float old_tex_translate_y = context.tex_translate_y;
			context.tex_translate_y = floatArgument(normalArgument);

			if (old_tex_translate_y != context.tex_translate_y)
			{
				textureMatrixUpload.Changed = true;
			}

			if (isLogDebugEnabled)
			{
				log(string.Format("sceGuTexOffset(X, v={0:F})", context.tex_translate_y));
			}
		}

		private void executeCommandOFFSETX()
		{
			int old_offset_x = context.offset_x;
			context.offset_x = normalArgument >> 4;
			if (old_offset_x != context.offset_x)
			{
				viewportChanged = true;
			}
		}

		private void executeCommandOFFSETY()
		{
			int old_offset_y = context.offset_y;
			context.offset_y = normalArgument >> 4;
			if (old_offset_y != context.offset_y)
			{
				viewportChanged = true;
			}

			if (isLogDebugEnabled)
			{
				log_Renamed.debug("sceGuOffset(x=" + context.offset_x + ",y=" + context.offset_y + ")");
			}
		}

		private void executeCommandSHADE()
		{
			context.shadeModel = normalArgument & 1;
			re.ShadeModel = context.shadeModel;
			if (isLogDebugEnabled)
			{
				log("sceGuShadeModel(" + ((context.shadeModel != 0) ? "smooth" : "flat") + ")");
			}
		}

		private void executeCommandRNORM()
		{
			// This seems to be taked into account when calculating the lighting
			// for the current normal.
			context.faceNormalReverseFlag.setEnabled(normalArgument);
		}

		private void executeCommandCMAT()
		{
			int old_mat_flags = context.mat_flags;
			context.mat_flags = normalArgument & 7;
			if (old_mat_flags != context.mat_flags)
			{
				materialChanged = true;
			}

			if (isLogDebugEnabled)
			{
				log("sceGuColorMaterial " + context.mat_flags);
			}
		}

		private void executeCommandEMC()
		{
			context.mat_emissive[0] = ((normalArgument) & 255) / 255.0f;
			context.mat_emissive[1] = ((normalArgument >> 8) & 255) / 255.0f;
			context.mat_emissive[2] = ((normalArgument >> 16) & 255) / 255.0f;
			context.mat_emissive[3] = 1.0f;
			materialChanged = true;
			re.MaterialEmissiveColor = context.mat_emissive;
			if (isLogDebugEnabled)
			{
				log("material emission " + string.Format("r={0:F1} g={1:F1} b={2:F1} ({3:X8})", context.mat_emissive[0], context.mat_emissive[1], context.mat_emissive[2], normalArgument));
			}
		}

		private void executeCommandAMC()
		{
			context.mat_ambient[0] = ((normalArgument) & 255) / 255.0f;
			context.mat_ambient[1] = ((normalArgument >> 8) & 255) / 255.0f;
			context.mat_ambient[2] = ((normalArgument >> 16) & 255) / 255.0f;
			materialChanged = true;
			if (isLogDebugEnabled)
			{
				log(string.Format("material ambient r={0:F1} g={1:F1} b={2:F1} ({3:X8})", context.mat_ambient[0], context.mat_ambient[1], context.mat_ambient[2], normalArgument));
			}
		}

		private void executeCommandDMC()
		{
			context.mat_diffuse[0] = ((normalArgument) & 255) / 255.0f;
			context.mat_diffuse[1] = ((normalArgument >> 8) & 255) / 255.0f;
			context.mat_diffuse[2] = ((normalArgument >> 16) & 255) / 255.0f;
			context.mat_diffuse[3] = 1.0f;
			materialChanged = true;
			if (isLogDebugEnabled)
			{
				log("material diffuse " + string.Format("r={0:F1} g={1:F1} b={2:F1} ({3:X8})", context.mat_diffuse[0], context.mat_diffuse[1], context.mat_diffuse[2], normalArgument));
			}
		}

		private void executeCommandSMC()
		{
			context.mat_specular[0] = ((normalArgument) & 255) / 255.0f;
			context.mat_specular[1] = ((normalArgument >> 8) & 255) / 255.0f;
			context.mat_specular[2] = ((normalArgument >> 16) & 255) / 255.0f;
			context.mat_specular[3] = 1.0f;
			materialChanged = true;
			if (isLogDebugEnabled)
			{
				log("material specular " + string.Format("r={0:F1} g={1:F1} b={2:F1} ({3:X8})", context.mat_specular[0], context.mat_specular[1], context.mat_specular[2], normalArgument));
			}
		}

		private void executeCommandAMA()
		{
			context.mat_ambient[3] = ((normalArgument) & 255) / 255.0f;
			materialChanged = true;
			if (isLogDebugEnabled)
			{
				log(string.Format("material ambient a={0:F1} ({1:X2})", context.mat_ambient[3], normalArgument & 255));
			}
		}

		private void executeCommandSPOW()
		{
			context.materialShininess = floatArgument(normalArgument);
			re.MaterialShininess = context.materialShininess;
			if (isLogDebugEnabled)
			{
				log("material shininess " + context.materialShininess);
			}
		}

		private void executeCommandALC()
		{
			context.ambient_light[0] = ((normalArgument) & 255) / 255.0f;
			context.ambient_light[1] = ((normalArgument >> 8) & 255) / 255.0f;
			context.ambient_light[2] = ((normalArgument >> 16) & 255) / 255.0f;
			re.LightModelAmbientColor = context.ambient_light;
			if (isLogDebugEnabled)
			{
				log_Renamed.debug(string.Format("ambient light r={0:F1} g={1:F1} b={2:F1} ({3:X6})", context.ambient_light[0], context.ambient_light[1], context.ambient_light[2], normalArgument));
			}
		}

		private void executeCommandALA()
		{
			context.ambient_light[3] = ((normalArgument) & 255) / 255.0f;
			re.LightModelAmbientColor = context.ambient_light;
		}

		private void executeCommandLMODE()
		{
			context.lightMode = normalArgument & 1;
			re.LightMode = context.lightMode;
			if (isLogDebugEnabled)
			{
				log_Renamed.debug("sceGuLightMode(" + ((context.lightMode != 0) ? "GU_SEPARATE_SPECULAR_COLOR" : "GU_SINGLE_COLOR") + ")");
			}

			// Check if other values than 0 and 1 are set
			if ((normalArgument & ~1) != 0)
			{
				log_Renamed.warn(string.Format("Unknown light mode sceGuLightMode({0:X6})", normalArgument));
			}
		}

		private void executeCommandLTn()
		{
			int lnum = command_Renamed - LT0;
			int old_light_type = context.light_type[lnum];
			int old_light_kind = context.light_kind[lnum];
			context.light_type[lnum] = (normalArgument >> 8) & 3;
			context.light_kind[lnum] = normalArgument & 3;

			if (old_light_type != context.light_type[lnum] || old_light_kind != context.light_kind[lnum])
			{
				lightingChanged = true;
			}

			if (context.light_type[lnum] == LIGHT_TYPE_UNKNOWN)
			{
				// Confirmed by testing with 3DStudio: light type 3 is equivalent to light type 2.
				context.light_type[lnum] = LIGHT_SPOT;
			}

			switch (context.light_type[lnum])
			{
				case LIGHT_DIRECTIONAL:
					context.light_pos[lnum][3] = 0.0f;
					break;
				case LIGHT_POINT:
					re.setLightSpotCutoff(lnum, 180);
					context.light_pos[lnum][3] = 1.0f;
					break;
				case LIGHT_SPOT:
					context.light_pos[lnum][3] = 1.0f;
					break;
				default:
					error(string.Format("Unknown light type: 0x{0:X6}", normalArgument));
				break;
			}
			re.setLightType(lnum, context.light_type[lnum], context.light_kind[lnum]);

			if (isLogDebugEnabled)
			{
				log_Renamed.debug(string.Format("Light #{0:D}: type {1:D}, kind {2:D}", lnum, (normalArgument >> 8) & 3, normalArgument & 3));
			}
		}

		private void executeCommandLXPn()
		{
			int lnum = (command_Renamed - LXP0) / 3;
			int component = (command_Renamed - LXP0) % 3;
			float old_light_pos = context.light_pos[lnum][component];
			context.light_pos[lnum][component] = floatArgument(normalArgument);

			if (old_light_pos != context.light_pos[lnum][component])
			{
				lightingChanged = true;

				// Environment mapping is using light positions
				if (context.tex_map_mode == TMAP_TEXTURE_MAP_MODE_ENVIRONMENT_MAP)
				{
					if (context.tex_shade_u == lnum || context.tex_shade_v == lnum)
					{
						textureMatrixUpload.Changed = true;
					}
				}
			}
			if (isLogDebugEnabled)
			{
				log_Renamed.debug(string.Format("Light {0:D} position ({1:F}, {2:F}, {3:F})", lnum, context.light_pos[lnum][0], context.light_pos[lnum][1], context.light_pos[lnum][2]));
			}
		}

		private void executeCommandLXDn()
		{
			int lnum = (command_Renamed - LXD0) / 3;
			int component = (command_Renamed - LXD0) % 3;
			float old_light_dir = context.light_dir[lnum][component];

			// OpenGL requires a normal in the opposite direction as the PSP
			context.light_dir[lnum][component] = -floatArgument(normalArgument);

			if (old_light_dir != context.light_dir[lnum][component])
			{
				lightingChanged = true;
			}
			if (isLogDebugEnabled)
			{
				log_Renamed.debug(string.Format("Light {0:D} direction ({1:F}, {2:F}, {3:F})", lnum, context.light_dir[lnum][0], context.light_dir[lnum][1], context.light_dir[lnum][2]));
			}
			// OpenGL parameter for light direction is set in initRendering
			// because it depends on the model/view matrix
		}

		private void executeCommandLCAn()
		{
			int lnum = (command_Renamed - LCA0) / 3;
			context.lightConstantAttenuation[lnum] = floatArgument(normalArgument);
			re.setLightConstantAttenuation(lnum, context.lightConstantAttenuation[lnum]);
		}

		private void executeCommandLLAn()
		{
			int lnum = (command_Renamed - LLA0) / 3;
			context.lightLinearAttenuation[lnum] = floatArgument(normalArgument);
			re.setLightLinearAttenuation(lnum, context.lightLinearAttenuation[lnum]);
		}

		private void executeCommandLQAn()
		{
			int lnum = (command_Renamed - LQA0) / 3;
			context.lightQuadraticAttenuation[lnum] = floatArgument(normalArgument);
			re.setLightQuadraticAttenuation(lnum, context.lightQuadraticAttenuation[lnum]);
		}

		private void executeCommandSLEn()
		{
			int lnum = command_Renamed - SLE0;
			float old_spotLightExponent = context.spotLightExponent[lnum];
			context.spotLightExponent[lnum] = floatArgument(normalArgument);

			if (old_spotLightExponent != context.spotLightExponent[lnum])
			{
				lightingChanged = true;
			}

			if (isLogDebugEnabled)
			{
				VideoEngine.log_Renamed.debug("sceGuLightSpot(" + lnum + ",X," + context.spotLightExponent[lnum] + ",X)");
			}
		}

		private void executeCommandSLFn()
		{
			int lnum = command_Renamed - SLF0;
			float old_spotLightCutoff = context.spotLightCutoff[lnum];

			// PSP Cutoff is cosine of angle, OpenGL expects degrees
			float floatArgument = VideoEngine.floatArgument(normalArgument);
			float degreeCutoff = (float) Math.toDegrees(System.Math.Acos(floatArgument));
			if ((degreeCutoff >= 0 && degreeCutoff <= 90) || degreeCutoff == 180)
			{
				context.spotLightCutoff[lnum] = degreeCutoff;
				context.spotLightCosCutoff[lnum] = floatArgument;

				if (old_spotLightCutoff != context.spotLightCutoff[lnum])
				{
					lightingChanged = true;
				}

				if (isLogDebugEnabled)
				{
					log_Renamed.debug("sceGuLightSpot(" + lnum + ",X,X," + floatArgument + "=" + degreeCutoff + ")");
				}
			}
			else
			{
				log_Renamed.warn("sceGuLightSpot(" + lnum + ",X,X," + floatArgument + ") invalid argument value");
			}
		}

		private void executeCommandALCn()
		{
			int lnum = (command_Renamed - ALC0) / 3;
			context.lightAmbientColor[lnum][0] = ((normalArgument) & 255) / 255.0f;
			context.lightAmbientColor[lnum][1] = ((normalArgument >> 8) & 255) / 255.0f;
			context.lightAmbientColor[lnum][2] = ((normalArgument >> 16) & 255) / 255.0f;
			context.lightAmbientColor[lnum][3] = 1.0f;
			re.setLightAmbientColor(lnum, context.lightAmbientColor[lnum]);
			if (isLogDebugEnabled)
			{
				log(string.Format("sceGuLightColor GU_LIGHT{0:D}, GU_AMBIENT r={1:F3}, b={2:F3}, g={3:F3}, a={4:F3}", lnum, context.lightAmbientColor[lnum][0], context.lightAmbientColor[lnum][1], context.lightAmbientColor[lnum][2], context.lightAmbientColor[lnum][3]));
			}
		}

		private void executeCommandDLCn()
		{
			int lnum = (command_Renamed - DLC0) / 3;
			context.lightDiffuseColor[lnum][0] = ((normalArgument) & 255) / 255.0f;
			context.lightDiffuseColor[lnum][1] = ((normalArgument >> 8) & 255) / 255.0f;
			context.lightDiffuseColor[lnum][2] = ((normalArgument >> 16) & 255) / 255.0f;
			context.lightDiffuseColor[lnum][3] = 1.0f;
			re.setLightDiffuseColor(lnum, context.lightDiffuseColor[lnum]);
			if (isLogDebugEnabled)
			{
				log(string.Format("sceGuLightColor GU_LIGHT{0:D}, GU_DIFFUSE r={1:F3}, b={2:F3}, g={3:F3}, a={4:F3}", lnum, context.lightDiffuseColor[lnum][0], context.lightDiffuseColor[lnum][1], context.lightDiffuseColor[lnum][2], context.lightDiffuseColor[lnum][3]));
			}
		}

		private void executeCommandSLCn()
		{
			int lnum = (command_Renamed - SLC0) / 3;
			float old_lightSpecularColor0 = context.lightSpecularColor[lnum][0];
			float old_lightSpecularColor1 = context.lightSpecularColor[lnum][1];
			float old_lightSpecularColor2 = context.lightSpecularColor[lnum][2];
			context.lightSpecularColor[lnum][0] = ((normalArgument) & 255) / 255.0f;
			context.lightSpecularColor[lnum][1] = ((normalArgument >> 8) & 255) / 255.0f;
			context.lightSpecularColor[lnum][2] = ((normalArgument >> 16) & 255) / 255.0f;
			context.lightSpecularColor[lnum][3] = 1.0f;

			if (old_lightSpecularColor0 != context.lightSpecularColor[lnum][0] || old_lightSpecularColor1 != context.lightSpecularColor[lnum][1] || old_lightSpecularColor2 != context.lightSpecularColor[lnum][2])
			{
				lightingChanged = true;
			}
			re.setLightSpecularColor(lnum, context.lightDiffuseColor[lnum]);
			if (isLogDebugEnabled)
			{
				log(string.Format("sceGuLightColor GU_LIGHT{0:D}, GU_SPECULAR r={1:F3}, b={2:F3}, g={3:F3}, a={4:F3}", lnum, context.lightSpecularColor[lnum][0], context.lightSpecularColor[lnum][1], context.lightSpecularColor[lnum][2], context.lightSpecularColor[lnum][3]));
			}
		}

		private void executeCommandFFACE()
		{
			context.frontFaceCw = normalArgument != 0;
			re.FrontFace = context.frontFaceCw;
			if (isLogDebugEnabled)
			{
				log(helper.getCommandString(FFACE) + " " + ((normalArgument != 0) ? "clockwise" : "counter-clockwise"));
			}
		}

		private void executeCommandFBP()
		{
			// FBP can be called before or after FBW
			context.fbp = (context.fbp & unchecked((int)0xff000000)) | normalArgument;
			if (isLogDebugEnabled)
			{
				log(string.Format("{0} fbp=0x{1:X8}, fbw={2:D}", helper.getCommandString(FBP), context.fbp, context.fbw));
			}
			geBufChanged = true;
		}

		private void executeCommandFBW()
		{
			context.fbw = normalArgument & 0xffff;
			if (isLogDebugEnabled)
			{
				log(string.Format("{0} fbp=0x{1:X8}, fbw={2:D}", helper.getCommandString(FBW), context.fbp, context.fbw));
			}
			geBufChanged = true;
		}

		private void executeCommandZBP()
		{
			context.zbp = (context.zbp & unchecked((int)0xff000000)) | normalArgument;
			if (isLogDebugEnabled)
			{
				log(string.Format("{0} zbp=0x{1:X8}, zbw={2:D}", helper.getCommandString(ZBP), context.zbp, context.zbw));
			}
		}

		private void executeCommandZBW()
		{
			context.zbw = normalArgument & 0xffff;
			if (isLogDebugEnabled)
			{
				log(string.Format("{0} zbp=0x{1:X8}, zbw={2:D}", helper.getCommandString(ZBW), context.zbp, context.zbw));
			}
		}

		private void executeCommandTBPn()
		{
			int level = command_Renamed - TBP0;
			int old_texture_base_pointer = context.texture_base_pointer[level];
			context.texture_base_pointer[level] = (context.texture_base_pointer[level] & unchecked((int)0xff000000)) | normalArgument;

			if (old_texture_base_pointer != context.texture_base_pointer[level])
			{
				textureChanged = true;
			}

			if (isLogDebugEnabled)
			{
				log(string.Format("sceGuTexImage(level={0:D}, X, X, X, lo(pointer=0x{1:X8})", level, context.texture_base_pointer[level]));
			}
		}

		private void executeCommandTBWn()
		{
			int level = command_Renamed - TBW0;
			int old_texture_base_pointer = context.texture_base_pointer[level];
			int old_texture_buffer_width = context.texture_buffer_width[level];
			context.texture_base_pointer[level] = (context.texture_base_pointer[level] & 0x00ffffff) | ((normalArgument << 8) & unchecked((int)0xff000000));
			context.texture_buffer_width[level] = normalArgument & 0xffff;

			if (old_texture_base_pointer != context.texture_base_pointer[level] || old_texture_buffer_width != context.texture_buffer_width[level])
			{
				textureChanged = true;
			}

			if (isLogDebugEnabled)
			{
				log(string.Format("sceGuTexImage(level={0:D}, X, X, texBufferWidth={1:D}, hi(pointer=0x{2:X8}))", level, context.texture_buffer_width[level], context.texture_base_pointer[level]));
			}
		}

		private void executeCommandCBP()
		{
			int old_tex_clut_addr = context.tex_clut_addr;
			context.tex_clut_addr = (context.tex_clut_addr & unchecked((int)0xff000000)) | normalArgument;

			clutIsDirty = true;
			if (old_tex_clut_addr != context.tex_clut_addr)
			{
				textureChanged = true;
			}

			if (isLogDebugEnabled)
			{
				log(string.Format("sceGuClutLoad(X, lo(cbp=0x{0:X8})", context.tex_clut_addr));
			}
		}

		private void executeCommandCBPH()
		{
			int old_tex_clut_addr = context.tex_clut_addr;
			context.tex_clut_addr = (context.tex_clut_addr & 0x00ffffff) | ((normalArgument << 8) & 0x0f000000);

			clutIsDirty = true;
			if (old_tex_clut_addr != context.tex_clut_addr)
			{
				textureChanged = true;
			}

			if (isLogDebugEnabled)
			{
				log(string.Format("sceGuClutLoad(X, hi(cbp=0x{0:X8})", context.tex_clut_addr));
			}
		}

		private void executeCommandTRXSBP()
		{
			context.textureTx_sourceAddress = (context.textureTx_sourceAddress & unchecked((int)0xFF000000)) | normalArgument;
			if (isLogDebugEnabled)
			{
				log_Renamed.debug(string.Format("{0} sourceAddress=0x{1:X8}", helper.getCommandString(command_Renamed), context.textureTx_sourceAddress));
			}
		}

		private void executeCommandTRXSBW()
		{
			context.textureTx_sourceAddress = (context.textureTx_sourceAddress & 0x00FFFFFF) | ((normalArgument << 8) & unchecked((int)0xFF000000));
			context.textureTx_sourceLineWidth = normalArgument & 0x0000FFFF;
			if (isLogDebugEnabled)
			{
				log_Renamed.debug(string.Format("{0} sourceAddress=0x{1:X8}, sourceLineWidth={2:D}", helper.getCommandString(command_Renamed), context.textureTx_sourceAddress, context.textureTx_sourceLineWidth));
			}

			// TODO Check when sx and sy are reset to 0. Here or after TRXKICK?
			context.textureTx_sx = 0;
			context.textureTx_sy = 0;
		}

		private void executeCommandTRXDBP()
		{
			context.textureTx_destinationAddress = (context.textureTx_destinationAddress & unchecked((int)0xFF000000)) | normalArgument;
			if (isLogDebugEnabled)
			{
				log_Renamed.debug(string.Format("{0} destinationAddress=0x{1:X8}", helper.getCommandString(command_Renamed), context.textureTx_destinationAddress));
			}
		}

		private void executeCommandTRXDBW()
		{
			context.textureTx_destinationAddress = (context.textureTx_destinationAddress & 0x00FFFFFF) | ((normalArgument << 8) & unchecked((int)0xFF000000));
			context.textureTx_destinationLineWidth = normalArgument & 0x0000FFFF;
			if (isLogDebugEnabled)
			{
				log_Renamed.debug(string.Format("{0} destinationAddress=0x{1:X8}, destinationLineWidth={2:D}", helper.getCommandString(command_Renamed), context.textureTx_destinationAddress, context.textureTx_destinationLineWidth));
			}

			// TODO Check when dx and dy are reset to 0. Here or after TRXKICK?
			context.textureTx_dx = 0;
			context.textureTx_dy = 0;
		}

		private void executeCommandTSIZEn()
		{
			int level = command_Renamed - TSIZE0;
			int old_texture_height = context.texture_height[level];
			int old_texture_width = context.texture_width[level];
			// Astonishia Story is using normalArgument = 0x1804
			// -> use texture_height = 1 << 0x08 (and not 1 << 0x18)
			//        texture_width  = 1 << 0x04
			// On a PSP, the maximum texture size is 512x512: the exponent value must be [0..9].
			// On the PS3 PSP emulator, the maximum texture size can be higher (e.g. 1024x1024 is valid).
			int height_exp2 = System.Math.Min((normalArgument >> 8) & 0x0F, maxTextureSizeLog2);
			int width_exp2 = System.Math.Min((normalArgument) & 0x0F, maxTextureSizeLog2);
			context.texture_height[level] = 1 << height_exp2;
			context.texture_width[level] = 1 << width_exp2;

			if (old_texture_height != context.texture_height[level] || old_texture_width != context.texture_width[level])
			{
				if (context.transform_mode == VTYPE_TRANSFORM_PIPELINE_RAW_COORD && level == 0)
				{
					textureMatrixUpload.Changed = true;
				}
				textureChanged = true;
			}

			if (isLogDebugEnabled)
			{
				log(string.Format("sceGuTexImage(level={0:D}, width={1:D}, height={2:D}, X, X)", level, context.texture_width[level], context.texture_height[level]));
			}
		}

		private void executeCommandTMAP()
		{
			int old_tex_map_mode = context.tex_map_mode;
			context.tex_map_mode = normalArgument & 3;
			context.tex_proj_map_mode = (normalArgument >> 8) & 3;

			if (old_tex_map_mode != context.tex_map_mode)
			{
				textureMatrixUpload.Changed = true;
			}

			if (context.tex_map_mode == TMAP_TEXTURE_MAP_MODE_UNKNOW)
			{
				// Confirmed by testing with 3DStudio: map mode 3 is equivalent to map mode 0
				context.tex_map_mode = TMAP_TEXTURE_MAP_MODE_TEXTURE_COORDIATES_UV;
			}

			if (isLogDebugEnabled)
			{
				log("sceGuTexMapMode(mode=" + context.tex_map_mode + ", X, X)");
				log("sceGuTexProjMapMode(mode=" + context.tex_proj_map_mode + ")");
			}
		}

		private void executeCommandTEXTURE_ENV_MAP_MATRIX()
		{
			context.tex_shade_u = (normalArgument >> 0) & 0x3;
			context.tex_shade_v = (normalArgument >> 8) & 0x3;

			textureMatrixUpload.Changed = true;
			if (isLogDebugEnabled)
			{
				log("sceGuTexMapMode(X, " + context.tex_shade_u + ", " + context.tex_shade_v + ")");
			}
		}

		private void executeCommandTMODE()
		{
			int old_texture_num_mip_maps = context.texture_num_mip_maps;
			bool old_mipmapShareClut = context.mipmapShareClut;
			bool old_texture_swizzle = context.texture_swizzle;
			context.texture_num_mip_maps = (normalArgument >> 16) & 0x7;
			// This parameter has only a meaning when
			//  texture_storage == GU_PSM_T4 and texture_num_mip_maps > 0
			// when parameter==0: all the mipmaps share the same clut entries (normal behavior)
			// when parameter==1: each mipmap has its own clut table, 16 entries each, stored sequentially
			context.mipmapShareClut = ((normalArgument >> 8) & 0x1) == 0;
			context.texture_swizzle = ((normalArgument) & 0x1) != 0;

			if (old_texture_num_mip_maps != context.texture_num_mip_maps || old_mipmapShareClut != context.mipmapShareClut || old_texture_swizzle != context.texture_swizzle)
			{
				textureChanged = true;
			}

			if (isLogDebugEnabled)
			{
				log("sceGuTexMode(X, mipmaps=" + context.texture_num_mip_maps + ", mipmapShareClut=" + context.mipmapShareClut + ", swizzle=" + context.texture_swizzle + ")");
			}
		}

		private void executeCommandTPSM()
		{
			int old_texture_storage = context.texture_storage;
			context.texture_storage = normalArgument & 0xF; // Lower four bits.

			if (old_texture_storage != context.texture_storage)
			{
				textureChanged = true;
			}

			if (isLogDebugEnabled)
			{
				log("sceGuTexMode(tpsm=" + context.texture_storage + "(" + getPsmName(context.texture_storage) + "), X, X, X)");
			}
		}

		private void executeCommandCLOAD()
		{
			int old_tex_clut_num_blocks = context.tex_clut_num_blocks;
			context.tex_clut_num_blocks = normalArgument & 0x3F;

			clutIsDirty = true;
			if (old_tex_clut_num_blocks != context.tex_clut_num_blocks)
			{
				textureChanged = true;
			}

			// Some games use the following sequence:
			// - sceGuClutLoad(num_blocks=32, X)
			// - sceGuClutLoad(num_blocks=1, X)
			// - tflush
			// - prim ... (texture data is referencing the clut entries from 32 blocks)
			//
			readClut();

			if (isLogDebugEnabled)
			{
				log("sceGuClutLoad(num_blocks=" + context.tex_clut_num_blocks + ", X)");
			}
		}

		private void executeCommandCMODE()
		{
			int old_tex_clut_mode = context.tex_clut_mode;
			int old_tex_clut_shift = context.tex_clut_shift;
			int old_tex_clut_mask = context.tex_clut_mask;
			int old_tex_clut_start = context.tex_clut_start;
			context.tex_clut_mode = normalArgument & 0x03;
			context.tex_clut_shift = (normalArgument >> 2) & 0x1F;
			context.tex_clut_mask = (normalArgument >> 8) & 0xFF;
			context.tex_clut_start = (normalArgument >> 16) & 0x1F;

			clutIsDirty = true;
			if (old_tex_clut_mode != context.tex_clut_mode || old_tex_clut_shift != context.tex_clut_shift || old_tex_clut_mask != context.tex_clut_mask || old_tex_clut_start != context.tex_clut_start)
			{
				textureChanged = true;
			}

			if (isLogDebugEnabled)
			{
				log("sceGuClutMode(cpsm=" + context.tex_clut_mode + "(" + getPsmName(context.tex_clut_mode) + "), shift=" + context.tex_clut_shift + ", mask=0x" + context.tex_clut_mask.ToString("x") + ", start=" + context.tex_clut_start + ")");
			}
		}

		private void executeCommandTFLT()
		{
			int old_tex_mag_filter = context.tex_mag_filter;
			int old_tex_min_filter = context.tex_min_filter;

			context.tex_min_filter = normalArgument & 0x7;
			context.tex_mag_filter = (normalArgument >> 8) & 0x1;

			if (isLogDebugEnabled)
			{
				log("sceGuTexFilter(min=" + context.tex_min_filter + ", mag=" + context.tex_mag_filter + ") (mm#" + context.texture_num_mip_maps + ")");
			}

			if (context.tex_min_filter == TFLT_UNKNOW1 || context.tex_min_filter == TFLT_UNKNOW2)
			{
				log_Renamed.warn("Unknown minimizing filter " + (normalArgument & 0xFF));
				context.tex_min_filter = TFLT_NEAREST;
			}

			if (old_tex_mag_filter != context.tex_mag_filter || old_tex_min_filter != context.tex_min_filter)
			{
				textureChanged = true;
			}
		}

		private void executeCommandTWRAP()
		{
			context.tex_wrap_s = normalArgument & 0x1;
			context.tex_wrap_t = (normalArgument >> 8) & 0x1;

			if (isLogDebugEnabled)
			{
				log_Renamed.debug(string.Format("sceGuTexWrap({0:D}, {1:D})", context.tex_wrap_s, context.tex_wrap_t));
			}
		}

		private void executeCommandTBIAS()
		{
			int old_tex_mipmap_mode = context.tex_mipmap_mode;
			int old_tex_mipmap_bias_int = context.tex_mipmap_bias_int;
			float old_tex_mipmap_bias = context.tex_mipmap_bias;

			context.tex_mipmap_mode = normalArgument & 0x3;
			int biasValue = (int)(sbyte)(normalArgument >> 16); // Signed 8-bit 4.4 fixed point value
			context.tex_mipmap_bias_int = biasValue >> 4;
			context.tex_mipmap_bias = biasValue / 16.0f;
			if (isLogDebugEnabled)
			{
				log_Renamed.debug("sceGuTexLevelMode(mode=" + context.tex_mipmap_mode + ", bias=" + context.tex_mipmap_bias + ")");
			}

			if (context.tex_mipmap_mode != old_tex_mipmap_mode || context.tex_mipmap_bias_int != old_tex_mipmap_bias_int || context.tex_mipmap_bias != old_tex_mipmap_bias)
			{
				textureChanged = true;
			}
		}

		private void executeCommandTEC()
		{
			context.tex_env_color[0] = ((normalArgument) & 255) / 255.0f;
			context.tex_env_color[1] = ((normalArgument >> 8) & 255) / 255.0f;
			context.tex_env_color[2] = ((normalArgument >> 16) & 255) / 255.0f;
			context.tex_env_color[3] = 1.0f;
			re.TextureEnvColor = context.tex_env_color;

			if (isLogDebugEnabled)
			{
				log(string.Format("sceGuTexEnvColor {0:X6} (no alpha)", normalArgument));
			}
		}

		private void executeCommandTFLUSH()
		{
			// Do not load the texture right now, clut parameters can still be
			// defined after the TFLUSH and before the PRIM command.
			// Delay the texture loading until the PRIM command.
			if (isLogDebugEnabled)
			{
				log("tflush (deferring to prim)");
			}
		}

		private void executeCommandTSYNC()
		{
			// Synchronize the GE when a drawing result is used as a texture.
			if (isLogDebugEnabled)
			{
				log(helper.getCommandString(TSYNC) + " wait for rendering completion.");
			}
			re.waitForRenderingCompletion();
		}

		private void executeCommandFFAR()
		{
			context.fog_far = floatArgument(normalArgument);
		}

		private void executeCommandFDIST()
		{
			context.fog_dist = floatArgument(normalArgument);
			re.setFogDist(context.fog_far, context.fog_dist);

			if (isLogDebugEnabled)
			{
				log(string.Format("sceGuFog(far={0:F}, dist={1:F}, X)", context.fog_far, context.fog_dist));
			}
		}

		private void executeCommandFCOL()
		{
			context.fog_color[0] = ((normalArgument) & 255) / 255.0f;
			context.fog_color[1] = ((normalArgument >> 8) & 255) / 255.0f;
			context.fog_color[2] = ((normalArgument >> 16) & 255) / 255.0f;
			context.fog_color[3] = 1.0f;
			re.FogColor = context.fog_color;

			if (isLogDebugEnabled)
			{
				log(string.Format("sceGuFog(X, X, color=0x{0:X6}) (no alpha)", normalArgument));
			}
		}

		private void executeCommandTSLOPE()
		{
			context.tslope_level = floatArgument(normalArgument);
			if (isLogDebugEnabled)
			{
				log(helper.getCommandString(TSLOPE) + " tslope_level=" + context.tslope_level);
			}
		}

		private void executeCommandPSM()
		{
			context.psm = normalArgument & 0x3;
			if (isLogDebugEnabled)
			{
				log(string.Format("psm={0:D} {1} (0x{2:X})", context.psm, getPsmName(context.psm), normalArgument));
			}
			geBufChanged = true;
		}

		private void executeCommandSCISSOR1()
		{
			context.scissor_x1 = normalArgument & 0x3ff;
			context.scissor_y1 = (normalArgument >> 10) & 0x3ff;

			// Already update width&height in case SCISSOR2 is not coming...
			context.scissor_width = 1 + context.scissor_x2 - context.scissor_x1;
			context.scissor_height = 1 + context.scissor_y2 - context.scissor_y1;

			scissorChanged = true;
		}

		private void executeCommandSCISSOR2()
		{
			context.scissor_x2 = normalArgument & 0x3ff;
			context.scissor_y2 = (normalArgument >> 10) & 0x3ff;
			context.scissor_width = 1 + context.scissor_x2 - context.scissor_x1;
			context.scissor_height = 1 + context.scissor_y2 - context.scissor_y1;
			if (isLogDebugEnabled)
			{
				log("sceGuScissor(" + context.scissor_x1 + "," + context.scissor_y1 + "," + context.scissor_width + "," + context.scissor_height + ")");
			}
			scissorChanged = true;
		}

		private void executeCommandNEARZ()
		{
			float old_nearZ = context.nearZ;
			context.nearZ = normalArgument & 0xFFFF;
			if (old_nearZ != context.nearZ)
			{
				depthChanged = true;
			}
		}

		private void executeCommandFARZ()
		{
			float old_farZ = context.farZ;
			context.farZ = normalArgument & 0xFFFF;
			if (old_farZ != context.farZ)
			{
				// OpenGL requires the Depth parameters to be reloaded
				depthChanged = true;
			}

			if (isLogDebugEnabled)
			{
				log_Renamed.debug("sceGuDepthRange(" + context.nearZ + ", " + context.farZ + ")");
			}
		}

		private void executeCommandCTST()
		{
			context.colorTestFunc = normalArgument & 3;
			re.ColorTestFunc = context.colorTestFunc;

			if (isLogDebugEnabled)
			{
				log_Renamed.debug(string.Format("sceGuColorFunc colorTestFunc={0:D}", context.colorTestFunc));
			}
		}

		private void executeCommandCREF()
		{
			context.colorTestRef[0] = (normalArgument) & 0xFF;
			context.colorTestRef[1] = (normalArgument >> 8) & 0xFF;
			context.colorTestRef[2] = (normalArgument >> 16) & 0xFF;
			re.ColorTestReference = context.colorTestRef;

			if (isLogDebugEnabled)
			{
				log_Renamed.debug(string.Format("sceGuColorFunc colorTestRef=0x{0:X6}", normalArgument));
			}
		}

		private void executeCommandCMSK()
		{
			context.colorTestMsk[0] = (normalArgument) & 0xFF;
			context.colorTestMsk[1] = (normalArgument >> 8) & 0xFF;
			context.colorTestMsk[2] = (normalArgument >> 16) & 0xFF;
			re.ColorTestMask = context.colorTestMsk;

			if (isLogDebugEnabled)
			{
				log_Renamed.debug(string.Format("sceGuColorFunc colorTestMsk=0x{0:X6}", normalArgument));
			}
		}

		private void executeCommandATST()
		{
			context.alphaFunc = normalArgument & 0x7;
			context.alphaRef = (normalArgument >> 8) & 0xFF;
			context.alphaMask = (normalArgument >> 16) & 0xFF;
			re.setAlphaFunc(context.alphaFunc, context.alphaRef, context.alphaMask);

			if (isLogDebugEnabled)
			{
				log(string.Format("sceGuAlphaFunc(func={0:D}, ref=0x{1:X2}, mask=0x{2:X2})", context.alphaFunc, context.alphaRef, context.alphaMask));
			}
		}

		private void executeCommandSTST()
		{
			context.stencilFunc = normalArgument & 0x7;
			context.stencilRef = (normalArgument >> 8) & 0xFF;
			switch (context.psm)
			{
				case TPSM_PIXEL_STORAGE_MODE_16BIT_BGR5650:
					context.stencilRef = 0;
					break;
				case TPSM_PIXEL_STORAGE_MODE_16BIT_ABGR5551:
					context.stencilRef &= 0x80;
					break;
				case TPSM_PIXEL_STORAGE_MODE_16BIT_ABGR4444:
					context.stencilRef &= 0xF0;
					break;
			}
			context.stencilMask = (normalArgument >> 16) & 0xFF;
			re.setStencilFunc(context.stencilFunc, context.stencilRef, context.stencilMask);

			if (isLogDebugEnabled)
			{
				log(string.Format("sceGuStencilFunc(func={0:D}, ref=0x{1:X2}, mask=0x{2:X2})", context.stencilFunc, context.stencilRef, context.stencilMask));
			}
		}

		private void executeCommandSOP()
		{
			context.stencilOpFail = normalArgument & 0xFF;
			context.stencilOpZFail = (normalArgument >> 8) & 0xFF;
			context.stencilOpZPass = (normalArgument >> 16) & 0xFF;

			if (context.stencilOpFail > SOP_DECREMENT_STENCIL_VALUE)
			{
				log_Renamed.warn("Unknown stencil operation " + context.stencilOpFail);
				context.stencilOpFail &= 0x7;
				if (context.stencilOpFail > SOP_DECREMENT_STENCIL_VALUE)
				{
					context.stencilOpFail = SOP_KEEP_STENCIL_VALUE;
				}
			}
			if (context.stencilOpZFail > SOP_DECREMENT_STENCIL_VALUE)
			{
				log_Renamed.warn("Unknown stencil operation " + context.stencilOpZFail);
				context.stencilOpZFail &= 0x7;
				if (context.stencilOpZFail > SOP_DECREMENT_STENCIL_VALUE)
				{
					context.stencilOpZFail = SOP_KEEP_STENCIL_VALUE;
				}
			}
			if (context.stencilOpZPass > SOP_DECREMENT_STENCIL_VALUE)
			{
				log_Renamed.warn("Unknown stencil operation " + context.stencilOpZPass);
				context.stencilOpZPass &= 0x7;
				if (context.stencilOpZPass > SOP_DECREMENT_STENCIL_VALUE)
				{
					context.stencilOpZPass = SOP_KEEP_STENCIL_VALUE;
				}
			}

			re.setStencilOp(context.stencilOpFail, context.stencilOpZFail, context.stencilOpZPass);

			if (isLogDebugEnabled)
			{
				log("sceGuStencilOp(fail=" + (normalArgument & 0xFF) + ", zfail=" + ((normalArgument >> 8) & 0xFF) + ", zpass=" + ((normalArgument >> 16) & 0xFF) + ")");
			}
		}

		private void executeCommandZTST()
		{
			int oldDepthFunc = context.depthFunc;

			context.depthFunc = normalArgument & 0xFF;
			if (context.depthFunc > ZTST_FUNCTION_PASS_PX_WHEN_DEPTH_IS_GREATER_OR_EQUAL)
			{
				log_Renamed.warn(string.Format("Unknown ztst function {0:D}", context.depthFunc));
				context.depthFunc &= 0x7;
			}

			if (oldDepthFunc != context.depthFunc)
			{
				depthChanged = true;
			}

			if (isLogDebugEnabled)
			{
				log("sceGuDepthFunc(" + normalArgument + ")");
			}
		}

		private void executeCommandALPHA()
		{
			context.blend_src = normalArgument & 0xF;
			context.blend_dst = (normalArgument >> 4) & 0xF;
			context.blendEquation = (normalArgument >> 8) & 0xF;

			if (context.blendEquation > ALPHA_SOURCE_BLEND_OPERATION_ABSOLUTE_VALUE)
			{
				log_Renamed.warn("Unhandled blend operation " + context.blendEquation);
				context.blendEquation = ALPHA_SOURCE_BLEND_OPERATION_ADD;
			}

			// Tested on PSP: alpha blend src/dst values in range [11..15] are equivalent to ALPHA_FIX
			if (context.blend_src > ALPHA_FIX)
			{
				if (isLogDebugEnabled)
				{
					log_Renamed.debug(string.Format("alpha blend src {0:D} changed to ALPHA_FIX", context.blend_src));
				}
				context.blend_src = ALPHA_FIX;
			}
			if (context.blend_dst > ALPHA_FIX)
			{
				if (isLogDebugEnabled)
				{
					log_Renamed.debug(string.Format("alpha blend dst {0:D} changed to ALPHA_FIX", context.blend_dst));
				}
				context.blend_dst = ALPHA_FIX;
			}

			re.BlendEquation = context.blendEquation;
			re.setBlendFunc(context.blend_src, context.blend_dst);

			if (isLogDebugEnabled)
			{
				log("sceGuBlendFunc(op=" + context.blendEquation + ", src=" + context.blend_src + ", dst=" + context.blend_dst + ")");
			}
		}

		private void executeCommandSFIX()
		{
			context.sfix = normalArgument;
			context.sfix_color[0] = ((context.sfix) & 255) / 255.0f;
			context.sfix_color[1] = ((context.sfix >> 8) & 255) / 255.0f;
			context.sfix_color[2] = ((context.sfix >> 16) & 255) / 255.0f;
			context.sfix_color[3] = 1.0f;

			re.setBlendSFix(context.sfix, context.sfix_color);

			if (isLogDebugEnabled)
			{
				log(string.Format("{0} : 0x{1:X6}", helper.getCommandString(SFIX), context.sfix));
			}
		}

		private void executeCommandDFIX()
		{
			context.dfix = normalArgument;
			context.dfix_color[0] = ((context.dfix) & 255) / 255.0f;
			context.dfix_color[1] = ((context.dfix >> 8) & 255) / 255.0f;
			context.dfix_color[2] = ((context.dfix >> 16) & 255) / 255.0f;
			context.dfix_color[3] = 1.0f;

			re.setBlendDFix(context.dfix, context.dfix_color);

			if (isLogDebugEnabled)
			{
				log(string.Format("{0} : 0x{1:X6}", helper.getCommandString(DFIX), context.dfix));
			}
		}

		private void setDitherMatrixValue(int index, int value)
		{
			// The dither matrix's values can vary between -8 and 7.
			context.dither_matrix[index] = ditherMatrixValueMapping[value & 0xF];
		}

		private void executeCommandDTH0()
		{
			setDitherMatrixValue(0, normalArgument);
			setDitherMatrixValue(1, normalArgument >> 4);
			setDitherMatrixValue(2, normalArgument >> 8);
			setDitherMatrixValue(3, normalArgument >> 12);

			if (isLogDebugEnabled)
			{
				log_Renamed.debug(string.Format("DTH0({0:X4}): {1:D}  {2:D}  {3:D}  {4:D}", normalArgument, context.dither_matrix[0], context.dither_matrix[1], context.dither_matrix[2], context.dither_matrix[3]));
			}
		}

		private void executeCommandDTH1()
		{
			setDitherMatrixValue(4, normalArgument);
			setDitherMatrixValue(5, normalArgument >> 4);
			setDitherMatrixValue(6, normalArgument >> 8);
			setDitherMatrixValue(7, normalArgument >> 12);

			if (isLogDebugEnabled)
			{
				log_Renamed.debug(string.Format("DTH1({0:X4}): {1:D}  {2:D}  {3:D}  {4:D}", normalArgument, context.dither_matrix[4], context.dither_matrix[5], context.dither_matrix[6], context.dither_matrix[7]));
			}
		}

		private void executeCommandDTH2()
		{
			setDitherMatrixValue(8, normalArgument);
			setDitherMatrixValue(9, normalArgument >> 4);
			setDitherMatrixValue(10, normalArgument >> 8);
			setDitherMatrixValue(11, normalArgument >> 12);

			if (isLogDebugEnabled)
			{
				log_Renamed.debug(string.Format("DTH2({0:X4}): {1:D}  {2:D}  {3:D}  {4:D}", normalArgument, context.dither_matrix[8], context.dither_matrix[9], context.dither_matrix[10], context.dither_matrix[11]));
			}
		}

		private void executeCommandDTH3()
		{
			setDitherMatrixValue(12, normalArgument);
			setDitherMatrixValue(13, normalArgument >> 4);
			setDitherMatrixValue(14, normalArgument >> 8);
			setDitherMatrixValue(15, normalArgument >> 12);

			if (isLogDebugEnabled)
			{
				log_Renamed.debug(string.Format("DTH3({0:X4}): {1:D}  {2:D}  {3:D}  {4:D}", normalArgument, context.dither_matrix[12], context.dither_matrix[13], context.dither_matrix[14], context.dither_matrix[15]));
			}
		}

		private void executeCommandLOP()
		{
			context.logicOp = normalArgument & 0xF;
			re.LogicOp = context.logicOp;
			if (isLogDebugEnabled)
			{
				log_Renamed.debug("sceGuLogicalOp(LogicOp=" + context.logicOp + "(" + getLOpName(context.logicOp) + "))");
			}
		}

		private void executeCommandZMSK()
		{
			// NOTE: PSP depth mask as 1 is meant to avoid depth writes,
			//       with OpenGL it's the opposite
			context.depthMask = (normalArgument == 0);
			re.DepthMask = context.depthMask;
			if (context.depthMask)
			{
				// OpenGL requires the Depth parameters to be reloaded
				depthChanged = true;
			}

			if (isLogDebugEnabled)
			{
				log("sceGuDepthMask(" + (normalArgument != 0 ? "disableWrites" : "enableWrites") + ")");
			}
		}

		private void executeCommandPMSKC()
		{
			context.colorMask[0] = normalArgument & 0xFF;
			context.colorMask[1] = (normalArgument >> 8) & 0xFF;
			context.colorMask[2] = (normalArgument >> 16) & 0xFF;
			re.setColorMask(context.colorMask[0], context.colorMask[1], context.colorMask[2], context.colorMask[3]);

			if (isLogDebugEnabled)
			{
				log(string.Format("{0} color mask=0x{1:X6}", helper.getCommandString(PMSKC), normalArgument));
			}
		}

		private void executeCommandPMSKA()
		{
			context.colorMask[3] = normalArgument & 0xFF;
			re.setColorMask(context.colorMask[0], context.colorMask[1], context.colorMask[2], context.colorMask[3]);

			if (isLogDebugEnabled)
			{
				log(string.Format("{0} alpha mask=0x{1:X2}", helper.getCommandString(PMSKA), normalArgument));
			}
		}

		private void executeCommandTRXPOS()
		{
			context.textureTx_sx = normalArgument & 0x1FF;
			context.textureTx_sy = (normalArgument >> 10) & 0x1FF;
		}

		private void executeCommandTRXDPOS()
		{
			context.textureTx_dx = normalArgument & 0x1FF;
			context.textureTx_dy = (normalArgument >> 10) & 0x1FF;
			if (isLogDebugEnabled)
			{
				log_Renamed.debug(string.Format("{0} dx={1:D}, dy={2:D}", helper.getCommandString(command_Renamed), context.textureTx_dx, context.textureTx_dy));
			}
		}

		private void executeCommandTRXSIZE()
		{
			context.textureTx_width = (normalArgument & 0x3FF) + 1;
			context.textureTx_height = ((normalArgument >> 10) & 0x3FF) + 1;
			if (isLogDebugEnabled)
			{
				log_Renamed.debug(string.Format("{0} width={1:D}, height={2:D}", helper.getCommandString(command_Renamed), context.textureTx_width, context.textureTx_height));
			}
		}

		private void executeCommandVSCX()
		{
			int coordX = normalArgument & 0xFFFF;
			log_Renamed.warn("Unimplemented VSCX: coordX=" + coordX);
		}

		private void executeCommandVSCY()
		{
			int coordY = normalArgument & 0xFFFF;
			log_Renamed.warn("Unimplemented VSCY: coordY=" + coordY);
		}

		private void executeCommandVSCZ()
		{
			int coordZ = normalArgument & 0xFFFF;
			log_Renamed.warn("Unimplemented VSCZ: coordZ=" + coordZ);
		}

		private void executeCommandVTCS()
		{
			float coordS = floatArgument(normalArgument);
			log_Renamed.warn("Unimplemented VTCS: coordS=" + coordS);
		}

		private void executeCommandVTCT()
		{
			float coordT = floatArgument(normalArgument);
			log_Renamed.warn("Unimplemented VTCT: coordT=" + coordT);
		}

		private void executeCommandVTCQ()
		{
			float coordQ = floatArgument(normalArgument);
			log_Renamed.warn("Unimplemented VTCQ: coordQ=" + coordQ);
		}

		private void executeCommandVCV()
		{
			int colorR = normalArgument & 0xFF;
			int colorG = (normalArgument >> 8) & 0xFF;
			int colorB = (normalArgument >> 16) & 0xFF;
			log_Renamed.warn("Unimplemented VCV: colorR=" + colorR + ", colorG=" + colorG + ", colorB=" + colorB);
		}

		private void executeCommandVAP()
		{
			int color = normalArgument & 0xFF; // Vertex color (8bit unsigned).
			int prim_type = (normalArgument >> 8) & 0x7; // Primitive used.
			int antialias = (normalArgument >> 11) & 0x1; // Perform antialiasing or not (only for PRIM_LINE or PRIM_LINES_STRIPS).
			int clip = (normalArgument >> 12) & 0x3F; // Clipping value (6bit unsigned).
			int shading = (normalArgument >> 18) & 0x1; // Shading mode.
			int cull = (normalArgument >> 19) & 0x1; // Back face culling.
			int v_order = (normalArgument >> 20) & 0x1; // Vertex order (only for PRIM_TRIANGLE).
			int map_texture = (normalArgument >> 21) & 0x1; // Texture mapping.
			int fog = (normalArgument >> 22) & 0x1; // Fogging.
			int dither = (normalArgument >> 23) & 0x1; // Dithering.
			log_Renamed.warn(string.Format("Unimplemented VAP: color={0:D}, prim_type={1:D}, antialias={2:D}, clip={3:D}, shading={4:D}, cull={5:D}" + ", v_order={6:D}, map_texture={7:D}, fog={8:D}, dither={9:D}", color, prim_type, antialias, clip, shading, cull, v_order, map_texture, fog, dither));
		}

		private void executeCommandVFC()
		{
			int fog = normalArgument & 0xFF;
			log_Renamed.warn("Unimplemented VFC: fog=" + fog);
		}

		private void executeCommandVSCV()
		{
			int colorR2 = normalArgument & 0xFF;
			int colorG2 = (normalArgument >> 8) & 0xFF;
			int colorB2 = (normalArgument >> 16) & 0xFF;
			log_Renamed.warn("Unimplemented VSCV: colorR2=" + colorR2 + ", colorG2=" + colorG2 + ", colorB2=" + colorB2);
		}

		private void executeCommandDUMMY()
		{
			// This command always appears before a BOFS command and seems to have
			// no special meaning.
			// The command also appears sometimes after a PRIM command.

			// Confirmed on PSP to be a dummy command and can be safely ignored.
			// This commands' normalArgument may not be always 0, as it's totally
			// discarded on the PSP.
			if (isLogDebugEnabled)
			{
				log_Renamed.debug("Ignored DUMMY video command.");
			}
		}

		private void enableClientState(bool useVertexColor, bool useTexture)
		{
			if (useTexture)
			{
				re.enableClientState(pspsharp.graphics.RE.IRenderingEngine_Fields.RE_TEXTURE);
			}
			else
			{
				re.disableClientState(pspsharp.graphics.RE.IRenderingEngine_Fields.RE_TEXTURE);
			}
			if (useVertexColor)
			{
				re.enableClientState(pspsharp.graphics.RE.IRenderingEngine_Fields.RE_COLOR);
			}
			else
			{
				re.disableClientState(pspsharp.graphics.RE.IRenderingEngine_Fields.RE_COLOR);
			}
			if (context.vinfo.normal != 0)
			{
				re.enableClientState(pspsharp.graphics.RE.IRenderingEngine_Fields.RE_NORMAL);
			}
			else
			{
				re.disableClientState(pspsharp.graphics.RE.IRenderingEngine_Fields.RE_NORMAL);
			}
			re.enableClientState(pspsharp.graphics.RE.IRenderingEngine_Fields.RE_VERTEX);
		}

		private void setTexCoordPointer(bool useTexture, int nTexCoord, int type, int stride, int offset, bool isNative, bool useBufferManager)
		{
			if (useTexture)
			{
				if (!useBufferManager)
				{
					re.setTexCoordPointer(nTexCoord, type, stride, offset);
				}
				else if (isNative)
				{
					bufferManager.setTexCoordPointer(nativeBufferId, nTexCoord, type, context.vinfo.vertexSize, offset);
				}
				else
				{
					bufferManager.setTexCoordPointer(bufferId, nTexCoord, type, stride, offset);
				}
			}
		}

		private void setColorPointer(bool useVertexColor, int nColor, int type, int stride, int offset, bool isNative, bool useBufferManager)
		{
			if (useVertexColor)
			{
				if (!useBufferManager)
				{
					re.setColorPointer(nColor, type, stride, offset);
				}
				else if (isNative)
				{
					bufferManager.setColorPointer(nativeBufferId, nColor, type, context.vinfo.vertexSize, offset);
				}
				else
				{
					bufferManager.setColorPointer(bufferId, nColor, type, stride, offset);
				}
			}
		}

		private void setVertexPointer(int nVertex, int type, int stride, int offset, bool isNative, bool useBufferManager)
		{
			if (!useBufferManager)
			{
				re.setVertexPointer(nVertex, type, stride, offset);
			}
			else if (isNative)
			{
				bufferManager.setVertexPointer(nativeBufferId, nVertex, type, context.vinfo.vertexSize, offset);
			}
			else
			{
				bufferManager.setVertexPointer(bufferId, nVertex, type, stride, offset);
			}
		}

		private void setNormalPointer(int type, int stride, int offset, bool isNative, bool useBufferManager)
		{
			if (context.vinfo.normal != 0)
			{
				if (!useBufferManager)
				{
					re.setNormalPointer(type, stride, offset);
				}
				else if (isNative)
				{
					bufferManager.setNormalPointer(nativeBufferId, type, context.vinfo.vertexSize, offset);
				}
				else
				{
					bufferManager.setNormalPointer(bufferId, type, stride, offset);
				}
			}
		}

		private void setWeightPointer(int numberOfWeightsForBuffer, int type, int stride, int offset, bool isNative, bool useBufferManager)
		{
			if (numberOfWeightsForBuffer > 0)
			{
				if (!useBufferManager)
				{
					re.setWeightPointer(numberOfWeightsForBuffer, type, stride, offset);
				}
				else if (isNative)
				{
					re.setWeightPointer(numberOfWeightsForBuffer, type, context.vinfo.vertexSize, offset);
				}
				else
				{
					re.setWeightPointer(numberOfWeightsForBuffer, type, stride, offset);
				}
			}
		}

		private void setDataPointers(int nVertex, bool useVertexColor, int nColor, bool useTexture, int nTexCoord, bool useNormal, int numberOfWeightsForBuffer, bool useBufferManager)
		{
			int stride = 0, cpos = 0, npos = 0, vpos = 0, wpos = 0;

			if (context.vinfo.texture != 0 || useTexture)
			{
				stride += SIZEOF_FLOAT * nTexCoord;
				cpos = npos = vpos = stride;
			}
			if (useVertexColor)
			{
				stride += SIZEOF_FLOAT * 4;
				npos = vpos = stride;
			}
			if (useNormal)
			{
				stride += SIZEOF_FLOAT * 3;
				vpos = stride;
			}
			stride += SIZEOF_FLOAT * 3;
			if (numberOfWeightsForBuffer > 0)
			{
				wpos = stride;
				stride += SIZEOF_FLOAT * numberOfWeightsForBuffer;
			}

			enableClientState(useVertexColor, useTexture);
			setTexCoordPointer(useTexture, nTexCoord, pspsharp.graphics.RE.IRenderingEngine_Fields.RE_FLOAT, stride, 0, false, useBufferManager);
			setColorPointer(useVertexColor, nColor, pspsharp.graphics.RE.IRenderingEngine_Fields.RE_FLOAT, stride, cpos, false, useBufferManager);
			setNormalPointer(pspsharp.graphics.RE.IRenderingEngine_Fields.RE_FLOAT, stride, npos, false, useBufferManager);
			setWeightPointer(numberOfWeightsForBuffer, pspsharp.graphics.RE.IRenderingEngine_Fields.RE_FLOAT, stride, wpos, false, useBufferManager);
			setVertexPointer(nVertex, pspsharp.graphics.RE.IRenderingEngine_Fields.RE_FLOAT, stride, vpos, false, useBufferManager);
		}

		public virtual void doPositionSkinning(VertexInfo vinfo, float[] boneWeights, float[] position)
		{
			float x = 0, y = 0, z = 0;
			for (int i = 0; i < context.vinfo.skinningWeightCount; i++)
			{
				if (boneWeights[i] != 0)
				{
					x += (position[0] * context.bone_uploaded_matrix[i][0] + position[1] * context.bone_uploaded_matrix[i][3] + position[2] * context.bone_uploaded_matrix[i][6] + context.bone_uploaded_matrix[i][9]) * boneWeights[i];

					y += (position[0] * context.bone_uploaded_matrix[i][1] + position[1] * context.bone_uploaded_matrix[i][4] + position[2] * context.bone_uploaded_matrix[i][7] + context.bone_uploaded_matrix[i][10]) * boneWeights[i];

					z += (position[0] * context.bone_uploaded_matrix[i][2] + position[1] * context.bone_uploaded_matrix[i][5] + position[2] * context.bone_uploaded_matrix[i][8] + context.bone_uploaded_matrix[i][11]) * boneWeights[i];
				}
			}

			position[0] = x;
			position[1] = y;
			position[2] = z;
		}

		public virtual void doNormalSkinning(VertexInfo vinfo, float[] boneWeights, float[] normal)
		{
			float nx = 0, ny = 0, nz = 0;
			for (int i = 0; i < context.vinfo.skinningWeightCount; i++)
			{
				if (boneWeights[i] != 0)
				{
					// Normals shouldn't be translated :)
					nx += (normal[0] * context.bone_uploaded_matrix[i][0] + normal[1] * context.bone_uploaded_matrix[i][3] + normal[2] * context.bone_uploaded_matrix[i][6]) * boneWeights[i];

					ny += (normal[0] * context.bone_uploaded_matrix[i][1] + normal[1] * context.bone_uploaded_matrix[i][4] + normal[2] * context.bone_uploaded_matrix[i][7]) * boneWeights[i];

					nz += (normal[0] * context.bone_uploaded_matrix[i][2] + normal[1] * context.bone_uploaded_matrix[i][5] + normal[2] * context.bone_uploaded_matrix[i][8]) * boneWeights[i];
				}
			}

			/*
			 // TODO: I doubt psp hardware normalizes normals after skinning,
			 // but if it does, this should be uncommented :)
			 float Length = nx*nx + ny*ny + nz*nz;
	
			 if (Length > 0.f) {
			 Length = 1.f / (float)Math.Sqrt(Length);
	
			 nx *= Length;
			 ny *= Length;
			 nz *= Length;
			 }
			 */
			normal[0] = nx;
			normal[1] = ny;
			normal[2] = nz;
		}

		public static void doSkinning(float[][] boneMatrix, VertexInfo vinfo, VertexState v)
		{
			float x = 0, y = 0, z = 0;
			float nx = 0, ny = 0, nz = 0;
			bool hasNormal = vinfo.normal != 0;
			for (int i = 0; i < vinfo.skinningWeightCount; ++i)
			{
				float boneWeight = v.boneWeights[i];
				if (boneWeight != 0.0f)
				{
					x += (v.p[0] * boneMatrix[i][0] + v.p[1] * boneMatrix[i][3] + v.p[2] * boneMatrix[i][6] + boneMatrix[i][9]) * boneWeight;

					y += (v.p[0] * boneMatrix[i][1] + v.p[1] * boneMatrix[i][4] + v.p[2] * boneMatrix[i][7] + boneMatrix[i][10]) * boneWeight;

					z += (v.p[0] * boneMatrix[i][2] + v.p[1] * boneMatrix[i][5] + v.p[2] * boneMatrix[i][8] + boneMatrix[i][11]) * boneWeight;

					if (hasNormal)
					{
						// Normals shouldn't be translated :)
						nx += (v.n[0] * boneMatrix[i][0] + v.n[1] * boneMatrix[i][3] + v.n[2] * boneMatrix[i][6]) * boneWeight;

						ny += (v.n[0] * boneMatrix[i][1] + v.n[1] * boneMatrix[i][4] + v.n[2] * boneMatrix[i][7]) * boneWeight;

						nz += (v.n[0] * boneMatrix[i][2] + v.n[1] * boneMatrix[i][5] + v.n[2] * boneMatrix[i][8]) * boneWeight;
					}
				}
			}

			v.p[0] = x;
			v.p[1] = y;
			v.p[2] = z;

			/*
			 // TODO: I doubt psp hardware normalizes normals after skinning,
			 // but if it does, this should be uncommented :)
			 float Length = nx*nx + ny*ny + nz*nz;
	
			 if (Length > 0.f) {
			 Length = 1.f / (float)Math.Sqrt(Length);
	
			 nx *= Length;
			 ny *= Length;
			 nz *= Length;
			 }
			 */
			if (hasNormal)
			{
				v.n[0] = nx;
				v.n[1] = ny;
				v.n[2] = nz;
			}
		}

		private void log(string commandString, float[] matrix)
		{
			if (isLogDebugEnabled)
			{
				for (int y = 0; y < 4; y++)
				{
					log(string.Format("{0} {1:F4} {2:F4} {3:F4} {4:F4}", commandString, matrix[0 + y * 4], matrix[1 + y * 4], matrix[2 + y * 4], matrix[3 + y * 4]));
				}
			}
		}

		public virtual bool isVideoTexture(int tex_addr)
		{
			if (videoTextures.Count > 0)
			{
				// Synchronize the access to videoTextures as it can be accessed
				// from a parallel threads (async display and PSP thread)
				lock (videoTextures)
				{
					foreach (AddressRange addressRange in videoTextures)
					{
						if (addressRange.contains(tex_addr))
						{
							if (log_Renamed.DebugEnabled)
							{
								log_Renamed.debug(string.Format("Texture at 0x{0:X8} is a video texture", tex_addr));
							}
							return true;
						}
					}
				}
			}

			return false;
		}

		private bool canCacheTexture(int tex_addr)
		{
			if (!useTextureCache)
			{
				return false;
			}

			// Some games are storing compressed textures in VRAM (e.g. Skate Park City).
			if (context.texture_storage >= TPSM_PIXEL_STORAGE_MODE_DXT1)
			{
				return true;
			}

			// Do not cache the texture if we are using the current GE directly as a texture
			if (display.isGeAddress(tex_addr))
			{
				return false;
			}

			if (isVideoTexture(tex_addr))
			{
				return false;
			}

			return true;
		}

		private GETexture FlippedTexture
		{
			set
			{
				// Textures are normally created as flipped textures (upside-down)
				// but the GE textures are not flipped, so we have to flip
				// them when rendering
				float newTextureFlipTranslateY = value.Height / (float) context.texture_height[0];
				if (!textureFlipped || textureFlipTranslateY != newTextureFlipTranslateY)
				{
					textureFlipped = true;
					textureFlipTranslateY = newTextureFlipTranslateY;
					textureMatrixUpload.Changed = true;
				}
			}
		}

		private bool loadGETexture(int tex_addr)
		{
			if (!display.SaveGEToTexture || isVideoTexture(tex_addr))
			{
				return false;
			}

			int widthGe = display.WidthGe;
			int heightGe = display.HeightGe;
			int bufferWidth = context.texture_buffer_width[0];
			int pixelFormat = context.texture_storage;

			GETexture geTexture;
			if (pspsharp.graphics.RE.IRenderingEngine_Fields.isTextureTypeIndexed[pixelFormat])
			{
				if (pixelFormat == TPSM_PIXEL_STORAGE_MODE_32BIT_INDEXED)
				{
					geTexture = GETextureManager.Instance.checkGETexture(tex_addr, bufferWidth, widthGe, heightGe, TPSM_PIXEL_STORAGE_MODE_32BIT_ABGR8888);
				}
				else
				{
					// We only know that the texture is 16-bit indexed, but we don't
					// know its exact pixel format (5650, 5551 or 4444)
					geTexture = null;

					if (context.tex_clut_mode >= TPSM_PIXEL_STORAGE_MODE_16BIT_BGR5650 && context.tex_clut_mode <= TPSM_PIXEL_STORAGE_MODE_16BIT_ABGR4444)
					{
						// First try with the same pixel format as the texture clut
						geTexture = GETextureManager.Instance.checkGETexture(tex_addr, bufferWidth, widthGe, heightGe, context.tex_clut_mode);
					}

					if (geTexture == null)
					{
						// As a last chance, try all the pixel formats: 5650, 5551, 4444
						for (int i = TPSM_PIXEL_STORAGE_MODE_16BIT_BGR5650; i <= TPSM_PIXEL_STORAGE_MODE_16BIT_ABGR4444; i++)
						{
							geTexture = GETextureManager.Instance.checkGETexture(tex_addr, bufferWidth, widthGe, heightGe, i);
							if (geTexture != null)
							{
								break;
							}
						}
					}
				}

				if (geTexture != null)
				{
					if (tex_addr == display.TopAddrGe)
					{
						// Using the active GE as texture
						geTexture.copyScreenToTexture(re);
					}

					if (!re.canNativeClut(tex_addr, pixelFormat, context.texture_swizzle) || context.texture_swizzle)
					{
						// Save the texture to memory, it will be reloaded using the CLUT
						geTexture.copyTextureToMemory(re);
						return false;
					}

					geTexture = GETextureManager.Instance.getGEIndexedTexture(re, geTexture, tex_addr, bufferWidth, context.texture_width[0], context.texture_height[0], pixelFormat);
					geTexture.bind(re, true);
					context.currentTextureId = geTexture.TextureId;
					FlippedTexture = geTexture;

					return true;
				}
			}
			else
			{
				geTexture = GETextureManager.Instance.checkGETexture(tex_addr, bufferWidth, widthGe, heightGe, pixelFormat);
			}

			if (geTexture == null)
			{
				return false;
			}

			if (tex_addr == display.TopAddrGe)
			{
				// Using the active GE as texture
				geTexture.copyScreenToTexture(re);
			}

			int width = context.texture_width[0];
			int height = context.texture_height[0];

			if (geTexture.WidthPow2 == width && geTexture.HeightPow2 == height)
			{
				if (isLogDebugEnabled)
				{
					log_Renamed.debug(string.Format("Reusing GETexture {0}", geTexture));
				}
			}
			else
			{
				// Resize the GETexture to the requested texture size
				if (isLogDebugEnabled)
				{
					log_Renamed.debug(string.Format("Resizing GETexture {0} to {1:D}x{2:D}", geTexture, width, height));
				}
				geTexture = GETextureManager.Instance.getGEResizedTexture(re, geTexture, tex_addr, bufferWidth, width, height, pixelFormat);
			}
			geTexture.bind(re, true);
			context.currentTextureId = geTexture.TextureId;
			FlippedTexture = geTexture;

			return true;
		}

		private int getValidNumberMipmaps(bool silent)
		{
			for (int level = 0; level <= context.texture_num_mip_maps; level++)
			{
				int texaddr = context.texture_base_pointer[level] & Memory.addressMask;
				if (!Memory.isAddressGood(texaddr))
				{
					if (texaddr == 0)
					{
						if (isLogDebugEnabled)
						{
							log_Renamed.debug(string.Format("Invalid texture address 0x{0:X8} for texture level {1:D}", texaddr, level));
						}
					}
					else
					{
						if (isLogWarnEnabled)
						{
							log_Renamed.warn(string.Format("Invalid texture address 0x{0:X8} for texture level {1:D}", texaddr, level));
						}
					}
					return System.Math.Max(level - 1, 0);
				}

				if (level > 0)
				{
					int previousWidth = context.texture_width[level - 1];
					int currentWidth = context.texture_width[level];
					int previousHeight = context.texture_height[level - 1];
					int currentHeight = context.texture_height[level];
					if (currentWidth * 2 != previousWidth || currentHeight * 2 != previousHeight)
					{
						if (!silent && isLogWarnEnabled)
						{
							log_Renamed.warn(string.Format("Texture mipmap with invalid dimension at level {0:D}: ({1:D}x{2:D})@0x{3:X8} -> ({4:D}x{5:D})@0x{6:X8}", level, previousWidth, previousHeight, context.texture_base_pointer[level - 1] & Memory.addressMask, currentWidth, currentHeight, texaddr));
							if (context.tex_mipmap_mode == TBIAS_MODE_CONST && context.tex_mipmap_bias_int >= level)
							{
								log_Renamed.warn(string.Format("... and this invalid Texture mipmap will be used with mipmap_mode={0:D}, mipmap_bias={1:D}", context.tex_mipmap_mode, context.tex_mipmap_bias_int));
							}
						}
						return level - 1;
					}
				}
			}

			return context.texture_num_mip_maps;
		}

		private string ModdedTextureDirectory
		{
			get
			{
				string tmpPath = Settings.Instance.readString("emu.tmppath");
				char sep = System.IO.Path.DirectorySeparatorChar;
				return string.Format("{0}{1}{2}{3}Textures{4}", tmpPath, sep, State.discId, sep, sep);
			}
		}

		private bool hasModdedTexture(int level, StringBuilder moddedTextureFileName)
		{
			// Quick check: if there is no Texture directory, no need to check for an image file
			if (!hasModdedTextureDirectory)
			{
				return false;
			}

			string directory = ModdedTextureDirectory;
			int origLength = moddedTextureFileName.Length;
			foreach (string extension in ImageIO.ReaderFileSuffixes)
			{
				moddedTextureFileName.Length = origLength;
				string fileNameSuffix = "";
				if (pspsharp.graphics.RE.IRenderingEngine_Fields.isTextureTypeIndexed[context.texture_storage])
				{
					// For textures using a CLUT, add the clut address to the file name.
					fileNameSuffix = string.Format("_{0:X8}", context.tex_clut_addr);
				}
				moddedTextureFileName.Append(string.Format("{0}Image{1:X8}{2}.{3}", directory, context.texture_base_pointer[level], fileNameSuffix, extension));
				File moddedTextureFile = new File(moddedTextureFileName.ToString());
				if (moddedTextureFile.canRead())
				{
					return true;
				}
			}

			return false;
		}

		public static int alignBufferWidth(int bufferWidth, int pixelFormat)
		{
			int alignment = pspsharp.graphics.RE.IRenderingEngine_Fields.alignementOfTextureBufferWidth[pixelFormat] - 1;
			if (alignment <= 0)
			{
				return bufferWidth;
			}
			bufferWidth &= ~alignment;

			if (bufferWidth == 0)
			{
				// bufferWidth of 0 is the same as the smallest valid bufferWidth
				// (tested on PSP)
				bufferWidth = alignment + 1;
			}

			return bufferWidth;
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public static int readLittleEndianShort(java.io.InputStream is) throws java.io.IOException
		public static int readLittleEndianShort(System.IO.Stream @is)
		{
			sbyte[] buffer = new sbyte[2];
			if (@is.Read(buffer, 0, buffer.Length) != buffer.Length)
			{
				return -1;
			}

			return (buffer[0] & 0xFF) | ((buffer[1] & 0xFF) << 8);
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public static int readLittleEndianInt(java.io.InputStream is) throws java.io.IOException
		public static int readLittleEndianInt(System.IO.Stream @is)
		{
			sbyte[] buffer = new sbyte[4];
			if (@is.Read(buffer, 0, buffer.Length) != buffer.Length)
			{
				return -1;
			}

			return (buffer[0] & 0xFF) | ((buffer[1] & 0xFF) << 8) | ((buffer[2] & 0xFF) << 16) | ((buffer[3] & 0xFF) << 24);
		}

		private void loadTexture(int level, int reTextureLevel)
		{
			Buffer final_buffer;
			int texclut = context.tex_clut_addr;

			int textureByteAlignment = 4; // 32 bits
			bool compressedTexture = false;

			// Extract texture information with the minor conversion possible
			// TODO: Get rid of information copying, and implement all the available formats
			int texaddr = context.texture_base_pointer[level] & Memory.addressMask;
			int compressedTextureSize = 0;
			int buffer_storage = context.texture_storage;
			// The texture buffer width must be a multiple of 4, 8, 16 or 32 depending
			// on the pixel format
			int textureBufferWidthInPixels = alignBufferWidth(context.texture_buffer_width[level], context.texture_storage);

			StringBuilder moddedTextureFileName = new StringBuilder();
			if (enableTextureModding && hasModdedTexture(level, moddedTextureFileName))
			{
				try
				{
					File moddedTextureFile = new File(moddedTextureFileName.ToString());
					int width = System.Math.Max(textureBufferWidthInPixels, context.texture_width[level]);
					int height = context.texture_height[level];
					bool readUsingImageIO = true;

					// ImageIO is losing the alpha color values while reading a BMP file :-(.
					// Read BMP files manually to retrieve the correct alpha color values.
					if (moddedTextureFile.Name.EndsWith(".bmp"))
					{
						System.IO.Stream @is = new BufferedInputStream(new System.IO.FileStream(moddedTextureFile, System.IO.FileMode.Open, System.IO.FileAccess.Read));

						// Reading file header
						int magic = readLittleEndianShort(@is);
						int fileSize = readLittleEndianInt(@is);
						@is.skip(4);
						int dataOffset = readLittleEndianInt(@is);

						// Reading DIB header
						int dibHeaderLength = readLittleEndianInt(@is);
						int imageWidth = readLittleEndianInt(@is);
						int imageHeight = readLittleEndianInt(@is);
						int numberPlanes = readLittleEndianShort(@is);
						int bitsPerPixel = readLittleEndianShort(@is);

						// Skip rest of DIB header until data start
						@is.skip(dataOffset - 14 - 16);

						if (magic == (('M' << 8) | 'B') && dibHeaderLength >= 16 && fileSize >= dataOffset && numberPlanes == 1 && bitsPerPixel == 32 && imageWidth == width && imageHeight == height)
						{
							for (int y = height - 1; y >= 0; y--)
							{
								for (int x = 0, i = y * width; x < width; x++, i++)
								{
									int argb = readLittleEndianInt(@is);
									// Convert ARGB into ABGR
									int abgr = (argb & unchecked((int)0xFF00FF00)) | ((argb & 0x00FF0000) >> 16) | ((argb & 0x000000FF) << 16);
									tmp_texture_buffer32[i] = abgr;
								}
							}
							readUsingImageIO = false;
						}

						@is.Close();
					}

					if (readUsingImageIO)
					{
						System.Drawing.Bitmap img = ImageIO.read(moddedTextureFile);
						for (int y = height - 1; y >= 0; y--)
						{
							for (int x = 0, i = y * width; x < width; x++, i++)
							{
								int argb = img.getRGB(x, y);
								// Convert ARGB into ABGR
								int abgr = (argb & unchecked((int)0xFF00FF00)) | ((argb & 0x00FF0000) >> 16) | ((argb & 0x000000FF) << 16);
								tmp_texture_buffer32[i] = abgr;
							}
						}
					}

					final_buffer = IntBuffer.wrap(tmp_texture_buffer32);
					textureByteAlignment = 4;
					buffer_storage = TPSM_PIXEL_STORAGE_MODE_32BIT_ABGR8888;
				}
				catch (IOException e)
				{
					log_Renamed.error("Error while reading modded texture file", e);
					return;
				}
			}
			else
			{
				switch (context.texture_storage)
				{
					case TPSM_PIXEL_STORAGE_MODE_4BIT_INDEXED:
					{
						if (texclut == 0)
						{
							return;
						}

						buffer_storage = context.tex_clut_mode;
						switch (context.tex_clut_mode)
						{
							case CMODE_FORMAT_16BIT_BGR5650:
							case CMODE_FORMAT_16BIT_ABGR5551:
							case CMODE_FORMAT_16BIT_ABGR4444:
							{
								textureByteAlignment = 2; // 16 bits
								short[] clut = readClut16(level);
								int clutSharingOffset = context.mipmapShareClut ? 0 : level * 16;

								if (!context.texture_swizzle)
								{
									int Length = System.Math.Max(textureBufferWidthInPixels, context.texture_width[level]) * context.texture_height[level];
									IMemoryReader memoryReader = MemoryReader.getMemoryReader(texaddr, Length / 2, 1);
									for (int i = 0; i < Length; i += 2)
									{
										int index = memoryReader.readNext();

										tmp_texture_buffer16[i] = clut[getClutIndex(index & 0xF) + clutSharingOffset];
										tmp_texture_buffer16[i + 1] = clut[getClutIndex((index >> 4) & 0xF) + clutSharingOffset];
									}
									final_buffer = ShortBuffer.wrap(tmp_texture_buffer16);

									if (State.captureGeNextFrame)
									{
										log_Renamed.info("Capture loadTexture clut 4/16 unswizzled");
										CaptureManager.captureRAM(texaddr, Length / 2);
									}
								}
								else
								{
									unswizzleTextureFromMemory(texaddr, 0, level, textureBufferWidthInPixels);
									int pixels = textureBufferWidthInPixels * context.texture_height[level];
									for (int i = 0, j = 0; i < pixels; i += 8, j++)
									{
										int n = tmp_texture_buffer32[j];
										int index = n & 0xF;
										tmp_texture_buffer16[i + 0] = clut[getClutIndex(index) + clutSharingOffset];
										index = (n >> 4) & 0xF;
										tmp_texture_buffer16[i + 1] = clut[getClutIndex(index) + clutSharingOffset];
										index = (n >> 8) & 0xF;
										tmp_texture_buffer16[i + 2] = clut[getClutIndex(index) + clutSharingOffset];
										index = (n >> 12) & 0xF;
										tmp_texture_buffer16[i + 3] = clut[getClutIndex(index) + clutSharingOffset];
										index = (n >> 16) & 0xF;
										tmp_texture_buffer16[i + 4] = clut[getClutIndex(index) + clutSharingOffset];
										index = (n >> 20) & 0xF;
										tmp_texture_buffer16[i + 5] = clut[getClutIndex(index) + clutSharingOffset];
										index = (n >> 24) & 0xF;
										tmp_texture_buffer16[i + 6] = clut[getClutIndex(index) + clutSharingOffset];
										index = (n >> 28) & 0xF;
										tmp_texture_buffer16[i + 7] = clut[getClutIndex(index) + clutSharingOffset];
									}
									final_buffer = ShortBuffer.wrap(tmp_texture_buffer16);
									break;
								}

								break;
							}

							case CMODE_FORMAT_32BIT_ABGR8888:
							{
								int[] clut = readClut32(level);
								int clutSharingOffset = context.mipmapShareClut ? 0 : level * 16;

								if (!context.texture_swizzle)
								{
									int Length = System.Math.Max(textureBufferWidthInPixels, context.texture_width[level]) * context.texture_height[level];
									IMemoryReader memoryReader = MemoryReader.getMemoryReader(texaddr, Length / 2, 1);
									for (int i = 0; i < Length; i += 2)
									{
										int index = memoryReader.readNext();

										tmp_texture_buffer32[i + 1] = clut[getClutIndex((index >> 4) & 0xF) + clutSharingOffset];
										tmp_texture_buffer32[i] = clut[getClutIndex(index & 0xF) + clutSharingOffset];
									}
									final_buffer = IntBuffer.wrap(tmp_texture_buffer32);

									if (State.captureGeNextFrame)
									{
										log_Renamed.info("Capture loadTexture clut 4/32 unswizzled");
										CaptureManager.captureRAM(texaddr, Length / 2);
									}
								}
								else
								{
									unswizzleTextureFromMemory(texaddr, 0, level, textureBufferWidthInPixels);
									int pixels = textureBufferWidthInPixels * context.texture_height[level];
									for (int i = pixels - 8, j = (pixels / 8) - 1; i >= 0; i -= 8, j--)
									{
										int n = tmp_texture_buffer32[j];
										int index = n & 0xF;
										tmp_texture_buffer32[i + 0] = clut[getClutIndex(index) + clutSharingOffset];
										index = (n >> 4) & 0xF;
										tmp_texture_buffer32[i + 1] = clut[getClutIndex(index) + clutSharingOffset];
										index = (n >> 8) & 0xF;
										tmp_texture_buffer32[i + 2] = clut[getClutIndex(index) + clutSharingOffset];
										index = (n >> 12) & 0xF;
										tmp_texture_buffer32[i + 3] = clut[getClutIndex(index) + clutSharingOffset];
										index = (n >> 16) & 0xF;
										tmp_texture_buffer32[i + 4] = clut[getClutIndex(index) + clutSharingOffset];
										index = (n >> 20) & 0xF;
										tmp_texture_buffer32[i + 5] = clut[getClutIndex(index) + clutSharingOffset];
										index = (n >> 24) & 0xF;
										tmp_texture_buffer32[i + 6] = clut[getClutIndex(index) + clutSharingOffset];
										index = (n >> 28) & 0xF;
										tmp_texture_buffer32[i + 7] = clut[getClutIndex(index) + clutSharingOffset];
									}
									final_buffer = IntBuffer.wrap(tmp_texture_buffer32);
								}

								break;
							}

							default:
							{
								error("Unhandled clut4 texture mode " + context.tex_clut_mode);
								return;
							}
						}

						break;
					}
					case TPSM_PIXEL_STORAGE_MODE_8BIT_INDEXED:
					{
						if (re.canNativeClut(texaddr, context.texture_storage, context.texture_swizzle))
						{
							final_buffer = getTextureBuffer(texaddr, 1, level, textureBufferWidthInPixels);
							textureByteAlignment = 1; // 8 bits
						}
						else
						{
							final_buffer = readIndexedTexture(level, texaddr, texclut, 1, textureBufferWidthInPixels);
							buffer_storage = context.tex_clut_mode;
							textureByteAlignment = textureByteAlignmentMapping[context.tex_clut_mode];
						}
						break;
					}
					case TPSM_PIXEL_STORAGE_MODE_16BIT_INDEXED:
					{
						if (re.canNativeClut(texaddr, context.texture_storage, context.texture_swizzle))
						{
							final_buffer = getTextureBuffer(texaddr, 2, level, textureBufferWidthInPixels);
							textureByteAlignment = 2; // 16 bits
						}
						else
						{
							final_buffer = readIndexedTexture(level, texaddr, texclut, 2, textureBufferWidthInPixels);
							buffer_storage = context.tex_clut_mode;
							textureByteAlignment = textureByteAlignmentMapping[context.tex_clut_mode];
						}
						break;
					}
					case TPSM_PIXEL_STORAGE_MODE_32BIT_INDEXED:
					{
						if (re.canNativeClut(texaddr, context.texture_storage, context.texture_swizzle))
						{
							final_buffer = getTextureBuffer(texaddr, 4, level, textureBufferWidthInPixels);
							textureByteAlignment = 4; // 32 bits
						}
						else
						{
							final_buffer = readIndexedTexture(level, texaddr, texclut, 4, textureBufferWidthInPixels);
							buffer_storage = context.tex_clut_mode;
							textureByteAlignment = textureByteAlignmentMapping[context.tex_clut_mode];
						}
						break;
					}
					case TPSM_PIXEL_STORAGE_MODE_16BIT_BGR5650:
					case TPSM_PIXEL_STORAGE_MODE_16BIT_ABGR5551:
					case TPSM_PIXEL_STORAGE_MODE_16BIT_ABGR4444:
					{
						textureByteAlignment = 2; // 16 bits

						if (!context.texture_swizzle)
						{
							int Length = System.Math.Max(textureBufferWidthInPixels, context.texture_width[level]) * context.texture_height[level];
							final_buffer = Memory.Instance.getBuffer(texaddr, Length * 2);
							if (final_buffer == null)
							{
								IMemoryReader memoryReader = MemoryReader.getMemoryReader(texaddr, Length * 2, 2);
								for (int i = 0; i < Length; i++)
								{
									int pixel = memoryReader.readNext();
									tmp_texture_buffer16[i] = (short) pixel;
								}

								final_buffer = ShortBuffer.wrap(tmp_texture_buffer16);
							}

							if (State.captureGeNextFrame)
							{
								log_Renamed.info("Capture loadTexture 16 unswizzled");
								CaptureManager.captureRAM(texaddr, Length * 2);
							}
						}
						else
						{
							final_buffer = unswizzleTextureFromMemory(texaddr, 2, level, textureBufferWidthInPixels);
						}

						break;
					}

					case TPSM_PIXEL_STORAGE_MODE_32BIT_ABGR8888:
					{
						final_buffer = getTextureBuffer(texaddr, 4, level, textureBufferWidthInPixels);
						break;
					}

					case TPSM_PIXEL_STORAGE_MODE_DXT1:
					{
						if (isLogDebugEnabled)
						{
							log_Renamed.debug("Loading texture TPSM_PIXEL_STORAGE_MODE_DXT1 " + texaddr.ToString("x"));
						}
						compressedTexture = true;
						compressedTextureSize = getCompressedTextureSize(level, 8);
						IMemoryReader memoryReader = MemoryReader.getMemoryReader(texaddr, compressedTextureSize, 4);
						// PSP DXT1 hardware format reverses the colors and the per-pixel
						// bits, and encodes the color in RGB 565 format
						int i = 0;
						int bufferWidth4 = Round4(context.texture_buffer_width[level]);
						int width4 = Round4(context.texture_width[level]);
						int height4 = Round4(context.texture_height[level]);
						int readWidth = min(bufferWidth4, width4);
						// Skip width if the buffer width is larger than the texture width
						int skipWidth = max(0, (bufferWidth4 - width4) >> 1);
						for (int y = 0; y < height4; y += 4)
						{
							for (int x = 0; x < readWidth; x += 4, i += 2)
							{
								tmp_texture_buffer32[i + 1] = memoryReader.readNext();
								tmp_texture_buffer32[i + 0] = memoryReader.readNext();
							}
							memoryReader.skip(skipWidth);
							for (int x = readWidth; x < width4; x += 4, i += 2)
							{
								tmp_texture_buffer32[i + 0] = 0;
								tmp_texture_buffer32[i + 1] = 0;
							}
						}
						final_buffer = IntBuffer.wrap(tmp_texture_buffer32);
						break;
					}

					case TPSM_PIXEL_STORAGE_MODE_DXT3:
					{
						if (isLogDebugEnabled)
						{
							log_Renamed.debug("Loading texture TPSM_PIXEL_STORAGE_MODE_DXT3 " + texaddr.ToString("x"));
						}
						compressedTexture = true;
						compressedTextureSize = getCompressedTextureSize(level, 4);
						IMemoryReader memoryReader = MemoryReader.getMemoryReader(texaddr, compressedTextureSize, 4);
						// PSP DXT3 format reverses the alpha and color parts of each block,
						// and reverses the color and per-pixel terms in the color part.
						int i = 0;
						int bufferWidth4 = Round4(context.texture_buffer_width[level]);
						int width4 = Round4(context.texture_width[level]);
						int height4 = Round4(context.texture_height[level]);
						int readWidth = min(bufferWidth4, width4);
						// Skip width if the buffer width is larger than the texture width
						int skipWidth = max(0, bufferWidth4 - width4);
						for (int y = 0; y < height4; y += 4)
						{
							for (int x = 0; x < readWidth; x += 4, i += 4)
							{
								// Color
								tmp_texture_buffer32[i + 3] = memoryReader.readNext();
								tmp_texture_buffer32[i + 2] = memoryReader.readNext();
								// Alpha
								tmp_texture_buffer32[i + 0] = memoryReader.readNext();
								tmp_texture_buffer32[i + 1] = memoryReader.readNext();
							}
							memoryReader.skip(skipWidth);
							for (int x = readWidth; x < width4; x += 4, i += 4)
							{
								tmp_texture_buffer32[i + 0] = 0;
								tmp_texture_buffer32[i + 1] = 0;
								tmp_texture_buffer32[i + 2] = 0;
								tmp_texture_buffer32[i + 3] = 0;
							}
						}
						final_buffer = IntBuffer.wrap(tmp_texture_buffer32);
						break;
					}

					case TPSM_PIXEL_STORAGE_MODE_DXT5:
					{
						if (isLogDebugEnabled)
						{
							log_Renamed.debug("Loading texture TPSM_PIXEL_STORAGE_MODE_DXT5 " + texaddr.ToString("x"));
						}
						compressedTexture = true;
						compressedTextureSize = getCompressedTextureSize(level, 4);
						IMemoryReader memoryReader = MemoryReader.getMemoryReader(texaddr, compressedTextureSize, 2);
						// PSP DXT5 format reverses the alpha and color parts of each block,
						// and reverses the color and per-pixel terms in the color part. In
						// the alpha part, the 2 reference alpha values are swapped with the
						// alpha interpolation values.
						int i = 0;
						int bufferWidth4 = Round4(context.texture_buffer_width[level]);
						int width4 = Round4(context.texture_width[level]);
						int height4 = Round4(context.texture_height[level]);
						int readWidth = min(bufferWidth4, width4);
						// Skip width if the buffer width is larger than the texture width
						int skipWidth = max(0, bufferWidth4 - width4);
						for (int y = 0; y < height4; y += 4)
						{
							for (int x = 0; x < readWidth; x += 4, i += 8)
							{
								// Color
								tmp_texture_buffer16[i + 6] = (short) memoryReader.readNext();
								tmp_texture_buffer16[i + 7] = (short) memoryReader.readNext();
								tmp_texture_buffer16[i + 4] = (short) memoryReader.readNext();
								tmp_texture_buffer16[i + 5] = (short) memoryReader.readNext();
								// Alpha
								tmp_texture_buffer16[i + 1] = (short) memoryReader.readNext();
								tmp_texture_buffer16[i + 2] = (short) memoryReader.readNext();
								tmp_texture_buffer16[i + 3] = (short) memoryReader.readNext();
								tmp_texture_buffer16[i + 0] = (short) memoryReader.readNext();
							}
							memoryReader.skip(skipWidth);
							for (int x = readWidth; x < width4; x += 4, i += 8)
							{
								tmp_texture_buffer16[i + 0] = 0;
								tmp_texture_buffer16[i + 1] = 0;
								tmp_texture_buffer16[i + 2] = 0;
								tmp_texture_buffer16[i + 3] = 0;
								tmp_texture_buffer16[i + 4] = 0;
								tmp_texture_buffer16[i + 5] = 0;
								tmp_texture_buffer16[i + 6] = 0;
								tmp_texture_buffer16[i + 7] = 0;
							}
						}
						final_buffer = ShortBuffer.wrap(tmp_texture_buffer16);
						break;
					}

					default:
					{
						error("Unhandled texture storage " + context.texture_storage);
						return;
					}
				}
			}

			// Check if scaling is needed for xBRZ.
			bool scale = UsexBRZFilter;
			if ((texaddr > 83886080) && (texaddr < 142606336))
			{
				scale = false;
			}

			// Upload texture to openGL.
			re.setPixelStore(textureBufferWidthInPixels, textureByteAlignment);

			if (compressedTexture)
			{
				re.setCompressedTexImage(reTextureLevel, buffer_storage, context.texture_width[level], context.texture_height[level], compressedTextureSize, final_buffer);
			}
			else if (UsexBRZFilter && scale)
			{
				int textureSize = System.Math.Max(textureBufferWidthInPixels, this.context.texture_width[level]) * this.context.texture_height[level] * textureByteAlignment;
				this.re.setTexImagexBRZ(reTextureLevel, buffer_storage, this.context.texture_width[level], this.context.texture_height[level], this.context.texture_buffer_width[level], buffer_storage, buffer_storage, textureSize, final_buffer);
			}
			else
			{
				int textureSize = System.Math.Max(textureBufferWidthInPixels, context.texture_width[level]) * context.texture_height[level] * textureByteAlignment;
				re.setTexImage(reTextureLevel, buffer_storage, context.texture_width[level], context.texture_height[level], buffer_storage, buffer_storage, textureSize, final_buffer);
			}

			if (State.captureGeNextFrame)
			{
				bool vramImage = Memory.isVRAM(texaddr);
				bool overwriteFile = !vramImage;
				if (vramImage || !CaptureManager.isImageCaptured(texaddr))
				{
					CaptureManager.captureImage(texaddr, level, final_buffer, context.texture_width[level], context.texture_height[level], context.texture_buffer_width[level], buffer_storage, compressedTexture, compressedTextureSize, false, overwriteFile);
				}
			}
		}

		private void loadTexture()
		{
			if (display.isGeAddress(context.texture_base_pointer[0]))
			{
				textureChanged = true;
				// wait for rendering completion when using the current GE as a texture...
				re.waitForRenderingCompletion();
			}

			// No need to reload or check the texture cache if no texture parameter
			// has been changed since last call loadTexture()
			if (!textureChanged)
			{
				return;
			}

			// HACK: avoid texture uploads of null pointers
			// This can come from Sony's GE init code (pspsdk GE init is ok)
			if (context.texture_base_pointer[0] == 0)
			{
				return;
			}

			// Texture not used when disabled or in clear mode.
			if (!context.textureFlag.Enabled || context.clearMode)
			{
				return;
			}

			int mipmapIndex = 0;
			//
			// Basic support of mipmaps that are indexed with a constant value (TBIAS_MODE_CONST).
			//
			// Load such a mipmap as a stand-alone texture. Some applications do define
			// multiple mipmap levels all having the same size (width & height).
			// The application is then accessing each mipmap with TBIAS_MODE_CONST.
			// Such a case is easier being implemented with independent textures
			// instead of trying to use OpenGL texture mipmaps
			// (which do require decreasing sizes at each level).
			if (context.tex_mipmap_mode == TBIAS_MODE_CONST)
			{
				mipmapIndex = System.Math.Max(context.tex_mipmap_bias_int, 0);
			}

			int tex_addr = context.texture_base_pointer[mipmapIndex] & Memory.addressMask;
			if (!Memory.isAddressGood(tex_addr))
			{
				if (isLogWarnEnabled)
				{
					log_Renamed.warn(string.Format("Invalid texture address 0x{0:X8} for texture level 0", tex_addr));
				}
				return;
			}

			re.setTextureFormat(context.texture_storage, context.texture_swizzle);
			bool compressedTexture = (context.texture_storage >= TPSM_PIXEL_STORAGE_MODE_DXT1 && context.texture_storage <= TPSM_PIXEL_STORAGE_MODE_DXT5);

			if (loadGETexture(tex_addr))
			{
				re.TextureMipmapMagFilter = context.tex_mag_filter;
				re.TextureMipmapMinFilter = context.tex_min_filter;
				checkTextureMinFilter(compressedTexture, context.texture_num_mip_maps);
				textureChanged = false;
				return;
			}

			Texture texture;
			if (!canCacheTexture(tex_addr))
			{
				texture = null;

				// Generate a texture id if we don't have one
				if (textureId == -1)
				{
					textureId = re.genTexture();
				}

				re.bindTexture(textureId);
				context.currentTextureId = textureId;
			}
			else
			{
				TextureCache textureCache = TextureCache.Instance;
				bool textureRequiresClut = pspsharp.graphics.RE.IRenderingEngine_Fields.isTextureTypeIndexed[context.texture_storage];
				if (textureRequiresClut && re.canNativeClut(tex_addr, context.texture_storage, context.texture_swizzle))
				{
					if (context.texture_storage >= TPSM_PIXEL_STORAGE_MODE_8BIT_INDEXED && context.texture_storage <= TPSM_PIXEL_STORAGE_MODE_32BIT_INDEXED)
					{
						// The Clut will be resolved by the shader
						textureRequiresClut = false;
					}
				}

				textureCacheLookupStatistics.start();
				// Check if the texture is in the cache
				if (textureRequiresClut)
				{
					texture = textureCache.getTexture(context.texture_base_pointer[mipmapIndex], context.texture_buffer_width[mipmapIndex], context.texture_width[mipmapIndex], context.texture_height[mipmapIndex], context.texture_storage, context.tex_clut_addr, context.tex_clut_mode, context.tex_clut_start, context.tex_clut_shift, context.tex_clut_mask, context.tex_clut_num_blocks, context.texture_num_mip_maps, context.mipmapShareClut, null, null);
				}
				else
				{
					texture = textureCache.getTexture(context.texture_base_pointer[mipmapIndex], context.texture_buffer_width[mipmapIndex], context.texture_width[mipmapIndex], context.texture_height[mipmapIndex], context.texture_storage, 0, 0, 0, 0, 0, 0, context.texture_num_mip_maps, false, null, null);
				}
				textureCacheLookupStatistics.end();

				// Create the texture if not yet in the cache
				if (texture == null)
				{
					if (textureRequiresClut)
					{
						texture = new Texture(textureCache, context.texture_base_pointer[mipmapIndex], context.texture_buffer_width[mipmapIndex], context.texture_width[mipmapIndex], context.texture_height[mipmapIndex], context.texture_storage, context.tex_clut_addr, context.tex_clut_mode, context.tex_clut_start, context.tex_clut_shift, context.tex_clut_mask, context.tex_clut_num_blocks, context.texture_num_mip_maps, context.mipmapShareClut, null, null);
					}
					else
					{
						texture = new Texture(textureCache, context.texture_base_pointer[mipmapIndex], context.texture_buffer_width[mipmapIndex], context.texture_width[mipmapIndex], context.texture_height[mipmapIndex], context.texture_storage, 0, 0, 0, 0, 0, 0, context.texture_num_mip_maps, false, null, null);
					}
					textureCache.addTexture(re, texture);
				}

				texture.bindTexture(re);
				context.currentTextureId = texture.getTextureId(re);
			}

			if (textureFlipped)
			{
				textureFlipped = false;
				textureMatrixUpload.Changed = true;
			}

			// Load the texture if not yet loaded
			if (texture == null || !texture.Loaded || State.captureGeNextFrame)
			{
				if (isLogDebugEnabled)
				{
					log(helper.getCommandString(TFLUSH) + " " + string.Format("0x{0:X8}", context.texture_base_pointer[mipmapIndex]) + ", buffer_width=" + context.texture_buffer_width[mipmapIndex] + " (" + context.texture_width[mipmapIndex] + "," + context.texture_height[mipmapIndex] + ")");

					log(helper.getCommandString(TFLUSH) + " texture_storage=0x" + context.texture_storage.ToString("x") + "(" + getPsmName(context.texture_storage) + "), tex_clut_mode=0x" + context.tex_clut_mode.ToString("x") + ", tex_clut_addr=" + string.Format("0x{0:X8}", context.tex_clut_addr) + ", texture_swizzle=" + context.texture_swizzle);
				}

				if (isGeProfilerEnabled)
				{
					GEProfiler.loadTexture();
				}

				// If the texture is the current GE
				// first save the GE to memory before loading the texture.
				if (tex_addr == context.fbp && context.texture_storage == context.psm && context.texture_buffer_width[mipmapIndex] == context.fbw)
				{
					display.copyGeToMemory(true, false);
					// Re-bind the texture to be loaded, as the bind might have been changed during the GE copy.
					re.bindTexture(context.currentTextureId);
				}

				int numberMipmaps = getValidNumberMipmaps(false);

				// Set the texture min/mag filters before uploading the texture
				// (some drivers have problems changing the parameters afterwards)
				re.TextureMipmapMagFilter = context.tex_mag_filter;
				re.TextureMipmapMinFilter = context.tex_min_filter;
				checkTextureMinFilter(compressedTexture, numberMipmaps);

				if (mipmapIndex == 0)
				{
					for (int level = 0; level <= numberMipmaps; level++)
					{
						loadTexture(level, level);
					}
				}
				else
				{
					// Load a single mipmap as a whole texture
					loadTexture(mipmapIndex, 0);
				}

				if (texture != null)
				{
					texture.setIsLoaded();
					if (isLogDebugEnabled)
					{
						log(helper.getCommandString(TFLUSH) + " Loaded texture " + texture.GlId);
					}
				}
			}
			else
			{
				re.TextureMipmapMagFilter = context.tex_mag_filter;
				re.TextureMipmapMinFilter = context.tex_min_filter;
				checkTextureMinFilter(compressedTexture, context.texture_num_mip_maps);

				if (isLogDebugEnabled)
				{
					log(helper.getCommandString(TFLUSH) + " Reusing cached texture " + texture.GlId);
				}
			}

			textureChanged = false;
		}

		private void checkTextureMinFilter(bool compressedTexture, int numberMipmaps)
		{
			// OpenGL/Hardware cannot interpolate between compressed textures;
			// this restriction has been checked on NVIDIA GeForce 8500 GT and 9800 GT
			if (compressedTexture)
			{
				int new_tex_min_filter;
				if (context.tex_min_filter == TFLT_NEAREST || context.tex_min_filter == TFLT_NEAREST_MIPMAP_LINEAR || context.tex_min_filter == TFLT_NEAREST_MIPMAP_NEAREST)
				{
					new_tex_min_filter = TFLT_NEAREST;
				}
				else
				{
					new_tex_min_filter = TFLT_LINEAR;
				}

				if (new_tex_min_filter != context.tex_min_filter)
				{
					re.TextureMipmapMinFilter = new_tex_min_filter;
					if (isLogDebugEnabled)
					{
						log("Overwriting texture min filter, no mipmap was generated but filter was set to use mipmap");
					}
				}
			}
		}

		private Buffer readIndexedTexture(int level, int texaddr, int texclut, int bytesPerIndex, int textureBufferWidthInPixels)
		{
			Buffer buffer = null;

			int Length = textureBufferWidthInPixels * context.texture_height[level];
			switch (context.tex_clut_mode)
			{
				case CMODE_FORMAT_16BIT_BGR5650:
				case CMODE_FORMAT_16BIT_ABGR5551:
				case CMODE_FORMAT_16BIT_ABGR4444:
				{
					if (texclut == 0)
					{
						return null;
					}

					short[] clut = readClut16(level);

					if (!context.texture_swizzle)
					{
						IMemoryReader memoryReader = MemoryReader.getMemoryReader(texaddr, Length * bytesPerIndex, bytesPerIndex);
						for (int i = 0; i < Length; i++)
						{
							int index = memoryReader.readNext();
							tmp_texture_buffer16[i] = clut[getClutIndex(index)];
						}
						buffer = ShortBuffer.wrap(tmp_texture_buffer16);

						if (State.captureGeNextFrame)
						{
							log_Renamed.info("Capture loadTexture clut 8/16 unswizzled");
							CaptureManager.captureRAM(texaddr, Length * bytesPerIndex);
						}
					}
					else
					{
						unswizzleTextureFromMemory(texaddr, bytesPerIndex, level, textureBufferWidthInPixels);
						switch (bytesPerIndex)
						{
							case 1:
							{
								for (int i = 0, j = 0; i < Length; i += 4, j++)
								{
									int n = tmp_texture_buffer32[j];
									int index = n & 0xFF;
									tmp_texture_buffer16[i + 0] = clut[getClutIndex(index)];
									index = (n >> 8) & 0xFF;
									tmp_texture_buffer16[i + 1] = clut[getClutIndex(index)];
									index = (n >> 16) & 0xFF;
									tmp_texture_buffer16[i + 2] = clut[getClutIndex(index)];
									index = (n >> 24) & 0xFF;
									tmp_texture_buffer16[i + 3] = clut[getClutIndex(index)];
								}
								break;
							}
							case 2:
							{
								for (int i = 0, j = 0; i < Length; i += 2, j++)
								{
									int n = tmp_texture_buffer32[j];
									tmp_texture_buffer16[i + 0] = clut[getClutIndex(n & 0xFFFF)];
									tmp_texture_buffer16[i + 1] = clut[getClutIndex((int)((uint)n >> 16))];
								}
								break;
							}
							case 4:
							{
								for (int i = 0; i < Length; i++)
								{
									int n = tmp_texture_buffer32[i];
									tmp_texture_buffer16[i] = clut[getClutIndex(n)];
								}
								break;
							}
						}
						buffer = ShortBuffer.wrap(tmp_texture_buffer16);
					}

					break;
				}

				case CMODE_FORMAT_32BIT_ABGR8888:
				{
					if (texclut == 0)
					{
						return null;
					}

					int[] clut = readClut32(level);

					if (!context.texture_swizzle)
					{
						IMemoryReader memoryReader = MemoryReader.getMemoryReader(texaddr, Length * bytesPerIndex, bytesPerIndex);
						for (int i = 0; i < Length; i++)
						{
							int index = memoryReader.readNext();
							tmp_texture_buffer32[i] = clut[getClutIndex(index)];
						}
						buffer = IntBuffer.wrap(tmp_texture_buffer32);

						if (State.captureGeNextFrame)
						{
							log_Renamed.info("Capture loadTexture clut 8/32 unswizzled");
							CaptureManager.captureRAM(texaddr, Length * bytesPerIndex);
						}
					}
					else
					{
						unswizzleTextureFromMemory(texaddr, bytesPerIndex, level, textureBufferWidthInPixels);
						switch (bytesPerIndex)
						{
							case 1:
							{
								for (int i = Length - 4, j = (Length / 4) - 1; i >= 0; i -= 4, j--)
								{
									int n = tmp_texture_buffer32[j];
									int index = n & 0xFF;
									tmp_texture_buffer32[i + 0] = clut[getClutIndex(index)];
									index = (n >> 8) & 0xFF;
									tmp_texture_buffer32[i + 1] = clut[getClutIndex(index)];
									index = (n >> 16) & 0xFF;
									tmp_texture_buffer32[i + 2] = clut[getClutIndex(index)];
									index = (n >> 24) & 0xFF;
									tmp_texture_buffer32[i + 3] = clut[getClutIndex(index)];
								}
								break;
							}
							case 2:
							{
								for (int i = Length - 2, j = (Length / 2) - 1; i >= 0; i -= 2, j--)
								{
									int n = tmp_texture_buffer32[j];
									tmp_texture_buffer32[i + 0] = clut[getClutIndex(n & 0xFFFF)];
									tmp_texture_buffer32[i + 1] = clut[getClutIndex((int)((uint)n >> 16))];
								}
								break;
							}
							case 4:
							{
								for (int i = 0; i < Length; i++)
								{
									int n = tmp_texture_buffer32[i];
									tmp_texture_buffer32[i] = clut[getClutIndex(n)];
								}
								break;
							}
						}
						buffer = IntBuffer.wrap(tmp_texture_buffer32);
					}

					break;
				}

				default:
				{
					error("Unhandled clut8 texture mode " + context.tex_clut_mode);
					break;
				}
			}

			return buffer;
		}

		private void setScissor()
		{
			if (context.scissor_x1 >= 0 && context.scissor_y1 >= 0 && context.scissor_width <= context.region_width && context.scissor_height <= context.region_height)
			{
				int scissorX = context.scissor_x1;
				int scissorY = context.scissor_y1;
				int scissorWidth = context.scissor_width;
				int scissorHeight = context.scissor_height;

				if (scissorHeight < Screen.height)
				{
					scissorY = Screen.height - scissorHeight - scissorY;
				}

				re.setScissor(scissorX, scissorY, scissorWidth, scissorHeight);
				context.scissorTestFlag.setEnabled(true);
			}
			else
			{
				context.scissorTestFlag.setEnabled(false);
			}
		}

		private float[] ProjectionMatrix
		{
			get
			{
				if (context.transform_mode == VTYPE_TRANSFORM_PIPELINE_RAW_COORD)
				{
					// 2D
					return null;
				}
    
				if (context.viewport_height <= 0 && context.viewport_width >= 0)
				{
					// Non-flipped 3D
					return context.proj_uploaded_matrix;
				}
    
				float[] flippedMatrix = new float[16];
				Array.Copy(context.proj_uploaded_matrix, 0, flippedMatrix, 0, flippedMatrix.Length);
				if (context.viewport_height > 0)
				{
					// Flip upside-down
					flippedMatrix[1] = -flippedMatrix[1];
					flippedMatrix[5] = -flippedMatrix[5];
					flippedMatrix[9] = -flippedMatrix[9];
					flippedMatrix[13] = -flippedMatrix[13];
				}
				if (context.viewport_width < 0)
				{
					// Flip right-to-left
					flippedMatrix[0] = -flippedMatrix[0];
					flippedMatrix[4] = -flippedMatrix[4];
					flippedMatrix[8] = -flippedMatrix[8];
					flippedMatrix[12] = -flippedMatrix[12];
				}
    
				return flippedMatrix;
			}
		}

		private void initRendering()
		{
			/*
			 * Defer transformations until primitive rendering
			 */

			/*
			 * Set Scissor
			 */
			if (scissorChanged)
			{
				setScissor();
				scissorChanged = false;
			}

			/*
			 * Apply projection matrix
			 */
			if (projectionMatrixUpload.Changed)
			{
				re.ProjectionMatrix = ProjectionMatrix;
				projectionMatrixUpload.Changed = false;

				// The viewport has to be reloaded when the projection matrix has changed
				viewportChanged = true;
			}

			/*
			 * Apply viewport
			 */
			bool loadOrtho2D = false;
			if (viewportChanged)
			{
				re.setViewportPos(context.viewport_cx, context.viewport_cy, context.zpos);
				re.setViewportScale(context.viewport_width, context.viewport_height, context.zscale);
				if (context.transform_mode == VTYPE_TRANSFORM_PIPELINE_RAW_COORD)
				{
					re.setViewport(0, 0, Screen.width, Screen.height);
					// Load the ortho for 2D after the depth settings
					loadOrtho2D = true;
				}
				else
				{
					int halfHeight = System.Math.Abs(context.viewport_height);
					int halfWidth = System.Math.Abs(context.viewport_width);
					int viewportX = context.viewport_cx - halfWidth - context.offset_x;
					int viewportY = context.viewport_cy - halfHeight - context.offset_y;
					int viewportWidth = 2 * halfWidth;
					int viewportHeight = 2 * halfHeight;

					// For OpenGL, translate the viewportY from the upper left corner
					// to the lower left corner.
					viewportY = Screen.height - viewportY - viewportHeight;

					re.setViewport(viewportX, viewportY, viewportWidth, viewportHeight);
				}
				viewportChanged = false;
			}

			/*
			 * Apply depth handling
			 */
			if (depthChanged)
			{
				if (context.transform_mode == VTYPE_TRANSFORM_PIPELINE_TRANS_COORD)
				{
					re.DepthFunc = context.depthFunc;
					re.setDepthRange(context.zpos, context.zscale, context.nearZ, context.farZ);
				}
				else
				{
					re.DepthFunc = context.depthFunc;
					re.setDepthRange(32767.5f, 32767.5f, 0x0000, 0xFFFF);
				}
				depthChanged = false;
			}

			/*
			 * Load the 2D ortho (only after the depth settings
			 */
			if (loadOrtho2D)
			{
				re.ProjectionMatrix = getOrthoMatrix(0, 480, 272, 0, 0, -0xFFFF);
			}

			/*
			 * 2D mode handling
			 */
			if (context.transform_mode == VTYPE_TRANSFORM_PIPELINE_RAW_COORD)
			{
				// 2D mode shouldn't be affected by the lighting and fog
				re.disableFlag(pspsharp.graphics.RE.IRenderingEngine_Fields.GU_LIGHTING);
				re.disableFlag(pspsharp.graphics.RE.IRenderingEngine_Fields.GU_FOG);

				// TODO I don't know why, but the GL_MODELVIEW matrix has to be reloaded
				// each time in 2D mode... Otherwise textures are not displayed.
				modelMatrixUpload.Changed = true;
			}
			else
			{
				context.lightingFlag.update();
				context.fogFlag.update();
			}

			/*
			 * Model-View matrix has to reloaded when
			 * - model matrix changed
			 * - view matrix changed
			 * - lighting has to be reloaded
			 */
			bool loadLightingSettings = (viewMatrixUpload.Changed || lightingChanged) && context.lightingFlag.Enabled && context.transform_mode == VTYPE_TRANSFORM_PIPELINE_TRANS_COORD;
			bool modelViewMatrixChanged = modelMatrixUpload.Changed || viewMatrixUpload.Changed || loadLightingSettings;

			/*
			 * Apply view matrix
			 */
			if (modelViewMatrixChanged)
			{
				if (context.transform_mode == VTYPE_TRANSFORM_PIPELINE_TRANS_COORD)
				{
					re.ViewMatrix = context.view_uploaded_matrix;
				}
				else
				{
					re.ViewMatrix = null;
				}
				viewMatrixUpload.Changed = false;
			}

			/*
			 *  Setup lights on when view transformation is set up.
			 *  The light positions and directions are defined in the View-Projection world.
			 *  The Model transformation does not apply for the lights.
			 */
			if (loadLightingSettings || (context.tex_map_mode == TMAP_TEXTURE_MAP_MODE_ENVIRONMENT_MAP && !context.vinfo.transform2D))
			{
				for (int i = 0; i < NUM_LIGHTS; i++)
				{
					if (context.lightFlags[i].Enabled || (context.tex_map_mode == TMAP_TEXTURE_MAP_MODE_ENVIRONMENT_MAP && (context.tex_shade_u == i || context.tex_shade_v == i)))
					{
						re.setLightPosition(i, context.light_pos[i]);
						re.setLightDirection(i, context.light_dir[i]);

						if (context.light_type[i] == LIGHT_SPOT)
						{
							re.setLightSpotExponent(i, context.spotLightExponent[i]);
							re.setLightSpotCutoff(i, context.spotLightCutoff[i]);
						}
						else
						{
							// uniform light distribution
							re.setLightSpotExponent(i, 0);
							re.setLightSpotCutoff(i, 180);
						}

						// Light kind:
						//  LIGHT_DIFFUSE_SPECULAR: use ambient, diffuse and specular colors
						//  all other light kinds: use ambient and diffuse colors (not specular)
						if (context.light_kind[i] != LIGHT_AMBIENT_DIFFUSE)
						{
							re.setLightSpecularColor(i, context.lightSpecularColor[i]);
						}
						else
						{
							re.setLightSpecularColor(i, blackColor);
						}
					}
				}

				lightingChanged = false;
			}

			if (modelViewMatrixChanged)
			{
				// Apply model matrix
				if (context.transform_mode == VTYPE_TRANSFORM_PIPELINE_TRANS_COORD)
				{
					re.ModelMatrix = context.model_uploaded_matrix;
				}
				else
				{
					re.ModelMatrix = null;
				}
				modelMatrixUpload.Changed = false;
				re.endModelViewMatrixUpdate();
			}

			/*
			 * Apply texture transforms
			 */
			if (textureMatrixUpload.Changed)
			{
				if (context.transform_mode != VTYPE_TRANSFORM_PIPELINE_TRANS_COORD)
				{
					re.setTextureMapMode(TMAP_TEXTURE_MAP_MODE_TEXTURE_COORDIATES_UV, TMAP_TEXTURE_PROJECTION_MODE_TEXTURE_COORDINATES);
					context.reTextureGenS.setEnabled(false);
					context.reTextureGenT.setEnabled(false);

					float[] textureMatrix = new float[]{1.0f / context.texture_width[0], 0, 0, 0, 0, 1.0f / context.texture_height[0], 0, 0, 0, 0, 1, 0, 0, 0, 0, 1};
					if (textureFlipped)
					{
						textureMatrix[5] = -textureMatrix[5];
						textureMatrix[13] = textureFlipTranslateY;
						if (isLogDebugEnabled)
						{
							log_Renamed.debug("Flipped 2D");
						}
					}
					re.TextureMatrix = textureMatrix;
				}
				else
				{
					re.setTextureMapMode(context.tex_map_mode, context.tex_proj_map_mode);
					switch (context.tex_map_mode)
					{
						case TMAP_TEXTURE_MAP_MODE_TEXTURE_COORDIATES_UV:
						{
							context.reTextureGenS.setEnabled(false);
							context.reTextureGenT.setEnabled(false);

							float[] textureMatrix = new float[]{context.tex_scale_x, 0, 0, 0, 0, context.tex_scale_y, 0, 0, 0, 0, 1, 0, context.tex_translate_x, context.tex_translate_y, 0, 1};
							if (textureFlipped)
							{
								if (textureMatrix[5] < 0f)
								{
									// If the texture was mapped upside-down, also invert the translation
									textureMatrix[13] = 1f - textureMatrix[13];
								}
								textureMatrix[5] = -textureMatrix[5];
								if (isLogDebugEnabled)
								{
									log_Renamed.debug("Flipped TMAP_TEXTURE_MAP_MODE_TEXTURE_COORDIATES_UV");
								}
							}
							re.TextureMatrix = textureMatrix;
							break;
						}

						case TMAP_TEXTURE_MAP_MODE_TEXTURE_MATRIX:
						{
							context.reTextureGenS.setEnabled(false);
							context.reTextureGenT.setEnabled(false);
							float[] textureMatrix = context.texture_uploaded_matrix;
							if (textureFlipped)
							{
								// Map the (U,V) from ([0..1],[0..1]) to ([0..1],[1..0])
								float[] flippedTextureMatrix = new float[]{textureMatrix[0], textureMatrix[1], textureMatrix[2], textureMatrix[3], -textureMatrix[4], -textureMatrix[5], -textureMatrix[6], textureMatrix[7], textureMatrix[8], textureMatrix[9], textureMatrix[10], textureMatrix[11], textureMatrix[12] + textureMatrix[4], textureMatrix[13] + textureMatrix[5], textureMatrix[14] + textureMatrix[6], textureMatrix[15]};
								textureMatrix = flippedTextureMatrix;
								if (isLogDebugEnabled)
								{
									log_Renamed.debug("Flipped TMAP_TEXTURE_MAP_MODE_TEXTURE_MATRIX");
								}
							}
							re.TextureMatrix = textureMatrix;
							break;
						}

						case TMAP_TEXTURE_MAP_MODE_ENVIRONMENT_MAP:
						{
							re.setTextureEnvironmentMapping(context.tex_shade_u, context.tex_shade_v);
							context.reTextureGenS.setEnabled(true);
							context.reTextureGenT.setEnabled(true);
							for (int i = 0; i < 3; i++)
							{
								context.tex_envmap_matrix[i + 0] = context.light_pos[context.tex_shade_u][i];
								context.tex_envmap_matrix[i + 4] = context.light_pos[context.tex_shade_v][i];
							}
							float[] textureMatrix = context.tex_envmap_matrix;
							if (textureFlipped)
							{
								// Map the (U,V) from ([0..1],[0..1]) to ([0..1],[1..0])
								float[] flippedTextureMatrix = new float[]{textureMatrix[0], textureMatrix[1], textureMatrix[2], textureMatrix[3], -textureMatrix[4], -textureMatrix[5], -textureMatrix[6], textureMatrix[7], textureMatrix[8], textureMatrix[9], textureMatrix[10], textureMatrix[11], textureMatrix[12] + textureMatrix[4], textureMatrix[13] + textureMatrix[5], textureMatrix[14] + textureMatrix[6], textureMatrix[15]};
								textureMatrix = flippedTextureMatrix;
								if (isLogDebugEnabled)
								{
									log_Renamed.debug("Flipped TMAP_TEXTURE_MAP_MODE_ENVIRONMENT_MAP");
								}
							}
							re.TextureMatrix = textureMatrix;
							break;
						}

						default:
							log_Renamed.warn(string.Format("Unhandled texture matrix mode {0:D}", context.tex_map_mode));
						break;
					}
				}

				textureMatrixUpload.Changed = false;
			}

			context.useVertexColor = false;
			if (!context.lightingFlag.Enabled || context.transform_mode == VTYPE_TRANSFORM_PIPELINE_RAW_COORD)
			{
				context.reColorMaterial.setEnabled(false);
				if (context.vinfo.color != 0)
				{
					context.useVertexColor = true;
				}
				else
				{
					re.VertexColor = context.mat_ambient;
				}
			}
			else if (context.vinfo.color != 0 && context.mat_flags != 0)
			{
				context.useVertexColor = true;
				if (materialChanged)
				{
					bool ambient = (context.mat_flags & 1) != 0;
					bool diffuse = (context.mat_flags & 2) != 0;
					bool specular = (context.mat_flags & 4) != 0;
					re.setColorMaterial(ambient, diffuse, specular);
					context.reColorMaterial.setEnabled(true);
					if (!ambient)
					{
						re.MaterialAmbientColor = context.mat_ambient;
					}
					if (!diffuse)
					{
						re.MaterialDiffuseColor = context.mat_diffuse;
					}
					if (!specular)
					{
						re.MaterialSpecularColor = context.mat_specular;
					}
					materialChanged = false;
				}
				re.VertexColor = context.mat_ambient;
			}
			else
			{
				context.reColorMaterial.setEnabled(false);
				if (materialChanged)
				{
					re.setColorMaterial(false, false, false);
					re.MaterialAmbientColor = context.mat_ambient;
					re.MaterialDiffuseColor = context.mat_diffuse;
					re.MaterialSpecularColor = context.mat_specular;
					materialChanged = false;
				}
				re.VertexColor = context.mat_ambient;
			}

			if (context.textureFlag.Enabled && !context.clearMode && !isBoundingBox)
			{
				re.setTextureWrapMode(context.tex_wrap_s, context.tex_wrap_t);

				int validNumberMipmaps = getValidNumberMipmaps(true);
				int mipmapBaseLevel = 0;
				int mipmapMaxLevel = validNumberMipmaps;
				if (context.tex_mipmap_mode == TBIAS_MODE_CONST)
				{
					// TBIAS_MODE_CONST uses the tex_mipmap_bias_int level supplied by TBIAS.
					mipmapBaseLevel = context.tex_mipmap_bias_int;
					mipmapMaxLevel = context.tex_mipmap_bias_int;
					if (isLogDebugEnabled)
					{
						log_Renamed.debug("TBIAS_MODE_CONST " + context.tex_mipmap_bias_int);
					}
				}
				else if (context.tex_mipmap_mode == TBIAS_MODE_AUTO)
				{
					// TODO implement TBIAS_MODE_AUTO. The following is not correct
					// TBIAS_MODE_AUTO performs a comparison between the texture's weight and height at level 0.
					// int maxValue = Math.max(context.texture_width[0], context.texture_height[0]);
					//
					// if(maxValue <= 1) {
					//     mipmapBaseLevel = 0;
					// } else {
					//     mipmapBaseLevel = (int) ((Math.log((Math.abs(maxValue) / Math.abs(context.zpos))) / Math.log(2)) + context.tex_mipmap_bias);
					// }
					// mipmapMaxLevel = mipmapBaseLevel;
					// if (isLogDebugEnabled) {
					//     System.Console.WriteLine("TBIAS_MODE_AUTO " + context.tex_mipmap_bias + ", param=" + maxValue);
					// }
				}
				else if (context.tex_mipmap_mode == TBIAS_MODE_SLOPE)
				{
					// TBIAS_MODE_SLOPE uses the tslope_level level supplied by TSLOPE.
					mipmapBaseLevel = (int)((System.Math.Log(System.Math.Abs(context.tslope_level) / System.Math.Abs(context.zpos)) / System.Math.Log(2)) + context.tex_mipmap_bias);
					mipmapMaxLevel = mipmapBaseLevel;
					if (isLogDebugEnabled)
					{
						log_Renamed.debug("TBIAS_MODE_SLOPE " + context.tex_mipmap_bias + ", slope=" + context.tslope_level);
					}
				}

				// Clamp to [0..validNumberMipmaps]
				mipmapBaseLevel = System.Math.Max(0, System.Math.Min(mipmapBaseLevel, validNumberMipmaps));
				// Clamp to [mipmapBaseLevel..validNumberMipmaps]
				mipmapMaxLevel = System.Math.Max(mipmapBaseLevel, System.Math.Min(mipmapMaxLevel, validNumberMipmaps));
				if (isLogDebugEnabled)
				{
					log_Renamed.debug(string.Format("Texture Mipmap base={0:D}, max={1:D}, validNumberMipmaps={2:D}", mipmapBaseLevel, mipmapMaxLevel, validNumberMipmaps));
				}
				re.TextureMipmapMinLevel = mipmapBaseLevel;
				re.TextureMipmapMaxLevel = mipmapMaxLevel;
			}
		}

		private void endRendering(int numberOfVertex)
		{
			// VADDR/IADDR are updated after vertex rendering
			// (IADDR when indexed and VADDR when not).
			// Some games rely on this and don't reload VADDR/IADDR between 2 PRIM/BBOX calls.
			if (context.vinfo.index == 0)
			{
				context.vinfo.ptr_vertex = context.vinfo.getAddress(Memory.Instance, numberOfVertex);
			}
			else
			{
				context.vinfo.ptr_index += numberOfVertex * context.vinfo.index;
			}
		}

		public static float[] getOrthoMatrix(float left, float right, float bottom, float top, float near, float far)
		{
			float dx = right - left;
			float dy = top - bottom;
			float dz = far - near;
			float[] orthoMatrix = new float[] {2.0f / dx, 0, 0, 0, 0, 2.0f / dy, 0, 0, 0, 0, -2.0f / dz, 0, -(right + left) / dx, -(top + bottom) / dy, -(far + near) / dz, 1};

			return orthoMatrix;
		}

		internal virtual float spline_n(int i, int j, float u, int[] knot)
		{
			if (j == 0)
			{
				if (knot[i] <= u && u < knot[i + 1])
				{
					return 1;
				}
				return 0;
			}
			float res = 0;
			if (knot[i + j] - knot[i] != 0)
			{
				res += (u - knot[i]) / (knot[i + j] - knot[i]) * spline_n(i, j - 1, u, knot);
			}
			if (knot[i + j + 1] - knot[i + 1] != 0)
			{
				res += (knot[i + j + 1] - u) / (knot[i + j + 1] - knot[i + 1]) * spline_n(i + 1, j - 1, u, knot);
			}
			return res;
		}

		internal virtual int[] spline_knot(int n, int type)
		{
			int[] knot = new int[n + 5];
			for (int i = 0; i < n - 1; i++)
			{
				knot[i + 3] = i;
			}

			if ((type & 1) == 0)
			{
				knot[0] = -3;
				knot[1] = -2;
				knot[2] = -1;
			}
			if ((type & 2) == 0)
			{
				knot[n + 2] = n - 1;
				knot[n + 3] = n;
				knot[n + 4] = n + 1;
			}
			else
			{
				knot[n + 2] = n - 2;
				knot[n + 3] = n - 2;
				knot[n + 4] = n - 2;
			}

			return knot;
		}

		private void drawSpline(int ucount, int vcount, int utype, int vtype)
		{
			if (ucount < 4 || vcount < 4)
			{
				log_Renamed.warn("Unsupported spline parameters uc=" + ucount + " vc=" + vcount);
				return;
			}
			if (context.patch_div_s <= 0 || context.patch_div_t <= 0)
			{
				log_Renamed.warn("Unsupported spline patches patch_div_s=" + context.patch_div_s + " patch_div_t=" + context.patch_div_t);
				return;
			}

			initRendering();
			bool useTexture = context.vinfo.texture != 0 || context.textureFlag.Enabled;
			bool useNormal = context.lightingFlag.Enabled;

			VertexInfo cachedVertexInfo = null;
			if (useVertexCache)
			{
				int numberOfVertex = context.patch_div_t * (context.patch_div_s + 1) * 2;
				vertexCacheLookupStatistics.start();
				cachedVertexInfo = VertexCache.Instance.getVertex(context.vinfo, numberOfVertex, context.bone_uploaded_matrix, 0);
				vertexCacheLookupStatistics.end();
			}

			VertexState[][] patch = null;
			if (cachedVertexInfo == null)
			{
				// Generate control points.
				VertexState[][] ctrlpoints = getControlPoints(ucount, vcount);

				// GE capture.
				if (State.captureGeNextFrame && !VertexBufferEmbedded)
				{
					log_Renamed.info("Capture drawSpline");
					CaptureManager.captureRAM(context.vinfo.ptr_vertex, context.vinfo.vertexSize * ucount * vcount);
				}

				// Generate patch VertexState.
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: patch = new VertexState[context.patch_div_s + 1][context.patch_div_t + 1];
				patch = RectangularArrays.ReturnRectangularVertexStateArray(context.patch_div_s + 1, context.patch_div_t + 1);

				// Calculate knot Array.
				int n = ucount - 1;
				int m = vcount - 1;
				int[] knot_u = spline_knot(n, utype);
				int[] knot_v = spline_knot(m, vtype);

				// The spline grows to a limit defined by n - 2 for u and m - 2 for v.
				// This limit is open, so we need to get a very close approximation of it.
				float limit = 2.000001f;

				// Process spline vertexes with Cox-deBoor's algorithm.
				for (int j = 0; j <= context.patch_div_t; j++)
				{
					float cv = (float) j * (float)(m - limit) / (float) context.patch_div_t;

					for (int i = 0; i <= context.patch_div_s; i++)
					{
						float cu = (float) i * (float)(n - limit) / (float) context.patch_div_s;

						patch[i][j] = new VertexState();
						VertexState p = patch[i][j];

						for (int ii = 0; ii <= n; ii++)
						{
							for (int jj = 0; jj <= m; jj++)
							{
								float f = spline_n(ii, 3, cu, knot_u) * spline_n(jj, 3, cv, knot_v);
								if (f != 0)
								{
									pointMultAdd(p, ctrlpoints[ii][jj], f, context.useVertexColor, useTexture, useNormal);
								}
							}
						}
						if (useTexture && context.vinfo.texture == 0)
						{
							p.t[0] = cu;
							p.t[1] = cv;
						}
					}
				}
			}

			drawCurvedSurface(patch, ucount, vcount, cachedVertexInfo, context.useVertexColor, useTexture, useNormal);
		}

		private void pointMultAdd(VertexState dest, VertexState src, float f, bool useVertexColor, bool useTexture, bool useNormal)
		{
			dest.p[0] += f * src.p[0];
			dest.p[1] += f * src.p[1];
			dest.p[2] += f * src.p[2];
			if (useTexture)
			{
				dest.t[0] += f * src.t[0];
				dest.t[1] += f * src.t[1];
			}
			if (useVertexColor)
			{
				dest.c[0] += f * src.c[0];
				dest.c[1] += f * src.c[1];
				dest.c[2] += f * src.c[2];
				dest.c[3] += f * src.c[3];
			}
			if (useNormal)
			{
				dest.n[0] += f * src.n[0];
				dest.n[1] += f * src.n[1];
				dest.n[2] += f * src.n[2];
			}
		}

		private void drawBezier(int ucount, int vcount)
		{
			if ((ucount - 1) % 3 != 0 || (vcount - 1) % 3 != 0)
			{
				log_Renamed.warn("Unsupported bezier parameters ucount=" + ucount + " vcount=" + vcount);
				return;
			}
			if (context.patch_div_s <= 0 || context.patch_div_t <= 0)
			{
				log_Renamed.warn("Unsupported bezier patches patch_div_s=" + context.patch_div_s + " patch_div_t=" + context.patch_div_t);
				return;
			}

			ucount = System.Math.Max(ucount, 4);
			vcount = System.Math.Max(vcount, 4);

			initRendering();
			bool useTexture = context.vinfo.texture != 0 || context.textureFlag.Enabled;
			bool useNormal = context.lightingFlag.Enabled;

			VertexInfo cachedVertexInfo = null;
			if (useVertexCache)
			{
				int numberOfVertex = context.patch_div_t * (context.patch_div_s + 1) * 2;
				vertexCacheLookupStatistics.start();
				cachedVertexInfo = VertexCache.Instance.getVertex(context.vinfo, numberOfVertex, context.bone_uploaded_matrix, 0);
				vertexCacheLookupStatistics.end();
			}

			VertexState[][] patch = null;
			if (cachedVertexInfo == null)
			{
				VertexState[][] anchors = getControlPoints(ucount, vcount);

				// Don't capture the ram if the vertex list is embedded in the display list. TODO handle stall_addr == 0 better
				// TODO may need to move inside the loop if indices are used, or find the largest index so we can calculate the size of the vertex list
				if (State.captureGeNextFrame && !VertexBufferEmbedded)
				{
					log_Renamed.info("Capture drawBezier");
					CaptureManager.captureRAM(context.vinfo.ptr_vertex, context.vinfo.vertexSize * ucount * vcount);
				}

				// Generate patch VertexState.
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: patch = new VertexState[context.patch_div_s + 1][context.patch_div_t + 1];
				patch = RectangularArrays.ReturnRectangularVertexStateArray(context.patch_div_s + 1, context.patch_div_t + 1);

				// Number of patches in the U and V directions
				int upcount = ucount / 3;
				int vpcount = vcount / 3;

				float[][] ucoeff = new float[context.patch_div_s + 1][];

				for (int j = 0; j <= context.patch_div_t; j++)
				{
					float vglobal = (float) j * vpcount / (float) context.patch_div_t;

					int vpatch = (int) vglobal; // Patch number
					float cv = vglobal - vpatch;
					if (j == context.patch_div_t)
					{
						vpatch--;
						cv = 1.0f;
					}
					float[] vcoeff = BernsteinCoeff(cv);

					for (int i = 0; i <= context.patch_div_s; i++)
					{
						float uglobal = (float) i * upcount / (float) context.patch_div_s;
						int upatch = (int) uglobal;
						float cu = uglobal - upatch;
						if (i == context.patch_div_s)
						{
							upatch--;
							cu = 1.0f;
						}
						ucoeff[i] = BernsteinCoeff(cu);

						VertexState p = new VertexState();
						patch[i][j] = p;

						for (int ii = 0; ii < 4; ++ii)
						{
							for (int jj = 0; jj < 4; ++jj)
							{
								pointMultAdd(p, anchors[3 * upatch + ii][3 * vpatch + jj], ucoeff[i][ii] * vcoeff[jj], context.useVertexColor, useTexture, useNormal);
							}
						}

						if (useTexture && context.vinfo.texture == 0)
						{
							p.t[0] = uglobal;
							p.t[1] = vglobal;
						}
					}
				}
			}

			drawCurvedSurface(patch, ucount, vcount, cachedVertexInfo, context.useVertexColor, useTexture, useNormal);
		}

		private void drawCurvedSurface(VertexState[][] patch, int ucount, int vcount, VertexInfo cachedVertexInfo, bool useVertexColor, bool useTexture, bool useNormal)
		{
			if (re.VertexArrayAvailable)
			{
				re.bindVertexArray(0);
			}

			int type = patch_prim_types[context.patch_prim];
			re.setVertexInfo(context.vinfo, false, useVertexColor, useTexture, type);

			// Triangle strips can be combined across rows into one single drawArrays call.
			// Two dummy vertices have to be added when switching from one row to the next one.
			// These dummy vertices each render an empty triangle
			// (e.g. a triangle where two corners are equal).
			bool combineRowPrimitives = (type == PRIM_TRIANGLE_STRIPS);
			// Row combination currently disabled for testing on ATI/AMD hardware.
			// Might be crashing the video driver.
			combineRowPrimitives = false;

			bool needSetDataPointers = true;
			int numberOfVertexPerRow = (context.patch_div_s + 1) * 2;
			if (cachedVertexInfo == null)
			{
				int ii = 0;
				for (int j = 0; j < context.patch_div_t; j++)
				{
					for (int i = 0; i <= context.patch_div_s; i++)
					{
						VertexState vs1 = patch[i][j];
						VertexState vs2 = patch[i][j + 1];

						if (useTexture)
						{
							floatBufferArray[ii++] = vs1.t[0];
							floatBufferArray[ii++] = vs1.t[1];
						}
						if (useVertexColor)
						{
							floatBufferArray[ii++] = vs1.c[0];
							floatBufferArray[ii++] = vs1.c[1];
							floatBufferArray[ii++] = vs1.c[2];
							floatBufferArray[ii++] = vs1.c[3];
						}
						if (useNormal)
						{
							floatBufferArray[ii++] = vs1.n[0];
							floatBufferArray[ii++] = vs1.n[1];
							floatBufferArray[ii++] = vs1.n[2];
						}
						floatBufferArray[ii++] = vs1.p[0];
						floatBufferArray[ii++] = vs1.p[1];
						floatBufferArray[ii++] = vs1.p[2];

						if (combineRowPrimitives && i == 0 && j > 0)
						{
							// First dummy vertex: add v1 again
							if (useTexture)
							{
								floatBufferArray[ii++] = vs1.t[0];
								floatBufferArray[ii++] = vs1.t[1];
							}
							if (useVertexColor)
							{
								floatBufferArray[ii++] = vs1.c[0];
								floatBufferArray[ii++] = vs1.c[1];
								floatBufferArray[ii++] = vs1.c[2];
								floatBufferArray[ii++] = vs1.c[3];
							}
							if (useNormal)
							{
								floatBufferArray[ii++] = vs1.n[0];
								floatBufferArray[ii++] = vs1.n[1];
								floatBufferArray[ii++] = vs1.n[2];
							}
							floatBufferArray[ii++] = vs1.p[0];
							floatBufferArray[ii++] = vs1.p[1];
							floatBufferArray[ii++] = vs1.p[2];
						}

						if (useTexture)
						{
							floatBufferArray[ii++] = vs2.t[0];
							floatBufferArray[ii++] = vs2.t[1];
						}
						if (useVertexColor)
						{
							floatBufferArray[ii++] = vs2.c[0];
							floatBufferArray[ii++] = vs2.c[1];
							floatBufferArray[ii++] = vs2.c[2];
							floatBufferArray[ii++] = vs2.c[3];
						}
						if (useNormal)
						{
							floatBufferArray[ii++] = vs2.n[0];
							floatBufferArray[ii++] = vs2.n[1];
							floatBufferArray[ii++] = vs2.n[2];
						}
						floatBufferArray[ii++] = vs2.p[0];
						floatBufferArray[ii++] = vs2.p[1];
						floatBufferArray[ii++] = vs2.p[2];

						if (combineRowPrimitives && i == context.patch_div_s && j < context.patch_div_t - 1)
						{
							// Second dummy vertex: add v2 again
							if (useTexture)
							{
								floatBufferArray[ii++] = vs2.t[0];
								floatBufferArray[ii++] = vs2.t[1];
							}
							if (useVertexColor)
							{
								floatBufferArray[ii++] = vs2.c[0];
								floatBufferArray[ii++] = vs2.c[1];
								floatBufferArray[ii++] = vs2.c[2];
								floatBufferArray[ii++] = vs2.c[3];
							}
							if (useNormal)
							{
								floatBufferArray[ii++] = vs2.n[0];
								floatBufferArray[ii++] = vs2.n[1];
								floatBufferArray[ii++] = vs2.n[2];
							}
							floatBufferArray[ii++] = vs2.p[0];
							floatBufferArray[ii++] = vs2.p[1];
							floatBufferArray[ii++] = vs2.p[2];
						}
					}
				}
				int bufferSizeInFloats = ii;

				if (useVertexCache)
				{
					cachedVertexInfo = new VertexInfo();
					int vtype = context.vinfo.vtype;
					if (useTexture)
					{
						// Float texture values
						vtype = (vtype & ~(0x3 << 0)) | VTYPE_TEXTURE_FORMAT_32_BIT;
					}
					if (useVertexColor)
					{
						// 8888 color values
						vtype = (vtype & ~(0x7 << 2)) | VTYPE_COLOR_FORMAT_32BIT_ABGR_8888;
					}
					if (useNormal)
					{
						// Float normal values
						vtype = (vtype & ~(0x3 << 5)) | VTYPE_NORMAL_FORMAT_32_BIT;
					}
					cachedVertexInfo.processType(vtype);
					int numberOfVertex = numberOfVertexPerRow * context.patch_div_t;
					VertexCache.Instance.addVertex(re, cachedVertexInfo, numberOfVertex, context.bone_uploaded_matrix, 0);
					needSetDataPointers = cachedVertexInfo.loadVertex(re, floatBufferArray, bufferSizeInFloats);
				}
				else
				{
					ByteBuffer byteBuffer = bufferManager.getBuffer(bufferId);
					byteBuffer.clear();
					byteBuffer.asFloatBuffer().put(floatBufferArray, 0, bufferSizeInFloats);
					bufferManager.setBufferSubData(pspsharp.graphics.RE.IRenderingEngine_Fields.RE_ARRAY_BUFFER, bufferId, 0, bufferSizeInFloats * SIZEOF_FLOAT, byteBuffer, pspsharp.graphics.RE.IRenderingEngine_Fields.RE_STREAM_DRAW);
				}
			}
			else
			{
				needSetDataPointers = cachedVertexInfo.bindVertex(re);
			}

			if (needSetDataPointers)
			{
				// TODO: Compute the normals
				setDataPointers(3, useVertexColor, 4, useTexture, 2, useNormal, 0, cachedVertexInfo == null);
			}

			if (combineRowPrimitives)
			{
				// Draw all the vertices in one call
				int numberOfVertexToDraw = numberOfVertexPerRow * context.patch_div_t + 2 * (context.patch_div_t - 1);
				drawArraysStatistics.start();
				re.drawArrays(type, 0, numberOfVertexToDraw);
				drawArraysStatistics.end();
			}
			else
			{
				// Draw the vertices one row at a time
				drawArraysStatistics.start();
				re.drawArrays(type, 0, numberOfVertexPerRow);
				for (int j = 1, first = numberOfVertexPerRow; j < context.patch_div_t; j++, first += numberOfVertexPerRow)
				{
					re.drawArraysBurstMode(type, first, numberOfVertexPerRow);
				}
				drawArraysStatistics.end();
			}

			if (State.captureGeNextFrame)
			{
				display.captureGeImage();
				textureChanged = true;
			}

			endRendering(ucount * vcount);
		}

		private VertexState[][] getControlPoints(int ucount, int vcount)
		{
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: VertexState[][] controlPoints = new VertexState[ucount][vcount];
			VertexState[][] controlPoints = RectangularArrays.ReturnRectangularVertexStateArray(ucount, vcount);

			bool readTexture = context.textureFlag.Enabled;
			Memory mem = Memory.Instance;
			for (int cu = 0; cu < ucount; cu++)
			{
				for (int cv = 0; cv < vcount; cv++)
				{
					int addr = context.vinfo.getAddress(mem, cv * ucount + cu);
					VertexState vs = context.vinfo.readVertex(mem, addr, readTexture, DoubleTexture2DCoords);
					if (context.vinfo.weight != 0 && context.vinfo.position != 0)
					{
						doSkinning(context.bone_uploaded_matrix, context.vinfo, vs);
					}
					if (isLogDebugEnabled)
					{
						log(string.Format("control point #{0:D},{1:D} p({2:F},{3:F},{4:F}) t({5:F},{6:F}), c(0x{7:X8})", cu, cv, vs.p[0], vs.p[1], vs.p[2], vs.t[0], vs.t[1], PixelColor.getColor(vs.c)));
					}
					controlPoints[cu][cv] = vs;
				}
			}
			return controlPoints;
		}

		private float[] BernsteinCoeff(float u)
		{
			float uPow2 = u * u;
			float uPow3 = uPow2 * u;
			float u1 = 1 - u;
			float u1Pow2 = u1 * u1;
			float u1Pow3 = u1Pow2 * u1;
			return new float[]{u1Pow3, 3 * u * u1Pow2, 3 * uPow2 * u1, uPow3};
		}

		private Buffer getTextureBuffer(int texaddr, int bytesPerPixel, int level, int textureBufferWidthInPixels)
		{
			Buffer final_buffer = null;

			if (!context.texture_swizzle)
			{
				// texture_width might be larger than texture_buffer_width
				int bufferlen = System.Math.Max(textureBufferWidthInPixels, context.texture_width[level]) * context.texture_height[level] * bytesPerPixel;
				final_buffer = Memory.Instance.getBuffer(texaddr, bufferlen);

				if (State.captureGeNextFrame)
				{
					log_Renamed.info("Capture getTextureBuffer unswizzled");
					CaptureManager.captureRAM(texaddr, bufferlen);
				}
			}
			else
			{
				final_buffer = unswizzleTextureFromMemory(texaddr, bytesPerPixel, level, textureBufferWidthInPixels);
			}

			return final_buffer;
		}

//JAVA TO C# CONVERTER WARNING: 'sealed override' parameters are not available in .NET:
//ORIGINAL LINE: public static String getPsmName(sealed override int psm)
		public static string getPsmName(int psm)
		{
			return (psm >= 0 && psm < psm_names.Length) ? psm_names[psm % psm_names.Length] : "PSM_UNKNOWN" + psm;
		}

//JAVA TO C# CONVERTER WARNING: 'sealed override' parameters are not available in .NET:
//ORIGINAL LINE: public static String getLOpName(sealed override int ops)
		public static string getLOpName(int ops)
		{
			return (ops >= 0 && ops < logical_ops_names.Length) ? logical_ops_names[ops % logical_ops_names.Length] : "UNKNOWN_LOP" + ops;
		}

		private int getCompressedTextureSize(int level, int compressionRatio)
		{
			// We load the texture with the size (texture_width, texture_height) using OpenGL.
			// Do not use the texture_buffer_width here.
			return getCompressedTextureSize(context.texture_width[level], context.texture_height[level], compressionRatio);
		}

		public static int getCompressedTextureSize(int width, int height, int compressionRatio)
		{
			return Round4(width) * Round4(height) * 4 / compressionRatio;
		}

		private void updateGeBuf()
		{
			if (geBufChanged)
			{
				display.hleDisplaySetGeBuf(context.fbp, context.fbw, context.psm, somethingDisplayed, forceLoadGEToScreen);

				if (useTextureCache)
				{
					TextureCache.Instance.deleteVramTextures(re, context.fbp, context.fbw * context.scissor_y2 * pspsharp.graphics.RE.IRenderingEngine_Fields.sizeOfTextureType[context.psm]);
				}

				forceLoadGEToScreen = false;
				geBufChanged = false;

				textureChanged = true;
				maxSpriteHeight = 0;
				maxSpriteWidth = 0;
				projectionMatrixUpload.Changed = true;
				modelMatrixUpload.Changed = true;
				viewMatrixUpload.Changed = true;
				textureMatrixUpload.Changed = true;
				viewportChanged = true;
				depthChanged = true;
				materialChanged = true;

				// wait for rendering completion when switching the GE buffer (in case it is reused as a texture)
				re.waitForRenderingCompletion();
			}
		}
		// For capture/replay

		public virtual int FBP
		{
			get
			{
				return context.fbp;
			}
		}

		public virtual int FBW
		{
			get
			{
				return context.fbw;
			}
		}

		public virtual int ZBP
		{
			get
			{
				return context.zbp;
			}
		}

		public virtual int ZBW
		{
			get
			{
				return context.zbw;
			}
		}

		public virtual int PSM
		{
			get
			{
				return context.psm;
			}
		}

		private bool VertexBufferEmbedded
		{
			get
			{
				// stall_addr may be 0
				return (context.vinfo.ptr_vertex >= currentList.list_addr && context.vinfo.ptr_vertex < currentList.StallAddr);
			}
		}

		public virtual int ClutNumEntries
		{
			get
			{
				// E.g. mask==0xFF requires 256 entries
				// also mask==0xF0 requires 256 entries
				return Integer.highestOneBit(context.tex_clut_mask | (context.tex_clut_start << 4)) << 1;
			}
		}

		private void hlePerformAction(IAction action, Semaphore sync)
		{
			hleAction = action;

			while (true)
			{
				try
				{
					sync.acquire();
					break;
				}
				catch (InterruptedException)
				{
					// Retry again..
				}
			}
		}

		public virtual void hleSaveContext(int addr)
		{
			// If we are rendering, we have to wait for a consistent state
			// before saving the context: let the display thread perform
			// the save when appropriate.
			if (hasDrawLists() || currentList != null)
			{
				Semaphore sync = new Semaphore(0);
				hlePerformAction(new SaveContextAction(this, addr, sync), sync);
			}
			else
			{
				saveContext(addr);
			}
		}

		public virtual void hleRestoreContext(int addr)
		{
			// If we are rendering, we have to wait for a consistent state
			// before restoring the context: let the display thread perform
			// the restore when appropriate.
			if (hasDrawLists() || currentList != null)
			{
				Semaphore sync = new Semaphore(0);
				hlePerformAction(new RestoreContextAction(this, addr, sync), sync);
			}
			else
			{
				restoreContext(addr);
			}
		}

		private void saveContext(int addr)
		{
			context.write(Memory.Instance, addr);
		}

		private void restoreContext(int addr)
		{
			context.read(Memory.Instance, addr);
			context.setDirty();

			projectionMatrixUpload.Changed = true;
			modelMatrixUpload.Changed = true;
			viewMatrixUpload.Changed = true;
			textureMatrixUpload.Changed = true;
			lightingChanged = true;
			textureChanged = true;
			geBufChanged = true;
			viewportChanged = true;
			depthChanged = true;
			materialChanged = true;
		}

		public virtual bool UsingTRXKICK
		{
			get
			{
				return usingTRXKICK;
			}
		}

		public virtual int MaxSpriteHeight
		{
			get
			{
				return maxSpriteHeight;
			}
		}

		public virtual int MaxSpriteWidth
		{
			get
			{
				return maxSpriteWidth;
			}
		}

		private bool UseVertexCache
		{
			set
			{
				// VertexCache is relying on VBO
				if (bufferManager != null && !bufferManager.useVBO())
				{
					value = false;
				}
    
				this.useVertexCache = value;
				if (value)
				{
					if (useAsyncVertexCache_Renamed)
					{
						AsyncVertexCache.Instance;
						if (useOptimisticVertexCache)
						{
							log_Renamed.info("Using Optimistic Async Vertex Cache");
						}
						else
						{
							log_Renamed.info("Using Async Vertex Cache");
						}
					}
					else
					{
						VertexCache.Instance;
						if (useOptimisticVertexCache)
						{
							log_Renamed.info("Using Optimistic Vertex Cache");
						}
						else
						{
							log_Renamed.info("Using Vertex Cache");
						}
					}
				}
			}
		}

		public virtual bool useAsyncVertexCache()
		{
			return useVertexCache && useAsyncVertexCache_Renamed;
		}

		public virtual bool disableOptimizedVertexInfoReading()
		{
			return disableOptimizedVertexInfoReading_Renamed;
		}

		private bool DisableOptimizedVertexInfoReading
		{
			set
			{
				this.disableOptimizedVertexInfoReading_Renamed = value;
    
				if (value)
				{
					log_Renamed.info("Using non-optimized VertexInfo reading");
				}
			}
		}

		public virtual int Base
		{
			get
			{
				return context.@base;
			}
			set
			{
				context.@base = value;
			}
		}


		public virtual int BaseOffset
		{
			get
			{
				return context.baseOffset;
			}
			set
			{
				context.baseOffset = value;
			}
		}


		private void addToVideoTextures(int startAddress, int endAddress)
		{
			// Synchronize the access to videoTextures as it can be accessed
			// from parallel threads (async display thread and PSP thread)
			lock (videoTextures)
			{
				foreach (AddressRange addressRange in videoTextures)
				{
					if (addressRange.Equals(startAddress, endAddress))
					{
						addressRange.hit();
						return;
					}
				}

				AddressRange addressRange = new AddressRange(startAddress, endAddress);
				videoTextures.AddLast(addressRange);
			}
		}

		public virtual void addVideoTexture(int destinationAddress, int sourceAddress, int Length)
		{
			if (ExternalGE.Active)
			{
				ExternalGE.addVideoTexture(destinationAddress, sourceAddress, Length);
			}

			addToVideoTextures(destinationAddress, destinationAddress + Length);
		}

		public virtual void addVideoTexture(int startAddress, int endAddress)
		{
			if (ExternalGE.Active)
			{
				ExternalGE.addVideoTexture(startAddress, startAddress, endAddress - startAddress);
			}

			addToVideoTextures(startAddress, endAddress);

			if (display != null && display.isFbAddress(startAddress))
			{
				display.GeDirty = true;
			}
		}

		public virtual void resetVideoTextures()
		{
			// Synchronize the access to videoTextures as it can be accessed
			// from a parallel threads (async display and PSP thread)
			lock (videoTextures)
			{
				videoTextures.Clear();
			}
		}

		public virtual bool UseTextureAnisotropicFilter
		{
			get
			{
				return useTextureAnisotropicFilter;
			}
			set
			{
				this.useTextureAnisotropicFilter = value;
			}
		}


		public virtual bool UsexBRZFilter
		{
			get
			{
				return usexBRZFilter;
			}
			set
			{
				this.usexBRZFilter = value;
			}
		}


		public virtual bool SkipThisFrame
		{
			set
			{
				this.skipThisFrame = value;
			}
			get
			{
				return skipThisFrame;
			}
		}


		public virtual void addCachedInstructions(int address, int[] instructions)
		{
			cachedInstructions[address] = instructions;
		}

		public virtual void clearCachedInstructions()
		{
			// TODO When should be cached instructions be cleared?
			cachedInstructions.Clear();
		}

		public virtual void clearTextureCache()
		{
			// Clear the cache before starting the rendering
			wantClearTextureCache = true;
		}

		public virtual void clearVertexCache()
		{
			// Clear the cache before starting the rendering
			wantClearVertexCache = true;
		}

		private void initTextureBuffers()
		{
			int maxTextureSize = 1 << maxTextureSizeLog2;
			// We might need more space for a texture of size 512x512 and bufferWidth=1024
			int tmpBufferSize = System.Math.Max(maxTextureSize, 1024) * maxTextureSize;
			if (tmp_texture_buffer32 == null || tmp_texture_buffer32.Length != tmpBufferSize)
			{
				// Tell the garbage collector that the old arrays are no longer in use.
				tmp_texture_buffer32 = null;
				tmp_texture_buffer16 = null;

				// Create the new arrays
				tmp_texture_buffer32 = new int[tmpBufferSize];
				tmp_texture_buffer16 = new short[tmpBufferSize];
			}
		}

		public virtual int MaxTextureSize
		{
			set
			{
				if ((1 << maxTextureSizeLog2) != value)
				{
					maxTextureSizeLog2 = 31 - Integer.numberOfLeadingZeros(value);
					log_Renamed.info(string.Format("Using maxTextureSize={0:D}(log2={1:D})", value, maxTextureSizeLog2));
				}
				initTextureBuffers();
			}
		}

		public virtual bool DoubleTexture2DCoords
		{
			set
			{
				if (this.doubleTexture2DCoords != value)
				{
					this.doubleTexture2DCoords = value;
	//JAVA TO C# CONVERTER TODO TASK: The following line has a Java format specifier which cannot be directly translated to .NET:
	//ORIGINAL LINE: System.Console.WriteLine(String.format("Using DoubleTexture2DCoords %b", value));
					log_Renamed.info(string.Format("Using DoubleTexture2DCoords %b", value));
				}
			}
			get
			{
				return doubleTexture2DCoords;
			}
		}


		private class SaveContextAction : IAction
		{
			private readonly VideoEngine outerInstance;


			internal int addr;
			internal Semaphore sync;

			public SaveContextAction(VideoEngine outerInstance, int addr, Semaphore sync)
			{
				this.outerInstance = outerInstance;
				this.addr = addr;
				this.sync = sync;
			}

			public virtual void execute()
			{
				outerInstance.saveContext(addr);
				sync.release();
			}
		}

		private class RestoreContextAction : IAction
		{
			private readonly VideoEngine outerInstance;


			internal int addr;
			internal Semaphore sync;

			public RestoreContextAction(VideoEngine outerInstance, int addr, Semaphore sync)
			{
				this.outerInstance = outerInstance;
				this.addr = addr;
				this.sync = sync;
			}

			public virtual void execute()
			{
				outerInstance.restoreContext(addr);
				outerInstance.context.update();
				sync.release();
			}
		}

		private class AddressRange
		{

			internal int start;
			internal int end;
			internal long timestamp;
			internal const long expirationTime = 1000; // 1 second

			public AddressRange(int start, int end)
			{
				this.start = start & Memory.addressMask;
				this.end = end & Memory.addressMask;
				hit();
			}

			public virtual bool contains(int address)
			{
				address &= Memory.addressMask;

				return address >= start && address < end;
			}

			public virtual bool Equals(int start, int end)
			{
				start &= Memory.addressMask;
				end &= Memory.addressMask;

				return start == this.start && end == this.end;
			}

			internal static long CurrentTimestamp
			{
				get
				{
					return Emulator.Clock.currentTimeMillis();
				}
			}

			public virtual void hit()
			{
				timestamp = CurrentTimestamp;
			}

			public virtual bool Obsolete
			{
				get
				{
					long now = CurrentTimestamp;
					return now - timestamp >= expirationTime;
				}
			}
		}

		protected internal class VertexIndexInfo
		{

			internal int minIndex;
			internal int maxIndex;
			internal bool sequence;

			public VertexIndexInfo(int minIndex, int maxIndex, bool sequence)
			{
				this.minIndex = minIndex;
				this.maxIndex = maxIndex;
				this.sequence = sequence;
			}

			public virtual int MinIndex
			{
				get
				{
					return minIndex;
				}
			}

			public virtual int MaxIndex
			{
				get
				{
					return maxIndex;
				}
			}

			public virtual int NumberOfVertex
			{
				get
				{
					return maxIndex - minIndex + 1;
				}
			}

			public virtual bool Sequence
			{
				get
				{
					return sequence;
				}
			}
		}
	}

}