import "./theme";

// Keep a small entry point so a missing editor chunk has a visible recovery action.
void import("./main").catch(() => {
  const status = document.getElementById("status")!;
  status.textContent = "The editor could not load. Reload to try again; saved IDL stays in this browser.";

  const retry = document.getElementById("retry") as HTMLButtonElement;
  retry.hidden = false;
  retry.textContent = "Reload playground";
  retry.onclick = () => location.reload();
});
