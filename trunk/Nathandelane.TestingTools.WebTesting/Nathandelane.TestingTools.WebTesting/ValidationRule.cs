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
using Nathandelane.TestingTools.WebTesting.Events;
using System.IO;
using System.Xml.Linq;

namespace Nathandelane.TestingTools.WebTesting
{
	public abstract class ValidationRule
	{
		#region Fields
		
		protected WebTestContext _context;
		protected string _message;

		private XDocument _document;
		private string _ruleDescription;
		private string _ruleName;
		private bool _required;

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the description of this ExtractionRule.
		/// </summary>
		public string RuleDescription
		{
			get { return _ruleDescription; }
			set { _ruleName = value; }
		}

		/// <summary>
		/// Gets or sets the name of this ExtractionRule.
		/// </summary>
		public string RuleName
		{
			get { return _ruleName; }
			set { _ruleName = value; }
		}

		/// <summary>
		/// Gets the Response body as an XDocument object.
		/// </summary>
		public XDocument Document
		{
			get { return _document; }
		}

		/// <summary>
		/// Gets or sets whether this rule is required.
		/// </summary>
		public bool Required
		{
			get { return _required; }
			set { _required = value; }
		}

		/// <summary>
		/// Gets the results of the web test.
		/// </summary>
		public WebTestOutcome Outcome
		{
			get { return _context.Outcome; }
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Creates an instance of ValidationRule.
		/// </summary>
		public ValidationRule()
		{
			_context = WebTestContext.GetContext();
		}

		/// <summary>
		/// Creates an instance of ValidationRule.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="description"></param>
		public ValidationRule(string name, string description)
		{
			_ruleName = name;
			_ruleDescription = description;
			_context = WebTestContext.GetContext();
		}

		#endregion

		#region Methods

		/// <summary>
		/// Returns a string representation of this ValidationRule based on the name and description.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return String.Format("{0}: {1}; {2}: {3}", _ruleName, _ruleDescription, _context.Outcome, _message);
		}

		/// <summary>
		/// When overridden in a derived class, this validates both the request and response.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public virtual void Validate(Object sender, ValidationEventArgs e)
		{
			_context.Outcome = WebTestOutcome.NotExecuted;

			HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
			document.LoadHtml(((WebTestRequest)e.WebTestItem).HttpResponseBody);
			document.OptionOutputAsXml = true;

			using (StringWriter writer = new StringWriter())
			{
				document.Save(writer);

				_document = XDocument.Parse(writer.GetStringBuilder().ToString());
			}
		}

		#endregion
	}
}
