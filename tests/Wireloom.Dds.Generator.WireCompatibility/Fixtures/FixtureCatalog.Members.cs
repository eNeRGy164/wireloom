using Rti.Types.Dynamic;
using System.Text.RegularExpressions;

namespace Wireloom.Dds.Generator.WireCompatibility;

// DynamicData uses loans to access nested aggregate and collection members.
internal static partial class FixtureCatalog
{
    private static void SetFixtureValue(DynamicData sample, string memberName, object value)
    {
        try
        {
            sample.SetAnyValue(memberName, value);
        }
        catch (Exception exception)
        {
            if (TrySetUsingLoans(sample, memberName, value))
            {
                return;
            }

            throw new FixtureException($"Could not set fixture member '{memberName}' ({exception.GetType().Name}).");
        }
    }

    private static bool TrySetUsingLoans(DynamicData sample, string memberPath, object value)
    {
        var tokens = Regex.Matches(memberPath, @"[^.\[\],]+|\d+")
            .Select(match => match.Value)
            .ToArray();
        if (tokens.Length == 0)
        {
            return false;
        }

        var loans = new List<LoanedDynamicData>();
        try
        {
            if (tokens.Length == 1 && value is System.Collections.IEnumerable items)
            {
                var collectionLoan = sample.LoanValue(tokens[0]);
                loans.Add(collectionLoan);
                var collection = collectionLoan.Data;
                var elementIndex = 0;
                foreach (var item in items)
                {
                    collection.SetAnyValue(elementIndex++, item!);
                }

                return true;
            }

            var current = sample;
            for (var index = 0; index < tokens.Length - 1; index++)
            {
                var token = tokens[index];
                var loan = int.TryParse(token, out var elementIndex)
                    ? current.LoanValue(elementIndex)
                    : current.LoanValue(token);
                loans.Add(loan);
                current = loan.Data;
            }

            var finalToken = tokens[^1];
            if (int.TryParse(finalToken, out var finalIndex))
            {
                current.SetAnyValue(finalIndex, value);
            }
            else
            {
                current.SetAnyValue(finalToken, value);
            }

            return true;
        }
        catch
        {
            return false;
        }
        finally
        {
            foreach (var loan in loans.AsEnumerable().Reverse())
            {
                loan.Dispose();
            }
        }
    }
}
