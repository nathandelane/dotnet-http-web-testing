using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nathandelane.TestingTools.WebTesting;
using Nathandelane.TestingTools.WebTesting.Rules;
using System.Xml.Linq;

namespace ExampleTestProject
{
	public class PhyleBoxLogin : WebTest
	{
		public PhyleBoxLogin()
		{
			base.PreAtuhenticate = true;
		}

		public override IEnumerator<WebTestRequest> GetRequestEnumerator()
		{
			ValidateResponseUrl rule1 = new ValidateResponseUrl();
			rule1.ExpectedResponseUrl = "http://phyer.net/phyle-box/index.php";

			WebTestRequest request1 = new WebTestRequest("http://phyer.net/phyle-box/index.php");
			request1.ValidateResponse += new WebTestRequest.ValidationEventHandler(rule1.Validate);
			yield return request1;
		}
	}
}
