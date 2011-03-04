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
	public abstract class WebTest
	{
		#region Fields

		private WebTestContext _context;
		private Guid _guid;
		private string _name;
		private bool _preAuthenticate;

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the context for this test.
		/// </summary>
		public WebTestContext Context
		{
			get { return _context; }
			set { _context = value; }
		}

		/// <summary>
		/// Gets or sets the outcome of this test.
		/// </summary>
		public WebTestOutcome Outcome
		{
			get { return _context.Outcome; }
			set { _context.Outcome = value; }
		}

		/// <summary>
		/// Gets or sets the GUID for this test.
		/// </summary>
		public Guid Guid
		{
			get { return _guid; }
			set { _guid = value; }
		}

		/// <summary>
		/// Gets or sets the name of this test.
		/// </summary>
		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}

		/// <summary>
		/// Gets or sets whether to pre-authenticate every request.
		/// </summary>
		public bool PreAtuhenticate
		{
			get { return _preAuthenticate; }
			set { _preAuthenticate = value; }
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Creates an instance of WebTest.
		/// </summary>
		public WebTest()
		{
			_context = WebTestContext.GetContext();
			_guid = Guid.NewGuid();
		}

		#endregion

		#region Methods

		/// <summary>
		/// When overridden in a derived class, returns an IEnumerator<T> interface that supports a simple iteration over a generic collection of WebTestRequest.
		/// </summary>
		/// <returns></returns>
		public abstract IEnumerator<WebTestRequest> GetRequestEnumerator();

		#endregion
	}
}
