namespace NPOI.SS.UserModel;

public enum ReadingOrder : short
{
    /// <summary>
    /// The reading order is Context(Default).
    /// </summary>
    CONTEXT = 0,
    /// <summary>
    /// The reading order is Left To Right.
    /// </summary>
    LEFT_TO_RIGHT = 1,
    /// <summary>
    /// The reading order is Right To Left.
    /// </summary>
    RIGHT_TO_LEFT = 2,
}
