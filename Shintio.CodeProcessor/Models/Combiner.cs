using System.Text;
using System.Text.RegularExpressions;

namespace Shintio.CodeProcessor.Models
{
	public class Combiner : ICombiner
	{
		public string Combine(CombineOptions options, params SharpFile[] files)
		{
			var result = new StringBuilder();

			foreach (var file in files)
			{
				var lines = file.Body.Split(file.LineEnding);

				var insideNamespace = false;
				foreach (var line in lines)
				{
					var trimmedLine = line.Trim();

					if (trimmedLine.StartsWith("namespace"))
					{
						insideNamespace = true;
					}

					if (insideNamespace && trimmedLine.Contains("{"))
					{
						result.AppendLine(line);
						result.AppendLine(file.Head);
						insideNamespace = false;
						continue;
					}

					result.AppendLine(line);
				}
			}

			return result.ToString();
		}

		private void Refactor(ref string head, ref string body)
		{
			// Remove head comments
			head = Regex.Replace(head, @"\/\*(.|\n)*\*\/", string.Empty);
			head = string.Join("", head.Split(Environment.NewLine).Where(x => !x.StartsWith(@"//")));

			// Remove duplicates and format usings
			var matches = Regex.Matches(head, @"using[ |\n\r]+(\w*)([ |\n\r]*\.[ |\n\r]*(\w*))*[ |\n\r]*;");
			var usings = matches.Select(g => $"using {string.Join(".", g.Groups[3].Captures.Prepend(g.Groups[1]))};");
			head = string.Join(Environment.NewLine, usings.Distinct());
		}
	}
}