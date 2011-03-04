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
using HtmlAgilityPack;
using System.Collections;
using System.Xml.Linq;

namespace Nathandelane.TestingTools.WebTesting.Rules
{
	public class ExtractHiddenFields : ExtractionRule
	{
		#region Methods

		public override void Extract(object sender, Events.ExtractionEventArgs e)
		{
			base.Extract(sender, e);

			IEnumerable<XElement> elements = base.Document.Document.Descendants();
			IEnumerable<XElement> hiddenElements = from el in elements where el.Name.LocalName.Equals("input") && el.Attribute(XName.Get("type")).Value.Equals("hidden") select el;

			foreach (XElement nextElement in hiddenElements)
			{
				string name = String.Empty;
				string value = String.Empty;

				if (nextElement.HasAttributes && nextElement.Attribute(XName.Get("name")) != null)
				{
					name = nextElement.Attribute(XName.Get("name")).Value;

					if (nextElement.Attribute(XName.Get("value")) != null)
					{
						value = nextElement.Attribute(XName.Get("value")).Value;
					}

					base._context[name] = value;
				}
			}
		}

		#endregion
	}
}
