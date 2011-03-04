/**
 * Web Testing Framework to automate HTTP-based web testing.
 * Copyright (C) 2011 Nathan Lane
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nathandelane.TestingTools.WebTesting
{
	public abstract class WebTestItem : ICloneable
	{
		#region Fields

		private static int _itemId = -1;

		#endregion

		#region Properties

		/// <summary>
		/// Zero-based sequence number of the item in the Web performance test.
		/// </summary>
		public int ItemId
		{
			get { return WebTestItem._itemId; }
		}

		#endregion

		#region Constructor

		public WebTestItem()
		{
			WebTestItem._itemId++;
		}

		#endregion

		#region Methods

		/// <summary>
		/// When it is overridden in a derived class, creates a copy of this object. 
		/// </summary>
		/// <returns></returns>
		public object Clone()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Determines whether the specified Object is equal to the current Object. (Inherited from Object.)
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}

		/// <summary>
		/// Serves as a hash function for a particular type. (Inherited from Object.)
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		/// <summary>
		/// Returns a String that represents the current Object. (Inherited from Object.)
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return base.ToString();
		}

		/// <summary>
		/// Executes the NetTestItem.
		/// </summary>
		public virtual void Execute()
		{
			throw new NotImplementedException("Execute is not implemented.");
		}

		#endregion
	}
}
