using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Xml.Linq;
using System.IO;

namespace Nathandelane.TestingTools.WebTesting.Driver
{
	class Program
	{
		#region Fields

		private static string __testListFileName = "TestList.xml";

		private XDocument _testList;

		#endregion

		#region Constructors

		private Program(FileInfo testListFile)
		{
			_testList = XDocument.Load(testListFile.OpenRead());
		}

		/// <summary>
		/// Runs the tests found in the TestList.xml.
		/// </summary>
		private void Run()
		{
			IEnumerable<XElement> assemblies = _testList.Document.Descendants(XName.Get("assembly"));

			if (assemblies.Count<XElement>() > 0)
			{
				foreach (XElement nextAssembly in assemblies)
				{
					string assemblyName = nextAssembly.Attribute(XName.Get("name")).Value;

					if (!assemblyName.EndsWith(".dll", StringComparison.InvariantCultureIgnoreCase))
					{
						assemblyName = String.Concat(assemblyName, ".dll");
					}

					Assembly assembly = Assembly.LoadFrom(assemblyName);
					Type[] types = assembly.GetTypes();

					foreach (Type nextType in types)
					{
						if (nextType.BaseType == typeof(WebTest))
						{
							WebTest nextWebTest = Activator.CreateInstance(nextType) as WebTest;
							IDictionary<string, WebTestOutcome> dependentRequestOutcomes = new Dictionary<string, WebTestOutcome>();

							if (String.IsNullOrEmpty(nextWebTest.Name))
							{
								Console.Write("{0}: ", nextType);
							}
							else
							{
								Console.Write("{0}: ", nextWebTest.Name);
							}

							IEnumerator<WebTestRequest> webTestRequests = nextWebTest.GetRequestEnumerator();
							
							while (webTestRequests.MoveNext())
							{
								WebTestRequest nextRequest = webTestRequests.Current;

								if (nextWebTest.PreAtuhenticate)
								{
									nextRequest.PreAuthenticate = true;
								}

								nextRequest.Execute();

								dependentRequestOutcomes.Add(nextRequest.Uri.ToString(), nextRequest.Outcome);
							}

							if (dependentRequestOutcomes.Values.Contains(WebTestOutcome.Failed))
							{
								Console.WriteLine("{0}", WebTestOutcome.Failed);
							}
							else if (dependentRequestOutcomes.Values.Contains(WebTestOutcome.Error))
							{
								Console.WriteLine("{0}", WebTestOutcome.Error);
							}
							else if (dependentRequestOutcomes.Values.Contains(WebTestOutcome.NotExecuted))
							{
								Console.WriteLine("{0}", WebTestOutcome.NotExecuted);
							}
							else
							{
								Console.WriteLine("{0}", WebTestOutcome.Passed);
							}

							foreach (string url in dependentRequestOutcomes.Keys)
							{
								Console.WriteLine("+ {0}: {1}", url, dependentRequestOutcomes[url]);
							}
						}
					}
				}
			}
		}

		#endregion

		public static void Main(string[] args)
		{
			FileInfo testListFileInfo;

			if (args.Length == 0)
			{
				string assumedPathOfTestList = Path.Combine(Environment.CurrentDirectory, "TestList.xml");

				if (!File.Exists(assumedPathOfTestList))
				{
					Console.WriteLine("Usage: WebTestDriver <TestListFileName>");

					return;
				}
				else
				{
					testListFileInfo = new FileInfo(assumedPathOfTestList);
				}
			}
			else
			{
				testListFileInfo = new FileInfo(args[0]);
			}

			if (testListFileInfo.Exists)
			{
				Program program = new Program(testListFileInfo);
				program.Run();
			}
			else
			{
				Console.WriteLine("Could not load test list file {0}.", testListFileInfo.Name);
			}
		}
	}
}
