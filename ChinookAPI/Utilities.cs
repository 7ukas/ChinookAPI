namespace ChinookAPI;

public static class Utilities {
    public static string ConvertMillisecondsToTime(int milliseconds) {
        if (milliseconds < 0) return "-";

        int minutes = milliseconds / 60000;
        int seconds = (int)Math.Ceiling((milliseconds % 60000) / 1000.0);

        string time = $"{minutes}:{seconds}";
        if (minutes < 10) time = "0" + time;
        if (seconds < 10) time = time.Substring(0, time.IndexOf(':')+1) + "0" + time.Substring(time.IndexOf(':')+1);

        return time;
    }

    public static double ConvertBytesToMegabytes(long bytes) {
        if (bytes < 0) return 0;

        return Math.Round((bytes / 1024f) / 1024f, 2);
    }
}

