using System.Collections.Generic;

public static class Arrays
{
    /// <summary>
    /// This function will produce an array of size 'length' starting with 'number' followed by multiples of 'number'.  For
    /// example, MultiplesOf(7, 5) will result in: {7, 14, 21, 28, 35}.  Assume that length is a positive
    /// integer greater than 0.
    /// </summary>
    /// <returns>array of doubles that are the multiples of the supplied number</returns>
    public static double[] MultiplesOf(double number, int length)
    {
        // TODO Problem 1 Start
        // Step 1: Create an array to hold exactly 'length' results.
        // Step 2: Loop through each index from 0 to length - 1.
        // Step 3: For each index i, compute the multiple as number * (i + 1).
        // Step 4: Store that value in the array at index i.
        // Step 5: Return the completed array.
        double[] result = new double[length];
        for (int i = 0; i < length; i++)
        {
            result[i] = number * (i + 1);
        }

        return result;
    }

    /// <summary>
    /// Rotate the 'data' to the right by the 'amount'.  For example, if the data is
    /// List<int>{1, 2, 3, 4, 5, 6, 7, 8, 9} and an amount is 3 then the list after the function runs should be
    /// List<int>{7, 8, 9, 1, 2, 3, 4, 5, 6}.  The value of amount will be in the range of 1 to data.Count, inclusive.
    ///
    /// Because a list is dynamic, this function will modify the existing data list rather than returning a new list.
    /// </summary>
    public static void RotateListRight(List<int> data, int amount)
    {   // TODO Problem 2 Start
        // Step 1: Identify the last 'amount' elements.
        // Step 2: Move them to the front.
        // Step 3: Keep original order of moved elements.
        // Step 4: Update the same list (no new return value)
        int n = data.Count;
        List<int> rotated = new List<int>(data);

        for (int i = 0; i < n; i++)
        {
            rotated[(i + amount) % n] = data[i];
        }

        for (int i = 0; i < n; i++)
        {
            data[i] = rotated[i];
        }
    }
}
