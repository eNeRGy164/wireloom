import assert from "node:assert/strict";
import test from "node:test";
import { generatedFileIconKind } from "./file-icon-kind.ts";

test("C++ source and header extensions use their distinct icon kinds", () => {
  // Arrange
  const projectName = "CMakeLists.txt";

  // Act
  const icons = ["main.cxx", "main.hpp", "mainPlugin.cxx", "mainPlugin.hpp"].map((name) =>
    generatedFileIconKind(name, "cpp", projectName),
  );

  // Assert
  assert.deepEqual(icons, ["cpp", "cpp-header", "cpp", "cpp-header"]);
});

test("CMake project and script files use distinct icon kinds", () => {
  // Arrange
  const projectName = "CMakeLists.txt";

  // Act
  const projectIcon = generatedFileIconKind(projectName, "cpp", projectName);
  const scriptIcon = generatedFileIconKind("cmake/Compare.cmake", "cpp", projectName);

  // Assert
  assert.equal(projectIcon, "cmake");
  assert.equal(scriptIcon, "cmake-script");
});

test("C# project and generated source use their own icon kinds", () => {
  // Arrange
  const projectName = "Wireloom.csproj";

  // Act
  const icons = [projectName, "Reading.cs"].map((name) => generatedFileIconKind(name, "csharp", projectName));

  // Assert
  assert.deepEqual(icons, ["csproj", "csharp"]);
});
