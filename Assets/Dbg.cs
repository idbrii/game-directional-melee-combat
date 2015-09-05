public static class Dbg {
    public static void Assert(bool condition, string msg) {
        if (!condition) {
            throw new System.Exception(msg);
        }
    }
}
