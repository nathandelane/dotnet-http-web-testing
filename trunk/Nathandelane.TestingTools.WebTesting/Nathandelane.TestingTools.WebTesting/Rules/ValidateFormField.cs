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
using Nathandelane.TestingTools.WebTesting;
using System.Xml.Linq;

namespace Nathandelane.TestingTools.WebTesting.Rules
{
	public class ValidateFormField : ValidationRule
	{
		#region Fields

		private string _name;
		private string _expectedValue;

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the form field's name.
		/// </summary>
		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}

		/// <summary>
		/// Gets or sets the form field's expected value.
		/// </summary>
		public string ExpectedValue
		{
			get { return _expectedValue; }
			set { _expectedValue = value; }
		}

		#endregion

		#region Methods

		public override void Validate(object sender, Events.ValidationEventArgs e)
		{
			if (String.IsNullOrEmpty(_name))
			{
				throw new ArgumentNullException("Name may not be null.");
			}

			try
			{
				base.Validate(sender, e);

				IEnumerable<XElement> formElements = base.Document.Document.Descendants(XName.Get("input", "http://www.w3.org/1999/xhtml"));

				foreach (XElement nextElement in formElements)
				{
					if (nextElement.Attribute(XName.Get("value")) != null && nextElement.Attribute(XName.Get("value")).Value.Equals(_expectedValue))
					{
						_context.Outcome = WebTestOutcome.Passed;

						break;
					}
				}

				if (_context.Outcome != WebTestOutcome.Passed)
				{
					_context.Outcome = WebTestOutcome.Failed;
				}
			}
			catch (Exception ex)
			{
				_context.Outcome = WebTestOutcome.Error;
				_message = ex.Message;
			}
		}

		#endregion
	}
}
