# Adjusted RTI C# wire oracle

Raw capture: `../features/07-union-char-numeric-255/`
The checked-in files apply this adjustment: Cast label 255 to char, correct its signed TypeCode
label, and preserve the incoming fallback discriminator. The raw RTI-generated files remain
unchanged. File-level RTI notices are preserved. See `manifest.json` for per-file source and
adjusted SHA-256 values.
