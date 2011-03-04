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
using System.Net;

namespace Nathandelane.TestingTools.WebTesting
{
	public class WebTestContext
	{
		#region Fields

		private static WebTestContext __context;

		private IDictionary<string, object> _contextItems;
		private CookieCollection _cookies;

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets a context item.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public object this[string key]
		{
			get
			{
				object result = String.Empty;

				if (_contextItems.ContainsKey(key))
				{
					result = _contextItems[key];
				}

				return result;
			}
			set { _contextItems[key] = value; }
		}

		/// <summary>
		/// Gets the number of items stored in the context.
		/// </summary>
		public int Count
		{
			get { return _contextItems.Count; }
		}

		/// <summary>
		/// Gets or sets the cookies to use for the next request.
		/// </summary>
		public CookieCollection Cookies
		{
			get { return _cookies; }
			set { _cookies = value; }
		}

		/// <summary>
		/// Gets or sets the outcome of the test.
		/// </summary>
		public WebTestOutcome Outcome { get; set; }

		#endregion

		#region Constructors

		private WebTestContext()
		{
			_contextItems = new Dictionary<string, object>();
			_cookies = new CookieCollection();
		}

		#endregion

		#region Methods

		/// <summary>
		/// Gets a reference to the instance of the context.
		/// </summary>
		/// <returns></returns>
		public static WebTestContext GetContext()
		{
			if (WebTestContext.__context == null)
			{
				WebTestContext.__context = new WebTestContext();
			}

			return WebTestContext.__context;
		}

		/// <summary>
		/// Resets the current context instance.
		/// </summary>
		public void Reset()
		{
			WebTestContext.__context = new WebTestContext();
		}

		/// <summary>
		/// Checks whether the context contains an item.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public bool ContainsKey(string key)
		{
			return _contextItems.ContainsKey(key);
		}

		#endregion
	}
}
