public static class Dbg {
    public static readonly bool IsDebug = true;
    public static void Assert(bool condition, string msg) {
        if (!condition) {
            throw new System.Exception(msg);
        }
    }
}
