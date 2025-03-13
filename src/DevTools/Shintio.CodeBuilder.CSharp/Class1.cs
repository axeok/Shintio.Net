using System;
using System.Collections.Generic;
using System.IO;
using Shintio.CodeBuilder.CSharp.CodeBlocks;
using Shintio.CodeBuilder.CSharp.Components;
using Shintio.CodeBuilder.CSharp.Extensions;
using Shintio.CodeBuilder.CSharp.Utils;

namespace Shintio.CodeBuilder.CSharp;

public class Class1
{
	public string? test;

	public static void Test()
	{
		var constructorParameters = new List<ParameterCodeBlock>() { new ParameterCodeBlock("int", "x") };

		var names = new string[] { "TestScene1", "TestScene2" };

		// public static readonly SceneInfo {name} = new SceneInfo(1, \"{name}\", true)"

		var file = CodeFactory.File.Create(
			[
				"System",
				"System.Collections.Generic",
			],
			"Shintio.TestNamespace.Scenes",
			builder => builder
				.AddFields(names, name =>
					new FieldCodeBlock("public", "SceneInfo", name)
						.Static()
						.Readonly())
				.AddAdditionalBlock(
					new ClassCodeBlock("SceneInfo")
						.AddField("public", "string", "Id")
						.AddField(CodeFactory.Field.Create(AccessModifier.Public, TypeInfo.String, "Id").Readonly())
						.AddProperty(CodeFactory.Property.CreateAuto(AccessModifier.Public, TypeInfo.String, "Id", true,
							defaultValue: "qwe"))
						.AddRaw("public readonly bool Enabled;")
						.AddConstructor(
							"public",
							new ParameterCodeBlock(TypeInfo.Int, "id"),
							new ParameterCodeBlock("string", "name"),
							new ParameterCodeBlock("bool", "enabled")
						)
						.WithBody("""
							Id = id;
							Name = name;
							Enabled = enabled;
							""")
				)
		);


		var result = file.GetCode(0);

		Console.WriteLine(result);

		File.WriteAllText("result.cs", result);
	}
}