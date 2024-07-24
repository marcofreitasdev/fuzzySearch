var choices = new List<string> { "apple", "banana", "grape", "orange", "pineapple" };

// String de busca
var searchTerm = "appl";

// Limite de similaridade
var threshold = 0.85;

// Realiza a busca fuzzy
foreach (var choice in choices)
{
    var similarity = JaroWinklerDistance(searchTerm, choice);
    if (similarity >= threshold)
    {
        Console.WriteLine($"Termo: {choice}, Similaridade: {similarity}");
    }
}

static double JaroWinklerDistance(string s1, string s2)
{
    double jaroDistance = JaroDistance(s1, s2);
    int prefixLength = CommonPrefixLength(s1, s2);

    return jaroDistance + 0.1 * prefixLength * (1 - jaroDistance);
}

static double JaroDistance(string s1, string s2)
{
    int s1Len = s1.Length;
    int s2Len = s2.Length;

    if (s1Len == 0)
        return s2Len == 0 ? 1.0 : 0.0;

    int matchDistance = Math.Max(s1Len, s2Len) / 2 - 1;
    bool[] s1Matches = new bool[s1Len];
    bool[] s2Matches = new bool[s2Len];

    int matches = 0;
    for (int i = 0; i < s1Len; i++)
    {
        int start = Math.Max(0, i - matchDistance);
        int end = Math.Min(i + matchDistance + 1, s2Len);

        for (int j = start; j < end; j++)
        {
            if (s2Matches[j]) continue;
            if (s1[i] != s2[j]) continue;
            s1Matches[i] = true;
            s2Matches[j] = true;
            matches++;
            break;
        }
    }

    if (matches == 0)
        return 0.0;

    double t = 0;
    int k = 0;

    for (int i = 0; i < s1Len; i++)
    {
        if (!s1Matches[i]) continue;

        while (!s2Matches[k]) k++;

        if (s1[i] != s2[k])
            t++;

        k++;
    }

    t /= 2.0;

    return ((double)matches / s1Len + (double)matches / s2Len + (matches - t) / matches) / 3.0;
}

static int CommonPrefixLength(string s1, string s2, int maxPrefixLength = 4)
{
    int n = Math.Min(Math.Min(s1.Length, s2.Length), maxPrefixLength);

    for (int i = 0; i < n; i++)
    {
        if (s1[i] != s2[i])
        {
            return i;
        }
    }

    return n;
}