﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

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
namespace pspsharp.Debugger.FileLogger
{


	using Modules = pspsharp.HLE.Modules;
	using IVirtualFile = pspsharp.HLE.VFS.IVirtualFile;
	using IIoListener = pspsharp.HLE.modules.IoFileMgrForUser.IIoListener;
	using SeekableDataInput = pspsharp.filesystems.SeekableDataInput;
	using UmdIsoReader = pspsharp.filesystems.umdiso.UmdIsoReader;
	using Settings = pspsharp.settings.Settings;
	using Constants = pspsharp.util.Constants;

	/// 
	/// <summary>
	/// @author fiveofhearts
	/// </summary>
	public class FileLoggerFrame : javax.swing.JFrame, ThreadStart, IIoListener
	{

		private const long serialVersionUID = 8455039521164613143L;
		private FileHandleModel fileHandleModel;
		private FileCommandModel fileCommandModel;
		private Thread refreshThread;
		private volatile bool dirty;
		private volatile bool sortRequired;

		/// <summary>
		/// Creates new form FileLoggerFrame
		/// </summary>
		public FileLoggerFrame()
		{
			fileHandleModel = new FileHandleModel(this);
			fileCommandModel = new FileCommandModel(this);

			initComponents();
			postInit();

			refreshThread = new Thread(this, "FileLogger");
			refreshThread.Start();

			if (Settings.Instance.readBool("emu.debug.enablefilelogger"))
			{
				cbFileTrace.Selected = true;
				Modules.IoFileMgrForUserModule.registerIoListener(this);
			}

			WindowPropSaver.loadWindowProperties(this);
		}

		public override bool Visible
		{
			set
			{
				base.Visible = value;
    
				lock (Instance)
				{
					if (!dirty)
					{
						dirty = true;
						Monitor.Pulse(Instance);
					}
				}
			}
		}

		/// <summary>
		/// This method is called from within the constructor to initialize the form.
		/// WARNING: Do NOT modify this code. The content of this method is always
		/// regenerated by the Form Editor.
		/// </summary>
		// <editor-fold defaultstate="collapsed" desc="Generated Code">//GEN-BEGIN:initComponents
		private void initComponents()
		{

			jPopupMenu1 = new JPopupMenu();
			copyItem = new JMenuItem();
			saveAsItem = new JMenuItem();
			jSplitPane1 = new javax.swing.JSplitPane();
			jScrollPane1 = new javax.swing.JScrollPane();
			commandLogTable = new JTable();
			jScrollPane2 = new javax.swing.JScrollPane();
			fileHandleTable = new JTable();
			cbFileTrace = new javax.swing.JCheckBox();

			copyItem.Accelerator = javax.swing.KeyStroke.getKeyStroke(java.awt.@event.KeyEvent.VK_C, java.awt.@event.InputEvent.CTRL_MASK);
			java.util.ResourceBundle bundle = java.util.ResourceBundle.getBundle("pspsharp/languages/pspsharp"); // NOI18N
			copyItem.Text = bundle.getString("FileLoggerFrame.copyItem.text"); // NOI18N
			copyItem.addActionListener(new ActionListenerAnonymousInnerClass(this));
			jPopupMenu1.add(copyItem);

			saveAsItem.Text = bundle.getString("FileLoggerFrame.saveAsItem.text"); // NOI18N
			saveAsItem.addActionListener(new ActionListenerAnonymousInnerClass2(this));
			jPopupMenu1.add(saveAsItem);

			DefaultCloseOperation = javax.swing.WindowConstants.DISPOSE_ON_CLOSE;
			Title = bundle.getString("FileLoggerFrame.title"); // NOI18N
			MinimumSize = new java.awt.Dimension(400, 200);

			jSplitPane1.DividerLocation = 100;
			jSplitPane1.Orientation = javax.swing.JSplitPane.VERTICAL_SPLIT;
			jSplitPane1.MinimumSize = new java.awt.Dimension(179, 100);

			commandLogTable.Model = fileCommandModel;
			commandLogTable.InheritsPopupMenu = true;
			commandLogTable.MinimumSize = new java.awt.Dimension(200, 100);
			commandLogTable.Name = bundle.getString("FileLoggerFrame.commandLogTable.name"); // NOI18N
			commandLogTable.addMouseListener(new MouseAdapterAnonymousInnerClass(this));
			jScrollPane1.ViewportView = commandLogTable;

			jSplitPane1.BottomComponent = jScrollPane1;

			fileHandleTable.Model = fileHandleModel;
			fileHandleTable.InheritsPopupMenu = true;
			fileHandleTable.MinimumSize = new java.awt.Dimension(200, 100);
			fileHandleTable.Name = bundle.getString("FileLoggerFrame.fileHandleTable.name"); // NOI18N
			fileHandleTable.addMouseListener(new MouseAdapterAnonymousInnerClass2(this));
			jScrollPane2.ViewportView = fileHandleTable;

			jSplitPane1.TopComponent = jScrollPane2;

			cbFileTrace.Text = bundle.getString("FileLoggerFrame.cbFileTrace.text"); // NOI18N
			cbFileTrace.addActionListener(new ActionListenerAnonymousInnerClass3(this));

			javax.swing.GroupLayout layout = new javax.swing.GroupLayout(ContentPane);
			ContentPane.Layout = layout;
			layout.HorizontalGroup = layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING).addGroup(layout.createSequentialGroup().addContainerGap().addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING).addGroup(layout.createSequentialGroup().addComponent(cbFileTrace).addGap(0, 0, short.MaxValue)).addComponent(jSplitPane1, javax.swing.GroupLayout.DEFAULT_SIZE, 628, short.MaxValue)).addContainerGap());
			layout.VerticalGroup = layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING).addGroup(javax.swing.GroupLayout.Alignment.TRAILING, layout.createSequentialGroup().addContainerGap().addComponent(cbFileTrace).addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED).addComponent(jSplitPane1, javax.swing.GroupLayout.DEFAULT_SIZE, 257, short.MaxValue).addContainerGap());

			pack();
		} // </editor-fold>//GEN-END:initComponents

		private class ActionListenerAnonymousInnerClass : java.awt.@event.ActionListener
		{
			private readonly FileLoggerFrame outerInstance;

			public ActionListenerAnonymousInnerClass(FileLoggerFrame outerInstance)
			{
				this.outerInstance = outerInstance;
			}

			public void actionPerformed(ActionEvent evt)
			{
				outerInstance.copyItemActionPerformed(evt);
			}
		}

		private class ActionListenerAnonymousInnerClass2 : java.awt.@event.ActionListener
		{
			private readonly FileLoggerFrame outerInstance;

			public ActionListenerAnonymousInnerClass2(FileLoggerFrame outerInstance)
			{
				this.outerInstance = outerInstance;
			}

			public void actionPerformed(ActionEvent evt)
			{
				outerInstance.saveAsItemActionPerformed(evt);
			}
		}

		private class MouseAdapterAnonymousInnerClass : java.awt.@event.MouseAdapter
		{
			private readonly FileLoggerFrame outerInstance;

			public MouseAdapterAnonymousInnerClass(FileLoggerFrame outerInstance)
			{
				this.outerInstance = outerInstance;
			}

			public void mousePressed(java.awt.@event.MouseEvent evt)
			{
				outerInstance.tableMousePressed(evt);
			}
			public void mouseReleased(java.awt.@event.MouseEvent evt)
			{
				outerInstance.tableMouseReleased(evt);
			}
		}

		private class MouseAdapterAnonymousInnerClass2 : java.awt.@event.MouseAdapter
		{
			private readonly FileLoggerFrame outerInstance;

			public MouseAdapterAnonymousInnerClass2(FileLoggerFrame outerInstance)
			{
				this.outerInstance = outerInstance;
			}

			public void mousePressed(java.awt.@event.MouseEvent evt)
			{
				outerInstance.tableMousePressed(evt);
			}
			public void mouseReleased(java.awt.@event.MouseEvent evt)
			{
				outerInstance.tableMouseReleased(evt);
			}
		}

		private class ActionListenerAnonymousInnerClass3 : java.awt.@event.ActionListener
		{
			private readonly FileLoggerFrame outerInstance;

			public ActionListenerAnonymousInnerClass3(FileLoggerFrame outerInstance)
			{
				this.outerInstance = outerInstance;
			}

			public void actionPerformed(ActionEvent evt)
			{
				outerInstance.cbFileTraceActionPerformed(evt);
			}
		}

		private void tableMousePressed(java.awt.@event.MouseEvent evt)
		{ //GEN-FIRST:event_tableMousePressed
			if (evt.PopupTrigger)
			{
				jPopupMenu1.show(evt.Component, evt.X, evt.Y);
			}
		} //GEN-LAST:event_tableMousePressed

		private void tableMouseReleased(java.awt.@event.MouseEvent evt)
		{ //GEN-FIRST:event_tableMouseReleased
			if (evt.PopupTrigger)
			{
				jPopupMenu1.show(evt.Component, evt.X, evt.Y);
			}
		} //GEN-LAST:event_tableMouseReleased

		private void copyItemActionPerformed(ActionEvent evt)
		{ //GEN-FIRST:event_copyItemActionPerformed
			JTable source = (JTable)((JPopupMenu)((JMenuItem) evt.Source).Parent).Invoker;

			ActionEvent ae = new ActionEvent(source, ActionEvent.ACTION_PERFORMED, "");
			source.ActionMap.get("copy").actionPerformed(ae);
		} //GEN-LAST:event_copyItemActionPerformed

		private void saveAsItemActionPerformed(ActionEvent evt)
		{ //GEN-FIRST:event_saveAsItemActionPerformed
			java.util.ResourceBundle bundle = java.util.ResourceBundle.getBundle("pspsharp/languages/pspsharp");
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'sealed override':
//ORIGINAL LINE: sealed override javax.swing.JFileChooser fc = new javax.swing.JFileChooser();
			JFileChooser fc = new JFileChooser();
			fc.DialogTitle = bundle.getString("FileLoggerFrame.strSaveTable.text");
			fc.SelectedFile = new File(State.discId + "_fileio.txt");
			fc.CurrentDirectory = new File(".");
			fc.addChoosableFileFilter(Constants.fltTextFiles);
			fc.FileFilter = Constants.fltTextFiles;

			if (fc.showSaveDialog(this) == JFileChooser.APPROVE_OPTION)
			{
				File f = fc.SelectedFile;
				if (f.exists())
				{

					int rc = MessageBox.Show(this, bundle.getString("ConsoleWindow.strFileExists.text"), bundle.getString("ConsoleWindow.strFileExistsTitle.text"), MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);

					if (rc != DialogResult.Yes)
					{
						return;
					}
				}

				try
				{
					JTable source = (JTable)((JPopupMenu)((JMenuItem) evt.Source).Parent).Invoker;
					string data = "";

					// list column headers
					for (int j = 0; j < source.ColumnCount; j++)
					{
						data += source.getColumnName(j) + ";";
					}
					// strip last semicolon and put a newline there instead
					data = data.Substring(0, data.Length - 1) + System.getProperty("line.separator");

					// list table content
					for (int i = 0; i < source.RowCount; i++)
					{
						for (int j = 0; j < source.ColumnCount; j++)
						{

							data += source.Model.getValueAt(i, j) + ";";
						}
						// strip last semicolon and put a newline there instead
						data = data.Substring(0, data.Length - 1) + System.getProperty("line.separator");
					}

					System.IO.StreamWriter os = new System.IO.StreamWriter(f);
					os.Write(data);
					os.Close();
				}
				catch (IOException ioe)
				{
					MessageBox.Show(this, bundle.getString("FileLoggerFrame.strSaveFailed.text") + ioe.LocalizedMessage);
				}
			}
		} //GEN-LAST:event_saveAsItemActionPerformed

		private FileLoggerFrame Instance
		{
			get
			{
				return this;
			}
		}

		private void cbFileTraceActionPerformed(ActionEvent evt)
		{ //GEN-FIRST:event_cbFileTraceActionPerformed
			if (cbFileTrace.Selected)
			{
				Modules.IoFileMgrForUserModule.registerIoListener(this);
				Settings.Instance.writeBool("emu.debug.enablefilelogger", true);
			}
			else
			{
				Modules.IoFileMgrForUserModule.unregisterIoListener(this);
				Settings.Instance.writeBool("emu.debug.enablefilelogger", false);
			}
		} //GEN-LAST:event_cbFileTraceActionPerformed

		// TODO does fireTableDataChanged need to be in the swing thread?
		// if not we could just call fireTableDataChanged(); from the logging functions
		public override void run()
		{
			ThreadStart refresher = () =>
			{
			if (sortRequired)
			{
				sortRequired = false;
				sortLists();
			}

			// Scroll to bottom of the tables
			int max = jScrollPane1.VerticalScrollBar.Maximum;
			jScrollPane1.VerticalScrollBar.Value = max;

			max = jScrollPane2.VerticalScrollBar.Maximum;
			jScrollPane2.VerticalScrollBar.Value = max;

			// Tell the tables to redraw
			fileHandleModel.fireTableDataChanged();
			fileCommandModel.fireTableDataChanged();
			};

			while (true)
			{
				try
				{
					lock (this)
					{
						while (!dirty)
						{
							Monitor.Wait(this);
						}
						dirty = false;
					}

					if (Instance.Visible)
					{
						SwingUtilities.invokeAndWait(refresher);
					}

					// Cap update frequency
					Thread.Sleep(200);
				}
				catch (Exception e)
				{
					Console.WriteLine(e.ToString());
					Console.Write(e.StackTrace);
				}
			}
		}

		/// <summary>
		/// Renders closed files in gray.
		/// </summary>
		public class FileHandleRenderer : DefaultTableCellRenderer
		{
			private readonly FileLoggerFrame outerInstance;

			public FileHandleRenderer(FileLoggerFrame outerInstance)
			{
				this.outerInstance = outerInstance;
			}


			internal const long serialVersionUID = -792377736132676194L;

			public override Component getTableCellRendererComponent(JTable table, object value, bool isSelected, bool hasFocus, int row, int column)
			{

				Component c = base.getTableCellRendererComponent(table, value, isSelected, hasFocus, row, column);

				c.ForegRound = Color.black;

				if (outerInstance.fileHandleList != null)
				{
					FileHandleInfo info = outerInstance.fileHandleList[row];
					if (!info.Open)
					{
						c.ForegRound = Color.gray;
					}
				}

				return c;
			}
		}

		private class FileHandleModel : AbstractTableModel
		{
			private readonly FileLoggerFrame outerInstance;

			public FileHandleModel(FileLoggerFrame outerInstance)
			{
				this.outerInstance = outerInstance;
			}


			internal const long serialVersionUID = -109193689444035593L;

			public override int ColumnCount
			{
				get
				{
					return 4;
				}
			}

			public override int RowCount
			{
				get
				{
					if (outerInstance.fileHandleList != null)
					{
						return outerInstance.fileHandleList.Count;
					}
					return 0;
				}
			}

			public override string getColumnName(int columnIndex)
			{
				java.util.ResourceBundle bundle = java.util.ResourceBundle.getBundle("pspsharp/languages/pspsharp");
				switch (columnIndex)
				{
					case 0:
						return bundle.getString("FileLoggerFrame.strFileID.text");
					case 1:
						return bundle.getString("FileLoggerFrame.strFileName.text");
					case 2:
						return bundle.getString("FileLoggerFrame.strRead.text");
					case 3:
						return bundle.getString("FileLoggerFrame.strWrite.text");
					default:
						throw new System.ArgumentException("invalid column index");
				}
			}

			public override object getValueAt(int row, int col)
			{
				FileHandleInfo info = outerInstance.fileHandleList[row];
				if (info != null)
				{
					switch (col)
					{
						case 0:
							return string.Format("0x{0:X4}", info.fd);
						case 1:
							return info.filename;
						case 2:
							return info.bytesRead;
						case 3:
							return info.bytesWritten;
					}
				}
				return null;
			}
		}

		private class FileCommandModel : AbstractTableModel
		{
			private readonly FileLoggerFrame outerInstance;

			public FileCommandModel(FileLoggerFrame outerInstance)
			{
				this.outerInstance = outerInstance;
			}


			internal const long serialVersionUID = -5088674695489235024L;

			public override int ColumnCount
			{
				get
				{
					return 6;
				}
			}

			public override int RowCount
			{
				get
				{
					if (outerInstance.fileCommandList != null)
					{
						return outerInstance.fileCommandList.Count;
					}
					return 0;
				}
			}

			public override string getColumnName(int columnIndex)
			{
				java.util.ResourceBundle bundle = java.util.ResourceBundle.getBundle("pspsharp/languages/pspsharp");
				switch (columnIndex)
				{
					case 0:
						return bundle.getString("FileLoggerFrame.strThreadID.text");
					case 1:
						return bundle.getString("FileLoggerFrame.strThreadName.text");
					case 2:
						return bundle.getString("FileLoggerFrame.strFileID.text");
					case 3:
						return bundle.getString("FileLoggerFrame.strCommand.text");
					case 4:
						return bundle.getString("FileLoggerFrame.strResult.text");
					case 5:
						return bundle.getString("FileLoggerFrame.strParameters.text");
					default:
						throw new System.ArgumentException("invalid column index");
				}
			}

			public override object getValueAt(int row, int col)
			{
				FileCommandInfo info = outerInstance.fileCommandList[row];
				if (info != null)
				{
					switch (col)
					{
						case 0:
							return string.Format("0x{0:X8}", info.threadId);
						case 1:
							return info.threadName;
						case 2:
							return (info.hasFd) ? string.Format("0x{0:X4}", info.fd) : "";
						case 3:
							return (info.occurences == 1) ? info.command : info.command + " " + info.occurences + "x";
						case 4:
							return string.Format("0x{0:X8}", info.result);
						case 5:
							return info.parameters;
					}
				}
				return null;
			}
		}

		public void postInit()
		{
			TableColumnModel columns;

			// We want the middle column to be the widest
			columns = fileHandleTable.ColumnModel;
			columns.getColumn(0).PreferredWidth = 75;
			columns.getColumn(1).PreferredWidth = 500;
			columns.getColumn(2).PreferredWidth = 75;
			columns.getColumn(3).PreferredWidth = 75;

			fileHandleTable.setDefaultRenderer(typeof(object), new FileHandleRenderer(this));

			columns = commandLogTable.ColumnModel;
			columns.getColumn(0).PreferredWidth = 75;
			columns.getColumn(1).PreferredWidth = 90;
			columns.getColumn(2).PreferredWidth = 50;
			columns.getColumn(4).PreferredWidth = 75;
			columns.getColumn(5).PreferredWidth = 275;

			resetLogging();

			// uncomment this for testing purposes
			// test();
		}

		public virtual void test()
		{
			Console.Error.WriteLine("test start");

			resetLogging();

			// file handle table
			sceIoOpen(1, 0x08800000, "test1.txt", 0xFF, 0xFF, "rw");
			sceIoOpen(2, 0x08800000, "test2.txt", 0xFF, 0xFF, "rw");
			sceIoOpen(3, 0x08800000, DateTimeHelper.CurrentUnixTimeMillis() + ".txt", 0xFF, 0xFF, "rw");
			sceIoClose(0, 1);
			sceIoClose(0, 2);
			sceIoOpen(1, 0x08800000, "test1.txt", 0xFF, 0xFF, "rw");

			// file command table
			sceIoRead(0x0, 1, 0x08800000, 0x400, 0x0, 0, null, null);

			Console.Error.WriteLine("test done");
		}

		private class FileCommandInfo
		{
			private readonly FileLoggerFrame outerInstance;


			public readonly bool hasFd;
			public readonly int threadId;
			public readonly string threadName;
			public readonly int fd;
			public readonly string command;
			public readonly int result;
			public readonly string parameters;
			public int occurences;

			internal FileCommandInfo(FileLoggerFrame outerInstance, bool hasFd, int fd, string command, int result, string parameters)
			{
				this.outerInstance = outerInstance;
				this.hasFd = hasFd;

				threadId = Modules.ThreadManForUserModule.CurrentThreadID;
				threadName = Modules.ThreadManForUserModule.getThreadName(threadId);
				this.fd = fd;
				this.command = command;
				this.result = result;
				this.parameters = parameters;
				occurences = 1;

				lock (outerInstance.Instance)
				{
					if (!outerInstance.dirty)
					{
						outerInstance.dirty = true;
						Monitor.Pulse(outerInstance.Instance);
					}
				}
			}

			/// <summary>
			/// Example: 0x1001, "close", 0x0, ""
			/// </summary>
			public FileCommandInfo(FileLoggerFrame outerInstance, int fd, string command, int result, string parameters) : this(outerInstance, true, fd, command, result, parameters)
			{
				this.outerInstance = outerInstance;
			}

			/// <summary>
			/// Example: "open", 0x1001, "path='test.txt' flags=0xFF perm=0777"
			/// </summary>
			public FileCommandInfo(FileLoggerFrame outerInstance, string command, int result, string parameters) : this(outerInstance, false, -2, command, result, parameters)
			{
				this.outerInstance = outerInstance;
			}

			public override bool Equals(object _obj)
			{
				if (_obj is FileCommandInfo)
				{
					FileCommandInfo obj = (FileCommandInfo) _obj;
					return threadId == obj.threadId && fd == obj.fd && command.Equals(obj.command) && result == obj.result && parameters.Equals(obj.parameters);
				}
				return false;
			}

			public override int GetHashCode()
			{
				int hash = 7;
				hash = 43 * hash + threadId;
				hash = 43 * hash + fd;
				hash = 43 * hash + (!string.ReferenceEquals(command, null) ? command.GetHashCode() : 0);
				hash = 43 * hash + result;
				hash = 43 * hash + (!string.ReferenceEquals(parameters, null) ? parameters.GetHashCode() : 0);
				return hash;
			}
		}
		// Emu interface
		private Dictionary<int, FileHandleInfo> fileHandleIdMap;
		private IList<FileHandleInfo> fileHandleList; // Cached sorted version of fileHandleIdMap
		private IList<FileCommandInfo> fileCommandList;

		public virtual void resetLogging()
		{
			lock (this)
			{
				fileHandleIdMap = new Dictionary<int, FileHandleInfo>();
				fileHandleList = new LinkedList<FileHandleInfo>();
        
				fileCommandList = new LinkedList<FileCommandInfo>();
        
				if (!dirty)
				{
					dirty = true;
					Monitor.Pulse(Instance);
				}
			}
		}

		private void sortLists()
		{
			// File handles
			Dictionary<int, FileHandleInfo>.ValueCollection c = fileHandleIdMap.Values;
			fileHandleList = new LinkedList<FileHandleInfo>(c);
			fileHandleList.Sort();
		}
		/// <summary>
		/// Handles repeated commands
		/// </summary>
		private FileCommandInfo lastFileCommand;

		private void logFileCommand(FileCommandInfo info)
		{
			if (lastFileCommand != null && info.Equals(lastFileCommand))
			{
				lastFileCommand.occurences++;
			}
			else
			{
				fileCommandList.Add(info);
				lastFileCommand = info;
			}
		}

		public virtual void sceIoSync(int result, int device_addr, string device, int unknown)
		{
			logFileCommand(new FileCommandInfo(this, "sync", result, string.Format("device=0x{0:X8}('{1}') unknown=0x{2:X8}", device_addr, device, unknown)));
		}

		public virtual void sceIoPollAsync(int result, int uid, int res_addr)
		{
			logFileCommand(new FileCommandInfo(this, uid, "poll async", result, string.Format("result=0x{0:X8}", res_addr)));
		}

		public virtual void sceIoWaitAsync(int result, int uid, int res_addr)
		{
			logFileCommand(new FileCommandInfo(this, uid, "wait async", result, string.Format("result=0x{0:X8}", res_addr)));
		}

		public virtual void sceIoOpen(int result, int filename_addr, string filename, int flags, int permissions, string mode)
		{
			// File handle list
			if (result >= 0)
			{
				FileHandleInfo info = new FileHandleInfo(result, filename);
				fileHandleIdMap[result] = info;
				sortRequired = true;
			}

			string filelog = string.Format("path=0x{0:X8}('{1}')", filename_addr, filename);
			if (filename.StartsWith("disc0:/sce_lbn", StringComparison.Ordinal))
			{
				// try to resolve LBA addressing if possible
				UmdIsoReader iso = Modules.IoFileMgrForUserModule.IsoReader;
				if (iso != null)
				{
					string filePath = filename;
					filePath = filePath.Substring(14); // Length of "disc0:/sce_lba"
					int sep = filePath.IndexOf("_size", StringComparison.Ordinal);
					int fileStart = Integer.decode(filePath.Substring(0, sep));
					string resolved = iso.getFileName(fileStart);
					if (!string.ReferenceEquals(resolved, null))
					{
						filelog = string.Format("path=0x{0:X8}('{1}', '{2}')", filename_addr, filename, resolved);
					}
				}
			}
			filelog += string.Format(" flags=0x{0:X4}, permissions=0x{1:X4}({2})", flags, permissions, mode);

			// File Command list
			logFileCommand(new FileCommandInfo(this, "open", result, filelog));
		}

		public virtual void sceIoClose(int result, int uid)
		{
			// File handle list
			if (result >= 0)
			{
				FileHandleInfo info = fileHandleIdMap[uid];
				if (info != null)
				{
					info.isOpen(false);
				}
			}

			// File Command list
			logFileCommand(new FileCommandInfo(this, uid, "close", result, ""));
		}

		public virtual void sceIoWrite(int result, int uid, int data_addr, int size, int bytesWritten)
		{
			FileHandleInfo info = fileHandleIdMap[uid];
			if (result >= 0 && info != null)
			{
				info.bytesWritten += bytesWritten;
			}

			logFileCommand(new FileCommandInfo(this, uid, "write", result, string.Format("data=0x{0:X8} size=0x{1:X8}", data_addr, size)));
		}

		public virtual void sceIoRead(int result, int uid, int data_addr, int size, int bytesRead, long position, SeekableDataInput dataInput, IVirtualFile vFile)
		{
			FileHandleInfo info = fileHandleIdMap[uid];
			if (result >= 0 && info != null)
			{
				info.bytesRead += bytesRead;
			}

			logFileCommand(new FileCommandInfo(this, uid, "read", result, string.Format("data=0x{0:X8} size=0x{1:X8}", data_addr, size)));
		}

		public virtual void sceIoCancel(int result, int uid)
		{
			logFileCommand(new FileCommandInfo(this, uid, "cancel", result, ""));
		}

		private string getWhenceName(int whence)
		{
			switch (whence)
			{
				case pspsharp.HLE.modules.IoFileMgrForUser.PSP_SEEK_SET:
					return whence + "(set)";
				case pspsharp.HLE.modules.IoFileMgrForUser.PSP_SEEK_CUR:
					return whence + "(cur)";
				case pspsharp.HLE.modules.IoFileMgrForUser.PSP_SEEK_END:
					return whence + "(end)";
				default:
					return "" + whence;
			}
		}

		public virtual void sceIoSeek32(int result, int uid, int offset, int whence)
		{
			logFileCommand(new FileCommandInfo(this, uid, "seek32", result, string.Format("offset=0x{0:X8} whence={1}", offset, getWhenceName(whence))));
		}

		public virtual void sceIoSeek64(long result, int uid, long offset, int whence)
		{
			logFileCommand(new FileCommandInfo(this, uid, "seek64", (int) result, string.Format("offset=0x{0:X8} whence={1}", offset, getWhenceName(whence))));
		}

		public virtual void sceIoMkdir(int result, int path_addr, string path, int permissions)
		{
			logFileCommand(new FileCommandInfo(this, "mkdir", result, string.Format("path=0x{0:X8}('{1}') permissions={2:X4}", path_addr, path, permissions)));
		}

		public virtual void sceIoRmdir(int result, int path_addr, string path)
		{
			logFileCommand(new FileCommandInfo(this, "rmdir", result, string.Format("path=0x{0:X8}('{1}')", path_addr, path)));
		}

		public virtual void sceIoChdir(int result, int path_addr, string path)
		{
			logFileCommand(new FileCommandInfo(this, "chdir", result, string.Format("path=0x{0:X8}('{1}')", path_addr, path)));
		}

		public virtual void sceIoDopen(int result, int path_addr, string path)
		{
			logFileCommand(new FileCommandInfo(this, "dopen", result, string.Format("path=0x{0:X8}('{1}')", path_addr, path)));
		}

		public virtual void sceIoDread(int result, int uid, int dirent_addr)
		{
			logFileCommand(new FileCommandInfo(this, uid, "dread", result, string.Format("dirent=0x{0:X8}", dirent_addr)));
		}

		public virtual void sceIoDclose(int result, int uid)
		{
			logFileCommand(new FileCommandInfo(this, uid, "dclose", result, ""));
		}

		public virtual void sceIoDevctl(int result, int device_addr, string device, int cmd, int indata_addr, int inlen, int outdata_addr, int outlen)
		{
			logFileCommand(new FileCommandInfo(this, "devctl", result, string.Format("device=0x{0:X8}('{1}') cmd=0x{2:X8} indata=0x{3:X8} inlen=0x{4:X8} outdata=0x{5:X8} outlen=0x{6:X8}", device_addr, device, cmd, indata_addr, inlen, outdata_addr, outlen)));
		}

		public virtual void sceIoIoctl(int result, int uid, int cmd, int indata_addr, int inlen, int outdata_addr, int outlen)
		{
			logFileCommand(new FileCommandInfo(this, uid, "ioctl", result, string.Format("cmd=0x{0:X8} indata=0x{1:X8} inlen=0x{2:X8} outdata=0x{3:X8} outlen=0x{4:X8}", cmd, indata_addr, inlen, outdata_addr, outlen)));
		}

		public virtual void sceIoAssign(int result, int dev1_addr, string dev1, int dev2_addr, string dev2, int dev3_addr, string dev3, int mode, int unk1, int unk2)
		{
			logFileCommand(new FileCommandInfo(this, "assign", result, string.Format("dev1=0x{0:X8}('{1}') dev2=0x{2:X8}('{3}') dev3=0x{4:X8}('{5}') mode=0x{6:X8} unk1=0x{7:X8} unk2=0x{8:X8}", dev1_addr, dev1, dev2_addr, dev2, dev3_addr, dev3, mode, unk1, unk2)));
		}

		public virtual void sceIoGetStat(int result, int path_addr, string path, int stat_addr)
		{
			logFileCommand(new FileCommandInfo(this, "stat", result, string.Format("path=0x{0:X8}('{1}') stat=0x{2:X8}", path_addr, path, stat_addr)));
		}

		public virtual void sceIoRemove(int result, int path_addr, string path)
		{
			logFileCommand(new FileCommandInfo(this, "remove", result, string.Format("path=0x{0:X8}('{1}')", path_addr, path)));
		}

		public virtual void sceIoChstat(int result, int path_addr, string path, int stat_addr, int bits)
		{
			logFileCommand(new FileCommandInfo(this, "chstat", result, string.Format("path=0x{0:X8}('{1}') stat=0x{2:X8} bits=0x{3:X8}", path_addr, path, stat_addr, bits)));
		}

		public virtual void sceIoRename(int result, int path_addr, string path, int new_path_addr, string newpath)
		{
			logFileCommand(new FileCommandInfo(this, "rename", result, string.Format("path=0x{0:X8}('{1}') newpath=0x{2:X8}('{3}')", path_addr, path, new_path_addr, newpath)));
		}

		public override void dispose()
		{
			Emulator.MainGUI.endWindowDialog();
			base.dispose();
		}
		// Variables declaration - do not modify//GEN-BEGIN:variables
		private javax.swing.JCheckBox cbFileTrace;
		private JTable commandLogTable;
		private JMenuItem copyItem;
		private JTable fileHandleTable;
		private JPopupMenu jPopupMenu1;
		private javax.swing.JScrollPane jScrollPane1;
		private javax.swing.JScrollPane jScrollPane2;
		private javax.swing.JSplitPane jSplitPane1;
		private JMenuItem saveAsItem;
		// End of variables declaration//GEN-END:variables
	}

}