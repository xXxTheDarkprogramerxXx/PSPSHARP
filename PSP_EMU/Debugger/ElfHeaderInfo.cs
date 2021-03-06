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
namespace pspsharp.Debugger
{

	public class ElfHeaderInfo : javax.swing.JFrame
	{

		private const long serialVersionUID = 1L;
		public static string PbpInfo;
		public static string ElfInfo;
		public static string ProgInfo;
		public static string SectInfo;

		/// <summary>
		/// Creates new form ElfHeaderInfo
		/// </summary>
		public ElfHeaderInfo()
		{
			initComponents();
			ELFInfoArea.append(PbpInfo);
			ELFInfoArea.append(ElfInfo);
			ELFInfoArea.append(ProgInfo);
			ELFInfoArea.append(SectInfo);

			WindowPropSaver.loadWindowProperties(this);
		}

		public virtual void RefreshWindow()
		{
			ELFInfoArea.Text = "";
			ELFInfoArea.append(PbpInfo);
			ELFInfoArea.append(ElfInfo);
			ELFInfoArea.append(ProgInfo);
			ELFInfoArea.append(SectInfo);
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
			ELFInfoArea = new javax.swing.JTextArea();

			java.util.ResourceBundle bundle = java.util.ResourceBundle.getBundle("pspsharp/languages/pspsharp"); // NOI18N
			Title = bundle.getString("ElfHeaderInfo.title"); // NOI18N
			Resizable = false;

			ELFInfoArea.Editable = false;
			ELFInfoArea.Columns = 20;
			ELFInfoArea.Font = new java.awt.Font("Courier New", 0, 12); // NOI18N
			ELFInfoArea.LineWrap = true;
			ELFInfoArea.Rows = 5;
			jScrollPane1.ViewportView = ELFInfoArea;

			javax.swing.GroupLayout layout = new javax.swing.GroupLayout(ContentPane);
			ContentPane.Layout = layout;
			layout.HorizontalGroup = layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING).addComponent(jScrollPane1, javax.swing.GroupLayout.DEFAULT_SIZE, 253, short.MaxValue);
			layout.VerticalGroup = layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING).addComponent(jScrollPane1, javax.swing.GroupLayout.DEFAULT_SIZE, 329, short.MaxValue);

			pack();
		} // </editor-fold>//GEN-END:initComponents

		public override void dispose()
		{
			Emulator.MainGUI.endWindowDialog();
			base.dispose();
		}
		// Variables declaration - do not modify//GEN-BEGIN:variables
		private javax.swing.JTextArea ELFInfoArea;
		private javax.swing.JScrollPane jScrollPane1;
		// End of variables declaration//GEN-END:variables
	}

}