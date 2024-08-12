using System.Text.RegularExpressions;

namespace Shintio.CodeProcessor.Models
{
	/// <summary>
	/// C# Source file
	/// </summary>
	public class SharpFile
	{
		public string Head { get; }
		public string Body { get; }
		public string Path { get; }
		public string LineEnding { get; }

		public SharpFile(string path, string content)
		{
			Path = path;
			content = content.Replace("#nullable enable", "");

			var blockComments = @"/\*(.*?)\*/";
			// var lineComments = @"//(.*?)\r?\n";
			var strings = @"""((\\[^\n]|[^""\n])*)""";
			var verbatimStrings = @"@(""[^""]*"")+";

			var noComments = Regex.Replace(content,
				blockComments + "|" + strings + "|" + verbatimStrings,
				m =>
				{
					if (m.Value.StartsWith("/*"))
						return Environment.NewLine;
					// Keep the literal strings
					return m.Value;
				},
				RegexOptions.Singleline);

			var match = Regex.Match(noComments.Trim(), @"^(.*using [^(]*?;)*(.*)$", RegexOptions.Singleline);
			Head = match.Groups[1].Value;
			Body = match.Groups[2].Value;
			LineEnding = GetLineEnding(content);
		}

		public SharpFile(string path) : this(path, File.ReadAllText(path))
		{
		}

		public override string ToString()
		{
			return $"{Head}{Body}";
		}

		private string GetLineEnding(string contents)
		{
			if (contents.Contains("\r\n"))
			{
				return "\r\n";
			}

			if (contents.Contains('\n'))
			{
				return "\n";
			}

			if (contents.Contains('\r'))
			{
				return "\r";
			}

			return Environment.NewLine;
		}
	}
}