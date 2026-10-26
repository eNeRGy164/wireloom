import { dotnet } from "./_framework/dotnet.js";
import { computeMd5 } from "./md5.mjs";

try {
  const runtime = await dotnet.create();
  runtime.setModuleImports("md5-provider", { computeMd5 });
  const api = await runtime.getAssemblyExports(runtime.getConfig().mainAssemblyName);
  postMessage({ type: "ready" });
  onmessage = ({ data }) => {
    try {
      const json = api.Wireloom.PlaygroundCompiler.Compile(JSON.stringify(data));
      postMessage({ type: "result", response: JSON.parse(json) });
    } catch (error) {
      postMessage({ type: "error", requestId: data.requestId, message: String(error) });
    }
  };
} catch (error) {
  postMessage({ type: "startup-error", message: String(error) });
}
