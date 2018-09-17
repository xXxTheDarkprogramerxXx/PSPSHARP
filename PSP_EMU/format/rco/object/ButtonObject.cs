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
namespace pspsharp.format.rco.@object
{

	using EventType = pspsharp.format.rco.type.EventType;
	using ImageType = pspsharp.format.rco.type.ImageType;
	using IntType = pspsharp.format.rco.type.IntType;
	using TextType = pspsharp.format.rco.type.TextType;

	public class ButtonObject : BasePositionObject
	{
		[ObjectField(order : 201)]
		public ImageType image;
		[ObjectField(order : 202)]
		public ImageType shadow;
		[ObjectField(order : 203)]
		public ImageType focus;
		[ObjectField(order : 204)]
		public TextType text;
		[ObjectField(order : 205)]
		public EventType onPush;
		[ObjectField(order : 206)]
		public EventType onFocusIn;
		[ObjectField(order : 207)]
		public EventType onFocusOut;
		[ObjectField(order : 208)]
		public EventType onFocusLeft;
		[ObjectField(order : 209)]
		public EventType onFocusRight;
		[ObjectField(order : 210)]
		public EventType onFocusUp;
		[ObjectField(order : 211)]
		public EventType onFocusDown;
		[ObjectField(order : 212)]
		public EventType onContextMenu;
		[ObjectField(order : 213)]
		public IntType unknownInt40;

		public override System.Drawing.Bitmap Image
		{
			get
			{
				return image.Image;
			}
		}
	}

}