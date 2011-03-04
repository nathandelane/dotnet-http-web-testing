using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nathandelane.TestingTools.WebTesting;
using Nathandelane.TestingTools.WebTesting.Rules;
using System.Xml.Linq;

namespace ExampleTestProject
{
	public class OreillyHomepage : WebTest
	{
		public override IEnumerator<WebTestRequest> GetRequestEnumerator()
		{
			WebTestRequest request1 = new WebTestRequest("http://oreilly.com/");
			ValidateFormField rule1 = new ValidateFormField();
			rule1.Name = "q";
			rule1.ExpectedValue = String.Empty;
			request1.ValidateResponse += new WebTestRequest.ValidationEventHandler(rule1.Validate);
			yield return request1;
		}
	}
}
