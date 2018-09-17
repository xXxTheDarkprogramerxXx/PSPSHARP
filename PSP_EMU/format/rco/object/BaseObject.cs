using System.Text;

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
namespace pspsharp.format.rco.@object
{

    //using Logger = org.apache.log4j.Logger;

    using RCOEntry = pspsharp.format.RCO.RCOEntry;
    using BaseType = pspsharp.format.rco.type.BaseType;
    using EventType = pspsharp.format.rco.type.EventType;
    using VSMX = pspsharp.format.rco.vsmx.VSMX;
    using VSMXBaseObject = pspsharp.format.rco.vsmx.interpreter.VSMXBaseObject;
    using VSMXInterpreter = pspsharp.format.rco.vsmx.interpreter.VSMXInterpreter;
    using VSMXNativeObject = pspsharp.format.rco.vsmx.interpreter.VSMXNativeObject;
    using VSMXString = pspsharp.format.rco.vsmx.interpreter.VSMXString;
    using BaseNativeObject = pspsharp.format.rco.vsmx.objects.BaseNativeObject;
    using Controller = pspsharp.format.rco.vsmx.objects.Controller;
    using Scheduler = pspsharp.scheduler.Scheduler;
    using System.Reflection;
    using System.Collections.Generic;
    using System;

    public abstract class BaseObject : BaseNativeObject
	{
		////protected internal new Logger log =VSMX.log;
		protected internal Display display;
		protected internal Controller controller;
		private string name;

		private class FieldComparator : IComparer<FieldInfo>
		{
			public virtual int Compare(FieldInfo f1, FieldInfo f2)
			{
                ObjectField o1 = (ObjectField)(f1.GetCustomAttributes(typeof(ObjectField), true)[0]);
                ObjectField o2 = (ObjectField)(f2.GetCustomAttributes(typeof(ObjectField), true)[0]);
                if (o1 == null || o2 == null)
				{
					return 0;
				}

				return o1.order - o2.order;
			}

		}

		private FieldInfo[] Fields
		{
			get
			{
				return this.GetType().GetFields();
			}
		}

		private FieldInfo[] SortedFields
		{
			get
			{
				FieldInfo[] fields = Fields;
    
				// According the definition of getFields():
				//   The elements in the array returned are not sorted and are not in any particular order.
				// So now, we need to sort the fields according to the "ObjectField" annotation.
				Array.Sort(fields, new FieldComparator());
    
				return fields;
			}
		}

		public virtual void read(RCOContext context)
		{
			FieldInfo[] fields = SortedFields;
			foreach (FieldInfo FieldInfo in fields)
			{
                if (FieldInfo.ReflectedType.IsAssignableFrom(typeof(BaseType)))
				{
					try
					{
						BaseType baseType = (BaseType) FieldInfo;
						if (baseType == null)
						{
							baseType = (BaseType) FieldInfo.Type.newInstance();
							FieldInfo = baseType;
						}
						baseType.read(context);
					}
					catch (InstantiationException)
					{
						// Ignore error
					}
					catch (IllegalAccessException)
					{
						// Ignore error
					}
				}
			}
		}

		public virtual int size()
		{
			int size = 0;
			FieldInfo[] fields = SortedFields;
			foreach (FieldInfo FieldInfo in fields)
			{
                if (FieldInfo.FieldType.IsAssignableFrom(typeof(BaseType)))
				{
					try
					{
						BaseType baseType = (BaseType) FieldInfo.ReflectedType.BaseType;
						if (baseType == null)
						{
							baseType = (BaseType) FieldInfo.ReflectedType.newInstance();
						}
						size += baseType.size();
					}
					catch (IllegalAccessException)
					{
						// Ignore error
					}
					catch (InstantiationException)
					{
						// Ignore error
					}
				}
			}

			return size;
		}

		public virtual VSMXBaseObject createVSMXObject(VSMXInterpreter interpreter, VSMXBaseObject parent, RCOEntry entry)
		{
			VSMXNativeObject @object = new VSMXNativeObject(interpreter, this);
			Object = @object;
			entry.vsmxBaseObject = @object;
			if (!string.ReferenceEquals(entry.label, null))
			{
				name = entry.label;
				@object.setPropertyValue("name", new VSMXString(interpreter, entry.label));
				parent.setPropertyValue(entry.label, @object);
			}

			if (entry.parent != null && entry.parent.vsmxBaseObject is VSMXNativeObject)
			{
				Parent = ((VSMXNativeObject) entry.parent.vsmxBaseObject).Object;
			}

			return @object;
		}

		public virtual Display Display
		{
			set
			{
				this.display = value;
			}
		}

		public virtual void onDisplayUpdated()
		{
			display.repaint();
		}

		public virtual Controller Controller
		{
			set
			{
				this.controller = value;
			}
		}

		protected internal static Scheduler Scheduler
		{
			get
			{
				return Emulator.Scheduler;
			}
		}

		public virtual string Name
		{
			get
			{
				return name;
			}
		}

		protected internal virtual void trigger(EventType @event)
		{
			if (!string.ReferenceEquals(@event.Event, null))
			{
				controller.Interpreter.interpretScript(@event.Object.Object, @event.Event);
			}
			else if (@event.Object != null && @event.Object is BasePositionObject)
			{
				((BasePositionObject) @event.Object).setFocus();
			}
		}

		public virtual void init(RCOContext context)
		{
			FieldInfo[] fields = Fields;
			foreach (FieldInfo FieldInfo in fields)
			{
				if (FieldInfo.Type.IsAssignableFrom(typeof(BaseType)))
				{
					try
					{
						BaseType baseType = (BaseType) FieldInfo.get(this);
						if (baseType != null)
						{
							baseType.init(context);
						}
					}
					catch (IllegalAccessException)
					{
						// Ignore error
					}
				}
			}
		}

		protected internal virtual void ToString(StringBuilder s)
		{
		}

		public override string ToString()
		{
			StringBuilder s = new StringBuilder();

			FieldInfo[] fields = SortedFields;
			s.Append(string.Format("{0}[name={1}", this.GetType().Name, name));
			bool firstField = false;
			foreach (FieldInfo FieldInfo in fields)
			{
				if (FieldInfo.Type.IsAssignableFrom(typeof(BaseType)))
				{
					try
					{
						BaseType baseType = (BaseType) FieldInfo.get(this);
						if (firstField)
						{
							firstField = false;
						}
						else
						{
							s.Append(", ");
						}
						s.Append(string.Format("{0}=({1})", FieldInfo.Name, baseType));
					}
					catch (IllegalAccessException)
					{
						// Ignore error
					}
				}
			}
			ToString(s);
			s.Append("]");

			return s.ToString();
		}
	}

}