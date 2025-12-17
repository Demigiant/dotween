public struct CustomRange
{
    public float min, max;

    public CustomRange(float min, float max)
    {
        this.min = min;
        this.max = max;
    }

    public static CustomRange operator +(CustomRange c1, CustomRange c2)
    {
        return new CustomRange(c1.min + c2.min, c1.max + c2.max);
    }

    public static CustomRange operator -(CustomRange c1, CustomRange c2)
    {
        return new CustomRange(c1.min - c2.min, c1.max - c2.max);
    }
}