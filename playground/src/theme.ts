type Theme = "light" | "dark";

const storageKey = "wireloom-theme";
const systemTheme = matchMedia("(prefers-color-scheme: dark)");
const listeners = new Set<(theme: Theme) => void>();
const controls = document.querySelectorAll<HTMLInputElement>('input[name="theme"]');

function readPreference(): Theme {
  try {
    const stored = localStorage.getItem(storageKey);
    if (stored === "light" || stored === "dark") return stored;
  } catch {
    /* Use the system's current appearance when preferences cannot be stored. */
  }

  return systemTheme.matches ? "dark" : "light";
}

let preference = readPreference();

function applyTheme() {
  const theme = preference;
  document.documentElement.dataset.theme = theme;
  document.documentElement.style.colorScheme = theme;
  for (const control of controls) control.checked = control.value === preference;
  for (const listener of listeners) listener(theme);
}

for (const control of controls) {
  control.addEventListener("change", () => {
    if (!control.checked) return;

    preference = control.value as Theme;

    try {
      localStorage.setItem(storageKey, preference);
    } catch {
      /* Keep the session choice usable. */
    }

    applyTheme();
  });
}

window.addEventListener("storage", (event) => {
  if (event.key === storageKey || event.key === null) {
    preference = readPreference();

    applyTheme();
  }
});

export function onThemeChange(listener: (theme: Theme) => void) {
  listeners.add(listener);
  listener(preference);

  return () => listeners.delete(listener);
}

// Apply the stored choice before the editor's larger application chunk loads.
// Resolve a first visit (or a legacy System preference) to a concrete choice.
try {
  localStorage.setItem(storageKey, preference);
} catch {
  /* Keep the session usable. */
}

applyTheme();
