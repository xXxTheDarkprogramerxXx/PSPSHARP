﻿using System;
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
namespace pspsharp.Debugger
{
	using Utilities = pspsharp.util.Utilities;

	/// 
	/// <summary>
	/// @author shadow
	/// </summary>
	public class System.ConsoleWindow : javax.swing.JFrame
	{
		private bool InstanceFieldsInitialized = false;

		private void InitializeInstanceFields()
		{
			m_stdoutPS = new PrintStream(new JTextAreaOutStream(this, new System.IO.MemoryStream()));
		}


		private const long serialVersionUID = 1L;
		[NonSerialized]
		private PrintStream m_stdoutPS;
		/// <summary>
		/// Display infinite characters in the textarea, no limit.
		/// <para>
		/// <b>NOTE:</b> Will slow down your application if a lot of messages are to
		/// be displayed to the textarea (more than a couple of Kbytes).
		/// </para>
		/// </summary>
		private int m_maxChars = -1;

		/// <summary>
		/// Creates new form LoggingWindow
		/// </summary>
		public System.ConsoleWindow()
		{
			if (!InstanceFieldsInitialized)
			{
				InitializeInstanceFields();
				InstanceFieldsInitialized = true;
			}
			initComponents();
			System.Out = m_stdoutPS;

		}

		/// <summary>
		/// This method is called from within the constructor to initialize the form.
		/// WARNING: Do NOT modify this code. The content of this method is always
		/// regenerated by the Form Editor.
		/// </summary>
		// <editor-fold defaultstate="collapsed" desc="Generated Code">//GEN-BEGIN:initComponents
		private void initComponents()
		{

			jScrollPane1 = new javax.swing.JScrollPane();
			talogging = new javax.swing.JTextArea();
			ClearMessageButton = new javax.swing.JButton();
			SaveMessageToFileButton = new javax.swing.JButton();

			java.util.ResourceBundle bundle = java.util.ResourceBundle.getBundle("pspsharp/languages/pspsharp"); // NOI18N
			Title = bundle.getString("System.ConsoleWindow.title"); // NOI18N
			Resizable = false;

			talogging.Columns = 20;
			talogging.Font = new java.awt.Font("Courier New", 0, 12); // NOI18N
			talogging.Rows = 5;
			jScrollPane1.ViewportView = talogging;

			ClearMessageButton.Text = bundle.getString("System.ConsoleWindow.ClearMessageButton.text"); // NOI18N
			ClearMessageButton.addActionListener(new ActionListenerAnonymousInnerClass(this));

			SaveMessageToFileButton.Text = bundle.getString("System.ConsoleWindow.SaveMessageToFileButton.text"); // NOI18N
			SaveMessageToFileButton.addActionListener(new ActionListenerAnonymousInnerClass2(this));

			javax.swing.GroupLayout layout = new javax.swing.GroupLayout(ContentPane);
			ContentPane.Layout = layout;
			layout.HorizontalGroup = layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING).addGroup(layout.createSequentialGroup().addContainerGap(334, short.MaxValue).addComponent(SaveMessageToFileButton).addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED).addComponent(ClearMessageButton)).addComponent(jScrollPane1, javax.swing.GroupLayout.Alignment.TRAILING, javax.swing.GroupLayout.DEFAULT_SIZE, 583, short.MaxValue);
			layout.VerticalGroup = layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING).addGroup(javax.swing.GroupLayout.Alignment.TRAILING, layout.createSequentialGroup().addComponent(jScrollPane1, javax.swing.GroupLayout.DEFAULT_SIZE, 215, short.MaxValue).addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED).addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING).addComponent(ClearMessageButton).addGroup(javax.swing.GroupLayout.Alignment.TRAILING, layout.createSequentialGroup().addComponent(SaveMessageToFileButton).addContainerGap())));

			pack();
		} // </editor-fold>//GEN-END:initComponents

		private class ActionListenerAnonymousInnerClass : java.awt.@event.ActionListener
		{
			private readonly System.ConsoleWindow outerInstance;

			public ActionListenerAnonymousInnerClass(System.ConsoleWindow outerInstance)
			{
				this.outerInstance = outerInstance;
			}

			public void actionPerformed(java.awt.@event.ActionEvent evt)
			{
				outerInstance.ClearMessageButtonActionPerformed(evt);
			}
		}

		private class ActionListenerAnonymousInnerClass2 : java.awt.@event.ActionListener
		{
			private readonly System.ConsoleWindow outerInstance;

			public ActionListenerAnonymousInnerClass2(System.ConsoleWindow outerInstance)
			{
				this.outerInstance = outerInstance;
			}

			public void actionPerformed(java.awt.@event.ActionEvent evt)
			{
				outerInstance.SaveMessageToFileButtonActionPerformed(evt);
			}
		}

	private void ClearMessageButtonActionPerformed(java.awt.@event.ActionEvent evt)
	{ //GEN-FIRST:event_ClearMessageButtonActionPerformed
			clearScreenMessages();
	} //GEN-LAST:event_ClearMessageButtonActionPerformed

	private void SaveMessageToFileButtonActionPerformed(java.awt.@event.ActionEvent evt)
	{ //GEN-FIRST:event_SaveMessageToFileButtonActionPerformed
			java.util.ResourceBundle bundle = java.util.ResourceBundle.getBundle("pspsharp/languages/pspsharp"); // NOI18N
			JFileChooser m_fileChooser = new JFileChooser();
			m_fileChooser.SelectedFile = new File("logoutput.txt");
			m_fileChooser.DialogTitle = bundle.getString("System.ConsoleWindow.strSaveLogging.text");
			m_fileChooser.CurrentDirectory = new File(".");
			int returnVal = m_fileChooser.showSaveDialog(this);
			if (returnVal != JFileChooser.APPROVE_OPTION)
			{
				return;
			}
			File f = m_fileChooser.SelectedFile;
			System.IO.StreamWriter @out = null;
			try
			{
				if (f.exists())
				{
					int res = MessageBox.Show(this, bundle.getString("System.ConsoleWindow.strFileExists.text"), bundle.getString("System.ConsoleWindow.strFileExistsTitle.text"), MessageBoxButtons.YesNo, MessageBoxIcon.Question);

					if (res != 0)
					{
						return;
					}
				}

				//IOHelper.saveTxtFile(f, ta_messages.getText(), false);
				@out = new System.IO.StreamWriter(f);
				@out.BaseStream.WriteByte(talogging.Text);
			}
			catch (Exception e)
			{
				System.Console.WriteLine(e.ToString());
				System.Console.Write(e.StackTrace);
			}
			finally
			{
				Utilities.close(@out);
			}
	} //GEN-LAST:event_SaveMessageToFileButtonActionPerformed

		/// <summary>
		/// Clears only the messages that are displayed in the textarea.
		/// </summary>
		public virtual void clearScreenMessages()
		{
			lock (this)
			{
				talogging.Text = "";
			}
		}
		// Variables declaration - do not modify//GEN-BEGIN:variables
		private javax.swing.JButton ClearMessageButton;
		private javax.swing.JButton SaveMessageToFileButton;
		private javax.swing.JScrollPane jScrollPane1;
		private javax.swing.JTextArea talogging;
		// End of variables declaration//GEN-END:variables

		/// <summary>
		/// Private inner class. Filter to redirect the data to the textarea.
		/// </summary>
		private sealed class JTextAreaOutStream : FilterOutputStream
		{
			private readonly System.ConsoleWindow outerInstance;


			/// <summary>
			/// Constructor.
			/// <para>
			/// 
			/// </para>
			/// </summary>
			/// <param name="aStream"> The <code>OutputStream</code>. </param>
			public JTextAreaOutStream(System.ConsoleWindow outerInstance, System.IO.Stream aStream) : base(aStream)
			{
				this.outerInstance = outerInstance;
			}

			/// <summary>
			/// Writes the messages.
			/// <para>
			/// 
			/// </para>
			/// </summary>
			/// <param name="b"> The message in a <code>byte[]</code> array.
			/// <para>
			/// </para>
			/// </param>
			/// <exception cref="IOException"> </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: @Override public synchronized void write(byte b[]) throws java.io.IOException
			public override void write(sbyte[] b)
			{
				lock (this)
				{
					string s = StringHelper.NewString(b);
					appendMessage(s);
					flushTextArea();
				}
			}

			/// <summary>
			/// Writes the messages.
			/// <para>
			/// 
			/// </para>
			/// </summary>
			/// <param name="b"> The message in a <code>byte[]</code> array. </param>
			/// <param name="off"> The offset. </param>
			/// <param name="len"> Length.
			/// <para>
			/// </para>
			/// </param>
			/// <exception cref="IOException"> </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: @Override public synchronized void write(byte b[], int off, int len) throws java.io.IOException
			public override void write(sbyte[] b, int off, int len)
			{
				lock (this)
				{
					string s = StringHelper.NewString(b, off, len);
					appendMessage(s);
					flushTextArea();
				}
			}

			/// <summary>
			/// Appends a message to the textarea and the
			/// <para>
			/// 
			/// </para>
			/// </summary>
			/// <param name="s"> The message. </param>
			internal void appendMessage(string s)
			{
				lock (this)
				{
					outerInstance.talogging.append(s);
				}
			}

			internal void flushTextArea()
			{
				lock (this)
				{
					int len = outerInstance.talogging.Text.Length();
        
					// Always scroll down to the last line
					outerInstance.talogging.CaretPosition = len;
        
					// if we have set a maximum characters limit and
					// we have exceeded that limit, clear the messages
					if (outerInstance.m_maxChars > 0 && len > outerInstance.m_maxChars)
					{
						outerInstance.clearScreenMessages();
					}
				}
			}
		}
	}

}