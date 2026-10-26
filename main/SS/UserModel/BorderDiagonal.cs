
namespace NPOI.SS.UserModel;

public enum BorderDiagonal
{
    /// <summary>
    /// No diagonal border
    /// </summary>
    None = 0,
    /// <summary>
    /// Backward diagonal border, from left-top to right-bottom
    /// </summary>
    Backward = 1,
    /// <summary>
    /// Forward diagonal border, from right-top to left-bottom
    /// </summary>
    Forward = 2,
    /// <summary>
    /// Both forward and backward diagonal border
    /// </summary>
    Both = 3
}
