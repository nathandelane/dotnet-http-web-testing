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
using Nathandelane.TestingTools.WebTesting.Rules;
using System.Diagnostics;

namespace Nathandelane.TestingTools.WebTesting.ConsoleTests
{
	class Program
	{
		static void Main(string[] args)
		{
			WebTestRequest request = new WebTestRequest("http://www.vehix.com/");
			ExtractHiddenFields extractionRule1 = new ExtractHiddenFields();

			request.ExtractValues += new WebTestRequest.ExtractionEventHandler(extractionRule1.Extract);

			request.Execute();

			Debug.Assert(request.Context.Count > 0);
		}
	}
}
